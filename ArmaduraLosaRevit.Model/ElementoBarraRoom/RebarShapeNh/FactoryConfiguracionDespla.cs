using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh
{
    class FactoryConfiguracionDespla
    {

        public static PathSymbol_REbarshape_FxxDTO ObtenerConfig(string _TipoBarra, TipoDireccion _TipoDireccion_, ManejadorEditarREbarShapYPAthSymbol_DTO _ManejadorEditarREbarShapYPAthSymbol_DTO)
        {
            try
            {
                double LargoAhoraDefinidoUsuario_foot = _ManejadorEditarREbarShapYPAthSymbol_DTO.LargoAhoraDefinidoUsuario_foot- ConstNH.CONST_EXTRADESDFASE_FOOT;
                double LargoAhoraDefinidoUsuario_foot_SIN = _ManejadorEditarREbarShapYPAthSymbol_DTO.LargoAhoraDefinidoUsuario_foot;
                double LargoAlternativeOffset_foot = _ManejadorEditarREbarShapYPAthSymbol_DTO.DatosPathRinforment.LargoAlternativeOffset_foot;
#pragma warning disable CS0219 // The variable 'largoPata_foot' is assigned but its value is never used
                double largoPata_foot = 0;// _ManejadorEditarREbarShapYPAthSymbol_DTO.DatosPathRinforment.LargoPataFoot();
#pragma warning restore CS0219 // The variable 'largoPata_foot' is assigned but its value is never used

                switch (_TipoBarra)
                {
                    case "f16a":
                        {
                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                            {
                                var LargoDerecho_foot = AyudaObtenerLargoAhorroF16.ObtenerAhorroDerecho_Foot(_ManejadorEditarREbarShapYPAthSymbol_DTO);
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF16A(LargoAhoraDefinidoUsuario_foot_SIN, LargoDerecho_foot);
                            }
                            else
                            {
                                var LargoIzq_foot = AyudaObtenerLargoAhorroF16.ObtenerAhorroIzq(_ManejadorEditarREbarShapYPAthSymbol_DTO);
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF16A(LargoIzq_foot, LargoAhoraDefinidoUsuario_foot_SIN);
                            }
                        }
                    case "f16b":
                        //return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF16A(largoPata_foot, largoPata_foot);
                        return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF16B(LargoAhoraDefinidoUsuario_foot, LargoAhoraDefinidoUsuario_foot); // desactivar si se quiere invertir la posicion del barra primaria y barra secuandaria
                    case "f17a":
                        if (_TipoDireccion_ == TipoDireccion.Izquierda)
                            return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                        else
                            return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF17A(-1, LargoAhoraDefinidoUsuario_foot);
                    case "f17b":
                        if (_TipoDireccion_ == TipoDireccion.Izquierda)
                            return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF17B(LargoAhoraDefinidoUsuario_foot, -1);
                        else
                            return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                    case "f18":
                        return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF18(LargoAhoraDefinidoUsuario_foot, LargoAhoraDefinidoUsuario_foot);
                    case "f19":
                        return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF19(LargoAhoraDefinidoUsuario_foot, LargoAhoraDefinidoUsuario_foot, LargoAhoraDefinidoUsuario_foot);
                    case "f20a":
                        {
                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20A(0, LargoAhoraDefinidoUsuario_foot, 0);

                        }
                    case "f20aInv":
                        {
                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20A(0, 0, LargoAhoraDefinidoUsuario_foot);
                        }                      
                    case "f20b":
                        {
                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20B(0, LargoAhoraDefinidoUsuario_foot, 0);
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                        }                        
                    case "f20bInv":
                        {
                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20B(0, 0, LargoAhoraDefinidoUsuario_foot);
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                        }                     
                    case "f21a":
                        if (_TipoDireccion_ == TipoDireccion.Izquierda)
                            return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                        else
                            return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF21A(-1, -1, LargoAhoraDefinidoUsuario_foot);

                    case "f21b":
                        if (_TipoDireccion_ == TipoDireccion.Izquierda)
                            return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF21B(-1, LargoAhoraDefinidoUsuario_foot, -1);
                        else
                            return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                      

                    case "f22a":
                        {

                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22A(LargoAhoraDefinidoUsuario_foot, 0);
                        }

                    case "f22aInv":
                        {

                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22A( 0, LargoAhoraDefinidoUsuario_foot);
                        }
                    case "f22b":
                        {
                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22B(0, LargoAhoraDefinidoUsuario_foot);
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();
                           
                        }
                    case "f22bInv":
                        {
                            if (_TipoDireccion_ == TipoDireccion.Izquierda)
                                return FactoryPathSymbol_REbarshape_FxxDTO.ConfigF22B(LargoAhoraDefinidoUsuario_foot, 0);
                            else
                                return FactoryPathSymbol_REbarshape_FxxDTO.Default();

                        }
                    default:
                        return FactoryPathSymbol_REbarshape_FxxDTO.Default();
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
