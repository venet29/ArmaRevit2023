using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf.Servicios
{
    class FactoryPathSymbolV2
    {
        public static IPathSymbol ObtenerPathSymbol(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO _DatosNuevaBarraDTO)
        {
            try
            {
                int _scaleView = uiapp.ActiveUIDocument.ActiveView.Scale;
                double largoPata_foot = _DatosNuevaBarraDTO.LargoPataFoot();

                switch (solicitudDTO.TipoBarra.ToLower())
                {
                    case "f1a":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbol1ADTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbol1ADTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbol1ADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF1A(largoPata_foot, largoPata_foot);

                            return new PathSymbolF16A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbol1ADTO);
                        }
                    case "f1b":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbol1BDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbol1BDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbol1BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF1B(largoPata_foot, largoPata_foot);

                            return new PathSymbolF16B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbol1BDTO);
                        }

                    case "f3_refuezosuple":
                    case "f3":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbol1BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigFf3(); ;
                            return new PathSymbolF3(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbol1BDTO);
                        }
                        
                    case "f4a":
                        {

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF4aDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF4aDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF4aDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF4A(largoPata_foot, largoPata_foot);

                            return new PathSymbolF17A(uiapp, _DatosNuevaBarraDTO, _PathSymbolF4aDTO);
                        }
                    case "f4b":
                        {

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF4bDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF4bDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF4bDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF4B(largoPata_foot, largoPata_foot);

                            return new PathSymbolF17A(uiapp, _DatosNuevaBarraDTO, _PathSymbolF4bDTO);
                        }
                    case "f11":
                        {

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF11DTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF11DTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF11DTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF11(Util.CmToFoot(_DatosNuevaBarraDTO.LargoPAtaIzqHook_cm), Util.CmToFoot(_DatosNuevaBarraDTO.LargoPAtaDereHook_cm));

                            return new PathSymbolF11(uiapp, solicitudDTO,_DatosNuevaBarraDTO, _PathSymbolF11DTO);
                        }
                    case "f11a":
                        {

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF11DTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF11DTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF11DTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF11(Util.CmToFoot(_DatosNuevaBarraDTO.LargoPAtaIzqHook_cm),0);

                            return new PathSymbolF11A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolF11DTO);
                        }
                    case "f11b":
                        {

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF11DTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF11DTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF11DTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF11(0, Util.CmToFoot(_DatosNuevaBarraDTO.LargoPAtaDereHook_cm));

                            return new PathSymbolF11B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolF11DTO);
                        }
                    case "f12":
                        {

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF12DTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF12DTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF12DTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF12(_DatosNuevaBarraDTO.LargoPAtaIzqHook_cm, _DatosNuevaBarraDTO.LargoPAtaDereHook_cm);

                            return new PathSymbolF12(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbolF12DTO);
                        }
                    default:
                        return new PathSymbolNull(uiapp, _DatosNuevaBarraDTO);
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
