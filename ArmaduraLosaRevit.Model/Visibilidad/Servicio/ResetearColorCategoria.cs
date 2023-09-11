using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace ArmaduraLosaRevit.Model.Visibilidad.Servicio
{


    public class ResetearColorCategoria
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public static bool ISCargado_ResetearLineaBArrasMagenta = true;

        public ResetearColorCategoria(UIApplication uiapp, View _view_)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _view_;
        }


        public bool ResetearLineaBArrasMagenta()
        {
            try
            {
               // Util.InfoMsg($"ISCargado_ResetearLineaBArrasMagenta: {ISCargado_ResetearLineaBArrasMagenta.ToString()}");
                if (!ISCargado_ResetearLineaBArrasMagenta) return true;

                if (!(_view.ViewType == ViewType.FloorPlan || _view.ViewType == ViewType.Section || _view.ViewType == ViewType.CeilingPlan)
                    || (!Directory.Exists(ConstNH.CONST_COT)))
                {
                    return false;
                }

                CategoriasDTO _CategoriasDTOLinea = AyudaListaCategoriasDTO.OBtenerLineaBarras();

                ListaVisibility_Graphics _ListaVisibility_Graphics = new ListaVisibility_Graphics(_uiapp, _view);
                _ListaVisibility_Graphics.AsignarLIstaInclusion(_CategoriasDTOLinea);
                _ListaVisibility_Graphics.CargarLista(CategoryType.Model);

                var lista = _ListaVisibility_Graphics
                    .ListElementoEncontrada.SelectMany(pp => pp.SubCategoria
                                                            .Where(r => !IsDefaultOverride(_view.GetCategoryOverrides(r.Category_.Id)))
                                                       ).Select(c => c.Category_.Id)
                                            .ToList();

                if (lista == null) return false;
                if (lista.Count == 0) return false;

                if (Resetear(lista))
                    ISCargado_ResetearLineaBArrasMagenta = false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }






        public bool Resetear(List<ElementId> ListaCategoria)
        {
            try
            {
                // Encuentra la categoría 'Lines' (Líneas)
                Category linesCategory = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

                // Restablecer las anulaciones gráficas para la categoría 'Lines' en la vista activa
                using (Transaction t = new Transaction(_doc, "Reset Category Graphic Overrides"))
                {
                    t.Start();

                    for (int i = 0; i < ListaCategoria.Count; i++)
                    {
                        OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                        _view.SetCategoryOverrides(ListaCategoria[i], ogs);
                    }
                    t.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }

        }


        private bool IsDefaultOverride(OverrideGraphicSettings ogs)
        {
            if ((ogs.CutBackgroundPatternId.IntegerValue == -1) &&
               (ogs.CutForegroundPatternId.IntegerValue == -1) &&
               (ogs.CutLinePatternId.IntegerValue == -1) &&

               (ogs.CutLineWeight == -1) &&

               (ogs.ProjectionLinePatternId.IntegerValue == -1) &&
               (ogs.CutLineWeight == -1) &&

               (ogs.SurfaceBackgroundPatternId.IntegerValue == -1) &&
               (ogs.SurfaceForegroundPatternId.IntegerValue == -1)) return true;

            return false;
        }
    }

}
