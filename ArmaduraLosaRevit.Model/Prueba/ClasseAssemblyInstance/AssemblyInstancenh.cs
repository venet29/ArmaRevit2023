using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.FAMILIA.Varios;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.Prueba
{


        public class AssemblyInstancenh
        {

            public AssemblyInstancenh()
            {

            }
           public List<string> ListElementsInAssembly(Document doc)
            {
                // 'FP Description' shared parameter GUID

                Guid guid = new Guid("ac6ed937-ffb7-4b18-9c69-7541f5c0319d");

                FilteredElementCollector assemblies = new FilteredElementCollector(doc).OfClass(typeof(AssemblyInstance));

                List<string> descriptions = new List<string>();

                int n;
                string s;

                foreach (AssemblyInstance a in assemblies)
                {
                    ICollection<ElementId> ids = a.GetMemberIds();

                    n = ids.Count;

                    s = string.Format(
                      "\r\nAssembly {0} has {1} member{2}{3}", a.get_Parameter(guid).AsString(), n, Util.PluralSuffix(n), Util.DotOrColon(n));

                    descriptions.Add(s);

                    n = 0;

                    foreach (ElementId id in ids)
                    {
                        Element e = doc.GetElement(id);

                        descriptions.Add(string.Format("{0}: {1}", n++, e.get_Parameter(guid).AsString()));
                    }
                }

                Debug.Print(string.Join("\r\n", descriptions));

                return descriptions;
            }

        }


}
