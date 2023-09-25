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
using ArmaduraLosaRevit.Model;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Extension;

namespace rmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public abstract class ARebarsSinTrans
    {

        protected IGeometriaTag _newGeometriaTag;

        public Document _doc { get; set; }
        public Rebar EstriboRebarCreado { get; set; }

        public View3D view3D_Visualizar { get; set; }
        public View viewActual { get; set; }
        private UIApplication _uiapp;


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
        
        public TipoRebar BarraTipo { get; set; } = TipoRebar.NONE;
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
        private readonly UIApplication uiapp;

        // private BoundarySegmentNH boundarySegmentNH;
        public double espesorLosa { get; set; }

        public string pier { get; set; }
        public string story { get; set; }
        #endregion
        public ARebarsSinTrans(UIApplication uiapp)
        {
            EndHook = null;
            StartHook = null;
            this._uiapp = uiapp;
        }
        public ARebarsSinTrans(UIApplication uiapp,IGeometriaTag _newGeometriaTag)
        {
            this._uiapp = uiapp;
            this._newGeometriaTag = _newGeometriaTag;
            EndHook = null;
            StartHook = null;
        }

        #region 2) metodos

        protected void M1_ConfigurarDatosIniciales()
        {
            startHookOrient = RebarHookOrientation.Right; //defecto
            endHookOrient = RebarHookOrientation.Right; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = false;
            listcurve = new List<Curve>();


            double aux_espesorLosa = 0.0;
            double.TryParse(ParameterUtil.FindParaByBuiltInParameter(ElementoHost, BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM, _doc), out aux_espesorLosa);
            espesorLosa = aux_espesorLosa;
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

        public Result M3_DibujarBarraCurve()
        {
            Result result = Result.Failed;

            try
            {

                EstriboRebarCreado = Rebar.CreateFromCurves(_doc,
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
                EstriboRebarCreado = Rebar.CreateFromRebarShape(_doc,
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
                string msj = ex.Message;
                result = Result.Cancelled;
            }
            return result;
        }

        public Result M1_6_visualizar()
        {
            Result result = Result.Failed;
            if(EstriboRebarCreado==null) result = Result.Failed;
            try
            {

                //CONFIGURACION DE PATHREINFORMENT
                if (view3D_Visualizar != null)
                {
                    //permite que la barra se vea en el 3d como solidD
                    EstriboRebarCreado.SetSolidInView(view3D_Visualizar, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    EstriboRebarCreado.SetUnobscuredInView(view3D_Visualizar, true);
                }

                if (viewActual != null)
                {
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    EstriboRebarCreado.SetUnobscuredInView(viewActual, true);


                }

            }
            catch (Exception ex)
            {
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
                //CONFIGURACION DE PATHREINFORMENT
                if (viewActual != null)
                {


                    _doc.ActiveView.SetElementOverrides(EstriboRebarCreado.Id, ORGS);
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;

                result = Result.Cancelled;
            }
            return result;
        }
        public bool DibujarTagsEstribo(ConfiguracionTAgEstriboDTo configuracionTAgEstriboDTo)
        {
            return DibujarTagsEstribo(configuracionTAgEstriboDTo, EstriboRebarCreado);

        }

        public bool DibujarTagRebarRefuerzoLosa(ConfiguracionTAgBarraDTo configuracionTAgBarraDTo)
        {
            return DibujarTagRebarRefuerzoLosa(configuracionTAgBarraDTo, EstriboRebarCreado);

        }
        public bool DibujarTagsEstribo(ConfiguracionTAgEstriboDTo configuracionTAgEstriboDTo, Element _rebarRef)
        {      
            try
            {
                if (_newGeometriaTag.M4_IsFAmiliaValida())
                {
                    foreach (TagBarra item in _newGeometriaTag.listaTag)
                    {
                        if (item == null) continue;
                        if (!item.IsOk) continue;
                        if (item.nombre == "ESP")
                        {
                            Rebar _rebarHost = _rebarRef as Rebar;
                            if (_rebarHost == null) continue;
                            ElementId _elemtoIdMuro = _rebarHost.GetHostId();
                            if (_elemtoIdMuro == null) continue;
                            Wall _wall = _doc.GetElement(_elemtoIdMuro) as Wall;
                            if (_wall == null) continue;
                            IndependentTag independentTag = IndependentTag.Create(_doc, item.ElementIndependentTagPath.Id, viewActual.Id, new Reference(_wall), false,
                                                   TagOrientation.Vertical, XYZ.Zero); //new XYZ(0, 0, 0)
                            independentTag.TagHeadPosition = item.posicion;
                            item.IsOk = false;
                        }
                        else
                            item.DibujarTagEstribo(_rebarRef, _uiapp, viewActual, configuracionTAgEstriboDTo, item.posicion);
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear tag  ex:{ex.Message}"  );
                return false;               
            }
            return true;
        }


        public bool DibujarTagRebarRefuerzoLosa(ConfiguracionTAgBarraDTo configuracionTAgBarraDTo, Element _rebarRef)
        {
            try
            {

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
            catch (Exception)
            {

                Util.ErrorMsg("Error al Tag Rebar Refuerzo Losa");
                return false;
            }
            return true;
        }

        public void M4_ConfigurarAsignarParametrosRebarshape()
        {


            try
            {


                if (viewActual != null && ParameterUtil.FindParaByName(EstriboRebarCreado, "NombreVista") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "BarraTipo") != null && BarraTipo != TipoRebar.NONE)
                    ParameterUtil.SetParaInt(EstriboRebarCreado, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(BarraTipo));  //"nombre de vista"

                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "CuantiaRefuerzo") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "CuantiaRefuerzo", "F=F'=" + numeroBarra + "Ø" + diamtroBarraMM);
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "LargoParciales") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "LargoParciales", largoParciales_CM);//(30+100+30)
                if (ParameterUtil.FindParaByName(EstriboRebarCreado, "LargoTotal") != null) ParameterUtil.SetParaInt(EstriboRebarCreado, "LargoTotal", largo_Cm);//(30+100+30)

            }
            catch (Exception ex)

            {
                string msj = ex.Message;
                TaskDialog.Show("Error", msj);
            }



        }




        public Result M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing(double espaciamiento)
        {
            Result result = Result.Failed;

            try
            {
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = EstriboRebarCreado.GetShapeDrivenAccessor();
                rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espaciamiento, espaciamiento, true, true, true);
                //este me genera que las barras se acorten a20 cm
                //rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(2, espaciamiento, true, true, true);

                result = Result.Succeeded;

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                result = Result.Cancelled;
            }




            return result;
        }
        #endregion

    }
}
