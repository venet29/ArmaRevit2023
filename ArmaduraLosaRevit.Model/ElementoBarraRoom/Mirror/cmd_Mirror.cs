using ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar.Model;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Mirror
{
    [Transaction(TransactionMode.Manual)]
    public class Mirror1BArra : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            ManejadorMirrorPathReinformen _ManejadorMirrorPathReinformen = new ManejadorMirrorPathReinformen(uiapp);
            return _ManejadorMirrorPathReinformen.Ejecutar1();
        }
    }


    [Transaction(TransactionMode.Manual)]
    public class MirrorMBArra : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            ManejadorMirrorPathReinformen _ManejadorMirrorPathReinformen = new ManejadorMirrorPathReinformen(uiapp);
            return _ManejadorMirrorPathReinformen.EjecutarMultiples();
        }
    }
}
