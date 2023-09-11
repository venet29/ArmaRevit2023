using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa
{
    public class FactoryObtenerZPara4ptosPath
    {


        public static IObtenerZPara4ptosPath Crear_ObtenerZPara4ptosPath(UIApplication uiapp, List<XYZ> listaPtos, Face f)
        {

            if (uiapp == null)
            {
                Util.ErrorMsg("UIApplication null en Factor4ptos");
                return new ObtenerZPara4ptosPathNULL();
            }

            if (f is PlanarFace)
            {
                if (((PlanarFace)f).FaceNormal.IsAlmostEqualTo(new XYZ(0, 0, 1), ConstNH.CONST_FACT_TOLERANCIA_3Deci)) // losa plaza
                { return new ObtenerZPara4ptosPathHorizontal(uiapp.ActiveUIDocument.Document, listaPtos, f); }
                else
                    return new ObtenerZPara4ptosPathInclinada(uiapp, listaPtos, (PlanarFace)f);
            }
            else if (f is RuledFace)
            {
                return new ObtenerZPara4ptosRuledFace(uiapp, listaPtos, (RuledFace)f);
            }
            else
                return new ObtenerZPara4ptosPathNULL();
        }
           


        //else if (p2.Z > p3.Z) //subiendo de derecha a izq
        //{ return new ObtenerZPara4ptosPathDereToIzq(listaPtos, f); }
        //else //if (p2.Z < p3.Z) //subiendo de IZq a rede
        //{ return new ObtenerZPara4ptosPathIZqToDere(listaPtos, f); }

    }

}
