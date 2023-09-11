using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Viewnh;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Visibilidad;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ViewFilter;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Seleccionar;
using Microsoft.VisualBasic;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.Pasadas.WPF;
using ArmaduraLosaRevit.Model.ConfiguracionInicial.Elemento;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial
{
    public class ManejadorConfiguracionInicialElevacion
    {
        private UIApplication _uiapp;
        private readonly string nombreTemplete;
        private Document _doc;
        private View _view;

        public string listaID { get; set; }

        private bool IsEncontrarDesAtachados;

        public ManejadorConfiguracionInicialElevacion(UIApplication _uiapp,string nombreTemplete)
        {
            this._uiapp = _uiapp;
            this.nombreTemplete = nombreTemplete;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
            listaID = "Lista ID=> \n";
            IsEncontrarDesAtachados = false;
        }
        public void cargar()
        {
            if (_uiapp == null) return;


            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorConfiguracionInicialElevacion");

            if (!resultadoConexion)
            {
                Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                return;
            }
            else if (!_ManejadorUsuarios.ObteneResultado())
            {
                Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                return;
            }


            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return;

            if (BuscarIsNombreViewActualizado.IsError_SOloconfiguracionInicial(_view, TipoBarraGeneral.Elevacion)) return;

            CreadorView.IsMje = true;
            LimpiandoListas.Limpiar();
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M5_DesCargarGenerar(_uiapp);

            try
            {

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CargarConfiguracion-NH");
                    CargarEnvista(_view);
                    t.Assimilate();
                }




                if (IsEncontrarDesAtachados)
                {
                    Util.InfoMsg($"Datos cargados correctamente.\nSe encontraron {SeleccionarWalls.listAWall.Count} muro sin atachar. En 'misDocumentos' se encuentra archivo 'listaMurodesatachados.txt' con los id de los muros. Si Muro no atachado a losa puede generar problemas al crear barras verticales.\n {listaID}");

                    ConstNH.sbLog.Clear();
                    ConstNH.sbLog.Append(listaID);
                    string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, mydocpath, "listaMurodesatachados.txt");
                }
                else
                    Util.InfoMsg("Datos cargados correctamente");

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }

            UpdateGeneral.M4_CargarGenerar(_uiapp);
        }

        public void cargarTodos()
        {
            if (_uiapp == null) return;

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorConfiguracionInicialElevacion");

            if (!resultadoConexion)
            {
                Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                return;
            }
            else if (!_ManejadorUsuarios.ObteneResultado())
            {
                Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                return;
            }

            RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
            if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return;
            // if (BuscarIsNombreViewActualizado.IsError_SOloconfiguracionInicial(_view, TipoBarraGeneral.Elevacion)) return;
            CreadorView.IsMje = true;
            SeleccionarView _SeleccionarView = new SeleccionarView();
            var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc);

            string input = Interaction.InputBox($"Seguro desea cargar 'configuracion' a {ListaViewSection.Count} view de elevaciones del proyecto?. Proceso puede tardar unos minutos.\n\nConfirmar escribiendo : Elevacion", "Borrar", "", 300, 300);
            if (input.Trim().ToLower() != "elevacion") return;

            LimpiandoListas.Limpiar();
      
            UpdateGeneral.M5_DesCargarGenerar(_uiapp);
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CargarConfiguracion-NH");

                    for (int i = 0; i < ListaViewSection.Count; i++)
                    {
                        if (!CargarEnvista(ListaViewSection[i]))
                        {
                            var result = Util.InfoMsg_YesNo($"Desea continuar?. Falta {ListaViewSection.Count - 1 - i} view por cargar configuraciones.");
                            if (result == System.Windows.Forms.DialogResult.No)
                            {
                                i = ListaViewSection.Count;
                                break;
                            }
                        }
                    }
                    t.Assimilate();
                }
                if (IsEncontrarDesAtachados)
                {
                    Util.InfoMsg($"Datos cargados correctamente.\nSe encontraron {SeleccionarWalls.listAWall.Count} muro sin atachar. En 'misDocumentos' se encuentra archivo 'listaMurodesatachados.txt' con los id de los muros. Si Muro no atachado a losa puede generar problemas al crear barras verticales.\n {listaID}");

                    ConstNH.sbLog.Clear();
                    ConstNH.sbLog.Append(listaID);
                    string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) ;
                    LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, mydocpath, "listaMurodesatachados.txt");

                }

                else
                    Util.InfoMsg("Datos cargados correctamente");
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }


            UpdateGeneral.M4_CargarGenerar(_uiapp);
        }
        private bool CargarEnvista(View _view, bool IsMensaje = true)
        {
            bool result = true;
            try
            {


                CreadorView _CreadorView = new CreadorView(_uiapp);
                _CreadorView.M2_ViewTemplateElevacion(_view, nombreTemplete );//ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV
                _CreadorView.M2_SinViewTemplate(_view);

                VisibilidadActualizarNombreVista _VisibilidadActualizarNombreVista = new VisibilidadActualizarNombreVista(_uiapp, "");
                _VisibilidadActualizarNombreVista.ActualizaParametroEnViewForzado(_view);

                ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(_uiapp);
                _ManejadorVisibilidadElemenNoEnView.Ejecutar(_view, _view.ObtenerNombreIsDependencia());

                _CreadorView.M2_ConfiguracionElevacionDibujar(_view);


                // esta opcion mas quenada es para resetear  3D_noEditar
                CreadorView CreadorView = new CreadorView(_uiapp);
                CreadorView.M2_CrearVIew3D(ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR);

                //if (uiapp == null) return;
                //ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(uiapp);

                //cargar filtros para solo ver barras creados en esta vista
                CreadorViewFilter_OcultarBarrasNoDeVista _CreadorViewFilter = new CreadorViewFilter_OcultarBarrasNoDeVista(_uiapp);
                _CreadorViewFilter.ObtenerNombreAntiguoVista(_view);
                _CreadorViewFilter.M2_CreateViewFilterElev(_view);

                VisibilidadCategorias visibilidadView = new VisibilidadCategorias(_view);
                List<string> listaExclusion = new List<string>();


                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Annotation, ListaExclusion_.ListaExclusionLosaAnotatior());
                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.AnalyticalModel, listaExclusion);
                visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Model, ListaExclusion_.ListaExclusionElev());

                SeleccionarWalls.SeleccionarTodasElementos(_doc, _view);

                listaID = listaID + $"\n{_view.Name}--->";
                if (SeleccionarWalls.listAWall.Count > 0 && IsMensaje)
                {

                    SeleccionarWalls.listAWall.ForEach(c => listaID = listaID + (c.get_Parameter(BuiltInParameter.WALL_TOP_IS_ATTACHED)?.AsInteger() == 0 ? c.Id.IntegerValue.ToString() + " ," : ""));

                    IsEncontrarDesAtachados = true;
                }

                // agrega el fill region por la especialidad de las pasada
                Manejador_PASADASBIM_en_elevaciones.EjecutarVistaActual(_uiapp,_view);

                //analizar muros de coronacion
                ManejadorPreAnalisisELemento _ManejadorPreAnalisisELemento = new ManejadorPreAnalisisELemento(_uiapp);
                _ManejadorPreAnalisisELemento.AnalisarMuros();
            }
            catch (Exception ex)
            {

                Util.InfoMsg($"Error al cargar parametros. ex:{ex.Message}");
                return false;
            }
            return result;
        }
    }
}
