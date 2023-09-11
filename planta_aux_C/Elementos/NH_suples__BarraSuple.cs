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
namespace planta_aux_C.Elementos
{
    public class NH_suples__BarraSuple
    {
        //0) propiedades
        public Polyline Barra { get; set; }

        //1) constructores
        public NH_suples__BarraSuple()
        {
            Barra = new Polyline();
        }

        //2) metodos






    }
}
