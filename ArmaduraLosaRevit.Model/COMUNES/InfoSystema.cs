using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;

namespace ArmaduraLosaRevit.Model.COMUNES
{
    public class InfoSystema
    {
        /*
         
         Dim infoSystema As InfoSystema = New InfoSystema(ruta2, "ProgramaControl")
        infoSystema.copiarInformacion()
        If (infoSystema.disco = " Disco Duro: 1838_9180_0122_0001_001B_448B_44B6_73D3.") Then

        End If
             
             */
        //Dim infoSystema As InfoSystema = New InfoSystema(ruta2, "CalculoCorte")
        //infoSystema.copiarInformacion()
        //   Public ruta2 As String = "\\Server-cdv\Usuarios\jose.huerta\programas\usuariosArmadura.txt"
        #region 0)propieades
        public string disco { get; set; }
        public string usuario { get; set; }
        public string macPc { get;  set; }
        public string ruta { get; set; }
        public string caso { get; set; }

        #endregion
        #region 1)Contructor

        public InfoSystema(string ruta, string caso)
        {
            this.ruta = ruta;
            this.caso = caso;
            InfoSistem2();

        }
        public InfoSystema()
        {

            InfoSistem2();

        }

        #endregion

        #region 2) metodos

        #region funciones sobre txt  -- escribir leer


        public void EscribirEnTxt(List<string> lista, string ruta)
        {

            try
            {
                // Write the string array to a new file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(ruta))
                {
                    foreach (string line in lista)
                        outputFile.WriteLine(line);
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }
        public async void WriteTextAsync(string text)
        {
            // Set a variable to the My Documents path.
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the text asynchronously to a new file named "WriteTextAsync.txt".
            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\WriteTextAsync.txt"))
            {
                await outputFile.WriteAsync(text);
            }
        }

        #endregion

        #region Metodos Sitema

        public void InfoSistem()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("SerialNumber: {0}", queryObj["SerialNumber"]);
                    Console.WriteLine("Signature: {0}", queryObj["Signature"]);
                }
            }
            catch (ManagementException e)
            {
                TaskDialog.Show("Revit", e.Message);
            }
        }

        public void copiarInformacion()
        {
            DateTime fecha2 = DateTime.Now;

            //const string fic = @"\\Server-cdv\Usuarios\jose.huerta\programas1\elev.txt";
            string texto = fecha2 + " -  " + Environment.UserName + " - Disco :  " + disco + "  ---  Rutina : " + caso;

            string directori = Path.GetDirectoryName(ruta);

            if (!Directory.Exists(directori)) return;


            if (!File.Exists(ruta))
                File.Create(ruta).Dispose();


            if (File.Exists(ruta))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta, true);
                sw.WriteLine(texto);
                sw.Close();
            }
        }
        //GetMacAddress().ToString()
        public  PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        public void InfoSistem2()
        {



            // OBTENER INFO DISCO DURO
            string consultaSQLArquitectura = "SELECT * FROM Win32_Processor";
            ManagementObjectSearcher objArquitectura = new ManagementObjectSearcher(consultaSQLArquitectura);
            ManagementObject serialDD = new ManagementObject(@"Win32_PhysicalMedia='\\.\PHYSICALDRIVE0'");
            disco = " Disco Duro: " + serialDD.GetPropertyValue("SerialNumber").ToString();
            usuario = Environment.UserName;
            macPc = GetMacAddress().ToString();

        }
        #endregion 


        public static string ObtenerRutaDeFamilias()
        {
            InfoSystema infoSystema = new InfoSystema();
            if (infoSystema.disco == " Disco Duro: 1838_9180_0122_0001_001B_448B_44B6_73D3." || infoSystema.disco.Trim() == "170306454715")
            {
                if (Directory.Exists(@"J:\_revit\FAMILY\"))
                    return @"J:\_revit\FAMILY\";
                else
                    return ConstNH.rutaFamilias;
            }

            else
            {
                return ConstNH.rutaFamilias;
            }
        }

        #endregion
    }
}
