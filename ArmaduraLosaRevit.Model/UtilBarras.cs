using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model
{
    public class UtilBarras
    {
        #region 4 metodod para notificaciones

        private static bool IsConNotificaciones_Aux=true;
        public static bool IsConNotificaciones { get; set; } = true;
        public static void IsConNotificacionesOff()
        {
            IsConNotificaciones_Aux = IsConNotificaciones;
            IsConNotificaciones = false;
        }
        public static void IsConNotificacionesON() => IsConNotificaciones = IsConNotificaciones_Aux;

        #endregion
        public static int DimatrolDeFootaMM(double espesor)
        {
            double delta = 0.001;

            if (Math.Abs(espesor - 0.0262467) < delta)
            { return 8; }
            else if (Math.Abs(espesor - 0.019685) < delta)
            { return 6; }
            else if (Math.Abs(espesor - 0.0328084) < delta)
            { return 10; }
            else if (Math.Abs(espesor - 0.0393701) < delta)
            { return 12; }

            else if (Math.Abs(espesor - 0.0524934) < delta)
            { return 16; }

            else if (Math.Abs(espesor - 0.0590551) < delta)
            { return 18; }

            else if (Math.Abs(espesor - 0.0721785) < delta)
            { return 22; }

            else if (Math.Abs(espesor - 0.082021) < delta)
            { return 25; }
            else if (Math.Abs(espesor - 0.0918635) < delta)
            { return 28; }

            else if (Math.Abs(espesor - 0.104987) < delta)
            { return 32; }

            else if (Math.Abs(espesor - 0.11811) < delta)
            { return 36; }


            return -1;
        }

        public static bool ValidarEspesor(double espesor)
        {
            if (DatosDiseño.DISENO_VALIDAR_ESPESOR == TipoValidarEspesor.NOVerificarEspesorMenor15)
                return false;
            else
                return (espesor < Util.CmToFoot(15.0) && espesor > 0);//  ACI 8.7.4.1.3
        }


        public static int ContadorMAyor12mt { get; set; } = 0;
        public static void BarrasMAyores()
        {
            ContadorMAyor12mt += 1;
            if (IsConNotificaciones)
                Util.ErrorMsg("Barras mayor de 12 mt");
        }



        //Obs4 ) definido por cdv
        public static double largo_L9_DesarrolloFoot_diamMM(int diamEnmm)
        {
            double largo_desarrollo = LargoBarras_x(diamEnmm);
            return largo_desarrollo;
        }
        public static double largo_L9_DesarrolloFoot_diamFoot(double diametroFoot)
        {
            //double largo_traslapo = 0;
            int diam = (int)Util.FootToMmInt(diametroFoot);
            return largo_L9_DesarrolloFoot_diamMM(diam);
        }




        #region casoso especiales
        //uso exclusivo para refuerzo losa
        public static double largo_L9_DesarrolloFoot_diamMM_cabeza_dentroMuro(int diamEnmm)
        {
            double largo_desarrollo = 0;
            switch (diamEnmm)
            {
                case 1:
                    largo_desarrollo = Util.CmToFoot(1);
                    break;
                case 6:
                    largo_desarrollo = Util.CmToFoot(50);
                    break;
                case 8:
                    largo_desarrollo = Util.CmToFoot(50);
                    break;
                case 10:
                    largo_desarrollo = Util.CmToFoot(60);
                    break;
                case 12:
                    largo_desarrollo = Util.CmToFoot(80);
                    break;
                case 16:
                    largo_desarrollo = Util.CmToFoot(80);
                    break;
                case 18:
                    largo_desarrollo = Util.CmToFoot(80);
                    break;
                case 22:
                    largo_desarrollo = Util.CmToFoot(165);
                    break;
                case 25:
                    largo_desarrollo = Util.CmToFoot(185);
                    break;
                case 28:
                    largo_desarrollo = Util.CmToFoot(210);
                    break;
                case 32:
                    largo_desarrollo = Util.CmToFoot(240);
                    break;
                case 36:
                    largo_desarrollo = Util.CmToFoot(260);
                    break;
            }
            return largo_desarrollo;
        }


        //uso exclusivo para refuerzo losa
        public static double largo_L9_DesarrolloFoot_diamMM_cabeza_fueramuro(int diamEnmm)
        {
            double largo_desarrollo = 0;
            switch (diamEnmm)
            {
                case 1:
                    largo_desarrollo = Util.CmToFoot(1);
                    break;
                case 6:
                    largo_desarrollo = Util.CmToFoot(50);
                    break;
                case 8:
                    largo_desarrollo = Util.CmToFoot(50);
                    break;
                case 10:
                    largo_desarrollo = Util.CmToFoot(60);
                    break;
                case 12:
                    largo_desarrollo = Util.CmToFoot(120);
                    break;
                case 16:
                    largo_desarrollo = Util.CmToFoot(120);
                    break;
                case 18:
                    largo_desarrollo = Util.CmToFoot(120);
                    break;
                case 22:
                    largo_desarrollo = Util.CmToFoot(165);
                    break;
                case 25:
                    largo_desarrollo = Util.CmToFoot(185);
                    break;
                case 28:
                    largo_desarrollo = Util.CmToFoot(210);
                    break;
                case 32:
                    largo_desarrollo = Util.CmToFoot(240);
                    break;
                case 36:
                    largo_desarrollo = Util.CmToFoot(260);
                    break;
            }
            return largo_desarrollo;
        }

        #endregion


        ///ob1)c 
        public static double largo_ganchoFoot_diamMM(int diamEnmm)
        {
            //diamtro var 12 * total   aprox
            //8   40  96  13.6    14
            //10  60  120 18  18
            //12  72  144 21.6    22
            //16  96  192 28.8    29
            //18  108 216 32.4    33
            //22  132 264 39.6    40
            //25  150 300 45  45
            //28  224 336 56  56
            //30  240 360 60  60
            //32  256 384 64  64
            //36  288 432 72  65

            //TABLA 1. DIÁMETRO MÍNIMO INTERIOR DE DOBLADO Y GEOMETRÍA DE GANCHO  ESTÁNDAR PARA BARRAS CORRUGADAS EN TRACCIÓN
            //tabla by NH
            double largo_traslapo = 0;
            switch (diamEnmm)
            {
                case 6:
                    largo_traslapo = 15;//12D=9.6   +5d=4

                    break;
                case 8:
                    largo_traslapo = 15;//12D=9.6 +5d=4
                    break;
                case 10:
                    largo_traslapo = 20;//12D=12 +6d=6
                    break;
                case 12:
                    largo_traslapo = 25;//12D=14.4+6d=7.2
                    break;
                case 16:
                    largo_traslapo = 30;//12D=19.2 +6d=9.6
                    break;
                case 18:
                    largo_traslapo = 35;//12D=21.6 +6d=10.8
                    break;
                case 22:
                    largo_traslapo = 40;//12D=26.4 +6d=13.2
                    break;
                case 25:
                    largo_traslapo = 45;//12D=30 +6d=15.0
                    break;
                case 28:
                    largo_traslapo = 50;//12D=33.6 +8d=22.4
                    break;
                case 32:
                    largo_traslapo = 55;//12D=36 +8d=25.6
                    break;
                case 36:
                    largo_traslapo = 60;//12D=38.4 +8d=28.8
                    break;
            }

            return Util.CmToFoot(largo_traslapo);
        }

        public static double largo_ganchoFoot_diamFoot(double diametro)
        {
            //double largo_traslapo = 0;
            int diam = (int)Util.FootToMmInt(diametro);
            return largo_ganchoFoot_diamMM(diam);
        }



        ///ob1)c 
        public static int largo_gancho_diamMM_soloFUndaciones_cm(int diamEnmm)
        {
            //diamtro var 12 * total   aprox
            //8   40  96  13.6    14
            //10  60  120 18  18
            //12  72  144 21.6    22
            //16  96  192 28.8    29
            //18  108 216 32.4    33
            //22  132 264 39.6    40
            //25  150 300 45  45
            //28  224 336 56  56
            //30  240 360 60  60
            //32  256 384 64  64
            //36  288 432 72  65

            //TABLA 1. DIÁMETRO MÍNIMO INTERIOR DE DOBLADO Y GEOMETRÍA DE GANCHO  ESTÁNDAR PARA BARRAS CORRUGADAS EN TRACCIÓN
            //tabla by NH
            int largo_PAtaFundaciones = 0;
            switch (diamEnmm)
            {
                case 6:
                    largo_PAtaFundaciones = 25;//12D=9.6   +5d=4

                    break;
                case 8:
                    largo_PAtaFundaciones = 25;//12D=9.6 +5d=4
                    break;
                case 10:
                    largo_PAtaFundaciones = 25;//12D=12 +6d=6
                    break;
                case 12:
                    largo_PAtaFundaciones = 25;//12D=14.4+6d=7.2
                    break;
                case 16:
                    largo_PAtaFundaciones = 30;//12D=19.2 +6d=9.6
                    break;
                case 18:
                    largo_PAtaFundaciones = 35;//12D=21.6 +6d=10.8
                    break;
                case 22:
                    largo_PAtaFundaciones = 40;//12D=26.4 +6d=13.2
                    break;
                case 25:
                    largo_PAtaFundaciones = 45;//12D=30 +6d=15.0
                    break;
                case 28:
                    largo_PAtaFundaciones = 50;//12D=33.6 +8d=22.4
                    break;
                case 32:
                    largo_PAtaFundaciones = 55;//12D=36 +8d=25.6
                    break;
                case 36:
                    largo_PAtaFundaciones = 60;//12D=38.4 +8d=28.8
                    break;
            }

            return largo_PAtaFundaciones;
        }

        public static double largo_traslapFoot_diamFoot(double diametro)
        {
            //double largo_traslapo = 0;
            int diam = (int)Util.FootToMmInt(diametro);
            return largo_traslapoFoot_diamMM(diam);
        }

        public static double largo_traslapoFoot_diamMM(int diamEnmm)
        {
            double largo_traslapo = LargoBarras_x(diamEnmm);
            return largo_traslapo;
        }
        private static double LargoBarras_x(int diamEnmm)
        {
            double largo_desarrollo = 0;
            switch (diamEnmm)
            {
                case 1:
                    largo_desarrollo = Util.CmToFoot(1);
                    break;
                case 6:
                    largo_desarrollo = Util.CmToFoot(50);
                    break;
                case 8:
                    largo_desarrollo = Util.CmToFoot(50);
                    break;
                case 10:
                    largo_desarrollo = Util.CmToFoot(60);
                    break;
                case 12:
                    largo_desarrollo = Util.CmToFoot(75);
                    break;
                case 16:
                    largo_desarrollo = Util.CmToFoot(95);
                    break;
                case 18:
                    largo_desarrollo = Util.CmToFoot(110);
                    break;
                case 22:
                    largo_desarrollo = Util.CmToFoot(165);
                    break;
                case 25:
                    largo_desarrollo = Util.CmToFoot(185);
                    break;
                case 28:
                    largo_desarrollo = Util.CmToFoot(210);
                    break;
                case 32:
                    largo_desarrollo = Util.CmToFoot(240);
                    break;
                case 36:
                    largo_desarrollo = Util.CmToFoot(260);
                    break;
            }

            return largo_desarrollo;
        }
        public static double ObtenerBendDiamFoot(int diamEnmm)
        {

            //tabla by NH
            double largo_traslapo = 0;
            switch (diamEnmm)
            {
                case 6:
                    largo_traslapo = 15;//12D=9.6   +5d=4

                    break;
                case 8:
                    largo_traslapo = 15;//12D=9.6 +5d=4
                    break;
                case 10:
                    largo_traslapo = 20;//12D=12 +6d=6
                    break;
                case 12:
                    largo_traslapo = 25;//12D=14.4+6d=7.2
                    break;
                case 16:
                    largo_traslapo = 30;//12D=19.2 +6d=9.6
                    break;
                case 18:
                    largo_traslapo = 35;//12D=21.6 +6d=10.8
                    break;
                case 22:
                    largo_traslapo = 40;//12D=26.4 +6d=13.2
                    break;
                case 25:
                    largo_traslapo = 45;//12D=30 +6d=15.0
                    break;
                case 28:
                    largo_traslapo = 50;//12D=33.6 +8d=22.4
                    break;
                case 32:
                    largo_traslapo = 55;//12D=36 +8d=25.6
                    break;
                case 36:
                    largo_traslapo = 60;//12D=38.4 +8d=28.8
                    break;
            }

            return Util.CmToFoot(largo_traslapo);
        }

        // pts[0]   pt1-------  pt2    pts[1]
        public static XYZ[] extenderLineaDistanciaPorDiamtro(XYZ pto1, XYZ pto2, int diam)
        {
            double largoTraslapo = largo_traslapoFoot_diamMM(diam);
            return extenderLineaDistancia(pto1, pto2, largoTraslapo);
        }


        public static XYZ[] extenderLineaDistancia(XYZ pto1, XYZ pto2, double largoExtension)
        {

            XYZ[] valoresOrdenados = Util.Ordena2Ptos(pto1, pto2);
            pto1 = valoresOrdenados[0];
            pto2 = valoresOrdenados[1];

            XYZ[] pts = new XYZ[2];

            Transform trans1 = null;
            Transform Invertrans1 = null;
            Transform trans2_rotacion = null;
            Transform InverTrans2_rotacion = null;
            double anguloBarra_ = Util.AnguloEntre2PtosGrado90(pto1, pto2, EnGrados: true);

            trans1 = Transform.CreateTranslation(-pto1);

            //if (anguloBarra_ > 90)
            //{ trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(anguloBarra_ - 180), XYZ.Zero); }
            //else
            //    trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(anguloBarra_), XYZ.Zero);

            trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(anguloBarra_), XYZ.Zero);
            Invertrans1 = trans1.Inverse;
            InverTrans2_rotacion = trans2_rotacion.Inverse;
            //trans1.Origin = listaPtos[3];


            //p1,p2,p3,p4 pto  del segemneto que 
            XYZ p1 = new XYZ();
            XYZ p2 = new XYZ();


            p1 = trans1.OfPoint(pto1);
            p2 = trans1.OfPoint(pto2);

            p1 = trans2_rotacion.OfPoint(trans1.OfPoint(pto1));
            p2 = trans2_rotacion.OfPoint(trans1.OfPoint(pto2));

            pts[0] = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(p1 + new XYZ(-largoExtension, 0, 0)));
            pts[1] = Invertrans1.OfPoint(InverTrans2_rotacion.OfPoint(p2 + new XYZ(largoExtension, 0, 0)));

            //vuelve a atras la configuracon de los puntos de la linea para tener sentido antihorario
            XYZ[] volverAtras = Util.Ordena2PtosVolverAtras(pts[0], pts[1]);

            pts[0] = volverAtras[0];
            pts[1] = volverAtras[1];


            return pts;
        }

        public static Tuple<int, int> ObtenerDiametroEspesorEnMMyCM(string CuantiaBarra)
        {
            Tuple<int, int> resultDefault = Tuple.Create(8, 20);
            string[] auxcuantia = CuantiaBarra.Split('a');

            int _diametro = 0;
            bool resulDiametro = int.TryParse(auxcuantia[0], out _diametro);
            if (!resulDiametro)
            {
                TaskDialog.Show("Error datos Losa", "Losa sin Diametro o mal definido");
                return resultDefault;
            }

            int _espaciamiento = 0;
            bool resulEspaciemientoo = int.TryParse(auxcuantia[1], out _espaciamiento);
            if (!resulEspaciemientoo)
            {
                TaskDialog.Show("Error datos Losa", "Losa sin espaciamiento o mal definido");
                return resultDefault;
            }

            Tuple<int, int> result = Tuple.Create(_diametro, _espaciamiento);

            return result;

        }

        public static XYZ ObtenerCentroPoligono(List<XYZ> listaPtos)
        {
            return new XYZ(listaPtos.Average(c => c.X),
                            listaPtos.Average(c => c.Y),
                            listaPtos.Average(c => c.Z));
        }

    }
}
