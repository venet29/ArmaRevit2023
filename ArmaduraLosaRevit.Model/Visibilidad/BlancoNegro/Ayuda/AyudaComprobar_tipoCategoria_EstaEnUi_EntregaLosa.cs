using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda
{
    public class AyudaComprobar_tipoCategoria_EstaEnUi_EntregaLosa
    {
      
        private static List<CategoriasDTO> listaInclusionDto;

        public static bool Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaInclusion(Category categoriaPrincipal, CategoryType _categoryType, View _view, TipoCategoria _TipoCategoria)
        {
            if (listaInclusionDto == null) ObtenerListaExclusion();
            if (listaInclusionDto.Count==0) ObtenerListaExclusion();

            if (_TipoCategoria == TipoCategoria.Principal)
            {
                //si no lo encuentra
                if (listaInclusionDto.Exists(c=>c.nombre== categoriaPrincipal.Name) == false) return false;
            }
            else
            {
                if (listaInclusionDto.SelectMany(c=>c.SubCategoria).FirstOrDefault(c => c.nombre == categoriaPrincipal.Name) == null) return false;
            }
            

            if (categoriaPrincipal.CategoryType != _categoryType ||
                     categoriaPrincipal.IsVisibleInUI == false ||
                     categoriaPrincipal.get_AllowsVisibilityControl(_view) == false) return false;

            return true;
        }
        public static bool Comprobar_tipoCategoria_EstaEnUi_IsModificarVisibi_EnListaIncluir(Category categoriaSecundaria, Category categoriaPrincipal,View _view)
        {
            if (listaInclusionDto == null) ObtenerListaExclusion();
            if (listaInclusionDto.Count == 0) ObtenerListaExclusion();

            if (listaInclusionDto.Where(c=>c.nombre== categoriaPrincipal.Name &&
                                           c.SubCategoria.Exists(j=>j.nombre== categoriaSecundaria.Name)==true).FirstOrDefault()== null) return false;

            if (categoriaSecundaria.IsVisibleInUI == false ||
                     categoriaPrincipal.get_AllowsVisibilityControl(_view) == false)
                return true;

            return true;
        }



        private static void ObtenerListaExclusion()
        {
       
            listaInclusionDto = new List<CategoriasDTO>();

            //1**
            listaInclusionDto.Add(new CategoriasDTO("Structural Rebar", TipoCategoria.Principal, CategoryType.Model,true));
            listaInclusionDto.Add(new CategoriasDTO("Walls", TipoCategoria.Principal, CategoryType.Model, true));
            listaInclusionDto.Add(new CategoriasDTO("Structural Framing", TipoCategoria.Principal, CategoryType.Model, true));
            listaInclusionDto.Add(new CategoriasDTO("Structural Columns", TipoCategoria.Principal, CategoryType.Model, true));

            CategoriasDTO _CategoriasDTOPAthSym = new CategoriasDTO("Structural Path Reinforcement Symbols", TipoCategoria.Principal, CategoryType.Annotation, true);
            _CategoriasDTOPAthSym.AgregarSecuandarios("Rebar"); _CategoriasDTOPAthSym.AgregarSecuandarios("Barra");
            listaInclusionDto.Add(_CategoriasDTOPAthSym);
            return;

        }
    }
}
