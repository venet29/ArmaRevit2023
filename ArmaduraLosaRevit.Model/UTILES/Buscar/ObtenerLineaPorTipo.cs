using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.Buscar
{
   public class ObtenerLineaPorNombreTipo
    {
        public static bool IsLineaPorNombre(Document _doc, Element element, string NombreTipoLine)
        {
			try
			{
                if (element == null) return false;
                if (element.Category.Name != "Lines") return false;

                var _parameterSet = element.Parameters;


                string val = "";

                foreach (Parameter param in _parameterSet)
                {
                    if (param.Definition.Name == "Line Style")
                    {

                        Autodesk.Revit.DB.StorageType type = param.StorageType;
                        switch (type)
                        {
                            case Autodesk.Revit.DB.StorageType.Double:
                                val = param.AsDouble().ToString();
                                break;
                            case Autodesk.Revit.DB.StorageType.ElementId:
                                Autodesk.Revit.DB.ElementId id = param.AsElementId();
                                Autodesk.Revit.DB.Element paraElem = _doc.GetElement(id);
                                if (paraElem != null)
                                {
                                    val = paraElem.Name;
                                    if (NombreTipoLine == val)
                                        return true;
                                    else
                                        return false;
                                }
                                break;
                            case Autodesk.Revit.DB.StorageType.Integer:
                                val = param.AsInteger().ToString();
                                break;
                            case Autodesk.Revit.DB.StorageType.String:
                                val = param.AsString();
                                break;
                            default:
                                break;
                        }
                    }

                }

         
            }
			catch (Exception ex)
			{
				Util.DebugDescripcion(ex);
			
			}
			return false;
        }

    }
}
