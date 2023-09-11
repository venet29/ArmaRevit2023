using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using planta_aux_C.Elementos;
using planta_aux_C.enumera;
using System.IO;
using Newtonsoft.Json;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Windows.Forms;


namespace planta_aux_C.Elemento_Losa
{
    public class NH_PoligonosDeLosas
    {
        #region 0)Propiedades
        public List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa_selec { get; set; }
        public List<NH_RefereciaLosa> ListaPOlilineaYEsferaLosa { get; set; }

        public List<PtosCrearSuples> ListasSuples_Todos { get; set; }

        public List<PtosCrearSuples> ListasSuples_Vertical { get; set; }  // barra en sentido del pelota de losa
        public List<PtosCrearSuples> ListasSuples_Horizontal { get; set; }


        public List<NH_RefereciaLosaParaBarra> ListaPtos_Vertical_Barra { get; set; }
        public List<NH_RefereciaLosaParaBarra> ListaPtos_Horizontal_Barra { get; set; }

        #endregion

        #region 1)Constructores


        public NH_PoligonosDeLosas()
        {
            ListaPOlilineaYEsferaLosa_selec = new List<NH_RefereciaLosa>();
            ListaPOlilineaYEsferaLosa = new List<NH_RefereciaLosa>();
            ListasSuples_Vertical = new List<PtosCrearSuples>();
            ListasSuples_Horizontal = new List<PtosCrearSuples>();

            ListaPtos_Vertical_Barra = new List<NH_RefereciaLosaParaBarra>();
            ListaPtos_Horizontal_Barra = new List<NH_RefereciaLosaParaBarra>();

            ListasSuples_Todos = new List<PtosCrearSuples>();
        }
        #endregion

        #region 2)Metodos

        public void Recargar()
        {
            if (ListaPOlilineaYEsferaLosa!= null)
            {
                ListaPOlilineaYEsferaLosa.Clear();
                ListaPOlilineaYEsferaLosa = AnalisisPelotaLosa.ListaDatosPoligonoLosa("SelectAll", 10f, 10.0f, false);
            }
        }

        //porcentajeUbicacion = 0.5
        public void ListaConPuntosParaSuples(float largominimo, float porcentajeUbicacion, bool dibujar_pto_horizontal, bool dibujar_pto_vertical)
        {

            if (AnalisisPelotaLosa.ListaPtos_Vertical_Suples == null && AnalisisPelotaLosa.ListaPtos_Horizontal_Suples == null)
            {
                AnalisisPelotaLosa.ListaPtos_Vertical_Suples = new List<NH_RefereciaLosaParaSuple>();
                AnalisisPelotaLosa.ListaPtos_Vertical_Suples = new List<NH_RefereciaLosaParaSuple>();
                AnalisisPelotaLosa.ListaCoordenadasDibujarBarrasAutomatico(largominimo, porcentajeUbicacion, dibujar_pto_horizontal, dibujar_pto_vertical); 
            }


            // ---------------------------------------------------------------------------------------------------------------------------------------------
            // VERTICAL   generar suples verticales
            ListasSuples_Vertical.Clear();
            foreach (NH_RefereciaLosaParaSuple refLosaSuple in AnalisisPelotaLosa.ListaPtos_Vertical_Suples)
            {
                BuscarPolyLosaContigua.SingleSuples = null;
                BuscarPolyLosaContigua.BuscaSiExistePoligonoLosaContigua(refLosaSuple.PoligonoLosa, refLosaSuple.PosicionPto_suple, "vertical_a", ListaPOlilineaYEsferaLosa, largominimo, largominimo, tipoDeBarra.suple);
                if (BuscarPolyLosaContigua.SingleSuples != null)
                {
                    BuscarPolyLosaContigua.SingleSuples.Plosa1 = refLosaSuple;
                    ListasSuples_Vertical.Add(BuscarPolyLosaContigua.SingleSuples);
                }
                BuscarPolyLosaContigua.SingleSuples = null;
            }



            // --------------------------------------------------------------------------------------------------------------------------------------------------
            // HORIZONTAL  generar suples horizontal



            ListasSuples_Horizontal.Clear();
            foreach (NH_RefereciaLosaParaSuple refLosaSuple in AnalisisPelotaLosa.ListaPtos_Horizontal_Suples)
            {
                BuscarPolyLosaContigua.SingleSuples = null;
                BuscarPolyLosaContigua.BuscaSiExistePoligonoLosaContigua(refLosaSuple.PoligonoLosa, refLosaSuple.PosicionPto_suple, "horizontal_i", ListaPOlilineaYEsferaLosa, largominimo, largominimo, tipoDeBarra.suple);
                if (BuscarPolyLosaContigua.SingleSuples != null)
                {
                    BuscarPolyLosaContigua.SingleSuples.Plosa1 = refLosaSuple;
                    ListasSuples_Horizontal.Add(BuscarPolyLosaContigua.SingleSuples); }
                BuscarPolyLosaContigua.SingleSuples = null;
            }


        }


        // calcula todos los datos de la losa, datos losa, largos minimos, suples y tipo de losa
        public void ListaParaLosasSeleccionados_AnalisisCompleto(float largominimo, float porcentajeTramosBarraInferior,float porcentajeTramosBarraSuple , bool dibujar_pto_horizontal, bool dibujar_pto_vertical)
        {
            //a)crea listas con losas y sis datos (seleccionados)
             ListaPOlilineaYEsferaLosa_selec = AnalisisPelotaLosa.ListaDatosPoligonoLosa("GetSelection", 10, 10, false);


             if (ListaPOlilineaYEsferaLosa_selec.Count == 0)
             {
                 System.Windows.Forms.MessageBox.Show("Por algun motivo no se puedo Agregar referecias de losa  a seleccion: \n Posibles errore \n - Error layer Referencia losa y poligono de losas \n - No agrupados o mas de 1 grupo por referencia de losa ");
                 return;
             }

            //b) obtener largos minimos de cada losas
             AnalisisPelotaLosa.ListaCoordenadasDibujarBarrasAutomatico(largominimo,
                                                                       porcentajeTramosBarraInferior, porcentajeTramosBarraSuple,
                                                                        dibujar_pto_horizontal, dibujar_pto_vertical, ListaPOlilineaYEsferaLosa_selec);

            if (ListaPOlilineaYEsferaLosa.Count==0)
                  Recargar();


            //c)Obtener Suples  - ( obtienen todos los suples de todas las pelotas seleccionados)
            ListaConPuntosParaSuples(largominimo, porcentajeTramosBarraSuple, dibujar_pto_horizontal, dibujar_pto_vertical);


  
            // ---------------------------------------------------------------------------------------------------------------------------------------------

            ListaPtos_Vertical_Barra.AddRange(AnalisisPelotaLosa.ListaPtos_Vertical_Barra);
            ListaPtos_Horizontal_Barra.AddRange(AnalisisPelotaLosa.ListaPtos_Horizontal_Barra);


            ListasSuples_Todos.AddRange(ListasSuples_Vertical);
            ListasSuples_Todos.AddRange(ListasSuples_Horizontal);
            // VERTICAL   generar suples verticales
            // agerga los suples a cada referecnia de losa  --- ()
            foreach (var item in ListaPOlilineaYEsferaLosa_selec)
            {
                //-lista suples horizontales
                var ListaSuplesHori = ListasSuples_Todos.Where(c => c.Plosa1.PelotaLosa == item.PelotaLosa && (c.tipo == UbicacionLosa.Izquierda || c.tipo == UbicacionLosa.Derecha)).ToList();

                item.ListaSuplesHorizontalLosa.Clear();
                if (ListaSuplesHori.Count > 0)
                { item.ListaSuplesHorizontalLosa.AddRange(ListaSuplesHori); }             


                //-lista suples verticales 
                var ListaSuplesVert = ListasSuples_Todos.Where(c => c.Plosa1.PelotaLosa == item.PelotaLosa && (c.tipo == UbicacionLosa.Superior || c.tipo == UbicacionLosa.Inferior)).ToList();
                item.ListaSuplesVerticalLosa.Clear();
                if (ListaSuplesVert.Count > 0)
                { item.ListaSuplesVerticalLosa.AddRange(ListaSuplesVert); }


                //-determinar tipo de losas
                var aux = ListasSuples_Todos.Where(c => c.Plosa1.PelotaLosa == item.PelotaLosa).Select(m => m.Plosa1.tipo).Distinct().ToList();
                if (aux.Count == 0)
                {
                    item.tipoEmpotramiento = 1;
                }
                else if (aux.Count == 1)
                {
                    if ((aux[0] == UbicacionLosa.Derecha) || (aux[0] == UbicacionLosa.Izquierda))
                    { item.tipoEmpotramiento = 3; }
                    else if ((aux[0] == UbicacionLosa.Superior) || (aux[0] == UbicacionLosa.Inferior))
                    { item.tipoEmpotramiento = 2; }
                }
                else if (aux.Count == 2)
                {

                    if (aux.Contains(UbicacionLosa.Derecha) && aux.Contains(UbicacionLosa.Izquierda))
                    { item.tipoEmpotramiento = 5; }
                    else if (aux.Contains(UbicacionLosa.Superior) && aux.Contains(UbicacionLosa.Inferior))
                    { item.tipoEmpotramiento = 4; }
                    else 
                    { item.tipoEmpotramiento = 6; }
              
                }
                else if (aux.Count == 3)
                {

                    if (aux.Contains(UbicacionLosa.Derecha) && aux.Contains(UbicacionLosa.Izquierda))
                    { item.tipoEmpotramiento = 8; }
                    else if (aux.Contains(UbicacionLosa.Superior) && aux.Contains(UbicacionLosa.Inferior))
                    { item.tipoEmpotramiento = 7; }
                }
                else if (aux.Count == 4)
                {
                    item.tipoEmpotramiento = 9;
                }
            }

        }

        public void Prueba_LeerArchivoJson()
        {
            //  string ruta = @"..\..\1-1-noparameters_deIngeneria.json";
            string ruta ="";  // @"J:\vlous\union.net\PLANTA\XXX) barra -XX-XX-XXXX -MODIFCANDO\barra\barra\1-noparameters_deIngeneria.json";
            string destino = "";
              OpenFileDialog openFileDialog1 = new OpenFileDialog();
              openFileDialog1.Filter = "json files (*.json) |*.json";
              openFileDialog1.FilterIndex = 1;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 ruta = openFileDialog1.FileName;
                 destino = Path.GetFileName(ruta);
            }



            Recargar();
            List<NH_RefereciaLosa_json> items;
            if (ruta == "") return;
            using (StreamReader r = new StreamReader(ruta))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<NH_RefereciaLosa_json>>(json);
            }


           
            List<NH_RefereciaLosa> ListaLosas = new List<NH_RefereciaLosa>();

            foreach (var item in items)
            {
                //ANALISIS POLIGONO LOSA
                var poligonoLosa = ListaPOlilineaYEsferaLosa.
                                            Where(c => c.PoligonoLosa.Handle.ToString() == item.HandlePoligonoLosa).Select(m => m.PoligonoLosa).FirstOrDefault();
                if (poligonoLosa.IsValid == false && item.posicionPelota!= null)
                {
                    poligonoLosa = ListaPOlilineaYEsferaLosa.
                                               Where(c => Math.Abs(c.posicionPelota.X - item.posicionPelota.X) < 5 && Math.Abs(c.posicionPelota.Y - item.posicionPelota.Y) < 5).
                                               Select(m => m.PoligonoLosa).FirstOrDefault();
                }
                else if (poligonoLosa.IsValid == false &&  item.posicionPelota == null)
                {
                    System.Windows.Forms.MessageBox.Show("Losa " + item.nombreLosa + " sin posicion de punto ubicacion", "Mensaje");
                }


                // ANALISIS PELOTA DE LOSA
                var pelotaLosa = ListaPOlilineaYEsferaLosa.
                                            Where(c => c.PelotaLosa.Handle.ToString() == item.HandlePelotaLosa).Select(m => m.PelotaLosa).FirstOrDefault();

                if (pelotaLosa.IsValid == false && item.posicionPelota != null)
                {
                    pelotaLosa = ListaPOlilineaYEsferaLosa.
                                               Where(c => Math.Abs(c.posicionPelota.X - item.posicionPelota.X) < 5 && Math.Abs(c.posicionPelota.Y - item.posicionPelota.Y) < 5).
                                               Select(m => m.PelotaLosa).FirstOrDefault();
                }

                if (pelotaLosa == null || poligonoLosa == null || pelotaLosa.IsValid == false || poligonoLosa.IsValid == false) continue;
                DaTosPelotaLosa(ref pelotaLosa, item);


                ListaLosas.Add(new NH_RefereciaLosa(item, poligonoLosa, pelotaLosa));
            }




            
            ListaPOlilineaYEsferaLosa_selec.AddRange(ListaLosas);


            ListaPtos_Horizontal_Barra.AddRange(ListaLosas.SelectMany(c => c.ListaPtos_Horizontal_Barra).ToList());
            ListaPtos_Vertical_Barra.AddRange(ListaLosas.SelectMany(c => c.ListaPtos_Vertical_Barra).ToList());
            ListasSuples_Horizontal.AddRange(ListaLosas.SelectMany(c => c.ListaSuplesHorizontalLosa).ToList());
            ListasSuples_Vertical.AddRange(ListaLosas.SelectMany(c => c.ListaSuplesVerticalLosa).ToList());

            ListasSuples_Todos.AddRange(ListasSuples_Horizontal);
            ListasSuples_Todos.AddRange(ListasSuples_Vertical);



        }


     



        public string[] DaTosPelotaLosa(ref ObjectId idObj_, NH_RefereciaLosa_json item)
        {
            Document Doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database DB = Doc.Database;
            Editor Ed = Doc.Editor;

            string[] datos = new string[2];
            using (Transaction tr = DB.TransactionManager.StartTransaction())
            {

                if (idObj_ != null)
                {


                    BlockReference blkRef = (BlockReference)tr.GetObject(idObj_, OpenMode.ForWrite);

                    AttributeCollection attCol = blkRef.AttributeCollection;


                    foreach (ObjectId attId in attCol)
                    {
                        AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForWrite);
                        //if (attRef.Tag == "E")
                        //   attRef.TextString = (item.espesor * 100.0f).ToString();
                        switch (attRef.Tag)
                        {
                            case "E":
                                attRef.TextString = item.espesor .ToString();
                                break;
                            case "NUMERO":
                                attRef.TextString = item.nombreLosa;
                                break;
                            case "CUAHOR":
                                attRef.TextString = item.cuantiaHorizontal;
                                break;
                            case "CUAVER":
                                attRef.TextString = item.cuantiaVertical;
                                break;
                            case "DIRHOR":
                                attRef.TextString = item.direccionHorizontal;
                                break;
                            case "DIRVER":
                                attRef.TextString = item.direccionVertical;
                                break;
                            default:
                                break;
                        }
                    }


                }
                tr.Commit();
            }
            return datos;
        }


        #endregion

    }



}
