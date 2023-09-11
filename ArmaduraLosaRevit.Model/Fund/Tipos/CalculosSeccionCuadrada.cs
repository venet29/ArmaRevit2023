using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using auxEnun= ArmaduraLosaRevit.Model.Enumeraciones;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.Tipos
{
    public class CalculosSeccionCuadrada
    {
        private readonly UIApplication _uiapp;
        private View _view;

        public List<XYZ> ListaPtosPerimetroBarras { get; internal set; }
        public List<XYZ> ListaPtosPerimetroBarrasParaDimensiones { get; private set; }

#pragma warning disable CS0649 // Field 'CalculosSeccionCuadrada.espesorCM_1' is never assigned to, and will always have its default value 0
        private double espesorCM_1;
#pragma warning restore CS0649 // Field 'CalculosSeccionCuadrada.espesorCM_1' is never assigned to, and will always have its default value 0
#pragma warning disable CS0649 // Field 'CalculosSeccionCuadrada.diametro' is never assigned to, and will always have its default value 0
        private int diametro;
#pragma warning restore CS0649 // Field 'CalculosSeccionCuadrada.diametro' is never assigned to, and will always have its default value 0

        public XYZ PtoConMouseEnlosa1;
        UbicacionLosa ubicacionEnlosa;
        private double Espaciamiento;
        private auxEnun.TipoBarra TipoBarraStr;
        private Element LosaSeleccionada1;
        private bool IsLuzSecuandiria;
        private TipoDireccionBarra TipoDireccionBarra_;

        public CalculosSeccionCuadrada(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this.ListaPtosPerimetroBarras = new List<XYZ>();
            this.ListaPtosPerimetroBarrasParaDimensiones = new List<XYZ>();
        }



        internal Result Load_fx()
        {
            try
            {
                SeleccionarFundConMouse seleccionarFundConMouse = new SeleccionarFundConMouse(_uiapp);

                if (!seleccionarFundConMouse.M1_Selecconafund(new FiltroFloorOrFund())) return Result.Cancelled;
                var  _datosPLanarface = seleccionarFundConMouse.M1_SeleccionarCaraInferiorFund();
                if (!_datosPLanarface.IsOK) return Result.Succeeded;   //ShaftIndividualNULL

                //ListaPtosPerimetroBarras
                //ListaPtosPerimetroBarrasParaDimensiones

                PtoConMouseEnlosa1 = XYZ.Zero;
                ubicacionEnlosa = UbicacionLosa.NONE;
                Espaciamiento = Util.CmToFoot(20);
                TipoBarraStr = auxEnun.TipoBarra.f11;
                LosaSeleccionada1 = null;
                IsLuzSecuandiria = true;
                TipoDireccionBarra_ = TipoDireccionBarra.Primaria;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener la forma cuadrada de fundacion. ex{ex.Message}");
                return Result.Failed;
            }

            return Result.Succeeded;

        }

        internal RebarInferiorDTO ObtenerGEometria()
        {
            RebarInferiorDTO _RebarInferiorDTO = new RebarInferiorDTO(_uiapp);


            List<XYZ> ListaPtosPerimetroBarras_ = ListaPtosPerimetroBarras;
            int barraMenos =0;
            bool usarPoligonoOriginal = true;
            RebarInferiorDTO rebarInferiorDTO = new RebarInferiorDTO(_uiapp);
            try
            {            
                rebarInferiorDTO.listaPtosPerimetroBarras.AddRange(ListaPtosPerimetroBarras_);

                var resultZ = _view.Obtener_Z_SoloPLantas();
                if (!resultZ.Isok)
                {
                    rebarInferiorDTO.IsOK = false;
                    return rebarInferiorDTO;
                }

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
                    List<XYZ> ListaPtosPerimetroBarrasOriginal = ListaPtosPerimetroBarrasParaDimensiones;
                    rebarInferiorDTO.PtoDirectriz1 = Line.CreateBound(ListaPtosPerimetroBarrasOriginal[1].GetXY0(), ListaPtosPerimetroBarrasOriginal[2].GetXY0())
                                                         .Project(PtoConMouseEnlosa1).XYZPoint.AsignarZ(resultZ.valorz);
                    rebarInferiorDTO.PtoDirectriz2 = Line.CreateBound(ListaPtosPerimetroBarrasOriginal[0].GetXY0(), ListaPtosPerimetroBarrasOriginal[3].GetXY0())
                                                         .Project(PtoConMouseEnlosa1.GetXY0()).XYZPoint.AsignarZ(resultZ.valorz);
                }
                rebarInferiorDTO.espesorLosaFoot = Util.CmToFoot(espesorCM_1);// Util.CmToFoot(15)

                if (rebarInferiorDTO.espesorLosaFoot <= 0)
                {
                    Util.ErrorMsg($"Error en el espesor de Room  e:{rebarInferiorDTO.espesorLosaFoot }");
                    rebarInferiorDTO.IsOK = false;
                }

                //rebarInferiorDTO.numeroBarra = 20;
                rebarInferiorDTO.diametroMM = diametro;
                rebarInferiorDTO.tipoBarra = TipoBarraStr;
                rebarInferiorDTO.ubicacionLosa = ubicacionEnlosa;
                rebarInferiorDTO.espaciamientoFoot = Espaciamiento;// Util.CmToFoot(15);
                rebarInferiorDTO.largo_recorridoFoot = ListaPtosPerimetroBarras_[0].DistanceTo(ListaPtosPerimetroBarras_[1]);// Util.CmToFoot(660);
                rebarInferiorDTO.cantidadBarras = (int)(rebarInferiorDTO.largo_recorridoFoot / rebarInferiorDTO.espaciamientoFoot) + 1 - barraMenos;

                rebarInferiorDTO.floor = LosaSeleccionada1;//  _uiapp.ActiveUIDocument.Document.GetElement(new ElementId(1111503));

                // rebarInferiorDTO.largomin_1 = _refereciaRoomDatos.largomin_1;

                rebarInferiorDTO.ptoSeleccionMouse = PtoConMouseEnlosa1.AsignarZ(resultZ.valorz);

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
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener la forma cuadrada de fundacion. ex{ex.Message}");
                rebarInferiorDTO.IsOK = false;
                return rebarInferiorDTO;
            }

            return _RebarInferiorDTO;
        }
    }
}
