using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FILTROS.Ayuda
{
    internal class QueFiltroSePuedeUSar
    {
        // permite determinar que parametros se puede usar como filtro
        //https://thebuildingcoder.typepad.com/blog/2022/02/utility-classes-and-constraining-stirrups.html
        public static void ObtenerListaFIltro(Document _doc)
        {
  
            List<string> parameterNames = new List<string>();
            try
            {
             
                IList<ElementId> wallCatList = new List<ElementId>() { new ElementId(BuiltInCategory.OST_Rebar) };

                var paramColl = ParameterFilterUtilities.GetFilterableParametersInCommon(_doc, wallCatList);
                foreach (ElementId param in paramColl)
                {

#pragma warning disable CS0168 // The variable 'value' is declared but never used
                    object value;
#pragma warning restore CS0168 // The variable 'value' is declared but never used
                    if (!Enum.IsDefined(typeof(BuiltInParameter), param.IntegerValue)) continue;

                    BuiltInParameter bip = (BuiltInParameter)param.IntegerValue;


                    string label = LabelUtils.GetLabelFor(bip);
                    if (label == null) continue;
                    parameterNames.Add(label);
                }

                parameterNames.Sort();
                StringBuilder sb = new StringBuilder();
                parameterNames.ForEach(e => sb.Append(e + "\r\n"));
                TaskDialog.Show("Filtered Parameters", sb.ToString());
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro 'ObtenerListaFIltro'  ex:{ex.Message}");
            }
        }
    }
}
