using ArmaduraLosaRevit.Model.RebarCopia.Entidades;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarCopia
{
    class ElementoConBarras
    {
        private readonly UIApplication _uiapp;
        private readonly Element _element;
        private Document _doc;
        private View _view;


        public ElementoConBarras(UIApplication _uiapp, Element element)
        {
            this._uiapp = _uiapp;
            this._element = element;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }

        public List<RebarCopiarDTo> ListaRebar { get; private set; }

        public bool Obtener()
        {
            try
            { 
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                seleccionarRebar.BuscarListaRebarSimple();
                ListaRebar = seleccionarRebar.ListaRebarSimple
                   .Where(c => c.Elemento.Id.IntegerValue == _element.Id.IntegerValue).ToList();

                if (ListaRebar.Count == 0)
                {
                    Util.ErrorMsg("Elemento sin barras");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener barras en elemento.  \nex:{ex.Message}");
            }
            return true;
        }
    }
}
