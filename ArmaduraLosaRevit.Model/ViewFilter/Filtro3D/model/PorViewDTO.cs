using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.modeloNH;

namespace ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.model
{
   public class PorViewDTO_
    {

        public  List<PorTiposNh> ListaLosa { get; set; } = new List<PorTiposNh>();
        public  List<PorTiposNh> ListaElev { get;  set; } = new List<PorTiposNh>();
    }
}
