using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class Cmd_ObtenerZPara4ptosPath : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            Document _doc = app.ActiveUIDocument.Document;

            List<XYZ> listaPtos_ = new List<XYZ>();
            listaPtos_.Add( new XYZ(-63.664700000, 14.107610000, -23.654860000));
            listaPtos_.Add(new XYZ(-63.664700000, -3.018370000, -23.654860000));
            listaPtos_.Add(new XYZ(-37.171920000, -3.018370000, -23.654860000));
            listaPtos_.Add(new XYZ(-37.171920000, 14.107610000, -23.654860000));


            int idn = 841579;
            Floor floor =  (Floor)_doc.GetElement(new ElementId(idn));
            if (floor == null) return Result.Failed;
            PlanarFace face_ = floor.ObtenerPLanarFAce_superior(); 
            if (face_ == null) return Result.Failed;

            ObtenerZPara4ptosPathInclinada ObtenerZPara4ptosPath_Aux = new ObtenerZPara4ptosPathInclinada(commandData.Application,listaPtos_, face_);
            List<XYZ> listaPtos_aux = ObtenerZPara4ptosPath_Aux.M1_Obtener4PtoConZCorrespondiente();

            Util.InfoMsg($" p1:{listaPtos_aux[0]} \n  p2:{listaPtos_aux[1]}   \n   p3:{listaPtos_aux[2]}    \n  p4:{listaPtos_aux[3]} "  );

            return Result.Succeeded;

        }

    }
}
