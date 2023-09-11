using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class IsDentroPoligono
    {

        #region BUscar puntos interior poligono e brras

        /// <summary>
        /// funcion para encontrar pto al inteior de LISTA de poligonos formados por los puntos que dibujan las barras path
        /// </summary>
        /// <param name="pto"></param>
        /// <param name="listaPolyRecorrido"></param>
        /// <returns></returns>
        public static bool Probar_Si_punto_alInterior_Polilinea(XYZ pto, List<List<XYZ>> listaPolyRecorrido)
        {
            foreach (List<XYZ> poly in listaPolyRecorrido)
            {
                // BUSCA SI PTO ESTA DENTRO DE UNA POLILINEA, SOLO PARA POLILINEA CERRADA SIMPLE
                if (IsPointInsidePolyline(pto,poly)) return true;
            }


            return false;
        }

        /// <summary>
        /// busca si punto esta al interrior de poligono formado por los puntos que dibujan la barra path
        /// </summary>
        /// <param name="pl"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static bool IsPointInsidePolyline(XYZ pt,List<XYZ> pl )
        {
            double anglulo1 = Util.Angle3Ptos(pl[0], pl[1], pt);
            double anglulo2 = Util.Angle3Ptos(pl[1], pl[2], pt);
            double anglulo3 = Util.Angle3Ptos(pl[2], pl[3], pt);
            double anglulo4 = 0;
            if (pl.Count == 5)
            {
                anglulo4 = Util.Angle3Ptos(pl[3], pl[4], pt);
            }
            else if (pl.Count == 4)
            {
                anglulo4 = Util.Angle3Ptos(pl[3], pl[0], pt);
            }
            else
            {
                return false;
            }
            double total = anglulo1 + anglulo2 + anglulo3 + anglulo4;

            if (Math.Abs(total - 2 * Math.PI) < 0.01)
                return true;
            else
                return false;


        }

        #endregion

    }
}
