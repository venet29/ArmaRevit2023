#region Namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;
using System;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraAreaPath.Intervalo;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
#endregion // Namespaces



namespace ArmaduraLosaRevit.Model.BarraAreaPath
{
    public class ManejadorMallaMuroAuto
    {
        private UIApplication _uiapp;
        private  List<IntervalosMallaDTOAuto> _listIntervalosMallaDTOAuto;

        private UIDocument _uidoc;
        private Document _doc;
        private View3D _view3D_buscar;
        private View3D _view3D_paraVisualizar;

        public ManejadorMallaMuroAuto(UIApplication uiapp, List<IntervalosMallaDTOAuto> ListIntervalosMallaDTOAuto)
        {
            this._uiapp = uiapp;
            _listIntervalosMallaDTOAuto = ListIntervalosMallaDTOAuto;


            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = uiapp.ActiveUIDocument.Document;
        }


        public void CrearMallaMuro()
        {

            if (_listIntervalosMallaDTOAuto.Count == 0) return;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();

            try
            {
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                if (!CalculosIniciales()) return;

                ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                List<Level> listaLevel = _seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_doc.ActiveView);

           
                for (int i = 0; i < _listIntervalosMallaDTOAuto.Count; i++)
                {
                    Debug.WriteLine($"ite:{i} dentro selec");
                    IntervalosMallaDTOAuto newIntervalosMallaDTOAuto = _listIntervalosMallaDTOAuto[i];
                    SeleccionMuroAreaPathAuto SeleccionMuroAreaPath = new SeleccionMuroAreaPathAuto(_uiapp, newIntervalosMallaDTOAuto, _view3D_buscar);
                    SeleccionMuroAreaPath.Ejecutar_SeleccionarMuroPtoAuto();
                    newIntervalosMallaDTOAuto.IsOk = SeleccionMuroAreaPath.IsOk;
                }

                List<IntervalosMallaDTOAuto> listOK = _listIntervalosMallaDTOAuto.Where(c => c.IsOk == true).ToList();
                InterrvalosPtosPathAreaAuto interrvalosPtosPathArea = new InterrvalosPtosPathAreaAuto(_uiapp, listOK, listaLevel, _view3D_buscar);
                interrvalosPtosPathArea.Ejecutar_auto();

                //5
                BarraAreasPath BarraAreasPath = new BarraAreasPath(_uiapp, _view3D_paraVisualizar, interrvalosPtosPathArea);
                BarraAreasPath.Ejecutar_conIntervalos2();
                //

                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            }
            catch (Exception ex)
            {
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }

#if DEBUG
            LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);
#endif
            return;
        }

        private bool CalculosIniciales()
        {
            _view3D_buscar = TiposFamilia3D.Get3DBuscar(_doc);
            _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            if (_view3D_buscar == null) return true;
            if (_view3D_paraVisualizar == null) return true;

            return true;

        }
    }
}
