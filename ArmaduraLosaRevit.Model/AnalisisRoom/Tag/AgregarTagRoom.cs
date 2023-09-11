using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Tag
{
    public class AgregarTagRoom
    {
        private readonly Document doc;
        private readonly double anguloPelotaRad;
        private View view;

        //private UIDocument UIdoc;

        public RoomTag roomTag { get; set; }

        public AgregarTagRoom(Document doc, double anguloPelotaRad)
        {
            this.doc = doc;
            this.anguloPelotaRad = anguloPelotaRad;
            this.view = doc.ActiveView;
        }




        /// <summary>
        /// crea tagroom
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="TipoOrientacion"></param>
        /// <returns></returns>
        public RoomTag M1CreaTag(Room room, RoomTagType typeRoom, XYZ ptoPelotaLosa)
        {

            //3) crea el tag con la cuentia de las barra 

            try
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Create creaTag-NH");

                    XYZ cen = M1_1_ObtieneCentroRoom(room);
                    if (cen.IsAlmostEqualTo(XYZ.Zero))
                        return null;
                            
                    UV center = new UV(cen.X, cen.Y);

                    M1_1_CrearTagRoom(room, typeRoom, ptoPelotaLosa);
                    M1_2_CambiarParametosModelA2(roomTag);
                    M1_3_RotarPelota(ptoPelotaLosa, roomTag);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msje = ex.Message;
                //t.RollBack();
                return null;
            }



            return roomTag;
        }

        //crea tag romm
        // result :  genara el 'roomTag'
        private void M1_1_CrearTagRoom(Room room, RoomTagType typeRoom, XYZ ptoPelotaLosa)
        {
            UV ptoPelotaLosaUV = new UV(ptoPelotaLosa.X, ptoPelotaLosa.Y);
            roomTag = doc.Create.NewRoomTag(new LinkElementId(room.Id), ptoPelotaLosaUV, view.Id);
            roomTag.RoomTagType = typeRoom;
        }



        /// <summary>
        /// crea tagroom aux1
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public XYZ M1_1_ObtieneCentroRoom(Room room)
        {
            // Get the room center point.
            XYZ boundCenter = M1_1_1_ObtieneCentroBoundingBoxXYZ(room);
            if (boundCenter.IsAlmostEqualTo(XYZ.Zero)) return boundCenter;
            LocationPoint locPt = (LocationPoint)room.Location;
            XYZ roomCenter = new XYZ(boundCenter.X, boundCenter.Y, locPt.Point.Z);
            return roomCenter;
        }
        /// <summary>
        /// crea tagroom auxtag2
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public XYZ M1_1_1_ObtieneCentroBoundingBoxXYZ(Element elem)
        {
            BoundingBoxXYZ bounding = elem.get_BoundingBox(null);
            if (bounding == null) return XYZ.Zero; 
            XYZ center = (bounding.Max + bounding.Min) * 0.5;
            return center;
        }

        private void M1_3_RotarPelota(XYZ pt_selec, RoomTag rt)
        {

            //opcion1       obliagatorio 'Mc1_CambiarParametosModelA2'
            LocationPoint lp = rt.Location as LocationPoint;
            Line line = Line.CreateBound(pt_selec, pt_selec + new XYZ(0, 0, 10));
            lp.Rotate(line, anguloPelotaRad);
            //opcion2
            //  Line axis = Line.CreateBound(pt_selec, pt_selec + new XYZ(0, 0, 10));
            //ElementTransformUtils.RotateElement(doc, elementID, axis, angleRad);
        }

        //parametro para 
        private void M1_2_CambiarParametosModelA2(Element familyInstance) => ParameterUtil.SetParaInt(familyInstance, "Orientation", 2);
    }
}
