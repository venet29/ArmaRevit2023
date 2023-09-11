using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda
{
    public class AyudaObtenerListaDesglosada
    {
        public static List<RebarDesglose> Lista_RebarDesglose { get;  set; }

        public static bool ObtenerLista( List<Element> _Listrebar, UIApplication uiapp)
        {
            try
            {
                Lista_RebarDesglose = new List<RebarDesglose>();
                foreach (Rebar _rebar in _Listrebar)
                {
                    RebarDesglose _RebarDesglose = new RebarDesglose(uiapp,_rebar);
                    _RebarDesglose.Ejecutar();
                    Lista_RebarDesglose.Add(_RebarDesglose);
                }         
            }
            catch (Exception ex )
            {
                Util.ErrorMsg($"Error al obtener lista rebar: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}
