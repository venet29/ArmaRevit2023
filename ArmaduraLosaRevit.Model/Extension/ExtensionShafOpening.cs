using Autodesk.Revit.DB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
  public static class ExtensionShafOpening
    {

        public static List<PlanarFace> ObtenerListaPlanarFace(this Opening _shaft)
        {
            var lista = new List<PlanarFace>();
            if (_shaft == null) return lista;

            Options opt = new Options();
            opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = true;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS


            GeometryElement geo = _shaft.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects
            //ConstantesGenerales.sbLog.Clear();
            //ConstantesGenerales.sbLog.AppendLine(DateTime.Now.ToString("MM_dd_yyyy Hmm"));
            foreach (GeometryObject obj in geo) // 2013
            {
                if (obj is Solid)
                {
                    Solid solid = obj as Solid;

                    foreach (Face face in solid.Faces)
                    {

                        PlanarFace pf = face as PlanarFace;
                        if (pf == null) continue;

                        lista.Add(pf);


                    }
                }


            }

            return lista;
        }
    }
}
