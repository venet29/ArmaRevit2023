using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Intervalos;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{

    public class SeleccionarLosa
    {
        protected Document _doc;
        protected UIDocument _uidoc;
        private View _view;
        protected UIApplication _uiapp;


        public bool _todoBien { get; set; }
        public List<Floor> ListaFloor { get; private set; }

        public SeleccionarLosa(UIApplication uiapp)
        {
            this._doc = uiapp.ActiveUIDocument.Document;
            this._uidoc = uiapp.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._uiapp = uiapp;
            _todoBien = true;
        }


        public bool ObtenerTodosLosFloot()
        {
            try
            {
                //buscar primer nivel
                FilteredElementCollector Colectornivel = new FilteredElementCollector(_doc);
                ListaFloor = Colectornivel
                         .OfClass(typeof(Floor))
                         .OfCategory(BuiltInCategory.OST_Floors)
                         .Cast<Floor>().ToList();

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }




    }
}
