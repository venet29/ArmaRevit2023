using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VARIOS;
using Microsoft.VisualBasic;
using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using Autodesk.Windows;

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using deveAutoCad2018.VARIOS;
using planta_aux_C.Elementos;
using planta_aux_C.Utiles;
using planta_aux_C.Elemento_Losa;
using Newtonsoft.Json;
using System.IO;
using VARIOS_C;

namespace planta_aux_C
{
    public class Formulario
    {

        public List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa { get; set; }
        public List<PtosCrearSuples> ListasSuples_Vertical { get; set; }  // barra en sentido del pelota de losa
        public List<PtosCrearSuples> ListasSuples_Horizontal { get; set; }              // barra perpendicul aa pelota de losa

        public float _largominimo { get; set; }
        public float _largoAhorro_barra { get; set; }
        public float _largoAhorro_recorrido { get; set; }


       // public List<Polyline> ListasPoligonoCircuncribeBarraInferior { get; set; }
       // public List<Polyline> ListasPoligonoCircuncribeSuples { get; set; }

        public List<Point3dCollection> ListasPuntosCircuncribeBarraInferior_H { get; set; }
        public List<Point3dCollection> ListasPuntosCircuncribeBarraInferior_V { get; set; }

        public List<Point3dCollection> ListasPoligonoCircuncribeSuples_H { get; set; }
        public List<Point3dCollection> ListasPoligonoCircuncribeSuples_V { get; set; }


        public Formulario()
        {
            ListaPOlilineaYEsferaLosa = new List<NH_RefereciaLosa>();
            ListasSuples_Horizontal = new List<PtosCrearSuples>();
            ListasSuples_Vertical = new List<PtosCrearSuples>();


          //  ListasPoligonoCircuncribeBarraInferior = new List<Polyline>();
          //  ListasPoligonoCircuncribeSuples = new List<Polyline>();



            ListasPuntosCircuncribeBarraInferior_H = new List<Point3dCollection>();
            ListasPuntosCircuncribeBarraInferior_V = new List<Point3dCollection>();

            ListasPoligonoCircuncribeSuples_H = new List<Point3dCollection>();
            ListasPoligonoCircuncribeSuples_V = new List<Point3dCollection>();



            cargar_ListaPolilineaYEsferaLosa();
            comunes.Create_ALayer("PERIMETROBARRAS", 2); //AMARILLO

        }

        public void cargar_ListaPolilineaYEsferaLosa()
        {

            if (ListaPOlilineaYEsferaLosa== null) return ;
            ListaPOlilineaYEsferaLosa.Clear();
            //var aux =  ListaDatosPoligonoLosa(double largoToleranciaTramo, double porcentajeTramos, bool dibujar_pto_horizontal, bool dibujar_pto_vertical, "SelectAll");
            ListaPOlilineaYEsferaLosa = AnalisisPelotaLosa.ListaDatosPoligonoLosa("SelectAll", 10f, 10.0f, false);
        }

        public bool BuscarEnListasSuples(PtosCrearSuples auxsuple)
        {
            if (auxsuple == null) return false;

            List<PtosCrearSuples> varListaSUple = new List<PtosCrearSuples>();
            varListaSUple.AddRange(ListasSuples_Horizontal);
            varListaSUple.AddRange(ListasSuples_Vertical);

            return varListaSUple.Find(c => (c.nombre1 == auxsuple.nombre1 && c.nombre2 == auxsuple.nombre2) ||
                                          (c.nombre1 == auxsuple.nombre2 && c.nombre2 == auxsuple.nombre1)
                                          ) == null ? true : false;
        }

        public bool Probar_Si_punto_alInterior_Polilinea(Point3d pto, List<Polyline> listaPolyRecorrido)
        {

            BuscarPtoSIDentropolylinea buscarPtoINpoly = new BuscarPtoSIDentropolylinea();
            bool dibujar = true;
            foreach (Polyline poly in listaPolyRecorrido)
            {
                // BUSCA SI PTO ESTA DENTRO DE UNA POLILINEA, SOLO PARA POLILINEA CERRADA SIMPLE
                if (buscarPtoINpoly.IsPointInPoligono(poly, pto))
                    return dibujar == false;
            }


            return dibujar;
        }


        public void ExportardatosParaIngenieria()
        {

            NH_PoligonosDeLosas NH_PoligonoDeLosa_analisis = new NH_PoligonosDeLosas();


     

            //carga toso los elemntos , de seleccion
            NH_PoligonoDeLosa_analisis.ListaParaLosasSeleccionados_AnalisisCompleto(60f, 0.3f, 0.3f, false, false);

            if (NH_PoligonoDeLosa_analisis.ListaPOlilineaYEsferaLosa_selec.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Exportacion de datos no finalizadas", "Mensaje");
                return;
            }


            List<NH_RefereciaLosa_json> NH_RefereciaLosa_json_ = new List<NH_RefereciaLosa_json>();

            foreach (var item in NH_PoligonoDeLosa_analisis.ListaPOlilineaYEsferaLosa_selec)
            {
                NH_RefereciaLosa_json_.Add(new NH_RefereciaLosa_json(item));
            }

            // string ruta = @"..\..\1-noparameters.json";

            string ruta = "";

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.json)|*.json";
            saveFileDialog.FileName = "Dibujo_Losax.json";
            //if (saveFileDialog.ShowDialog())
            //    ruta = saveFileDialog.FileName;
            saveFileDialog.ShowDialog();

            string RutaArchivo = saveFileDialog.FileName;

            if ((RutaArchivo != null) && (RutaArchivo != ""))
            {

                if (Path.GetExtension(RutaArchivo) != ".json")
                {
                    System.Windows.Forms.MessageBox.Show("Ruta de archivo no corresponde o formato incorrecto", "Mensaje");
                    return;
                }
                JsonSerializer serializer;
                serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(RutaArchivo))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, NH_RefereciaLosa_json_);
                    }
                }


                System.Windows.Forms.MessageBox.Show("Exportacion de datos realizada correctamente", "Confirmation");

            }



        }

    }


}
