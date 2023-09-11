using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TiposPathReinformentSymbolElement
    {
        public static ElementId pathReinforcementTypeId;


        public static ElementId ObtenerPathReinfDefaul(Document _doc)
        {
            if (pathReinforcementTypeId == null)
                pathReinforcementTypeId = PathReinforcementType.CreateDefaultPathReinforcementType(_doc);
            //pathReinforcementTypeId = PathReinforcementType.CreateDefaultPathReinforcementType(_doc);
            return pathReinforcementTypeId;
        }

        public void ObtenerPathReinfDefaul_forzado(Document _doc)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "EDITTOPO"))
                {
                    tr.Start();
                    pathReinforcementTypeId = PathReinforcementType.CreateDefaultPathReinforcementType(_doc);
                    tr.Commit();
                }
          
            }
            catch (Exception)
            {

                ;
            }
                //pathReinforcementTypeId = PathReinforcementType.CreateDefaultPathReinforcementType(_doc);

        }

    }
}
