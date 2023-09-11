using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag
{
    public partial class GeomeTagBase
    {

        public bool Intercambiar_F_L()
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("tCreateAndStore");



                    if (TagP0_F != null && TagP0_L != null)
                    {
                        XYZ ptF = TagP0_F.posicion;
                        XYZ ptL = TagP0_L.posicion;

                        ElementTransformUtils.MoveElement(_doc, TagP0_F.ElementIndependentTagPath.Id, ptL);
                        ElementTransformUtils.MoveElement(_doc, TagP0_L.ElementIndependentTagPath.Id, ptF);
                    }

                    if (TagP0_F2 != null && TagP0_L2 != null)
                    {
                        XYZ ptF = TagP0_F2.posicion;
                        XYZ ptL = TagP0_L2.posicion;

                        ElementTransformUtils.MoveElement(_doc, TagP0_F2.ElementIndependentTagPath.Id, ptL);
                        ElementTransformUtils.MoveElement(_doc, TagP0_L2.ElementIndependentTagPath.Id, ptF);
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al intercambiar tag F y L.  ex:{ex.Message} ");
                return false;
            }
            return true;
        }

    }
}
