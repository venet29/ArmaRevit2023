using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Servicio;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Assert
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class Cmd_BuscarLosaIncilnada : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            Document _doc = app.ActiveUIDocument.Document;


            //***************************************************************************************************
            //buscar losas al finla (derecha - superior) 
            List<XYZ> listaPtos_ = new List<XYZ>();
            listaPtos_.Add(new XYZ(-102.444230000, 30.183730000, -19.225720000));
            listaPtos_.Add(new XYZ(-102.444230000, -3.018370000, -19.225720000));
            listaPtos_.Add(new XYZ(-63.664700000, -3.018370000, -19.225720000));
            listaPtos_.Add(new XYZ(-63.664700000, 30.183730000, -19.225720000));
            BuscarLosaIncilnada BuscarLosaIncilnada = new BuscarLosaIncilnada(_doc, listaPtos_, 100);

            BuscarLosaIncilnada.obtenerPendienteLosaContiguaFinal(new XYZ(0, 0, -1));
            Util.InfoMsg($"Angulo grado {BuscarLosaIncilnada.angleGRADO} ");

            //****************************************************************************************************
            //buscar losa hacia atras  (izquierda   -- inferoopr)
            List<XYZ> listaPtos_Atras = new List<XYZ>();

            listaPtos_Atras.Add(new XYZ(-37.992130000, -17.290030000, -13.320210000));
            listaPtos_Atras.Add(new XYZ(-37.992130000, -37.500000000, -13.320210000));
            listaPtos_Atras.Add(new XYZ(-20.992130000, -37.500000000, -13.320210000));
            listaPtos_Atras.Add(new XYZ(-20.992130000, -17.290030000, -13.320210000));
            BuscarLosaIncilnada BuscarLosaIncilnada_Atras = new BuscarLosaIncilnada(_doc, listaPtos_Atras, 100);

            BuscarLosaIncilnada_Atras.obtenerPendienteLosaContiguaInicia(new XYZ(0, 0, -1));
            XYZ piniMenosPtoFin = new XYZ(-0.994847194, 0.000000000, -0.101385701);
            Debug.Assert(BuscarLosaIncilnada_Atras.VectorDireccionLosaExternaInclinada.IsAlmostEqualTo(piniMenosPtoFin));

            Util.InfoMsg($"Angulo grado {BuscarLosaIncilnada.angleGRADO} ");

            return Result.Succeeded;

        }


    }


    [TransactionAttribute(TransactionMode.Manual)]

    public class Cmd_BuscarLosaIncilnadaPLamoRef : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            Document _doc = app.ActiveUIDocument.Document;


            //***************************************************************************************************
            //buscar losas al finla (derecha - superior) 
            List<XYZ> listaPtos_ = new List<XYZ>();
            listaPtos_.Add(new XYZ(-102.444230000, 30.183730000, -19.225720000));
            listaPtos_.Add(new XYZ(-102.444230000, -3.018370000, -19.225720000));
            listaPtos_.Add(new XYZ(-63.664700000, -3.018370000, -19.225720000));
            listaPtos_.Add(new XYZ(-63.664700000, 30.183730000, -19.225720000));

            PlanarFace face = null;
            BuscarPtoProyeccionEnLosaInclinada BuscarPtoProyeccionEnLosaInclinada = new BuscarPtoProyeccionEnLosaInclinada(app, face);



            return Result.Succeeded;

        }


    }
}
