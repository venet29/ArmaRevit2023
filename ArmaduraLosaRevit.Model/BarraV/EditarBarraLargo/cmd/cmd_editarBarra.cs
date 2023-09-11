using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.WPFEdB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.cmd
{
  

    [TransactionAttribute(TransactionMode.Manual)]

    public class cmd_editarBarra : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            ManejadorWPF_EditarBarraLargo manejadorWPF_EstriboV = new ManejadorWPF_EditarBarraLargo(commandData);
            return manejadorWPF_EstriboV.Execute();
        }

    }


}
