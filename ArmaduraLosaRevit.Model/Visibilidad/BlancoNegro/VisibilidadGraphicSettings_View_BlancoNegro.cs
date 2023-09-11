using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro
{

    public class VisibilidadGraphicSettings_View_BlancoNegro
    {
        protected UIApplication uiapp;
        protected View _view;
        protected Document _doc;
        protected List<CategoriasDTO> ListElementoVisibles;

        public Dictionary<string, List<Element>> ListaResult { get; private set; }

        public VisibilidadGraphicSettings_View_BlancoNegro(UIApplication uiapp,View view)
        {
            this.uiapp = uiapp;
            this._view = view;
            this._doc = _view.Document;
            this.ListElementoVisibles = new List<CategoriasDTO>();
        }

        public bool M1_CambiarColor_BlancoONegro(bool estado, CategoryType _categoryType)
        {
            M0_ObtenerListaElementosVisibles(_categoryType);

            if (ListElementoVisibles.Count == 0) return false;

            //desactivar viewtempalte
            //_view.DesactivarViewTemplate_ConTrans();

            int i = 0;
            string namecageroria = "";
            // estado true desactivar
            //        false activar 
            try
            {
                using (Transaction t = new Transaction(_view.Document))
                {
                    t.Start($"CambioBancoInegro3-NH");
                    for (i = 0; i < ListElementoVisibles.Count; i++)
                    {
                        var item = ListElementoVisibles.ElementAt(i);
                        //var _nombre = item.nombre;
                        Category _categoria = item.Category_;
                        //namecageroria = _categoria.Name;
                        // categoria princiapal
                        M1_1_cambiarColorCategory(estado, _categoria);

                        //sub categoria
                        foreach (CategoriasDTO subItem in item.SubCategoria)
                        {
                            M1_1_cambiarColorCategory(estado, subItem.Category_);
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en el formato blanco negro en '{_categoryType}' en la posicion {i}  --> nombre:{namecageroria}");
                string msj = ex.Message;
                return false;
            }
            return true;
        }

        private void M1_1_cambiarColorCategory(bool estado, Category _categoria)
        {

            OverrideGraphicSettings ogs = new OverrideGraphicSettings();

            if (estado)
            {
                ogs = FactoryBlancoNegro.ObtenerOverrideGraphicSettings_BlancoNegro(_categoria.Name, _doc);
                if (ogs == null) return;
            }
            else
            {
                if (_categoria.Name.Contains("Membrane Layer"))
                { }
                var ogsAux = FactoryBlancoNegro.ObtenerOverrideGraphicSettings_Normal(_categoria.Name, _doc);
                if (ogsAux != null)
                    ogs = ogsAux;

            }

            _view.SetCategoryOverrides(_categoria.Id, ogs); // 2017
        }

        protected void M0_ObtenerListaElementosVisibles(CategoryType _categoryType)
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
            int contador = 1;
            foreach (Category categoriaPrincipal in categories)
            {
               
                CategoriasDTO  categoriaPrincipal_=null;

                Debug.WriteLine($"A) categoria analizada : {categoriaPrincipal.Name}    contador:{contador}");
                if (!AyudaAyudaComprobar_tipoCategoria_EstaEnUi_BlancoNegro.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaInclusion(categoriaPrincipal, _categoryType, _view,TipoCategoria.Principal)) continue;

                //Checks if elements of the given category are set to be invisible (hidden) in this view. 
                bool IsCategoriaVISIBLE = !_view.GetCategoryHidden(categoriaPrincipal.Id);// categoriaPrincipal.get_Visible(_view);
                if (IsCategoriaVISIBLE == ISvisible)//True if the category is invisible (hidden), false otherwise.
                {
                    categoriaPrincipal_ = new CategoriasDTO(categoriaPrincipal.Name, categoriaPrincipal, TipoCategoria.Principal,IsVisible: IsCategoriaVISIBLE);
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

                        if (!AyudaAyudaComprobar_tipoCategoria_EstaEnUi_BlancoNegro.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaIncluir(item, categoriaPrincipal,_view)) continue;

                        if(item.Parent.Name!= categoriaPrincipal.Name) continue;
                        bool estadoCategoriaitem = !_view.GetCategoryHidden(item.Id);
                        if (estadoCategoriaitem == ISvisible)
                            listaSubClases.Add(new CategoriasDTO(item.Name, item, TipoCategoria.Secuandario, IsVisible: IsCategoriaVISIBLE));

                        contador2 = contador2+1;
                    }

                    if (listaSubClases.Count > 0)
                        categoriaPrincipal_.SubCategoria = listaSubClases;                 
                }
                contador = contador+1;
            }
        }


        public bool M2_CambiarColor_BlancoONegro_porElemento(bool estado)//, BuiltInCategory _BuiltInCategory, string nameCategoriaFactory)
        {
            //        false activar 
            int i = 0;
            string nameCategoriaFactory = "";
            try
            {
                using (Transaction t = new Transaction(_view.Document))
                {
                    t.Start($"CambioBancoInegro4-NH");


                    if (!M2_1_ObtenerListaElementos()) return false;

                    for (int x = 0; x < ListaResult.Count; x++)
                    {
                        nameCategoriaFactory = ListaResult.Keys.ElementAt(x);
                        List<Element> ListElement = ListaResult[ListaResult.Keys.ElementAt(x)];

                        // Element indTag = ListElement[x];
                        OverrideGraphicSettings ogs = new OverrideGraphicSettings();

                        if (estado)
                        {
                            ogs = FactoryBlancoNegro.ObtenerOverrideGraphicSettings_BlancoNegro(nameCategoriaFactory, _doc);
                            if (ogs == null) continue;
                        }
                        else
                        {
                            var ogsAux = FactoryBlancoNegro.ObtenerOverrideGraphicSettings_Normal(nameCategoriaFactory, _doc);
                            if (ogsAux != null)
                                ogs = ogsAux;
                        }


                        for (i = 0; i < ListElement.Count; i++)
                        {
                            _view.SetElementOverrides(ListElement[i].Id, ogs); // 2017
                        }
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en el formato blanco negro por elemento en '{nameCategoriaFactory}' en la posicion {i}");
                string msj = ex.Message;
                return false;
            }
            return true;
        }

        private bool M2_1_ObtenerListaElementos()
        {
            try
            {
                Dictionary<string, BuiltInCategory> listaiNPUT = new Dictionary<string, BuiltInCategory>()
                    {
                   
                     { "Grids",BuiltInCategory.OST_Grids },
                        { "Structural Rebar Tags" ,BuiltInCategory.OST_RebarTags },
                        {  "Structural Rebar",BuiltInCategory.OST_Rebar },
                        { "Structural Path Reinforcement Tags", BuiltInCategory.OST_PathReinTags },
                       {  "Dimensions",BuiltInCategory.OST_Dimensions},
                       { "Room Tags",BuiltInCategory.OST_RoomTags },
                        { "Text Notes",BuiltInCategory.OST_TextNotes },
                       { "Generic Annotations",BuiltInCategory.OST_GenericAnnotation },
                       { "Detail Groups",BuiltInCategory.OST_IOSDetailGroups },
                       { "Wall Tags",BuiltInCategory.OST_WallTags },
                       { "Structural Framing Tags",BuiltInCategory.OST_StructuralFramingTags }

                    };

                ListaResult = new Dictionary<string, List<Element>>();
                foreach (var item in listaiNPUT)
                {
                    string nombre = item.Key;
                    BuiltInCategory tipo = item.Value;

                    TiposPorBuiltInCategory _TiposBuiltInCategory = new TiposPorBuiltInCategory(_doc, _view);
                    if (_TiposBuiltInCategory.cargarListaDeTagRebar(tipo))
                    {
                        ListaResult.Add(nombre, _TiposBuiltInCategory.ListaElemento);
                    }


                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'ObtenerListaElementos'   ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
