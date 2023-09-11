using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Bim.Model;
using ArmaduraLosaRevit.Model.Bim.DTO;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.Bim.SumarLargo.Ayuda;

namespace ArmaduraLosaRevit.Model.Bim.SumarLargo
{
    public class ManejadorSumarLArgo
    {
        private UIApplication _uiapp;

        private Document _doc;
        private View _view;


        public SeleccionarPipes Seleccionarpipes_ { get; private set; }

        public ManejadorSumarLArgo(UIApplication uiapp)//para atuomatico
        {
            this._uiapp = uiapp;

            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            //this._escale = _view.Scale;
        }

        public Result EjecutarCOnTag()

        {
            try
            {

                ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                _ManejadorCargarFAmilias.cargarFamiliasBim_run();

                ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                configuracionInicial.AgregarParametrosShareBIM();

                Seleccionarpipes_ = new SeleccionarPipes(_uiapp);
                if (!Seleccionarpipes_.Ejecutar_codo_punto()) return Result.Failed;


                AgrupadorPipes AgrupadorBarrasManual = new AgrupadorPipes(_uiapp, Seleccionarpipes_.ListPipes);
                if (!AgrupadorBarrasManual.GEnerarGrupos()) return Result.Failed;
                if (!AgrupadorBarrasManual.GenerarTExto()) return Result.Failed;

                string nombreFamilia = ConstBim.NombreFamiliaTAgPipes;
                Element IndependentTagPath = TiposTagPorBuilInCategory.M1_GetTag(_doc, BuiltInCategory.OST_PipeCurves, nombreFamilia);

                if (IndependentTagPath == null)
                {
                    Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreFamilia}");
                    return Result.Cancelled;
                }


                ConfiguracionTagDTO _ConfiguracionTAgBarraDTo = new ConfiguracionTagDTO()
                {
                    IsDIrectriz = true,
                    tagOrientation = TagOrientation.Horizontal,
                    LeaderElbow = Seleccionarpipes_.ptoMouseCodo,
                    TagHeadPosition = Seleccionarpipes_.ptoMouseFinal,
                    desplazamientoPathReinSpanSymbol = XYZ.Zero
                };


                using (Transaction t = new Transaction(_view.Document))
                {

                    t.Start($"CrearTAgPipes-NH");
                    TagBAseBim _TagBAseBim = new TagBAseBim(_uiapp, IndependentTagPath);
                    _TagBAseBim.DibujarTag(Seleccionarpipes_.ElementoSeleccionado, _ConfiguracionTAgBarraDTo);

                    bool resultBarraTipo = ParameterUtil.SetParaStringNH(Seleccionarpipes_.ElementoSeleccionado, "SumaLargoPipes", AgrupadorBarrasManual.textoSumaLArgos);

                    t.Commit();
                }



            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        public Result EjecutarTExto()
        {
            try
            {
                bool mintras = true;
                while (mintras)
                {
                    AyudaObtenerTipoLetra.M2_ObtenerTipoDeTExto(_uiapp);

                    Seleccionarpipes_ = new SeleccionarPipes(_uiapp);
                    if (!Seleccionarpipes_.Ejecutar_puntoParaTexto()) return Result.Succeeded;

                    AgrupadorPipes AgrupadorBarrasManual = new AgrupadorPipes(_uiapp, Seleccionarpipes_.ListPipes);
                    if (!AgrupadorBarrasManual.GEnerarGrupos()) return Result.Failed;
                    if (!AgrupadorBarrasManual.GenerarTExto()) return Result.Failed;

                    CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.TextoSumaLargoPipe, TipoCOloresTexto.Blanco);

                    _CrearTexNote.M1_CrearConTrans(Seleccionarpipes_.ptoMouseFinal + new XYZ(Util.CmToFoot(-10), Util.CmToFoot(+10), 0), AgrupadorBarrasManual.textoSumaLArgos, 0);
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }
}
