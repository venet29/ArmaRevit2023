using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using planta_aux_C.enumera;
using planta_aux_C.Elementos;

namespace planta_aux_C.Elemento_Losa
{
    public class NH_RefereciaLosa
    {
        #region 0)Propiedades
                
        public string nombreLosa { get; set; }
        public float espesor { get; set; }
        public Point2d_nh posicionPelota { get; set; }
        public float LargoHorizontal { get; set; }
        public float LargoVertical { get; set; }
        public string direccionHorizontal { get; set; }
        public string direccionVertical { get; set; }
        public string cuantiaHorizontal { get; set; }
        public string cuantiaVertical { get; set; }
        public ObjectId PoligonoLosa { get; set; }
        public ObjectId PelotaLosa { get; set; }
        public double anguloPelotaLosa { get; set; }
        public int tipoEmpotramiento { get; set; }

        public List<Point2d> ListaPtoLineaDireccionLosa { get; set; }
        public List<Point2d> ListaPtoLineaDireccionPerpLosa { get; set; }

        public List<PtosCrearSuples> ListaSuplesVerticalLosa { get; set; }
        public List<PtosCrearSuples> ListaSuplesHorizontalLosa { get; set; }

        public List<NH_RefereciaLosaParaBarra> ListaPtos_Vertical_Barra { get; set; }
        public List<NH_RefereciaLosaParaBarra> ListaPtos_Horizontal_Barra { get; set; }

        public List<NH_RefereciaLosaParaSuple> ListaPtos_Vertical_Suple { get; set; }
        public List<NH_RefereciaLosaParaSuple> ListaPtos_Horizontal_Suple { get; set; }


        public List<Point2d> ListaVerticesPoligonoLosa { get; set; }
        #endregion

        #region 1) contructor

        public NH_RefereciaLosa(NH_RefereciaLosa_json obj, ObjectId PoligonoLosa, ObjectId PelotaLosa)
        {
            ListaPtoLineaDireccionLosa = new List<Point2d>();
            ListaPtoLineaDireccionPerpLosa = new List<Point2d>();

            ListaSuplesVerticalLosa = new List<PtosCrearSuples>();
            ListaSuplesHorizontalLosa = new List<PtosCrearSuples>();

            ListaPtos_Vertical_Barra = new List<NH_RefereciaLosaParaBarra>();
            ListaPtos_Horizontal_Barra = new List<NH_RefereciaLosaParaBarra>();

            ListaPtos_Vertical_Suple = new List<NH_RefereciaLosaParaSuple>();
            ListaPtos_Horizontal_Suple = new List<NH_RefereciaLosaParaSuple>();


            ListaVerticesPoligonoLosa = new List<Point2d>();
            this.LargoHorizontal = obj.LargoHorizontal;
            this.LargoVertical = obj.LargoVertical;
            this.nombreLosa = obj.nombreLosa;
            this.posicionPelota = obj.posicionPelota;
            this.espesor = obj.espesor;
            this.direccionHorizontal = obj.direccionHorizontal;
            this.direccionVertical = obj.direccionVertical;
            this.cuantiaHorizontal = obj.cuantiaHorizontal;
            this.cuantiaVertical = obj.cuantiaVertical;
            this.PelotaLosa = PelotaLosa;//
            this.PoligonoLosa = PoligonoLosa;//this.anguloPelotaLosa = obj.anguloPelotaLosa;
            this.tipoEmpotramiento = obj.tipoEmpotramiento;

            foreach (var item in obj.ListaPtoLineaDireccionLosa)
            {
                var pto = item.Split(new char[] { ',' });
                ListaPtoLineaDireccionLosa.Add(new Point2d(Convert.ToDouble(pto[0]), Convert.ToDouble(pto[1])));
            }
            foreach (var item in obj.ListaPtoLineaDireccionPerpLosa)
            {
                var pto = item.Split(new char[] { ',' });
                ListaPtoLineaDireccionPerpLosa.Add(new Point2d(Convert.ToDouble(pto[0]), Convert.ToDouble(pto[1])));
            }
            foreach (var item in obj.ListaVerticesPoligonoLosa)
            {
                var pto = item.Split(new char[] { ',' });
                ListaVerticesPoligonoLosa.Add(new Point2d(Convert.ToDouble(pto[0]), Convert.ToDouble(pto[1])));
            }
            //******************************************
            foreach (var item in obj.ListaSuplesVerticalLosa)
            {
                ListaSuplesVerticalLosa.Add(new PtosCrearSuples(item));
            }
            foreach (var item in obj.ListaSuplesHorizontalLosa)
            {
                ListaSuplesHorizontalLosa.Add(new PtosCrearSuples(item));
            }

            foreach (var item in obj.ListaPtos_Vertical_Barra)
            {
                ListaPtos_Vertical_Barra.Add(new NH_RefereciaLosaParaBarra(item));
            }
            foreach (var item in obj.ListaPtos_Horizontal_Barra)
            {
                ListaPtos_Horizontal_Barra.Add(new NH_RefereciaLosaParaBarra(item));
            }



        }

        public NH_RefereciaLosa()
        {
            ListaPtoLineaDireccionLosa = new List<Point2d>();
            ListaPtoLineaDireccionPerpLosa = new List<Point2d>();

            ListaSuplesVerticalLosa = new List<PtosCrearSuples>();
            ListaSuplesHorizontalLosa = new List<PtosCrearSuples>();

            ListaPtos_Vertical_Barra = new List<NH_RefereciaLosaParaBarra>();
            ListaPtos_Horizontal_Barra = new List<NH_RefereciaLosaParaBarra>();

            ListaPtos_Vertical_Suple = new List<NH_RefereciaLosaParaSuple>();
            ListaPtos_Horizontal_Suple = new List<NH_RefereciaLosaParaSuple>();

            ListaVerticesPoligonoLosa = new List<Point2d>();


        }

        #endregion


    }
 
}
