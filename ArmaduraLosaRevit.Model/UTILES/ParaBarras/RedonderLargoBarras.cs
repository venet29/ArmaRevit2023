using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class RedonderLargoBarras
    {
        private static double LargoBarraInicialcm;
        public static double DeltaDesplazaminetoRedondeoFoot { get; set; } // distancia que se acorta para hacer redondedo  eje, L=252  DeltaDesplazaminetoRedondeoFoot=2    ;;   L=558   DeltaDesplazaminetoRedondeoFoot=2 --> L=560
        public static double NuevoLargobarraFoot { get; set; }
        public static CoordenadaPath CoordenadaPath4 { get; set; }
        public static double NuevoLargobarracm { get; set; }
        public static double largoModificar_deltaCM { get; private set; }
        public static double largoPata_L1 { get; internal set; }

        public static bool RedondearPtos_5MasCercano(XYZ p1, XYZ p2, XYZ p3, XYZ p4)
        {
            try
            {
                double LargoBarraInicialcm_ = Util.FootToCm(p3.DistanceTo(p2)); //756.5
                if (Redondear5(LargoBarraInicialcm_))
                {
                    XYZ direccion23 = (p2 - p3).Normalize();
                    XYZ p1N = p1 + direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;
                    XYZ p2N = p2 + direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;
                    XYZ p3N = p3 - direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;
                    XYZ p4N = p4 - direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;

                    CoordenadaPath4 = new CoordenadaPath(p1N, p2N, p3N, p4N, Enumeraciones.TipoCaraObjeto.Superior);
                }
                else
                    return false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Debug.WriteLine("Error al redondear");
                return false;
            }
            return true;
        }

        internal static bool RedondearFoot5_MasArriba(double v)
        {
            throw new NotImplementedException();
        }

        public static bool RedondearPtos_5MasCercano_Alt(XYZ p1, XYZ p2, XYZ p3, XYZ p4)
        {

            try
            {
                double LargoBarraInicialcm_ = Util.FootToCm(p3.DistanceTo(p2)); //756.5
                if (Redondear5_alt(LargoBarraInicialcm_))
                {
                    XYZ direccion23 = (p2 - p3).Normalize();
                    XYZ p1N = p1 + direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;
                    XYZ p2N = p2 + direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;

                    XYZ p3N = p3 - direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;
                    XYZ p4N = p4 - direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;

                    CoordenadaPath4 = new CoordenadaPath(p1N, p2N, p3N, p4N, Enumeraciones.TipoCaraObjeto.Superior);

                }
                else
                    return false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Debug.WriteLine("Error al redondear");
                return false;
            }
            return true;
        }


        public static bool RedondearPtos_1MasCercano(XYZ p1, XYZ p2, XYZ p3, XYZ p4)
        {

            try
            {
                double LargoBarraInicialcm_ = Util.FootToCm(p3.DistanceTo(p2)); //756.5
                if (Redondear1(LargoBarraInicialcm_))
                {
                    XYZ direccion23 = (p2 - p3).Normalize();
                    XYZ p1N = p1 + direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;
                    XYZ p2N = p2 + direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;

                    XYZ p3N = p3 - direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;
                    XYZ p4N = p4 - direccion23 * DeltaDesplazaminetoRedondeoFoot / 2;

                    CoordenadaPath4 = new CoordenadaPath(p1N, p2N, p3N, p4N, Enumeraciones.TipoCaraObjeto.Superior);

                }
                else
                    return false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Debug.WriteLine("Error al redondear");
                return false;
            }
            return true;
        }

        //************************************************

        public static bool RedondearFoot5_AltMascercano(double LargoBarraInicialfoot_) => Redondear5_alt(Util.FootToCm(LargoBarraInicialfoot_));

        public static bool Redondear5_alt(double LargoBarraInicialcm_)
        {

            try
            {

                LargoBarraInicialcm = LargoBarraInicialcm_;
                double parteEntera = Util.ParteEnteraInt(LargoBarraInicialcm);//756
                double parteDecimal = Util.ParteDecimal(LargoBarraInicialcm);//0.5

                double parteDecimal2 = Util.ParteDecimal(parteEntera / 10); //75.6  -> 0.6
                double parteEntera2 = Util.ParteEnteraInt(parteEntera / 10) * 10; //75.6  -> 75*10=750  
                double numeroRedondear = parteDecimal2 * 10 + parteDecimal; // 0.6*10+0.5 = 6.5

                NuevoLargobarracm = 0;
                double DeltaDesplazaminetoRedondeoCM = 0;
                DeltaDesplazaminetoRedondeoFoot = 0;

                if (numeroRedondear >= 8)
                    NuevoLargobarracm = parteEntera2 + 10; //760
                else if (numeroRedondear >= 3)
                    NuevoLargobarracm = parteEntera2 + 5;//755
                else
                    NuevoLargobarracm = parteEntera2; //750

                DeltaDesplazaminetoRedondeoCM = NuevoLargobarracm - LargoBarraInicialcm;
                DeltaDesplazaminetoRedondeoFoot = Util.CmToFoot(DeltaDesplazaminetoRedondeoCM);
                NuevoLargobarraFoot = Util.CmToFoot(NuevoLargobarracm);


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'Redondear5()' path   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        //************************************************
        public static bool RedondearFoot5_mascercano(double LargoBarraInicialfoot_) => Redondear5(Util.FootToCm(LargoBarraInicialfoot_));

        public static bool Redondear5(double LargoBarraInicialcm_)
        {

            try
            {

                LargoBarraInicialcm = LargoBarraInicialcm_;
                double parteEntera = Util.ParteEnteraInt(LargoBarraInicialcm);//756
                double parteDecimal = Util.ParteDecimal(LargoBarraInicialcm);//0.5

                double parteDecimal2 = Util.ParteDecimal(parteEntera / 10); //75.6  -> 0.6
                double parteEntera2 = Util.ParteEnteraInt(parteEntera / 10) * 10; //75.6  -> 75*10=750  
                double numeroRedondear = parteDecimal2 * 10 + parteDecimal; // 0.6*10+0.5 = 6.5

                NuevoLargobarracm = 0;
                largoModificar_deltaCM = 0;
                DeltaDesplazaminetoRedondeoFoot = 0;

                if (numeroRedondear >= 7.5)
                    NuevoLargobarracm = parteEntera2 + 10; //760
                else if (numeroRedondear >= 2.5)
                    NuevoLargobarracm = parteEntera2 + 5;//755
                else
                    NuevoLargobarracm = parteEntera2; //750

                largoModificar_deltaCM = NuevoLargobarracm - LargoBarraInicialcm;
                DeltaDesplazaminetoRedondeoFoot = Util.CmToFoot(largoModificar_deltaCM);
                NuevoLargobarraFoot = Util.CmToFoot(NuevoLargobarracm);


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'Redondear5()' path   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }




        //***********************************************/

        public static bool RedondearFoot1_masbajo(double LargoBarraInicialfoot_)
        { 
            try
            {
                LargoBarraInicialcm = Util.FootToCm(LargoBarraInicialfoot_);
                var LargoBarraFinalcm = Math.Floor(LargoBarraInicialcm);
                largoModificar_deltaCM = Math.Round(LargoBarraFinalcm - LargoBarraInicialcm,0);
                DeltaDesplazaminetoRedondeoFoot = Util.CmToFoot(largoModificar_deltaCM);
                NuevoLargobarraFoot = Util.CmToFoot(LargoBarraFinalcm);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'Redondear5()' path   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public static bool RedondearFoot5_masArriba(double LargoBarraInicialfoot_)
        {
            try
            {           
                LargoBarraInicialcm = Util.FootToCm(LargoBarraInicialfoot_);
                double parteEntera = Util.ParteEnteraInt(LargoBarraInicialcm);//756
                double parteDecimal = Util.ParteDecimal(LargoBarraInicialcm);//0.5

                double parteDecimal2 = Util.ParteDecimal(parteEntera / 10); //75.6  -> 0.6
                double parteEntera2 = Util.ParteEnteraInt(parteEntera / 10) * 10; //75.6  -> 75*10=750  
                double numeroRedondear = parteDecimal2 * 10 + parteDecimal; // 0.6*10+0.5 = 6.5

                NuevoLargobarracm = 0;
                largoModificar_deltaCM = 0;
                DeltaDesplazaminetoRedondeoFoot = 0;

                if (numeroRedondear >= 5)
                    NuevoLargobarracm = parteEntera2 + 10; //760
                else if (numeroRedondear >= 0.1)
                    NuevoLargobarracm = parteEntera2 + 5;//755
                else
                    NuevoLargobarracm = parteEntera2; //750

                largoModificar_deltaCM = NuevoLargobarracm - LargoBarraInicialcm;
                DeltaDesplazaminetoRedondeoFoot = Util.CmToFoot(largoModificar_deltaCM);
                NuevoLargobarraFoot = Util.CmToFoot(NuevoLargobarracm);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'Redondear5()' path   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public static bool RedondearFoot1_mascercano(double LargoBarraInicialfoot_) => Redondear1(Util.FootToCm(LargoBarraInicialfoot_));

        public static bool Redondear1(double LargoBarraInicialcm_)
        {

            try
            {

                LargoBarraInicialcm = LargoBarraInicialcm_;
                double parteEntera = Util.ParteEnteraInt(LargoBarraInicialcm);//756
                double parteDecimal = Util.ParteDecimal(LargoBarraInicialcm);//0.5

                double parteDecimal2 = Util.ParteDecimal(parteEntera); //75.6  -> 0.6
                double parteEntera2 = Util.ParteEnteraInt(parteEntera); //75.6  -> 75*10=750  
                double numeroRedondear = parteDecimal * 10; // 0.6*10+0.5 = 6.5

                NuevoLargobarracm = 0;
                largoModificar_deltaCM = 0;
                DeltaDesplazaminetoRedondeoFoot = 0;

                if (numeroRedondear >= 7.5)
                    NuevoLargobarracm = parteEntera2 + 1; //760
                else if (numeroRedondear >= 2.5)
                    NuevoLargobarracm = parteEntera2 + 1;//755
                else
                    NuevoLargobarracm = parteEntera2; //750

                largoModificar_deltaCM = NuevoLargobarracm - LargoBarraInicialcm;
                DeltaDesplazaminetoRedondeoFoot = Util.CmToFoot(largoModificar_deltaCM);
                NuevoLargobarraFoot = Util.CmToFoot(NuevoLargobarracm);


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'Redondear5()' path   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }


        //redondear pata
        internal static bool Redondear5_LargoPAta_L1(double largoAhorroIzq, double espesorLosa_EnFoot)
        {
            try
            {
                if (Redondear5(Util.FootToCm(largoAhorroIzq + espesorLosa_EnFoot)))
                {
                    largoPata_L1 = NuevoLargobarraFoot - espesorLosa_EnFoot;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'Redondear5()' path   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
