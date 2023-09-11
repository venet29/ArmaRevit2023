using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class LogNH
    {

        public static StringBuilder _sbuilder { get; set; } = new StringBuilder();

        public static void Nombrdll(Assembly e)
        {

            string exeAssembly = e.ManifestModule.Name;
            Util.InfoMsg(exeAssembly + "  contador numeroValid:" + ManejadorDatos.ObteneContador());

        }
        public static void Limpiar_sbuilder()
        {
            _sbuilder = new StringBuilder();//Definimos un listado de cadenas de caracteres o texto

        }
        public static void Agregar_registro(string rutexto)
        {
            if (_sbuilder == null)
                _sbuilder = new StringBuilder();//Definimos un listado de cadenas de caracteres o texto
            _sbuilder.AppendLine(rutexto);
        }

        public static void Guardar_registro(string nombreArchivo="", string ruta = "")
        {
            if (_sbuilder == null)
                Limpiar_sbuilder();

            nombreArchivo = NOmbreArchivo(nombreArchivo);

            Guardar_registro_ConStringBuilder(_sbuilder, (ruta == "" ? ConstNH.rutaLogNh : ruta), nombreArchivo);

        }

        private static string NOmbreArchivo(string nombreArchivo)=> (nombreArchivo == "" ? DateTime.Now.ToString("MM_dd_yyyy Hmmss") : nombreArchivo);

        // ejemplo uso LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
        public static void Guardar_registro_ConStringBuilder(StringBuilder sb, string ruta = "", string _nombreArchivo = "")
        {       
            _nombreArchivo = NOmbreArchivo(_nombreArchivo);

            if (ruta == "")
                ruta = Util.InfoRutadll();

            string newRuta = ruta + @"\" + _nombreArchivo + ".txt";
            //   string ruta = @"\\Server-cdv\Usuarios\jose.huerta\programas\general- programa reporte C.txt";
            string texto = Environment.UserName + "--> " + _nombreArchivo + " -  ";

            //ruta = @"D:\Documents\Escritorio\1\general- programa reporte C.txt";
            try
            {
                if (!File.Exists(ruta))
                {
                    // Create a file to write to.
                    System.IO.File.WriteAllText(newRuta, sb.ToString());
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        // ejemplo uso LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
        public static void guardar_registro_txt(string sb, string ruta, string _nombreArchivo="")
        {
            _nombreArchivo = NOmbreArchivo(_nombreArchivo);

            string path = ruta + @"\" + _nombreArchivo + ".txt";

            //ruta = @"D:\Documents\Escritorio\1\general- programa reporte C.txt";
            try
            {
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(sb);
                    }
                }
                else
                {
                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(sb);
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        internal static void MostarVentaConDatos()
        {
            Util.InfoMsg(_sbuilder.ToString());
        }
    }
}
