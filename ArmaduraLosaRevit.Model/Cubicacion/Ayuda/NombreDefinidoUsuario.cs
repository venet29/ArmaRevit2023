using ArmaduraLosaRevit.Model.Cubicacion.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cubicacion.Ayuda
{
   public class NombreDefinidoUsuario
    {
        public static string nivelModificado { get; private set; }

        public static double OrdeElevacion { get; private set; }

        public static bool ObtenerNombreDefinidoUsuario(string nivel, List<LevelDTO> lista_Level_Habilitados)
        {
            try
            {
                if (nivel == null) return false; ;
                if (lista_Level_Habilitados == null) return false;

                var auxNivel = lista_Level_Habilitados.Where(c => c.Nombre == nivel).FirstOrDefault();
                if (auxNivel != null)
                    nivelModificado = auxNivel.Nombre_cub;
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        public static bool ObtenerOrdenPorElevacion(string nivel, List<LevelDTO> lista_Level_Habilitados)
        {
            try
            {
                if (nivel == null) return false; ;
                if (lista_Level_Habilitados == null) return false;

                var auxNivel = lista_Level_Habilitados.Where(c => c.Nombre == nivel).FirstOrDefault();
                if (auxNivel != null)
                    OrdeElevacion = auxNivel.Elevacion;
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
