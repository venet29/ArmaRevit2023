using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using planta_aux_C.Elemento_Losa;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using planta_aux_C.Utiles;
using Newtonsoft.Json;
using System.IO;



//[assembly: CommandClass(typeof(planta_aux_C.RutinasSoloPruebas.Prueba_ListaDatosPoligonoLosa))]


namespace planta_aux_C.RutinasSoloPruebas
{
    public class Prueba_ListaDatosPoligonoLosa
    {

        [CommandMethod("Prueba_ListaDatosPoligonoLosa_")]
        public void Prueba_ListaDatosPoligonoLosa_()
        {
            var lista = AnalisisPelotaLosa.ListaDatosPoligonoLosa("GetSelection", 10, 10, true);
        }


        [CommandMethod("Prueba_ListaDatosPoligonoLosaconSuples_")]
        public void Prueba_ListaDatosPoligonoLosaconSuples_()
        {

            NH_PoligonosDeLosas NH_PoligonoDeLosa_analisis = new NH_PoligonosDeLosas();

            //carga toso los elemntos , de seleccion
            NH_PoligonoDeLosa_analisis.ListaParaLosasSeleccionados_AnalisisCompleto(60f, 0.3f,0.3f, false, false);


            List<NH_RefereciaLosa_json> NH_RefereciaLosa_json_ = new List<NH_RefereciaLosa_json>();

            foreach (var item in NH_PoligonoDeLosa_analisis.ListaPOlilineaYEsferaLosa_selec)
            {
                NH_RefereciaLosa_json_.Add(new NH_RefereciaLosa_json(item));
            }

           // string ruta = @"..\..\1-noparameters.json";

            string ruta = "";

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.json)|*.json";
            saveFileDialog.FileName = "Losax.json";
            //if (saveFileDialog.ShowDialog())
            //    ruta = saveFileDialog.FileName;
            saveFileDialog.ShowDialog();

            string RutaArchivo = saveFileDialog.FileName;

            if ((RutaArchivo != null) && (RutaArchivo != ""))
            {

                if (File.Exists(RutaArchivo) && Path.GetExtension(RutaArchivo) == ".json")
                {
                    System.Windows.Forms.MessageBox.Show("Ruta de archivo no corresponde o formato incorrecto", "Mensaje"); 
                           return;
                }
                JsonSerializer serializer;
                serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(ruta))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, NH_RefereciaLosa_json_);
                    }
                }

            }



        }


        [CommandMethod("Prueba_LeerArchivoJson_")]
        public void Prueba_LeerArchivoJson_()
        {
          //  string ruta = @"..\..\1-1-noparameters_deIngeneria.json";
            string ruta = @"J:\vlous\union.net\PLANTA\XXX) barra -XX-XX-XXXX -MODIFCANDO\barra\barra\1-noparameters_deIngeneria.json";

            List<NH_RefereciaLosa_json> items;
            using (StreamReader r = new StreamReader(ruta))
            {
                string json = r.ReadToEnd();
             items = JsonConvert.DeserializeObject<List<NH_RefereciaLosa_json>>(json);
            }


            NH_PoligonosDeLosas NH_PoligonoDeLosa_analisis = new NH_PoligonosDeLosas();
            NH_PoligonoDeLosa_analisis.Recargar();

            List<NH_RefereciaLosa> ListaLosas = new List<NH_RefereciaLosa>();

            foreach (var item in items)
            {
                var poligonoLosa = NH_PoligonoDeLosa_analisis.ListaPOlilineaYEsferaLosa.
                                                            Where(c => c.PoligonoLosa.Handle.ToString()==item.HandlePoligonoLosa).Select(m=> m.PoligonoLosa).FirstOrDefault();
                var pelotaLosa = NH_PoligonoDeLosa_analisis.ListaPOlilineaYEsferaLosa.
                                                            Where(c => c.PelotaLosa.Handle.ToString() == item.HandlePelotaLosa).Select(m => m.PelotaLosa).FirstOrDefault();

                if (pelotaLosa == null || poligonoLosa == null || pelotaLosa.IsValid == false || poligonoLosa.IsValid == false) continue;
                NH_PoligonoDeLosa_analisis.DaTosPelotaLosa(ref pelotaLosa, item);
                ListaLosas.Add(new NH_RefereciaLosa(item, poligonoLosa, pelotaLosa));
            }




            NH_PoligonoDeLosa_analisis.ListaPOlilineaYEsferaLosa_selec.AddRange(ListaLosas);


            NH_PoligonoDeLosa_analisis.ListaPtos_Horizontal_Barra.AddRange(ListaLosas.SelectMany(c => c.ListaPtos_Horizontal_Barra).ToList());
            NH_PoligonoDeLosa_analisis.ListaPtos_Vertical_Barra.AddRange(ListaLosas.SelectMany(c => c.ListaPtos_Vertical_Barra).ToList());
            NH_PoligonoDeLosa_analisis.ListasSuples_Horizontal.AddRange(ListaLosas.SelectMany(c => c.ListaSuplesHorizontalLosa).ToList());
            NH_PoligonoDeLosa_analisis.ListasSuples_Vertical.AddRange(ListaLosas.SelectMany(c => c.ListaSuplesVerticalLosa).ToList());

            NH_PoligonoDeLosa_analisis.ListasSuples_Todos.AddRange(NH_PoligonoDeLosa_analisis.ListasSuples_Horizontal);
            NH_PoligonoDeLosa_analisis.ListasSuples_Todos.AddRange(NH_PoligonoDeLosa_analisis.ListasSuples_Vertical);

       

        }


        [CommandMethod("Prueba_ListVertices")]
        public void ListVertices()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            PromptEntityResult per = ed.GetEntity("Select a polyline");

            if (per.Status == PromptStatus.OK)
            {
                var lista = RutinasPolilinea.ListVerticesPolinea(per.ObjectId);


            }

        }

        #region PRUEBA Prueba_Serielizacion




        [CommandMethod("Prueba_Serielizacion")]
        public void Prueba_Serielizacion()
        {


            Author author = new Author();
            author.name = "Xavier Morera";
            author.since = new DateTime(2014, 01, 15);

            JsonSerializer serializer;
            serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(@"..\..\1-noparameters.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, author);
                }
            }



        }
        public class Author
        {
            public string name { get; set; }
            public string[] courses { get; set; }
            public DateTime since { get; set; }
            public bool happy { get; set; }
            public object issues { get; set; }
            public Car car { get; set; }
        }

        public class Car
        {
            public string model { get; set; }
            public int year { get; set; }
        }
        #endregion
    }


}

