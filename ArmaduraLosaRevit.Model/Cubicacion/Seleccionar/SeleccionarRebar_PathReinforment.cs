using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cubicacion.Seleccionar
{
    public class SeleccionarRebar_PathReinforment
    {

        private UIApplication _uiapp;
        private Document _doc;
        private UIDocument _uidoc;
        private View _view;
        private FilteredElementCollector _collectorRebarViewActual;
        private FilteredElementCollector _collectorPathReinfNivelActual;

        public List<Element> _lista_A_TodasRebarNivelActual_MENOSRebarSystem { get; set; }
        public List<Element> _lista_A_DePathReinfNivelActual { get; set; }

        public List<ElementoPathRebar> _lista_A_ElementoREbarDTO { get; set; }
        public string NombreVistaAnalizada { get; private set; }
        public List<ElementoPathRein> _lista_A_ElementoPathReinformentDTO { get; set; }

        public SeleccionarRebar_PathReinforment(UIApplication _uiapp, View _viewObtenerBArras)
        {
            this._uiapp = _uiapp;
            this._uidoc = _uiapp.ActiveUIDocument;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _viewObtenerBArras;
            this._lista_A_TodasRebarNivelActual_MENOSRebarSystem = new List<Element>();
            this._lista_A_ElementoREbarDTO = new List<ElementoPathRebar>();
            this.NombreVistaAnalizada = _view.Name;
            this._lista_A_ElementoPathReinformentDTO = new List<ElementoPathRein>();
        }




        public bool M0_Cargar_rebar()
        {
            try
            {

                ICollection<Element> collectorElementosRebarOcultos = new FilteredElementCollector(_uidoc.Document).WhereElementIsNotElementType()
                                                            .Where(pt => pt is Rebar && pt.IsHidden(_view)).ToList();

                _collectorRebarViewActual = new FilteredElementCollector(_uidoc.Document, _view.Id);
                //para las familias en el para las instacias de familia en el plano
                _collectorRebarViewActual.OfCategory(BuiltInCategory.OST_Rebar).WhereElementIsNotElementType();

                _lista_A_TodasRebarNivelActual_MENOSRebarSystem = _collectorRebarViewActual
                                                .Where(pt => (pt is Rebar && (((Rebar)pt).TotalLength>0)))
                                                .ToList();

                if (collectorElementosRebarOcultos.Count > 0)
                {
                    _lista_A_TodasRebarNivelActual_MENOSRebarSystem.AddRange(collectorElementosRebarOcultos);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool M1_Ejecutar_rebar()
        {
            try
            {
#pragma warning disable CS0219 // The variable 'i' is assigned but its value is never used
                int i = 0;
#pragma warning restore CS0219 // The variable 'i' is assigned but its value is never used
                foreach (Element ee in _lista_A_TodasRebarNivelActual_MENOSRebarSystem)
                {
                    if (!(ee is Rebar)) continue;

                    Rebar _rebar = (Rebar)ee;
                    if (!_rebar.IsValidObject) continue;

                    ElementoPathRebar visibilidadElementoPathDTO = ElementoPathRebar.CrearVisibilidadElementoRebarhDTO(_doc, _rebar, new List<Element>());
                    _lista_A_ElementoREbarDTO.Add(visibilidadElementoPathDTO);
                 // if(i++==10)  break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }



        public bool M0_CArgar_PAthReinforment()
        {
            try
            {

                ICollection<Element> collectorElementosPathOcultos = new FilteredElementCollector(_uidoc.Document).WhereElementIsNotElementType()
                                             .Where(pt => pt is PathReinforcement && pt.IsHidden(_view)).ToList();
         


                _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _view.Id);
                //para las familias en el para las instacias de familia en el plano
                _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsNotElementType();
                _lista_A_DePathReinfNivelActual = _collectorPathReinfNivelActual
                                                .Where(pt => pt is PathReinforcement)
                                                .ToList();
                if (collectorElementosPathOcultos.Count > 0)
                {
                    _lista_A_DePathReinfNivelActual.AddRange(collectorElementosPathOcultos);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public  bool M1_Ejecutar_PAthReinforment()
        {
            try
            {
#pragma warning disable CS0219 // The variable 'i' is assigned but its value is never used
                int i = 0;
#pragma warning restore CS0219 // The variable 'i' is assigned but its value is never used
                foreach (Element ee in _lista_A_DePathReinfNivelActual)
                {
                    if (!(ee is PathReinforcement)) continue;

                    PathReinforcement _pathReinforcement = (PathReinforcement)ee;
                    if (!_pathReinforcement.IsValidObject) continue;

                    ElementoPathRein visibilidadElementoPathDTO = ElementoPathRein.ObtenerElementoPathRein(_doc, _pathReinforcement, null, new List<Element>());
                    _lista_A_ElementoPathReinformentDTO.Add(visibilidadElementoPathDTO);
                   // if (i++ == 100) break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
