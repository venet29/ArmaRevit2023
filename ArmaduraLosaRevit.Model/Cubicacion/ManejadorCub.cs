using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.Cubicacion.Repetir;
using ArmaduraLosaRevit.Model.Cubicacion.Seleccionar;
using ArmaduraLosaRevit.Model.DatosExcel;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.TablasSchedule;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.AyudasView;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cubicacion
{
    public class ManejadorCubDTO
    {
        public string nombreProyecto { get; set; }
        public string numeroObra { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public List<LevelDTO> lista_view { get; set; }
        public List<LosaCubDto> lista_Losa { get; internal set; }
        public List<ElevCubDto> lista_Elev { get; internal set; }
        public string Nombre3d { get; internal set; }

        public ManejadorCubDTO()
        {
            nombreProyecto = "PRoyectoX";
            numeroObra = "NUmeroXX";
            NombreArchivo = "";
        }
    }

    public class ManejadorCub
    {
        private UIApplication _uiapp;
        private  ManejadorCubDTO manejadorCubDTO;
        private  List<LevelDTO> _lista_Level;
        private string _nombreProyecto;
        private string _numeroObra;
        private Document _doc;
        private View _view;

        public List<CubBarra> ListaCubBarra { get; set; }

        private ObtenerNivelPorOrigenBarra _obtenerNivelPorOrigenBarra;
        private List<View> listaView;
        private List<CubBarra> ListaCubBarra_path;
        private int i;
        private List<CubBarra> ListaCubBarra_rebar;
        private SeleccionarRebar_PathReinforment _SeleccionarRebar_PathReinforment;
        private List<ElementoPathRebarInSystem> _lista_A_DeRebarInSystemv2;

        private List<object[]> ListaObjetos;
        private List<LevelDTO> _lista_Level_habilitados;

        private List<LosaCubDto> _lista_losa;

        private List<ElevCubDto> _lista_Elev;

        public ManejadorCub(UIApplication uiapp, ManejadorCubDTO manejadorCubDTO)
        {
            this._uiapp = uiapp;
            this.manejadorCubDTO = manejadorCubDTO;
            this._lista_Level = new List<LevelDTO>();
            if (manejadorCubDTO.lista_view != null)           
                this._lista_Level.AddRange(manejadorCubDTO.lista_view);

            this._lista_losa = new List<LosaCubDto>();
            if (manejadorCubDTO.lista_Losa!= null)
                this._lista_losa.AddRange( manejadorCubDTO.lista_Losa);

            this._lista_Elev = new List<ElevCubDto>();
            if (manejadorCubDTO.lista_Elev != null)
                _lista_Elev.AddRange(manejadorCubDTO.lista_Elev);

            this._nombreProyecto = manejadorCubDTO.nombreProyecto;
            this._numeroObra = manejadorCubDTO.numeroObra;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            ListaCubBarra = new List<CubBarra>();
            _obtenerNivelPorOrigenBarra = new ObtenerNivelPorOrigenBarra(uiapp);
            ListaObjetos = new List<object[]>();
            _lista_Level_habilitados = new List<LevelDTO>();

        }

        public bool Ejecutar(View view = null)
        {

            UtilStopWatch _utilStopWatch = new UtilStopWatch();
            _utilStopWatch.IniciarMedicion();
            ConstNH.sbLog.Clear();
            ConstNH.sbLog.AppendLine($"Id,CasoRabar_,BarraIdem,TipoBarra_,Caso_,Tipo_cub,nivel,N_Barras,Dia[mm],Largo[cm],Eje,Orientacion_,Elemento_,peso[kg]");
             _lista_Level_habilitados = _lista_Level.Where(c => c.IsSelected).ToList();


            try
            {
                View3D view3D_Visualizar = TiposFamilia3D.Get3DBuscar(_doc, manejadorCubDTO.Nombre3d);

 

                _uiapp.ActiveUIDocument.ActiveView = view3D_Visualizar;
                string _PathName = _doc.PathName;
                string _name = $"{_numeroObra} {_nombreProyecto} {(DateTime.Now).ToString("MM_dd_yyyy")} (Delporte)";//    Path.GetFileName(_PathName);
                string _direc = Path.GetDirectoryName(_PathName);

                manejadorCubDTO.NombreArchivo = _name;
                manejadorCubDTO.RutaArchivo = _direc;

                if (view == null)
                    _view = view3D_Visualizar;
                else
                    _view = view;

                var lista1 = _uiapp.GetRibbonPanels("Diseño de Losas").SelectMany(pa => pa.GetItems()).First(it => it.Name == "BordeBarra");

                IVisibilidadView visibilidad = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_PathReinBoundary, "Boundary");

                bool result = visibilidad.EstadoActualHide();
                if (visibilidad.EstadoActualHide())
                    visibilidad.CambiarVisibilityBuiltInCategory();
                lista1.ItemText = (!visibilidad.EstadoActualHide() ? "On" : "Off");


                IVisibilidadView PathRein_ = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_PathRein, "Structural Path Reinforcement");
                if (PathRein_.EstadoActualHide())
                    PathRein_.CambiarVisibilityBuiltInCategory();

                IVisibilidadView Rebar_ = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_Rebar, "Structural Rebar");
                if (Rebar_.EstadoActualHide())
                    Rebar_.CambiarVisibilityBuiltInCategory();


                _SeleccionarRebar_PathReinforment = new SeleccionarRebar_PathReinforment(_uiapp, (view == null ? view3D_Visualizar : view));

                if (!M1_ValidacionesDatos()) return false;

                if (M2_ObtenerBArrasRebar())
                    ListaCubBarra.AddRange(ListaCubBarra_rebar);

                if (M3_ObtenerBArrasPAthReinf())
                    ListaCubBarra.AddRange(ListaCubBarra_path);

                //caopiar entre losas
                M4_Analisis_Losa();

                M5_Analisis_Elev();

                ManejadorSchedule _ManejadorSchedule = new ManejadorSchedule(_uiapp);
                _ManejadorSchedule.ObtenerDatosExcel_Volument_moldaje(_lista_Level_habilitados, false);

                //   M4_GuardarTxt();
                ExportarExcelDatos _ExportarExcelDatos = new ExportarExcelDatos(manejadorCubDTO);
                //  _ExportarExcelDatos.M4_GuardareExcel(ListaObjetos);
                _ExportarExcelDatos.M4_GuardareExcelHormigon(ListaObjetos, _ManejadorSchedule);

                //_ExportarExcelDatos.Exportar(ListaObjetos, RUTAOP);nn
                LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);//DEGUBnn
            }
            catch (Exception ex)
            {
                _utilStopWatch.Terminar();
                Util.ErrorMsg($"Error al exportar datos.\nDuracion:{_utilStopWatch._listIntervalos.Last()}.\n\n ex:{ex.Message}");
                return false;
            }

            _utilStopWatch.Terminar();
            Util.InfoMsg($"Cubicacion terminada.\nDuracion:{_utilStopWatch._listIntervalos.Last()}");
            return true;
        }

        private void M5_Analisis_Elev()
        {
            EntreElev _EntreElev = new EntreElev(ListaCubBarra, manejadorCubDTO);

            if (_EntreElev.VerificarSiExcluyenElev())
            {
                ListaCubBarra.Clear();
                ListaCubBarra.AddRange(_EntreElev.ListaCubBarra_SinElevExluidas);
                ListaObjetos.Clear();
                ListaObjetos.AddRange(_EntreElev.ListaCubBarra_SinElevExluidas.Select(x => x.ObtenerDato_array()));
            }

            if (_EntreElev.CopiarEntreaElev())
            {
                ListaCubBarra.Clear();
                ListaCubBarra.AddRange(_EntreElev.listaCubBarra);

                ListaObjetos.Clear();
                ListaObjetos.AddRange(_EntreElev.listaCubBarra.Select(x => x.ObtenerDato_array()));


                // AgregarRepetidos
                ListaCubBarra.AddRange(_EntreElev.ListaCubBarra_ConRepetidas);
                ListaObjetos.AddRange(_EntreElev.ListaCubBarra_ConRepetidas.Select(x => x.ObtenerDato_array()));
            }
        }

        private void M4_Analisis_Losa()
        {
            EntreLosas _EntreLosas = new EntreLosas(ListaCubBarra, manejadorCubDTO);

            if (_EntreLosas.VerificarSiExcluyenLosa())
            {
                ListaCubBarra.Clear();
                ListaCubBarra.AddRange(_EntreLosas.ListaCubBarra_SinLosaExluidas);
                ListaObjetos.Clear();
                ListaObjetos.AddRange(_EntreLosas.ListaCubBarra_SinLosaExluidas.Select(x => x.ObtenerDato_array()));
            }

            if (_EntreLosas.CopiarEntrealosas())
            {
                // ListaCubBarra.Clear();
                ListaCubBarra.AddRange(_EntreLosas.ListaCubBarra_ConRepetidas);
                ListaObjetos.AddRange(_EntreLosas.ListaCubBarra_ConRepetidas.Select(x => x.ObtenerDato_array()));
            }
        }

        private bool M1_ValidacionesDatos()
        {
            try
            {
                if (!_SeleccionarRebar_PathReinforment.M0_CArgar_PAthReinforment()) return false;
                if (!_SeleccionarRebar_PathReinforment.M0_Cargar_rebar()) return false;

                if (!_SeleccionarRebar_PathReinforment.M1_Ejecutar_rebar())
                {
                    Util.ErrorMsg("Error al obtener Lista Rebar");
                    return false;
                }
                if (!_SeleccionarRebar_PathReinforment.M1_Ejecutar_PAthReinforment())
                {
                    Util.ErrorMsg("Error al obtener Lista PAthReinforment");
                    return false;
                }

                if (!_obtenerNivelPorOrigenBarra.CalcularNivelesLosa())
                {
                    Util.ErrorMsg("Error al obtener niveles");
                    return false;
                };
                listaView = SeleccionarView.ObtenerView_losa_elev_fund(_uiapp.ActiveUIDocument);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en validad datos iniciales ex{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M2_ObtenerBArrasRebar()
        {
            try
            {
                ListaCubBarra_rebar = new List<CubBarra>();
                //var task_rebar = new Task(() =>
                //{
                    foreach (ElementoPathRebar item in _SeleccionarRebar_PathReinforment._lista_A_ElementoREbarDTO)
                    {
                        CubBarra _newCubBarra = new CubBarra(item, listaView, _obtenerNivelPorOrigenBarra, _lista_Level_habilitados);
                        if (_newCubBarra.Calcular_Rebar())
                        {
                         //   ConstNH.sbLog.AppendLine(_newCubBarra.ObtenerDato());
                            ListaCubBarra_rebar.Add(_newCubBarra);
                            ListaObjetos.Add(_newCubBarra.ObtenerDato_array());

                            if (_newCubBarra.ListaBArrasReparticion.Count > 0)
                            {
                                foreach (CubBarra itemReparticom in _newCubBarra.ListaBArrasReparticion)
                                {
                                    Debug.WriteLine($"caso traba malla:{i++}");
                                   // ConstNH.sbLog.AppendLine(itemReparticom.ObtenerDato());

                                    ListaCubBarra_rebar.Add(itemReparticom);
                                    ListaObjetos.Add(itemReparticom.ObtenerDato_array());
                                }
                            }
                        }
                        else
                        {
                            ConstNH.sbLog.AppendLine($"{i++}) rebar :{item._rebar.Id.IntegerValue}  --error");
                        }
                    }
                //});
                //task_rebar.Start();

                //task_rebar.Wait();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en calcular barras de rebar ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M3_ObtenerBArrasPAthReinf()
        {
            try
            {
                ListaCubBarra_path = new List<CubBarra>();
                i = 1;
                var task_path = new Task(() =>
                {
                    ObtenerListaRebarInSystemv();

                    foreach (ElementoPathRebarInSystem _elementoPathRein in _lista_A_DeRebarInSystemv2)
                    {
                        CubBarra _newCubBarra = new CubBarra(_elementoPathRein, listaView, _obtenerNivelPorOrigenBarra, _lista_Level_habilitados);
                        if (_newCubBarra.Calcular_RebarInSystem())
                        {
                            //ConstantesGenerales.sbLog.AppendLine(_newCubBarra.ObtenerDato());
                            ListaCubBarra_path.Add(_newCubBarra);
                            ListaObjetos.Add(_newCubBarra.ObtenerDato_array());

                            if (_newCubBarra.ListaBArrasReparticion.Count > 0)
                            {
                                foreach (CubBarra itemReparticom in _newCubBarra.ListaBArrasReparticion)
                                {
                                    ListaCubBarra_path.Add(itemReparticom);
                                    ListaObjetos.Add(itemReparticom.ObtenerDato_array());
                                }
                            }
                        }
                        else
                        {
                            //  ConstantesGenerales.sbLog.AppendLine($"{i++}) Pathreinforment :{_elementoPathRein._rebarInSystem.Id.IntegerValue}  --error");
                        }
                    }
                });
                task_path.Start();
                task_path.Wait();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en calcular barras de Pathreinforment ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void ObtenerListaRebarInSystemv()
        {
            _lista_A_DeRebarInSystemv2 = new List<ElementoPathRebarInSystem>();
            var ListaAllRebarInSystem = _SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO.SelectMany(c => c._lista_A_DeRebarInSystem).ToList();

            foreach (ElementoPathRein elementoPathRein in _SeleccionarRebar_PathReinforment._lista_A_ElementoPathReinformentDTO)
            {
                foreach (RebarInSystem rebarInSystem in elementoPathRein._lista_A_DeRebarInSystem)
                {
                    ElementoPathRebarInSystem visibilidadElementoPathDTO =
                    ElementoPathRebarInSystem.CrearVisibilidadElementoRebarhDTO(_doc, rebarInSystem, elementoPathRein._pathReinforcement, new List<Element>());
                    _lista_A_DeRebarInSystemv2.Add(visibilidadElementoPathDTO);
                }

            }
        }

        private bool M4_GuardarTxt()
        {
            bool result = false;
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog.FileName = "Cubicacion";
            //if (saveFileDialog.ShowDialog())
            //    ruta = saveFileDialog.FileName;
            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return false;

            string RutaArchivo = saveFileDialog.FileName;

            if ((RutaArchivo != null) && (RutaArchivo != ""))
            {

                if (Path.GetExtension(RutaArchivo) != ".txt")
                {
                    System.Windows.Forms.MessageBox.Show("Ruta de archivo no corresponde o formato incorrecto", "Mensaje");
                    return false;
                }
                string name = Path.GetFileName(RutaArchivo);
                string direc = Path.GetDirectoryName(RutaArchivo);

                LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, direc, name);

                System.Windows.Forms.MessageBox.Show("Exportacion de datos realizada correctamente", "Confirmation");

            }

            result = true;
            return result;
        }
    }
}
