using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.CopiaLocal.WPF;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Fund.Entidad;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.RebarFundaciones.Geom
{



    public class GenerarGeometriaFund
    {
        private UIApplication _uiapp;
        private View _view;
        public List<XYZ> ListaPtosPerimetroBarras { get; set; }

        private XYZ PtoConMouseEnlosa1;
        private int factorEspaciamiento;

        //  private SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom;

        bool IsLuzSecuandiria;
        TipoDireccionBarra TipoDireccionBarra_;
        UbicacionLosa ubicacionEnlosa;
        string TipoBarraStr;

        private FundIndividual fundIndividuas;
        private SeleccionarFundConMouse seleccionarFundConMouse;

        double Espaciamiento_foot;
        int diametro_mm;

        public GenerarGeometriaFund(UIApplication uiapp, FundIndividual fundIndividuas, SeleccionarFundConMouse seleccionarFundConMouse)
        {
            this._uiapp = uiapp;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this.fundIndividuas = fundIndividuas;
            this.seleccionarFundConMouse = seleccionarFundConMouse;
        }

        public bool Ejecutar(UbicacionPtoMouse _UbicacionPtoMouse, DatosNuevaBarraDTO _datosNuevaBarraDTO, UbicacionLosa _UbicacionLosa)
        {
            try
            {
                Espaciamiento_foot = _datosNuevaBarraDTO.EspaciamientoFoot;// Util.CmToFoot(20);
                diametro_mm = (int)_datosNuevaBarraDTO.DiametroMM;
                string _TipoBarraStr = _datosNuevaBarraDTO.TipoBarra;

                if (fundIndividuas == null) return false;
                // _seleccionarLosaBarraRoom = seleccionarFundConMouse._elementSelecciondo;

                IsLuzSecuandiria = false;
                TipoDireccionBarra_ = TipoDireccionBarra.Primaria;
                ubicacionEnlosa = _UbicacionLosa;// UbicacionLosa.Izquierda;
                TipoBarraStr = _TipoBarraStr;
                ListaPtosPerimetroBarras = fundIndividuas.ListaVertices;

                //PtoConMouseEnlosa1 = seleccionarFundConMouse._caraInferior.ObtenerCenterDeCara()
                //                            .AsignarZ( seleccionarFundConMouse.PtoMOuse_sobreFundacion.Z);
                PtoConMouseEnlosa1 = _datosNuevaBarraDTO.PtoMouse;

                //this.factorEspaciamiento = factorEspaciamiento;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener datos en 'GenerarGeometriaAhorro'  ex:{ex.Message}");
                return false;
            }
            return true;

        }



        public RebarInferiorDTO ObtenerGEometria(ObtenerGEometriaDTO _ObtenerGEometriaDTO)
        {

            List<XYZ> ListaPtosPerimetroBarras_ = _ObtenerGEometriaDTO.ListaPtosPerimetroBarras;
            int barraMenos = _ObtenerGEometriaDTO.barraMenos;
            bool usarPoligonoOriginal = _ObtenerGEometriaDTO.usarPoligonoOriginal;


            RebarInferiorDTO rebarInferiorDTO = new RebarInferiorDTO(_uiapp);
            try
            {
                var resultZ = _view.Obtener_Z_SoloPLantas();
                if (!resultZ.Isok)
                {
                    rebarInferiorDTO.IsOK = false;
                    return rebarInferiorDTO;
                }

                rebarInferiorDTO.listaPtosPerimetroBarras.AddRange(ListaPtosPerimetroBarras_);

                rebarInferiorDTO.barraIni = Line.CreateBound(ListaPtosPerimetroBarras_[0].GetXY0(), ListaPtosPerimetroBarras_[1].GetXY0())
                                                .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);
                //Tb sirve
                XYZ ptoIniINter = Line.CreateBound(ListaPtosPerimetroBarras_[0], ListaPtosPerimetroBarras_[1]).ProjectSINExtendida3D(PtoConMouseEnlosa1);

                rebarInferiorDTO.barraFin = Line.CreateBound(ListaPtosPerimetroBarras_[3].GetXY0(), ListaPtosPerimetroBarras_[2].GetXY0())
                                                .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);
                //Tb sirve
                XYZ ptoFin = Line.CreateBound(ListaPtosPerimetroBarras_[3], ListaPtosPerimetroBarras_[2]).ProjectSINExtendida3D(PtoConMouseEnlosa1);

                if (usarPoligonoOriginal == false)
                {
                    rebarInferiorDTO.PtoDirectriz1 = Line.CreateBound(ListaPtosPerimetroBarras_[1].GetXY0(), ListaPtosPerimetroBarras_[2].GetXY0())
                                                         .Project(PtoConMouseEnlosa1).XYZPoint.AsignarZ(resultZ.valorz);
                    rebarInferiorDTO.PtoDirectriz2 = Line.CreateBound(ListaPtosPerimetroBarras_[0].GetXY0(), ListaPtosPerimetroBarras_[3].GetXY0())
                                                         .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);
                }
                else
                {
                    List<XYZ> ListaPtosPerimetroBarrasOriginal = _ObtenerGEometriaDTO.ListaPtosPerimetroBarrasParaDimensiones;
                    rebarInferiorDTO.PtoDirectriz1 = Line.CreateBound(ListaPtosPerimetroBarrasOriginal[1].GetXY0(), ListaPtosPerimetroBarrasOriginal[2].GetXY0())
                                                         .Project(PtoConMouseEnlosa1).XYZPoint.AsignarZ(resultZ.valorz);
                    rebarInferiorDTO.PtoDirectriz2 = Line.CreateBound(ListaPtosPerimetroBarrasOriginal[0].GetXY0(), ListaPtosPerimetroBarrasOriginal[3].GetXY0())
                                                         .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);
                }

                double espesor_foot = seleccionarFundConMouse._elementSelecciondo.ObtenerEspesorConPtos_foot(seleccionarFundConMouse.PtoMOuse_sobreFundacion, -XYZ.BasisZ);
                rebarInferiorDTO.espesorLosaFoot = espesor_foot;// Util.CmToFoot(15)

                if (rebarInferiorDTO.espesorLosaFoot <= 0)
                {
                    Util.ErrorMsg($"Error en el espesor de Room  e:{rebarInferiorDTO.espesorLosaFoot}");
                    rebarInferiorDTO.IsOK = false;
                    return null;
                }

                //rebarInferiorDTO.numeroBarra = 20;
                rebarInferiorDTO.diametroMM = diametro_mm;
                rebarInferiorDTO.tipoBarra = EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, TipoBarraStr);
                rebarInferiorDTO.ubicacionLosa = ubicacionEnlosa;
                rebarInferiorDTO.espaciamientoFoot = Espaciamiento_foot;// * factorEspaciamiento;// Util.CmToFoot(15);
                rebarInferiorDTO.largo_recorridoFoot = ListaPtosPerimetroBarras_[0].DistanceTo(ListaPtosPerimetroBarras_[1]);// Util.CmToFoot(660);
                rebarInferiorDTO.cantidadBarras = (int)(rebarInferiorDTO.largo_recorridoFoot / rebarInferiorDTO.espaciamientoFoot) + 1 - barraMenos;

                rebarInferiorDTO.floor = seleccionarFundConMouse._elementSelecciondo;//  _uiapp.ActiveUIDocument.Document.GetElement(new ElementId(1111503));
                rebarInferiorDTO.planarfaceAnalizada = (_ObtenerGEometriaDTO.TipoUbicacionFund == TipoCaraObjeto.Inferior
                                                            ? seleccionarFundConMouse._caraInferior
                                                            : seleccionarFundConMouse._caraSuperior);
                rebarInferiorDTO.TipoUbicacionFund = _ObtenerGEometriaDTO.TipoUbicacionFund;
                rebarInferiorDTO.largomin_1 = Util.CmToFoot(100);

                rebarInferiorDTO.ptoSeleccionMouse = PtoConMouseEnlosa1.AsignarZ(_view.GenLevel.ProjectElevation);

                int AcortamientoEspesorSecundario = (IsLuzSecuandiria == true ? 1 : 0);
                rebarInferiorDTO.AcortamientoEspesorSecundario = AcortamientoEspesorSecundario;
                rebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT = rebarInferiorDTO.espesorLosaFoot + Util.CmToFoot(-ConstNH.RECUBRIMIENTO_LOSA_SUP_cm - ConstNH.RECUBRIMIENTO_LOSA_INF_cm - AcortamientoEspesorSecundario);

                rebarInferiorDTO.anguloBarraGrados = Util.AnguloEntre2PtosGrados_enPlanoXY(rebarInferiorDTO.barraIni, rebarInferiorDTO.barraFin);
                rebarInferiorDTO.anguloBarraRad = Util.GradosToRadianes(rebarInferiorDTO.anguloBarraGrados);
                rebarInferiorDTO.TipoDireccionBarra_ = TipoDireccionBarra_;
                rebarInferiorDTO.anguloTramoRad = Util.GradosToRadianes(31.5);
                rebarInferiorDTO.LargoPata = Util.CmToFoot(100);
                rebarInferiorDTO.IsOK = true;
            }
            catch (Exception)
            {
                return null;
            }

            return rebarInferiorDTO;
        }


    }
}
