using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using planta_aux_C.Elementos;
using Autodesk.AutoCAD.Geometry;
using System.IO;

namespace planta_aux_C.Elemento_Losa
{
    public class NH_RefereciaLosa_json
    {
        #region 0)propiedades


        public string nombreLosa { get; set; }
        public float espesor { get; set; }
        public Point2d_nh posicionPelota { get; set; }

        public float LargoHorizontal { get; set; }
        public float LargoVertical { get; set; }

        public string direccionHorizontal { get; set; }
        public string direccionVertical { get; set; }

        public string cuantiaHorizontal { get; set; }
        public string cuantiaVertical { get; set; }
        public string HandlePoligonoLosa { get; set; }
        public string HandlePelotaLosa { get; set; }
        public double anguloPelotaLosa { get; set; }
        public int tipoEmpotramiento { get; set; }


        public List<string> ListaPtoLineaDireccionLosa { get; set; }
        public List<string> ListaPtoLineaDireccionPerpLosa { get; set; }

        public List<string> ListaSuplesVerticalLosa { get; set; }
        public List<string> ListaSuplesHorizontalLosa { get; set; }

        public List<string> ListaPtos_Vertical_Barra { get; set; }
        public List<string> ListaPtos_Horizontal_Barra { get; set; }

        public List<string> ListaVerticesPoligonoLosa { get; set; }

        #endregion

        #region 1)Contructor


        public NH_RefereciaLosa_json()
        {
            ListaPtoLineaDireccionLosa = new List<string>();
            ListaPtoLineaDireccionPerpLosa = new List<string>();

            ListaSuplesVerticalLosa = new List<string>();
            ListaSuplesHorizontalLosa = new List<string>();

            ListaPtos_Vertical_Barra = new List<string>();
            ListaPtos_Horizontal_Barra = new List<string>();


            ListaVerticesPoligonoLosa = new List<string>();
        }
        public NH_RefereciaLosa_json(StreamReader obj)
        {
            ListaPtoLineaDireccionLosa = new List<string>();
            ListaPtoLineaDireccionPerpLosa = new List<string>();

            ListaSuplesVerticalLosa = new List<string>();
            ListaSuplesHorizontalLosa = new List<string>();

            ListaPtos_Vertical_Barra = new List<string>();
            ListaPtos_Horizontal_Barra = new List<string>();


            ListaVerticesPoligonoLosa = new List<string>();
        }
        public NH_RefereciaLosa_json(NH_RefereciaLosa obj)
        {
            ListaPtoLineaDireccionLosa = new List<string>();
            ListaPtoLineaDireccionPerpLosa = new List<string>();

            ListaSuplesVerticalLosa = new List<string>();
            ListaSuplesHorizontalLosa = new List<string>();

            ListaPtos_Vertical_Barra = new List<string>();
            ListaPtos_Horizontal_Barra = new List<string>();

            ListaVerticesPoligonoLosa = new List<string>();
            this.LargoHorizontal = obj.LargoHorizontal;
            this.LargoVertical = obj.LargoVertical;
            this.nombreLosa = obj.nombreLosa;
            this.espesor = obj.espesor;
            this.posicionPelota = obj.posicionPelota;
            this.direccionHorizontal = obj.direccionHorizontal;
            this.direccionVertical = obj.direccionVertical;
            this.cuantiaHorizontal = obj.cuantiaHorizontal;
            this.cuantiaVertical = obj.cuantiaVertical;
            this.HandlePoligonoLosa = obj.PoligonoLosa.Handle.ToString();
            this.HandlePelotaLosa = obj.PelotaLosa.Handle.ToString();
            this.anguloPelotaLosa = obj.anguloPelotaLosa;
            this.tipoEmpotramiento = obj.tipoEmpotramiento;

            foreach (var item in obj.ListaPtoLineaDireccionLosa)
            {
                ListaPtoLineaDireccionLosa.Add(item.X.ToString() + "," + item.Y.ToString());
            }
            foreach (var item in obj.ListaPtoLineaDireccionPerpLosa)
            {
                ListaPtoLineaDireccionPerpLosa.Add(item.X.ToString() + "," + item.Y.ToString());
            }
            foreach (var item in obj.ListaVerticesPoligonoLosa)
            {
                ListaVerticesPoligonoLosa.Add(item.X.ToString() + "," + item.Y.ToString());
            }
            //*****************************************************************************
            foreach (var item in obj.ListaSuplesVerticalLosa)
            {
                ListaSuplesVerticalLosa.Add(ObtenerStringSuple(item));
            }

            foreach (var item in obj.ListaSuplesHorizontalLosa)
            {
                ListaSuplesHorizontalLosa.Add(ObtenerStringSuple(item));
            }
            foreach (var item in obj.ListaPtos_Vertical_Barra)
            {
                this.ListaPtos_Vertical_Barra.Add(ObtenerStringBarra(item));
            }

            foreach (var item in obj.ListaPtos_Horizontal_Barra)
            {
                this.ListaPtos_Horizontal_Barra.Add(ObtenerStringBarra(item));
            }

        }


        #endregion

        #region 2)MEtodo

        private string ObtenerStringBarra(NH_RefereciaLosaParaBarra item)
        {
            return item.nombreLosa.ToString() + "," + item.espesor.ToString() + "," + item.anguloPelotaLosa.ToString() + "," +
                                                      item.PosicionPelotaLosa.X.ToString() + ":" + item.PosicionPelotaLosa.Y.ToString() + "," +
                                                      item.PosicionPto_Barra.X.ToString() + ":" + item.PosicionPto_Barra.Y.ToString() + "," +
                                                      item.tipo + "," + item.PoligonoLosa.Handle.ToString() + "," + item.PelotaLosa.Handle.ToString(); 
           // return "";
        }
        private string ObtenerStringSuple(PtosCrearSuples item)
        {
            return item.nombre1.ToString() + "," + item.nombre2.ToString() + "," +
                                                        item.pto1.X.ToString() + ":" + item.pto1.Y.ToString() + "," +
                                                        item.pto2.X.ToString() + ":" + item.pto2.Y.ToString() + "," +
                                                        item.Plosa1.tipo + "," + item.diametro + "," + item.espaciamiento + "," + item.cuantia;
        }
        #endregion

    }


}
