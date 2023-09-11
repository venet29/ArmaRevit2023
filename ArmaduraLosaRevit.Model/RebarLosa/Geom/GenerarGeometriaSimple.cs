using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.RebarLosa.Servicio
{
    public enum UbicacionPtoMouse
    {
        superior,
        centro,
        inferior
    }

 

    public class GenerarGeometriaSimple
    {
        private UIApplication _uiapp;
        private BarraRoom _barraRoom;
        private View _view;


        public List<XYZ> ListaPtosPerimetroBarras { get; set; }

        private XYZ PtoConMouseEnlosa1;
        private int factorEspaciamiento;

        //  public List<XYZ> ListaPtosPerimetroBarras1 { get; set; }
        //  public List<XYZ> ListaPtosPerimetroBarras2 { get; set; }

        private SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom;
        private ReferenciaRoomDatos _refereciaRoomDatos;
        bool IsLuzSecuandiria;
        TipoDireccionBarra TipoDireccionBarra_;
        UbicacionLosa ubicacionEnlosa;
        string TipoBarraStr;

        public GenerarGeometriaSimple(UIApplication _uiapp, BarraRoom barraRoom)
        {
            this._uiapp = _uiapp;
            this._barraRoom = barraRoom;
            this._view = _uiapp.ActiveUIDocument.ActiveView;
        }

        public bool Ejecutar(int factorEspaciamiento, UbicacionPtoMouse _UbicacionPtoMouse)
        {
            try
            {
                GenerarGeometriaAhorroDTO _GenerarGeometriaAhorroDTO = _barraRoom.ObtenerGenerarGeometriaAhorroDTO();

                if (_GenerarGeometriaAhorroDTO == null) return false;
                _seleccionarLosaBarraRoom = _GenerarGeometriaAhorroDTO._seleccionarLosaBarraRoom;
                _refereciaRoomDatos = _GenerarGeometriaAhorroDTO._refereciaRoomDatos;
                IsLuzSecuandiria = _GenerarGeometriaAhorroDTO.IsLuzSecuandiria;
                TipoDireccionBarra_ = _GenerarGeometriaAhorroDTO.TipoDireccionBarra_;
                ubicacionEnlosa = _GenerarGeometriaAhorroDTO.ubicacionEnlosa;
                TipoBarraStr = _GenerarGeometriaAhorroDTO.TipoBarraStr;
                ListaPtosPerimetroBarras = _GenerarGeometriaAhorroDTO.ListaPtosPerimetroBarras;

                PtoConMouseEnlosa1 = _seleccionarLosaBarraRoom.PtoConMouseEnlosa1;


                XYZ direc_p1_p1 = (ListaPtosPerimetroBarras[0] - ListaPtosPerimetroBarras[1]).Normalize();

                if (_UbicacionPtoMouse == UbicacionPtoMouse.superior)
                    PtoConMouseEnlosa1 = PtoConMouseEnlosa1 + direc_p1_p1 * _refereciaRoomDatos.Espaciamiento;
                else if (_UbicacionPtoMouse == UbicacionPtoMouse.inferior)
                    PtoConMouseEnlosa1 = PtoConMouseEnlosa1 - direc_p1_p1 * _refereciaRoomDatos.Espaciamiento * 2;

                this.factorEspaciamiento = factorEspaciamiento;
                
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
                rebarInferiorDTO.espesorLosaFoot = Util.CmToFoot(_refereciaRoomDatos.espesorCM_1);// Util.CmToFoot(15)

                if (rebarInferiorDTO.espesorLosaFoot <= 0)
                {
                    Util.ErrorMsg($"Error en el espesor de Room  e:{rebarInferiorDTO.espesorLosaFoot }");
                    rebarInferiorDTO.IsOK = false;
                }

                //rebarInferiorDTO.numeroBarra = 20;
                rebarInferiorDTO.diametroMM = _refereciaRoomDatos.diametro;
                rebarInferiorDTO.tipoBarra = EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, TipoBarraStr);
                rebarInferiorDTO.ubicacionLosa = ubicacionEnlosa;
                rebarInferiorDTO.espaciamientoFoot = _refereciaRoomDatos.Espaciamiento*2 * factorEspaciamiento;// Util.CmToFoot(15);
                rebarInferiorDTO.largo_recorridoFoot = ListaPtosPerimetroBarras_[0].DistanceTo(ListaPtosPerimetroBarras_[1]);// Util.CmToFoot(660);
                rebarInferiorDTO.cantidadBarras = (int)(rebarInferiorDTO.largo_recorridoFoot / rebarInferiorDTO.espaciamientoFoot) + 1 - barraMenos;

                rebarInferiorDTO.floor = _seleccionarLosaBarraRoom.LosaSeleccionada1;//  _uiapp.ActiveUIDocument.Document.GetElement(new ElementId(1111503));

                rebarInferiorDTO.largomin_1 = _refereciaRoomDatos.largomin_1;

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
