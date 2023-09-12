using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.TipoTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    public class AyudaAsignarTipoBarra
    {
        public TipoRebar _tipoRebar { get; set; }
        public AyudaAsignarTipoBarra()
        {

        }
        public bool AsignarBarrasInferiore(TipoBarra tipoBarra)
        {
            try
            {   
                switch (tipoBarra)
                {
                    case TipoBarra.f11:
                    case TipoBarra.f9:
                    case TipoBarra.f7:
                    case TipoBarra.f3:
                    case TipoBarra.f4:
                    case TipoBarra.f1:
                    case TipoBarra.f16:
                    case TipoBarra.f16_Izq:
                    case TipoBarra.f16_Dere:
                    case TipoBarra.f17:
                    case TipoBarra.f17A_Tras:
                    case TipoBarra.f17B_Tras:
                    case TipoBarra.f18:
                    case TipoBarra.f19:
                    case TipoBarra.f19_Izq:
                    case TipoBarra.f19_Dere:
                    case TipoBarra.f20:
                    case TipoBarra.f20A_Izq_Tras:
                    case TipoBarra.f20B_Dere_Tras:
                    case TipoBarra.f20A_Dere_Tras:
                    case TipoBarra.f20B_Izq_Tras:
                    case TipoBarra.f21:
                    case TipoBarra.f21A_Izq_Tras:
                    case TipoBarra.f21A_Dere_Tras:
                    case TipoBarra.f21B_Izq_Tras:
                    case TipoBarra.f21B_Dere_Tras:
                        _tipoRebar = TipoRebar.LOSA_INF;
                        break;
                    case TipoBarra.f1_SUP:
                        _tipoRebar = TipoRebar.LOSA_SUP;
                        break;
                    case TipoBarra.s1:
                        _tipoRebar = TipoRebar.LOSA_SUP_S1;
                        break;
                    case TipoBarra.s2:
                        _tipoRebar = TipoRebar.LOSA_SUP_S2;
                        break;
                    case TipoBarra.s3:
                        _tipoRebar = TipoRebar.LOSA_SUP_S3;
                        break;
                    case TipoBarra.s4:
                        _tipoRebar = TipoRebar.LOSA_SUP_S4;
                        break;
                    case TipoBarra.f3_esc45:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_45;
                        break;
                    case TipoBarra.f3_esc135:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_135;
                        break;
                    case TipoBarra.f3b_esc45:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3B_45;
                        break;
                    case TipoBarra.f3b_esc135:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3B_135;
                        break;                   
                    case TipoBarra.f1_esc45:
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_45;
                        break;
                    case TipoBarra.f1_esc45_conpata:
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_45_CONPATA;
                        break;
                    case TipoBarra.f1_esc135_sinpata:
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_135_SINPATA;
                        break;
                    case TipoBarra.f1_b:
                        _tipoRebar = TipoRebar.LOSA_ESC_F1B;
                        break;
                    case TipoBarra.f1_a:
                        _tipoRebar = TipoRebar.LOSA_ESC_F1A;
                        break;
                    case TipoBarra.f1_ab:
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_AB;
                        break;
                    case TipoBarra.f3_ab:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_AB;
                        break;
                    case TipoBarra.f3_ba:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_BA;
                        break;
                    case TipoBarra.f3_a0:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_A0;
                        break;
                    case TipoBarra.f3_b0:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_B0;
                        break;
                    case TipoBarra.f3_0a:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_0A;
                        break;
                    case TipoBarra.f3_0b:
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_0B;
                        break;
                    case TipoBarra.f1_incli:
                        _tipoRebar = TipoRebar.LOSA_INCLI_F1;
                        break;
                    case TipoBarra.f3_incli:
                        _tipoRebar = TipoRebar.LOSA_INCLI_F3;
                        break;
                    case TipoBarra.f4_incli:
                        _tipoRebar = TipoRebar.LOSA_INCLI_F4;
                        break;               
                    default:
                        _tipoRebar = TipoRebar.NONE;
                        break;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool AsignarBarrasInferiore(string tipoBarra)
        {
            try
            {
                switch (tipoBarra)
                {
                    case "f11":
                    case "f9":
                    case "f7":
                    case "f3":
                    case "f4":
                    case "f1":
                    case "f16":
                    case "f16_Izq":
                    case "f16_Dere":
                    case "f17":
                    case "f17A_Tras":
                    case "f17B_Tras":
                    case "f18":
                    case "f19":
                    case "f19_Izq":
                    case "f19_Dere":
                    case "f20":
                    case "f20A_Izq_Tras":
                    case "f20B_Dere_Tras":
                    case "f20A_Dere_Tras":
                    case "f20B_Izq_Tras":
                    case "f21":
                    case "f21A_Izq_Tras":
                    case "f21A_Dere_Tras":
                    case "f21B_Izq_Tras":
                    case "f21B_Dere_Tras":
                        _tipoRebar = TipoRebar.LOSA_INF;
                        break;
                    case "f1_SUP":
                        _tipoRebar = TipoRebar.LOSA_SUP;
                        break;
                    case "s1":
                        _tipoRebar = TipoRebar.LOSA_SUP_S1;
                        break;
                    case "s2":
                        _tipoRebar = TipoRebar.LOSA_SUP_S2;
                        break;
                    case "s3":
                        _tipoRebar = TipoRebar.LOSA_SUP_S3;
                        break;
                    case "s4":
                        _tipoRebar = TipoRebar.LOSA_SUP_S4;
                        break;
                    case "f3_esc45":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_45;
                        break;
                    case "f3_esc135":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_135;
                        break;
                    case "f3b_esc45":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3B_45;
                        break;
                    case "f3b_esc135":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3B_135;
                        break;
                    case "f1_esc45":
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_45;
                        break;
                    case "f1_esc45_conpata":
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_45_CONPATA;
                        break;
                    case "f1_esc135_sinpata":
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_135_SINPATA;
                        break;
                    case "f1_b":
                        _tipoRebar = TipoRebar.LOSA_ESC_F1B;
                        break;
                    case "f1_a":
                        _tipoRebar = TipoRebar.LOSA_ESC_F1A;
                        break;
                    case "f1_ab":
                        _tipoRebar = TipoRebar.LOSA_ESC_F1_AB;
                        break;
                    case "f3_ab":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_AB;
                        break;
                    case "f3_ba":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_BA;
                        break;
                    case "f3_a0":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_A0;
                        break;
                    case "f3_b0":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_B0;
                        break;
                    case "f3_0a":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_0A;
                        break;
                    case "f3_0b":
                        _tipoRebar = TipoRebar.LOSA_ESC_F3_0B;
                        break;
                    case "f1_incli":
                        _tipoRebar = TipoRebar.LOSA_INCLI_F1;
                        break;
                    case "f3_incli":
                        _tipoRebar = TipoRebar.LOSA_INCLI_F3;
                        break;
                    case "f4_incli":
                        _tipoRebar = TipoRebar.LOSA_INCLI_F4;
                        break;
                    default:
                        _tipoRebar = TipoRebar.NONE;
                        break;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        public bool AsignarBarraSuperior(string tipoBarra)
        {

            try
            {
                if (tipoBarra == "s1")
                    _tipoRebar = TipoRebar.LOSA_SUP_S1;
                else if (tipoBarra == "s2")
                    _tipoRebar = TipoRebar.LOSA_SUP_S2;
                else if (tipoBarra == "s3")
                    _tipoRebar = TipoRebar.LOSA_SUP_S3;
                else if (tipoBarra == "s4")
                    _tipoRebar = TipoRebar.LOSA_SUP_S4;
                else
                    return false;

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
