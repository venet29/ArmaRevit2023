using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GEOM;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda
{
    public class AyudaLosa
    {
        //public static Face obtenerFaceSuperiorLosa(Floor floor)
        //{
        //    var lista = floor.ListaFace();

        //    if (lista == null) return null;
        //    if (lista.Count == 0) return null;

        //    Face FaceSuperior = lista[0].Where(c => Util.PointsUpwards_soloViga(c.FaceNormal)).FirstOrDefault();

        //    if (FaceSuperior == null)
        //        Util.ErrorMsg($"No se encontro face superior  en losa id:{floor.Id}");

        //    return FaceSuperior;
        //}

        #region 3 metodos para caras ruledFace
        public static RuledFaceDTo obtenerFaceSuperiorLosa_RuledFace(Element Elemeto) => ObtenerListaRuledFace(Elemeto).FirstOrDefault(c=>c.Posicion==Posicion.Superior);
        public static RuledFaceDTo obtenerFaceInferiorLosa_RuledFace(Element Elemeto) => ObtenerListaRuledFace(Elemeto).FirstOrDefault(c => c.Posicion == Posicion.Inferior);
        public static List<RuledFaceDTo> ObtenerListaRuledFace(Element Elemeto)
        {
            List<RuledFaceDTo> ListaRuledFaceDTo = new List<RuledFaceDTo>();
            var listaRuledFace = Elemeto.ListaRuledFace();

            if (listaRuledFace == null) return ListaRuledFaceDTo;
            if (listaRuledFace.Count == 0) return ListaRuledFaceDTo;

            foreach (RuledFace item in listaRuledFace[0])
            {
                ListaRuledFaceDTo.Add(item.ObtenerDatosRuledFace());
            }
            var sup = ListaRuledFaceDTo.OrderByDescending(c => c.midpoint.Z).FirstOrDefault();
            if(sup.normal.Z>0)
                sup.Posicion = Posicion.Superior;

            var inf = ListaRuledFaceDTo.OrderBy(c => c.midpoint.Z).FirstOrDefault();
            if (sup.normal.Z < 0)
                inf.Posicion = Posicion.Inferior;

            return ListaRuledFaceDTo;
        }
        #endregion

        public static string ObtenerListaPto(CurveArray cArray)
        {
            string result = "";
            for (int i = 0; i < cArray.Size; i++)
            {
                Curve ln = cArray.get_Item(i);
                result = result + $"Line{i} : p1:{ln.GetEndPoint(0).REdondearString_foot(2)}  ,  p2:{ln.GetEndPoint(1).REdondearString_foot(2)}\n ";
            }
            return result;
        }

    }
}
