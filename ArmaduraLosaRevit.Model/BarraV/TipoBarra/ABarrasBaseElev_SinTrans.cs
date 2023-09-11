using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public enum TipoRedondero
    {
        Redonder_Uno_1 = 1,
        Redondeat_Cinco_5 = 2
    }

    public abstract class ABarrasBaseElev_SinTrans
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected IntervaloBarrasDTO _interBArraDto;
        protected View _view;
        protected List<Curve> _listcurveElevacion;
   
        protected List<Curve> _listcurve;
        protected RebarStyle _rebarStyle;
        protected RebarBarType _rebarBarType;
        protected Rebar _rebar;
        protected XYZ _VectorMover;
        protected XYZ _Direccion_ptoIniToPtoFin;
        protected IGeometriaTag _newGeometriaTag;
        protected TipoRebar _BarraTipo = TipoRebar.NONE;
        protected DiametrosBarrasDTO _DiametrosBarrasDTO;
        protected double largoPata;

        public RebarHookType _tipoHookInicial { get; set; }
        public RebarHookType _tipoHookFinal { get; set; }
        public bool IsSoloTag { get; set; }
        public RebarHookOrientation startHookOrient { get; set; }
        public RebarHookOrientation endHookOrient { get; set; }
        public bool useExistingShapeIfPossible { get; set; }
        public bool createNewShape { get; set; }

        public TipoRedondero tipoRedondeo { get; set; }
        protected double delta;

        public ABarrasBaseElev_SinTrans(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._interBArraDto = interBArraDto;
            this._newGeometriaTag = _newGeometriaTag;
            this._view = _doc.ActiveView;
            this._listcurve = new List<Curve>();
            this._listcurveElevacion = new List<Curve>();
            this.startHookOrient = RebarHookOrientation.Right; //defecto
            this.endHookOrient = RebarHookOrientation.Right; //defecto      

            tipoRedondeo = TipoBarra.TipoRedondero.Redondeat_Cinco_5;
            delta = 0;
        }

  

        //public XYZ GetIsPo_P2paraTag()
        //{
        //    return _interBArraDto.ptoPosicionTAg;
        //}

        public bool M1_1_CalculosIniciales()
        {
            _tipoHookInicial = _interBArraDto.tipoHookInicial;
            _tipoHookFinal = _interBArraDto.tipoHookFinal;
            if (_interBArraDto._tipoLineaMallaH == TipoPataBarra.BarraVPataInicial)
            {
                startHookOrient = RebarHookOrientation.Right;
                endHookOrient = RebarHookOrientation.Right;

            }
            else if (_interBArraDto._tipoLineaMallaH == TipoPataBarra.BarraVPataFinal)
            {
                startHookOrient = RebarHookOrientation.Left;
                endHookOrient = RebarHookOrientation.Left;
            }

            _Direccion_ptoIniToPtoFin = (_interBArraDto.ptofinal - _interBArraDto.ptoini).Normalize();
            useExistingShapeIfPossible = true;
            createNewShape = true;

            _rebarStyle = RebarStyle.Standard;
            _rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + _interBArraDto.diametroMM, _doc, true);
       
            if (_rebarBarType == null)
            {
                Util.ErrorMsg($"Error al obtener el tipo { "Ø " + _interBArraDto.diametroMM}");
                return false;
            }


            _DiametrosBarrasDTO = TipoDiametrosBarrasDTO.ObtenerListaDiametro().Where(c => c.diametro_mm == _interBArraDto.diametroMM).FirstOrDefault();
            double _LargoDedesarrolloExtra = 0;
            //if (_DiametrosBarrasDTO != null)
            //{ _LargoDedesarrolloExtra = Util.MmToFoot( _DiametrosBarrasDTO.Standard_Bend_Diameter_mm)/2; }
            largoPata = UtilBarras.largo_ganchoFoot_diamMM(_interBArraDto.diametroMM)+ _LargoDedesarrolloExtra;// Util.CmToFoot(20);// UtilBarras.largo_L9_DesarrolloFoot_diamMM(_interBArraDto.diametroMM);

            if (BuscarIsNombreViewActualizado.IsError(_doc.ActiveView)) return false;

            return true;
        }


        public Result M1_3_DibujarBarraCurve()
        {
            Result result = Result.Failed;
            try
            {
                _rebar = Rebar.CreateFromCurves(_doc,
                                           _rebarStyle,
                                           _rebarBarType,
                                           _tipoHookInicial,
                                           _tipoHookFinal,
                                           _interBArraDto.ElementoHost,
                                           _interBArraDto.DireccionRecorridoBarra,
                                           _listcurve,
                                           startHookOrient,
                                           endHookOrient,
                                           useExistingShapeIfPossible,
                                           createNewShape);
                result = Result.Succeeded;

              
            }
            catch (Exception ex)
            {
                string msj = ex.Message;

                result = Result.Cancelled;
            }

   
            //  if (_rebar != null) listaGrupo.Add(_rebar.Id);
            if (_rebar == null) return result = Result.Cancelled;

            // ConstantesGenerales.sbLog.AppendLine($"_rebar.Id:{_rebar.Id} ");
            return result;
        }

        public ElementId M3_ObtenerIdRebar()
        {
            return (_rebar != null ? _rebar.Id : null);
        }



        public IbarraBaseResultDTO GetResult()
        {
            return new IbarraBaseResultDTO()
            {
                _rebar = _rebar,
                OrientacionTagGrupoBarras = _interBArraDto.OrientacionTagGrupoBarras,
                IsNoProloganLosaArriba = (_interBArraDto.IsNoProloganLosaArriba ? true : false),
                ptoPosicionTAg = _interBArraDto.ptoPosicionTAg,
                IsConDirectriz = (_interBArraDto.IsIsDirectriz ? true : false)
            };
        }
        public void M1_4_ConfigurarAsignarParametrosRebarshape(ParametrosInternoRebarDTO _parametrosInternoRebarDTO, string tipoBarraHorizontal)
        {
            try
            {
                if (_view != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null) ParameterUtil.SetParaInt(_rebar, "NombreVista", _view.ObtenerNombreIsDependencia());  //"nombre de vista"

                if (ParameterUtil.FindParaByName(_rebar, "BarraTipo") != null && _BarraTipo != TipoRebar.NONE)
                    ParameterUtil.SetParaInt(_rebar, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo));  //"nombre de vista"

                if (_parametrosInternoRebarDTO._texToCantidadoBArras != "" && ParameterUtil.FindParaByName(_rebar, "CantidadBarra") != null) ParameterUtil.SetParaInt(_rebar, "CantidadBarra", _parametrosInternoRebarDTO._texToCantidadoBArras);  //"(2+2+2+2)"
                if (_parametrosInternoRebarDTO._texToLargoParciales != "" && ParameterUtil.FindParaByName(_rebar, "LargoParciales") != null) ParameterUtil.SetParaInt(_rebar, "LargoParciales", _parametrosInternoRebarDTO._texToLargoParciales);//(30+100+30)
                if (_parametrosInternoRebarDTO._texToLargoTotal != "" && ParameterUtil.FindParaByName(_rebar, "LargoTotal") != null) ParameterUtil.SetParaInt(_rebar, "LargoTotal", _parametrosInternoRebarDTO._texToLargoTotal);//(30+100+30)
                if (_parametrosInternoRebarDTO._texToTipoDiam != "" && ParameterUtil.FindParaByName(_rebar, "TipoDiametro") != null) ParameterUtil.SetParaInt(_rebar, "TipoDiametro", _parametrosInternoRebarDTO._texToTipoDiam);//(30+100+30)

                if (_parametrosInternoRebarDTO._NUmeroLinea_paraTagRefuerzo > 0 && ParameterUtil.FindParaByName(_rebar, "NumeroLinea") != null)
                    ParameterUtil.SetParaStringNH(_rebar, "NumeroLinea", _parametrosInternoRebarDTO._NUmeroLinea_paraTagRefuerzo.ToString());

                if (tipoBarraHorizontal != "") ParameterUtil.SetParaInt(_rebar, "IDTipo", tipoBarraHorizontal);

                if (_parametrosInternoRebarDTO._IsMalla)
                {
                    if (_parametrosInternoRebarDTO._cuantiaMalla != "" && ParameterUtil.FindParaByName(_rebar, "MallaRebarMuro") != null) ParameterUtil.SetParaInt(_rebar, "MallaRebarMuro", _parametrosInternoRebarDTO._cuantiaMalla);//(30+100+30)
                    if (_parametrosInternoRebarDTO._IdMalla != "" && ParameterUtil.FindParaByName(_rebar, "IdMallaGrupo") != null) ParameterUtil.SetParaInt(_rebar, "IdMallaGrupo", _parametrosInternoRebarDTO._IdMalla);//(30+100+30)
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                TaskDialog.Show("Error ConfigurarAsignarParametrosRebarshape", msj);
            }

        }

        public Result M1_5__ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing()
        {
            if (_interBArraDto.espaciamientoRecorridoBarrasFoot == -1)
                return Result.Succeeded;

            Result result = Result.Failed;
            try
            {
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing((int)_interBArraDto._nuevaLineaCantidadbarra, _interBArraDto.espaciamientoRecorridoBarrasFoot, true, true, true);
                result = Result.Succeeded;

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }

        public Result M1_6_visualizar()
        {
            Result result = Result.Failed;
            try
            {

                //CONFIGURACION DE PATHREINFORMENT
                if (_interBArraDto.view3D_paraVisualizar != null)
                {
                    //permite que la barra se vea en el 3d como solidD
                    _rebar.SetSolidInView(_interBArraDto.view3D_paraVisualizar, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    _rebar.SetUnobscuredInView(_interBArraDto.view3D_paraVisualizar, true);
                }

                if (_view != null)
                {
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    _rebar.SetUnobscuredInView(_view, true);
                }
                //  M3_VerificarBarraMenor15mt();
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }


        public virtual bool M2_DibujarTags(ConfiguracionTAgBarraDTo confBarraTag)
        {
            if (_rebar == null) return false;
            try
            {

                if (_newGeometriaTag.M4_IsFAmiliaValida())
                {
                    foreach (TagBarra item in _newGeometriaTag.listaTag)
                    {
                        if (item == null) continue;
                        if (!item.IsOk) continue;
                        item.DibujarTagRebarV(_rebar, _uiapp, _view, confBarraTag);
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear tag:{ex.Message}");
                return false;
            }
            return true;
        }
    
        protected void MOverHaciaBajo()
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CMOver barra-NH");

                    ElementTransformUtils.MoveElement(_doc, _rebar.Id, _VectorMover);
                    t.Commit();
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puede mover barra");
            }
        }

        protected void M3_VerificarBarraMenor15mt()
        {


        }

    }
}
