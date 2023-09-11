using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Json;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArmaduraLosaRevit.Model
{
    public static class CreadoJson
    {
        public static string ruta { get; private set; }

        public static bool ExportarAJson(List<ReferenciaRoom>  ListaNH_RefereciaLosa_selec , string nombreView)
        {
            bool result = false;
            #region Exportar datos



            List<RefereciaRoom_json> NH_RefereciaLosnna_json_ = new List<RefereciaRoom_json>();
            foreach (var item in ListaNH_RefereciaLosa_selec)
            {
                NH_RefereciaLosnna_json_.Add(RefereciaRoom_json.ctro_NH_RefereciaLosa_json_enCM(item));
            }


            // string ruta = @"..\..\1-noparameters.json";

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.json)|*.json";
            saveFileDialog.FileName = nombreView + ".json";
            //if (saveFileDialog.ShowDialog())
            //    ruta = saveFileDialog.FileName;
            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return false;

            string RutaArchivo = saveFileDialog.FileName;

            if ((RutaArchivo != null) && (RutaArchivo != ""))
            {

                if (Path.GetExtension(RutaArchivo) != ".json")
                {
                    System.Windows.Forms.MessageBox.Show("Ruta de archivo no corresponde o formato incorrecto", "Mensaje");
                    return false;
                }
      
                JsonSerializer serializer;
                serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(RutaArchivo))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, NH_RefereciaLosnna_json_);
                    }
                }


                System.Windows.Forms.MessageBox.Show("Exportacion de datos realizada correctamente", "Confirmation");

            }
            #endregion
            result = true;
            return result;
        }



        public static List<IntervalosBarraAutoDtoIMPORTAR> LeerArchivoJsonING()
        {

            List<IntervalosBarraAutoDtoIMPORTAR> items = new List<IntervalosBarraAutoDtoIMPORTAR>(); ; 
            //  string ruta = @"..\..\1-1-noparameters_deIngeneria.json";
             ruta = "";  // @"J:\vlous\union.net\PLANTA\XXX) barra -XX-XX-XXXX -MODIFCANDO\barra\barra\1-noparameters_deIngeneria.json";
            string destino = "";
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "json files (*.json) |*.json";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ruta = openFileDialog1.FileName;
                destino = Path.GetFileName(ruta);
            }
            else
            {
                return items;
            }

            return LeerArchivoJsonING_conruta(ruta);
        }

        public static List<IntervalosBarraAutoDtoIMPORTAR> LeerArchivoJsonING_conruta(string ruta)
        {
            List<IntervalosBarraAutoDtoIMPORTAR> items = new List<IntervalosBarraAutoDtoIMPORTAR>(); 
            try
            {

                if (ruta == "") return null;
                using (StreamReader r = new StreamReader(ruta))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<IntervalosBarraAutoDtoIMPORTAR>>(json);
                }
                return items;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg($"Erroa al exportar datos de archivo:\n{ruta}");
              return  new List<IntervalosBarraAutoDtoIMPORTAR>();
            }
         
        }


        internal static Tabla05_Objeto_con_todas_las_Tablas LeerArchivoJsonING_conruta_Tabla05_Objeto_con_todas_las_Tablas(string ruta)
        {
            Tabla05_Objeto_con_todas_las_Tablas items = new Tabla05_Objeto_con_todas_las_Tablas();
            try
            {

                if (ruta == "") return null;
                using (StreamReader r = new StreamReader(ruta))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<Tabla05_Objeto_con_todas_las_Tablas>(json);
                }
                return items;
            }
            catch (Exception)
            {

                return new Tabla05_Objeto_con_todas_las_Tablas();
            }

        }
    }
}
