using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Horizontal;
using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public class FactoryBarraHorizontal
    {
        internal static IbarraBase GeneraraIbarraHorizontal(UIApplication uiapp, IntervaloBarrasDTO itemIntervaloBarrasDTO, IGeometriaTag _newGeometriaTag)
        {
            

            switch (itemIntervaloBarrasDTO.tipobarraV)
            {
                case Enumeraciones.TipoPataBarra.BarraVPataInicial:
                    // IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataInicialH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVPataFinal:
                    //  IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataFinalH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVSinPatas:

                    return new BarraVSinPatasH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                case Enumeraciones.TipoPataBarra.BarraVPataAmbos:
                    //IGeometriaTag _newGeometriaTag = null;
                    return new BarraVPataAmbosH(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
                default:
                    return new BarraVNULL(uiapp, itemIntervaloBarrasDTO, _newGeometriaTag);
            }
        }
    }
}
