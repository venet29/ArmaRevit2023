using Autodesk.Revit.DB;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Niveles.Vigas
{
     public class NivelVigaInvertida
    {
        //obs2) caso viga
        public static List<double>  CasoVigaInvertido(List<double> ListaLevelIntervalo, double _PtoInicioIntervaloBarra_coordZ)
        {
            double minNivel = ListaLevelIntervalo.MinBy(c => c);
            //si, pto seleccion +50cm es mayor qeu menor nivel salir
            if (minNivel < _PtoInicioIntervaloBarra_coordZ + Util.CmToFoot(50))
            {
                ListaLevelIntervalo.Clear();
                return ListaLevelIntervalo;
            }

            if (ListaLevelIntervalo.Count > 1) ListaLevelIntervalo.Remove(minNivel);

            ListaLevelIntervalo.Add(_PtoInicioIntervaloBarra_coordZ);

            ListaLevelIntervalo = ListaLevelIntervalo.OrderBy(nn => nn).ToList();

            return ListaLevelIntervalo;
        }
    }
}
