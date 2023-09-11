using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
using planta_aux_C.Utiles;
using VARIOS_C;

namespace planta_aux_C
{
    public class PerimetroBarraConRecorrido
    {
        #region 0) propiedades
    
        //DOCUMENTACION1
        //0) PROPIEDADES
        public ObjectId PoligonoDeLosa { get; set; }
        public Point3d Pt1 { get; set; }
        public Point3d Pt2 { get; set; }
        public string Direccion { get; set; }

        public Point3d Pt_InicialRecorrido { get; set; }
        public Point3d Pt_FinalRecorrido { get; set; }

        public Point3d Pt_Pini0 { get; set; }
        public Point3d Pt_Pini1 { get; set; }

        public Point3d Pt_Pfin0 { get; set; }
        public Point3d Pt_Pfin1 { get; set; }

        public Point3d Pt_PerimetroIni0 { get; set; }
        public Point3d Pt_PerimetroIni1 { get; set; }

        public Point3d Pt_PerimetroFin0 { get; set; }
        public Point3d Pt_PerimetroFin1 { get; set; }

        public Document Doc { get; set; }
        public float Angulo_losa { get; set; }
        public float LargoRecorrido { get; set; }
     #endregion

        #region  1) CONTRUCTOR

        //1) CONSTRUCTOR
        public PerimetroBarraConRecorrido(ObjectId poligonoDeLosa, Point3d pt1_, Point3d pt2_, string direccion, Document doc, float angulo_losa)
        {
            Direccion = direccion;
            PoligonoDeLosa = poligonoDeLosa;
            Pt1 = pt1_;
            Pt2 = pt2_;
            Doc = doc;
            Angulo_losa = angulo_losa;
        }

        public PerimetroBarraConRecorrido()
        { }
        #endregion

        #region 2) METODOS

         
        #region  rutinas para calcular las putos importantes del recorrido  -- ve5 documentacion 1

        // calculas pos putos de recorriddo inicial y final, asi como tambien los puntos que circunscriben el area abarcada por el recorrido y la barra
        public void CalcularPuntosDeRecorrido(Point3d pt_ref)
        {

            if (pt_ref == null)
            {
                pt_ref = new Point3d((Pt1.X + Pt2.X) / 2, (Pt1.Y + Pt2.Y) / 2, 0);
            }
            float dista_pfin_0, dista_pfin_1, dista_pini_0, dista_pini_1;
            float angulo_segmento = 0.0f; // angulo auxliar extra para barra horizontalk para dibujar recorrido

            if (Direccion == "horizontal_i" || Direccion == "horizontal_d") angulo_segmento = (float)Math.PI / 2;

            if (Pt_Pini1 == new Point3d(0, 0, 0) && Pt_Pfin1 == new Point3d(0, 0, 0)) PtosDeTrasmosQUeSeIntersectaConBarraSUperioreYnferior();

            dista_pfin_0 = (float)Math.Abs(Pt2.DistanceTo(Pt_Pfin0) * Math.Cos(comunes.angulo_entre_pt(Pt2, Pt_Pfin0) - Angulo_losa + angulo_segmento));
            dista_pfin_1 = (float)Math.Abs(Pt2.DistanceTo(Pt_Pfin1) * Math.Cos(comunes.angulo_entre_pt(Pt2, Pt_Pfin1) - Angulo_losa - angulo_segmento));

            dista_pini_0 = (float)Math.Abs(Pt1.DistanceTo(Pt_Pini0) * Math.Cos(comunes.angulo_entre_pt(Pt1, Pt_Pini0) - Angulo_losa + angulo_segmento));
            dista_pini_1 = (float)Math.Abs(Pt1.DistanceTo(Pt_Pini1) * Math.Cos(comunes.angulo_entre_pt(Pt1, Pt_Pini1) - Angulo_losa - angulo_segmento));


            //angle = angulo pelota losa

            int factor_sentido = -1;

            float angle_ini = -(Angulo_losa + angulo_segmento);
            Pt_FinalRecorrido = new Point3d(pt_ref.X + factor_sentido * Math.Min(dista_pini_1, dista_pfin_1) * Math.Cos(angle_ini),
                                            pt_ref.Y + -factor_sentido * Math.Min(dista_pini_1, dista_pfin_1) * Math.Sin(angle_ini), 0);


            Pt_PerimetroIni0 = new Point3d(Pt1.X + factor_sentido * Math.Min(dista_pini_1, dista_pfin_1) * Math.Cos(angle_ini),
                                           Pt1.Y + -factor_sentido * Math.Min(dista_pini_1, dista_pfin_1) * Math.Sin(angle_ini), 0);


            Pt_PerimetroFin0 = new Point3d(Pt2.X + factor_sentido * Math.Min(dista_pini_1, dista_pfin_1) * Math.Cos(angle_ini),
                                           Pt2.Y + -factor_sentido * Math.Min(dista_pini_1, dista_pfin_1) * Math.Sin(angle_ini), 0);
            //****************************************************************************************************
            //angle = angulo pelota losa
            float angle_fin = Angulo_losa + angulo_segmento;
            Pt_InicialRecorrido = new Point3d(pt_ref.X + Math.Min(dista_pfin_0, dista_pini_0) * Math.Cos(angle_fin),
                                              pt_ref.Y + Math.Min(dista_pfin_0, dista_pini_0) * Math.Sin(angle_fin), 0);

            Pt_PerimetroIni1 = new Point3d(Pt1.X + Math.Min(dista_pfin_0, dista_pini_0) * Math.Cos(angle_fin),
                                           Pt1.Y + Math.Min(dista_pfin_0, dista_pini_0) * Math.Sin(angle_fin), 0);

            Pt_PerimetroFin1 = new Point3d(Pt2.X + Math.Min(dista_pfin_0, dista_pini_0) * Math.Cos(angle_fin),
                                           Pt2.Y + Math.Min(dista_pfin_0, dista_pini_0) * Math.Sin(angle_fin), 0);

            LargoRecorrido = (float)Pt_InicialRecorrido.DistanceTo(Pt_FinalRecorrido);

        }

        //  se obtienes los puntos (inicial y final) de los tramos de la polilinea con las que la barra se intersecta, al principio y al final  
        public void PtosDeTrasmosQUeSeIntersectaConBarraSUperioreYnferior()
        {
            Point3d[] pini = new Point3d[2]; // pini :  PTOS  DE  EXTREMOS DE SECCIONA DE POLIGONO DE LOSA CON EL QUE INTERSA PTO 'pt1' de barra
            Point3d[] pfin = new Point3d[2]; // pfin :  PTOS  DE  EXTREMOS DE SECCIONA DE POLIGONO DE LOSA CON EL QUE INTERSA PTO 'pt2' de barra

            // pt1 Y pt2 : PTO DE BARRA 

            // resultado coordenada_modificar_ (PT1_ (MAYOR) PTO_2(MENOR) ))
            pini = RutinasPolilinea.pto_inter3(Doc, Pt1, PoligonoDeLosa, Direccion);
            pfin = RutinasPolilinea.pto_inter3(Doc, Pt2, PoligonoDeLosa, Direccion);

            if (!(pini.Length == 2 & pfin.Length == 2))
                return;

            double anguloBarraDibujada = comunes.angulo_entre_pt(Pt1, Pt2);

            double anguloPini = comunes.angulo_entre_pt(Pt1, pini[0]);
            double anguloPfin = comunes.angulo_entre_pt(Pt1, pfin[0]);


            // angle = angulo pelota losa
            // TRAMOS DE SECCION DE POLIGONO DE LOSA CON EL QUE INTERSA PTO 'pt1' Y 'pt2' de barra


            if (pini[0].IsEqualTo(pfin[1]))
            {
                Point3d[] pxxx_aux = new Point3d[2];

                if ((pini[0].X < pini[1].X & pfin[0].X > pfin[1].X))
                {
                    pxxx_aux[0] = new Point3d(pfin[0].X, pfin[0].Y, 0);
                    pxxx_aux[1] = new Point3d(pfin[1].X, pfin[1].Y, 0);
                    pfin[0] = new Point3d(pxxx_aux[1].X, pxxx_aux[1].Y, 0);
                    pfin[1] = new Point3d(pxxx_aux[0].X, pxxx_aux[0].Y, 0);
                }
                else if ((pini[0].X > pini[1].X & pfin[0].X < pfin[1].X))
                {
                    pxxx_aux[0] = new Point3d(pini[0].X, pini[0].Y, 0);
                    pxxx_aux[1] = new Point3d(pini[1].X, pini[1].Y, 0);
                    pini[0] = new Point3d(pxxx_aux[1].X, pxxx_aux[1].Y, 0);
                    pini[1] = new Point3d(pxxx_aux[0].X, pxxx_aux[0].Y, 0);
                }
                else if (pini[0].X > pini[1].X & pfin[0].X > pfin[1].X)
                {
                    if (pini[0].Y < pini[1].Y)
                    {
                        pxxx_aux[0] = new Point3d(pfin[0].X, pfin[0].Y, 0);
                        pxxx_aux[1] = new Point3d(pfin[1].X, pfin[1].Y, 0);
                        pfin[0] = new Point3d(pxxx_aux[1].X, pxxx_aux[1].Y, 0);
                        pfin[1] = new Point3d(pxxx_aux[0].X, pxxx_aux[0].Y, 0);
                    }
                    else if (pfin[0].Y < pfin[1].Y)
                    {
                        pxxx_aux[0] = new Point3d(pini[0].X, pini[0].Y, 0);
                        pxxx_aux[1] = new Point3d(pini[1].X, pini[1].Y, 0);
                        pini[0] = new Point3d(pxxx_aux[1].X, pxxx_aux[1].Y, 0);
                        pini[1] = new Point3d(pxxx_aux[0].X, pxxx_aux[0].Y, 0);
                    }
                }
                else if ((pini[0].X < pini[1].X & pfin[0].X < pfin[1].X))
                {
                    //  Autodesk.Windows.MessageBox.Show("Caso no programado -->  PLANTA> dibujar_dimension2");
                    return;
                }
            }
            else if (pini[1].IsEqualTo(pfin[0]))
            {
                Point3d[] pxxx_aux = new Point3d[2];

                pxxx_aux[0] = new Point3d(pfin[0].X, pfin[0].Y, 0);
                pxxx_aux[1] = new Point3d(pfin[1].X, pfin[1].Y, 0);
                pfin[0] = new Point3d(pxxx_aux[1].X, pxxx_aux[1].Y, 0);
                pfin[1] = new Point3d(pxxx_aux[0].X, pxxx_aux[0].Y, 0);
            }


            //****************************
            Pt_Pini0 = pini[0];
            Pt_Pini1 = pini[1];


            Pt_Pfin0 = pfin[0];
            Pt_Pfin1 = pfin[1];

        }
       
        #endregion
     
        // se utiliza para detarminar  si un punto esta dentro de una polilinea, codigo simple para poligonos , todos los vertices con angulos menor a 90°
        private bool IsPointInsidePolyline(Polyline pl, Point3d pt)
        {
            double anglulo1 = comunes.Angle3Ptos(pl.GetPoint3dAt(0), pl.GetPoint3dAt(1), pt);
            double anglulo2 = comunes.Angle3Ptos(pl.GetPoint3dAt(1), pl.GetPoint3dAt(2), pt);
            double anglulo3 = comunes.Angle3Ptos(pl.GetPoint3dAt(2), pl.GetPoint3dAt(3), pt);
            double anglulo4 = 0;
            if (pl.NumberOfVertices == 5)
            {
                anglulo4 = comunes.Angle3Ptos(pl.GetPoint3dAt(3), pl.GetPoint3dAt(4), pt);
            }
            else if (pl.NumberOfVertices == 4)
            {
                anglulo4 = comunes.Angle3Ptos(pl.GetPoint3dAt(3), pl.GetPoint3dAt(0), pt);
            }
            else
            {
                return false;
            }
            double total = anglulo1 + anglulo2 + anglulo3 + anglulo4;

            if (Math.Abs(total - 2 * Math.PI) < 0.01)
                return true;
            else
                return false;


        }
        
        #endregion

    }
}
