using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BorrarSeleccion
{
   public  class BorrarElemento
    {


        public static bool Borrar1Elemento(Document _doc, Element ElemmntosBorrar)
        {

            try
            {
                List<Element> listaElemmntosBorrar = new List<Element>();
                listaElemmntosBorrar.Add(ElemmntosBorrar);
                BorrarElementos(_doc,listaElemmntosBorrar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  ex{ex.Message}");
                return false;
            }
            return true;
        }

        public static bool BorrarElementos(Document _doc, List<Element> listaElemmntosBorrar)
        {


            try
            {
                using (Transaction transaction = new Transaction(_doc))
                {
                    transaction.Start("BorrarElemento");
                    foreach (var item in listaElemmntosBorrar)
                    {
                        if (item.IsValidObject)
                            _doc.Delete(item.Id);
                    }
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"  ex{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
