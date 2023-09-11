using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda
{
    public class AyudaAyudaComprobar_tipoCategoria_EstaEnUi_BlancoNegro
    {
        private static List<string> listaInclusion;

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
            listaInclusion = new List<string>();
            listaInclusionDto = new List<CategoriasDTO>();

            //1**
            CategoriasDTO _CategoriasDTORebar = new CategoriasDTO("Structural Rebar", TipoCategoria.Principal, CategoryType.Model);
            _CategoriasDTORebar.AgregarSecuandarios("Ø8"); _CategoriasDTORebar.AgregarSecuandarios("Ø10"); _CategoriasDTORebar.AgregarSecuandarios("Ø12");
            _CategoriasDTORebar.AgregarSecuandarios("Ø16"); _CategoriasDTORebar.AgregarSecuandarios("Ø18"); _CategoriasDTORebar.AgregarSecuandarios("Ø22");
            _CategoriasDTORebar.AgregarSecuandarios("Ø25"); _CategoriasDTORebar.AgregarSecuandarios("Ø28"); _CategoriasDTORebar.AgregarSecuandarios("Ø30");
            _CategoriasDTORebar.AgregarSecuandarios("Ø32");

            listaInclusionDto.Add(_CategoriasDTORebar);
            //*

            //2
            listaInclusionDto.Add(new CategoriasDTO("Structural Rebar Tags", TipoCategoria.Principal, CategoryType.Model));
            listaInclusionDto.Add(new CategoriasDTO("Floors", TipoCategoria.Principal, CategoryType.Model));
            listaInclusionDto.Add(new CategoriasDTO("Walls", TipoCategoria.Principal, CategoryType.Model));
            listaInclusionDto.Add(new CategoriasDTO("Wall Tags", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Generic Annotations", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Spot Elevations", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Structural Framing", TipoCategoria.Principal, CategoryType.Model));
            listaInclusionDto.Add(new CategoriasDTO("Structural Framing Tags", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Structural Path Reinforcement", TipoCategoria.Principal, CategoryType.Model));            
            listaInclusionDto.Add(new CategoriasDTO("Structural Path Reinforcement Tags", TipoCategoria.Principal, CategoryType.Annotation));

            CategoriasDTO _CategoriasDTOPAthSym = new CategoriasDTO("Structural Path Reinforcement Symbols", TipoCategoria.Principal, CategoryType.Annotation);
            _CategoriasDTOPAthSym.AgregarSecuandarios("Rebar"); _CategoriasDTOPAthSym.AgregarSecuandarios("Barra");
            listaInclusionDto.Add(_CategoriasDTOPAthSym);

            listaInclusionDto.Add(new CategoriasDTO("Room Tags", TipoCategoria.Principal, CategoryType.Annotation));
           
            listaInclusionDto.Add(new CategoriasDTO("Text Notes", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Generic Annotations", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Dimensions", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Grid", TipoCategoria.Principal, CategoryType.Annotation));
            listaInclusionDto.Add(new CategoriasDTO("Structural Foundations", TipoCategoria.Principal, CategoryType.Model));

            //1
            listaInclusion.Add("Structural Rebar");
            listaInclusion.Add("Ø8"); listaInclusion.Add("Ø10"); listaInclusion.Add("Ø12");
            listaInclusion.Add("Ø16"); listaInclusion.Add("Ø18"); listaInclusion.Add("Ø22");
            listaInclusion.Add("Ø25"); listaInclusion.Add("Ø28"); listaInclusion.Add("Ø30"); listaInclusion.Add("Ø32");

            listaInclusion.Add("Structural Rebar Tags");
            listaInclusion.Add("Floors");
            listaInclusion.Add("Walls");
            listaInclusion.Add("<Hidden Lines>");
            listaInclusion.Add("Wall Tags");
            listaInclusion.Add("Generic Annotations");
            listaInclusion.Add("Spot Elevations");
            listaInclusion.Add("Structural Framing"); listaInclusion.Add("Structural Framing Tags");
            listaInclusion.Add("Structural Path Reinforcement Tags");
            listaInclusion.Add("Structural Path Reinforcement");
            listaInclusion.Add("Structural Path Reinforcement Symbols");
            listaInclusion.Add("Rebar");
            listaInclusion.Add("Barra");
            listaInclusion.Add("Room Tags");
            listaInclusion.Add("Dimensiones");
            listaInclusion.Add("Text Notes");
            listaInclusion.Add("Generic Annotations");
            listaInclusion.Add("Dimensions");
            listaInclusion.Add("Grid");

            listaInclusion.Add("Structural Foundations");


        }
    }
}
