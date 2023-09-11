using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment
{
    class FactoryPathSymbol
    {

        public static IPathSymbol ObtenerPathSymbol(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO _DatosNuevaBarraDTO)
        {
            try
            {
                int _scaleView = uiapp.ActiveUIDocument.ActiveView.Scale;
                double largoPata_foot = _DatosNuevaBarraDTO.LargoPataFoot();

                switch (solicitudDTO.TipoBarra)
                {
                    case "f16a":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbol16ADTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbol16ADTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbol16ADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF16A(largoPata_foot, largoPata_foot);

                            return new PathSymbolF16A(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbol16ADTO);
                        }
                    case "f16b":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbol16BDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbol16BDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbol16BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF16B(largoPata_foot, largoPata_foot);

                            return new PathSymbolF16B(uiapp, solicitudDTO, _DatosNuevaBarraDTO, _PathSymbol16BDTO);
                        }
                    case "f17a":
                        {

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF17aDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF17aDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF17aDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF17A(largoPata_foot, largoPata_foot);

                            return new PathSymbolF17A(uiapp, _DatosNuevaBarraDTO, _PathSymbolF17aDTO);
                        }
                    case "f17b":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF17bDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF17bDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                                _PathSymbolF17bDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF17B(largoPata_foot, largoPata_foot);

                            return new PathSymbolF17B(uiapp, _DatosNuevaBarraDTO, _PathSymbolF17bDTO);
                        }
                    case "f18":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF18DTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF18(largoPata_foot, largoPata_foot);
                            return new PathSymbolF18(uiapp, _DatosNuevaBarraDTO, _PathSymbolF18DTO);
                        }
                    case "f19":
                        {
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF19DTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF19(largoPata_foot, largoPata_foot, largoPata_foot);
                            return new PathSymbolF19(uiapp, _DatosNuevaBarraDTO, _PathSymbolF19DTO);
                        }
                    case "f20aInv":
                    case "f20a":
                        {

                            //(solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Inferior || solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Izquierda)
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF20ADTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF20ADTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                            {
                                if (solicitudDTO.TipoBarra == "f20a")
                                    _PathSymbolF20ADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20A(largoPata_foot, largoPata_foot, 0);
                                else
                                    _PathSymbolF20ADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20A(largoPata_foot, 0, largoPata_foot);

                            }

                            return new PathSymbolF20A(uiapp, _DatosNuevaBarraDTO, _PathSymbolF20ADTO);
                        }
                    case "f20bInv":
                    case "f20b":
                        {
                            //(solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Inferior || solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Izquierda)
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF20BDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF20BDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                            {
                                if (solicitudDTO.TipoBarra == "f20b")
                                    _PathSymbolF20BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20B(largoPata_foot, largoPata_foot, 0);
                                else
                                    _PathSymbolF20BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20B(largoPata_foot, 0, largoPata_foot);


                            }

                            return new PathSymbolF20B(uiapp, _DatosNuevaBarraDTO, _PathSymbolF20BDTO);
                        }


                    case "f21a":
                        {
                            if (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Izquierda) new PathSymbolNull(uiapp, _DatosNuevaBarraDTO);
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF21aDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                            {
                                _PathSymbolF21aDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                                //_PathSymbolF21aDTO.DesIzqSup_foot = largoPata_foot;
                            }
                            else
                                _PathSymbolF21aDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF21A(largoPata_foot, largoPata_foot, largoPata_foot);
                            return new PathSymbolF21A(uiapp, _DatosNuevaBarraDTO, _PathSymbolF21aDTO);
                        }
                    case "f21b":
                        {

                            if (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Derecha) new PathSymbolNull(uiapp, _DatosNuevaBarraDTO);

                            PathSymbol_REbarshape_FxxDTO _PathSymbolF21bDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                            {
                                _PathSymbolF21bDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                                _PathSymbolF21bDTO.DesDereInf_foot = largoPata_foot;  // para mantener el desplazmiento igual que la pata
                            }
                            else
                                _PathSymbolF21bDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF21B(largoPata_foot, largoPata_foot, largoPata_foot);

                            return new PathSymbolF21B(uiapp, _DatosNuevaBarraDTO, _PathSymbolF21bDTO);
                        }
                    case "f22aInv":

                        PathSymbol_REbarshape_FxxDTO _PathSymbolF20AInvADTO = null;
                        if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                            _PathSymbolF20AInvADTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                        else
                        {

                            _PathSymbolF20AInvADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22A(0, largoPata_foot);
                        }

                        return new PathSymbolF22A(uiapp, _DatosNuevaBarraDTO, _PathSymbolF20AInvADTO);
                    case "f22a":
                        {

                            //(solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Inferior || solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Izquierda)
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF20ADTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF20ADTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                            {
                                _PathSymbolF20ADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22A(largoPata_foot, 0);
                                //if (solicitudDTO.TipoBarra == "f22a")
                                //    _PathSymbolF20ADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22A(largoPata_foot, 0);
                                //else
                                //    _PathSymbolF20ADTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22A(0, largoPata_foot);
                            }

                            return new PathSymbolF22A(uiapp, _DatosNuevaBarraDTO, _PathSymbolF20ADTO);
                        }
                    case "f22bInv":
                        PathSymbol_REbarshape_FxxDTO _PathSymbolF20BInvDTO = null;
                         
                        if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                            _PathSymbolF20BInvDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                        else
                        {
                            _PathSymbolF20BInvDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22B(largoPata_foot, 0);
                        }

                        return new PathSymbolF22B(uiapp, _DatosNuevaBarraDTO, _PathSymbolF20BInvDTO);
                    case "f22b":
                        {
                            //(solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Inferior || solicitudDTO.UbicacionEnlosa == Enumeraciones.UbicacionLosa.Izquierda)
                            PathSymbol_REbarshape_FxxDTO _PathSymbolF20BDTO = null;
                            if (_DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.IsOK)
                                _PathSymbolF20BDTO = _DatosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                            else
                            {
                                _PathSymbolF20BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22B(0, largoPata_foot);
                                //if (solicitudDTO.TipoBarra == "f22b")
                                //    _PathSymbolF20BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22B(0, largoPata_foot);
                                //else
                                //    _PathSymbolF20BDTO = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22B(largoPata_foot, 0);
                            }

                            return new PathSymbolF22B(uiapp, _DatosNuevaBarraDTO, _PathSymbolF20BDTO);
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


        //verificar si corresponde edicion segun el lado selecconado
        // sino corresponde deja el mismo pathsymbol
        public static bool VerificarSICOrresponde_ObtenerPathSymbol(SolicitudBarraDTO solicitudDTO)
        {
            try
            {
                switch (solicitudDTO.TipoBarra)
                {
                    case "f16a":
                        return true;
                    case "f16b":
                        return true;
                    case "f17a":
                        return true;
                    case "f17b":
                        return true;
                    case "f18":
                        return true;
                    case "f19":
                        return true;
                    case "f20a":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Izquierda ? false : true);
                    case "f20aInv":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Izquierda ? false : true);
                    case "f20b":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Derecha ? false : true);
                    case "f20bInv":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Derecha ? false : true);
                    case "f21a":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Izquierda ? false : true);
                    case "f21b":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Derecha ? false : true);
                    case "f22a":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Izquierda ? false : true);
                    case "f22b":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Derecha ? false : true);
                    case "f22aInv":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Izquierda ? false : true);
                    case "f22bInv":
                        return (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Derecha ? false : true);
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'VerificarSICOrresponde_ObtenerPathSymbol' \nex:{ex.Message}");
                return false;

            }

        }

    }
}
