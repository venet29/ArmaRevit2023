using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Enumeraciones
{

    public enum TipoBarra
    {
        f1_SUP,
        f1, f3, f4, f7, f9, f9a,f10, f10a, f11,f11a, f11b, f11A, f11B, f12, f12a, f12b,
        f16a, f16b,
        f16, f16_Izq, f16_Dere,
        f17, f17a, f17A_Tras, f17B_Tras, f17b,
        f18,
        f19, f19_Izq, f19_Dere,
        f20, f20A_Izq_Tras, f20B_Izq_Tras, f20A_Dere_Tras, f20B_Dere_Tras,
        f21, f21A_Izq_Tras, f21B_Izq_Tras, f21A_Dere_Tras, f21B_Dere_Tras,
        f22, f22Tras, NONE,
        s1,s2,s3,s4,
        f1_esc45_conpata, f1_esc135_sinpata,
        f3_esc45, f3_esc135,f3b_esc45, f3b_esc135,
        f3_ba,f3_ab, f3_0a,f3_a0,f3_0b,f3_b0,
        f1A_pataB,
        f1B_pataB,
        f1A_pataA,
        f1B_pataA,
        f1_ab,
        f1_b,
        f3_incli,
        f3_incli_esc,
        f4_incli_esc,
        f1_a,
        f4_incli,
     
        f1_incliInf,
        f3_refuezoSuple,
        f22_Izq,
        f22_Dere,
        
        f20a,
        f20b,
        
        f20Inv,
        f20bInv,
        f20aInv,
        f22b,
        f22a,
        f21a,
        f21b,
        f22aInv,
        f22bInv,
        s4_Inclinada,
        f10_fund,
        f11_fund,
        f10A_fund,
        f10B_fund,
        f11A_fund,
        f11B_fund,
    }

    public enum NombreParametros { A_,B_,C_, D_,E_,F_, FF,G_,H_,LL}
    public enum TipoBarraTraslapoIzqBajo { f1, f3, f4, f7, f9, f9a, f10a, f11, f16, f17, f18, f19, f20, f21, NONE }
    public enum TipoBarraTraslapoDereArriba { f1, f3, f4, f7, f9, f9a, f10, f10a, f11, f11a, f12, f16, f17, f18, f19, f20, f20Inv, f21, s1, s2, s4, NONE }
    public enum TipoEditarPAth { f1, f3, f4, f7, f9, f9a, f10, f10a, f11, f11a, f12, f16,f16a,f16b, f17, f18, f19, f20, f20Inv, f21, f22, f22Inv, s1, s2, s4, NONE }
    public enum DiamtrosBarras_ { d6, d8,d10,d12,d16,d18,d22,d25,d28,d32,d36}
    
}
