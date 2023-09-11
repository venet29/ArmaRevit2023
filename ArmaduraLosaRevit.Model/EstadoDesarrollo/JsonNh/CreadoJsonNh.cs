using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Json;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.EstadoDesarrollo.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArmaduraLosaRevit.Model.EstadoDesarrollo.JsonNh
{
    public static class CreadoJsonEStado
    {
        public static string ruta { get; private set; }

        public static bool ExportarAJson(ContenedorProyectos ListaProyectos, string nombreView)
        {
            bool result = false;
            #region Exportar datos




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
                        serializer.Serialize(writer, ListaProyectos);
                    }
                }


                System.Windows.Forms.MessageBox.Show("Exportacion de datos realizada correctamente", "Confirmation");

            }
            #endregion
            result = true;
            return result;
        }
        public static bool ExportarAJson_conRuta(ContenedorProyectos ListaProyectos, string RutaArchivo)
        {
            bool result = false;
            #region Exportar datos

            if (!ValidarDirectory(Path.GetDirectoryName(RutaArchivo))) return false;

            if (Path.GetExtension(RutaArchivo) != ".json")
            {
                System.Windows.Forms.MessageBox.Show("Ruta de archivo no corresponde al formato correcto json", "Mensaje");
                return false;
            }

            JsonSerializer serializer;
            serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(RutaArchivo))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, ListaProyectos);
                }
            }


            System.Windows.Forms.MessageBox.Show("Exportacion de datos realizada correctamente", "Confirmation");
            #endregion
            result = true;
            return result;
        }


        public static ContenedorProyectos LeerArchivoJsonING()
        {

            ContenedorProyectos ListaProyectos_ = new ContenedorProyectos(); ;
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
                return ListaProyectos_;
            }

            return LeerArchivoJsonING_conruta(ruta);
        }

        public static ContenedorProyectos LeerArchivoJsonING_conruta(string ruta)
        {
            ContenedorProyectos items = new ContenedorProyectos();
            try
            {
                if (!ValidarRuta(ruta)) return null;
                using (StreamReader r = new StreamReader(ruta))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<ContenedorProyectos>(json);
                }
                return items;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erroa al exportar datos de archivo:\n{ruta}  ex:{ex.Message}");
                return new ContenedorProyectos();
            }

        }


        public static ContenedorProyectosJson LeerArchivoJsonING2_conruta(string ruta)
        {
            ContenedorProyectosJson items = new ContenedorProyectosJson();
            try
            {

                if (!ValidarRuta(ruta)) return null;
                using (StreamReader r = new StreamReader(ruta))
                {
                    string json = r.ReadToEnd();
                    var items_asd = JsonConvert.DeserializeObject<ContenedorProyectosJson>(json);
                }
                return items;
            }
            catch (Exception ex)

            {
                Util.ErrorMsg($"Erroa al exportar datos de archivo:\n{ruta}   ex:{ex.Message}");
                return new ContenedorProyectosJson();
            }

        }
        private static bool ValidarRuta(string rutaArchivo)
        {
            if (rutaArchivo == "" || (!File.Exists(rutaArchivo)))
            {
                Util.InfoMsg("No se encuentra ruta de bases de datos con la informacion de los proyectos");
                return false;
            }
            else
                return true;
        }
        private static bool ValidarDirectory(string directorio)
        {
            if (ruta == "" || (!Directory.Exists(directorio)))
            {
                Util.InfoMsg("No se encuentra ruta de bases de datos con la informacion de los proyectos");
                return false;
            }
            else
                return true;
        }
    }
}
