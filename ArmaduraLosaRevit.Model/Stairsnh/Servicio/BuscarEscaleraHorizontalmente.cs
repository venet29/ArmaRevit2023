using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Stairsnh.Servicio
{
    public class BuscarEscaleraHorizontalmente
    {
        private readonly UIApplication _uiapp;
        private readonly Document _doc;
        private double _largoDeBUsquedaFoot;

        public BuscarEscaleraHorizontalmente(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;

            this._largoDeBUsquedaFoot = Util.CmToFoot(20);
        }

        public Stairs buscarEscaleraHorizontal(View3D elem3d, XYZ PtoOrigen, XYZ VectorDireccionBusqueda , bool dibujarMOdelLIne=false)
        {
            if (dibujarMOdelLIne)
            {
#if (DEBUG)
                CrearLIneaAux CrearLIneaAux = new CrearLIneaAux(_doc );
                CrearLIneaAux.CrearLinea(PtoOrigen, PtoOrigen + VectorDireccionBusqueda * 5);
#endif
            }

            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Stairs);
            ReferenceIntersector ri = new ReferenceIntersector(filter, FindReferenceTarget.All, elem3d);
            ReferenceWithContext ref2 = ri.FindNearest(PtoOrigen, VectorDireccionBusqueda);


            if (ref2 != null)
            {
                if (ref2.Proximity < _largoDeBUsquedaFoot)
                {
                    Reference ceilingRef = ref2.GetReference();
                    Stairs StairsIntersectada = _doc.GetElement(ceilingRef) as Stairs;
                    return StairsIntersectada;
                }
            }
            return null;
        }
    }
}
