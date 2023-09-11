using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Varios;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo
{
    public abstract class ARebarRefuerzo
    {

        protected IGeometriaTag _newGeometriaTag;
        protected UIApplication _uiapp;

        public Document _doc { get; set; }
        public Rebar _rebar { get; set; }

        public View3D view3D_Visualizar { get; set; }
        public View viewActual { get; set; }

        #region 0)propiedades

        //curva de barra referencia
        public List<Curve> listcurve { get; set; }
        public XYZ ptoInicial { get; set; }
        public XYZ xvec { get; set; }
        public XYZ yvec { get; set; }
        public XYZ origen_forCreateFromRebarShape { get; set; }
        public XYZ norm { get; set; }
        public string largo_Cm { get; set; }
        public string largoParciales_CM { get; set; }


        public RebarHookType StartHook { get; set; }
        public RebarHookType EndHook { get; set; }

        public RebarShape rebarShape { get; set; }
        public RebarStyle rebarStyle { get; set; }
        public RebarBarType rebarBarType { get; set; }
        public Element ElementoHost { get; set; }
        public RebarHookOrientation startHookOrient { get; set; }
        public RebarHookOrientation endHookOrient { get; set; }
        public bool useExistingShapeIfPossible { get; set; }
        public bool createNewShape { get; set; }

        public int numeroBarra { get; set; }
        public int diamtroBarraMM { get; set; }
        public int tipoEstribo { get; set; }

        protected int espesorLosaCM;
        // private BoundarySegmentNH boundarySegmentNH;
        public double espesorLosa { get; set; }
        public TipoCOlores tipoColor { get; set; }

        public bool IsOK { get; set; }

        #endregion


        public ARebarRefuerzo(UIApplication _uiapp, IGeometriaTag _newGeometriaTag = null)
        {
            this._newGeometriaTag = _newGeometriaTag;
            this._uiapp = _uiapp;
            this.EndHook = null;
            this.StartHook = null;
            this.IsOK = true;
        }

        #region 2) metodos

        protected void M1_ConfigurarDatosIniciales()
        {
            startHookOrient = RebarHookOrientation.Right; //defecto
            endHookOrient = RebarHookOrientation.Right; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = false;
            listcurve = new List<Curve>();


            double aux_espesorLosa = 0.3;
            double.TryParse(ParameterUtil.FindParaByBuiltInParameter(ElementoHost, BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM, _doc), out aux_espesorLosa);
            espesorLosa = aux_espesorLosa;
            if (espesorLosa == 0)
            {

            }
            espesorLosaCM = (int)Util.FootToCm(espesorLosa);


            rebarStyle = RebarStyle.Standard;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diamtroBarraMM, _doc, true);

            if (view3D_Visualizar == null)
                view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            if (view3D_Visualizar == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return;
            }
            //vista actual
            viewActual = _doc.ActiveView;
        }
        protected void M1_Configurar_cambiarTipoBarraEStribo()
        {
            rebarStyle = RebarStyle.StirrupTie;
        }

        public abstract void M2_ConfigurarBarraCurve();

        public Result M3_DibujarBarraCurve_ConTrans()
        {
            Result result = Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  nuevo refuerzo-NH");
                    _rebar = Rebar.CreateFromCurves(_doc,
                                               rebarStyle,
                                               rebarBarType,
                                               StartHook,
                                               EndHook,
                                               ElementoHost,
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
            return result;
        }

        public Result M3_DibujarBarraCurve_SinTrans()
        {
            Result result = Result.Failed;
            try
            {

                _rebar = Rebar.CreateFromCurves(_doc,
                                           rebarStyle,
                                           rebarBarType,
                                           StartHook,
                                           EndHook,
                                           ElementoHost,
                                           norm,
                                           listcurve,
                                           startHookOrient,
                                           endHookOrient,
                                           useExistingShapeIfPossible,
                                           createNewShape);


                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                IsOK = false;
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }

        public Result M1_2_DibujarBarraRebarShape()
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
                                               ElementoHost,
                                               origen_forCreateFromRebarShape,
                                               xvec,
                                               yvec);


                    t.Commit();

                }
                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                IsOK = false;
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }

        public Result M1_2_DibujarBarraRebarShape_sinTrans()
        {
            Result result = Result.Failed;
            try
            {
                _rebar = Rebar.CreateFromRebarShape(_doc,
                                           rebarShape,
                                           rebarBarType,
                                           ElementoHost,
                                           origen_forCreateFromRebarShape,
                                           xvec,
                                           yvec);

                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                IsOK = false;
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }

        public Result M1_6_visualizar()
        {
            Result result = Result.Failed;
            if (_rebar == null) return Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  visualizar-NH");
                    //CONFIGURACION DE PATHREINFORMENT
                    if (view3D_Visualizar != null)
                    {
                        //permite que la barra se vea en el 3d como solidD
                        _rebar.SetSolidInView(view3D_Visualizar, true);
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        _rebar.SetUnobscuredInView(view3D_Visualizar, true);
                    }
                    if (viewActual != null)
                    {
                        //permite que la barra se vea en el 3d como sin interferecnias 
                        _rebar.SetUnobscuredInView(viewActual, true);
                    }


                    VisibilidadElementRebarLosa visibilidadElement = new VisibilidadElementRebarLosa(_uiapp);
                    visibilidadElement.ChangeElementColorSinTrans(_rebar.Id, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta), false);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                IsOK = false;
                string msj = ex.Message;
                result = Result.Cancelled;
            }


            return result;
        }

        public Result M1_6_visualizar_sintrans()
        {
            Result result = Result.Failed;
            if (_rebar == null) return Result.Failed;
            if (!_rebar.IsValidObject) return Result.Failed;
            try
            {

                //CONFIGURACION DE PATHREINFORMENT
                if (view3D_Visualizar != null)
                {
                    //permite que la barra se vea en el 3d como solidD
                    _rebar.SetSolidInView(view3D_Visualizar, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    _rebar.SetUnobscuredInView(view3D_Visualizar, true);
                }
                if (viewActual != null)
                {
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    _rebar.SetUnobscuredInView(viewActual, true);
                }


                VisibilidadElementRebarLosa visibilidadElement = new VisibilidadElementRebarLosa(_uiapp);
                visibilidadElement.ChangeElementColorSinTrans(_rebar.Id, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta), false);
                result = Result.Succeeded;
            }
            catch (Exception ex)
            {
                IsOK = false;
                string msj = ex.Message;
                result = Result.Cancelled;
            }


            return result;
        }


        public Result M1_7_Color(List<ElementId> ListaElemenId)
        {
            Result result = Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  CambiarColor-NH");
                    //CONFIGURACION DE PATHREINFORMENT

                    VisibilidadElementOrientacion visibilidadElement = new VisibilidadElementOrientacion(_doc);
                    visibilidadElement.ChangeListaElementColorConTrans(ListaElemenId, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta));

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                IsOK = false;
                string msj = ex.Message;
                result = Result.Cancelled;
            }


            return result;
        }



        public Result M1_6_visualizarHalftone()
        {
            Result result = Result.Failed;
            OverrideGraphicSettings ORGS = new OverrideGraphicSettings();
            ORGS.SetHalftone(false);

            try
            {

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  visualizar Halftone-NH");
                    //CONFIGURACION DE PATHREINFORMENT

                    if (viewActual != null)
                    {
                        _doc.ActiveView.SetElementOverrides(_rebar.Id, ORGS);
                    }
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                IsOK = false;
                result = Result.Cancelled;
            }


            return result;
        }
        public bool DibujarTagsEstribo(ConfiguracionTAgEstriboDTo configuracionTAgEstriboDTo)
        {
            return DibujarTagsEstribo(configuracionTAgEstriboDTo, _rebar);

        }

        public bool DibujarTagRebarRefuerzoLosa(ConfiguracionTAgBarraDTo configuracionTAgBarraDTo)
        {
            return DibujarTagRebarRefuerzoLosa(configuracionTAgBarraDTo, _rebar);

        }
        public bool DibujarTagsEstribo(ConfiguracionTAgEstriboDTo configuracionTAgEstriboDTo, Element _rebarRef)
        {
            try
            {
                if (_newGeometriaTag == null)
                {
                    Util.ErrorMsg("Error IGeometriaTag==null ");
                    return false;
                }
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("creando tag-NH");
                    if (_newGeometriaTag.M4_IsFAmiliaValida())
                    {
                        foreach (TagBarra item in _newGeometriaTag.listaTag)
                        {
                            if (item == null) continue;
                            if (!item.IsOk) continue;
                            item.DibujarTagEstribo(_rebarRef, _doc, viewActual, configuracionTAgEstriboDTo, item.posicion);
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                IsOK = false;
                Util.ErrorMsg($"Error al Crear tag  ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool DibujarTagRebarRefuerzoLosa(ConfiguracionTAgBarraDTo configuracionTAgBarraDTo, Element _rebarRef)
        {
            try
            {
                if (_newGeometriaTag == null)
                {
                    Util.ErrorMsg("Error IGeometriaTag==null ");
                    return false;
                }
                using (Transaction tx = new Transaction(_doc))
                {

                    tx.Start("creando tag-NH");
                    if (_newGeometriaTag.M4_IsFAmiliaValida())
                    {
                        foreach (TagBarra item in _newGeometriaTag.listaTag)
                        {
                            if (item == null) continue;
                            if (!item.IsOk) continue;
                            item.DibujarTagRebarRefuerzoLosa(_rebarRef, _uiapp, viewActual, configuracionTAgBarraDTo);
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                IsOK = false;
                Util.ErrorMsg($"Error al Crear tag  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool DibujarTagRebarRefuerzoLosa_SinTrans(ConfiguracionTAgBarraDTo configuracionTAgBarraDTo)
        {
            return DibujarTagRebarRefuerzoLosa_SinTrans(configuracionTAgBarraDTo, _rebar);

        }
        public bool DibujarTagRebarRefuerzoLosa_SinTrans(ConfiguracionTAgBarraDTo configuracionTAgBarraDTo, Element _rebarRef)
        {
            try
            {
                if (_newGeometriaTag == null)
                {
                    Util.ErrorMsg("Error IGeometriaTag==null ");
                    return false;
                }
                if (_rebar == null) return false;
                if (_newGeometriaTag.M4_IsFAmiliaValida())
                {
                    foreach (TagBarra item in _newGeometriaTag.listaTag)
                    {
                        if (item == null) continue;
                        if (!item.IsOk) continue;
                        item.DibujarTagRebarRefuerzoLosa(_rebarRef, _uiapp, viewActual, configuracionTAgBarraDTo);
                    }
                }

            }
            catch (Exception ex)
            {
                IsOK = false;
                Util.ErrorMsg($"Error al Crear tag  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public Result M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing(double espaciamiento)
        {
            Result result = Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ConfiguraEspaciamiento barra refuerzo-NH");
                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espaciamiento, espaciamiento, true, true, true);
                    //este me genera que las barras se acorten a20 cm
                    //rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(2, espaciamiento, true, true, true);
                    result = Result.Succeeded;
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                IsOK = false;
                Util.ErrorMsg($"Error configurar espaciamiento  ex:{ex.Message}");
                result = Result.Cancelled;
            }

            return result;
        }


        public Result M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing(int NumeroBarras, double espaciamiento)
        {
            Result result = Result.Failed;
            try
            {
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(NumeroBarras, espaciamiento, true, true, true);
            }
            catch (Exception ex)
            {
                IsOK = false;
                Util.ErrorMsg($"Error configurar espaciamiento  ex:{ex.Message}");
                result = Result.Cancelled;
            }
            return result;
        }

        #endregion

    }
}
