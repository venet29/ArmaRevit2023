using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Viewnh;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ViewFilter;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.ParametrosShare.Actualizar;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.TextoNoteNH;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial
{
    public class ManejadorConfiguracionInicialGeneral
    {

        public static void cargar(UIApplication _uiapp, bool IsMje = true)
        {
            if (_uiapp == null) return;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            View _view = _doc.ActiveView;

            var result = Util.InfoMsg_YesNo($"Confirma que de desea continuar con Comando 'Cargar configuracion Inicial?.\nProceso podria durar unos minutos.");
            if (result == System.Windows.Forms.DialogResult.No) return; 
         
            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorConfiguracionInicialGeneral");

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

          
            UpdateGeneral.M5_DesCargarGenerar(_uiapp);


            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Inicio ConfiguracionInicialGeneral-NH");

                    DefinirArchivoShare.Ejecutar(_uiapp);

                    //limppiar lista de datos
                    LimpiandoListas.Limpiar();

                    //1-parametros compartidos
                    ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                    configuracionInicial.AgregarParametrosShareLosa();

                    //2-paramtros compartidos               
                    ConfiguracionInicialParametros configuracionInicialElev = new ConfiguracionInicialParametros(_uiapp);
                    configuracionInicialElev.AgregarParametrosShareElevacion();

                    configuracionInicialElev.AgregarParametrosShareElevacionElementos();

                    // desactiva los parametros compartidos de los viewtemplate
                    ViewTemplateInclude _viewTemplate = new ViewTemplateInclude(_uiapp);
                    _viewTemplate.Ejecutar();

                    //workset
                    CreadorWorkset _CreadorWorkset = new CreadorWorkset(_uiapp);
                    _CreadorWorkset.CreateWorkset_COnTrasn(new List<string>() { "HORMIGON" });
                   // _CreadorWorkset.AgregarElementosAHormigon();
                    _CreadorWorkset.CreateWorkset_COnTrasn(new List<string>() { "BARRA" });

                    //2-paramtros compartidos               
                    ConfiguracionInicialParametros configuracionInicialview = new ConfiguracionInicialParametros(_uiapp);
                    configuracionInicialview.AgregarParametrosShareView();

                    //crear lineas
                    CrearLineStyle CrearLineStyle = new CrearLineStyle(_doc, "Barra", 1, new Color(255, 0, 255), "IMPORT-HIDDEN");
                    CrearLineStyle.CreateLineStyleConTrans();

                    CrearLineStyle CrearLineStyle_Horq = new CrearLineStyle(_doc, "Horq",5 , new Color(0, 0, 0), "IMPORT-HIDDEN");
                    CrearLineStyle_Horq.CreateLineStyleConTrans();

                    CrearLineStyle CrearLineStyleBarraCirculo = new CrearLineStyle(_doc, "BarraCirculo", 1, new Color(0, 0, 0), "IMPORT-HIDDEN");
                    CrearLineStyleBarraCirculo.CreateLineStyleConTrans();

                    CrearLineStyle CrearLineStyleLineaDimension = new CrearLineStyle(_doc, "LineaDimension", 1, new Color(0, 0, 0), "IMPORT-HIDDEN");
                    CrearLineStyleLineaDimension.CreateLineStyleConTrans();

                    CrearLineStyle CrearLineStyleBarraCDimension = new CrearLineStyle(_doc, "BarraDimension", 1, new Color(0, 255, 0), "IMPORT-HIDDEN");
                    CrearLineStyleBarraCDimension.CreateLineStyleConTrans();

                    CrearLineStyle CrearLineStyleVerdePrimaria = new CrearLineStyle(_doc, "VerdePrimaria", 1, new Color(0, 255, 0), "IMPORT-HIDDEN");
                    CrearLineStyleVerdePrimaria.CreateLineStyleConTrans();

                    CrearLineStyle CrearLineStyleRojoPrimaria = new CrearLineStyle(_doc, "RojaSecundario", 1, new Color(255, 0, 0), "IMPORT-HIDDEN");
                    CrearLineStyleRojoPrimaria.CreateLineStyleConTrans();

                    CrearLineStyle CrearLineStylE_BLANCO = new CrearLineStyle(_doc, "BLANCO", 1, new Color(0, 0, 0), "Dash");
                    CrearLineStylE_BLANCO.CreateLineStyleConTrans();

                    // modificar recubrimieto
                    SeleccionarRebarCoverType modificarParametrosRoom = new SeleccionarRebarCoverType(_uiapp);
                    modificarParametrosRoom.ObtenerListaRebarCoverType();
                    modificarParametrosRoom.CambiarRecubrimiento(ConstNH.CONST_RECUBRIMIENTO_BAse2cm_MM);

                    //crear Hook
                    TipoRebarHookType.CrearHookIniciales(_doc);

                    //crear 3Dview
                    CreadorView _creadorView = new CreadorView(_uiapp);
                    _creadorView.M2_CrearVIew3D("3D_NoEditar");
                    _creadorView.M2_CrearVIew3D("3D_NoEditarOp2");


                    //crear tipotextNote
                    var ListaTipoNote = FActoryTipoTextNote.ObtenerLista();
                    CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.AcotarBarra);
                    _CrearTexNote.M2_CrearListaTipoText_ConTrans(ListaTipoNote);

                    // crear arrow
                    Tipos_Arrow.CrearArropwIniciales(_uiapp.ActiveUIDocument.Document);


                    CrearTipoDimension _newCrearDimension = new CrearTipoDimension(_uiapp, "DimensionRebar");
                    _newCrearDimension.CrearTipoDimensionConTrasn();

                    CrearTipoDimension _newCrearDimensionTraslpoa = new CrearTipoDimension(_uiapp, "Traslapo");
                    _newCrearDimensionTraslpoa.CrearTipoDimensionTraslpoConTrasn();

                    // crearfiltros para no ver barras en otras vista
                    CreadorViewFilterAllView _CreadorViewFilter = new CreadorViewFilterAllView(_uiapp);
                    _CreadorViewFilter.M1_CreateViewFilterEnTodasVistas();

                    _creadorView.Configurar3D_OcultarBarras();


                    // generar rebar, con radio de giro y dar color
                    //ManejadorCargarFAmilias manejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                    //manejadorCargarFAmilias.DuplicarFamilasReBarBarv2();
                    TiposRebarBarType.DuplicarFamilasReBarBarv2(_doc);

                    //crear fiilregion 
                    Tipos_FiilRegion.CrearFillIniciales(_doc);

                    //*** 22-09 -2022 solo se usa para cambiar letra de tag
                    //cambiar letras Arieal
                    //MAnejadorCambiarLetraArial _MAnejadorCambiarLetraArial = new MAnejadorCambiarLetraArial(_uiapp);
                    //_MAnejadorCambiarLetraArial.Cambiar();

                    transGroup.Assimilate();
                }
                if (IsMje) Util.InfoMsg("Datos cargados correctamente");

            }
            catch (Exception ex)
            {
                 
                Util.InfoMsg($"Error al cargar parametros ex:{ex.Message}");
            }

            UpdateGeneral.M4_CargarGenerar(_uiapp);
        }
    }
}
