using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Dimensiones;
using ApiRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.RebarLosa.ParametrosCompartidos;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using System.Runtime.InteropServices.WindowsRuntime;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras
{
    public abstract class ARebarLosa
    {

        public Document _doc { get; set; }
        protected UIApplication _uiapp;
        public Rebar _rebar { get; set; }

        public View3D view3D { get; set; }

        private View3D view3D_paraVisualizar;

        public View _view { get; set; }

        #region 0)propiedades
        List<ElementId> listaGrupo_LineaRebar = new List<ElementId>();
        List<ElementId> listaGrupo_DimensionCirculo = new List<ElementId>();
        List<ElementId> listaGrupo_Tag = new List<ElementId>();
        //curva de barra referencia
        public List<Curve> listcurve { get; set; }
        protected string _Prefijo_F;
        public XYZ xvec { get; set; }
        public XYZ yvec { get; set; }
        public XYZ origen_forCreateFromRebarShape { get; set; }
        public XYZ norm { get; set; }

        public TipoDireccionBarra TipoDireccionBarra_ { get; set; } = TipoDireccionBarra.NONE;

        #region propiedades para crear Barra
        public RebarShape rebarShape { get; set; }
        public RebarStyle rebarStyle { get; set; }
        public RebarBarType rebarBarType { get; set; }
        //  public Element floor { get; set; }
        public RebarHookOrientation startHookOrient { get; set; }
        public RebarHookOrientation endHookOrient { get; set; }
        public bool useExistingShapeIfPossible { get; set; }
        public bool createNewShape { get; set; }

        #endregion


        public int espesorLosaCM { get; set; }

        protected RebarInferiorDTO _rebarInferiorDTO;
        private Element line_styles_BARRA;

        public IGeometriaTag _newGeometriaTag { get; set; }
        public double espesorLosaFoot { get; set; }

        public double largo { get; set; }
        protected double _EspesorMuro_Izq_abajo = Util.CmToFoot(15);
        protected double _EspesorMuro_Dere_abajo = Util.CmToFoot(15);
        protected double _patabarra;
        public List<Line> ListaFalsoPAthSymbol { get; set; }
        public double elevacionVIew { get; set; }

        protected BarraParametrosCompartidos barraParametrosCompartidos;
        protected XYZ ptoini;
        protected XYZ ptofin;
        protected XYZ ptoMouseAnivelVista;
        protected XYZ direccionBarra;
        protected XYZ dirBarraPerpen;

        public double LargoRecorrido { get; set; }

        protected XYZ _VectorDIreccionLosaFinalExternaInclinada;
        protected XYZ _VectorDIreccionLosaInicialExternaInclinada;
        protected XYZ _VectorMover;

        protected Line ladoAB;
        protected Line ladoBC;
        protected Line ladoCD;
        protected Line ladoDE;
        protected Line ladoEG;
        protected Line ladoGH;


        protected Line ladoAB_pathSym;
        protected Line ladoBC_pathSym;
        protected Line ladoCD_pathSym;
        protected Line ladoDE_pathSym;

        protected Line ladoFG_pathSym;
        protected Line ladoEF_pathSym;

        protected double offInferiorHaciaArribaLosa;
        protected double offSuperiorhaciaBajoLosa;

        protected double _largoPataInclinada;

        protected TipoRebar _BarraTipo;

        protected double mitadDiam_foot;
        protected XYZ dire_Pfin_Pini_XY0 = XYZ.Zero;
        protected XYZ direPAtaINi = XYZ.Zero;
        protected XYZ direPAtaFin = XYZ.Zero;



        #endregion

        #region 'metodos M1A_IsTodoOK()'
        public ARebarLosa(RebarInferiorDTO RebarInferiorDTO)
        {
            this._uiapp = RebarInferiorDTO.uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._rebarInferiorDTO = RebarInferiorDTO;
            this.espesorLosaFoot = RebarInferiorDTO.espesorLosaFoot;
            listaGrupo_LineaRebar = new List<ElementId>();
            listaGrupo_DimensionCirculo = new List<ElementId>();
            listaGrupo_Tag = new List<ElementId>();

            barraParametrosCompartidos = new BarraParametrosCompartidos(this._doc);
            ListaFalsoPAthSymbol = new List<Line>();
            listcurve = new List<Curve>();

            _VectorMover = new XYZ(0, 0, -ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT);
            CalculosIniciales();
            ObtenerBarraTipo();
        }

        private bool CalculosIniciales()
        {
            bool result = true;
            try
            {
                _patabarra = _rebarInferiorDTO.largomin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA + ConstNH.ESPESORMURO_Izq_abajoFOOT;

                this.offInferiorHaciaArribaLosa = Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm);
                this.offSuperiorhaciaBajoLosa = Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm);

                ptoini = _rebarInferiorDTO.listaPtosPerimetroBarras[1];// + new XYZ(0, 0, -offSuperiorhaciaBajoLosa);
                ptofin = _rebarInferiorDTO.listaPtosPerimetroBarras[2];// + new XYZ(0, 0, -offSuperiorhaciaBajoLosa);

                ptoMouseAnivelVista = _rebarInferiorDTO.ptoSeleccionMouse;
                //direccion fin - ini
                //direccionBarra = (ptofin.GetXY0() - ptoini.GetXY0()).Normalize();
                direccionBarra = (ptofin - ptoini).Normalize();
                //si dirbarra = X  ->  dirBarraPerpen=Y
                //dirBarraPerpen = -Util.GetVectorPerpendicular2(direccionBarra);
                XYZ nomalFloor = XYZ.Zero;
                if (_rebarInferiorDTO.floor is Floor)
                    nomalFloor = ((Floor)_rebarInferiorDTO.floor).ObtenerNormal();
                else
                {
                    var caraInf = _rebarInferiorDTO.floor.ObtenerCaraInferior();
                    nomalFloor = caraInf.FaceNormal;

                }
                dirBarraPerpen = -direccionBarra.CrossProduct(nomalFloor);
                LargoRecorrido = _rebarInferiorDTO.listaPtosPerimetroBarras[0].DistanceTo(_rebarInferiorDTO.listaPtosPerimetroBarras[1]);

                XYZ dirBarraPerpen_aux = (_rebarInferiorDTO.listaPtosPerimetroBarras[0] - ptoini).Normalize();

                XYZ var1 = Util.CrossProduct(_rebarInferiorDTO.listaPtosPerimetroBarras[0] - _rebarInferiorDTO.listaPtosPerimetroBarras[1], _rebarInferiorDTO.listaPtosPerimetroBarras[2] - _rebarInferiorDTO.listaPtosPerimetroBarras[1]).Normalize();
                XYZ var2 = Util.CrossProduct(_rebarInferiorDTO.listaPtosPerimetroBarras[2] - _rebarInferiorDTO.listaPtosPerimetroBarras[1], _rebarInferiorDTO.listaPtosPerimetroBarras[0] - _rebarInferiorDTO.listaPtosPerimetroBarras[1]).Normalize();


                _VectorDIreccionLosaFinalExternaInclinada = ptofin - ptoini;
                _VectorDIreccionLosaInicialExternaInclinada = ptoini - ptofin;

                double _largoTraslapo = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_rebarInferiorDTO.diametroMM);
                double largoDefult = Util.CmToFoot(60);
                _largoPataInclinada = (largoDefult / 2 > _largoTraslapo / 2 ? largoDefult / 2 : _largoTraslapo / 2);

                //ConstantesGenerales.sbLog.AppendLine("f3_incli:");
                string nombreElevacion = _uiapp.ActiveUIDocument.Document.ActiveView.Name;

                if (nombreElevacion.Contains("3D"))
                    this.elevacionVIew = _rebarInferiorDTO.listaPtosPerimetroBarras.Max(c => c.Z);
                else
                {
                    var resultZ = _uiapp.ActiveUIDocument.Document.ActiveView.Obtener_Z_SoloPLantas();
                    if (!resultZ.Isok)
                    {
                        result = false;

                    }
                    this.elevacionVIew = resultZ.valorz;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'ObtnerPtosInicialYFinalDeBarra' en {this.GetType().Name}.   ex:{ex.Message} ");
                return false;
            }
            return result;

        }

        private void ObtenerBarraTipo()
        {


            switch (_rebarInferiorDTO.tipoBarra)
            {

                case TipoBarra.NONE:
                    break;
                case TipoBarra.f1_esc45_conpata:
                    _BarraTipo = TipoRebar.LOSA_ESC_F1_45_CONPATA;
                    break;
                case TipoBarra.f1_esc135_sinpata:
                    _BarraTipo = TipoRebar.LOSA_ESC_F1_135_SINPATA;
                    break;
                case TipoBarra.f3_esc45:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_45;
                    break;
                case TipoBarra.f3_esc135:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_135;
                    break;
                case TipoBarra.f3b_esc45:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3B_45;
                    break;
                case TipoBarra.f3b_esc135:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3B_135;
                    break;
                case TipoBarra.f3_ba:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_BA;
                    break;
                case TipoBarra.f3_ab:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_AB;
                    break;
                case TipoBarra.f3_0a:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_0A;
                    break;
                case TipoBarra.f3_a0:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_A0;
                    break;
                case TipoBarra.f3_0b:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_0B;
                    break;
                case TipoBarra.f3_b0:
                    _BarraTipo = TipoRebar.LOSA_ESC_F3_B0;
                    break;
                case TipoBarra.f1A_pataB:
                    break;
                case TipoBarra.f1B_pataB:
                    break;
                case TipoBarra.f1A_pataA:
                    break;
                case TipoBarra.f1B_pataA:
                    break;
                case TipoBarra.f1_ab:
                    break;
                case TipoBarra.f1_b:
                    break;
                case TipoBarra.f3_incli:
                    _BarraTipo = TipoRebar.LOSA_INCLI_F3;
                    break;
                case TipoBarra.f3_incli_esc:
                    break;
                case TipoBarra.f4_incli_esc:
                    break;
                case TipoBarra.f1_a:
                    break;
                case TipoBarra.f4_incli:
                    _BarraTipo = TipoRebar.LOSA_INCLI_F4;
                    break;
                case TipoBarra.f12:
                    break;
                case TipoBarra.f1_incliInf:
                    _BarraTipo = TipoRebar.LOSA_INCLI_F1;
                    break;
                case TipoBarra.f3_refuezoSuple:
                    break;
                default:
                    break;
            }
            //_BarraTipo
        }

        #region 2) metodos



        private void AnalizarLado(Line Ladoxx, NombreParametros para)
        {
            if (Ladoxx != null)
            {
                // listcurve.Add(Ladoxx);
                barraParametrosCompartidos.listaParametroBarra.Add(new ParametroBarra(para, Ladoxx.Length));
                largo += Ladoxx.Length;
            }
            else
                barraParametrosCompartidos.listaParametroBarra.Add(new ParametroBarra(para, 0));
        }
        private void AnalizarLadoREbar(Line Ladoxx)
        {
            if (Ladoxx != null) listcurve.Add(Ladoxx);
        }
        //busca el pto de interseccion entre la linea de direccitz y la lineacentas del pathsymbolfalso
        protected void ObtenerNuevoptoCirculo()
        {
            Line LineaMayorLArgo = ListaFalsoPAthSymbol.MinBy(c => -c.Length);
            if (LineaMayorLArgo == null)
            {
                Util.ErrorMsg(" No se enontro Pathfalso para obtener ubicacion de circulo");
                return;
            }
            XYZ ptoIniPAthSymbolNivelView = LineaMayorLArgo.GetEndPoint(0);// ListaFalsoPAthSymbol[elemntosPAthsimbol].GetEndPoint(0);
            XYZ ptoFinPAthSymbolNivelView = LineaMayorLArgo.GetEndPoint(1); //ListaFalsoPAthSymbol[elemntosPAthsimbol].GetEndPoint(1);
            ptoMouseAnivelVista = Util.IntersectionXYZ(ptoIniPAthSymbolNivelView.GetXY0(), ptoFinPAthSymbolNivelView.GetXY0(),
                                                      _rebarInferiorDTO.PtoDirectriz1.GetXY0(), _rebarInferiorDTO.PtoDirectriz2.GetXY0()).AsignarZ(elevacionVIew);
        }

        public bool CopiandoParametrosLado_COnALargoRebar()
        {
            try
            {
                norm = dirBarraPerpen.Normalize();


                ConstNH.corte();
                listcurve.Clear();
                // borrar las 6 lineas de abajo y desmarcar donde dice aqui
                AnalizarLadoREbar(ladoAB);
                AnalizarLadoREbar(ladoBC);
                AnalizarLadoREbar(ladoCD);
                AnalizarLadoREbar(ladoDE);
                AnalizarLadoREbar(ladoEG);
                AnalizarLadoREbar(ladoGH);

                //** aqui
                //AnalizarLado(ladoAB, NombreParametros.A_);
                //AnalizarLado(ladoBC, NombreParametros.B_);
                //AnalizarLado(ladoCD, NombreParametros.C_);
                //AnalizarLado(ladoDE, NombreParametros.D_);
                //AnalizarLado(ladoEG, NombreParametros.E_);
                //AnalizarLado(ladoGH, NombreParametros.G_);
                //barraParametrosCompartidos.listaParametroBarra.Add(new ParametroBarra(NombreParametros.LL, largo));
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener largos con rebar : {ex.Message} ");
                return false;
            }
            return true;
        }


        public bool CopiandoParametrosLado_COnPAthSymbol()
        {
            ConstNH.corte();
            //borrar toda esta seccion
            try
            {
                // norm = dirBarraPerpen.Normalize();
                barraParametrosCompartidos.listaParametroBarra.Clear();
                largo = 0;
                AnalizarLado(ladoAB_pathSym, NombreParametros.A_);
                AnalizarLado(ladoBC_pathSym, NombreParametros.B_);
                AnalizarLado(ladoCD_pathSym, NombreParametros.C_);
                AnalizarLado(ladoDE_pathSym, NombreParametros.D_);
                AnalizarLado(ladoEF_pathSym, NombreParametros.E_);
                AnalizarLado(ladoFG_pathSym, NombreParametros.G_);

                barraParametrosCompartidos.listaParametroBarra.Add(new ParametroBarra(NombreParametros.LL, largo));
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener largos con pathsymbol : {ex.Message} ");
                return false;
            }
            return true;
        }

        protected void OBtenerListaFalsoPAthSymbol()
        {
            if (ladoAB_pathSym != null) ListaFalsoPAthSymbol.Add(ladoAB_pathSym);
            if (ladoBC_pathSym != null) ListaFalsoPAthSymbol.Add(ladoBC_pathSym);
            if (ladoCD_pathSym != null) ListaFalsoPAthSymbol.Add(ladoCD_pathSym);
            if (ladoDE_pathSym != null) ListaFalsoPAthSymbol.Add(ladoDE_pathSym);
            if (ladoEF_pathSym != null) ListaFalsoPAthSymbol.Add(ladoEF_pathSym);
            if (ladoFG_pathSym != null) ListaFalsoPAthSymbol.Add(ladoFG_pathSym);
        }


        public virtual void ObtenerPAthSymbolTAG()
        {
            _newGeometriaTag.Ejecutar(new GeomeTagArgs() { angulorad = _rebarInferiorDTO.anguloBarraRad });
            //_newGeometriaTag.listaTag.Where(rr=>rr.nombre=="").ToList().ForEach(c=> c.posicion. )

        }
        #endregion


        #region Metodos 'M2A_GenerarBarra()'


        public virtual void M2A_GenerarBarra()
        {
            listaGrupo_LineaRebar.Clear();
            M1_ConfigurarDatosIniciales();

            if (!CopiandoParametrosLado_COnALargoRebar()) return;
            if (!CopiandoParametrosLado_COnPAthSymbol()) return;

            if (M3_DibujarBarraCurve() != Result.Succeeded) return;
            if (_rebar == null)
            {
                Util.ErrorMsg("Error al crear rebar. Rebar igual null");
                return;
            }

            M3A_1_CopiarParametrosCOmpartidos();
            //parametros no son correctos
            // M4_ConfigurarAsignarParametrosRebarshape();
            M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing();
            M6_visualizar();
            M8_CrearPatSymbolFalso();
            M9_CreaTAg();
            M10_CreaDimension();
            M11_CreaCirculo();
            M11_CrearGrupo();
            //M12_MOverHaciaBajo();

        }



        public void M1_ConfigurarDatosIniciales()
        {
            startHookOrient = RebarHookOrientation.Left; //defecto
            endHookOrient = RebarHookOrientation.Left; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = true;
            //listcurve = new List<Curve>();


            //double aux_espesorLosa = 0.0;
            //double.TryParse(ParameterUtil.FindParaByBuiltInParameter(floor, BuiltInParameter.FLOOR_ATTR_THICKNESSre_PARAM, doc), out aux_espesorLosa);
            //espesorLosaFoot = aux_espesorLosa;
            espesorLosaCM = (int)Util.FootToCm(espesorLosaFoot);

            rebarStyle = RebarStyle.Standard;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + _rebarInferiorDTO.diametroMM, _doc, true);

            view3D = TiposFamilia3D.Get3DBuscar(_doc);
            if (view3D == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return;
            }
            view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            //vista actual
            _view = _doc.ActiveView;
        }


        public Result M3_DibujarBarraCurve()
        {
            Result result = Result.Failed;

            if (listcurve.Count == 0)
            {
                Util.ErrorMsg("Error: Lista de curvas de barras igual a cero. Revisar");

                return Result.Failed;
            }
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  nuevo refuerzo-NH");
                    _rebar = Rebar.CreateFromCurves(_doc,
                                               rebarStyle,
                                               rebarBarType,
                                               null,
                                               null,
                                               _rebarInferiorDTO.floor,
                                               norm,
                                               listcurve,
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
            if (_rebar == null)
            {

                Util.ErrorMsg($"Error: al crear barras rebar. Valor igual null.");

                return Result.Failed;
            }
            if (_rebar.TotalLength == 0)
            {
                Util.ErrorMsg($"Error: Barra creada de largo cero. Revisar\n\n{AyudaPuntosDeCurva.ObtnertextoListapto(listcurve)}");
                CompatibilityMethods.DeleteNh(_doc, _rebar);
                return Result.Failed;
            }
            //  if (_rebar != null) listaGrupo.Add(_rebar.Id);


            return result;
        }

        public Result M3_DibujarBarraRebarShape()
        {
            Result result = Result.Failed;

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  nuevo refuerzo rebarshape-NH");
                    _rebar = Rebar.CreateFromRebarShape(_doc,
                                               rebarShape,
                                               rebarBarType,
                                               _rebarInferiorDTO.floor,
                                               origen_forCreateFromRebarShape,
                                               xvec,
                                               yvec);
                    result = Result.Succeeded;
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
                TaskDialog.Show("Error DibujarBarraRebarShape", msj);
            }

            return result;
        }

        public void M3A_1_CopiarParametrosCOmpartidos()
        {
            barraParametrosCompartidos.CopiarParametrosCompartidos_fundaciones(_rebar, _BarraTipo, _Prefijo_F, TipoDireccionBarra_, _rebarInferiorDTO);


        }

        //no necesaria

        public Result M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing()
        {
            Result result = Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ConfiguraEspaciamiento barra refuerzo-NH");


                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    //rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(_rebarInferiorDTO.espaciamientoFoot, _rebarInferiorDTO.largo_recorridoFoot, true, true, true);
                    //  rebarShapeDrivenAccessor.SetLayoutAsFixedNumber(2,1,  true, true, true);
                    rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(_rebarInferiorDTO.cantidadBarras, _rebarInferiorDTO.espaciamientoFoot, true, true, true);
                    //  rebarShapeDrivenAccessor.SetLayoutAsFixedNumber(2, 1, true, true, true);

                    for (int i = 0; i < _rebar.Quantity; i++)
                    {
                        if ((int)(_rebar.Quantity / 2) != i)
                            _rebar.SetBarHiddenStatus(_view, i, true);
                    }


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
        public Result M6_visualizar()
        {
            Result result = Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  visualizar-NH");
                    //CONFIGURACION DE PATHREINFORMENT
                    if (view3D_paraVisualizar != null)
                    {
                        //permite que la barra se vea en el 3d como solidD
                        _rebar.SetSolidInView(view3D_paraVisualizar, true);
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        _rebar.SetUnobscuredInView(view3D_paraVisualizar, true);
                    }
                    if (_view != null && (!(_view is View3D)))
                    {
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        //  _rebar.SetPresentationMode(viewActual, RebarPresentationMode.FirstLast);
                        // _rebar.SetBarHiddenStatus(viewActual,0,true);
                        //_rebar.IncludeLastBar = false;
                        //_rebar.IncludeFirstBar = false;
                        _rebar.SetUnobscuredInView(_view, false);
                    }

                    M6_1_CAmbiarColor();

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



        public void M6_1_CAmbiarColor()
        {
            if (_rebar == null) return;


            Color newcolorBlanco = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.blanco);


            VisibilidadElementoLosa visibilidadElement = new VisibilidadElementoLosa(_uiapp);
            visibilidadElement.ChangeElementColorSinTrans(_rebar.Id, newcolorBlanco, true);
            //visibilidadElement
        }

        public void M7_OcultarBarraCreada()
        {
            if (!_rebar.IsValidObject || _rebar.IsHidden(_view)) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Ocultando RebarLosa-NH");
                    List<ElementId> ListElemId = new List<ElementId>();
                    ListElemId.Add(_rebar.Id);

                    if (_rebar.CanBeHidden(_view)) _view.HideElements(ListElemId);
                    // doc.Regenerate();
                    trans2.Commit();
                    // uidoc.RefreshActiveView();
                } // fin trans 
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ocultar rebar: {ex.Message}");
            }

        }

        public void M8_CrearPatSymbolFalso()
        {
            try
            {


                if (ListaFalsoPAthSymbol.Count == 0) return;
                M8_1_ObtenerLineStyle_Barra();
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("Creata PatSymbolFalso-NH");
                    foreach (Line item in ListaFalsoPAthSymbol)
                    {
                        DetailLine lineafalsa = _doc.Create.NewDetailCurve(_view, item) as DetailLine;
                        lineafalsa.LineStyle = line_styles_BARRA;
                        if (lineafalsa != null) listaGrupo_LineaRebar.Add(lineafalsa.Id);
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear PatSymbol Falso :{ex.Message}");
            }

        }
        private void M8_1_ObtenerLineStyle_Barra()
        {


            line_styles_BARRA = TiposLineaPattern.ObtenerTipoLinea("Barra", _doc);
            if (line_styles_BARRA == null)
            {
                CrearLineStyle CrearLineStyle = new CrearLineStyle(_doc, "Barra", 1, new Color(255, 0, 255), "IMPORT-HIDDEN");
                CrearLineStyle.CreateLineStyleConTrans();

                line_styles_BARRA = TiposLineaPattern.ObtenerTipoLinea("Barra", _doc);
            }
        }

        public void M9_CreaTAg()
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
                            item.DibujarTagrREbarLosa(_rebar, _uiapp, _view, new XYZ(0, 0, 0));
                            if (item.ElementIndependentTagPath != null) listaGrupo_Tag.Add(item.ElementIndependentTagPath.Id);
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al Crear tag");
            }
        }
        public void M10_CreaDimension()
        {
            try
            {
                CreadorDimensiones EditarPathReinMouse = new CreadorDimensiones(_doc, _rebarInferiorDTO.PtoDirectriz1, _rebarInferiorDTO.PtoDirectriz2, "DimensionRebar");
                Dimension resultDimension = EditarPathReinMouse.Crear_conTrans("BarraDimension");
                if (resultDimension != null) listaGrupo_DimensionCirculo.Add(EditarPathReinMouse.lineafalsa.Id);
            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al Crear tag");
            }
        }
        public void M11_CreaCirculo()
        {
            try
            {
                var circ = CreadorCirculo.CrearCirculo_DetailLine_ConTrans(Util.CmToFoot(8), ptoMouseAnivelVista, _uiapp.ActiveUIDocument, XYZ.BasisX, XYZ.BasisY, "BarraCirculo");
                if (circ != null) listaGrupo_DimensionCirculo.Add(circ.Id);
            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al Crear tag");
            }
        }
        public void M11_CrearGrupo()
        {
            if (listaGrupo_LineaRebar.Count < 1) return;

            if (listaGrupo_DimensionCirculo.Count > 0)
                listaGrupo_LineaRebar.AddRange(listaGrupo_DimensionCirculo);

            try
            {
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("Creando grupo-NH");

                    if (listaGrupo_LineaRebar.Count > 1)
                    {
                        Group grup = _doc.Create.NewGroup(listaGrupo_LineaRebar);
                    }

                    //  if(listaGrupo_DimensionCirculo.Count>1)
                    //_doc.Create.NewGroup(listaGrupo_DimensionCirculo); 
                    //var grouptag = _doc.Create.NewGroup(listaGrupo_Tag);
                    tx.Commit();
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("Error al Crear tag");
            }
        }
        #endregion
        #endregion
        protected void M12_MOverHaciaBajo(int i = 1)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CMOver barra-NH");

                    ElementTransformUtils.MoveElement(_doc, _rebar.Id, i * _VectorMover);
                    t.Commit();
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puede mover barra");
            }
        }

        /// <summary>
        /// para usar redefinir 
        ///  _VectorMover = new XYZ(0, 0, -(deltaMOverAbajo));
        /// </summary>
        protected void M12_MOverSegunVector()
        {
            M12_MOverHaciaBajo();
        }

        protected void M12_MOverHaciaArriba()
        {
            M12_MOverHaciaBajo(-1);
        }
    }
}
