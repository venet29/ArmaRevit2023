using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilApiRevit.Archivos
{
    public class CopiarFile
    {
        private string pathServer;

        public CopiarFile()
        {
            pathServer = @"\\Server-cdv\Usuarios\jose.huerta\uribe-55\DES\revit\2020";
        }


        public void copiarServer()
        {

            if (Directory.Exists(pathServer))
            {

            }
        }

        public void CopiarArchivo(string rutaOrigen, string rutaDestino)
        {
            try
            {
                File.Copy(rutaOrigen, rutaDestino, true);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}
