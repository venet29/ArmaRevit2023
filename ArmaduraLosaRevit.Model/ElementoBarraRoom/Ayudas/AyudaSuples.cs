using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
   public class AyudaSuples
    {


        public static XYZ ObtenerPtoPathSymbolSx(List<XYZ> ListaPtosPerimetroBarras)
        {
            XYZ Ptoresul = new XYZ(ListaPtosPerimetroBarras.Average(p => p.X),
                                  ListaPtosPerimetroBarras.Average(p => p.Y),
                                  ListaPtosPerimetroBarras.Average(p => p.Z));
            try
            {

  
            XYZ p1 = ListaPtosPerimetroBarras[0];
            XYZ p2 = ListaPtosPerimetroBarras[1];
            XYZ p3 = ListaPtosPerimetroBarras[2];
            double angulo = Util.AnguloEntre2PtosGrados_enPlanoXY(p2, p3);
            CrearTrasformadaSobreEjeZ _CrearTrasformada = new CrearTrasformadaSobreEjeZ(Ptoresul,angulo);

            XYZ p12_medio = _CrearTrasformada.EjecutarTransform((p1+ p2)/2);

                Ptoresul = _CrearTrasformada.EjecutarTransformInvertida(new XYZ((angulo >90?1:- 1),0,0));
            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al obtenre pto PathSymbol suple. Se utiliza centro de path");
            }
            return Ptoresul;
        }
    }
}
