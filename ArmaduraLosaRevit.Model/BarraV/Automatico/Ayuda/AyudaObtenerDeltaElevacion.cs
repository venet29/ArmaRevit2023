using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda
{
    public class AyudaObtenerDeltaElevacion
    {

        public static double ObtenerDeltaElevation(UIApplication _uiapp)
        {

            SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);

            List<Level> listaLevel = _seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_uiapp.ActiveUIDocument.ActiveView);

            List<double> listaDelta = new List<double>();
            foreach (var item in listaLevel)
            {
                double desnivel = item.ProjectElevation - item.Elevation;
                listaDelta.Add(desnivel);
            }
            
            if (listaDelta.Count == 0)
            {
                Util.ErrorMsg($" Elevacion sin Niveles");
                return 0; 
            }

            // para comprobar si No todos los niveles tiene difenrentes delta de altura
            //var val = listaDelta.FirstOrDefault();
            //if (!listaDelta.All(x => x == val))
            //    Util.ErrorMsg($" No todos los niveles tiene igual delta Valores en Level  ");


            return (listaDelta.Count == 0 ? 0 : listaDelta[0]);

        }
    }
}
