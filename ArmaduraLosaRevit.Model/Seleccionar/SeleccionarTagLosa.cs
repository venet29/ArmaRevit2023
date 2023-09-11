using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Seleccionar.Modelo;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarTagLosa
    {
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private Document _doc;
        private View _view;
        private IList<Element> _listaElementsTagRebarSeleccionado;
        private List<WrapperPathReinTag> ListaGeneral;
 

        public XYZ ptoMouse { get; set; }
        public List<IndependentTag> ListIndependentTag { get; set; }

        public XYZ _direccionMoverTag { get; set; }

        public SeleccionarTagLosa(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uidoc.Document;

            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
            ListIndependentTag = new List<IndependentTag>();

        }
        public bool EjecutarConMouse()
        {
            if (!SeleccionarTagRebar()) return false;

            if (!SeleccionarDireccionMouse()) return false;
            return true;
        }

        private bool SeleccionarTagRebar()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                ISelectionFilter f = new FiltroRebarTagAgrupar();
                //selecciona un objeto floor
                _listaElementsTagRebarSeleccionado = _uidoc.Selection.PickElementsByRectangle(f, "Seleccionar TagRebar");
                if (_listaElementsTagRebarSeleccionado.Count == 0) return false;

                foreach (var item in _listaElementsTagRebarSeleccionado)
                {
                    if (item is IndependentTag)
                        ListIndependentTag.Add(item as IndependentTag);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
                return false;
            }
            return true;
            // if (_listaElementsRebarSeleccionado.Count > 0) ObtenerListaDeREbarNivelActualS();
        }
        private bool SeleccionarDireccionMouse()
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {
                //Definimos primero los tipos de OSNAP para seleccionar los puntos
                ObjectSnapTypes snapTypes = ObjectSnapTypes.None ;
                //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                 ptoMouse = _uidoc.Selection.PickPoint(snapTypes, "SELECCIONA PICKPOINT: EXTREMOS O INTERSECCIONES DE ELEMENTOS");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
                return false;
            }
            return true;
        }


        public bool ObtenerTagDePathEnView(ElementId idPath)
        {
            try
            {
                GenerarListaPathReinTags();

                ListIndependentTag = ListaGeneral.Where(c => c.IdPathreinforment == idPath).Select(s=>s.tag).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SelesccionarTagDePathEnView  -->  ex:{ex.Message}");
                return false;
            }
            return true;
        }



        private bool GenerarListaPathReinTags()
        {
            try
            {
                if(ListaGeneral==null)
                    ListaGeneral = TiposPathReinTagsEnView.M1_2_BuscarPathReinInView(_doc, _doc.ActiveView.Id)
                                                .Select( c=> new WrapperPathReinTag() {
                                                    tag =c,
                                                    IdPathreinforment = c.Obtener_GetTaggedLocalElementID()
                                                }).ToList();


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }

    }
}
