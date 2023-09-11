using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;

using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Varios;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo
{
    public class BarraRefuerzoEstribo : ARebarRefuerzo
    {
        private double recubrimiento;
        private TipoEstriboRefuerzoLosa _candidadEstribo;

        #region 0)propiedades

        public Curve Curva1 { get; set; }
        public Curve Curva2 { get; set; }
        public double LargoCurvaRuta { get; set; }
        public TipoBorde TipoBorde { get; set; }



        // ancho en el plano xy se autliza para diseñar automaticamnte
        public double espesorEstribo { get; set; }

        private double anchoEstribo;

        private TipoRebar _BarraTipo;

        public double espaciamientoEntreEstribo_foot { get; set; }


        #endregion

        #region 1) Constructores
        public BarraRefuerzoEstribo(WrapperBoundarySegment boundarySegmentNH,
                                    TipoBorde TipoBorde,
                                    Floor floor,
                                    double espesorEstribo, double espaciamientoEntreEstribo_Foot, int diamtroBarra, TipoEstriboRefuerzoLosa cantidadEstribo, TipoRebar _TipoRebar) : base(boundarySegmentNH._uiapp)
        {
            this.Curva1 = boundarySegmentNH.boundarySegment.GetCurve();
            this.Curva2 = null;
            this.espesorEstribo = espesorEstribo;
            this.anchoEstribo = 1.6;
            this.espaciamientoEntreEstribo_foot = espaciamientoEntreEstribo_Foot;
            this.diamtroBarraMM = diamtroBarra;
            this.TipoBorde = TipoBorde;
            this.ElementoHost = floor;
            this.origen_forCreateFromRebarShape = boundarySegmentNH.coordenadasBorde.StartPoint;
            this._candidadEstribo = cantidadEstribo;
            this._doc = boundarySegmentNH.doc;
            this._BarraTipo = _TipoRebar;
        }



        public BarraRefuerzoEstribo(UIApplication uiapp, TipoBorde TipoBorde, Element floor, EstriboRefuerzoDTO er, IGeometriaTag _iGeometriaTagRefuerzo, TipoRebar _TipoRebar) : base(uiapp, _iGeometriaTagRefuerzo)
        {
            this.Curva1 = er.Curve;
            this.Curva2 = null;
            this.espesorEstribo = 0.5;
            this.anchoEstribo = er.AnchoFoot;
            this.espaciamientoEntreEstribo_foot = er.EspaciamientoEntreEstribo_Foot;
            this.diamtroBarraMM = er.DiamtroBarraEnMM;
            this.TipoBorde = TipoBorde;
            this.ElementoHost = floor;
            this.origen_forCreateFromRebarShape = er.Origen;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.recubrimiento = Util.CmToFoot(2);
            this._candidadEstribo = er.cantidadEstribo;
            this._BarraTipo = er.BarraTipo;
            this._BarraTipo = _TipoRebar;
        }


        #endregion

        #region 2) metodos
        public bool generarBarra()
        {
            try
            {
                //CONFIGURAR DATOS INICIALES
                ConfigurarDatosIniciales();

                //CONFIGURAR CURVA
                M2_ConfigurarBarraCurve();

                //OBTENER Y CONFIGURAR REBARSHAPE
                if (!ConfigurarRebarShape()) return false;

                // ConfigurarParametrosRebarshapeANTESDibujuhar();
                //DIBUJAR
                M1_2_DibujarBarraRebarShape();

                //configura espaciemientore
                ConfiguraEspaciamiento();
                //cambiar dimensiones
                ConfigurarAsignarParametrosRebarshape();
                // visualizar en 3d
                M1_6_visualizar();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public bool generarBarra_Sintrans()
        {
            try
            {


                //CONFIGURAR DATOS INICIALES
                ConfigurarDatosIniciales();

                //CONFIGURAR CURVA
                M2_ConfigurarBarraCurve();

                //OBTENER Y CONFIGURAR REBARSHAPE
                if (!ConfigurarRebarShape()) return false;

                // ConfigurarParametrosRebarshapeANTESDibujuhar();
                //DIBUJAR
                M1_2_DibujarBarraRebarShape_sinTrans();

                //configura espaciemientore
                ConfiguraEspaciamiento_SinTrans();
                //cambiar dimensiones
                // ConfigurarAsignarParametrosRebarshape_Sintrans();
                // visualizar en 3d
                //M1_6_visualizar();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        private void ConfigurarDatosIniciales()
        {
            //vista actual
            viewActual = _doc.ActiveView;
            double aux_espesorLosa = 0.3;
            double.TryParse(ParameterUtil.FindParaByBuiltInParameter(ElementoHost, BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM, _doc), out aux_espesorLosa);
            espesorLosa = aux_espesorLosa;
            if (espesorLosa == 0)
            {
                VerificarEspesor();

            }

            if (view3D_Visualizar == null)
                view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            if (view3D_Visualizar == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return;
            }

        }
        private void VerificarEspesor()
        {
            if (espesorLosa == 0 && (ElementoHost is Floor))
                espesorLosa = ((Floor)ElementoHost).ObtenerEspesorConPtosFloor((Curva1.GetEndPoint(1) + Curva1.GetEndPoint(0)) / 2, false);// ObtenerEspesorLosaVariable.ObtenerEspesorConPtos((Floor)ElementoHost, (Curva1.GetEndPoint(1)+ Curva1.GetEndPoint(0))/2);
        
                
        }
        public override void M2_ConfigurarBarraCurve()

        {
            LargoCurvaRuta = Curva1.Length;
            yvec = new XYZ(0, 0, -1);
            xvec = yvec.CrossProduct(((Line)Curva1).Direction);

        }

        private bool ConfigurarRebarShape()
        {
            try
            {

                rebarStyle = RebarStyle.Standard;
                rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diamtroBarraMM, _doc, true);
                if (rebarBarType == null)
                {
                    Util.ErrorMsg($"Error al  obterner Barras tipo Ø{diamtroBarraMM} de estribo");
                    return false;
                }
                // rebar shape
                rebarShape = TiposFormasRebarShape.getRebarShape("M_T1", _doc);
                if (rebarShape == null)
                {
                    Util.ErrorMsg("Error al  obterner rebarShape de estribo");
                    return false;
                }
                //ConfigurarAsignarParametrosRebarshapev2();
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        public Result ConfiguraEspaciamiento()
        {
            Result result = Result.Failed;

            try
            {
                using (Transaction t = new Transaction(_doc))
                {

                    t.Start("ConfiguraEspaciamiento barra refuerzo viga-NH");
                    RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                    // rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espesorLosa - Util.CmToFoot(4), 1, true, true, true);

                    int numerobarras = (int)(LargoCurvaRuta / espaciamientoEntreEstribo_foot) + 1;
                    XYZ aux_xvect = xvec * 0.5;//*espesorEstribo;  0.5 es para que en rebarshape inicial no que fuera y generen mensaje advertencia
                    XYZ aux_yvect = yvec * espesorLosa;
                    rebarShapeDrivenAccessor.ScaleToBox(origen_forCreateFromRebarShape, aux_xvect, aux_yvect);

                    rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientoEntreEstribo_foot, true, true, true);
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
        public Result ConfiguraEspaciamiento_SinTrans()
        {
            Result result = Result.Failed;

            try
            {
                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                // rebarShapeDrivenAccessor.SetLayoutAsMaximumSpacing(espesorLosa - Util.CmToFoot(4), 1, true, true, true);

                int numerobarras = (int)(LargoCurvaRuta / espaciamientoEntreEstribo_foot) + 1;
                XYZ aux_xvect = xvec * 0.5;//*espesorEstribo;  0.5 es para que en rebarshape inicial no que fuera y generen mensaje advertencia
                XYZ aux_yvect = yvec * espesorLosa;
                rebarShapeDrivenAccessor.ScaleToBox(origen_forCreateFromRebarShape, aux_xvect, aux_yvect);

                rebarShapeDrivenAccessor.SetLayoutAsNumberWithSpacing(numerobarras, espaciamientoEntreEstribo_foot, true, true, true);
                result = Result.Succeeded;

            }
            catch (Exception ex)
            {
                string msj = ex.Message;

                result = Result.Cancelled;
            }
            return result;
        }
        //redimensiona el rebarshap
        private void ConfigurarAsignarParametrosRebarshape()
        {
            // double anchoEstriboCm = Util.CmToFoot(40);
            double espesorRealEstriboCm = espesorLosa - recubrimiento * 2;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("modificar parametros rebarshape-NH");
                    if (viewActual != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null) ParameterUtil.SetParaInt(_rebar, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"
                    if (ParameterUtil.FindParaByName(_rebar, "B") != null) ParameterUtil.SetParaInt(_rebar, "B", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "C") != null) ParameterUtil.SetParaInt(_rebar, "C", espesorRealEstriboCm);
                    if (ParameterUtil.FindParaByName(_rebar, "D") != null) ParameterUtil.SetParaInt(_rebar, "D", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "E") != null) ParameterUtil.SetParaInt(_rebar, "E", espesorRealEstriboCm);

                    if (ParameterUtil.FindParaByName(_rebar, "B_") != null) ParameterUtil.SetParaInt(_rebar, "B_", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "C_") != null) ParameterUtil.SetParaInt(_rebar, "C_", espesorRealEstriboCm);
                    if (ParameterUtil.FindParaByName(_rebar, "D_") != null) ParameterUtil.SetParaInt(_rebar, "D_", anchoEstribo);
                    if (ParameterUtil.FindParaByName(_rebar, "E_") != null) ParameterUtil.SetParaInt(_rebar, "E_", espesorRealEstriboCm);


                    if (ParameterUtil.FindParaByName(_rebar, "CantidadEstriboCONF") != null)
                        ParameterUtil.SetParaInt(_rebar, $"CantidadEstriboCONF", CambiarTipo());

                    if (ParameterUtil.FindParaByName(_rebar, "CuantiaRefuerzo") != null)
                        ParameterUtil.SetParaInt(_rebar, "CuantiaRefuerzo", tipoEstribo.ToString() + "Ø" + diamtroBarraMM + "a" + Util.FootToCm(espaciamientoEntreEstribo_foot));
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }

        public void ConfigurarAsignarParametrosRebarshape_Sintrans()
        {
            // double anchoEstriboCm = Util.CmToFoot(40);
            double espesorRealEstriboCm = espesorLosa - recubrimiento * 2;
            try
            {
                if (ParameterUtil.FindParaByName(_rebar, "BarraTipo") != null && _BarraTipo != TipoRebar.NONE)
                    ParameterUtil.SetParaInt(_rebar, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo));

                if (viewActual != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null) ParameterUtil.SetParaInt(_rebar, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"
                if (ParameterUtil.FindParaByName(_rebar, "B") != null) ParameterUtil.SetParaInt(_rebar, "B", anchoEstribo);
                if (ParameterUtil.FindParaByName(_rebar, "C") != null) ParameterUtil.SetParaInt(_rebar, "C", espesorRealEstriboCm);
                if (ParameterUtil.FindParaByName(_rebar, "D") != null) ParameterUtil.SetParaInt(_rebar, "D", anchoEstribo);
                if (ParameterUtil.FindParaByName(_rebar, "E") != null) ParameterUtil.SetParaInt(_rebar, "E", espesorRealEstriboCm);

                if (ParameterUtil.FindParaByName(_rebar, "B_") != null) ParameterUtil.SetParaInt(_rebar, "B_", anchoEstribo);
                if (ParameterUtil.FindParaByName(_rebar, "C_") != null) ParameterUtil.SetParaInt(_rebar, "C_", espesorRealEstriboCm);
                if (ParameterUtil.FindParaByName(_rebar, "D_") != null) ParameterUtil.SetParaInt(_rebar, "D_", anchoEstribo);
                if (ParameterUtil.FindParaByName(_rebar, "E_") != null) ParameterUtil.SetParaInt(_rebar, "E_", espesorRealEstriboCm);


                if (ParameterUtil.FindParaByName(_rebar, "CantidadEstriboCONF") != null)
                    ParameterUtil.SetParaInt(_rebar, $"CantidadEstriboCONF", CambiarTipo());

                if (ParameterUtil.FindParaByName(_rebar, "CuantiaRefuerzo") != null)
                    ParameterUtil.SetParaInt(_rebar, "CuantiaRefuerzo", tipoEstribo.ToString() + "Ø" + diamtroBarraMM + "a" + Util.FootToCm(espaciamientoEntreEstribo_foot));

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }
        private string CambiarTipo()
        {
            switch (_candidadEstribo)
            {
                case TipoEstriboRefuerzoLosa.E:
                    return "E.";
                case TipoEstriboRefuerzoLosa.ED:
                    return "E.D.";
                case TipoEstriboRefuerzoLosa.ET:
                    return "E.T.";
                case TipoEstriboRefuerzoLosa.CT:
                    return "E.C.";
                default:
                    return "E.";
            }
        }
        #endregion
    }
}
