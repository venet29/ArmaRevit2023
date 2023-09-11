using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Data.Servicio
{
    public class ServicioModificarCoordinaBArrasPorTraslapo
    {
        public ServicioModificarCoordinaBArrasPorTraslapo()
        {

        }
        public BarraFlexionTramosDTO ModificarInicial(BarraFlexionTramosDTO barra)
        {



            if (barra.IdentiFIcadorParaTraslapo == "SUP_1")
            {
                if (barra.CasosTraslapoDTO_InicioTramo == null) return barra;

                CAmbiarTraslapoPrimeraLInea(barra);
            }
            else if (barra.IdentiFIcadorParaTraslapo == "SUP_2")
            {
                if (barra.CasosTraslapoDTO_InicioTramo == null) return barra;
            }
            else if (barra.IdentiFIcadorParaTraslapo == "INF_1")
            {
                if (barra.CasosTraslapoDTO_InicioTramo == null) return barra;
                CAmbiarTraslapoPrimeraLInea(barra);
            }
            else if (barra.IdentiFIcadorParaTraslapo == "INF_2")
            {
                if (barra.CasosTraslapoDTO_InicioTramo == null) return barra;
            }
            else
                Util.ErrorMsg($"No esta creada modificar coordenadas para caso{barra.IdentiFIcadorParaTraslapo} para viga id:{barra.ID_Name_REVIT_Inicio}");


            return barra;
        }

        private void CAmbiarTraslapoPrimeraLInea(BarraFlexionTramosDTO barra)
        {
            int minDIam = 0;
            //largoTraslaPorDiamtro_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(barra.diametro_Barras__mm));
            minDIam = Math.Min(barra.CasosTraslapoDTO_InicioTramo.BarraTramosAnterior.diametro_Barras__mm,
                                     barra.CasosTraslapoDTO_InicioTramo.BarraTramosPosterior.diametro_Barras__mm);

            double largoPataTrasccion_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(minDIam));
            double largoPataCompresion_mmm = 30 * minDIam;

            barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.NONE;
            barra.TipoPataIzqInf = TipoPataBarra.BarraVSinPatas;// NO BUSCA NI EXTIENDE
            switch (barra.CasosTraslapoDTO_InicioTramo.TipoTraslapoEnSeccionNh_)
            {
                case TipoTraslapoEnSeccionNh.Tt_1:
                    barra.p1_mm.X = barra.p1_mm.X - largoPataTrasccion_mm / 2.0d;
                    break;
                case TipoTraslapoEnSeccionNh.Tt_izq_2:
                    barra.p1_mm.X = barra.CasosTraslapoDTO_FinTramo.BarraTramosAnterior.p2_mm.X - largoPataTrasccion_mm;
                    //barra.p1_mm.X = barra.p1_mm.X - largoPataTrasccion_mm;
                    break;
                case TipoTraslapoEnSeccionNh.Tt_dere_3:
                    break;
                case TipoTraslapoEnSeccionNh.Tc_4:
                    barra.p1_mm.X = barra.p1_mm.X - largoPataCompresion_mmm / 2.0d;
                    break;
                case TipoTraslapoEnSeccionNh.Tc_izq_5:
                    barra.p1_mm.X = barra.CasosTraslapoDTO_FinTramo.BarraTramosAnterior.p2_mm.X - largoPataCompresion_mmm;
                    break;
                case TipoTraslapoEnSeccionNh.Tc_dere_6:
         
                    break;
                case TipoTraslapoEnSeccionNh.G_7:
                    break;
                case TipoTraslapoEnSeccionNh.NONE:
                    break;
            }
        }

        internal BarraFlexionTramosDTO ModificarFinal(BarraFlexionTramosDTO barra)
        {
            if (barra.IdentiFIcadorParaTraslapo == "SUP_1")
            {
                if (barra.CasosTraslapoDTO_FinTramo == null) return barra;
                CAmbiarTraslapoPrimeraLInea_final(barra);
            }
            else if (barra.IdentiFIcadorParaTraslapo == "SUP_2")
            {
                if (barra.CasosTraslapoDTO_FinTramo == null) return barra;
            }
            else if (barra.IdentiFIcadorParaTraslapo == "INF_1")
            {
                if (barra.CasosTraslapoDTO_FinTramo == null) return barra;
                CAmbiarTraslapoPrimeraLInea_final(barra);
            }
            else if (barra.IdentiFIcadorParaTraslapo == "INF_2")
            {
                if (barra.CasosTraslapoDTO_FinTramo == null) return barra;
            }
            else
                Util.ErrorMsg($"No esta creada modificar coordenadas para caso{barra.IdentiFIcadorParaTraslapo} para viga id:{barra.ID_Name_REVIT_Inicio}");

            return barra;
        }


        private void CAmbiarTraslapoPrimeraLInea_final(BarraFlexionTramosDTO barra)
        {                                 
            int minDIam = Math.Min(barra.CasosTraslapoDTO_FinTramo.BarraTramosAnterior.diametro_Barras__mm,
                                     barra.CasosTraslapoDTO_FinTramo.BarraTramosPosterior.diametro_Barras__mm);

            double largoPataTrasccion_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(minDIam));
            double largoPataCompresion_mm = 30 * minDIam;

            //borrar largo traslapo, paradibujar barra sin tralapo
            //barra.p2_mm.X = barra.p2_mm.X - largoPataTrasccion_mm;

            barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.NONE;
            barra.TipoPataDereSup = TipoPataBarra.BarraVSinPatas;// NO BUSCA NI EXTIENDE

            switch (barra.CasosTraslapoDTO_FinTramo.TipoTraslapoEnSeccionNh_)
            {
                case TipoTraslapoEnSeccionNh.Tt_1:
                    //barra.p2_mm.X = barra.p2_mm.X - largoPataTrasccion_mm / 2.0d;
                    barra.p2_mm.X = barra.p2_mm.X + largoPataTrasccion_mm / 2.0d;
                    break;
                case TipoTraslapoEnSeccionNh.Tt_izq_2:
                    //barra.TraslapoFinTramo.BarraTramosPosterior.p1_mm.X = barra.p2_mm.X;                                    
                    break;
                case TipoTraslapoEnSeccionNh.Tt_dere_3:
                    barra.p2_mm.X = barra.CasosTraslapoDTO_FinTramo.BarraTramosPosterior.p1_mm.X + largoPataTrasccion_mm;
                    break;
                case TipoTraslapoEnSeccionNh.Tc_4:
                    barra.p2_mm.X = barra.p2_mm.X - largoPataCompresion_mm / 2.0d;
                    break;
                case TipoTraslapoEnSeccionNh.Tc_izq_5:
                  //  barra.p2_mm.X = barra.p2_mm.X - largoPataCompresion_mm;                    
                    break;
                case TipoTraslapoEnSeccionNh.Tc_dere_6:
                    //dir_ = (barra.TraslapoFinTramo.BarraTramosAnterior != null ? barra.TraslapoFinTramo.BarraTramosAnterior.p1_mm.X : barra.p2_mm.X);
                    barra.p2_mm.X = barra.CasosTraslapoDTO_FinTramo.BarraTramosPosterior.p1_mm.X + largoPataCompresion_mm;
                    break;
                case TipoTraslapoEnSeccionNh.G_7:
                    break;
                case TipoTraslapoEnSeccionNh.NONE:
                    break;
            }

        }
    }
}
