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
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
#endregion // Namespaces



namespace ArmaduraLosaRevit.Model.BarraAreaPath
{
    public class ManejadorMallaMuro
    {
        private UIApplication _uiapp;
        private  DatosMallasAutoDTO _datosMallasDTO;
        private UIDocument _uidoc;
        private Document _doc;
        private View3D _view3D_buscar;
        private View3D _view3D_paraVisualizar;

        public ManejadorMallaMuro(UIApplication uiapp, DatosMallasAutoDTO datosMallasDTO)
        {
            this._uiapp = uiapp;
            this._datosMallasDTO = datosMallasDTO;
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = uiapp.ActiveUIDocument.Document;
        }


        public void CrearMallaMuro()
        {
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();

            try
            {
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);
              //  UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                if (!CalculosIniciales()) return;


                //1
                SeleccionMuroAreaPath SeleccionMuroAreaPath = new SeleccionMuroAreaPath(_uiapp);
                SeleccionMuroAreaPath.Ejecutar();

                if (SeleccionMuroAreaPath.IsOk)
                {
                    //2
                    ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                    List<Level> listaLevel = _seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_doc.ActiveView);

                    //3obtener resultados de seleccion
                    SeleccionMuroAreaPathDTO _seleccionMuroAreaPathDTO = SeleccionMuroAreaPath.Resultado();
                    _seleccionMuroAreaPathDTO.diamtromallaVertical = _datosMallasDTO.diametroV;

                    _datosMallasDTO.espesorFoot = _seleccionMuroAreaPathDTO._espesorMuroFoot;
                    
                    //4
                    InterrvalosPtosPathArea interrvalosPtosPathArea = new InterrvalosPtosPathArea(_uiapp, _seleccionMuroAreaPathDTO, _datosMallasDTO, listaLevel, _view3D_buscar);
                    interrvalosPtosPathArea.Ejecutar();

                    //5
                    BarraAreasPath BarraAreasPath = new BarraAreasPath(_uiapp, _view3D_paraVisualizar, interrvalosPtosPathArea);
                    BarraAreasPath.Ejecutar_conIntervalos2();
                    //BarraAreasRebar BarraAreasPath = new BarraAreasRebar(_uiapp, _view3D_paraVisualizar, interrvalosPtosPathArea);
                    //BarraAreasPath.Ejecutar_conIntervalos2();
                }
              //  UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            }
            catch (Exception ex)
            {
                //UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
#if DEBUG
            LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);
#endif
            return;
        }

        private bool CalculosIniciales()
        {
            _view3D_buscar = TiposFamilia3D.Get3DBuscar(_doc);
            _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            if (_view3D_buscar==null) return true;
            if (_view3D_paraVisualizar == null) return true;

            return true;

        }
    }
}
