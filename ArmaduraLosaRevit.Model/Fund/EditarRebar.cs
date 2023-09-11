using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund
{
    public class EditarRebar
    {
        public EditarRebar()
        {
            
        }


        public bool EditarTIpo()
        {
            try
            {
                //diamtro 
                //espacimiento
                // tipo

               


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
