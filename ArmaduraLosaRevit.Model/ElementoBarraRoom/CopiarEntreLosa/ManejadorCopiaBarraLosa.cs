using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa.wpf;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa
{
    public class ManejadorCopiaBarraLosa
    {
        private UIApplication _uiapp;

        private List<string> ListaPlantasSelec;
        private Document _doc;
        private View _view_origen;
        private CopiaBarrasEntreLosa _CopiaBarrasEntreLosa;
        Stopwatch timeMeasure = new Stopwatch();
        public static int barrasCopiadas;
#pragma warning disable CS0169 // The field 'ManejadorCopiaBarraLosa.util' is never used
        private object util;
#pragma warning restore CS0169 // The field 'ManejadorCopiaBarraLosa.util' is never used
        private List<View> listaView;
        private CopiarRebarEntreLosas _CopiarPathReinfWpf;
        private SeleccionarPathReinforment_InfoCompleta _SeleccionarPathReinformentParaCopiar;
        private SeleccionarRebarVisibilidad _seleccionarRebarVisibilidad;
        private SeleccionarGroup _seleccionarGroup;
        public ManejadorCopiaBarraLosa(UIApplication uiapp)
        {
            this._uiapp = uiapp;

            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view_origen = _doc.ActiveView;
            barrasCopiadas = 0;

            _SeleccionarPathReinformentParaCopiar = new SeleccionarPathReinforment_InfoCompleta(_uiapp);
            _seleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _view_origen);
            _seleccionarGroup = new SeleccionarGroup(_uiapp);

        }

        public Result Execute()
        {
            try
            {
                listaView = SeleccionarView.ObtenerView_losa_elev_fund(_uiapp.ActiveUIDocument);
                List<string> listaViewName = listaView.Where(c => c.ViewType == ViewType.FloorPlan).Select(c => c.Name).ToList();

                _CopiarPathReinfWpf = new CopiarRebarEntreLosas(listaViewName, _view_origen.Name);
                _CopiarPathReinfWpf.ShowDialog();

                if (_CopiarPathReinfWpf.IsOk) Ejecutar();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }

        }

        public bool Ejecutar()
        {

            timeMeasure.Start();
            //1)seleccionar nivel
            if (!M1_ObtenerNivelesSeleccionadosWpf(listaView)) return false;

            if (!M0_VerificarVistasConNOmbreActualizado()) return false;

            //2) obter barras

            if (!_SeleccionarPathReinformentParaCopiar.GenerarListaCopiar_PathReinforment()
              & !_seleccionarRebarVisibilidad.M1_EjecutarPreSeleccion()
              & !_seleccionarGroup.PreseleccionarGroup()) return false;

            _CopiaBarrasEntreLosa = null;
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CopiarBarrasEntreLosas-NH");

                    for (int i = 0; i < ListaPlantasSelec.Count; i++)
                    {
                        string plantaSelec = ListaPlantasSelec[i];
                        View _viewDestino = listaView.Where(c => c.Name == plantaSelec).FirstOrDefault();
                        if (_viewDestino == null) continue;

                        if (_uiapp.ActiveUIDocument.ActiveView.Name != _viewDestino.Name)
                        {
                            _uiapp.ActiveUIDocument.ActiveView = _viewDestino;
                        }

                        //3) copiar datos
                        bool resultCopiar = M3_CopiarBarrasIdem(_viewDestino);
                        if (resultCopiar == false)
                        {
                            t.RollBack();
                            return false;
                        }

                        ManejadorCmdVisibilidadElement.CmdM3_MostrarPAthNormal(_uiapp, _viewDestino);

                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }



            TimeSpan ts = timeMeasure.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Util.InfoMsg($"Proceso terminado. Tiempio : {elapsedTime}");

            return true;
        }

        private bool M0_VerificarVistasConNOmbreActualizado()
        {
            try
            {
                for (int i = 0; i < ListaPlantasSelec.Count; i++)
                {
                    string plantaSelec = ListaPlantasSelec[i];
                    View _viewDestino = listaView.Where(c => c.Name == plantaSelec).FirstOrDefault();
                    if (BuscarIsNombreViewActualizado.IsError(_viewDestino, false))
                    {
                        Util.ErrorMsg($"Vista '{_viewDestino.Name}' no tiene el nombre de vista actualizado. Ejecutar comando 'Actualizar Nombre View' y vuelva a ejecutar copia de barra entre losas.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al comprobar´'NOmbreActualizadoVIew' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M1_ObtenerNivelesSeleccionadosWpf(List<View> listaView)
        {
            ListaPlantasSelec = _CopiarPathReinfWpf.ObtenerListaPlantasSeleccionadas();

            foreach (string item in ListaPlantasSelec)
            {
                View _viewSelec = listaView.Where(c => c.Name == item).FirstOrDefault();
                var para = _viewSelec.GetParameter2("View Template").AsElementId();

                if (para == null)
                {
                    Util.ErrorMsg($" View '{item}' tiene asignado un templete igual a NULL. Desactivar y ejecutar 'Cargar Configuracion'");
                    return false;
                }

                if (para.IntegerValue != -1)
                {
                    Util.ErrorMsg($" View '{item}' tiene asignado un templete. Desactivar y ejecutar 'Cargar Configuracion'");
                    return false;
                }
            }
            // ListaPlantasSelec.Where(c=> c.GetParameter2("View Template"))


            if (ListaPlantasSelec.Count == 0) return false;

            return true;
        }

        private bool M3_CopiarBarrasIdem(View _viewDestino)
        {
            try
            {
                if (_view_origen.ViewType != ViewType.FloorPlan)
                {
                    Util.ErrorMsg("Vista debe ser FloorPlan");
                    return false;
                }
                _CopiaBarrasEntreLosa = new CopiaBarrasEntreLosa(_uiapp, _view_origen,
                                            _SeleccionarPathReinformentParaCopiar.ListaElementoPathRein,
                                            _seleccionarRebarVisibilidad._lista_A_VisibilidadElementoREbarDTO_preseleccionado,
                                            _seleccionarGroup.ListaGroup);
                _CopiaBarrasEntreLosa.CopiarConTrasnAll(_viewDestino);
            }

            catch (Exception ex)
            {
                Util.ErrorMsg($"error al copiar barras Idem \n  ex: {ex.Message}");
                return false;
            }
            return true;
        }


    }
}
