using Autodesk.Revit.UI;
using System.Collections.Generic;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

//using planta_aux_C.Elemento_Losa;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using System;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.EditarPath.Seleccion;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.Extension;
using static ArmaduraLosaRevit.Model.Extension.ExtensionPathReinforment;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Extension.modelo;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using System.Linq;

//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{


    public class SeleccionarPathReinfomentConPto
    {
        public TipoCasoAlternativo TipoCasoAlternativo_ { get; internal set; }

        private UIDocument _uidoc;

        public XYZ PuntoSeleccionMouse { get; set; }
        public PathReinforcement PathReinforcement { get; set; }
        public PathReinSpanSymbol PathReinforcementSymbol { get; set; }

        public ElementId tipodeHookStartPrincipal { get; set; }
        public ElementId tipodeHookEndPrincipal { get; set; }

        public CoordenadaPath _coordenadaPath { get; set; }

        public XYZ UbicacionPathReinforcementSymbol { get; private set; }

        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorEditarREbarShapYPAthSymbol _AyudaObtenerLArgoPata { get; set; }


        public Room RoomSelecionado { get; set; }
        public ReferenciaRoomDatos _referenciaRoomDatos { get; private set; }
        public string _tipobarra { get; set; }
        public string _direccion { get; set; }
        public string _diametro { get; set; }
        public string _espaciamiento { get; set; }
        public string _Prefijo_F { get; set; }
        public string _LetraParametro { get; set; }

        public XYZ _TagHeadPosition { get; set; } = XYZ.Zero;
        public XYZ _LeaderElbow { get; set; } = XYZ.Zero;
        public XYZ _LeaderEnd { get; set; } = XYZ.Zero;

        public bool IsLeaderElbow { get; private set; }
        public bool IsLeaderEnd { get; private set; }
  


        public string _TipoDireccionBarra { get; set; }
        public TipoRebar _barraTipo { get; private set; }

        internal bool Crear_AyudaObtenerLArgoPata(double distanciaDefinir_foot)
        {
            try
            {


                ManejadorEditarREbarShapYPAthSymbol_DTO AyudaObtenerLArgoPataDTO = new ManejadorEditarREbarShapYPAthSymbol_DTO()
                {
                    PathReinforcement = PathReinforcement,
                    _TipoBarra = _tipobarra,
                    DiametroMM = Util.ConvertirStringInDouble(_diametro),
                    LargoAhoraDefinidoUsuario_foot = distanciaDefinir_foot,
                    _direccion = EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, _direccion),
                    PathReinforcementSymbol = PathReinforcementSymbol
                };

                _AyudaObtenerLArgoPata = new ManejadorEditarREbarShapYPAthSymbol(_uiapp, AyudaObtenerLArgoPataDTO, _referenciaRoomDatos);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error Crear_AyudaObtenerLArgoPata: {ex.Message}");
                return false;
            }
            return true;
        }

        public HookPAthRein _HoookPAthRein { get; private set; }


        public SeleccionarPathReinfomentConPto(UIDocument uidoc, Application app)
        {
            Options opt = app.Create.NewGeometryOptions();
            this._uidoc = uidoc;
        }

        public SeleccionarPathReinfomentConPto(UIApplication uiapp)
        {
            Options opt = uiapp.Application.Create.NewGeometryOptions();
            this._uidoc = uiapp.ActiveUIDocument;
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        #region 1) Metodo 'SeleccionarPathReinforment'


        public bool InsertarPathReinforment(string idElement, bool isFundaciones = false)
        {
            try
            {
                if (Util.IsNumeric(idElement))
                {
                    int idElem = Util.ConvertirStringInInteger(idElement);
                    ElementId elID = new ElementId(idElem);
                    Element Elem_PathReinSymbol = _uidoc.Document.GetElement(elID);
                    if (Elem_PathReinSymbol == null) return false;

                    if (!(Elem_PathReinSymbol is PathReinSpanSymbol)) { return false; }
                    PathReinforcementSymbol = Elem_PathReinSymbol as PathReinSpanSymbol;
                }
                else
                {
                    if (!SeleccionarPathReinfromenSymbol()) return false; ;
                }

                //SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view);
                //_SelecPathReinVisibilidad.M1_ejecutar();

                _LeaderElbow = PathReinforcementSymbol.Obtener_LeaderElbow(_uiapp);//  LeaderElbow;
                _LeaderEnd = PathReinforcementSymbol.Obtener_LeaderEnd(_uiapp);

                if (!ObtenerPathRein()) return false;
                if (!SeleccionarRoom(isFundaciones)) return false;
                if (!ObtenerDatosPathRein()) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'Insertar PathReinforment'  \n ex:{ex.Message}");

                return false;
            }
            return IsPathReiformentNull();
        }

        public bool SeleccionarPathReinforment(bool IsSaltarRoom = false)
        {
            try
            {
                //SeleccionarPathReinfromen();
                if (!SeleccionarPathReinfromenSymbol()) return false;
                if (!ObtenerPathRein()) return false;
                if (!ObtenerPtoMouse()) return false;
                if (!ObtenerDatosPathRein()) return false;
                if (!SeleccionarRoom(IsSaltarRoom)) return false;

                ManejadorEditarREbarShapYPAthSymbol_DTO AyudaObtenerLArgoPataDTO = new ManejadorEditarREbarShapYPAthSymbol_DTO()
                {
                    PathReinforcement = PathReinforcement,
                    _TipoBarra = _tipobarra,
                    DiametroMM = Util.ConvertirStringInDouble(_diametro),
                    LargoAhoraDefinidoUsuario_foot = 0
                };

                _AyudaObtenerLArgoPata = new ManejadorEditarREbarShapYPAthSymbol(_uiapp, AyudaObtenerLArgoPataDTO, _referenciaRoomDatos);
                //   _AyudaObtenerLArgoPata.ObtenerLArgoPAta();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'Seleccionar PathReinforment'  \n ex:{ex.Message}");
                return false;
            }
            return IsPathReiformentNull();
        }
        public bool SeleccionarPathReinformentFund()
        {
            //SeleccionarPathReinfromen();
            if (!SeleccionarPathReinforment()) return false;
            if (!ObtenerHookFund()) return false;

            return IsPathReiformentNull();
        }

        private bool ObtenerHookFund()
        {
            try
            {
                _HoookPAthRein = PathReinforcement.ObtenerHooks();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener HooK ex{ex.Message}");
                return false;
            }
            return true;
        }

        public bool AsignarPathReinformentSymbol(Element PathReinSymbol, bool isFundaciones = false)
        {
            if (PathReinSymbol == null) return false;
            if (!(PathReinSymbol is PathReinSpanSymbol)) { return false; }

            PathReinforcementSymbol = PathReinSymbol as PathReinSpanSymbol;
            if (!ObtenerPathRein()) return false;
            if (!ObtenerPtoMouse()) return false;
            if (!ObtenerDatosPathRein()) return false;
            if (!SeleccionarRoom(isFundaciones)) return false;
            return IsPathReiformentNull();
        }
        public bool AsignarPathReinformentSymbol_sinSelecRoom(Element PathReinSymbol)
        {
            return AsignarPathReinformentSymbolFund(PathReinSymbol);
        }
        public bool AsignarPathReinformentSymbolFund(Element PathReinSymbol)
        {
            if (PathReinSymbol == null) return false;
            if (!(PathReinSymbol is PathReinSpanSymbol)) { return false; }

            PathReinforcementSymbol = PathReinSymbol as PathReinSpanSymbol;
            if (!ObtenerPathRein()) return false;
            if (!ObtenerPtoMouse()) return false;
            if (!ObtenerDatosPathRein()) return false;

            return IsPathReiformentNull();
        }


        private bool ObtenerPtoMouse()
        {
            try
            {
                if (PathReinforcementSymbol == null)
                {
                    Util.ErrorMsg($" PathReinforcementSymbol igual null.");
                    return false;
                }
                //PuntoSeleccionMouse = PathReinforcementSymbol.TagHeadPosition;
                if(! ObtenerPuntosTag()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en '' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool ObtenerPuntosTag()
        {
            IsLeaderElbow = false;
            IsLeaderEnd = false;
            var result = SeleccionarPathReinfomentVisibilidadStatic.Obtener_ElementoPathRein_conPathSymbol(_uiapp, _doc.ActiveView, PathReinforcementSymbol);
            if (result != null)
            {
                var ResulTag = result.ListTagpath.Where(c => c.Name.Contains("_Ffund_")).FirstOrDefault() as IndependentTag;
                if (ResulTag != null && ResulTag.HasLeader)
                {
                    _TagHeadPosition = ResulTag.TagHeadPosition;

                    if (ResulTag.HasLeader)
                    {
                        IsLeaderElbow = true;                         
                        _LeaderElbow = ResulTag.Obtener_LeaderElbow();
                    }

                    if (ResulTag.LeaderEndCondition == LeaderEndCondition.Free)
                    {
                        IsLeaderEnd = true;
                        _LeaderEnd =  ResulTag.Obtener_LeaderEnd();
                    }

                }


                
            }
            return true;
        }

        private bool ObtenerDatosPathRein()
        {
            try
            {
                Parameter aux_tipobarra = ParameterUtil.FindParaByName(PathReinforcement, "IDTipo");
                if (aux_tipobarra != null)
                    _tipobarra = aux_tipobarra.AsString();

                //10-07-2023   caso especial de inverti caso f22a y b
                if ((PathReinforcementSymbol.Name.Contains("_DI") || PathReinforcementSymbol.Name.Contains("_II"))
                    && (_tipobarra == "f22a" || _tipobarra == "f22b"))
                {
                    _tipobarra = _tipobarra + "Inv";
                }

                Parameter _BarraTipoAux = ParameterUtil.FindParaByName(PathReinforcement, "BarraTipo");
                if (_BarraTipoAux != null)
                    _barraTipo = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, _BarraTipoAux.AsString());

                Parameter aux_direccion = ParameterUtil.FindParaByName(PathReinforcement, "IDTipoDireccion");
                if (aux_direccion != null)
                    _direccion = aux_direccion.AsString();


                Parameter aux_TipoDireccionBarra = ParameterUtil.FindParaByName(PathReinforcement, "TipoDireccionBarra");
                if (aux_TipoDireccionBarra != null)
                    _TipoDireccionBarra = aux_TipoDireccionBarra.AsString();

                if ((_tipobarra == "s1" || _tipobarra == "s2" || _tipobarra == "s3" || _tipobarra == "s4" || _tipobarra == "f12") &&
                   _TipoDireccionBarra == null) _TipoDireccionBarra = "i";

                Parameter aux_diametro = PathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_TYPE_1);
                if (aux_diametro != null)
                    _diametro = aux_diametro.AsValueString().Replace("Ø", "");

                Parameter aux_espaciamiento = PathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_SPACING);
                if (aux_espaciamiento != null)
                    _espaciamiento = Util.FootToCm(aux_espaciamiento.AsDouble()).ToString();

                Parameter aux_Prefi_F = ParameterUtil.FindParaByName(PathReinforcement, "Prefijo_F");
                if (aux_Prefi_F != null)
                {
                    _Prefijo_F = aux_Prefi_F.AsString();
                    if (_Prefijo_F == null) _Prefijo_F = "F=";
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error al obtener parametros de pathreinformen ex:{ex.Message}");
                return false;
            }
            return true;

        }


        private bool SeleccionarPathReinfromenSymbol()
        {
            try
            {
                // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
                ISelectionFilter f = new PathReinSymbolSelectionFilter();
                //selecciona un objeto floor
                Reference referen;
                try
                {
                    referen = _uidoc.Selection.PickObject(ObjectType.Element, f, "Seleccionar path");
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    referen = null;
                }

                if (referen == null) return false;
                PuntoSeleccionMouse = Util.PtoDeLevelDeGlobalPoint(referen.GlobalPoint, _uidoc.Document);

                Element PathReinSymbol = _uidoc.Document.GetElement(referen);
                // si refere3ncia es null salir
                if (PathReinSymbol == null) return false;

                if (!(PathReinSymbol is PathReinSpanSymbol)) { return false; ; }

                PathReinforcementSymbol = PathReinSymbol as PathReinSpanSymbol;

                UbicacionPathReinforcementSymbol = PathReinforcementSymbol.TagHeadPosition;

                //if (!ObtenerPuntosTag()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Seleccionar PathReinfromenSymbol' ex:{ex.Message} ");
                return false;
            }
            return true;

        }

        private bool ObtenerPathRein()
        {
            try
            {
                if (PathReinforcementSymbol == null) return false;
                if (!PathReinforcementSymbol.IsValidObject) return false;
                Element r = PathReinforcementSymbol.Obtener_TaggedLocalElement(_uiapp);
                //obtiene una referencia floor con la referencia r
                if (r is PathReinforcement)
                    PathReinforcement = r as PathReinforcement;

                if (PathReinforcement.Pinned)
                {
                    Util.ErrorMsg($"Barra tiene Pin asigando, no es posible editar. ");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" No se pudo obtener Pathreinfirment desde pathsymbol ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool SeleccionarRoom(bool isFunda = false)
        {
            if (isFunda) return true;
            try
            {
                if (PuntoSeleccionMouse == null) return false;
                RoomSelecionado = RoomSeleccionar.GetRoomConPtoNivelActual(_uidoc, (UbicacionPathReinforcementSymbol != null ? UbicacionPathReinforcementSymbol.AsignarZ(PuntoSeleccionMouse.Z) : PuntoSeleccionMouse));


                if (RoomSelecionado == null && _tipobarra != "f12" && VariablesSistemas.IS_MENSAJE_BUSCAR_ROOM)
                {
                    Util.InfoMsg($"Error al obtener Rooms.No se encontro Room en view analisada.\n NOTA: Esto solo afecta si se desea cambiar tipo de barra porque es necesario obtener el 'largo minimo' y ' espesor' de losa");
                }
                _referenciaRoomDatos = new ReferenciaRoomDatos(_doc, RoomSelecionado);
                _referenciaRoomDatos.GetParametrosUnRoom();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'SeleccionarRoom' \nex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool IsPathReiformentNull()
        {
            if ((PathReinforcement == null) || (PuntoSeleccionMouse == XYZ.Zero)) return false;
            return true;
        }

        #endregion

        public Document GetDoc()
        {
            return _uidoc.Document;
        }

    }
}
