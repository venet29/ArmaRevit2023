using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Mirror.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Mirror.DireccionMirror;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ocultar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar.Model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Mirror
{
    public class ManejadorMirrorPathReinformen
    {
        private UIApplication uiapp;

        private Document _doc;
        private SeleccionarPathReinfomentREsaltar _SeleccionarPathReinfomentREsaltar;
        private OcultarBarras _OcultarBarras;
        private Line _EjeReferencia;
#pragma warning disable CS0169 // The field 'ManejadorMirrorPathReinformen._CoordenadaPath' is never used
        private CoordenadaPath _CoordenadaPath;
#pragma warning restore CS0169 // The field 'ManejadorMirrorPathReinformen._CoordenadaPath' is never used
#pragma warning disable CS0169 // The field 'ManejadorMirrorPathReinformen.VectorDesplazamiento1PathSymb' is never used
        private XYZ VectorDesplazamiento1PathSymb;
#pragma warning restore CS0169 // The field 'ManejadorMirrorPathReinformen.VectorDesplazamiento1PathSymb' is never used

        public List<PathReinforcement> ListaFinalCopiados { get; set; }
        public List<WrapperBarrasLosa> ListaWrapperBarrasLosa { get; set; }
        public XYZ VectorDesplazamientoPAth { get; set; }

        public ManejadorMirrorPathReinformen()
        {

        }

        public ManejadorMirrorPathReinformen(UIApplication uiapp)
        {
            this.uiapp = uiapp;

            this._doc = uiapp.ActiveUIDocument.Document;
            _SeleccionarPathReinfomentREsaltar = new SeleccionarPathReinfomentREsaltar(uiapp);

            _OcultarBarras = new OcultarBarras(_doc);
            ListaFinalCopiados = new List<PathReinforcement>();
        }



        public Result Ejecutar1()
        {
            try
            {
                _SeleccionarPathReinfomentREsaltar.Ejecutar_seleccion1();
                Ejecutar_procediminto();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Ejecutar1  ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }
        public Result EjecutarMultiples()
        {
            try
            {
                _SeleccionarPathReinfomentREsaltar.Ejecutar_seleccionMultiples();
                Ejecutar_procediminto();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"EjecutarMultiples  ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        private void Ejecutar_procediminto()
        {
            if (_SeleccionarPathReinfomentREsaltar == null) return;
            ListaWrapperBarrasLosa = _SeleccionarPathReinfomentREsaltar.ListaWrapperBarrasLosa;
            if (ListaWrapperBarrasLosa.Count == 0) return;

            M1_SeleccionarEjes();

            CalculosPtos _CalculosPtos = new CalculosPtos(_doc, ListaWrapperBarrasLosa, _EjeReferencia);
            _CalculosPtos.M2_CalcularPtos();
            // M2_CalcularPtos();


            AnalisasSentido _AnalisasSentido = new AnalisasSentido(_CalculosPtos._CoordenadaPath, _EjeReferencia);
            _AnalisasSentido.M1_ejecutar();

            if (_AnalisasSentido.SentidoEje == SentidoMirror.sentidoDiagonalBarra)
            {
                Util.ErrorMsg(" Error, eje de referencia no es ortogonal a barras");
                return;
            }

            try
            {
                using (TransactionGroup trans2 = new TransactionGroup(_doc))
                {
                    trans2.Start("MirrorPath-NH");

                    if (_AnalisasSentido.SentidoEje == SentidoMirror.sentidoParaleloBarra)
                        M3_EjecutarMirror();
                    else
                        M4_EjePerpendicular();
                    _OcultarBarras.OcultarListaBarraCreada_AgregarParametroARebarSystemConTrasn(ListaFinalCopiados);
                    trans2.Assimilate();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private bool M1_SeleccionarEjes()
        {
            try
            {
                SeleccionarEjeMirror _SeleccionarEjeMirror = new SeleccionarEjeMirror(uiapp);
                if (!_SeleccionarEjeMirror.Ejecutar_SeleccionarEjeMirror()) return false;

                _EjeReferencia = _SeleccionarEjeMirror.lineReferncia;
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        private bool M3_EjecutarMirror()
        {

            MirrorParaleloBarras _MirrorParaleloBarras = new MirrorParaleloBarras(_doc, ListaWrapperBarrasLosa, _EjeReferencia);
            _MirrorParaleloBarras.Mirror();

            if (_MirrorParaleloBarras.ListaFinalCopiados.Count > 0)
                ListaFinalCopiados.AddRange(_MirrorParaleloBarras.ListaFinalCopiados);

            return _MirrorParaleloBarras.Isok;
        }
        private bool M4_EjePerpendicular()
        {
            MirrorPerpendicularBarras _MirrorPerpendicularBarras = new MirrorPerpendicularBarras(_doc, ListaWrapperBarrasLosa, _EjeReferencia);
            _MirrorPerpendicularBarras.Mirror();

            if (_MirrorPerpendicularBarras.ListaFinalCopiados.Count > 0)
                ListaFinalCopiados.AddRange(_MirrorPerpendicularBarras.ListaFinalCopiados);

            return _MirrorPerpendicularBarras.Isok;
        }








    }
}
