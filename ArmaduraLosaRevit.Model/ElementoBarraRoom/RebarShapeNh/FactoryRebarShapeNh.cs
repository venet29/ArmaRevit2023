using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Tipos;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh
{
    class FactoryRebarShapeNh
    {

        public static IRebarShapeNh ObtenerRebarShapeNh(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO _DatosNuevaBarraDTO)
        {
            try
            {
                int _scaleView = uiapp.ActiveUIDocument.ActiveView.Scale;
                double largoPataFoot = _DatosNuevaBarraDTO.LargoPataFoot();

                PathSymbol_REbarshape_FxxDTO _PathSymbolFXXADTO = null;
                if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                    _PathSymbolFXXADTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                else
                    _PathSymbolFXXADTO = FactoryPathSymbol_REbarshape_FxxDTO.Default();


                switch (solicitudDTO.TipoBarra)
                {
                    case "f16a":
                        return new RebarShapeNhF16A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f16b":
                        return new RebarShapeNhF16B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f17a":
                        return new RebarShapeNhF17A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f17b":
                        return new RebarShapeNhF17B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);

                    case "f18":
                        return new RebarShapeNhF18(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f19":
                        return new RebarShapeNhF19(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);

                    case "f20a":
                        return new RebarShapeNhF20A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f20b":
                        return new RebarShapeNhF20B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f20aInv":
                        return new RebarShapeNhF20A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f20bInv":
                        return new RebarShapeNhF20B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);

                    case "f21a":
                        return new RebarShapeNhF21A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f21b":
                        return new RebarShapeNhF21B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f22a":
                        return new RebarShapeNhF22A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f22b":
                        return new RebarShapeNhF22B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f22aInv":
                        return new RebarShapeNhF22A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    case "f22bInv":
                        return new RebarShapeNhF22B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolFXXADTO);
                    default:
                        return new REbarShapeNHFXXDTONull(uiapp, solicitudDTO, _DatosNuevaBarraDTO);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Tipo pathReinformentSymbol \nex:{ex.Message}");
                return null;

            }

        }

    }
}
