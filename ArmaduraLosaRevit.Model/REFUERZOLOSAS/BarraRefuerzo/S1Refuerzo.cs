using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo
{
    public class S1Refuerzo
    {
        private readonly UIApplication _uiapp;
        private readonly DatosRefuerzoCabezaMuroDTO _datosRefuerzoCabezaMuroDTO;
        private Document _doc;
        private int _diamS1_MM;
        private int _diamtroBarraRefuerzo_MM;
        private double _espacimiamietoS1_foot;
        private readonly string _tipobarra;
        private readonly SeleccionarLosaConMouse _seleccionarLosaConMouse;

        public List<XYZ> ListaPto { get; }

        XYZ PtoInsercionS1;

        List<XYZ> ListaPtosPoligonoLosa;
        private SolicitudBarraDTO _solicitudBarraDTO;
        private SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom;
        private ReferenciaRoomDatos _roomAnalizado;
        private double largoTraslapo;

        public List<XYZ> ListaPtosPerimetroBarras { get; set; }
        public IList<Curve> curvesPathreiforment { get; set; }

        public S1Refuerzo(UIApplication uiapp, List<XYZ> listaPto, DatosRefuerzoCabezaMuroDTO datosRefuerzoCabezaMuroDTO, SeleccionarLosaConMouse seleccionarLosaConMouse)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.ListaPto = listaPto;
            this._datosRefuerzoCabezaMuroDTO = datosRefuerzoCabezaMuroDTO;
            this._diamS1_MM = datosRefuerzoCabezaMuroDTO.diamtroBarraS1_MM;
            this._diamtroBarraRefuerzo_MM = datosRefuerzoCabezaMuroDTO.diamtroBarraRefuerzo_MM;
            this._espacimiamietoS1_foot = Util.CmToFoot(datosRefuerzoCabezaMuroDTO.espacimientoS1_Cm);
            this._tipobarra = datosRefuerzoCabezaMuroDTO.tipobarra;
            this._seleccionarLosaConMouse = seleccionarLosaConMouse;
            ListaPtosPoligonoLosa = new List<XYZ>();
        }

        public bool Obtener4PtosPath()
        {
            try
            {
                largoTraslapo = (_datosRefuerzoCabezaMuroDTO.IsUsar2Pto ? 0 : UtilBarras.largo_L9_DesarrolloFoot_diamMM(_diamtroBarraRefuerzo_MM));

                //falta definir  la direccion de seleccionan para asignar correctamente los largo en *** (linea 103)
                double largoDesarrollo_dentroMuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM_cabeza_dentroMuro(_diamtroBarraRefuerzo_MM);
                double largoDesarrollo_fueramuro = UtilBarras.largo_L9_DesarrolloFoot_diamMM_cabeza_fueramuro(_diamtroBarraRefuerzo_MM);

                XYZ pointSobreLosa1cm = _seleccionarLosaConMouse._ptoSeleccionEnLosa + new XYZ(0, 0, 1);
                Room RoomSelecionado1 = _doc.GetRoomAtPoint(pointSobreLosa1cm);
                if (RoomSelecionado1 != null)
                {
                    try
                    {
                        ReferenciaRoomDatos _ReferenciaRoomDatos = new ReferenciaRoomDatos(_doc, RoomSelecionado1);
                        _ReferenciaRoomDatos.GetParametrosUnRoom();
                        if (_datosRefuerzoCabezaMuroDTO.IsUsar2Pto)
                            largoTraslapo = 0;
                        else
                            largoTraslapo = _ReferenciaRoomDatos.largomin_1 * 0.25;
                    }
                    catch (System.Exception)
                    {
                        Util.ErrorMsg("Error al obtener parametros de room. Se utiliza dos veces largo de desarrollo mas espesor como largo de suple");
                        return false;
                    }
                }
                else
                {
                    //if (_datosRefuerzoCabezaMuroDTO.IsUsar2Pto)
                    // Util.ErrorMsg("No se encontro Room.Se cancela dibujo de suple");
                    largoTraslapo = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_diamS1_MM);
                    if (_datosRefuerzoCabezaMuroDTO._tipoRefuerzoLOSA == TipoRefuerzoLOSA.losa)
                        Util.InfoMsg("No se encontro Room. Se utiliza dos veces largo de desarrollo mas espesor como largo de suple");
                }

                ///  if (datosRefuerzoCabezaMuroDTO.IsUsar2Pto) largoTraslapo = 0;
                XYZ p1 = ListaPto[0];
                XYZ p2 = ListaPto[1];
                XYZ p3 = ListaPto[2];
                XYZ p4 = ListaPto[3];

                PtoInsercionS1 = (p1 + p2 + p3 + p4) / 4;
                //***
                XYZ dire1 = (p1 - p4).Normalize();
                p1 = p1 + dire1 * largoDesarrollo_dentroMuro;
                p4 = p4 + -dire1 * largoDesarrollo_fueramuro;

                XYZ dire2 = (p2 - p3).Normalize();
                p2 = p2 + dire2 * largoDesarrollo_dentroMuro;
                p3 = p3 + -dire2 * largoDesarrollo_fueramuro;

                ListaPto.Clear();
                ListaPto.Add(p1);
                ListaPto.Add(p2);
                ListaPto.Add(p3);
                ListaPto.Add(p4);


                ListaPtosPoligonoLosa.Add(p1);
                ListaPtosPoligonoLosa.Add(p4);
                ListaPtosPoligonoLosa.Add(p3);
                ListaPtosPoligonoLosa.Add(p2);

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;

        }

        public bool Obtener4PtosPath2ptos()
        {
            try
            {


                ///  if (datosRefuerzoCabezaMuroDTO.IsUsar2Pto) largoTraslapo = 0;
                XYZ p1 = ListaPto[0];
                XYZ p2 = ListaPto[1];
                XYZ p3 = ListaPto[2];
                XYZ p4 = ListaPto[3];

                PtoInsercionS1 = (p1 + p2 + p3 + p4) / 4;
                //***
                //XYZ dire1 = (p1 - p4).Normalize();
                //p1 = p1 + dire1 * largoDesarrollo_dentroMuro;
                //p4 = p4 + -dire1 * largoDesarrollo_fueramuro;

                //XYZ dire2 = (p2 - p3).Normalize();
                //p2 = p2 + dire2 * largoDesarrollo_dentroMuro;
                //p3 = p3 + -dire2 * largoDesarrollo_fueramuro;

                ListaPto.Clear();
                ListaPto.Add(p1);
                ListaPto.Add(p2);
                ListaPto.Add(p3);
                ListaPto.Add(p4);


                ListaPtosPoligonoLosa.Add(p1);
                ListaPtosPoligonoLosa.Add(p4);
                ListaPtosPoligonoLosa.Add(p3);
                ListaPtosPoligonoLosa.Add(p2);

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;

        }


        public bool Ejecutar()
        {
            if (!Obtener4PtosPath()) return false;
            if (!ObtenerDatosNEcesarios()) return false;
            if (!ObtenerPoligonos()) return false;
            if (!dibujarBarras()) return false;

            return true;
        }
        public bool Ejecutar2ptos()
        {
            if (!Obtener4PtosPath2ptos()) return false;
            if (!ObtenerDatosNEcesarios()) return false;
            if (!ObtenerPoligonos()) return false;
            if (!dibujarBarras()) return false;

            return true;
        }
        public bool ObtenerDatosNEcesarios()
        {
            try
            {
                _solicitudBarraDTO = new SolicitudBarraDTO(_uiapp, _tipobarra, UbicacionLosa.Izquierda, TipoConfiguracionBarra.suple, false);

                _seleccionarLosaBarraRoom = new SeleccionarLosaBarraRoom(_uiapp.ActiveUIDocument, _solicitudBarraDTO);
                _seleccionarLosaBarraRoom.AsignarUnRoom(_seleccionarLosaConMouse._ptoSeleccionEnLosa, _seleccionarLosaConMouse.LosaSelecionado);
                _seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado = Util.angulo_entre_pt_Rad_XY0(ListaPto[0], ListaPto[1]);

                if (_seleccionarLosaBarraRoom.ObtenerUNRoom())
                {
                    _roomAnalizado = new ReferenciaRoomDatos(_seleccionarLosaBarraRoom, _solicitudBarraDTO);
                    _roomAnalizado.GetParametrosUnRoom();
                }
                else
                {
                    _roomAnalizado = new ReferenciaRoomDatos(_seleccionarLosaBarraRoom, _solicitudBarraDTO);
                    _roomAnalizado.espesorCM_1 = (float)_seleccionarLosaBarraRoom.LosaSeleccionada1.ObtenerEspesorConPtosFloor(_seleccionarLosaConMouse._ptoSeleccionEnLosa);

                    _roomAnalizado.direccionHorizontal = "2";
                    _roomAnalizado.direccionVertical = "1";
                    _roomAnalizado.largomin_1 = largoTraslapo / 0.15;
                    _roomAnalizado.largomin_2 = largoTraslapo / 0.15;
                    _roomAnalizado.anguloBarraLosaGrado_1 = 0;
                }
            }
            catch (System.Exception)
            {

                return false;
            }
            return true;
        }

        public bool ObtenerPoligonos()
        {
            try
            {
                var result = BarraRoomGeometria.ListaFinal_ptov2(ListaPtosPoligonoLosa, _uiapp, _seleccionarLosaBarraRoom, _solicitudBarraDTO, _roomAnalizado, false);
                if (result == null) return false;
                ListaPtosPerimetroBarras = result.Item1;
                //ListaPtosPerimetroBarras.AddRange(ListaPtosPoligonoLosa);
                //GraficarPtos DibujoPtosTrasladados = new GraficarPtos(ListaPtosPerimetroBarras);
                //DibujoPtosTrasladados.ShowDialog();
                curvesPathreiforment = result.Item2;
            }
            catch (System.Exception)
            {
                return false; ;
            }
            return true;
        }

        public bool dibujarBarras()
        {

            try
            {
                BarraRoomDTO barraRoomDTO = new BarraRoomDTO()
                {
                    diametroEnMM = _diamS1_MM,
                    LosaSeleccionada1 = _seleccionarLosaConMouse.LosaSelecionado,
                    EspesorLosaCm_1 = _roomAnalizado.espesorCM_1,
                    Espaciamiento = _espacimiamietoS1_foot,
                    TipoBarraStr = _tipobarra,
                    ubicacionEnlosa = UbicacionLosa.Izquierda,
                    AnguloBordeRoomYSegundoPtoMouseGrado = _seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado,
                    _TipoRebar = TipoRebar.REFUERZO_SUPLE_CAB_MU,
                    Prefijo_cuantia = "F'="
                };

                BarraRoom newBarralosa_izq = new BarraRoom(_uiapp, barraRoomDTO,
                                                          ListaPtosPerimetroBarras, curvesPathreiforment, PtoInsercionS1);
                bool auxRedondeo = DatosDiseño.IS_PATHREIN_AJUSTADO;
                DatosDiseño.IS_PATHREIN_AJUSTADO = false;
                if (newBarralosa_izq.statusbarra == Result.Succeeded)
                {
                    newBarralosa_izq.BUscarViewSUPERIOR();
                    if (newBarralosa_izq.CrearBarra(newBarralosa_izq.CurvesPathreiforment,
                                                newBarralosa_izq.LargoPathreiforment,
                                                newBarralosa_izq.nombreSimboloPathReinforcement,
                                                newBarralosa_izq.diametroEnMM,
                                                newBarralosa_izq.Espaciamiento,
                                                PtoInsercionS1) != Result.Succeeded) return false;
                }
                DatosDiseño.IS_PATHREIN_AJUSTADO = auxRedondeo;
            }
            catch (System.Exception)
            {
                return false; ;
            }
            return true;
        }
    }
}
