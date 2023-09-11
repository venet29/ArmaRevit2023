using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad.Ayuda
{
   public class AyudaViewTemple
    {
        public static void M2_SinViewTemplate(View _view, Document _doc)
        {
            try
            {
                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();
                    Parameter par = _view.GetParameters("View Template").FirstOrDefault();
                    par.Set(new ElementId(-1));
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }
    }
}
