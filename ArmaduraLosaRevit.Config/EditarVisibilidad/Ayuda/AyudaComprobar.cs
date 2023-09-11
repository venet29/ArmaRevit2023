using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad.Ayuda
{
    public class AyudaComprobar
    {
   



        public static bool Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaExclusion(Category categoriaPrincipal, CategoryType _categoryType, List<string> _listaExclusion,View _view)
        {
            if (_listaExclusion.Exists(c => c.ToString() == categoriaPrincipal.Name) == true) return true;

            if (categoriaPrincipal.CategoryType != _categoryType ||
                     categoriaPrincipal.IsVisibleInUI == false ||
                     categoriaPrincipal.get_AllowsVisibilityControl(_view) == false ) return true;

            return false;
        }

        public static bool Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaIncluir(Category categoriaPrincipal, List<string> _listaInclusion)
        {
            if (categoriaPrincipal.IsVisibleInUI == true &&

              (_listaInclusion.Exists(c => c.ToString() == categoriaPrincipal.Name) == true)) return true;

            return false;
        }


    }
}
