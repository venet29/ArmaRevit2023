using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad.Ayuda;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad
{

    public class VisibilidadCategorias
    {
        private View _view;
        private  Document _doc;

        public VisibilidadCategorias(View view)
        {
            this._view = view;
            this._doc = _view.Document;
        }

        public void CambiarEstado_ConlistaExclusion(bool EstadoOculto, CategoryType _categoryType, List<string> _listaExclusion)
        {

            Categories categories = _doc.Settings.Categories;
            List<ElementId> ListElementoVisibles = new List<ElementId>();

            foreach (Category categoriaPrincipal in categories)
            {
                if (AyudaComprobar.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaExclusion(categoriaPrincipal, _categoryType, _listaExclusion,_view)) continue;

                bool IsCategoriaInvisible = _view.GetCategoryHidden(categoriaPrincipal.Id);// categoriaPrincipal.get_Visible(_view);
                if (IsCategoriaInvisible != EstadoOculto)
                    ListElementoVisibles.Add(categoriaPrincipal.Id);

                CategoryNameMap subcats = categoriaPrincipal.SubCategories;
                if (subcats.Size > 0)
                {
                    foreach (Category item in subcats)
                    {
                        if (AyudaComprobar.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaExclusion(item, _categoryType, _listaExclusion,_view)) continue;

                        bool estadoCategoriaitem = _view.GetCategoryHidden(item.Id);
                        if (estadoCategoriaitem != EstadoOculto)
                            ListElementoVisibles.Add(item.Id);
                    }
                }
            }

            if (ListElementoVisibles.Count == 0) return;

            // estado true desactivar
            //        false activar 
            EjecutarCambios(EstadoOculto, ListElementoVisibles);
        }


        public void CambiarEstado_ListaAplicar(bool EstadoOculto, List<string> _listaAplicar)
        {

            Categories categories = _doc.Settings.Categories;
            List<ElementId> ListElementoVisibles = new List<ElementId>();

            foreach (Category categoriaPrincipal in categories)
            {
                if (!AyudaComprobar.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaIncluir(categoriaPrincipal,  _listaAplicar)) continue;

                bool IsCategoriaInvisible = _view.GetCategoryHidden(categoriaPrincipal.Id);// categoriaPrincipal.get_Visible(_view);

                bool isget_Visible = categoriaPrincipal.get_Visible(_view);
                bool isget_AllowsVisibilityControl = categoriaPrincipal.get_AllowsVisibilityControl(_view);
                bool isIsCuttable = categoriaPrincipal.IsCuttable;

                if (IsCategoriaInvisible != EstadoOculto)
                    ListElementoVisibles.Add(categoriaPrincipal.Id);


                CategoryNameMap subcats = categoriaPrincipal.SubCategories;
                if (subcats.Size > 0)
                {
                    foreach (Category item in subcats)
                    {
                        if (!AyudaComprobar.Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaIncluir(item,  _listaAplicar)) continue;

                        bool estadoCategoriaitem = _view.GetCategoryHidden(item.Id);
                        if (estadoCategoriaitem != EstadoOculto)
                            ListElementoVisibles.Add(item.Id);
                    }

                }

            }

            if (ListElementoVisibles.Count == 0) return;

            // estado true desactivar
            //        false activar 
            EjecutarCambios(EstadoOculto, ListElementoVisibles);

        }


        private void EjecutarCambios(bool EstadoOculto, List<ElementId> ListElementoVisibles)
        {
            try
            {
                using (Transaction t = new Transaction(_view.Document))
                {

                    t.Start($"CambioAct-NH");
                    for (int i = 0; i < ListElementoVisibles.Count; i++)
                    {
                        _view.SetCategoryHidden(ListElementoVisibles[i], EstadoOculto);
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }






        public void verTodasCategorias()
        {


            Document doc = _view.Document;
            Categories categories = doc.Settings.Categories;
            int cont = 0;
            foreach (Category categoriaPrincipal in categories)
            {
                Debug.WriteLine($"{cont += 1}) ,   {categoriaPrincipal.Name}  ,   ,    {categoriaPrincipal.CategoryType.ToString()}");

                CategoryNameMap subcats = categoriaPrincipal.SubCategories;
                if (subcats.Size > 0)
                {
                    foreach (Category item in subcats)
                    {
                        Debug.WriteLine($"  ,  ,   {item.Name}    ,   {item.CategoryType.ToString()}");
                    }
                }
            }
        }
    }
}
