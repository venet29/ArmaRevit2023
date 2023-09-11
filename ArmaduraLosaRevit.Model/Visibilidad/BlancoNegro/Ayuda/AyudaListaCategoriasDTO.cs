using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.Ayuda
{
    public class AyudaListaCategoriasDTO
    {
        public static CategoriasDTO OBtenerLineaBarras()
        {
            CategoriasDTO _CategoriasDTOTipo = new CategoriasDTO("Barra", TipoCategoria.Secuandario, CategoryType.Model);
            CategoriasDTO _CategoriasDTOLinea = new CategoriasDTO("Lines", TipoCategoria.Principal, CategoryType.Model);
            _CategoriasDTOLinea.SubCategoria.Add(_CategoriasDTOTipo);

            return _CategoriasDTOLinea;
        }

    }
}
