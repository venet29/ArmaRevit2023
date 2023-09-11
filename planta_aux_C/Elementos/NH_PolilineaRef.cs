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
using planta_aux_C.Utiles;
using planta_aux_C.enumera;
using planta_aux_C.Elementos;
using planta_aux_C;
using planta_aux_C.Elemento_Losa;
using planta_aux_C.RutinasSoloPruebas;
using VARIOS_C;
namespace planta_aux_C.Rutinas
{
    public class NH_PolilineaRef
    {

        public Point3d PuntoSelecionInicialMouse { get; set; }
        public Point3d PuntoSelecionFinalMouse { get; set; }
        public TipoSeleccionMouse TipoSeleccionMouse_ { get; set; }

        public Point2d ptIni { get; set; }
        public Point2d ptFin { get; set; }

        public Point2d ptIni_ext_para_dibujar { get; set; }
        public Point2d ptFin_ext_para_dibujar { get; set; }

        public LineSegment2d linea { get; set; }
        public Polyline poly { get; set; }
        public Line line { get; set; }

        public double angle { get; set; }




        #region 2)metodos

        public void CalcularAngulo()
        {

            if ((ptIni != new Point2d(0, 0)) && (ptFin != new Point2d(0, 0)))
            {

               angle= comunes.coordenada__angulo_p1_p2_losa(ptIni, ptFin);
            }
        }

        #endregion


        public void CalcularAngulo(double angle_)
        {
            if (angle_ > 0)
            { angle = -(Math.PI/2-angle_); }
            else if (angle_ == Math.PI/2)
            { angle = 0; }
            else if (angle_ == 0)
            { angle = Math.PI / 2; }
            else if (angle_ <0)
            { angle = -(-Math.PI/2 - angle_); }
        }
    }
}
