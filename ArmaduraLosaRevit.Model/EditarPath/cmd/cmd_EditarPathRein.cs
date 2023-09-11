using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.EditarPath.cmd
{


  //  [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AlargarPathReinDere : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(commandData.Application);
            return EditarPathReinMouse.M3_EjecutarExtenderPath( DireccionEdicionPathRein.Derecha, 5);
        }
    }


   // [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class AlargarPathReinIzq : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(commandData.Application);
            return EditarPathReinMouse.M3_EjecutarExtenderPath(DireccionEdicionPathRein.Izquierda, 5);
        }

    }


  //  [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AlargarPathReinSuperior : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(commandData.Application);
            return EditarPathReinMouse.M3_EjecutarExtenderPath(DireccionEdicionPathRein.Superior, 5);
        }

    }


   // [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class AlargarPathReinAbajo : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(commandData.Application);
            return EditarPathReinMouse.M3_EjecutarExtenderPath(DireccionEdicionPathRein.Inferior, 5);
        }

    }





}
