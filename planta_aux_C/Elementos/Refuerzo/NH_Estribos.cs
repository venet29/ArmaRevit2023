
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
using planta_aux_C;

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using planta_aux_C.enumera;
using planta_aux_C.Rutinas;
using planta_aux_C.Utiles;
using VARIOS_C;

namespace planta_aux_C.Elementos.Refuerzo
{
    public class NH_Estribos
    {

        #region 0) propiedades

        public string cantidadEstribo { get; set; }
        public int diametroEstribo { get; set; }
        public int espaciamientoEstribo { get; set; }

        public string Cuantia { get; set; }
        public OrientacionEstribo Orientacion { get; set; }
        public Point3d posiconTexto_superior { get; set; }
        public Point3d posiconTexto_inferior { get; set; }

        public Point2d p1_ { get; set; }
        public Point2d p2_ { get; set; }
        public Point2d p3_ { get; set; }
        public Point2d p4_ { get; set; }

        public string layerMalla { get; set; }
        public string layerTexto { get; set; }
        public double angle { get; set; }
        public double angle_estribo { get; set; }

        public string listaPuntnos { get; set; }

        #endregion

        #region 1) constructores

        public NH_Estribos(List<NH_PolilineaRef> lista, string Cuantia)
        {
            layerMalla = "CESTRIBO_E";
            layerTexto = "ESTRIBO";
            var poli1 = lista.ElementAt(0);
            var poli2 = lista.ElementAt(1);
            this.Cuantia = Cuantia;

            if (!(Intersecciones.InterSection4Point(new Point3d(poli1.ptIni.X, poli1.ptIni.Y, 0),
                                                  new Point3d(poli2.ptIni.X, poli2.ptIni.Y, 0),
                                                  new Point3d(poli1.ptFin.X, poli1.ptFin.Y, 0),
                                                  new Point3d(poli2.ptFin.X, poli2.ptFin.Y, 0), Intersect.OnBothOperands)))
            {
                p1_ = poli1.ptIni;
                p2_ = poli1.ptFin;

                p3_ = poli2.ptFin;
                p4_ = poli2.ptIni;
            }
            else
            {
                p1_ = poli1.ptIni;
                p2_ = poli1.ptFin;

                p3_ = poli2.ptIni;
                p4_ = poli2.ptFin;
            }
              listaPuntnos = "(" + p1_.X + "," + p1_.Y + ")-(" + p2_.X + "," + p2_.Y + ")-(" + p3_.X + "," + p3_.Y + ")-(" + p4_.X + "," + p4_.Y + ")";

              Point3d pt1_, pt2_;
              if (poli1.linea != null)
              {pt1_= new Point3d(poli1.linea.MidPoint.X,poli1.linea.MidPoint.Y,0);}
              else
              { pt1_ = poli1.line.GetPointAtDist(poli1.line.EndParam*0.5); }

              if (poli2.linea != null)
              {pt2_ = new Point3d(poli2.linea.MidPoint.X, poli2.linea.MidPoint.Y, 0); }
              else
              { pt2_ = poli2.line.GetPointAtDist(poli2.line.EndParam * 0.5); }// EndParam= muetsra largo total de la barra   ---GetPointAtDist(poli2.line.EndParam * 0.5): muestra el punto ala mitad de la linea
            
              angle = comunes.coordenada__angulo_p1_p2_losa(pt1_, pt2_); 
                
              PosicionTexto();



        }

        public NH_Estribos(NH_Refuerzo NH_Refuerzo_, string cantidadEstribo, int diametroEstribo, int espaciamientoEstribo)
        {
            layerMalla = "CESTRIBO_E";
            layerTexto = "ESTRIBO";

            //this.Cuantia = Cuantia;

            p1_ = NH_Refuerzo_.lineaReferenciaInferior_ultima.ptIni;
            p2_ = NH_Refuerzo_.lineaReferenciaInferior_ultima.ptFin;
            p3_ =  NH_Refuerzo_.lineaReferenciaSuperior_ultima.ptFin;
            p4_ = NH_Refuerzo_.lineaReferenciaSuperior_ultima.ptIni;

            listaPuntnos = "(" + p1_.X + "," + p1_.Y + ")-(" + p2_.X + "," + p2_.Y + ")-(" + p3_.X + "," + p3_.Y + ")-(" + p4_.X + "," + p4_.Y + ")";
            
            Point3d pt1_, pt2_;
            pt1_ = new Point3d((p1_.X + p4_.X) / 2, (p1_.Y + p4_.Y) / 2, 0);
            pt2_ = new Point3d((p2_.X + p3_.X) / 2, (p2_.Y + p3_.Y) / 2, 0);

          
    
            angle = comunes.coordenada__angulo_p1_p2_losa(pt1_, pt2_);

            if (angle >= 0)
            { angle_estribo = angle - Math.PI / 2; }
            else
            { angle_estribo = angle + Math.PI / 2; }


            this.cantidadEstribo = cantidadEstribo;
            this.diametroEstribo = diametroEstribo;
            this.espaciamientoEstribo = espaciamientoEstribo;
            this.Cuantia = cantidadEstribo + "%%c" + diametroEstribo + "a" + espaciamientoEstribo;
             PosicionTexto();

        }

        #endregion

        #region 2) metodos

        private void PosicionTexto()
        {
          
            if (Math.Abs(angle) < 0.01)
            {
                Orientacion = OrientacionEstribo.Horizontal;
                int despla = 10;
                if (p1_.Y < p4_.Y)
                {
                    posiconTexto_superior = new Point3d((p4_.X + p3_.X) / 2 + Math.Sin(angle) * despla, (p4_.Y + p3_.Y) / 2 + Math.Cos(angle) * despla, 0);
                    posiconTexto_inferior = new Point3d((p1_.X + p2_.X) / 2 - Math.Sin(angle) * despla, (p1_.Y + p2_.Y) / 2 - Math.Cos(angle) * despla, 0);
                }
                else
                { 
                    posiconTexto_superior = new Point3d((p1_.X + p2_.X) / 2 + Math.Sin(angle) * despla, (p1_.Y + p2_.Y) / 2 + Math.Cos(angle) * despla, 0);
                    posiconTexto_superior = new Point3d((p4_.X + p3_.X) / 2 - Math.Sin(angle) * despla, (p4_.Y + p3_.Y) / 2 - Math.Cos(angle) * despla, 0);
                
                }

            }
            else if (Math.Abs(angle - Math.PI/2) < 0.01)
            {
                Orientacion = OrientacionEstribo.Vertical;
                int despla = 10;
                posiconTexto_superior = new Point3d((p4_.X + p3_.X) / 2 - Math.Sin(angle) * despla, (p4_.Y + p3_.Y) / 2 + Math.Cos(angle) * despla, 0);
                posiconTexto_inferior = new Point3d((p1_.X + p2_.X) / 2 + Math.Sin(angle) * despla, (p1_.Y + p2_.Y) / 2 + Math.Cos(angle) * despla, 0);

                //if (p1_.Y < p4_.Y)
                //{
                //    ;
                //}
                //else
                //{ posiconTexto = new Point3d((p1_.X + p2_.X) / 2 + Math.Sin(angle) * despla, (p1_.Y + p2_.Y) / 2 + Math.cos(angle) * despla, 0); }

            }
            else
            {
                Orientacion = OrientacionEstribo.Oblicuo;
                int despla = 10;

                if (angle>=0)
                {
                    posiconTexto_superior = new Point3d((p4_.X + p3_.X) / 2 - Math.Cos(angle) * despla, (p4_.Y + p3_.Y) / 2 + Math.Sin(angle) * despla, 0);
                    posiconTexto_inferior= new Point3d((p1_.X + p2_.X) / 2 + Math.Cos(angle) * despla, (p1_.Y + p2_.Y) / 2 - Math.Sin(angle) * despla, 0);
                }
                else
                {
                    posiconTexto_superior = new Point3d((p4_.X + p3_.X) / 2 + Math.Cos(Math.Abs(angle)) * despla, (p4_.Y + p3_.Y) / 2 + Math.Sin(Math.Abs(angle)) * despla, 0);
                    posiconTexto_inferior = new Point3d((p1_.X + p2_.X) / 2 - Math.Cos(Math.Abs(angle)) * despla, (p1_.Y + p2_.Y) / 2 - Math.Sin(Math.Abs(angle)) * despla, 0);
                }
            }
        }

        #endregion
    }
}
