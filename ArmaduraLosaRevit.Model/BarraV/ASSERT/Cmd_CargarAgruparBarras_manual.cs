using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.BarraV.Agrupar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{
    //[Transaction(TransactionMode.Manual)]
  //  [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CargarAgruparBarras_manual : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ManejadorAgrupar ManejadorAgrupar = new ManejadorAgrupar(commandData.Application);
            ManejadorAgrupar.Ejecutar();
            return Result.Succeeded;
        }
    }
}
