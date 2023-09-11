
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
using planta_aux_C.Rutinas;
using VARIOS_C;
using planta_aux_C.Utiles;

namespace planta_aux_C.Elementos
{
    public class NH_ref__Barra : NH_Base__Barra
    {
        #region 0)Propiedades
        public NH_PolilineaRef line_Sup { get; set; }
        public NH_PolilineaRef line_Inf { get; set; }
        #endregion

        #region 1)Constructor
        
        public NH_ref__Barra():base()
        {
            line_Sup = new NH_PolilineaRef();
            line_Inf = new NH_PolilineaRef();
        }

        #endregion

        #region 2)Metodos

         

        #endregion

        internal void rellenarListaPuntos(List<NH_PolilineaRef> list_pto_Rutinas_polilineas)
        {
            Load_CargandoPtoBarraSegunOrientacion(list_pto_Rutinas_polilineas);


            listaPuntos.Add(line_Inf.ptIni);
            listaPuntos.Add(line_Inf.ptFin);
            listaPuntos.Add(line_Sup.ptFin);
            listaPuntos.Add(line_Sup.ptIni);


        }

        private void Load_CargandoPtoBarraSegunOrientacion(List<NH_PolilineaRef> lista)
        {
            var poli1 = lista.ElementAt(0);
            var aux_1 = comunes.coordenada_modificar_Y_manda(poli1.ptIni, poli1.ptFin);

            if (Math.Abs(poli1.angle) < 0.01)// solo si  poli1 compeltamente horizontal
            {
                poli1.ptIni = aux_1[1];
                poli1.ptFin = aux_1[0];
            }
            else
            {
                poli1.ptIni = aux_1[0];
                poli1.ptFin = aux_1[1];
            }
            var poli2 = lista.ElementAt(1);
            var aux_2 = comunes.coordenada_modificar_Y_manda(poli2.ptIni, poli2.ptFin);

            if (Math.Abs(poli2.angle) < 0.01)// solo si  poli1 compeltamente horizontal
            {
                poli2.ptIni = aux_2[1];
                poli2.ptFin = aux_2[0];
            }
            else
            {
                poli2.ptIni = aux_2[0];
                poli2.ptFin = aux_2[1];
            }


            if (!(Intersecciones.InterSection4Point(new Point3d(poli1.ptIni.X, poli1.ptIni.Y, 0),
                                                  new Point3d(poli2.ptIni.X, poli2.ptIni.Y, 0),
                                                  new Point3d(poli1.ptFin.X, poli1.ptFin.Y, 0),
                                                  new Point3d(poli2.ptFin.X, poli2.ptFin.Y, 0), Intersect.OnBothOperands)))
            {


                var aux = comunes.coordenada_modificar(poli1.ptIni, poli2.ptIni);
                line_Inf.ptIni = new Point2d(aux[0].X, aux[0].Y);
                line_Inf.ptFin = new Point2d(aux[1].X, aux[1].Y);

                var aux1 = comunes.coordenada_modificar(poli1.ptFin, poli2.ptFin);
                line_Sup.ptIni = new Point2d(aux1[0].X, aux1[0].Y);
                line_Sup.ptFin = new Point2d(aux1[1].X, aux1[1].Y);
            }
            else
            {
                var aux = comunes.coordenada_modificar(poli1.ptIni, poli2.ptFin);
                line_Inf.ptIni = new Point2d(aux[0].X, aux[0].Y);
                line_Inf.ptFin = new Point2d(aux[1].X, aux[1].Y);

                var aux1 = comunes.coordenada_modificar(poli1.ptFin, poli2.ptIni);
                line_Sup.ptIni = new Point2d(aux1[0].X, aux1[0].Y);
                line_Sup.ptFin = new Point2d(aux1[1].X, aux1[1].Y);
            }
        }
        // calcula las barras superiores he inferiores de refuerzo
        //line_Sup: se utiliza para generar las barras superiores
        //line_Inf: se utiliza para generar las barras Inferiore

    }
}
