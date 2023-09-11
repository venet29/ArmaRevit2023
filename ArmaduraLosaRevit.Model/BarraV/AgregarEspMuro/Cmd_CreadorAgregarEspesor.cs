#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Elemento_Losa;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.AgregarEspMuro;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.AgregarEspMuro
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CreadorAgregarEspesor : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;


            // Level 2 example criteria
            ManejadorAgregarEspesor _ManejadorAgregarEspesor = new ManejadorAgregarEspesor(uiapp);
            _ManejadorAgregarEspesor.Ejecutar1View();
            return Result.Succeeded;
        }


    }


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CreadorAgregarEspesorALlView : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;


            // Level 2 example criteria
            ManejadorAgregarEspesor _ManejadorAgregarEspesor = new ManejadorAgregarEspesor(uiapp);
            _ManejadorAgregarEspesor.EjecutarAllView();
            return Result.Succeeded;
        }


    }
}
