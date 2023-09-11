using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ParametrosShare.Actualizar
{
    //https://thebuildingcoder.typepad.com/blog/2018/11/view-template-include-setting.html
    public class ViewTemplateInclude
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View viewTemplate;

        public ViewTemplateInclude(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _doc = uiapp.ActiveUIDocument.Document;
        }

        public bool Ejecutar()
        {
            try
            {
                List<string> listaPAra = new List<string>() { "ViewNombre", "View Scale" , "NIVEL TITULO PLANO", "EscalaConfiguracion" , "V/G Overrides RVT Links" };
                viewTemplate = SeleccionarView.ObtenerViewPOrNombre(_uiapp.ActiveUIDocument, ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV);
                ActualizaParametro(listaPAra);

                listaPAra = new List<string>() { "ViewNombre", "View Range" ,"TIPO DE ESTRUCTURA (VISTA)", "View Scale", "NIVEL TITULO PLANO","EscalaConfiguracion" };
                viewTemplate = SeleccionarView.ObtenerViewPOrNombre(_uiapp.ActiveUIDocument, ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA);
                ActualizaParametro(listaPAra);

                viewTemplate = SeleccionarView.ObtenerViewPOrNombre(_uiapp.ActiveUIDocument, ConstNH.NOMBRE_VIEW_TEMPLATE_ESTRUC);
                ActualizaParametro(listaPAra);

                viewTemplate = SeleccionarView.ObtenerViewPOrNombre(_uiapp.ActiveUIDocument, ConstNH.NOMBRE_VIEW_TEMPLATE_FUND);
                ActualizaParametro(listaPAra);

            }
            catch (Exception)
            {

                return false; 
            }
            return true;
        }
        private bool ActualizaParametro(List<string> listaPAra)
        {
            try
            {
                if (viewTemplate == null) return false;
                // Create a list so that I can use linq

                var viewparams = new List<Parameter>();
                foreach (Parameter p in viewTemplate.Parameters)
                    viewparams.Add(p);

                // Get parameters by name (safety checks needed)
                var _lista = new List<ElementId>();
                for (int i = 0; i < listaPAra.Count; i++)
                {
                    string _para = listaPAra[i];
                    var _paraENcontrado = viewparams.Where(p => p.Definition.Name == _para).FirstOrDefault();


                    if (_paraENcontrado!=null)
                        _lista.Add(_paraENcontrado.Id);
                }

                if (_lista.Count == 0) return true;
                // Set includes
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start($"MOdificarViewTemplate-NH");
                    viewTemplate.SetNonControlledTemplateParameterIds(_lista);
                    t.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
