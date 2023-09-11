using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cubicacion.Ayuda
{
    public class AyudaObtenerNombreEjeElev
    {



        public static string ObtenerSufijo(string nombre)
        {
            string result = "";
            if (nombre.ToLower().Contains("ejes"))
            {
                result = EjecutarSUfijo(nombre, "ejes");


            }
            else if (nombre.ToLower().Contains("eje"))
            {
                result = EjecutarSUfijo(nombre, "eje");

            }else
            {
                var nombreSplit = nombre.Split(' ');

                if (nombreSplit[0].ToLower().Contains("elev") && nombreSplit[0].ToLower().Contains("="))
                {
                    result = nombreSplit[1].Trim();
                    result = Regex.Replace(result, @"\s", "");
                }
                else
                {
                    Util.ErrorMsg($"Error al obtener el nombre base de elevacion '{nombre}'.\nCambie nombre base para copiar elevacion en otra  " +
                        $"\nej:'Elevacion Eje x1=x2'" +
                        $"\nej: 'Eje x1=x2'");
                    result = nombre;
                }

            }

            return result;
        }

        private static string EjecutarSUfijo(string nombre,string compara)
        {
            string result;
            int largoComparar = compara.Length;
            var Posicon = nombre.ToLower().IndexOf(compara);
            var final_aux = nombre.Substring(Posicon + largoComparar);


            Posicon = Posicon + largoComparar;

            for (int i = 0; i < final_aux.Length - 1; i++)
            {
                if (final_aux[i].ToString().Trim() != "")
                {
                    Posicon = Posicon  + i;
                    break;
                }

            }

            result = nombre.Substring(Posicon);
            result = Regex.Replace(result, @"\s", "");
            return result;
        }

        //*************************
        public static string ObtenerPrefijo(string nombre)
        {
            string result = "";
            if (nombre.ToLower().Contains("ejes"))
            {
                result = EjecutarPrefijo(nombre, "ejes");


            }
            else if (nombre.ToLower().Contains("eje"))
            {
                result = EjecutarPrefijo(nombre, "eje");

            }
            else
            {
                var nombreSplit=nombre.Split(' ');

                if (nombreSplit.Length==2 &&  nombreSplit[0].ToLower().Contains("elev") && nombreSplit[0].ToLower().Contains("="))
                {
                    result = nombreSplit[0].Trim();
                }
                else
                {
                    Util.ErrorMsg($"Error al obtener el nombre base de elevacion '{nombre}'.\nCambie nombre base para copiar elevacion en otra  " +
                      $"\nej:'Elevacion Eje x1=x2'" +
                      $"\nej:'Eje x1=x2'");

                }

            }

            return result;
        }

        private static string EjecutarPrefijo(string nombre, string compara)
        {
            string result;
            int largoComparar = compara.Length;
            var Posicon = nombre.ToLower().IndexOf(compara);

            result = nombre.Substring(0, Posicon + largoComparar);


            return result;
        }
    }

}
