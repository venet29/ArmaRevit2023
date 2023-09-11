using ArmaduraLosaRevit.Model.BarraV.TipoTagH;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagFundRebarDerePata : GeomeTagFundRebarAmbasPata_auto
    {
  
        public GeomeTagFundRebarDerePata(UIApplication uiapp, XYZ ptoMOuse, PathReinfSeleccionDTO _PathReinfSeleccionDTO) :
            base(uiapp, ptoMOuse, _PathReinfSeleccionDTO)
        {

        }

    }
}
