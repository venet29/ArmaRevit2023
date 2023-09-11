using Autodesk.Revit.DB;
using System;
using System.Diagnostics;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad
{



    public class VisibilidadView : IVisibilidadView
    {
        private View _view;
        private BuiltInCategory _nameBuiltInCategory;
        private string _nameParametro;
        //  private bool actual;

        private bool _estadoActualVisibilidad;
        private Category _categoriaAnalizada;
        private bool _isOKVisibilidad;

        //HideLightingFixtureHosts(doc.ActiveView, BuiltInCategory.OST_Rooms,"Interior Fill",false);

        public static IVisibilidadView Creador_Visibilidad(View view, BuiltInCategory nameBuiltInCategory, string name)
        {
            VisibilidadView visibilidad = new VisibilidadView(view, nameBuiltInCategory, name);

            if (visibilidad.BuscarVisibilityActualBuiltInCategory())
                return visibilidad;

            return new VisibilidadViewNUll();

        }

        public static VisibilidadView Creador_Visibilidad_SinInterfase(View view, BuiltInCategory nameBuiltInCategory, string name)
        {
            VisibilidadView visibilidad = new VisibilidadView(view, nameBuiltInCategory, name);

            if (visibilidad.BuscarVisibilityActualBuiltInCategory())
                return visibilidad;

            return null;

        }

        public static IVisibilidadView Creador_Visibilidad_ViewTipo(View view)
        {

            VisibilidadView visibilidad = new VisibilidadView(view);

            return visibilidad;


        }
   

        private VisibilidadView(View view)
        { this._view = view;
        }
        private VisibilidadView(View view, BuiltInCategory nameBuiltInCategory, string name)
        {
            this._view = view;
            this._nameBuiltInCategory = nameBuiltInCategory;
            this._nameParametro = name;
            this._estadoActualVisibilidad = false;
            this._categoriaAnalizada = null;
            this._isOKVisibilidad = false;
        }

        public bool EstadoActualHide() => _estadoActualVisibilidad;


        public bool IsOKVisibilidad() => _isOKVisibilidad;
        private bool BuscarVisibilityActualBuiltInCategory()
        {


            // verTodasCategorias();
            try
            {
                using (Transaction t = new Transaction(_view.Document))
                {
                    t.Start("Buscar estado visibilidad-NH");
                    _categoriaAnalizada = null;
                    Document doc = _view.Document;
                    Categories categories = doc.Settings.Categories;
                    Category categoriaPrincipal = categories.get_Item(_nameBuiltInCategory);



                    CategoryNameMap subcats = categoriaPrincipal.SubCategories;


                    if (subcats.Size > 0)
                    {
                        if (subcats.Contains(_nameParametro))
                        {
                            Category catHosts = subcats.get_Item(_nameParametro);
                            _categoriaAnalizada = catHosts;
                        }
                        else
                            _categoriaAnalizada = categoriaPrincipal;
                    }
                    else
                    {
                        _categoriaAnalizada = categoriaPrincipal;
                    }
                    Debug.WriteLine(_categoriaAnalizada.Name);
                    _estadoActualVisibilidad = _view.GetCategoryHidden(_categoriaAnalizada.Id);
                    _isOKVisibilidad = true;


                    t.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }


            return false;
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

        public void CambiarVisibilityBuiltInCategory()
        {
            AsignarVisibilityBuiltInCategory(!_estadoActualVisibilidad);
        }

        public void CambiarVisibilityBuiltInCategory(bool cambiarEstado)
        {
            AsignarVisibilityBuiltInCategory(cambiarEstado);
        }
        public void AsignarVisibilityBuiltInCategory(bool estado)
        {

            // estado true desactivar
            //        false activar 

            try
            {
                using (Transaction t = new Transaction(_view.Document))
                {
                    t.Start("Asignar estado visibilidad-NH");
                    _view.SetCategoryHidden(_categoriaAnalizada.Id, estado); // 2017
                    _estadoActualVisibilidad = estado;
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

        }

    }
}
