using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO
{
   public class CategoriasDTO
    {
        public string nombre { get; set; }
        public Category Category_ { get; set; }
        public TipoCategoria _TipoCategoria { get; set; }
        public bool IsVisible { get; }
        public CategoryType _CategoryType { get; set; }
        public List<CategoriasDTO> SubCategoria { get; set; }

        public CategoriasDTO(string name, Category categoriaPrincipal, TipoCategoria _TipoCategoria,bool IsVisible=true)
        {
            this.nombre = name;
            this.Category_ = categoriaPrincipal;
            this._TipoCategoria = _TipoCategoria;
            this.IsVisible = IsVisible;
            SubCategoria = new List<CategoriasDTO>();


        }
        public CategoriasDTO(string name, TipoCategoria _TipoCategoria, CategoryType _CategoryType, bool IsVisible=true)
        {
            this.nombre = name;  
            this._TipoCategoria = _TipoCategoria;
            this._CategoryType = _CategoryType;
            SubCategoria = new List<CategoriasDTO>();
            this.IsVisible = IsVisible;
        }

        internal void AgregarSecuandarios(string Nombre)
        {
            SubCategoria.Add( new CategoriasDTO(Nombre, TipoCategoria.Secuandario, _CategoryType,IsVisible));
        }
    }
}
