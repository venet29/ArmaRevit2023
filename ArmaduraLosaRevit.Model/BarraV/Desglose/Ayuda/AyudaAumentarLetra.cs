using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda
{
   public class AyudaAumentarLetra
    {
        public static char result { get; private set; }

        public static  bool Ejecutar(char car )
        {

            try
            {
              //  char car = 'A';
                car++;
                result = car; // B 
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg("Error al incrementar letra");
                return false;
            }

            return true;
        }
    }
}
