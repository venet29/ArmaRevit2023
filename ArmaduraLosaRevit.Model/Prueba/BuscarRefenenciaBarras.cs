
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Prueba
{
   public class BuscarRefenenciaBarras
    {
        public static void ejecutar(UIDocument _uidoc)
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

            if (element is Rebar)
            {
                Rebar _rebas = (element as Rebar);

                Line rebarSeg1 = null;
                bool bOk = _rebas.getRebarSegmentMasLArgo(out rebarSeg1);
                if (!bOk)
                    return ;
                var ref1 = _rebas.getReferenceForEndOfBar(_doc.ActiveView, rebarSeg1);
            }
        }
    }
}
