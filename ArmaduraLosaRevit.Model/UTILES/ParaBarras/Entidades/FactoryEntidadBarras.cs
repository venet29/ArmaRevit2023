using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades
{
    public class FactoryEntidadBarras
    {
        public static List<EntidadBarras> ObtenerListaEntidades() => new List<EntidadBarras>() {
               new EntidadBarras(TipoRebar.ELEV_BA_H,"ELEV_BA_H",TipoBarraGeneral.Elevacion,"BA",Orientacion_Cub.H,Elemento_cub.VIGA ),
                new EntidadBarras(TipoRebar.ELEV_BA_V,"ELEV_BA_V" ,TipoBarraGeneral.Elevacion ,"BA",Orientacion_Cub.V,Elemento_cub.MURO ),
                new EntidadBarras(TipoRebar.ELEV_CO,"ELEV_CO" ,TipoBarraGeneral.Elevacion,"CO",Orientacion_Cub.V,Elemento_cub.MURO ),
                new EntidadBarras(TipoRebar.ELEV_CO_T,"ELEV_CO_T" ,TipoBarraGeneral.Elevacion ,"CO-T",Orientacion_Cub.V,Elemento_cub.MURO),
                new EntidadBarras(TipoRebar.ELEV_ES,"ELEV_ES" ,TipoBarraGeneral.Elevacion,"ES" ,Orientacion_Cub.V,Elemento_cub.MURO),
                new EntidadBarras(TipoRebar.ELEV_ES_L,"ELEV_ES_L" ,TipoBarraGeneral.Elevacion, "ES-L",Orientacion_Cub.V,Elemento_cub.MURO),
                new EntidadBarras(TipoRebar.ELEV_ES_T,"ELEV_ES_T",TipoBarraGeneral.Elevacion ,"ES-T",Orientacion_Cub.V,Elemento_cub.MURO),
                new EntidadBarras(TipoRebar.ELEV_ES_V,"ELEV_ES_V" ,TipoBarraGeneral.Elevacion,"ES-V" ,Orientacion_Cub.H,Elemento_cub.VIGA ),
                new EntidadBarras(TipoRebar.ELEV_ES_VL,"ELEV_ES_VL" ,TipoBarraGeneral.Elevacion ,"ES-VL",Orientacion_Cub.H,Elemento_cub.VIGA ),
                new EntidadBarras(TipoRebar.ELEV_ES_VT,"ELEV_ES_VT" ,TipoBarraGeneral.Elevacion ,"ES-VT",Orientacion_Cub.H,Elemento_cub.VIGA ),
                new EntidadBarras(TipoRebar.ELEV_MA_H,"ELEV_MA_H" ,TipoBarraGeneral.Elevacion,"MA" ,Orientacion_Cub.V,Elemento_cub.MURO),
                new EntidadBarras(TipoRebar.ELEV_MA_V,"ELEV_MA_V" ,TipoBarraGeneral.Elevacion,"MA" ,Orientacion_Cub.V,Elemento_cub.MURO),
                new EntidadBarras(TipoRebar.ELEV_MA_T,"ELEV_MA_T" ,TipoBarraGeneral.Elevacion,"MA-T" ,Orientacion_Cub.V,Elemento_cub.MURO),
                new EntidadBarras(TipoRebar.ELEV_REFUERZO_MURO,"ELEV_REFUERZO_MURO" ,TipoBarraGeneral.Elevacion,"BA" ,Orientacion_Cub.V,Elemento_cub.MURO),
                //new EntidadBarras(TipoRebar.ELEV_HORQ,"ELEV_HORQ" ,TipoBarraGeneral.Elevacion,"BA" ,Orientacion_Cub.H,Elemento_cub.VIGA),
                new EntidadBarras(TipoRebar.ELEV_BA_HORQ,"ELEV_BA_HORQ" ,TipoBarraGeneral.Elevacion,"BA" ,Orientacion_Cub.H,Elemento_cub.VIGA),

                new EntidadBarras(TipoRebar.ELEV_ESCA,"ELEV_ESCA" ,TipoBarraGeneral.Escalera,"BA" ,Orientacion_Cub.ESC,Elemento_cub.ESC),
                new EntidadBarras(TipoRebar.ELEV_ESC,"ELEV_ESC" ,TipoBarraGeneral.Escalera,"BA" ,Orientacion_Cub.ESC,Elemento_cub.ESC),


                new EntidadBarras(TipoRebar.REFUERZO_BA,"REFUERZO_BA" , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.REFUERZO_BA_CAB_MURO,"REFUERZO_BA_CAB_MURO"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.REFUERZO_BA_REF_LO,"REFUERZO_BA_REF_LO"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.REFUERZO_BA_BORDE,"REFUERZO_BA_BORDE"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.REFUERZO_SUPLE_CAB_MU,"REFUERZO_SUPLE_CAB_MU"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),

                new EntidadBarras(TipoRebar.REFUERZO_ES,"REFUERZO_ES"  , TipoBarraGeneral.Losa,"ES-LOSA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.REFUERZO_EST_REF_LO,"REFUERZO_EST_REF_LO"  , TipoBarraGeneral.Losa,"ES-LOSA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.REFUERZO_EST_BORDE,"REFUERZO_EST_BORDE"  , TipoBarraGeneral.Losa,"ES-LOSA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_INF,"LOSA_INF"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_SUP,"LOSA_SUP"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_SUP_S1,"LOSA_SUP_S1"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_SUP_S2,"LOSA_SUP_S2"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_SUP_S3,"LOSA_SUP_S3"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_SUP_S4,"LOSA_SUP_S4"  , TipoBarraGeneral.Losa,"BA",Orientacion_Cub.sup,Elemento_cub.LOSA),



                new EntidadBarras(TipoRebar.LOSA_ESC_F3_45,"LOSA_ESC_F3_45",TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_ESC_F3_135,"LOSA_ESC_F3_135" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_ESC_F3B_45,"LOSA_ESC_F3B_45" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                new EntidadBarras(TipoRebar.LOSA_ESC_F3B_135,"LOSA_ESC_F3B_135" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F1_45,"LOSA_ESC_F1_45" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F1_45_CONPATA,"LOSA_ESC_F1_45_CONPATA" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F1_135_SINPATA,"LOSA_ESC_F1_135_SINPATA" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F3_BA,"LOSA_ESC_F3_BA" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F3_AB,"LOSA_ESC_F3_AB" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F3_0A,"LOSA_ESC_F3_0A" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F3_A0,"LOSA_ESC_F3_0A" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F3_0B,"LOSA_ESC_F3_0B" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_ESC_F3_B0,"LOSA_ESC_F3_B0" ,TipoBarraGeneral.LosaEscalera,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),

                 new EntidadBarras(TipoRebar.LOSA_INCLI_F1,"LOSA_INCLI_F1" ,TipoBarraGeneral.LosaInclinida,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_INCLI_F3,"LOSA_INCLI_F3"  ,TipoBarraGeneral.LosaInclinida,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_INCLI_F4,"LOSA_INCLI_F4"  ,TipoBarraGeneral.LosaInclinida,"BA",Orientacion_Cub.inf,Elemento_cub.LOSA),

                 new EntidadBarras(TipoRebar.LOSA_SUP_BPT,"LOSA_SUP_BPT"  ,TipoBarraGeneral.Losa,"BPT",Orientacion_Cub.sup,Elemento_cub.LOSA),
                 new EntidadBarras(TipoRebar.LOSA_SUP_BR,"LOSA_SUP_BR"  ,TipoBarraGeneral.Losa,"BR",Orientacion_Cub.sup,Elemento_cub.LOSA),




                 new EntidadBarras(TipoRebar.FUND_BA_INF,"FUND_BA"  ,TipoBarraGeneral.Fundaciones,"BA",Orientacion_Cub.inf,Elemento_cub.FUND),
                 new EntidadBarras(TipoRebar.FUND_TRABA,"FUND_TRABA"  ,TipoBarraGeneral.Fundaciones,"BA-T",Orientacion_Cub.inf,Elemento_cub.FUND),
                   new EntidadBarras(TipoRebar.FUND_BA_INF,"FUND_BA_INF"  ,TipoBarraGeneral.Fundaciones,"BA",Orientacion_Cub.inf,Elemento_cub.FUND),
                 new EntidadBarras(TipoRebar.FUND_BA_SUP,"FUND_BA_SUP"  ,TipoBarraGeneral.Fundaciones,"BA",Orientacion_Cub.sup,Elemento_cub.FUND),
                 new EntidadBarras(TipoRebar.FUND_ES,"FUND_ES"  ,TipoBarraGeneral.Fundaciones,"ES-FUND",Orientacion_Cub.sup,Elemento_cub.FUND),
                 new EntidadBarras(TipoRebar.FUND_ES_T,"FUND_ES_T"  ,TipoBarraGeneral.Fundaciones,"ES-FUND",Orientacion_Cub.sup,Elemento_cub.FUND),
                  new EntidadBarras(TipoRebar.FUND_BA_BPT,"FUND_BA_BPT"  ,TipoBarraGeneral.Fundaciones,"BPT",Orientacion_Cub.sup,Elemento_cub.FUND),
                 new EntidadBarras(TipoRebar.NONE,"NONE"  ,TipoBarraGeneral.NONE,"NONE",Orientacion_Cub.NONE,Elemento_cub.NONE)
            };
    }
}
