using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FAMILIA;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos
{
 
    class PathSymbolNull : ChangeFamilyPathReinforceSymbol, IPathSymbol
    {


        public PathSymbolNull(UIApplication uiapp, DatosNuevaBarraDTO _DatosNuevaBarraDTO) : base(uiapp, _DatosNuevaBarraDTO)
        {
        }


        public bool M1_ObtenerPArametros() => false;

        public bool M2_ejecutar(ParametroNewTipoPathSymbol TipoFamilia_conpata = null) =>  false;

    }
}
