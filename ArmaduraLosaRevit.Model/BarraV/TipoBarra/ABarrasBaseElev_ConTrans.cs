using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public abstract class ABarrasBaseElev_ConTrans
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected IntervaloBarrasDTO _interBArraDto;
        protected View _View;
        protected List<Curve> _listcurve;
        protected RebarStyle _rebarStyle;
        protected  RebarBarType _rebarBarType;
        protected Rebar _rebar;
        protected XYZ _VectorMover;
        protected IGeometriaTag _newGeometriaTag;
        protected TipoRebar _BarraTipo = TipoRebar.NONE;
        public RebarHookOrientation startHookOrient { get; set; }
        public RebarHookOrientation endHookOrient { get; set; }
        public bool useExistingShapeIfPossible { get; set; }
        public bool createNewShape { get; set; }
        public RebarHookType _tipoHookFinal { get; set; }
        public bool IsSoloTag { get; set; }

        public TipoRedondero tipoRedondeo { get; set; }
        protected double delta;
        public ABarrasBaseElev_ConTrans(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._interBArraDto = interBArraDto;
            this._newGeometriaTag = _newGeometriaTag;
            this._View = _doc.ActiveView;
            this._listcurve = new List<Curve>();

            tipoRedondeo = TipoBarra.TipoRedondero.Redondeat_Cinco_5;
            delta = 0;
        }


    
        //public XYZ GetIsPo_P2paraTag()
        //{
        //    return _interBArraDto.ptofinal;
        //}

        public bool M1_1_CalculosIniciales()
        {
            startHookOrient = RebarHookOrientation.Left; //defecto
            endHookOrient = RebarHookOrientation.Left; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = true;

            _rebarStyle = RebarStyle.Standard;
            _rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + _interBArraDto.diametroMM, _doc, true);

            if (_rebarBarType == null)
            {
                Util.ErrorMsg($"Error al obtener el tipo { "Ø "+ _interBArraDto.diametroMM}");
                return false;
            }
            return true;
        }


        public Result M1_3_DibujarBarraCurve()
        {
            Result result = Result.Failed;


            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  nuevo refuerzo-NH");
                    _rebar = Rebar.CreateFromCurves(_doc,
                                               _rebarStyle,
                                               _rebarBarType,
                                               null,
                                               null,
                                               _interBArraDto.ElementoHost,
                                               _interBArraDto.DireccionRecorridoBarra,
                                               _listcurve,
                                               startHookOrient,
                                               endHookOrient,
                                               useExistingShapeIfPossible,
                                               createNewShape);

                    t.Commit();


                }

                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                string msj = ex.Message;

                result = Result.Cancelled;
            }

            if (_rebar == null) return result = Result.Cancelled;

            return result;
        }

        public ElementId M3_ObtenerIdRebar()
        {
            return _rebar.Id;
        }

     
        public IbarraBaseResultDTO GetResult()
        {
            return new IbarraBaseResultDTO() {
                _rebar = _rebar,
                OrientacionTagGrupoBarras = _interBArraDto.OrientacionTagGrupoBarras,
                IsNoProloganLosaArriba= _interBArraDto.IsNoProloganLosaArriba
             };
        }

        public void M1_4_ConfigurarAsignarParametrosRebarshape(ParametrosInternoRebarDTO _parametrosInternoRebarDTO,string tipoBarraHorizontal)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("modificar parametros rebarrefuerzo-NH");
                    if (_View != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null) ParameterUtil.SetParaInt(_rebar, "NombreVista", _View.ObtenerNombreIsDependencia());  //"nombre de vista"
                   
                    if ( ParameterUtil.FindParaByName(_rebar, "BarraTipo" ) != null && _BarraTipo != TipoRebar.NONE)
                        ParameterUtil.SetParaInt(_rebar, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo));  //"nombre de vista"

                    if (_parametrosInternoRebarDTO._texToCantidadoBArras != "" && ParameterUtil.FindParaByName(_rebar, "CantidadBarra") != null) ParameterUtil.SetParaInt(_rebar, "CantidadBarra", _parametrosInternoRebarDTO._texToCantidadoBArras);  //"(2+2+2+2)"
                    if (_parametrosInternoRebarDTO._texToLargoParciales != "" && ParameterUtil.FindParaByName(_rebar, "LargoParciales") != null) ParameterUtil.SetParaInt(_rebar, "LargoParciales", _parametrosInternoRebarDTO._texToLargoParciales);//(30+100+30)
                    if (_parametrosInternoRebarDTO._texToLargoTotal != "" && ParameterUtil.FindParaByName(_rebar, "LargoTotal") != null) ParameterUtil.SetParaInt(_rebar, "LargoTotal", _parametrosInternoRebarDTO._texToLargoTotal);//(30+100+30)
                    if (_parametrosInternoRebarDTO._texToTipoDiam != "" && ParameterUtil.FindParaByName(_rebar, "TipoDiametro") != null) 
                        ParameterUtil.SetParaInt(_rebar, "TipoDiametro", _parametrosInternoRebarDTO._texToTipoDiam);//(30+100+30)

                    if (_parametrosInternoRebarDTO._NUmeroLinea_paraTagRefuerzo>0 && ParameterUtil.FindParaByName(_rebar, "NumeroLinea") != null)
                        ParameterUtil.SetParaStringNH(_rebar, "NumeroLinea", _parametrosInternoRebarDTO._NUmeroLinea_paraTagRefuerzo.ToString());

                    if (tipoBarraHorizontal != "") ParameterUtil.SetParaInt(_rebar, "IDTipo", tipoBarraHorizontal);
                    t.Commit();
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


            Result result = Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ConfiguraEspaciamiento barra refuerzo-NH");
                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(_interBArraDto.espaciamientoRecorridoBarrasFoot, _interBArraDto.recorridoBarrar, true, true, true);
                    //  rebarShapeDrivenAccessor.SetLayoutAsFixedNumber(2,1,  true, true, true);
                    //rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(2, espesorLosaFoot - offInferiorHaciaArribaLosa - offSuperiorhaciaBajoLosa, true, true, true);
                    //  rebarShapeDrivenAccessor.SetLayoutAsFixedNumber(2, 1, true, true, true);
                    result = Result.Succeeded;
                    t.Commit();
                }
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
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  visualizar-NH");
                    //CONFIGURACION DE PATHREINFORMENT
                    if (_interBArraDto.view3D_paraVisualizar != null)
                    {
                        //permite que la barra se vea en el 3d como solidD
                        _rebar.SetSolidInView(_interBArraDto.view3D_paraVisualizar, true);
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        _rebar.SetUnobscuredInView(_interBArraDto.view3D_paraVisualizar, true);
                 
                    }

                    if (_View != null)
                    {
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        _rebar.SetUnobscuredInView(_View, true);
                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }


        public bool M2_DibujarTags(ConfiguracionTAgBarraDTo confBarraTag)
        {
            try
            {
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("creando tag-NH");
                    if (_newGeometriaTag.M4_IsFAmiliaValida())
                    {
                        foreach (TagBarra item in _newGeometriaTag.listaTag)
                        {
                            if (item == null) continue;
                            if (!item.IsOk) continue;
                            item.DibujarTagRebarV(_rebar, _uiapp, _View, confBarraTag);
                            // if (item.ElementIndependentTagPath != null) listaGrupo_Tag.Add(item.ElementIndependentTagPath.Id);
                        }
                    }
                    tx.Commit();
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

        //public bool ObtenerTipoRedondero()
        //{
        //    try
        //    {

        //        if (tipoRedondeo == TipoBarra.TipoRedondero.Redondeat_Cinco_5)
        //        {
        //            if (RedonderLargoBarras.RedondearFoot5_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot(_interBArraDto.diametroMM)))
        //                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);
        //        }
        //        else
        //        {
        //            if (RedonderLargoBarras.RedondearFoot1_mascercano(_interBArraDto.ptoini.DistanceTo(_interBArraDto.ptofinal) + Util.MmToFoot(_interBArraDto.diametroMM)))
        //                delta = Util.CmToFoot(RedonderLargoBarras.largoModificar_deltaCM);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Util.ErrorMsg($"Error al redondear barra. ex:{ex.Message}");
        //        return false;
        //    }
        //    return true;
        //}

    }
}
