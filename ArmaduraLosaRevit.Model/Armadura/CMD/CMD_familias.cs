using Autodesk.Revit.Attributes;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura.CMD
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_CargarFamiliasHook : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;

            TipoRebarHookType.CrearHookIniciales(_doc);

            return Result.Succeeded;

        }
    }


  //  [Transaction(TransactionMode.Manual)]
  //  [Regeneration(RegenerationOption.Manual)]
    public class Cmd_11111111_TiposPathReinTagsEnView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;

            TiposRoomTagEnView.M1_GetFamilySymbol_nh(new ElementId(1410980), _doc,_doc.ActiveView.Id);

            return Result.Succeeded;

        }
    }
}
