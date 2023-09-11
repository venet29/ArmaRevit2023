using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.Ayuda
{
    public class AyudaGruposBarrasIguaoP2
    {
        private static int desplazaFactor;

        public static XYZ ObtenerPtoInsersiontag(List<BarraIng> _ListaIBarrasPorPtoPartida_PierYOrientacion, XYZ _RightDirection, int _escala)
        {
            desplazaFactor = 0;

            UbicacionEnPier _ubicacionEnPier = _ListaIBarrasPorPtoPartida_PierYOrientacion[0].ubicacionEnPier;
            Orientacion _Orientacion = _ListaIBarrasPorPtoPartida_PierYOrientacion[0].OrientacionTagGrupoBarras;
            XYZ ptoInsercion = XYZ.Zero;


            var sdf = System.Reflection.Assembly.GetExecutingAssembly();

            ObtenerFactor(_escala, _ubicacionEnPier, _Orientacion);

            if (_ubicacionEnPier == UbicacionEnPier.izquierda)
            {
                if (_Orientacion == Orientacion.izquierda)
                {
                    ptoInsercion = _ListaIBarrasPorPtoPartida_PierYOrientacion[0].ptoInserccionTag_nivelLosa.GetXYZ();
                    ptoInsercion = ptoInsercion - _RightDirection * Util.CmToFoot(35 + desplazaFactor);
                }
                else
                {
                    double ztag = _ListaIBarrasPorPtoPartida_PierYOrientacion[0].ptoInserccionTag_nivelLosa.GetXYZ().Z;
                    XYZnh aux_pt = _ListaIBarrasPorPtoPartida_PierYOrientacion.MinBy(c => -c.distanciaRespectoBorde).P1;
                    ptoInsercion = aux_pt.AsignarZ(ztag).GetXYZ();

                    ptoInsercion = ptoInsercion + _RightDirection * Util.CmToFoot(50 + desplazaFactor);
                }
            }
            else if (_ubicacionEnPier == UbicacionEnPier.derecha)
            {
                if (_Orientacion == Orientacion.izquierda)
                {
                    double ztag = _ListaIBarrasPorPtoPartida_PierYOrientacion[0].ptoInserccionTag_nivelLosa.GetXYZ().Z;
                    XYZnh aux_pt = _ListaIBarrasPorPtoPartida_PierYOrientacion.MinBy(c => -c.distanciaRespectoBorde).P1;
                    ptoInsercion = aux_pt.AsignarZ(ztag).GetXYZ();

                    ptoInsercion = ptoInsercion - _RightDirection * Util.CmToFoot(35 + desplazaFactor);
                }
                else
                {
                    ptoInsercion = _ListaIBarrasPorPtoPartida_PierYOrientacion[0].ptoInserccionTag_nivelLosa.GetXYZ();

                    ptoInsercion = ptoInsercion + _RightDirection * Util.CmToFoot(50 + desplazaFactor);
                }

            }

            return ptoInsercion;
        }

        private static void ObtenerFactor(int escala, UbicacionEnPier ubicacionEnPier, Orientacion orientacionTagGrupoBarras)
        {
            if (escala == 50)
            {
                escala50(ubicacionEnPier, orientacionTagGrupoBarras);
            }
            else if (escala == 75)
            {
                escala75(ubicacionEnPier, orientacionTagGrupoBarras);
            }
            else if (escala == 100)
            {
                escala100(ubicacionEnPier, orientacionTagGrupoBarras);
            }
        }

        private static void escala50(UbicacionEnPier ubicacionEnPier, Orientacion orientacionTagGrupoBarras)
        {
            if (ubicacionEnPier == UbicacionEnPier.izquierda)
            {
                if (orientacionTagGrupoBarras == Orientacion.izquierda)
                    desplazaFactor = 35;
                else
                    desplazaFactor = 50;
            }
            else if (ubicacionEnPier == UbicacionEnPier.derecha)
            {
                if (orientacionTagGrupoBarras == Orientacion.izquierda)
                    desplazaFactor = 35;
                else
                    desplazaFactor = 50;
            }
        }

        private static void escala75(UbicacionEnPier ubicacionEnPier, Orientacion orientacionTagGrupoBarras)
        {
           
            if (ubicacionEnPier == UbicacionEnPier.izquierda)
            {
                if (orientacionTagGrupoBarras == Orientacion.izquierda)
                    desplazaFactor = (int)(35 + 5);
                else
                    desplazaFactor = (int)(50 + 5);
            }
            else if (ubicacionEnPier == UbicacionEnPier.derecha)
            {
                if (orientacionTagGrupoBarras == Orientacion.izquierda)
                    desplazaFactor = (int)(35 + 5);
                else
                    desplazaFactor = (int)(50 + 5);
            }
        }

        private static void escala100(UbicacionEnPier ubicacionEnPier, Orientacion orientacionTagGrupoBarras)
        {
            if (ubicacionEnPier == UbicacionEnPier.izquierda)
            {
                if (orientacionTagGrupoBarras == Orientacion.izquierda)
                    desplazaFactor = 35 +10;
                else
                    desplazaFactor = 50 + 10;
            }
            else if (ubicacionEnPier == UbicacionEnPier.derecha)
            {
                if (orientacionTagGrupoBarras == Orientacion.izquierda)
                    desplazaFactor = 35 + 10;
                else
                    desplazaFactor = 50 + 10;
            }
        }
    }
}
