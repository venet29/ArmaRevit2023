using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.Buscar
{
    public class ObtenerListaParametros
    {
        public static void Ejecutar(UIDocument _uidoc)
        {
            Document _doc = _uidoc.Document;
            Element element = null;
            try
            {
                Reference ref_baar1 = _uidoc.Selection.PickObject(ObjectType.Element, "Seleccionar :");

                element = _doc.GetElement(ref_baar1);
            }
            catch (Exception)
            {

                return;
            }

            try
            {
                if (element == null) return;

                var _parameterSet = element.Parameters;

                string val = "";
                int cont = 1;
                LogNH.Limpiar_sbuilder();
                foreach (Parameter param in _parameterSet)
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
                                val = paraElem.Name;
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
                    string resul = $" {cont})  {param.Definition.Name}: {val} ";
                    LogNH.Agregar_registro(resul);
                    Debug.WriteLine(resul);
                    cont = cont + 1;
                }

                Util.InfoMsg(LogNH._sbuilder.ToString());
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);

            }     
        }

    }
}
