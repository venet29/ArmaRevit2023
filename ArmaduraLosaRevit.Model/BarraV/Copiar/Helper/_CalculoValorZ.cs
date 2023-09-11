using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar.Helper
{
    public class _CalculoValorZ
    {

        public static Dictionary<string, double> ObtenerLista_Z_Level(UIApplication _uiapp,View _view)
        {

            ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
            List<Level> listaLevel = _seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(_view);
            Dictionary<string, double> listalevel = new Dictionary<string, double>() ;

            foreach (var item in listaLevel)
            {
                listalevel.Add(item.Name, item.ProjectElevation);
            }
            return listalevel;
        }

    }
}
