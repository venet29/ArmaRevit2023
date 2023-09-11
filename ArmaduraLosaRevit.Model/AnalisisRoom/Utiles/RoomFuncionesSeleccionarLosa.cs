using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Utiles
{
   public class RoomFuncionesSeleccionarLosa
    {
        //obtiene objeto 'Element' del floor con un pto
        static public Element ObtenerElement_DeLosaConUnPto(UIDocument uidoc, XYZ roomCenter)
        {

            // eñ valor de 1 es negativo: no se entiende el pq, si es positivo al parecer selecciona los cielo, camboando typeof(Floor) por typeof(Ceiling)
            XYZ rayDirection = new XYZ(0, 0, -1);

            FilteredElementCollector collector = new FilteredElementCollector(uidoc.Document);
            Func<View3D, bool> isNotTemplate = v3 => !(v3.IsTemplate);
            //View3D View3D = collector.OfClass(typeof(View3D)).Cast<View3D>().First<View3D>(isNotTemplate);
            View3D View3D = TiposFamilia3D.Get3DBuscar(uidoc.Document);
            if (View3D == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return null;
            }

            ElementClassFilter FloorFilter = new ElementClassFilter(typeof(Floor));

            // BoundingBoxXYZ roomBox = selectedRoom.get_BoundingBox(View3D);
            ReferenceIntersector FloorIntersector = new ReferenceIntersector(FloorFilter, FindReferenceTarget.Element, View3D);
            ReferenceWithContext referenceWithContext = FloorIntersector.FindNearest(roomCenter, rayDirection);
            if (referenceWithContext == null)
            {
                TaskDialog.Show("Error", "Erron en 'RoomFuncionesSeleccionarLosa' al buscar losa, posible error el  Section Box 3D. Posible solucion Desactivar 'Section Box' 3D");
                return null;
            }
            Reference ceilingRef = referenceWithContext.GetReference();
            Element selectedCFloor = uidoc.Document.GetElement(ceilingRef) as Element;


            return selectedCFloor;
        }

        //obtiene objeto 'Floor' de un room
        static public Floor ObtenerFloor_ConUNRoom(Room room)
        {

            GeometryElement sad = room.ClosedShell;
            XYZ minf = sad.GetBoundingBox().Min;
            XYZ mxf = sad.GetBoundingBox().Max;
            XYZ pt_selec = (mxf + minf) / 2;
            Floor floorSeleccionada = null;

            List<Element> floors = SeleccionElement.GetElementoFromLevel(room.Document, typeof(Floor), room.Level);

            if (floors.Count > 0)
                floorSeleccionada = ObtenerFloor_ConListaLosaYConUNPunto(pt_selec, floors);

            return floorSeleccionada;
        }



        /// <summary>
        /// busca la losa (dentro de una lista de losas) que contiene 1 pto
        /// determinado
        /// </summary>
        /// <param name="pto"> pto buscadoi</param>
        /// <param name="floors"> lista de losas </param>
        /// <returns> floor que contiene el pto buscado</returns>
        static public Floor ObtenerFloor_ConListaLosaYConUNPunto(XYZ pto, List<Element> floors)
        {

            Floor LosaConPto = null;
            foreach (var floor in floors)
            {
                Element floor_1 = null;
                floor_1 = floor as Floor;
                Options gOptions = new Options();
                gOptions.ComputeReferences = false;
                gOptions.DetailLevel = ViewDetailLevel.Undefined;
                gOptions.IncludeNonVisibleObjects = false;
                GeometryElement geo = floor.get_Geometry(gOptions);
                foreach (GeometryObject obj in geo) // 2013
                {
                    if (obj is Solid)
                    {
                        Solid solid = obj as Solid;
                        foreach (Face face in solid.Faces)
                        {
                            PlanarFace pf = face as PlanarFace;
                            if (pf == null) continue;
                            if (SeleccionarPtoDentroPlanarFace.EStaPuntoALInteriroDeCaraDeUnaLosa(pto, pf))
                            {
                                LosaConPto = floor as Floor;
                                return LosaConPto;
                            }
                        }
                    }
                    Solid floor_ = obj as Solid;

                }
            }
            return LosaConPto;
        }

        public static bool NoEsStructural(Element elementRoomSelec)
        {
            if( elementRoomSelec.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL).AsInteger()!=1)
            {
                Util.ErrorMsg("Losa encontrada no es de tipo 'structural'. Marcar opcion.");
                return false;
            }
            else
                return true;
        }
    }
}
