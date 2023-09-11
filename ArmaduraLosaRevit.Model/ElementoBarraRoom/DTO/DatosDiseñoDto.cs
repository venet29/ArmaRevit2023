using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO
{
    public class DatosDiseñoDto
    {

        public bool IsConsiderarDatosIniciales { get; set; }
        public int DiamnetroMM { get; set; } // 8,10
        public double espaciamientoCm { get; set; } //"8a20"
        public string TiposBarras { get; set; } //"8a20"

        public PathSymbol_REbarshape_FxxDTO PathSymbol_REbarshape_FXXDTO_ { get; set; }
        public Ui_pathSymbolDTO Ui_pathSymbolDTO_ { get; internal set; }

        public DatosDiseñoDto()
        {
            IsConsiderarDatosIniciales = false;
        }

        
    }
}
