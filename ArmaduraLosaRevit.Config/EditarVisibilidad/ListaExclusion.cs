using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad
{
   public class ListaExclusion
    {

        public static List<string> ListaExclusionLosa()
        {
            List<string> listaExclusionLosa = new List<string>();

            listaExclusionLosa.Add("Areas");
            listaExclusionLosa.Add("Color Fill");

            return listaExclusionLosa;
        }
    }
}
