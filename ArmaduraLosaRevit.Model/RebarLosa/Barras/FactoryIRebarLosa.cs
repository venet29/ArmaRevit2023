using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Fund.TipoBarraFund;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Tipo;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras
{
    public class FactoryIRebarLosa
    {

        public static IRebarLosa CrearIRebarLosa(UIApplication _uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag _newIGeometriaTag)
        {


            switch (_rebarInferiorDTO.tipoBarra)
            {
                case TipoBarra.f1:
                    return new fx_null();
                case TipoBarra.f1_incliInf:
                    return new f1_incliInf(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                    
                case TipoBarra.f1_esc135_sinpata:
                    return new f1_esc135_sinpata(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f1_esc45_conpata:
                    return new f1_esc45_conpata(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f1_b:
                    return new f1_ab(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f1_a:
                    return new f1_ab(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f1_ab:
                    return new f1_ab(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);

                case TipoBarra.f3_ab:
                case TipoBarra.f3_ba:
                case TipoBarra.f3_a0:
                case TipoBarra.f3_b0:
                case TipoBarra.f3_0a:
                case TipoBarra.f3_0b:
                    return new fx_null();
                case TipoBarra.f3_incli:
                    return new f3_incli(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                    //escalera********
                    
                case TipoBarra.f3_incli_esc:
                    return new f3_incli_esc(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f3_esc45:
                    return new f3_esc45(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f3_esc135:
                    return new f3_esc135(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);

                //************************************************************
                case TipoBarra.f3:
                    return new fx_null();
                case TipoBarra.f4:
                    return new f4_incliInf(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f4_incli:
                    return new f4_incliInf(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f4_incli_esc:
                    return new f4_incli_esc(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f7:
                    return new fx_null();
                case TipoBarra.f10_fund:
                    return new f10_fund(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f10A_fund:
                    return new f10A_fund(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f10B_fund:
                    return new f10B_fund(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f10:
                    return new f10(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f11:
                    return new f11(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f11_fund:
                    return new f11_fund(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f11A_fund:
                    return new f11A_fund(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f11B_fund:
                    return new f11B_fund(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                case TipoBarra.f9:
                    return new fx_null();
                case TipoBarra.f16:
                    return new fx_null();
                case TipoBarra.f16_Izq:
                    return new fx_null();
                case TipoBarra.f16_Dere:
                    return new fx_null();
                case TipoBarra.f17:
                    return new fx_null();
                case TipoBarra.f17A_Tras:
                    return new fx_null();
                case TipoBarra.f17B_Tras:
                    return new fx_null();
                case TipoBarra.f18:
                    return new fx_null();
                case TipoBarra.f19:
                    return new fx_null();
                case TipoBarra.f19_Izq:
                    return new fx_null();
                case TipoBarra.f19_Dere:
                    return new fx_null();
                case TipoBarra.f20:
                    return new fx_null();
                case TipoBarra.f20A_Izq_Tras:
                case TipoBarra.f20B_Dere_Tras:
                    return new fx_null();
                case TipoBarra.f20A_Dere_Tras:
                case TipoBarra.f20B_Izq_Tras:
                    return new fx_null();
                case TipoBarra.f21:
                    return new fx_null();
                case TipoBarra.f21A_Izq_Tras:
                    return new fx_null();
                case TipoBarra.f21A_Dere_Tras:
                    return new fx_null();
                case TipoBarra.f21B_Izq_Tras:
                    return new fx_null();
                case TipoBarra.f21B_Dere_Tras:
                    return new fx_null();

                case TipoBarra.f1_SUP:
                    return new fx_null();

                case TipoBarra.s1:
                    return new fx_null();
                case TipoBarra.s2:
                    return new fx_null();
                case TipoBarra.s3:
                    return new fx_null();
                case TipoBarra.s4_Inclinada:
                    return new s4_incli(_uiapp, _rebarInferiorDTO, _newIGeometriaTag);
                default:
                    return new fx_null();
            }

        }



    }
}
