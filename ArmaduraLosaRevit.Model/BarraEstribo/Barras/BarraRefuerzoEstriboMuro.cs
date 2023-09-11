
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using rmaduraLosaRevit.Model.BarraV.TipoBarra;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Barras
{
    public class BarraRefuerzoEstriboMuro : ARebarRefuerzo
    {
        private double recubrimiento;
        private string _candidadEstribo;

        #region 0)propiedades
        public double LargoCurvaRutaFoor { get; set; }
        public double espesorRealEstriboFoot { get; set; }
        private double anchoEstribo;
        private string _nombreREbarshape;
        private XYZ yvecLat;
        private XYZ xvecLat;
        private double espaciamientoFoot;
        private double recorridoBrrar;
        private double _largoCurva;
        private double espesorMuroVigaFoot;

        private readonly GenerarDatosIniciales_Service _generarDatosIniciales_Service;
        private readonly EstriboMuroDTO _estriboMuroDTO;
        public TipoRebar BarraTipo { get; set; } = TipoRebar.NONE;
        public double espaciamientoEntreEstriboFoot { get; set; }
        #endregion

        #region 1) Constructores
        public BarraRefuerzoEstriboMuro(UIApplication uiapp, View view, GenerarDatosIniciales_Service generarDatosIniciales_Service, EstriboMuroDTO er, IGeometriaTag _newGeometriaTag) : base( uiapp, _newGeometriaTag)
        {
            this._uiapp = uiapp;
            this._generarDatosIniciales_Service = generarDatosIniciales_Service;
            this._estriboMuroDTO = er;
            this._newGeometriaTag = _newGeometriaTag;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.anchoEstribo = er.AnchoVisibleFoot;
            this.espaciamientoEntreEstriboFoot = er.EspaciamientoEntreEstriboFoot;
            this.diamtroBarraMM = er.DiamtroBarraEnMM;
            this.espesorMuroVigaFoot = er.Espesor_ElementHostFoot;
            this.ElementoHost = er.ElementHost;
            this.origen_forCreateFromRebarShape = er.OrigenEstribo;
            this.recubrimiento = ConstNH.RECUBRIMIENTO_MURO_CM;
            this._candidadEstribo = er.cantidadEstribo;
            view3D_Visualizar = generarDatosIniciales_Service._view3D_VISUALIZAR;
            //vista actual
            viewActual = view;

            LargoCurvaRutaFoor = er.largoRecorridoEstriboFoot;
            yvec = er.direccionPerpenEntradoHaciaViewSecction;
            xvec = er.direccionParalelaViewSecctioin;

            xvecLat = er.direccionPerpenEntradoHaciaViewSecction;
            yvecLat = er.direccionParalelaViewSecctioin;
            espesorRealEstriboFoot = er._anchoEstribo1Foot;
            _nombreREbarshape = "M_T1";

        }
        #endregion

        #region 2) metodos
        public void M1_GenerarBarra()
        {
            //CONFIGURAR DATOS INICIALES
            //  ConfigurarDatosIniciales();

            //CONFIGURAR CURVA
            //  M2_ConfigurarBarraCurve();

            //OBTENER Y CONFIGURAR REBARSHAPE
            M1_1_ConfigurarRebarShape();
            M1_Configurar_cambiarTipoBarraEStribo();
            // ConfigurarParametrosRebarshapeANTESDibujuhar();
            //DIBUJAR
            M1_2_DibujarBarraRebarShape();

            //configura espaciemientore
            M1_3_ConfiguraEspaciamientoEstribo();
            //cambiar dimensiones
            M1_4_ConfigurarAsignarParametrosRebarshape();

            // visualizar en 3d
            M1_6_visualizar();

        }
        public void M1_GenerarBarra_2parte()
        {


            //configura espaciemientore
            M1_3_ConfiguraEspaciamientoEstribo();
            //cambiar dimensiones
            M1_4_ConfigurarAsignarParametrosRebarshape();

            // visualizar en 3d
            //     M1_6_visualizar();

        }
        public List<ElementId> GenerarLaterales()
        {
            List<ElementId> listaIdLaterales = new List<ElementId>();
            if (_estriboMuroDTO.ListaLateralesDTO == null) return listaIdLaterales;
            foreach (BarraLateralesDTO item in _estriboMuroDTO.ListaLateralesDTO)
            {
                M1_1_AsignarEspaciamientoYrecorrido();
                M2_ConfigurarLateralCurve(item._diamtroLat);
                M0_CalcularCurva(item);
                M3_DibujarBarraCurve_ConTrans();
                M1_5__ConfiguraEspaciamientoLaterales();
                //  M1_3_ConfiguraEspaciamientoLaterales();
                M4_ConfigurarAsignarParametrosRebarLaterales(item);
                M1_6_visualizar();
                listaIdLaterales.Add(_rebar.Id);
            }

            return listaIdLaterales;
        }

        //en desarrollo
        public List<ElementId> GenerarTrabas()
        {
            List<ElementId> listaIdTrabas = new List<ElementId>();

            if (_estriboMuroDTO.ListaTrabasDTO == null) return listaIdTrabas;
            foreach (BarraTrabaDTO item in _estriboMuroDTO.ListaTrabasDTO)
            {
                M2_ConfigurarTrabaCurve(item);
                //M1_Configurar_cambiarTipoBarraEStribo();
                M0_CalcularCurvaTraba(item);

                M3_DibujarBarraCurve_ConTrans();
                M1_3_ConfiguraEspaciamientoTraba();
                M4_ConfigurarAsignarParametrosRebarTraba(item);
                M1_6_visualizar();
                listaIdTrabas.Add(_rebar.Id);
            }

            return listaIdTrabas;
        }


        private void M0_CalcularCurvaTraba(BarraTrabaDTO item)
        {

            listcurve = new System.Collections.Generic.List<Curve>();
            Curve ladoCentral = Line.CreateBound(item._startPont_, item._endPoint);
            _largoCurva = Math.Round(ladoCentral.Length, 2);
            listcurve.Add(ladoCentral);
        }

        public override void M2_ConfigurarBarraCurve()
        {
            GenerarLaterales();
        }
        public bool M2_ConfigurarLateralCurve(int diametro)
        {

            startHookOrient = RebarHookOrientation.Left; //defecto
            endHookOrient = RebarHookOrientation.Left; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = true;

            rebarStyle = RebarStyle.Standard;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametro, _doc, true);
            norm = _estriboMuroDTO.direccionPerpenEntradoHaciaViewSecction;


            if (rebarBarType == null)
            {
                Util.ErrorMsg($"Error al obtener el tipo { "Ø " + diametro}");
                return false;
            }
            return true;
        }
        private void M1_1_AsignarEspaciamientoYrecorrido()
        {
            int _nuevaLineaCantidadbarra = 2;
            if (_nuevaLineaCantidadbarra == 1)
            {
                throw new NotImplementedException("Solo Una linea de barra.Caso no implementado");
            }
            else
            {

                this.espaciamientoFoot = (this.espesorMuroVigaFoot
                                        - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) * 2.0
                                        - Util.MmToFoot(diamtroBarraMM)) / (_nuevaLineaCantidadbarra - 1);
                this.recorridoBrrar = (this.espesorMuroVigaFoot
                                        - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM) * 2.0
                                        - Util.MmToFoot(diamtroBarraMM));
            }
        }
        public bool M2_ConfigurarTrabaCurve(BarraTrabaDTO item)
        {

            startHookOrient = item._ubicacionHook; //defecto
            endHookOrient = item._ubicacionHook; //defecto        
            useExistingShapeIfPossible = true;
            createNewShape = true;

            EndHook = TipoRebarHookType.ObtenerHook("Stirrup/Tie - 135 degNH", _doc); ;
            StartHook = TipoRebarHookType.ObtenerHook("Stirrup/Tie - 135 degNH", _doc); ;

            rebarStyle = RebarStyle.StirrupTie;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + item._diamtroTraba, _doc, true);
            norm = new XYZ(0, 0, 1);


            if (rebarBarType == null)
            {
                Util.ErrorMsg($"Error al obtener el tipo { "Ø " + item._diamtroTraba}");
                return false;
            }
            return true;
        }
        public void M0_CalcularCurva(BarraLateralesDTO item)
        {
            //  listcurve.Clear();
            listcurve = new System.Collections.Generic.List<Curve>();
            Curve ladoCentral = Line.CreateBound(item._startPont_, item._endPoint);
            _largoCurva = Math.Round(ladoCentral.Length, 2);
            listcurve.Add(ladoCentral);

        }
        private void M1_1_ConfigurarRebarShape()
        {
            rebarStyle = RebarStyle.Standard;
            rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diamtroBarraMM, _doc, true);
            // rebar shape
            rebarShape = TiposFormasRebarShape.getRebarShape(_nombreREbarshape, _doc);
            //ConfigurarAsignarParametrosRebarshapev2();

        }
        public Result M1_3_ConfiguraEspaciamientoEstribo()
        {
            Result result = Result.Failed;

            try
            {

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ConfiguraEspaciamiento estribo confinamiento-NH");

                    Element paraElem = _doc.GetElement(_rebar.Id);
                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    // rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espesorLosa - Util.CmToFoot(4), 1, true, true, true);

                    int numerobarras = (int)(LargoCurvaRutaFoor / espaciamientoEntreEstriboFoot) + 1;
                    XYZ aux_xvect = xvec * 0.5;//*espesorEstribo;  0.5 es para que en rebarshape inicial no que fuera y generen mensaje advertencia
                    XYZ aux_yvect = yvec * espesorRealEstriboFoot;
                    rebarShapeDrivenAccessor.ScaleToBox(origen_forCreateFromRebarShape, aux_xvect, aux_yvect);

                    rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientoEntreEstriboFoot, true, true, true);
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

        public Result M1_3_ConfiguraEspaciamientoTraba()
        {
            Result result = Result.Failed;

            //solucion parche
            double espaciamientolateralFoot = Util.CmToFoot(_generarDatosIniciales_Service._configuracionInicialEstriboDTO.espaciamientoTrabaCM);
            try
            {

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ConfiguraEspaciamiento traba confinamiento-NH");

                    Element paraElem = _doc.GetElement(_rebar.Id);
                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    int numerobarras = (int)(LargoCurvaRutaFoor / espaciamientolateralFoot) + 1;
                    rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientolateralFoot, true, true, true);
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



        public Result M1_5__ConfiguraEspaciamientoLaterales()
        {


            Result result = Result.Failed;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ConfiguraEspaciamiento barra refuerzo-NH");
                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espaciamientoFoot, recorridoBrrar, true, true, true);
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

        public Result M1_3_ConfiguraEspaciamientoLaterales()
        {
            int _nuevaLineaCantidadbarra = 2;
            this.espaciamientoFoot = (this.espesorRealEstriboFoot - Util.CmToFoot((ConstNH.RECUBRIMIENTO_MURO_CM + (diamtroBarraMM) / 10)) * 2.0) / (_nuevaLineaCantidadbarra - 1);
            this.recorridoBrrar = (this.espesorRealEstriboFoot - Util.CmToFoot((ConstNH.RECUBRIMIENTO_MURO_CM + (diamtroBarraMM) / 10)) * 2.0);

            Result result = Result.Failed;

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("ConfiguraEspaciamiento estribo confinamiento-NH");

                    //  Element paraElem = _doc.GetElement(_rebar.Id);
                    //   ParameterUtil.SetParaInt(paraElem, $"CantidadEstriboCONF", _candidadEstribo);
                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    // rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espesorLosa - Util.CmToFoot(4), 1, true, true, true);

                    int numerobarras = 2;
                    rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientoFoot, true, true, true);
                    //   rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espesorRealEstriboFoot, true, true, true);
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


        private void M1_4_ConfigurarAsignarParametrosRebarshape()
        {
            try
            {

                using (Transaction t = new Transaction(_doc))
                {

                    t.Start("modificar parametros rebarshape-NH");


                    if (viewActual != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null)
                        ParameterUtil.SetParaInt(_rebar, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"

                    if (ParameterUtil.FindParaByName(_rebar, "BarraTipo") != null && BarraTipo != TipoRebar.NONE)
                        ParameterUtil.SetParaInt(_rebar, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(BarraTipo));  //"nombre de vista"

                    if (ParameterUtil.FindParaByName(_rebar, "B") != null) ParameterUtil.SetParaInt(_rebar, "B", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "C") != null) ParameterUtil.SetParaInt(_rebar, "C", espesorRealEstriboFoot);
                    if (ParameterUtil.FindParaByName(_rebar, "D") != null) ParameterUtil.SetParaInt(_rebar, "D", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "E") != null) ParameterUtil.SetParaInt(_rebar, "E", espesorRealEstriboFoot);

                    if (ParameterUtil.FindParaByName(_rebar, "B_") != null) ParameterUtil.SetParaInt(_rebar, "B_", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "C_") != null) ParameterUtil.SetParaInt(_rebar, "C_", espesorRealEstriboFoot);
                    if (ParameterUtil.FindParaByName(_rebar, "D_") != null) ParameterUtil.SetParaInt(_rebar, "D_", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "E_") != null) ParameterUtil.SetParaInt(_rebar, "E_", espesorRealEstriboFoot);

                    if (ParameterUtil.FindParaByName(_rebar, "CantidadEstriboCONF") != null)
                        ParameterUtil.SetParaInt(_rebar, $"CantidadEstriboCONF", _candidadEstribo);


                    if (ParameterUtil.FindParaByName(_rebar, "CantidadEstriboLAT") != null)
                        ParameterUtil.SetParaInt(_rebar, "CantidadEstriboLAT", tipoEstribo + "Ø" + diamtroBarraMM + "a" + espaciamientoEntreEstriboFoot);


                    if (ParameterUtil.FindParaByName(_rebar, "CantidadEstriboTRABA") != null && _estriboMuroDTO.TextoAUXTraba != "")
                        ParameterUtil.SetParaInt(_rebar, "CantidadEstriboTRABA", _estriboMuroDTO.TextoAUXTraba);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

        }



        private void M4_ConfigurarAsignarParametrosRebarLaterales(BarraLateralesDTO item)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("modificar parametros LATERALES-NH");

                    if (ParameterUtil.FindParaByName(_rebar, "B") != null) ParameterUtil.SetParaInt(_rebar, "B", _largoCurva);

                    if (viewActual != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null)
                        ParameterUtil.SetParaInt(_rebar, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"
                   
                    if (ParameterUtil.FindParaByName(_rebar, "BarraTipo") != null && BarraTipo != TipoRebar.NONE)
                        ParameterUtil.SetParaInt(_rebar, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(BarraTipo));  //"nombre de vista"

                    if (ParameterUtil.FindParaByName(_rebar, "CantidadEstriboLAT") != null)
                        ParameterUtil.SetParaInt(_rebar, "CantidadEstriboLAT", item._textoLat);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

        }



        private void M4_ConfigurarAsignarParametrosRebarTraba(BarraTrabaDTO item)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("modificar parametros TRABA-NH");

                    if (viewActual != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null)
                        ParameterUtil.SetParaInt(_rebar, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"
                  
                    if (ParameterUtil.FindParaByName(_rebar, "BarraTipo") != null && BarraTipo != TipoRebar.NONE)
                        ParameterUtil.SetParaInt(_rebar, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(BarraTipo));  //"nombre de vista"

                    //if (ParameterUtil.FindParaByName(_rebar, "B") != null) ParameterUtil.SetParaInt(_rebar, "B", _largoCurva);
                    if (ParameterUtil.FindParaByName(_rebar, "CantidadEstriboTRABA") != null && item._textoTraba != "")
                        ParameterUtil.SetParaInt(_rebar, "CantidadEstriboTRABA", item._textoTraba);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

        }

        #endregion

    }
}
