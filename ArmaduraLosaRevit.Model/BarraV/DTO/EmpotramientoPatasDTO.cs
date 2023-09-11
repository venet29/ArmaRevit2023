using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class EmpotramientoPatasDTO
    {
        public TipoEmpotramiento _conEmpotramientoDereSup { get; set; }
        public TipoEmpotramiento _conEmpotramientoIzqInf { get; set; }


        public TipoPataBarra TipoPataIzqInf { get; set; }

        public TipoPataBarra TipoPataDereSup { get; set; }
        

    }
}
