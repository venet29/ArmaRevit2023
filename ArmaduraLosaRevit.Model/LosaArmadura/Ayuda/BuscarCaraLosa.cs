using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LosaArmadura.Ayuda
{
    //clase obsoleta, remplzadas por metodo de extension element
    public class BuscarCaraLosa
    {


        public delegate bool DireccionDeBusqueda(XYZ pt);
        public Func<XYZ, bool> CaraSuperior = (pt) => SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZPositivo(pt);
        public Func<XYZ, bool> CaraInferior = (pt) => SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZNegativo(pt);
        public Func<XYZ, bool> PointsUpwards = (pt) => Util.PointsUpwards(pt);
        public Face ObtenerCaraLosa(Element _solidoSeleccionado, Func<XYZ, bool> func)
        {
            //SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZPositivo(pf.FaceNormal)
            #region PASO 3 OPCIONES DE GEOMETRIA
            StringBuilder sbuilder = new StringBuilder();

            Options opt = new Options();
            opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = true;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            #endregion

            GeometryElement geo = _solidoSeleccionado.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects
            //ConstantesGenerales.sbLog.Clear();
            //ConstantesGenerales.sbLog.AppendLine(DateTime.Now.ToString("MM_dd_yyyy Hmm"));
            foreach (GeometryObject obj in geo) // 2013
            {
                if (obj is Solid)
                {
                    Solid solid = obj as Solid;
                    int con = 0;
                    foreach (Face face in solid.Faces)
                    {

                        PlanarFace pf = face as PlanarFace;
                        Debug.Print(++con + "Planar face encontraa" + pf.Origin.ToString() );
                        
                        if (func(pf.FaceNormal) )
                        {
                            Debug.Print("Planar face encontraa" + face.GetBoundingBox().ToString());
                            return face;
                        }
                    }
                }
            }
            return null;
      
        }


 
    }
}
