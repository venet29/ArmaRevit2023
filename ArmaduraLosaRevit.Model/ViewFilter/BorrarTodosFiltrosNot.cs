using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter
{
    class BorrarTodosFiltrosNot
    {
        private readonly UIApplication _uiapp;
        private readonly Document _doc;

        public BorrarTodosFiltrosNot(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public bool BOrrarTodosFiltros()
        {
            string input = Interaction.InputBox("Seguro desea borrar las filtros 'NOT' del proyecto?.\n\nConfirmar escribiendo : borrar", "Borrar", "", 300, 300);
            if (input.Trim().ToLower() != "borrar") return false;

            try
            {
                List<ElementId> AllFilters = TiposFiltros.M2_GetAllFiltros_nh(_doc)
                                                         .Select(c => (ParameterFilterElement)c.Value).
                                                         Where(c => c.Name.Contains("Not")).
                                                         Select(c => c.Id).ToList();
                if (AllFilters.Count == 0) return true;
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Borrar Familias Armadura-NH");
                    _doc.Delete(AllFilters);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                TaskDialog.Show("Error", message);
                return false;
            }
            return true;
        }
    }
}
