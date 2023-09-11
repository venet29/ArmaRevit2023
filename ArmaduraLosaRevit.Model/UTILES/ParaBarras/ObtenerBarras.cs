using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class ObtenerBarras
    {

        public static ICollection<Element> ObtenerBArrasENvista( View _view)
        {
            try
            {
                Document _doc = _view.Document;


                ICollection<Element> collectorElementosRebarOcultos = new FilteredElementCollector(_doc).WhereElementIsNotElementType()
                                                            .Where(pt => pt is Rebar && pt.IsHidden(_view)).ToList();

                return collectorElementosRebarOcultos;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }

            return new List<Element>();
        }


        public static ICollection<ElementId> ObtenerBArrasTodoPoryecto(Document _doc)
        {
            try
            {
                ICollection<ElementId> ListaId = new FilteredElementCollector(_doc).OfClass(typeof(Rebar)).ToElementIds();

                return ListaId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }

            return new List<ElementId>();
        }
    }
}
