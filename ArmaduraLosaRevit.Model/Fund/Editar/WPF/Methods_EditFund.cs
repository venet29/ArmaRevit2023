using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.EditarTipoPath;
using ArmaduraLosaRevit.Model.Fund.DTO;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.WPFEdB;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.EditarBarra;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.WPFref;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS;

namespace ArmaduraLosaRevit.Model.Fund.Editar.WPF
{
    /// <summary>
    /// 
    /// </summary>
    internal class Methods_EditFund
    {






        public static void M1_DocumentEditPAth(Ui_FundEditar ui, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;

            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);

            string tipoPosiicon = ui.BotonOprimido;
            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);

            ui.Hide();
            bool aux_IS_MENSAJE_BUSCAR_ROOM = VariablesSistemas.IS_MENSAJE_BUSCAR_ROOM;
            VariablesSistemas.IS_MENSAJE_BUSCAR_ROOM = false;
            try
            {
                if (tipoPosiicon == "pathtoPto")
                {
                    EditarPathReinMouse_ExtederPathA2punto _EditarPathReinMouse_ExtederPathApunto = new EditarPathReinMouse_ExtederPathA2punto(_uiapp);
                    M1_2_MetodoPathToPto_pto2(ui, _EditarPathReinMouse_ExtederPathApunto);
                }
                else if (tipoPosiicon == "RebartoPto")
                {
                    EditarBarraDTO newEditarBarraDTO = new EditarBarraDTO()
                    {
                        IsCambiarDiametroYEspacia = false
                    };

                    if (!Util.IsNumeric(ui.distanPtoPto.Text)) return;

                    double dist = Util.ConvertirStringInDouble(ui.distanPtoPto.Text);
                    EditarBarraLargoDTO _EditarBarraLargoDTO = new EditarBarraLargoDTO()
                    {
                        IsUsarMouse = true,
                        DeltaUsarMouse_cm = dist
                    };

                    ManejadorBarraV_LargoBarra ManejadorBarraV_CambiarBarra = new ManejadorBarraV_LargoBarra(_uiapp, newEditarBarraDTO, _EditarBarraLargoDTO);
                    ManejadorBarraV_CambiarBarra.CambiarLargoBarra();
                }

                else if (tipoPosiicon == "btnSoloDistancia")
                {
                    M1_4_MetodoPathDistanciaMouse(ui, EditarPathReinMouse);
                }
                else if (tipoPosiicon == "btnActualizar")
                {
                    M1_6_MetodoPathActulizarTipoPreseleccionado(ui, _uiapp);
                }
                else if (tipoPosiicon == "barraSinH" || tipoPosiicon == "barraInferiorH" || tipoPosiicon == "barraSuperiorH" || tipoPosiicon == "barraAmbosH")
                {
                    EditarBarraDTO newEditarBarraDTO = new EditarBarraDTO()
                    {
                        cantidad = 2,
                        diametro = 10,
                        TipoCasobarra = TipoCasobarra.BarraRefuerzoLosa,
                        IsCambiarDiametroYEspacia = false,
                    };

                    switch (tipoPosiicon)
                    {
                        case "barraSinH":
                            newEditarBarraDTO.tipobarraV = TipoPataBarra.BarraVSinPatas;
                            break;
                        case "barraInferiorH":
                            newEditarBarraDTO.tipobarraV = TipoPataBarra.BarraVPataInicial;
                            break;
                        case "barraSuperiorH":
                            newEditarBarraDTO.tipobarraV = TipoPataBarra.BarraVPataFinal;
                            break;
                        case "barraAmbosH":
                            newEditarBarraDTO.tipobarraV = TipoPataBarra.BarraVPataAmbos;
                            break;
                        default:
                            break;
                    }
                    if (ui.DireccionImagen == "Arriba")
                        newEditarBarraDTO.ModificadorDireccionEnfierrado = 1;
                    else
                        newEditarBarraDTO.ModificadorDireccionEnfierrado = -1;

                    try
                    {

                        ManejadorBarraRefuerzo_CambiarBarra ManejadorBarraH_CambiarBarra = new ManejadorBarraRefuerzo_CambiarBarra(_uiapp, newEditarBarraDTO);
                        ManejadorBarraH_CambiarBarra.CambiarFormaBarra();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Ex :" + ex.Message);
                    }
                    //Util.ErrorMsg("En desarrollo");
                }
                else if (tipoPosiicon == "btnAceptar_Cuantia")
                {
                    bool isSoloCAmbiarDatosInternos = false;

                    int CantidadBarras = Util.ConvertirStringInInteger(ui.dtCantidadCuantia.Text);

                    ManejadorModificarTag manejadorRefuerzoTipoBorde = new ManejadorModificarTag(_uiapp, CantidadBarras);
                    if (isSoloCAmbiarDatosInternos)
                    { manejadorRefuerzoTipoBorde.EjecutarCambioSintag(); }
                    else
                    { manejadorRefuerzoTipoBorde.EjecutarCambioContag(); }
                }
                else if (tipoPosiicon == "btnSeleccionarPath")
                {
                    M1_5_MetodoPathEditarTipoMouse(ui, _uiapp);
                }
                else if (tipoPosiicon == "btnAmbosLados2")
                {
                    M1_7_MetodoPathDistanciaBotton(ui, EditarPathReinMouse);
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
            VariablesSistemas.IS_MENSAJE_BUSCAR_ROOM = aux_IS_MENSAJE_BUSCAR_ROOM;
            ui.Show();
            UpdateGeneral.M2_CargarBArras(_uiapp);

            //CargarCambiarPathReinfomenConPto_Wpf
        }




        private static void M1_2_MetodoPathToPto_pto2(Ui_FundEditar ui, EditarPathReinMouse_ExtederPathA2punto _EditarPathReinMouse_ExtederPathApunto2)
        {

            if (!Util.IsNumeric(ui.distanPtoPto.Text))
            {

                return;
            }

            double dist = Util.ConvertirStringInDouble(ui.distanPtoPto.Text);

            TipoCasoAlternativo _tipoCasoAlternativo = new TipoCasoAlternativo()
            {
                distancia_foot = Util.CmToFoot(dist),
                TipoCasoAlternativo_ = TipoCasoAlternativo_enum.Proporcional
            };

            _EditarPathReinMouse_ExtederPathApunto2.M1_ExtederPathApunto(_tipoCasoAlternativo);

        }
        private static void M1_3_MetodoPathDistanciaBotton(Ui_FundEditar ui, EditarPathReinMouse EditarPathReinMouse, string tipoPosiicon)
        {
            if (!Util.IsNumeric(ui.distan.Text) || ui.distan.Text == "0")
            {
                return;
            }

            DireccionEdicionPathRein direccionEdicionPathRein = ObtenerDIreccion(tipoPosiicon);
            double dist = Util.ConvertirStringInDouble(ui.distan.Text);

            EditarPathReinMouse.M3_EjecutarExtenderPath(direccionEdicionPathRein, Util.CmToFoot(dist));


        }

        private static void M1_7_MetodoPathDistanciaBotton(Ui_FundEditar ui, EditarPathReinMouse EditarPathReinMouse)
        {
            if (!Util.IsNumeric(ui.distan.Text) || ui.distan.Text == "-0")
            {
                return;
            }

            double ladoIzq = 200;
            double ladoDere = 150;

            if (ui.largototal.IsChecked == true)
            {
                if (!Util.IsNumeric(ui.largoTotal.Text) || ui.largoTotal.Text == "0") return;
                double dist = Util.ConvertirStringInDouble(ui.largoTotal.Text);
                if (dist <= 0) return;
                ladoIzq = dist / 2;
                ladoDere = dist / 2;
            }
            else
            {
                if (!Util.IsNumeric(ui.largoParcialIzq.Text) || ui.largoParcialIzq.Text == "0") return;
                if (!Util.IsNumeric(ui.largoparcialDere.Text) || ui.largoparcialDere.Text == "0") return;

                ladoIzq = Util.ConvertirStringInDouble(ui.largoParcialIzq.Text);
                ladoDere = Util.ConvertirStringInDouble(ui.largoparcialDere.Text);
                if (ladoIzq <= 0) return;
                if (ladoDere <= 0) return;
            }

            TipoCasoAlternativo _tipoCasoAlternativo = new TipoCasoAlternativo() { TipoCasoAlternativo_ = TipoCasoAlternativo_enum.Proporcional };
            EditarPathReinMouse.M5_EjecutarAmbosLados(Util.CmToFoot(ladoIzq), Util.CmToFoot(ladoDere), _tipoCasoAlternativo);

        }
        private static void M1_4_MetodoPathDistanciaMouse(Ui_FundEditar ui, EditarPathReinMouse EditarPathReinMouse)
        {
            if (!Util.IsNumeric(ui.distan2.Text) || ui.distan2.Text == "0")
            {
                return;
            }

            int contadorPAthSelecionado = 1;
            bool mientras = true;
            while (mientras)
            {
                double dist = Util.ConvertirStringInDouble(ui.distan2.Text);
                mientras = EditarPathReinMouse.M4_ExtederPathDistancia(Util.CmToFoot(dist)) == Result.Succeeded;
                contadorPAthSelecionado += 1;
            }
        }
        private static void M1_5_MetodoPathEditarTipoMouse(Ui_FundEditar ui, UIApplication uiapp)
        {
            if (!M1_5_1_ValidacionCAmbioTipo(ui)) return;
            double dist = Util.ConvertirStringInDouble(ui.distan.Text);
            ui.idElem.Text = "";

            CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto = null;
            CargarActualizarPathReinfoment_Wpf.ExecuteSeleccionador_YActualizar(uiapp,
                                                       UbicacionLosa.Izquierda,
                                                       EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, ui.dtTipo.Text),
                                                      Util.ConvertirStringInDouble(ui.dtDia.Text.Replace("d", "")),
                                                      Util.ConvertirStringInDouble(ui.dtEsp.Text),
                                                      ui.idElem.Text, true);
            if (CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto != null)
                ui._seleccionarPathReinfomentConPto = ReAsignarNuevoPathCreado(ui, uiapp);
        }

        private static void M1_6_MetodoPathActulizarTipoPreseleccionado(Ui_FundEditar ui, UIApplication _uiapp)
        {
            if (!M1_5_1_ValidacionCAmbioTipo(ui)) return;

            double dist = Util.ConvertirStringInDouble(ui.distan.Text);
            ui.idElem.Text = "";


            if (ui._seleccionarPathReinfomentConPto.PathReinforcement.IsValidObject == false)
            {
                Util.ErrorMsg("PathReinforcement no valido. Posiblemente se haya borrado previamente");
            }
            else if (ui._seleccionarPathReinfomentConPto.PathReinforcementSymbol.IsValidObject == false)
            {
                Util.ErrorMsg("PathReinforcementSymbol no valido. Posiblemente se haya borrado previamente");
            }
            else if (ui._seleccionarPathReinfomentConPto != null)
            {




                DatosEditarFundacionesDTO _datosEditarFundacionesDTO = new DatosEditarFundacionesDTO()
                {
                    _seleccionarPathReinfomentConPto = ui._seleccionarPathReinfomentConPto,
                    _PathReinforcement = ui._seleccionarPathReinfomentConPto.PathReinforcement,

                    Diametro_mm = Util.ConvertirStringInInteger(ui.dtDia.Text.Replace("d", "")),
                    _Espaciamiento_foot = Util.CmToFoot(Util.ConvertirStringInDouble(ui.dtEsp.Text)),
                    _IsCambioEspaciamiento = (Util.IsSimilarValor(ui.EspacimientoOriginal, Util.ConvertirStringInDouble(ui.dtEsp.Text), 0.001) ? false : true),
                    _TipoCambioFund = TipoCambioFund.CambiarDatos,
                    _TipoUbicacionFund = TipoCaraUbicacion.Inferior
                };

                if (_datosEditarFundacionesDTO._TipoCambioFund == TipoCambioFund.CambiarDatos)
                {
                    CambiarDatosFund _CambiarDatosFund = new CambiarDatosFund(_uiapp, _datosEditarFundacionesDTO);
                    if (!_CambiarDatosFund.M1_ObtenerNuevoTipoDIametro()) return;

                    using (TransactionGroup transGroup = new TransactionGroup(_uiapp.ActiveUIDocument.Document))
                    {
                        transGroup.Start("Editarfundacion-NH");

                        _CambiarDatosFund.M2_Editar();
                        _CambiarDatosFund.M3_RedefinirPathPorEspaciamiento();

                        transGroup.Assimilate();
                    }
                }
                CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto = null;
            }
            else
            {
                Util.ErrorMsg("No se ha seleccionado ningun PathReinformeSymbol");
            }
            if (CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto != null)
                ui._seleccionarPathReinfomentConPto = ReAsignarNuevoPathCreado(ui, _uiapp);
        }

        private static SeleccionarPathReinfomentConPto ReAsignarNuevoPathCreado(Ui_FundEditar ui, UIApplication uiapp)
        {
            SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPtoNew = new SeleccionarPathReinfomentConPto(uiapp.ActiveUIDocument, uiapp.Application);
            PathReinSpanSymbol aux_PathReinforcementSymbol = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto.PathReinforcementSymbol;
            if (!seleccionarPathReinfomentConPtoNew.AsignarPathReinformentSymbol(aux_PathReinforcementSymbol, true))
            {
                seleccionarPathReinfomentConPtoNew = null;
            }
            else
            {
                string tipoBArra = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._tipobarra.ToString();
                tipoBArra = ObtenerCasoAoBDeAhorro.ConversortoS4(tipoBArra);

                ui.idElem.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto.PathReinforcementSymbol.Id.IntegerValue.ToString();
                ui._TipoDireccionBarra = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._TipoDireccionBarra.ToString();
                ui.dtTipo.Text = tipoBArra;
                ui.dtDia.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._diametro.ToString();
                ui.dtEsp.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._espaciamiento.ToString();

                ui.EspacimientoOriginal = Util.ConvertirStringInDouble(CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._espaciamiento);
                //ui.dtorient.Text = CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto._direccion.ToString();
            }
            return CargarActualizarPathReinfoment_Wpf.SeleccionarPathReinfomentConPto;
        }

        private static bool M1_5_1_ValidacionCAmbioTipo(Ui_FundEditar ui)
        {
            if (ui.dtTipo.Text == "")
            {
                return false;
            }
            if (ui.dtDia.Text == "")
            {
                return false;
            }
            if (!Util.IsNumeric(ui.dtEsp.Text) || ui.dtEsp.Text == "0")
            {
                return false;
            }
            return true;
        }


        private static DireccionEdicionPathRein ObtenerDIreccion(string tipoPosiicon)
        {
            DireccionEdicionPathRein direccionEdicionPathRein = DireccionEdicionPathRein.NONE;
            if (tipoPosiicon == "btnSup")
            {
                direccionEdicionPathRein = DireccionEdicionPathRein.Superior;
            }
            else if (tipoPosiicon == "btnIzq")
            { direccionEdicionPathRein = DireccionEdicionPathRein.Izquierda; }
            else if (tipoPosiicon == "btnDere")
            { direccionEdicionPathRein = DireccionEdicionPathRein.Derecha; }
            else if (tipoPosiicon == "btnInf")
            { direccionEdicionPathRein = DireccionEdicionPathRein.Inferior; }

            return direccionEdicionPathRein;
        }

    }
}