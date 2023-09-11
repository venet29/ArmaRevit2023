using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoBarra
{
    internal class BarraVPataAmbosRefuerzo : BarraVPataAmbos, IbarraBase
    {

        public BarraVPataAmbosRefuerzo(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO, IGeometriaTag newGeometriaTag) : base(uiapp, itemIntervaloBarrasDTO, newGeometriaTag)
        {
            _BarraTipo = TipoRebar.REFUERZO_BA;
        }

    }
}