using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BorrarSeleccion.SharePArameter
{





    public class BorrarShareParameter
    {
        private readonly Document doc;

        public BorrarShareParameter(Document doc)
        {
            this.doc = doc;
        }

        //guid="1941bdda-f6f3-4908-a1f2-25d0f968cb02"
        // Guid myGuid2 = new Guid("6dfc994d-99a0-4a67-8594-ed6ef0b8f402");
        // BorrarShareParameter.BorrarPArametro(myGuid2);
        public void BorrarPArametro(Guid myGuid)
        {


            // GUID of shared parameter
            SharedParameterElement sParamElement = SharedParameterElement.Lookup(doc, myGuid);
            if (sParamElement == null) return;
            using (Transaction t = new Transaction(doc, "remove sharedParameter"))
            {
                t.Start();
                doc.Delete(sParamElement.Id);
                t.Commit();
            }
        }

        //guid="1941bdda-f6f3-4908-a1f2-25d0f968cb02"
        // BorrarShareParameter.BorrarPArametro(guid);
        public void BorrarPArametro(string guid)
        {
            Guid myGuid = new Guid(guid);
            SharedParameterElement sParamElement = SharedParameterElement.Lookup(doc, myGuid);
            if (sParamElement == null) return;
            using (Transaction t = new Transaction(doc, "remove sharedParameter"))
            {
                t.Start();
                doc.Delete(sParamElement.Id);
                t.Commit();
            }
        }
        //idElement_ShareParameter=947287
        //BorrarShareParameter.BorrarPArametro(idElement_ShareParameter);
        public void BorrarPArametro(int idElement)
        {


            SharedParameterElement sParamElement = (SharedParameterElement)doc.GetElement(new ElementId(idElement));

            if (sParamElement == null) return;
            using (Transaction t = new Transaction(doc, "remove sharedParameter"))
            {
                t.Start();
                doc.Delete(sParamElement.Id);
                t.Commit();
            }
        }
        public void BorrarId(int idElement)
        {
            try
            {

                ElementId elemId = new ElementId(idElement);

                if (elemId == null)
                {
                    Util.ErrorMsg($"Id:{idElement} = null. No se puede borrar");
                    return;
                }
                using (Transaction t = new Transaction(doc, "remove elemento"))
                {
                    t.Start();
                    doc.Delete(elemId);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error Id:{idElement}. No se puede borrar.  ex:{ex.Message}");
            }
        }

        public void BorrarPArametro(List<int> ListidElement)
        {
            int cont = 1;
            foreach (int item in ListidElement)
            {
                SharedParameterElement sParamElement = (SharedParameterElement)doc.GetElement(new ElementId(item));

                if (sParamElement == null)
                {
                    Debug.Write($" {cont }) null elemento :" + item);
                    continue;
                }
                using (Transaction t = new Transaction(doc, "remove sharedParameter"))
                {
                    t.Start();
                    doc.Delete(sParamElement.Id);
                    t.Commit();

                    Debug.Write($"{cont })Elemento :" + item + " OK");
                }
                cont += 1;
            }

        }
    }
}




