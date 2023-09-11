using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Dimensiones
{
    [Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class CrearDimension : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {

      
                CreadorDimensiones EditarPathReinMouse =
                                    new CreadorDimensiones(commandData.Application.ActiveUIDocument.Document,
                                    new XYZ(-53.58,24.1, 26.5419947506562), 
                                    new XYZ(-45.58, 24.1, 26.5419947506562), "COTA 50 (J.D.)");
            EditarPathReinMouse.Crear_conTrans();
            return Result.Succeeded;
        }
    }


    
}
