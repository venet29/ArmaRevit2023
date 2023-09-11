using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

using ArmaduraLosaRevit.Model.UTILES;

using ArmaduraLosaRevit.Model.Dimensiones;
using ApiRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;

using ArmaduraLosaRevit.Model.Extension;



using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.RebarLosa.ParametrosCompartidos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.TextoNoteNH;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos
{
    public abstract class ARebarLosa_desglose
    {

        public Document _doc { get; set; }
        protected UIApplication _uiapp;
        protected List<ParametroShareNhDTO> listaPArametroSharenh;

        public double LargoTotalSumaParcialesFoot { get; set; }
        public Rebar _rebar { get; set; }

        public View3D view3D { get; set; }

#pragma warning disable CS0649 // Field 'ARebarLosa_desglose.view3D_paraVisualizar' is never assigned to, and will always have its default value null
        private View3D view3D_paraVisualizar;
#pragma warning restore CS0649 // Field 'ARebarLosa_desglose.view3D_paraVisualizar' is never assigned to, and will always have its default value null

        public View _view { get; set; }

        #region 0)propiedades
        List<ElementId> listaGrupo_LineaRebar = new List<ElementId>();
        List<ElementId> listaGrupo_DimensionCirculo = new List<ElementId>();
        List<ElementId> listaGrupo_Tag = new List<ElementId>();
        //curva de barra referencia
        public List<Curve> listcurve { get; set; }

        private string graphic_stylesName;
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

        protected RebarElevDTO _rebarInferiorDTO;
        private Element line_styles_BARRA;

        public IGeometriaTag _newGeometriaTag { get; set; }
        public double espesorLosaFoot { get; set; }

        public double largo { get; set; }
        protected double _EspesorMuro_Izq_abajo = Util.CmToFoot(15);
        protected double _EspesorMuro_Dere_abajo = Util.CmToFoot(15);
        protected double _patabarra;
        public List<Curve> ListaFalsoPAthSymbol { get; set; }
        public double elevacionVIew { get; set; }

        protected BarraParametrosCompartidos barraParametrosCompartidos;
        protected XYZ ptoini;
        protected XYZ ptofin;
        protected XYZ ptoMouseAnivelVista;
        protected XYZ direccionBarra;
        protected XYZ dirBarraPerpen;
        private double angleRADNormalHostYEJeZ;

        public double LargoRecorrido { get; set; }

        protected XYZ _VectorDIreccionLosaFinalExternaInclinada;
        protected XYZ _VectorDIreccionLosaInicialExternaInclinada;
        protected XYZ _VectorMover;

        protected Line ladoAB;
        protected Line ladoBC;
        protected Line ladoCD;
        protected Line ladoDE;
        protected Line ladoGH;
        protected Line ladoEG;

        protected Curve ladoAB_pathSym;
        protected Curve ladoBC_pathSym;
        protected Curve ladoCD_pathSym;
        protected Curve ladoDE_pathSym;
        protected Curve ladoEF_pathSym;
        protected Curve ladoFG_pathSym;
        protected Curve ladoGH_pathSym;
        protected Curve ladoHI_pathSym;
        protected Curve ladoIJ_pathSym;
        protected Curve ladoJK_pathSym;
        protected Curve ladoKL_pathSym;


        protected double offInferiorHaciaArribaLosa;
        protected double offSuperiorhaciaBajoLosa;

        protected double _largoPataInclinada;

        protected TipoRebar _BarraTipo;

        protected XYZ _ptoTexto;
        protected string _textoBArras;
        private Element line_styles_srv;
        protected TipoCOnfLargo _configLargo;
        protected double _tolerancia;
        #endregion

        #region 'metodos M1A_IsTodoOK()'
        public ARebarLosa_desglose(RebarElevDTO _RebarInferiorDTO)
        {
            this._uiapp = _RebarInferiorDTO.uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = (_RebarInferiorDTO._View == null ? _doc.ActiveView : _RebarInferiorDTO._View);
            this._rebarInferiorDTO = _RebarInferiorDTO;
            this.espesorLosaFoot = _RebarInferiorDTO.espesorLosaFoot;

            this.listaPArametroSharenh = new List<ParametroShareNhDTO>();

            this.LargoTotalSumaParcialesFoot = _RebarInferiorDTO.LargoTotalSumaParcialesFoot;
            
           listaGrupo_LineaRebar = new List<ElementId>();
            listaGrupo_DimensionCirculo = new List<ElementId>();
            listaGrupo_Tag = new List<ElementId>();

            barraParametrosCompartidos = new BarraParametrosCompartidos(this._doc);
            ListaFalsoPAthSymbol = new List<Curve>();
            listcurve = new List<Curve>();
            graphic_stylesName = "SRV-3";
            _VectorMover = new XYZ(0, 0, -ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT);

            if (_RebarInferiorDTO.Config_EspecialCorte != null)
            {
                _configLargo = _RebarInferiorDTO.Config_EspecialCorte.TipoCOnfigLargo;
                _tolerancia = _RebarInferiorDTO.Config_EspecialCorte.tolerancia;
            }
            CalculosIniciales();
        }

        private void CalculosIniciales()
        {
            _rebar = _rebarInferiorDTO.Rebar_;
            _patabarra = _rebarInferiorDTO.largomin_1 * ConstNH.CONST_PORCENTAJE_LARGOPATA + ConstNH.ESPESORMURO_Izq_abajoFOOT;

            this.offInferiorHaciaArribaLosa = Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm);
            this.offSuperiorhaciaBajoLosa = Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm);

            ptoini = _rebarInferiorDTO.ptoini;// listaPtosPerimetroBarras[1];// + new XYZ(0, 0, -offSuperiorhaciaBajoLosa);
            ptofin = _rebarInferiorDTO.ptofinal;// listaPtosPerimetroBarras[2];// + new XYZ(0, 0, -offSuperiorhaciaBajoLosa);

            ptoMouseAnivelVista = _rebarInferiorDTO.ptoSeleccionMouse;
            //direccion fin - ini
            direccionBarra = (ptofin - ptoini).Normalize();
            //si dirbarra = X  ->  dirBarraPerpen=Y
            dirBarraPerpen = -Util.GetVectorPerpendicular2(direccionBarra);

            angleRADNormalHostYEJeZ = direccionBarra.GetAngleEnZ_respectoPlanoXY(false);

            _VectorDIreccionLosaFinalExternaInclinada = ptofin - ptoini;
            _VectorDIreccionLosaInicialExternaInclinada = ptoini - ptofin;

            double _largoTraslapo = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_rebarInferiorDTO.diametroMM);
            double largoDefult = Util.CmToFoot(60);
            _largoPataInclinada = (largoDefult / 2 > _largoTraslapo / 2 ? largoDefult / 2 : _largoTraslapo / 2);

            //ConstantesGenerales.sbLog.AppendLine("f3_incli:");
            string nombreElevacion = _uiapp.ActiveUIDocument.Document.ActiveView.Name;
            //this.elevacionVIew = (nombreElevacion.Contains("3D") ? _rebarInferiorDTO.listaPtosPerimetroBarras.Max(c => c.Z)
            //                                                     : _uiapp.ActiveUIDocument.Document.ActiveView.GenLevel.ProjectElevation);

            //line_styles_srv = TiposLinea.ObtenerTipoLinea(graphic_stylesName, _doc);
            M8_1_ObtenerLineStyle_srv();
        }

        private void M8_1_ObtenerLineStyle_srv()
        {


            line_styles_srv = TiposLineaPattern.ObtenerTipoLinea(graphic_stylesName, _doc);
            if (line_styles_srv == null)
            {
                CrearLineStyle CrearLineStyle = new CrearLineStyle(_doc, graphic_stylesName, 1, new Color(255, 255, 255), "IMPORT-CENTER");
                CrearLineStyle.CreateLineStyleConTrans();

                line_styles_srv = TiposLineaPattern.ObtenerTipoLinea(graphic_stylesName, _doc);
            }
        }
        private void M8_1_ObtenerLineStyle_Barra()
        {


            line_styles_BARRA = TiposLineaPattern.ObtenerTipoLinea("Lines", _doc);
            if (line_styles_BARRA == null)
            {
                CrearLineStyle CrearLineStyle = new CrearLineStyle(_doc, "Barra", 1, new Color(255, 0, 255), "IMPORT-CENTER");
                CrearLineStyle.CreateLineStyleConTrans();

                line_styles_BARRA = TiposLineaPattern.ObtenerTipoLinea("Barra", _doc);
            }
        }

        protected void redonderar_5()
        {
            double largo = Math.Round(Util.FootToCm(_rebarInferiorDTO.LargoTotalSumaParcialesFoot));
            double b = (largo / 10.0f % 1);  // b:0.48
            double dif = 0;
            if (b < _tolerancia / 10.0f)
            {
                //dif = (_tolerancia / 10.0f);
                LargoTotalSumaParcialesFoot = Util.CmToFoot(largo - b * 10);
            }
            else if (b < 0.5)
            {
                dif = 0.5 - b;
                LargoTotalSumaParcialesFoot = Util.CmToFoot(largo + dif * 10);
            }
            else if (0.5 < b && b < 0.5 + _tolerancia / 10.0f)
            {
                dif = b - 0.5;
                LargoTotalSumaParcialesFoot = Util.CmToFoot(largo - dif * 10);
            }
            else if (0.5 + _tolerancia / 10.0f < b && b < 1)
            {
                dif = 1 - b;
                LargoTotalSumaParcialesFoot = Util.CmToFoot(largo + dif * 10);
            }
        }
        protected void redonderar_10()
        {
            double largo = Math.Round(Util.FootToCm(_rebarInferiorDTO.LargoTotalSumaParcialesFoot));
            double b = (largo / 10.0f % 1);  // b:0.48
            double dif = 0;
            if (b < _tolerancia / 10.0f)
            {
                //dif = (_tolerancia / 10.0f);
                LargoTotalSumaParcialesFoot = Util.CmToFoot(largo - b * 10);
            }
            else if (b < 1)
            {
                dif = 1 - b;
                LargoTotalSumaParcialesFoot = Util.CmToFoot(largo + dif * 10);
            }

        }
        #region 2) metodos




        protected void OBtenerListaFalsoPAthSymbol()
        {
            if (ladoAB_pathSym != null) ListaFalsoPAthSymbol.Add(ladoAB_pathSym);
            if (ladoBC_pathSym != null) ListaFalsoPAthSymbol.Add(ladoBC_pathSym);
            if (ladoCD_pathSym != null) ListaFalsoPAthSymbol.Add(ladoCD_pathSym);
            if (ladoDE_pathSym != null) ListaFalsoPAthSymbol.Add(ladoDE_pathSym);
            if (ladoEF_pathSym != null) ListaFalsoPAthSymbol.Add(ladoEF_pathSym);
            if (ladoFG_pathSym != null) ListaFalsoPAthSymbol.Add(ladoFG_pathSym);
            if (ladoGH_pathSym != null) ListaFalsoPAthSymbol.Add(ladoGH_pathSym);
            if (ladoHI_pathSym != null) ListaFalsoPAthSymbol.Add(ladoHI_pathSym);

            if (ladoIJ_pathSym != null) ListaFalsoPAthSymbol.Add(ladoIJ_pathSym);
            if (ladoJK_pathSym != null) ListaFalsoPAthSymbol.Add(ladoJK_pathSym);
            if (ladoKL_pathSym != null) ListaFalsoPAthSymbol.Add(ladoKL_pathSym);
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
            M1_ConfigurarDatosIniciales();
            // if (M3_DibujarBarraCurve() != Result.Succeeded) return;
            // if (_rebar == null)
            //{
            //   Util.ErrorMsg("Error al crear rebar. Rebar igual null");
            //   return;
            // }

            //M3A_1_CopiarParametrosCOmpartidos();
            //parametros no son correctos
            M4_ConfigurarAsignarParametrosRebarshape();
            //M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing();
            // M6_visualizar();
            M8_CrearPatSymbolFalso();
            M9_CreaTAg();
            M4_CrearTExto(_ptoTexto, _textoBArras, angleRADNormalHostYEJeZ);

            //M10_CreaDimension();
            // M11_CreaCirculo();
            // M11_CrearGrupo();
            //  M12_MOverHaciaBajo();

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
            //rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + _rebarInferiorDTO.diametroMM, _doc, true);

            //view3D = TiposFamilia3D.Get3DBuscar(_doc);
            //if (view3D == null)
            //{
            //  //  Util.ErrorMsg("Error, favor cargar configuracion inicial");
            //    return;
            //}
            //view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
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
                Util.ErrorMsg($"Error: Barra creada de largo cero.");
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

        //public void M3A_1_CopiarParametrosCOmpartidos()
        //{
        //    barraParametrosCompartidos.CopiarParametrosCompartidos(_rebar, _BarraTipo, _Prefijo_F, TipoDireccionBarra_, _rebarInferiorDTO.ubicacionLosa);
        //}

        public void M4_ConfigurarAsignarParametrosRebarshape()
        {

#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
            Result result = Result.Failed;
#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crar parametros compartidos");
                    foreach (var item in listaPArametroSharenh)
                    {
                        if (item.Isok == false) continue;

                        ParameterUtil.SetParaStringNH(_rebar, item.NombrePAra, item.valor);
                    }

                    ParameterUtil.SetParaDoubleNH(_rebar, "LargoSumaParciales", LargoTotalSumaParcialesFoot);

                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
                TaskDialog.Show("Error DibujarBarraRebarShape", msj);
            }

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


        public void M8_CrearPatSymbolFalso()
        {
            try
            {


                if (ListaFalsoPAthSymbol.Count == 0) return;

                // M8_1_ObtenerLineStyle_Barra();


                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("Creata PatSymbolFalso-NH");
                    foreach (Curve item in ListaFalsoPAthSymbol)
                    {
                        if (item is Line)
                        {
                            DetailLine lineafalsa = _doc.Create.NewDetailCurve(_view, item) as DetailLine;
                            lineafalsa.LineStyle = line_styles_srv;
                            if (lineafalsa != null) listaGrupo_LineaRebar.Add(lineafalsa.Id);
                        }
                        else if (item is Arc)
                        {
                            DetailArc lineafalsa = _doc.Create.NewDetailCurve(_view, item) as DetailArc;
                            lineafalsa.LineStyle = line_styles_srv;// line_styles_BARRA;
                            if (lineafalsa != null) listaGrupo_LineaRebar.Add(lineafalsa.Id);
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear PatSymbol Falso :{ex.Message}");
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
            if (listaGrupo_LineaRebar.Count < 1 || listaGrupo_Tag.Count < 2) return;

            if (listaGrupo_DimensionCirculo.Count > 0)
                listaGrupo_LineaRebar.AddRange(listaGrupo_DimensionCirculo);

            try
            {
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("Creando grupo-NH");

                    if (listaGrupo_LineaRebar.Count > 1)
                        _doc.Create.NewGroup(listaGrupo_LineaRebar);

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

        public bool M4_CrearTExto(XYZ ptoInserccion, string texto, double anguloRad)
        {

            try
            {
                if (texto == null) return false;
                if (texto == "") return false;
                CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, FActoryTipoTextNote.AcotarBarra, TipoCOloresTexto.rojo);

                _CrearTexNote.M1_CrearConTrans(ptoInserccion, texto, anguloRad);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear 'CrearTExto' ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
