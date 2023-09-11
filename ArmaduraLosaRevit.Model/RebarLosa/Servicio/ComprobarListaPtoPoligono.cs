using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.Servicio
{
    public class ComprobarListaPtoPoligono
    {
        private BarraRoom barraRoom;

        public ComprobarListaPtoPoligono()
        {

        }

        public ComprobarListaPtoPoligono(BarraRoom barraRoom)
        {
            this.barraRoom = barraRoom;
        }

        public bool ComprobarPoligono()
        {
            if (barraRoom.ListaPtosPerimetroBarras.Count != 4)
            {
                Util.ErrorMsg($"Error en puntos de perimetro de barra   N° {barraRoom.ListaPtosPoligonoLosa.Count }");
                return false;
            }

            RedulicarLArgos_casoS4();

            return true;
        }

        private void RedulicarLArgos_casoS4()
        {
            if (barraRoom.TipoBarraStr == "s4_Inclinada")
            {


                var largobarra = Math.Max((barraRoom.LargoMin_1 * ConstNH.PORCENTAJE_LARGO_PATA), ConstNH.LARGO_MIN_PATH_S4_FOOT );
                var sss = barraRoom.ListaPtosPerimetroBarras[1].DistanceTo(barraRoom.ListaPtosPerimetroBarras[2]);
                if (barraRoom.ubicacionEnlosa == Enumeraciones.UbicacionLosa.Inferior || barraRoom.ubicacionEnlosa == Enumeraciones.UbicacionLosa.Izquierda)
                {
                    var Diree = (barraRoom.ListaPtosPerimetroBarras[2] - barraRoom.ListaPtosPerimetroBarras[1]).Normalize();

                    barraRoom.ListaPtosPerimetroBarras[3] = barraRoom.ListaPtosPerimetroBarras[0] + largobarra * Diree;
                    barraRoom.ListaPtosPerimetroBarras[2] = barraRoom.ListaPtosPerimetroBarras[1] + largobarra * Diree;
                }
                else
                {
                    var Diree = (barraRoom.ListaPtosPerimetroBarras[1] - barraRoom.ListaPtosPerimetroBarras[2]).Normalize();

                    barraRoom.ListaPtosPerimetroBarras[0] = barraRoom.ListaPtosPerimetroBarras[3] + largobarra * Diree;
                    barraRoom.ListaPtosPerimetroBarras[1] = barraRoom.ListaPtosPerimetroBarras[2] + largobarra * Diree;
                }
                var auxLineCentral_conZcero= Autodesk.Revit.DB.Line.CreateBound((barraRoom.ListaPtosPerimetroBarras[3]+ barraRoom.ListaPtosPerimetroBarras[0]).AsignarZ(0)/2,
                                                                       (barraRoom.ListaPtosPerimetroBarras[2] + barraRoom.ListaPtosPerimetroBarras[1]).AsignarZ(0) / 2);
                barraRoom._seleccionarLosaBarraRoom.PtoConMouseEnlosa1 = auxLineCentral_conZcero.ProjectExtendidaXY0(barraRoom._seleccionarLosaBarraRoom.PtoConMouseEnlosa1.AsignarZ(0));
            }
        }
    }
}
