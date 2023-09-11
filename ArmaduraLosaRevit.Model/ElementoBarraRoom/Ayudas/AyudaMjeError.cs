using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
   public class AyudaMjeError
    {
        public static string getErrorLosas(Exception ex)
        {
            if (ex.HResult == -2146233088)
            {
                return $"\nex:{ex.Message} \n\n Posiblemente losa no este definida como 'Structural'";
            }
            return "";
        }
    }
}
