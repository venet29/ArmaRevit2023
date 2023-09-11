using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;

namespace ArmaduraLosaRevit.Model.EditarPath
{



   // [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class SeleccionarPathOpciones_PAth_Pto_PickObject : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            EditarPathReinMouse_ExtederPathApunto _EditarPathReinMouse_ExtederPathApunto = new EditarPathReinMouse_ExtederPathApunto(commandData.Application);
            return _EditarPathReinMouse_ExtederPathApunto.M1_ExtederPathApunto();

        }

   
    }


  //  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class SeleccionarPathOpciones_Path_Path_PickObject : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            // Nota Extiende 1 path al 2 path
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            EditarPathReinMouse ExtenderPathBAse = new EditarPathReinMouse(commandData.Application);
            return ExtenderPathBAse.M2_ExtenderPathaPath();

        }

    
    }




  //  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class SeleccionarFormularioPath : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)

            EditarPathReinManejador3 EditarPathReinManejador = new EditarPathReinManejador3(commandData.Application);
            EditarPathReinManejador.Ejecutar();

      

            return Result.Succeeded;
        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class SeleccionarPathOpciones_PickPoint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)

            ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Centers | ObjectSnapTypes.Points;
            //selecciona un objeto floor
            XYZ referen;
            try
            {
                referen = uidoc.Selection.PickPoint(snapTypes, "Element:Seleccionar Barra para traslapar");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                referen = null;
            }

            return Result.Succeeded;
        }
    }







}
