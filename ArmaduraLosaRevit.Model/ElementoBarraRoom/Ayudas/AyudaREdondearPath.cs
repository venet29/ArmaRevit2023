using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    class AyudaREdondearPath
    {
        public static double LargoCuadradoPAth { get; internal set; }


        internal static bool Redondera(double largoActualPAth, int redondeo)
        {
            try
            {
                if (redondeo == 5)
                    Redonderar5(largoActualPAth);
                else
                {
                    Util.ErrorMsg($" Redondeo a {redondeo} no esta implemnetado");
                    return false;
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'Redondera()' path   \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private static void Redonderar5(double largoActualPAth)
        {
            throw new NotImplementedException();
        }



    }




}
