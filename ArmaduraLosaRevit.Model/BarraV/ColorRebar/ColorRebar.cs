using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.ColorRebar
{
   public class ColorRebarAsignar
    {
        private List<Rebar> _listaRebar;
        private  UIApplication _uiapp;
        private double largoRef = 1200;     

        public ColorRebarAsignar(UIApplication uiapp, List<Rebar> _listaRebar)
        {
            this._uiapp = uiapp;
            this._listaRebar = _listaRebar;
        }

        public void M7_CAmbiarColor()
        {
            if (_listaRebar == null) return;
            if (_listaRebar.Count == 0) return;
            VerificarMayores12mt();

            ColorearBarrasMenor12mt_magenta();

        }

        private void ColorearBarrasMenor12mt_magenta()
        {
            List<ElementId> _listaRebarId = _listaRebar
                                .Where(c => c.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble() < Util.CmToFoot(largoRef))
                                .Select(r => r.Id).ToList();

            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(_listaRebarId, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta));
        }

        private void VerificarMayores12mt()
        {

            List<ElementId> _listaRebarId = _listaRebar
                         .Where(c => c.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble() > Util.CmToFoot(largoRef))
                         .Select(r => r.Id).ToList();
            VisibilidadElementRebarLosa visibilidadElement = new VisibilidadElementRebarLosa(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(_listaRebarId, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.amarillo), false);

            //visibilidadElement
        }
    }
}
