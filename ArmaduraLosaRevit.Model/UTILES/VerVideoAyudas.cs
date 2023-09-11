using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class VerVideoAyudas
    {
        private static Dictionary<string, string> listaRutas;

        public static Dictionary<string, string> CrearDiccionarioImagenes()
        {
             listaRutas = new Dictionary<string, string>(20);

            listaRutas.Add("selecBorde", @"\\SERVER-CDV\Dibujo2\Proyectos\PROYECTOS REVIT\PLANTILLAS Y FAMILIAS (ULTIMAS)\videosNH\armadura fund\seleccionarBorde.mp4");
            listaRutas.Add("selecPunto", @"\\SERVER-CDV\Dibujo2\Proyectos\PROYECTOS REVIT\PLANTILLAS Y FAMILIAS (ULTIMAS)\videosNH\armadura fund\seleccionarLinea.mp4");
            listaRutas.Add("AgruparDesAgrupar", @"\\SERVER-CDV\Dibujo2\Proyectos\PROYECTOS REVIT\PLANTILLAS Y FAMILIAS (ULTIMAS)\videosNH\armadura elev\AgruparDesAgrupar\AgruparDesagrupar_texto.mp4");


            return listaRutas;
        }



        public static bool Ejecutar(string NombreVideo)
        {
            if (listaRutas == null)
                CrearDiccionarioImagenes();
            if (listaRutas.Count==0)
                CrearDiccionarioImagenes();

            try
            {
                if (!listaRutas.ContainsKey(NombreVideo))
                {
                    Util.ErrorMsg("No se encontro video ayuda");
                    return false;
                }

                // buscar ruta con nombre 
                string rutaVideo = listaRutas[NombreVideo];
                if (rutaVideo == null)
                {
                    Util.ErrorMsg("No se encontro video ayuda");
                    return false;
                }
                if (rutaVideo == "")
                {
                    Util.ErrorMsg("No se encontro video ayuda");
                    return false;
                }

                if (!File.Exists(rutaVideo))
                {
                    Util.ErrorMsg("No se encontro archivo de video ayuda");
                    return false;
                }

                System.Diagnostics.Process.Start(rutaVideo);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'VerVideoAyudas'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
