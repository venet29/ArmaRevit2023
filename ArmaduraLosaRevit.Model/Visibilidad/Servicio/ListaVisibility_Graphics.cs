using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Visibilidad.Servicio
{
    //obtine una lista con los elementos  de la ventana 'Visibility/Graphics'
    // que se carga con 'VV'
    public class ListaVisibility_Graphics
    {
        protected UIApplication uiapp;
        protected View _view;

        protected Document _doc;
        public List<CategoriasDTO> ListElementoEncontrada { get; set; }

        private List<CategoriasDTO> ListaInclusionDto;

        public ListaVisibility_Graphics(UIApplication uiapp, View view)
        {
            this.uiapp = uiapp;
            this._view = view;
            this._doc = _view.Document;
            this.ListElementoEncontrada = new List<CategoriasDTO>();
            this.ListaInclusionDto = new List<CategoriasDTO>();
        }


        public void AsignarLIstaInclusion(CategoriasDTO _CategoriasDTO)
        {
            List<CategoriasDTO> ListaInclusionDto_aux= new List<CategoriasDTO> ();
            ListaInclusionDto_aux.Add(_CategoriasDTO);
            AsignarLIstaInclusion(ListaInclusionDto_aux);
        }

        public void AsignarLIstaInclusion(List<CategoriasDTO> ListaInclusionDto_aux) =>
                ListaInclusionDto = ListaInclusionDto_aux;


        public bool CargarLista(CategoryType _categoryType, bool COnsiderarListaExclusionOInclusion = true)
        {
            try
            {
                //Document doc = _view.Document;
                Categories categories = _doc.Settings.Categories;
                ListElementoEncontrada = new List<CategoriasDTO>();

              //  bool ISvisible = true;

                int contador = 0;
                foreach (Category categoriaPrincipal in categories)
                {
                    contador = contador + 1;
                    CategoriasDTO categoriaPrincipal_ = null;

                    if (categoriaPrincipal.CategoryType != _categoryType) continue;

                    if (categoriaPrincipal.Name.ToLower().Contains(".dwg")) continue;

                    Debug.WriteLine($"A{contador}) categoria analizada : {categoriaPrincipal.Name}");

                    // esto pendiente a desarrollar
                    if (COnsiderarListaExclusionOInclusion)
                    {
                        if (!ComprobarEnListaInclusion_CategoriaPrimariaOSecuandario(categoriaPrincipal, _categoryType, _view))
                            continue;
                    }
                    //Checks if elements of the given category are set to be invisible (hidden) in this view. 
                    bool IsCategoriaVISIBLE = !_view.GetCategoryHidden(categoriaPrincipal.Id);// categoriaPrincipal.get_Visible(_view);

                    categoriaPrincipal_ = new CategoriasDTO(categoriaPrincipal.Name, categoriaPrincipal, TipoCategoria.Principal, IsVisible: IsCategoriaVISIBLE);
                    ListElementoEncontrada.Add(categoriaPrincipal_);

                    int contador2 = 0;
                    CategoryNameMap subcats = categoriaPrincipal.SubCategories;
                    if (subcats.Size > 0)
                    {
                        List<CategoriasDTO> listaSubClases = new List<CategoriasDTO>();

                        foreach (Category _subCategoria in subcats)
                        {
                            contador2 = contador2 + 1;
                            Debug.WriteLine($"      B{contador2}) SUB categoria analizada : {_subCategoria.Name}");
                            if (COnsiderarListaExclusionOInclusion)
                            {
                                if (!ComprobarEnListaInclusion_CategoriaSecundaria(_subCategoria, _categoryType, _view)) continue;
                            }
                            if (_subCategoria.Parent.Name != categoriaPrincipal.Name) continue;
                            bool estadoCategoriaitem = !_view.GetCategoryHidden(_subCategoria.Id);

                            listaSubClases.Add(new CategoriasDTO(_subCategoria.Name, _subCategoria, TipoCategoria.Secuandario, IsVisible: estadoCategoriaitem));

                            contador2 = contador2 + 1;
                        }
                        if (listaSubClases.Count > 0)
                            categoriaPrincipal_.SubCategoria = listaSubClases;
                    }
         
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool ComprobarEnListaInclusion_CategoriaPrimariaOSecuandario(Category categoriaBuscar, CategoryType _categoryType, View _view)
        {
            try
            {
                if (ComprobarEnListaInclusion_CategoriaPrimaria(categoriaBuscar, _categoryType, _view)) return true;
                if (ComprobarEnListaInclusion_CategoriaSecundaria(categoriaBuscar, _categoryType, _view)) return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return false;
        }


        private bool ComprobarEnListaInclusion_CategoriaPrimaria(Category categoriaBuscar, CategoryType _categoryType, View _view)
        {
            if (ListaInclusionDto.Count == 0) return false;
            if (ListaInclusionDto.Exists(c => c.nombre == categoriaBuscar.Name) == false) return false;

            if (categoriaBuscar.CategoryType != _categoryType ||
                     categoriaBuscar.IsVisibleInUI == false ||
                     categoriaBuscar.get_AllowsVisibilityControl(_view) == false) return false;

            return true;
        }
        private bool ComprobarEnListaInclusion_CategoriaSecundaria(Category categoriabuscar, CategoryType _categoryType, View _view)
        {
            if (ListaInclusionDto.Count == 0) return false;
            if (ListaInclusionDto.SelectMany(c => c.SubCategoria).FirstOrDefault(c => c.nombre == categoriabuscar.Name) == null) return false;

            if (categoriabuscar.CategoryType != _categoryType ||
                     categoriabuscar.IsVisibleInUI == false ||
                     categoriabuscar.get_AllowsVisibilityControl(_view) == false) return false;

            return true;
        }

        private bool ComprobarEnListaInclusion_CategoriaPrimariaSecudario(Category categoriaSecundaria, Category categoriaPrincipal, View _view)
        {
            if (ListaInclusionDto.Count == 0) return false;
            if (ListaInclusionDto.Where(c => c.nombre == categoriaPrincipal.Name &&
                                           c.SubCategoria.Exists(j => j.nombre == categoriaSecundaria.Name) == true).FirstOrDefault() == null) return false;

            if (categoriaSecundaria.IsVisibleInUI == false ||
                     categoriaPrincipal.get_AllowsVisibilityControl(_view) == false)
                return true;

            return true;
        }

    }

}
