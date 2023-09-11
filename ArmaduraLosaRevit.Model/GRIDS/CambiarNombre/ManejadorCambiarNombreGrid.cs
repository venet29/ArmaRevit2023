using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GRIDS
{
 
    public class ManejadorCambiarNombreGrid
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ManejadorCambiarNombreGrid(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }



        public bool Ejecutar()
        {
            try
            {
                //Lista     grid  nombreantiguo - nuevo Nombre -->  y vistas asociados

                // cabiar nombre

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ManejadorCambiarNombreGrid'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }

}
