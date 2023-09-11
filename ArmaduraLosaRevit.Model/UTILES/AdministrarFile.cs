using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class AdministrarFile
    {
        public static string NombreArchivo { get; set; }
        public static string RutaArchivo { get;  set; }

        public static bool LeerArchivoJsonING()
        {
            RutaArchivo = "";  // @"J:\vlous\union.net\PLANTA\XXX) barra -XX-XX-XXXX -MODIFCANDO\barra\barra\1-noparameters_deIngeneria.json";
            try
            {      
                System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                openFileDialog1.Filter = "json files (*.json) |*.json";
                openFileDialog1.FilterIndex = 1;
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    RutaArchivo = openFileDialog1.FileName;
                    NombreArchivo = Path.GetFileName(RutaArchivo);
                    return true;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
            return false;
        }

    }
}
