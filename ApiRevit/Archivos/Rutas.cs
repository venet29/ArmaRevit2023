using Autodesk.Revit.UI;
using CrearRibbon.Help;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CrearRibbon.Servicios
{

    /// <summary>
    /// RUTINAN QUE  "nameCarpetaReferencia" como referecia para obtener la version de dll mas alta
    /// </summary>
    public class Rutas
    {
        #region 0) PROPIEDADES

        //string ruta_addn = @"\\Server-cdv\usuarios2\jose.huerta\uribe-55\DES\revit\";
        string ruta_addn = "";// @"j:\_revit\PROYECTOS_REVIT\CrearRibbon\dll_cargar\revit\";

        Result EstadoOperacion;
        /// <summary>
        /// Lista que se comprueba que existan dentro del assembler, solo verificacionm
        /// </summary>
        string[] ListaClasesVerificarExisten;
        //string[] Lista_ImageFolderName;

        // const string ruta_addn = @"J:\_revit\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit\bin\Debug\";

        public string _imageFolderNh { get; internal set; }
        public string _imageFolder { get; set; }

       
        public string nombreCarpetaDll { get; set; }
        public string rutaCarpetaDll { get; set; }
        public string rutaArchivoDll { get; set; }

        public string nameCarpetaReferencia { get; set; }

        #endregion

        #region 1) CONTRUCTORES
        //COntrutos 
        public Rutas(string _version,string nameCarpetaReferencia, string[] ListaClasesVerificarExisten)
        {


            //_imageFolder = _imageFolderName;
            //_imageFolderNh = _imageFolderNameBarrasLosas;

            InfoSystema infoSystema = new InfoSystema();

            string versionREviT = Util.ObtenerVErsion(_version);

            if (infoSystema.disco.Trim() == "Disco Duro: 1838_9180_0122_0001_001B_448B_44B6_73D3.")
            { ruta_addn = @"j:\_revit\PROYECTOS_REVIT\CrearRibbon\dll_cargar\revit\"+ versionREviT; }
            else
            { ruta_addn = @"\\Server-cdv\usuarios2\jose.huerta\uribe-55\DES\revit\"+ versionREviT; }


            this.nameCarpetaReferencia = nameCarpetaReferencia;
            this.ListaClasesVerificarExisten = ListaClasesVerificarExisten;

            cargaRutas();
            cargarAssembley();
        }
        #endregion


        #region 2) METODOS

        #region 2.1) cargar dll
        public void cargaRutas()
        {
            /// obtner carpeta
            cargaRutas_buscar_addin();
            //obtener
            cargaRutas_ObteneRutaDll();

            if (!File.Exists(rutaArchivoDll))
            {
                TaskDialog.Show("UIRibbon", "External command assembly no encontrado: " + nameCarpetaReferencia);
                EstadoOperacion =Result.Failed;
            }

            EstadoOperacion = Result.Succeeded;
        }

        /// <summary>
        /// obtiene la ruta del utltimo dll agregado
        /// </summary>
        /// <param name="name"> nombre la carpeta a buscar, busca la mayor </param>
        /// <returns></returns>
        private string cargaRutas_buscar_addin()
        {
            string result = "";
            DataTable tablalist_addin = new DataTable();
            tablalist_addin.Clear();
            int i = 1;
            if (tablalist_addin.Columns.Count <= 0)
            {
                tablalist_addin.Columns.Add("iD");
                tablalist_addin.Columns.Add("Nombre");
            }
            // Dim fileNames = My.Computer.FileSystem.GetFiles(folderPath, FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
            if (Directory.Exists(ruta_addn))
            {
                foreach (string FileName in Directory.GetFiles(ruta_addn, "*dll", SearchOption.TopDirectoryOnly))
                {
                    // MessageBox.Show(FileName)
                    tablalist_addin.Rows.Add(i, FileName.Substring(ruta_addn.Length));
                    i = i + 1;
                }
            }

            DataRow[] p_25drift_ROW = tablalist_addin.Select("Nombre LIKE '%" + nameCarpetaReferencia + "%'", "Nombre DESC");

            if (p_25drift_ROW.Length > 0) nombreCarpetaDll = p_25drift_ROW[0][1].ToString().Replace(".dll", "");

            return result;
        }


        /// <summary>
        /// obtiene la ruta de ubicacion del dll
        /// </summary>
        private void cargaRutas_ObteneRutaDll()
        {
            rutaCarpetaDll = ruta_addn + nombreCarpetaDll + @"\";
            rutaArchivoDll = Path.Combine(rutaCarpetaDll, nombreCarpetaDll + ".dll");
        }


        #endregion



        #region helper
        /// <summary>
        /// Display error message
        /// </summary>
        /// <param name="msg">Message to display</param>
        public void ErrorMsg(string msg)
        {
            System.Diagnostics.Debug.WriteLine("RvtSamples: " + msg);
            Autodesk.Revit.UI.TaskDialog.Show("RvtSamples", msg, Autodesk.Revit.UI.TaskDialogCommonButtons.Ok);
        }
        #endregion


     
        #endregion


    }
}
