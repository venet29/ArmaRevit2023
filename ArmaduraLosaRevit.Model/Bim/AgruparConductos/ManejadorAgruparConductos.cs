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

namespace ArmaduraLosaRevit.Model.Bim
{
    public class ManejadorAgruparConductos
    {
        private UIApplication _uiapp;

        private Document _doc;
        private View _view;


        public SeleccionarConduct SeleccionarConduct_ { get; private set; }

        public ManejadorAgruparConductos(UIApplication uiapp)//para atuomatico
        {
            this._uiapp = uiapp;

            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            //this._escale = _view.Scale;
        }


        public Result Ejecutar()

        {
            try
            {

                ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                _ManejadorCargarFAmilias.cargarFamiliasBim_run();

                ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                configuracionInicial.AgregarParametrosShareBIM();

                SeleccionarConduct_ = new SeleccionarConduct(_uiapp);
                if (!SeleccionarConduct_.Ejecutar()) return Result.Failed;


                AgrupadorConduct AgrupadorBarrasManual = new AgrupadorConduct(_uiapp, SeleccionarConduct_.ListConduct);
                if (!AgrupadorBarrasManual.GEnerarGrupos()) return Result.Failed;
                if (!AgrupadorBarrasManual.GenerarTExto()) return Result.Failed;

                string nombreFamilia = ConstBim.NombreFamiliaTAg;
                Element IndependentTagPath = TiposTagPorBuilInCategory.M1_GetTag(_doc, BuiltInCategory.OST_ConduitTags, nombreFamilia);

                if (IndependentTagPath == null)
                {
                    Util.ErrorMsg($"NO se puedo encontrar  familia de letra del tag de barra :{nombreFamilia}");
                    return Result.Cancelled;
                }


                ConfiguracionTagDTO _ConfiguracionTAgBarraDTo = new ConfiguracionTagDTO()
                {
                    IsDIrectriz = true,
                    tagOrientation=TagOrientation.Horizontal,
                    LeaderElbow= SeleccionarConduct_.ptoMouseCodo,
                    TagHeadPosition = SeleccionarConduct_.ptoMouseFinal,
                    desplazamientoPathReinSpanSymbol = XYZ.Zero
                };


                using (Transaction t = new Transaction(_view.Document))
                {

                    t.Start($"CrearTAgDuctos-NH");
                    TagBAseBim _TagBAseBim = new TagBAseBim(_uiapp, IndependentTagPath);
                    _TagBAseBim.DibujarTag(SeleccionarConduct_.ElementoSeleccionado, _ConfiguracionTAgBarraDTo);

                    bool resultBarraTipo = ParameterUtil.SetParaStringNH(SeleccionarConduct_.ElementoSeleccionado, "GrupoConduit", AgrupadorBarrasManual.textoGrupo);

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




    }
}
