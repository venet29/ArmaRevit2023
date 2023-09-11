using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Ayuda
{
    public class AyudaRebarVertical
    {
        public static TipoPataBarra obtenerTipoBarras(List<Curve> listacurvas)
        {

            switch (listacurvas.Count())
            {
                case 3:
                    return TipoPataBarra.BarraVPataAmbos;
                case 2:
                    Curve curvaLarga = listacurvas.MinBy(c => -c.Length);
                    double CentroZcurvaLarga = (curvaLarga.GetEndPoint(0).Z + curvaLarga.GetEndPoint(1).Z) / 2;
                    Curve curvaCorta = listacurvas.MinBy(c => c.Length);
                    double CentroZcurvaCorta = (curvaCorta.GetEndPoint(0).Z + curvaCorta.GetEndPoint(1).Z) / 2;
                    return ( CentroZcurvaLarga> CentroZcurvaCorta?TipoPataBarra.BarraVPataInicial:TipoPataBarra.BarraVPataFinal);
                case 1:
                    return TipoPataBarra.BarraVSinPatas;
                default:
                    Util.ErrorMsg($"Error con la configuracion de las curvas de  barras");
                    return TipoPataBarra.BarraVPataInicial;
                    
            }
        }
    }
}
