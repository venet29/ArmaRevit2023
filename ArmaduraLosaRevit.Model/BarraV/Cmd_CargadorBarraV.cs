#region Namespaces
using Autodesk.Revit.Attributes;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Pasadas.WPFPasada;
using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.RebarLosa;
using ArmaduraLosaRevit.Model.Fund.WPFfund;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.WPFp_suple;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.WPFref;
using ArmaduraLosaRevit.Model.BarraAreaPath.WPFm;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto;
using ArmaduraLosaRevit.Model.BarraEstribo.WPFc;
using ArmaduraLosaRevit.Model.BarraEstriboP.WPFp;
using ArmaduraLosaRevit.Model.BarraEstriboV.WPFv;
using System;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.BarraV.WPFb;
using ArmaduraLosaRevit.Model.LosaEstructural.WPFp;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.LosaArmadura;
using ArmaduraLosaRevit.Model.EditarTipoPath;
using ArmaduraLosaRevit.Model.BarraV.Borrar;
using ArmaduraLosaRevit.Model.BarraV.Agrupar;
using ArmaduraLosaRevit.Model.GRIDS.NombreEje.WPFGrid;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Pin.WPF_pin;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa;
using ArmaduraLosaRevit.Model.Cubicacion;
using ArmaduraLosaRevit.Model.UTILES.CargarVarios;
using System.Reflection;
using ArmaduraLosaRevit.Model.BarraV.Copiar;
using ArmaduraLosaRevit.Model.Cambiar.CambiarEspesor;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Viewnh.UpDate;
using ArmaduraLosaRevit.Model.Fund.Traslapo;
using ArmaduraLosaRevit.Model.RefuerzoSupleMuro.WPFp;
using ArmaduraLosaRevit.Model.Fund.Editar.WPF;
using ArmaduraLosaRevit.Model.TablasSchedule;
using ArmaduraLosaRevit.Model.DatosExcel;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.WPF;
using ArmaduraLosaRevit.Model.FAMILIA.Varios;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.WPF;
using ArmaduraLosaRevit.Model.Traslapo.WPF;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.FAMILIA.Borrar;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.WPF_pathSymbol;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.WPFEdB;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Configuracion;
using ArmaduraLosaRevit.Model.Cubicacion.WPF;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.UTILES.Buscar;
using ArmaduraLosaRevit.Model.TablasSchedule.Wpf;
using ArmaduraLosaRevit.Model.ViewFilter.WPF;
#pragma warning disable CS0105 // The using directive for 'ArmaduraLosaRevit.Model.Pasadas.WPFPasada' appeared previously in this namespace
#pragma warning restore CS0105 // The using directive for 'ArmaduraLosaRevit.Model.Pasadas.WPFPasada' appeared previously in this namespace
using ArmaduraLosaRevit.Model.CopiaLocal.WPF;
using ArmaduraLosaRevit.Model.DocumentosNh;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.ExtStore.Prueba;
using ArmaduraLosaRevit.Model.ConfiguracionInicial.WPF;
using ArmaduraLosaRevit.Model.EstadoDesarrollo.WPF;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.WPF_Pasada;
using ArmaduraLosaRevit.Model.GRIDS.WPF_CambiarNombreGrid;
using ArmaduraLosaRevit.Model.Visibilidad.Servicio;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using System.Linq;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda;
using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB.Structure;
using System.Security.Cryptography;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.ParametrosShare.Servicio;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Automatico;


#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CargadorBarraV : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {


            CargadorBarraV _newCmd_CargadorBarraV = new CargadorBarraV(commandData);

            _newCmd_CargadorBarraV.ShowDialog();
            UIApplication _uiapp = commandData.Application;
            Document _doc = commandData.Application.ActiveUIDocument.Document;
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            if (_newCmd_CargadorBarraV.tipodeCaso == "prueba")
            {

                AyudaCreaarDefinition _AyudaCreaarDefinition = new AyudaCreaarDefinition(_uiapp);
                _AyudaCreaarDefinition.Ejecutar("Altura", "SpecTypeId.String.Text");
                ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
                _definicionManejador.EjecutarBIM();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "button_cambiarGrid")
            {
                ManejadorWPF_CambiarNombreGrid _ManejadorWPF_CambiarNombreGrid = new ManejadorWPF_CambiarNombreGrid(_uiapp);
                _ManejadorWPF_CambiarNombreGrid.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "AcotarShaft")
            {
                ManejadorWPF_AcotarPasada _ManejadorWPF_AcotarPasada = new ManejadorWPF_AcotarPasada(_uiapp);
                _ManejadorWPF_AcotarPasada.Execute();
                //ManejadorAcotarPAsada _ManejadorAcotarPAsada = new ManejadorAcotarPAsada(_uiapp);
                ////_ManejadorAcotarPAsada.EjecutarAcotarPasadasVarios();
                //_ManejadorAcotarPAsada.M4_EjecutarAcotarPasadasD_conRectagulo_2doPtoAutomatico();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "estadoProyecto")
            {
                ManejadorWPF_UI_EstadoDesarrollo _ManejadorWPF_UI_EstadoDesarrollo = new ManejadorWPF_UI_EstadoDesarrollo(_uiapp);
                _ManejadorWPF_UI_EstadoDesarrollo.Execute();
            }
            //else if (_newCmd_CargadorBarraV.tipodeCaso == "VigaAuto_")
            //{
            //    VariablesSistemas.IsUsarTraslapoDimension_sinMoverBarras = true;
            //    ManejadorVigasAuto _ManejadorVigasAuto = new ManejadorVigasAuto(commandData.Application);
            //    _ManejadorVigasAuto.ImportarBarrasViga();

            //}
            else if (_newCmd_CargadorBarraV.tipodeCaso == "confiiguracion_Elevaciones")
            {
                Manejador_COnf_Elevaciones.Ejecutar(commandData.Application);

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "Actualizar_familias")
            {
                ManejadorSaveArchivos _ManejadorSaveArchivos = new ManejadorSaveArchivos(_uiapp);
                _ManejadorSaveArchivos.EjecutarVArios_op2();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "pasadas")
            {
                //  ManejadorWPF_UI_Pasadas _ManejadorWPF_UI_Pasadas = new ManejadorWPF_UI_Pasadas();
                ManejadorWPF_UI_Pasadas _ManejadorWPF_UI_Pasadas = new ManejadorWPF_UI_Pasadas(commandData.Application);
                _ManejadorWPF_UI_Pasadas.Execute();

            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "button_copiaLocal")
            {

                ManejadorWPF_CopiaLocal _ManejadorWPF_CopiaLocal = new ManejadorWPF_CopiaLocal(commandData.Application);
                _ManejadorWPF_CopiaLocal.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "Filtro3D")
            {

                ManejadorWPF_Filtro3D _ManejadorWPF_Filtro3D = new ManejadorWPF_Filtro3D(commandData.Application);
                _ManejadorWPF_Filtro3D.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "button_losaexporta")
            {

                Formulario formulario = new Formulario(commandData);
                formulario.ExportardatosParaIngenieria();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "LosaImportInf")
            {

                VariablesSistemasDTO vdto = new VariablesSistemasDTO()
                {
                    IsConAhorro = VariablesSistemas.IsConAhorro,
                    IsDibujarS4 = VariablesSistemas.IsDibujarS4,
                    IsVerificarEspesor = VariablesSistemas.IsVerificarEspesor,
                    IsAjusteBarra_Recorrido = VariablesSistemas.IsAjusteBarra_Recorrido,
                    IsAjusteBarra_Largo = VariablesSistemas.IsAjusteBarra_Largo,
                    IsReSeleccionarPuntoRango = false,
                    LargoBarras_cm = VariablesSistemas.LargoBarras_cm,
                    LargoRecorrido_cm = VariablesSistemas.LargoRecorrido_cm,
                    tipoPorF1 = VariablesSistemas.tipoPorF1.ToString(),
                    tipoPorF3 = VariablesSistemas.tipoPorF3.ToString(),
                    tipoPorF4 = VariablesSistemas.tipoPorF4.ToString()
                };

                FormularioAUTO formularioAUTO = new FormularioAUTO(vdto);
                formularioAUTO.ShowDialog();

                if (formularioAUTO.Ok)
                {
                    VariablesSistemas.IsConAhorro = formularioAUTO.IsAhorro;
                    VariablesSistemas.IsVerificarEspesor = formularioAUTO.IsConVerificar;

                    VariablesSistemas.LargoBarras_cm = formularioAUTO.LargoBarras;
                    VariablesSistemas.LargoRecorrido_cm = formularioAUTO.LargoRecorrido;

                    VariablesSistemas.tipoPorF1 = formularioAUTO.tipoPorF1;
                    VariablesSistemas.tipoPorF3 = formularioAUTO.tipoPorF3;
                    VariablesSistemas.tipoPorF4 = formularioAUTO.tipoPorF4;

                    Formulario formulario = new Formulario(commandData);
                    formulario.ImportarBarrasParaDibujo(TipoConfiguracionBarra.refuerzoInferior, formularioAUTO.configuracionAhorro, formularioAUTO.IsSoloCopiarDatos);
                }

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "LosaImportSup")
            {

                FormularioAUTOSUP formularioAUTO = new FormularioAUTOSUP();
                formularioAUTO.ShowDialog();

                if (formularioAUTO.Ok)
                {
                    Formulario formulario = new Formulario(commandData);
                    formulario.ImportarBarrasParaDibujo(TipoConfiguracionBarra.suple, formularioAUTO.configuracionAhorro);
                }


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "ListaPara")
            {
                ObtenerListaParametros.Ejecutar(commandData.Application.ActiveUIDocument);
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "CargarUpdateREbar")
            {
                switch (_newCmd_CargadorBarraV.tipoCasoUpdate)
                {
                    case "todos":
                        {
                            //    UpdateGeneral _UpdateGeneral = new UpdateGeneral(commandData.Application);
                            UpdateGeneral.M2_CargarBArras(_uiapp);
                            return Result.Succeeded;
                        }
                    case "barras":
                        {
                            //  Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(commandData.Application);
                            Manejador_UpdateRebar.CargarUpdateREbar(_uiapp);
                            return Result.Succeeded;
                        }
                    case "view":
                        {
                            //ManejadorUpdaterNombreView _manejadorUpdaterNombreView = new ManejadorUpdaterNombreView(commandData.Application);
                            ManejadorUpdaterNombreView.CargarUpdaterNombreView(_uiapp);
                            return Result.Succeeded;
                        }
                    default:
                        break;
                }


                _newCmd_CargadorBarraV.ShowDialog();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "DesCargarUpdateREbar")
            {
                switch (_newCmd_CargadorBarraV.tipoCasoUpdate)
                {
                    case "todos":
                        {
                            //UpdateGeneral _UpdateGeneral = new UpdateGeneral(commandData.Application);
                            UpdateGeneral.M3_DesCargarBarras(_uiapp);
                            return Result.Succeeded;
                        }
                    case "barras":
                        {
                            //    Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(commandData.Application);
                            Manejador_UpdateRebar.DesCargarUpdateREbar(_uiapp);
                            return Result.Succeeded;
                        }
                    case "view":
                        {
                            //ManejadorUpdaterNombreView _manejadorUpdaterNombreView = new ManejadorUpdaterNombreView(commandData.Application);
                            ManejadorUpdaterNombreView.DesCargarUpdaterNombreView(_uiapp);
                            return Result.Succeeded;
                        }
                    default:
                        break;
                }
                _newCmd_CargadorBarraV.ShowDialog();
            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "ConfigLosa")
            {

                ManejadorConfigLosa _ManejadorConfigLosa = new ManejadorConfigLosa();

                return _ManejadorConfigLosa.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "actualizaNombre1_vistaActual")
            {
                ManejadorVisibilidadActualizarNombreVista _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadActualizarNombreVista(_uiapp);
                _ManejadorVisibilidadElemenNoEnView.Ejecutar();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "rebarInclinadoEspecial")
            {

                ArmaduraLosaRevit.Model.ElementoBarraRoom.WPFp.ManejadorWPF manejadorWPF = new ArmaduraLosaRevit.Model.ElementoBarraRoom.WPFp.ManejadorWPF(commandData);
                manejadorWPF.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "REbarInclinadas")
            {
                probar_fx_sx _probar_fx_sx = new probar_fx_sx();
                _probar_fx_sx.Text = "Losa inclinadas";
                _probar_fx_sx.ShowDialog();

                // Manejador_UpdateRebar _manejadorUpdateRebar = new Manejador_UpdateRebar(commandData.Application);
                Manejador_UpdateRebar.DesCargarUpdateREbar(commandData.Application);
                if (_probar_fx_sx.IsOK)
                {
                    ArmaduraLosaRevit.Model.Enumeraciones.TipoBarra varsad = EnumeracionBuscador.ObtenerEnumGenerico(ArmaduraLosaRevit.Model.Enumeraciones.TipoBarra.NONE, _probar_fx_sx.tipobarra);
                    ManejadorRebarLosaInclinada barraManejador = new ManejadorRebarLosaInclinada(commandData.Application);
                    barraManejador.BarraInferiores(varsad, _probar_fx_sx.direccion);
                }
                Manejador_UpdateRebar.CargarUpdateREbar(commandData.Application);
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "wpfFundaciones")
            {

                ManejadorWPFfund manejadorWPF = new ManejadorWPFfund(commandData);
                manejadorWPF.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "editarBarra")
            {

                ManejadorWPF_EditarBarraLargo manejadorWPF_EstriboV = new ManejadorWPF_EditarBarraLargo(commandData);
                manejadorWPF_EstriboV.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "refuerzoFUndaciones")
            {
                ManejadorWPFref manejadorWPF = new ManejadorWPFref(commandData, TipoRefuerzoLOSA.fundacion);
                manejadorWPF.Execute();

            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "dibujarPathsymbol")
            {

                ManejadorWPF_Ui_pathSymbol _ManejadorWPF_Ui_pathSymbol = new ManejadorWPF_Ui_pathSymbol(commandData);
                _ManejadorWPF_Ui_pathSymbol.Execute();

            }


            else if (_newCmd_CargadorBarraV.tipodeCaso == "WPFSuple")
            {
                ManejadorWPFSuple manejadorWPF = new ManejadorWPFSuple(commandData);
                manejadorWPF.Execute();

            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "editarPathRein")
            {
                CargarEditPathReinforme _CargarPathReinforme = new CargarEditPathReinforme(commandData, TabEditarPath.Datos);
                _CargarPathReinforme.Cargar();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "editFund")
            {
                CargarEditFundaciones _CargarEditFundaciones = new CargarEditFundaciones(commandData.Application, TabEditarPath.Datos);
                _CargarEditFundaciones.Cargar();

            }
            //else if (_newCmd_CargadorBarraV.tipodeCaso == "editFund")
            //{
            //    ManejadorEditarFundaciones _manejadorEditarFundaciones = new ManejadorEditarFundaciones(commandData.Application);
            //    return _manejadorEditarFundaciones.EjecutarEdicion();
            //}

            else if (_newCmd_CargadorBarraV.tipodeCaso == "WPFRefuerzoLosa")
            {
                ManejadorWPFref manejadorWPF = new ManejadorWPFref(commandData, TipoRefuerzoLOSA.losa);
                manejadorWPF.Execute();


            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "caso_barras_Auto")
            {
                ManejadorWPF_AutoElev manejadorWPF_BarraV = new ManejadorWPF_AutoElev(commandData);
                manejadorWPF_BarraV.Execute();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "caso_barras_manual")
            {
                //var datos = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                probar_fx_sx probar_fx_sx = new probar_fx_sx();

                probar_fx_sx.ShowDialog();

                DatosDiseño.DISENO_VALIDAR_ESPESOR = TipoValidarEspesor.NOVerificarEspesorMenor15;

                bool valor_antiguo = VariablesSistemas.IsAjusteBarra_Recorrido;
                VariablesSistemas.IsAjusteBarra_Recorrido = probar_fx_sx.IsAjusteBarra_Recorrido;

                //  Result result = CargarBarraRoom.Cargar(commandData.Application, probar_fx_sx.tipobarra, probar_fx_sx.direccion);

                CargarBarraRoomDTO _CargarBarraRoomDTO = probar_fx_sx.ObtenerCargarBarraRoomDTO();
                if (_CargarBarraRoomDTO == null) return Result.Failed;
                Result result = CargarBarraRoom.Cargar(commandData.Application, _CargarBarraRoomDTO);


                VariablesSistemas.IsAjusteBarra_Recorrido = valor_antiguo;
                return result;


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "agruparbarra")
            {
                ManejadorAgrupar ManejadorAgrupar = new ManejadorAgrupar(commandData.Application);
                ManejadorAgrupar.M1_EjecutarVerticales();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "agruparbarraH")
            {
                ManejadorAgrupar ManejadorAgrupar = new ManejadorAgrupar(commandData.Application);
                ManejadorAgrupar.M2_EjecutarHorizontal();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "caso_barras")
            {
                ManejadorWPF_BarraV manejadorWPF_BarraV = new ManejadorWPF_BarraV(commandData);
                manejadorWPF_BarraV.Execute();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "caso_malla")
            {

                ManejadorWPF_Malla manejadorWPF_Malla = new ManejadorWPF_Malla(commandData);
                manejadorWPF_Malla.Execute();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "caso_econf")
            {

                ManejadorWPF_EstriboC manejadorWPF_EstriboC = new ManejadorWPF_EstriboC(commandData);
                manejadorWPF_EstriboC.Execute();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "caso_epilar")
            {
                ManejadorWPF_EstriboP manejadorWPF_EstriboPilar = new ManejadorWPF_EstriboP(commandData);
                manejadorWPF_EstriboPilar.Execute();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "caso_eviga")
            {

                ManejadorWPF_EstriboV manejadorWPF_EstriboV = new ManejadorWPF_EstriboV(commandData);
                manejadorWPF_EstriboV.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "crear_pelota_losa")
            {
                ManejadorWPF_pelotaLosa _manejadorWPF_pelotaLosa = new ManejadorWPF_pelotaLosa(commandData);
                _manejadorWPF_pelotaLosa.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "crear_room_losa")
            {
                ISeleccionAnotationPelotaLosa _seleccionAnotationPelotaLosa = new SeleccionAnotationPelotaLosa();
                View view = commandData.Application.ActiveUIDocument.Document.ActiveView;
                CreardorCrearRoomConPelotaLosa crearRoomConPelotaLosa = new CreardorCrearRoomConPelotaLosa(commandData, view, _seleccionAnotationPelotaLosa);
                return crearRoomConPelotaLosa.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "button_crearRoomLosa")
            {
                ISeleccionAnotationPelotaLosa _seleccionAnotationPelotaLosa = new SeleccionAnotationPelotaLosa();
                View view = commandData.Application.ActiveUIDocument.Document.ActiveView;
                ISeleccionarLosaConMouse seleccionarLosaConMouse = new SeleccionarLosaConMouse(commandData.Application);

                CreardorCrearRoomConPelotaLosaIndividual crearRoomConPelotaLosa =
                    new CreardorCrearRoomConPelotaLosaIndividual(commandData, view, _seleccionAnotationPelotaLosa, seleccionarLosaConMouse);
                return crearRoomConPelotaLosa.Execute_seleccionandoLosa();
            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "borrarLosaRebar")
            {

                BorrarRebarRectangulo.EjecutarLosa(commandData.Application);
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "Elevbarra")
            {
                BorrarRebarRectangulo.EjecutarElevaciones(commandData.Application, "soloBarras");
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "ElevTodos")
            {
                BorrarRebarRectangulo.EjecutarElevaciones(commandData.Application, "todos");
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "Elevmalla")
            {
                BorrarRebarRectangulo.EjecutarElevaciones(commandData.Application, "soloMalla");
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "crearTextoGrid")
            {
                // MAnejadorCrearTextoEje _MAnejadorCrearTextoEje = new MAnejadorCrearTextoEje(commandData.Application);
                //  _MAnejadorCrearTextoEje.GenerarTexto();
                ManejadorWPF_Ui_UIGrid manejadorWPF_EstriboV = new ManejadorWPF_Ui_UIGrid(commandData);
                manejadorWPF_EstriboV.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "PelotaLosaEstru")
            {
                ManejadorWPF_pelotaLosa manejadorWPF = new ManejadorWPF_pelotaLosa(commandData);
                manejadorWPF.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "ExtStore")
            {
                ManejadotExtStore_Wall _ManejadotExtStore = new ManejadotExtStore_Wall(commandData.Application);
                _ManejadotExtStore.AgregarExtStore();
#pragma warning disable CS0219 // The variable 'tipo' is assigned but its value is never used
                int tipo = 0;
#pragma warning restore CS0219 // The variable 'tipo' is assigned but its value is never used
                //if(tipo==0)
                //    _ManejadotExtStore.AgregarExtStore_multiples();
                //else if (tipo == 1)
                //    _ManejadotExtStore.UpdateExtStore_multiples();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "_AssemblyInstanceNH")
            {

                AssemblyInstancenh _AssemblyInstancenh = new AssemblyInstancenh();
                _AssemblyInstancenh.ListElementsInAssembly(commandData.Application.ActiveUIDocument.Document);
            }


            else if (_newCmd_CargadorBarraV.tipodeCaso == "traslapar")
            {


                ManejadorWPF_Traslapo _ManejadorWPF_Traslapo = new ManejadorWPF_Traslapo(commandData);
                _ManejadorWPF_Traslapo.Execute();


            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "traslaparFund")
            {
                CArgarTraslapoManejadorFund _CArgarTraslapoManejadorFund = new CArgarTraslapoManejadorFund(_uiapp);
                _CArgarTraslapoManejadorFund.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "pinCargar")
            {
                ManejadorWPF_Pin _ManejadorWPF_Pin = new ManejadorWPF_Pin(_uiapp);
                _ManejadorWPF_Pin.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "copiarEntrelosa")
            {
                ManejadorCopiaBarraLosa _manejadorCopiaBarraLosa = new ManejadorCopiaBarraLosa(_uiapp);
                _manejadorCopiaBarraLosa.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "cub" || _newCmd_CargadorBarraV.tipodeCaso == "schedule")
            {

                Ui_Schedule _FormCub = new Ui_Schedule(_uiapp);
                _FormCub.ShowDialog();

                if (_FormCub.casoTipo == "btn_crearExcel")
                {

                    ManejadorWPF_Cub manejadorWPF = new ManejadorWPF_Cub(_uiapp);
                    manejadorWPF.Execute();

                }
                else if (_FormCub.casoTipo == "btn_crearSchedules")
                {
                    ManejadorSchedule _ManejadorSchedule = new ManejadorSchedule(_uiapp);
                    _ManejadorSchedule.CrearSchedule_CubicacionBarras();
                }

                //ManejadorWPF_Cub manejadorWPF = new ManejadorWPF_Cub(_uiapp);
                //manejadorWPF.Execute();
            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "datosschedule")
            {
                ManejadorSchedule _ManejadorSchedule = new ManejadorSchedule(_uiapp);
                _ManejadorSchedule.ObtenerDatosExcel_Volument_moldaje(new List<LevelDTO>());
                var manejadorCubDTO = new ManejadorCubDTO();
                ExportarExcelDatos _ExportarExcelDatos = new ExportarExcelDatos(manejadorCubDTO);
                _ExportarExcelDatos.M4_GuardareExcelHormigon(null, _ManejadorSchedule);
            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "CArgar_CopiarElev")

            {

                CArgar_CopiarElev_WPF.Ejecutar(_uiapp);
            }

            else if (_newCmd_CargadorBarraV.tipodeCaso == "Formayuda")
            {

                //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
                try
                {
                    UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                    ManejadorFormVariosAyudas _ManejadorFormVariosAyudas = new ManejadorFormVariosAyudas(_uiapp);
                    _ManejadorFormVariosAyudas.Ejecutar(Assembly.GetExecutingAssembly());

                    UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

                }
                catch (System.Exception ex)
                {

                    string msje = ex.Message;

                }
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "cambio_espesor")
            {
                ISeleccionarLosaConMouse ISeleccionarLosaConMose = new SeleccionarLosaConMouse(commandData.Application);
                IManejadorPathCambioEspesor iManejadorPathCambioEspesor = new ManejadorPathCambioEspesor(commandData);
                IManejadorRoomCambioEspesor iManejadorRoomCambioEspesor = new ManejadorRoomCambioEspesor(commandData);
                CambiarEspesorLosa_CambiaPAthYRoom cambiarEspesorAParametroPath = new CambiarEspesorLosa_CambiaPAthYRoom(commandData,
                                                                                                                         ISeleccionarLosaConMose,
                                                                                                                         iManejadorPathCambioEspesor,
                                                                                                                         iManejadorRoomCambioEspesor);
                cambiarEspesorAParametroPath.Ejecutar();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "suple_muro")
            {
                ManejadorWPF_RefuerzoSupleMuro manejadorWPF = new ManejadorWPF_RefuerzoSupleMuro(commandData);
                return manejadorWPF.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "casoHorquilla")
            {
                ManejadorWPF_Horquilla _manejadorWPF_Horquilla = new ManejadorWPF_Horquilla(commandData.Application);
                return _manejadorWPF_Horquilla.Execute();
            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "BlancoNegro")
            {
                ManejadorWPF_FormatoEntrega _ManejadorWPF_FormatoEntrega = new ManejadorWPF_FormatoEntrega(commandData.Application);
                return _ManejadorWPF_FormatoEntrega.Execute();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "borrarPArametros")
            {
                DefinicionBorrarManejador _definicionManejador = new DefinicionBorrarManejador(commandData.Application);
                _definicionManejador.EjecutarBorrar();

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "borrarfamilias")
            {

                BorrarFamilia borrarFamilia = new BorrarFamilia(commandData.Application);
                borrarFamilia.BorrarTodasLasFamilias();
                TaskDialog.Show("ok", "Elementos cargados correctamente");

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "cargarFamiliasYotros")
            {
                try
                {
                    ManejadorConfiguracionInicialGeneral.cargar(commandData.Application, false);
                }
                catch (Exception ex)
                {
                    // If there are something wrong, give error information and return failed
                    message = ex.Message;
                    UpdateGeneral.M3_DesCargarBarras(_uiapp);
                    return Autodesk.Revit.UI.Result.Failed;
                }

                //UpdateGeneral _updateGeneral = new UpdateGeneral(commandData.Application);
                UpdateGeneral.M5_DesCargarGenerar(_uiapp);
                try
                {
                    //
                    ManejadorCargarFAmilias manejadorCargarFAmilias = new ManejadorCargarFAmilias(commandData.Application);
                    // manejadorCargarFAmilias.DuplicarFamilasReBarBarv2();
                    manejadorCargarFAmilias.cargarFamilias_run();

                    // cambiar pelota de directriz 
                    PelotaDedirectriz PelotaDedirectriz = new PelotaDedirectriz(commandData.Application);
                    PelotaDedirectriz.Ejecutar();

                    TaskDialog.Show("ok", "Elementos cargados correctamente");
                }
                catch (Exception ex)
                {
                    // If there are something wrong, give error information and return failed
                    message = ex.Message;
               
                }
                UpdateGeneral.M4_CargarGenerar(_uiapp);
                TaskDialog.Show("ok", "Elementos cargados correctamente");

            }
            else if (_newCmd_CargadorBarraV.tipodeCaso == "cargarParametros")
            {
                ManejadorConfiguracionInicialGeneral.cargar(commandData.Application);

            }

            return Result.Succeeded;

        }
    }
}

