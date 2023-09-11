using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Viewnh;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.ViewFilter;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Prueba.User;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial
{
    public class ManejadorConfiguracionInicialLosa
    {
        private readonly UIApplication _uiapp;
        private readonly string nombreTemplete;
        private Document _doc;
        private View _view;

        public ManejadorConfiguracionInicialLosa(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
             _view = _doc.ActiveView;
    
            this.nombreTemplete = ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA; 
        }
        public  void cargar()
        {
            if (_uiapp == null) return;
             
        
            if (BuscarIsNombreViewActualizado.IsError_SOloconfiguracionInicial(_view,TipoBarraGeneral.Losa)) return;


            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorConfiguracionInicialLosa");

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

          //  //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M5_DesCargarGenerar(_uiapp);
         

            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Inicio ConfiguracionInicialLosa-NH");
                    LimpiandoListas.Limpiar();

                    CreadorView _CreadorView = new CreadorView(_uiapp);
                    _CreadorView.M2_ViewTemplateLosa(_view, nombreTemplete);//ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV
                    _CreadorView.M2_SinViewTemplate(_view); 
                    _CreadorView.M2_ConfiguracionLosa(_view);

                    VisibilidadActualizarNombreVista _VisibilidadActualizarNombreVista = new VisibilidadActualizarNombreVista(_uiapp, "");
                    _VisibilidadActualizarNombreVista.ActualizaParametroEnViewForzado(_view);

                    ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                    configuracionInicial.ReiniciarViewRange();
                    //  configuracionInicial.AgregarParametrosShareLosa();

                    TiposPathReinformentSymbolElement _TiposPathReinformentSymbolElement = new TiposPathReinformentSymbolElement();
                    _TiposPathReinformentSymbolElement.ObtenerPathReinfDefaul_forzado(_doc);
                    ////crear lineas
                    //CrearLineStyle CrearLineStyle = new CrearLineStyle(uiapp.ActiveUIDocument.Document, "Barra", 1, new Color(255, 0, 255), "IMPORT-HIDDEN");
                    //CrearLineStyle.CreateLineStyle();

                    ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(_uiapp);
                    _ManejadorVisibilidadElemenNoEnView.Ejecutar(_view, _view.ObtenerNombreIsDependencia());

                    //CrearLineStyle CrearLineStylE_BLANCO = new CrearLineStyle(uiapp.ActiveUIDocument.Document, "BLANCO", 1, new Color(0, 0, 0), "Dash");
                    //CrearLineStylE_BLANCO.CreateLineStyle();


                    // esta opcion mas quenada es para resetear  3D_noEditar
                    CreadorView CreadorView = new CreadorView(_uiapp);
                    CreadorView.M2_CrearVIew3D(ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR);

                    //configura visibility/graphics    -->> hotkey 'vv'
                    IVisibilidadView visibilidad2 = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_Rooms, "Interior Fill");
                    visibilidad2.CambiarVisibilityBuiltInCategory(true);

                    IVisibilidadView visibilidad3 = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_Rooms, "Reference");
                    visibilidad3.CambiarVisibilityBuiltInCategory(true);

                    IVisibilidadView visibilidad4 = VisibilidadView.Creador_Visibilidad(_view, BuiltInCategory.OST_Grids, "Grids");
                    visibilidad4.CambiarVisibilityBuiltInCategory(true);

                    //
                    VisibilidadCategorias visibilidadView = new VisibilidadCategorias(_view);
                    List<string> listaExclusion = new List<string>();

                    visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Annotation, ListaExclusion_.ListaExclusionLosaAnotatior());
                    visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.AnalyticalModel, listaExclusion);
                    visibilidadView.CambiarEstado_ConlistaExclusion(true, CategoryType.Model, ListaExclusion_.ListaExclusionLosa());

                    //cargar filtros para solo ver barras creados en esta vista
                    CreadorViewFilter_OcultarBarrasNoDeVista _CreadorViewFilter = new CreadorViewFilter_OcultarBarrasNoDeVista(_uiapp);
                    _CreadorViewFilter.M2_CreateViewFilterLosa(_view);

                    transGroup.Assimilate();
                }
                Util.InfoMsg("Datos cargados correctamente");

            }
            catch (Exception)
            {

                Util.InfoMsg("Error al cargar parametros");
            }

            UpdateGeneral.M4_CargarGenerar(_uiapp);

        }
    }
}
