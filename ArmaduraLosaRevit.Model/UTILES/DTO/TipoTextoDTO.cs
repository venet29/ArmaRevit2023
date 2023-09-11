using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.DTO
{
   public  class TipoTextoDTO
    {
        public string _Nombre { get; set; }
        public int _red { get; set; }
        public int _green { get; set; }
        public int _blue { get; set; }
        public string _TEXT_FONT { get; set; }
        public double _TEXT_SIZE { get; set; }
        public double _TEXT_TAB_SIZE { get; set; }
        public TipoTextoDTO(string Nombre, int _red, int _green, int _blue, string _TEXT_FONT, double _TEXT_SIZE, double _TEXT_TAB_SIZE)
        {
            this._Nombre = Nombre;
            this._red = _red;
            this._green = _green;
            this._blue = _blue;
            this._TEXT_FONT = _TEXT_FONT;
            this._TEXT_SIZE = _TEXT_SIZE;
            this._TEXT_TAB_SIZE = _TEXT_TAB_SIZE;
        }
    }
}
