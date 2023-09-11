using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Traslapo.extension
{
   public static class MetodoExtensionTraslapo
    {

         //verifica si linea en opuesta a linea de path y perpendicular al recorrido de la barra
         //  |   ___________  |
         //  |                |
         //  buscar            path
        public static bool LineaOpuesta(this GeometryObject Gobj, CoordenadaPath _4pointPathReinf)
        {
            if (!(Gobj is  Line)) return false;

            Line _line = (Gobj as Line);
            XYZ pt1 = _line.GetPoint2(0);
            XYZ pt2 = _line.GetPoint2(1);
            return (Math.Abs(_line.GetPoint2(0).Z - _4pointPathReinf.p3.Z) < Util.CmToFoot(5) &&
                   !_line.Contains(_4pointPathReinf.p3) && !_line.Contains(_4pointPathReinf.p4));

           // return (!_line.Contains(_4pointPathReinf.p3) && !_line.Contains(_4pointPathReinf.p4));
        }

        //public static ContenedorDatosTraslapo CambiarSentido(this ContenedorDatosTraslapo contenedorDatosTraslapo, UbicacionLosa _ubicacionLosa)
        //{
        //    //devolver las mismas coordinadas sin cambio
        //    if (UbicacionLosa.Inferior == _ubicacionLosa || UbicacionLosa.Izquierda == _ubicacionLosa) return contenedorDatosTraslapo;

        //    //cambiar coordenadas
        //    TipoBarra _AuxtipoBarraTraslapoDereArriba = contenedorDatosTraslapo._tipoBarraTraslapoDereArriba; ;
        //    contenedorDatosTraslapo._tipoBarraTraslapoIzqBajo = contenedorDatosTraslapo._tipoBarraTraslapoDereArriba;
        //    contenedorDatosTraslapo._tipoBarraTraslapoDereArriba = _AuxtipoBarraTraslapoDereArriba;
        //    return contenedorDatosTraslapo;
        //}
    }



}
 