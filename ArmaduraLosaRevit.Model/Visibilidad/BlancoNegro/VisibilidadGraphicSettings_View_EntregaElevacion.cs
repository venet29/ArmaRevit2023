using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro
{
    //modifica el vv
    public class VisibilidadGraphicSettings_View_EntregaElevacion
    {
        private View _view;
        private Document _doc;
        private List<CategoriasDTO> ListElementoVisibles;

        public Dictionary<string, List<Element>> ListaResult { get; private set; }
        public object FactoryGraphicSettingViewEntrega_ { get; private set; }

        public VisibilidadGraphicSettings_View_EntregaElevacion(View view)
        {
            this._view = view;
            this._doc = _view.Document;
            this.ListElementoVisibles = new List<CategoriasDTO>();
        }
        public bool M1_ObtenerListaElementosVisibles(CategoryType _categoryType)
        {
            //Document doc = _view.Document;
            Categories categories = _doc.Settings.Categories;
            ListElementoVisibles = new List<CategoriasDTO>();

            bool ISvisible = true;

            /* LISTA CATEGORIAS
            Debug.WriteLine($"INICIO***************************************************************************************");
            foreach (Category categoriaPrincipal in categories)
            {

                Debug.WriteLine($" {categoriaPrincipal.Name},");
                CategoryNameMap subcats = categoriaPrincipal.SubCategories;
                if (subcats.Size > 0)
                {
                    foreach (Category item in subcats)
                    {
                        Debug.WriteLine($", {item.Name}");
                    }
                }
            }
            Debug.WriteLine($"FIN***************************************************************************************");
            */
            try
            {

            int contador = 1;
            foreach (Category categoriaPrincipal in categories)
            {

                CategoriasDTO categoriaPrincipal_ = null;

                if (!AyudaComprobar_tipoCategoria_EstaEnUi_EntregaElev.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaInclusion(categoriaPrincipal, _categoryType, _view, TipoCategoria.Principal)) continue;

                //Checks if elements of the given category are set to be invisible (hidden) in this view. 
                bool IsCategoriaVISIBLE = !_view.GetCategoryHidden(categoriaPrincipal.Id);// categoriaPrincipal.get_Visible(_view);
                if (IsCategoriaVISIBLE == ISvisible)//True if the category is invisible (hidden), false otherwise.
                {
                    categoriaPrincipal_ = new CategoriasDTO(categoriaPrincipal.Name, categoriaPrincipal, TipoCategoria.Principal, IsCategoriaVISIBLE);
                    ListElementoVisibles.Add(categoriaPrincipal_);
                }
                else
                    continue;

                int contador2 = 1;
                CategoryNameMap subcats = categoriaPrincipal.SubCategories;
                if (subcats.Size > 0)
                {
                    List<CategoriasDTO> listaSubClases = new List<CategoriasDTO>();

                    foreach (Category item in subcats)
                    {
                        Debug.WriteLine($"B) SUB categoria analizada : {item.Name}   contador2:{contador2}");

                        if (!AyudaComprobar_tipoCategoria_EstaEnUi_EntregaElev.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaIncluir(item, categoriaPrincipal, _view)) continue;

                        if (item.Parent.Name != categoriaPrincipal.Name) continue;
                        bool estadoCategoriaitem = !_view.GetCategoryHidden(item.Id);

                            //para desocultar categoriua
                          //  _view.SetCategoryHidden(ListElementoVisibles[i], EstadoOculto);

                            if (estadoCategoriaitem == ISvisible)
                            listaSubClases.Add(new CategoriasDTO(item.Name, item, TipoCategoria.Secuandario, IsCategoriaVISIBLE));

                        contador2 = contador2 + 1;
                    }

                    if (listaSubClases.Count > 0)
                        categoriaPrincipal_.SubCategoria = listaSubClases;
                }
                contador = contador + 1;
            }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en  'M1_ObtenerListaElementosVisible'.\n ex:{ex.Message}   ");
                return false;
            }
            return true;
        }
        public bool M2_CambiarColor_EntregaElev(bool estado, CategoryType _categoryType)
        {       
            if (ListElementoVisibles.Count == 0) return false;

            int i = 0;
            string namecageroria = "";
            // estado true desactivar
            //        false activar 
            try
            {
                    for (i = 0; i < ListElementoVisibles.Count; i++)
                    {
                        var item = ListElementoVisibles.ElementAt(i);
                        //var _nombre = item.nombre;
                        Category _categoria = item.Category_;
                        //namecageroria = _categoria.Name;
                        // categoria princiapal
                        M2_1_cambiarColorCategory(estado, _categoria);

                        //sub categoria
                        foreach (CategoriasDTO subItem in item.SubCategoria)
                        {
                            M2_1_cambiarColorCategory(estado, subItem.Category_);
                        }
                    }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error vista '{_view.Name}', al buscar categegoria tipo: '{_categoryType}' en la posicion {i}  \n--> nombre categoria :{namecageroria}   \n ex:{ex.Message}   ");
                string msj = ex.Message;
                return false;
            }
            return true;
        }

        private void M2_1_cambiarColorCategory(bool estado, Category _categoria)
        {
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();

            if (estado)
            {
                ogs = FactoryGraphicSetting_ViewELEVEntrega_VV.ObtenerOverrideGraphicSettings_Entrega(_categoria.Name, _doc);
                if (ogs == null) return;
            }
            else
            {
                var ogsAux = FactoryGraphicSetting_ViewELEVEntrega_VV.ObtenerOverrideGraphicSettings_Entrega(_categoria.Name, _doc);
                if (ogsAux != null)
                    ogs = ogsAux;

            }
            _view.SetCategoryOverrides(_categoria.Id, ogs); // 2017
        }
    }
}
