using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Fund.TAG;
using ArmaduraLosaRevit.Model.PathReinf.Ayuda;
using ArmaduraLosaRevit.Model.PathReinf.Barras;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using ArmaduraLosaRevit.Model.PathReinf.Servicios;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Prueba.User;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf
{
    public class ManejadorPathReinf
    {
        private UIApplication _uiapp;
        private Document _doc;
        public PathReinf_nh pathReinf_nh { get; set; }
        public bool IsOK { get; private set; }

        public ManejadorPathReinf(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }



        public bool Ejecutar(SolicitudBarraDTO _solicitudBarraDTO, PathReinfSeleccionDTO _pathReinfSeleccionDTO, DatosNuevaBarraDTO _datosNuevaBarraDTO)
        {
            try
            {

                #region Validar

                ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
                bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorPathReinf");

                if (!resultadoConexion)
                {
                    Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                    return false;
                }
                else if (!_ManejadorUsuarios.ObteneResultado())
                {
                    Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                    return false;
                }


                RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
                if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return false ;

                #endregion

                if (!Directory.Exists(ConstNH.CONST_COT)) return false;
                //no se utiliza los ganchosRE
                //obtener hook
                ITipoHook ObtenerTipoHook = new TipoHook(_solicitudBarraDTO);
                ObtenerTipoHook.DefinirHook();

                ITiposRebarShape PathRebarShape = new TiposRebarShapeFund(_solicitudBarraDTO, _datosNuevaBarraDTO, _pathReinfSeleccionDTO.EspesorCm_1);
                _datosNuevaBarraDTO = PathRebarShape.DefinirRebarShape();

                //path symbol
                TiposPathReinformentSymbol_X ptSymbol = new TiposPathReinformentSymbol_X(_solicitudBarraDTO);
                _datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();
                _datosNuevaBarraDTO.ElementSimboloPathReinforcementElement = null;

                if (ObtenerCasoBarraSimple.IsCaso_f1_f3Suplemuro_f4(_solicitudBarraDTO.TipoBarra))
                {
                    IPathSymbol _IPathSymbolcs = FactoryPathSymbolV2.ObtenerPathSymbol(_uiapp, _solicitudBarraDTO, _datosNuevaBarraDTO);
                    if (_IPathSymbolcs.M1_ObtenerPArametros())
                    {
                        if (_IPathSymbolcs.M2_ejecutar())
                            _datosNuevaBarraDTO.ElementSimboloPathReinforcementElement  = _IPathSymbolcs.elemtoSymboloPath;
                        else
                        {
                            IsOK = false;
                            return false;
                        }
                    }
                }


                if (_datosNuevaBarraDTO.ElementSimboloPathReinforcementElement == null) return false;

                //5)
                ICasoBarraX iTipoBarra = FactoryICasoBarra.ObtenerICasoyBarraX(_solicitudBarraDTO, _datosNuevaBarraDTO);
                if (iTipoBarra == null) return false;

                ///6
                IGeometriaTag _listaTAgBArra = FactoryGeomTag.CrearGeometriaTag(_doc, _pathReinfSeleccionDTO.ptoConMouse, _solicitudBarraDTO, _pathReinfSeleccionDTO.ListaPtosPerimetroBarras);
                _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(_pathReinfSeleccionDTO.Angle_pelotaLosa1Grado) });


                _solicitudBarraDTO.IsDirectriz = false;
                _datosNuevaBarraDTO.IsRedefinirTagHeadPosition = true;
                //***lisato
                pathReinf_nh = new PathReinf_nh(_uiapp, _solicitudBarraDTO, _pathReinfSeleccionDTO, _datosNuevaBarraDTO, iTipoBarra, _listaTAgBArra);

                //orienacion  , se observo q si origen de path esta bajo la cota Z, dibuja el path hacio arriba(contrario a cambio original)
                //  double ZOrigenDecurva = _datosNuevaBarraDTO.CurvesPathreiforment[0].GetEndPoint(0).Z;
                //  bool aux_flop = (ZOrigenDecurva > 0 ? true : false);

                Result reuslt = pathReinf_nh.CrearBarra();
                if (reuslt != Result.Succeeded) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ManejadorPathReinf' ex:{ex.Message}");
                return false; ;
            }

            return true;

        }

        public bool EjecutarParaFundaciones(SolicitudBarraDTO _solicitudBarraDTO, PathReinfSeleccionDTO _pathReinfSeleccionDTO, DatosNuevaBarraDTO _datosNuevaBarraDTO)
        {
            try
            {
                #region Validar

                ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
                bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorPathReinf");

                if (!resultadoConexion)
                {
                    Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                    return false;
                }
                else if (!_ManejadorUsuarios.ObteneResultado())
                {
                    Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                    return false;
                }


                RepositorioUsuarios _RepositorioUsuarios = new RepositorioUsuarios(NombreServer.EUGENIA);
                if (_RepositorioUsuarios.GetRolUsuarioSPorMac("") == null) return false;

                #endregion

                //no se utiliza los ganchosRE
                //obtener hook
                ITipoHook ObtenerTipoHook = new TipoHook(_solicitudBarraDTO);
                ObtenerTipoHook.DefinirHook();

                ITiposRebarShape PathRebarShape = new TiposRebarShapeFund(_solicitudBarraDTO, _datosNuevaBarraDTO, _pathReinfSeleccionDTO.EspesorCm_1);
                _datosNuevaBarraDTO = PathRebarShape.DefinirRebarShape();

                ITiposPathReinformentSymbol ptSymbol = new TiposPathReinformentSymbol_Fund(_solicitudBarraDTO, _datosNuevaBarraDTO);
                _datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();
                _datosNuevaBarraDTO.ElementSimboloPathReinforcementElement = ptSymbol.ObtenerFamilia();
                if (_datosNuevaBarraDTO.ElementSimboloPathReinforcementElement == null)
                {
                    Util.ErrorMsg($"Error no se puede obtener familia: {_datosNuevaBarraDTO.nombreSimboloPathReinforcement}");
                    return false;
                }

                if (ObtenerCasoBarraSimple.IsCaso_f12(_solicitudBarraDTO.TipoBarra))
                {
                    IPathSymbol _IPathSymbolcs = FactoryPathSymbolV2.ObtenerPathSymbol(_uiapp, _solicitudBarraDTO, _datosNuevaBarraDTO);
                    if (_IPathSymbolcs.M1_ObtenerPArametros())
                    {
                        if (_IPathSymbolcs.M2_ejecutar())
                            _datosNuevaBarraDTO.ElementSimboloPathReinforcementElement = _IPathSymbolcs.elemtoSymboloPath;
                        else
                        {
                            IsOK = false;
                            return false;
                        }
                    }
                }

                //5)
                ICasoBarraX iTipoBarra = FactoryICasoBarra.ObtenerICasoyBarraXFunda(_solicitudBarraDTO, _datosNuevaBarraDTO);
                //6)
                IGeometriaTag _listaTAgBArra = FactoryGeomTag_fund.CrearGeometriaTag(_doc, new XYZ(0, 0, 0), _solicitudBarraDTO, _pathReinfSeleccionDTO);
                _listaTAgBArra.Ejecutar(new GeomeTagArgs() { angulorad = Util.GradosToRadianes(_pathReinfSeleccionDTO.Angle_pelotaLosa1Grado) });


                //***lisato
                _solicitudBarraDTO.IsDirectriz = true;
                _datosNuevaBarraDTO.IsRedefinirTagHeadPosition = true;

                //
                _datosNuevaBarraDTO.Prefijo_cuantia = (_datosNuevaBarraDTO.TipoCaraObjeto_ == TipoCaraObjeto.Inferior ? "F=" : "F'=");


                pathReinf_nh = new PathReinf_nh(_uiapp, _solicitudBarraDTO, _pathReinfSeleccionDTO, _datosNuevaBarraDTO, iTipoBarra, _listaTAgBArra);

                pathReinf_nh.CrearBarra();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false; ;
            }

            return true;
        }

    }
}
