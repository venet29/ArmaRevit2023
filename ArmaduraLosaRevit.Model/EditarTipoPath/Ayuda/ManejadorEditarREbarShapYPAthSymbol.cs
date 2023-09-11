using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda
{

    public class ManejadorEditarREbarShapYPAthSymbol_DTO
    {
        public PathReinforcement PathReinforcement { get; set; }
        public PathReinSpanSymbol PathReinforcementSymbol { get; set; }
        public string _TipoBarra { get; set; }
        public double DiametroMM { get; internal set; }
        public double LargoAhoraDefinidoUsuario_foot { get; internal set; }
        public UbicacionLosa _direccion { get; internal set; }
        public DatosPathRinforment DatosPathRinforment { get; internal set; }
    }


    // redefine largo de pata al cambiar rebarshape
    public class ManejadorEditarREbarShapYPAthSymbol
    {
        private readonly UIApplication _uiapp;
        private readonly ManejadorEditarREbarShapYPAthSymbol_DTO _ManejadorEditarREbarShapYPAthSymbol_DTO;
        private readonly PathReinforcement _pathReinforcement;
        private Document _doc;
        private View _view;
        private TipoDireccion _tipoDireccion;
        private DatosPathRinforment _DatosPathRinforment;
        private PathSymbol_REbarshape_FxxDTO ConfigUsuario;
        private bool IsOK_PathRebarShape_;
        private Element elemtoSymboloPath;
        private bool IsOK_PathSymbol_;
        private DatosNuevaBarraDTO _datosNuevaBarraDTO;
        private SolicitudBarraDTO _solicitudBarraDTO;
        private readonly string _TipoBarra;
        private readonly ReferenciaRoomDatos _ReferenciaRoomDatos;


        public ManejadorEditarREbarShapYPAthSymbol(UIApplication uiapp, ManejadorEditarREbarShapYPAthSymbol_DTO _AyudaObtenerLArgoPataDTO, ReferenciaRoomDatos _referenciaRoomDatos)
        {
            this._uiapp = uiapp;
            this._ManejadorEditarREbarShapYPAthSymbol_DTO = _AyudaObtenerLArgoPataDTO;
            this._pathReinforcement = _AyudaObtenerLArgoPataDTO.PathReinforcement;
            this._doc = _pathReinforcement.Document;
            this._view = _doc.ActiveView;
            this._TipoBarra = _AyudaObtenerLArgoPataDTO._TipoBarra;
            this._ReferenciaRoomDatos = _referenciaRoomDatos;


            this.ConfigUsuario = null;
            IsOK_PathRebarShape_ = false;
            IsOK_PathSymbol_ = false;
            elemtoSymboloPath = null;
        }

        public bool M1_ActualizarPAtaPAthReinforment_ConTrans(TipoDireccion _TipoDireccion_)
        {
            try
            {
                _tipoDireccion = _TipoDireccion_;
                //a) datos 
                _DatosPathRinforment = new DatosPathRinforment(_pathReinforcement);
                if (!_DatosPathRinforment.M1_ObtenerDatosGenerales()) return false;

                _ManejadorEditarREbarShapYPAthSymbol_DTO.DatosPathRinforment = _DatosPathRinforment;
                //b)  configuracion desplaz       
                if (!M1_2_ObtenerCOnfiguracionDesplamineto()) return false;

                if (!M1_3_Obtener_DatosNuevaBarraDTO_SolicitudBarraDTO()) return false;
                //****
                M1_4_ObtenerRebarShape();
                //**************
                M1_5_ObtenerPathsymbol();
                //***************


                if ((!IsOK_PathRebarShape_) && (!IsOK_PathSymbol_)) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ModificarRebarshape_porTraslapo-NH");

                    //path reinfoment
                    _pathReinforcement.PrimaryBarShapeId = _datosNuevaBarraDTO.tipoRebarShapePrincipal.Id;

                    if (_pathReinforcement.IsAlternatingLayerEnabled())
                        _pathReinforcement.AlternatingBarShapeId = _datosNuevaBarraDTO.tipoRebarShapeAlternativa.Id;

                    // reemplazar path symbol  o
                    // crear nuevo pathsymbol con la misma posicion
                    if (_ManejadorEditarREbarShapYPAthSymbol_DTO.LargoAhoraDefinidoUsuario_foot > 0 && IsOK_PathSymbol_)
                    {
                        XYZ posiconTAg = _ManejadorEditarREbarShapYPAthSymbol_DTO.PathReinforcementSymbol.TagHeadPosition;
                        var _PathReinSpanSymbol = PathReinSpanSymbol.Create(_doc, _view.Id, new LinkElementId(_pathReinforcement.Id), XYZ.Zero, elemtoSymboloPath.Id);
                        if (posiconTAg != null) _PathReinSpanSymbol.TagHeadPosition = posiconTAg;

                        _doc.Delete(_ManejadorEditarREbarShapYPAthSymbol_DTO.PathReinforcementSymbol.Id);
                    }

                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ActualizarPAtaPAthReinforment'    \n ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private bool M1_2_ObtenerCOnfiguracionDesplamineto()
        {
            try
            {
                if (_ManejadorEditarREbarShapYPAthSymbol_DTO.LargoAhoraDefinidoUsuario_foot > 0)
                    ConfigUsuario = FactoryConfiguracionDespla.ObtenerConfig(_TipoBarra,_tipoDireccion , _ManejadorEditarREbarShapYPAthSymbol_DTO);
                else
                    ConfigUsuario = FactoryPathSymbol_REbarshape_FxxDTO.Default();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'DatosNuevaBarraDTO_SolicitudBarraDTO' \n ex:{ex.Message}");
                return true;
            }
            return true;
        }
        private bool M1_3_Obtener_DatosNuevaBarraDTO_SolicitudBarraDTO()
        {
            try
            {
                _datosNuevaBarraDTO = new DatosNuevaBarraDTO()
                {
                    LargoPathreiforment = _DatosPathRinforment.ObtenerLArgoPAth(),
                    LargoMininoLosa = _ReferenciaRoomDatos.largomin_1,
                    IsLuzSecuandiria = _DatosPathRinforment.IsBarraSecuandaria,
                    EspesorLosaCm_1 = _pathReinforcement.EspesorLosa(),
                    DiametroMM = _ManejadorEditarREbarShapYPAthSymbol_DTO.DiametroMM,
                    PathSymbol_REbarshape_FXXDTO_ = ConfigUsuario
                };


                _solicitudBarraDTO = new SolicitudBarraDTO();
                _solicitudBarraDTO.TipoBarra = _TipoBarra;
                _solicitudBarraDTO.UbicacionEnlosa = _ReferenciaRoomDatos._ubicacionBarraEnlosa;
                _solicitudBarraDTO.UIdoc = _uiapp.ActiveUIDocument;
                _solicitudBarraDTO.UbicacionEnlosa = _ManejadorEditarREbarShapYPAthSymbol_DTO._direccion;
                _solicitudBarraDTO.DireccionEditarDesplazamiento = _tipoDireccion;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'DatosNuevaBarraDTO_SolicitudBarraDTO' \n ex:{ex.Message}");
                return true;
            }
            return true;
        }

        private void M1_4_ObtenerRebarShape()
        {
            try
            {
                if (ObtenerCasoAoBDeAhorro.IScaso_16_17_18_19_20_20Inv_21_22_AoB(_solicitudBarraDTO.TipoBarra))
                {
                    IRebarShapeNh PathRebarShape_ = FactoryRebarShapeNh.ObtenerRebarShapeNh(_uiapp, _solicitudBarraDTO, _datosNuevaBarraDTO);
                    if (PathRebarShape_.M0_Ejecutar())
                    {
                        IsOK_PathRebarShape_ = true;
                        _datosNuevaBarraDTO = PathRebarShape_.DatosNuevaBarraDTO_;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerRebarShape'    \n ex:{ex.Message}");
                IsOK_PathRebarShape_ = false;
            }
        }
        private void M1_5_ObtenerPathsymbol(bool IsInvertir=true )
        {
            try
            {
                CrearPAthSYmbolInvertido_CASOF22a_f22b(IsInvertir);

                if (_ManejadorEditarREbarShapYPAthSymbol_DTO.LargoAhoraDefinidoUsuario_foot > 0 &&
                    FactoryPathSymbol.VerificarSICOrresponde_ObtenerPathSymbol(_solicitudBarraDTO))
                {
                    ITiposPathReinformentSymbol ptSymbol = new TiposPathReinformentSymbol(_solicitudBarraDTO);
                    _datosNuevaBarraDTO.nombreSimboloPathReinforcement = ptSymbol.M1_DefinirPathReinformentSymbol();

                    if (ObtenerCasoAoBDeAhorro.IScaso_16_17_18_19_20_20Inv_21_22_AoB(_solicitudBarraDTO.TipoBarra))
                    {
                        IPathSymbol _IPathSymbolcs = FactoryPathSymbol.ObtenerPathSymbol(_uiapp, _solicitudBarraDTO, _datosNuevaBarraDTO);
                        if (_IPathSymbolcs.M1_ObtenerPArametros())
                        {

                            if (_IPathSymbolcs.M2_ejecutar())
                            {
                                elemtoSymboloPath = _IPathSymbolcs.elemtoSymboloPath;
                                IsOK_PathSymbol_ = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'M1_5_ObtenerPathsymbol'    \n ex:{ex.Message}");
                IsOK_PathSymbol_ = false;
            }
        }

        // si creo csaso f22a -> tb se crea  f22aInv  ********* si Muevo f22aInv -> tb se crea f22a igual desplazamineto
        // si creo csaso f22b -> tb se crea  f22bInv  ********* si Muevo f22bInv -> tb se crea f22b igual desplazamineto
        private void CrearPAthSYmbolInvertido_CASOF22a_f22b(bool IsInvertir)
        {
            //generar psthsymol nomral e invertido
            if (_solicitudBarraDTO.TipoBarra == "f22a" && IsInvertir)
            {
                //cambiar a f22aInv para generar pathsimbol --- no se utiliza, solo se deja disponible para invertir 
                var auxCOnf = _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20A(0, 0, auxCOnf.DesDereSup_foot);
                _solicitudBarraDTO.TipoBarra = "f22aInv";
                M1_5_ObtenerPathsymbol(false);

                //volver f22a
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = auxCOnf;
                _solicitudBarraDTO.TipoBarra = "f22a";
            }
            else if (_solicitudBarraDTO.TipoBarra == "f22aInv" && IsInvertir)
            {
                var auxCOnf = _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20A(0, auxCOnf.DesDereInf_foot, 0);
                _solicitudBarraDTO.TipoBarra = "f22a";
                M1_5_ObtenerPathsymbol(false);

                //VOLVER f22b
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = auxCOnf;
                _solicitudBarraDTO.TipoBarra = "f22aInv";
            }
            else if (_solicitudBarraDTO.TipoBarra == "f22b" && IsInvertir)
            {
                //cambiar a f22bInv para generar pathsimbol --- no se utiliza, solo se deja disponible para invertir 
                var auxCOnf = _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20B(0, 0, auxCOnf.DesIzqSup_foot);
                _solicitudBarraDTO.TipoBarra = "f22bInv";
                M1_5_ObtenerPathsymbol(false);

                //volver f22B
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = auxCOnf;
                _solicitudBarraDTO.TipoBarra = "f22b";
            }
            else if (_solicitudBarraDTO.TipoBarra == "f22bInv" && IsInvertir)
            {
                var auxCOnf = _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_;
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = FactoryPathSymbol_REbarshape_FxxDTO.ConfigF20B(0, auxCOnf.DesIzqInf_foot, 0);
                _solicitudBarraDTO.TipoBarra = "f22b";
                M1_5_ObtenerPathsymbol(false);

                //volver f22B
                _datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_ = auxCOnf;
                _solicitudBarraDTO.TipoBarra = "f22bInv";
            }
        }
    }
}
