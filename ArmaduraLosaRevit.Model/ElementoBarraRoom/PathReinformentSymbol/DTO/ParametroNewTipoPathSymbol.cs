using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO
{
    public class ParametroNewTipoPathSymbol
    {


        public ParametroNewTipoPathSymbol(string nombreFamiliaBase_conEscala, List<PathReinforceSymbolDTO> listaPara)
        {
            this.nombreNewFamilyPathSYmbol = nombreFamiliaBase_conEscala;
            this._listaPrametros = listaPara;
        }

        public string nombreNewFamilyPathSYmbol { get; set; }
        public List<PathReinforceSymbolDTO> _listaPrametros { get; set; }
       
    }
}
