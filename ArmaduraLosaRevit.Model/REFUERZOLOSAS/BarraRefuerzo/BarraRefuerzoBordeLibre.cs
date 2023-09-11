using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Varios;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo
{
    public class BarraRefuerzoBordeLibre : ARebarRefuerzo
    {
        #region 0)propiedades
        private CalculoBarraRefuerzo _calculoBarraRefuerzo;
        private readonly string textREfuerzoBarra;
        private double _largoPataGeneral;
        private TipoRebar _BarraTipo;

        // private BoundarySegmentNH boundarySegmentNH;
        public TipoBorde TipoBorde { get; set; }

        public double offSuperiorhaciaBajoLosa { get; set; }
        public double offInferiorHaciaArribaLosa { get; set; }

        public XYZ startPont_offset { get; set; }
        public XYZ EndPoint_offse { get; set; }

        public int diamtroBarraEstriboMM { get; set; }

        #endregion

        #region 1) Constructores


        public BarraRefuerzoBordeLibre(WrapperBoundarySegment boundarySegmentNH, TipoBorde TipoBorde, Opening selectedOpening, Floor floor, int diamtroBarra, int numeroBarra) : base(boundarySegmentNH._uiapp)
        {

            this.offSuperiorhaciaBajoLosa = boundarySegmentNH.coordenadasBorde.offSuperiorhaciaBajoLosa;
            this.offInferiorHaciaArribaLosa = boundarySegmentNH.coordenadasBorde.offInferiorHaciaArribaLosa;

            this.startPont_offset = boundarySegmentNH.coordenadasBorde.startPont_offsetIntRoom;
            this.EndPoint_offse = boundarySegmentNH.coordenadasBorde.EndPoint_offseIntRoom;
            this._uiapp = boundarySegmentNH._uiapp;
            this.TipoBorde = TipoBorde;
            //this.selectedOpening = selectedOpening;
            this.ElementoHost = floor;

            this._doc = boundarySegmentNH.doc;
            this.norm = new XYZ(0, 0, 1);
            this.diamtroBarraMM = diamtroBarra;
            this.diamtroBarraEstriboMM = 12;
            this.numeroBarra = numeroBarra;
            this._BarraTipo = TipoRebar.REFUERZO_BA;

        }

        public BarraRefuerzoBordeLibre(UIApplication _uiapp, DtoCrearBarraRefuerzoBordeLibre _dtoBarraRefBordeLibre) : base(_uiapp, _dtoBarraRefBordeLibre._iGeometriaTagRefuerzo)
        {

            this.offSuperiorhaciaBajoLosa = Util.CmToFoot(2);
            this.offInferiorHaciaArribaLosa = Util.CmToFoot(5);

            this.startPont_offset = _dtoBarraRefBordeLibre.br.ptoini_interseccion;
            this.EndPoint_offse = _dtoBarraRefBordeLibre.br.ptofinal_interseccion;

            this.TipoBorde = TipoBorde;
            this._calculoBarraRefuerzo = _dtoBarraRefBordeLibre.br;
            this.textREfuerzoBarra = _dtoBarraRefBordeLibre.textREfuerzoBarra;
            this.ElementoHost = _dtoBarraRefBordeLibre.floor;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.norm = (_dtoBarraRefBordeLibre.br._NormalCaraSuperiorLosa == null || _dtoBarraRefBordeLibre.br._NormalCaraSuperiorLosa.IsAlmostEqualTo(XYZ.Zero)
                ? new XYZ(0, 0, -1)
                : _dtoBarraRefBordeLibre.br._NormalCaraSuperiorLosa);
            this.diamtroBarraMM = _dtoBarraRefBordeLibre.br.diamEnMM;
            this.numeroBarra = _dtoBarraRefBordeLibre.numeroBarra;
            this.diamtroBarraEstriboMM = 8;
            this._largoPataGeneral = Util.CmToFoot(20);
            this._BarraTipo = _dtoBarraRefBordeLibre._TipoRebar;
        }

        #endregion

        #region 2) metodos
        /// <summary>
        /// dibuja las barra 
        /// </summary>
        public bool generarBarra_ConTrans()
        {
            try
            {
                //datos inciiales
                M1_ConfigurarDatosIniciales();
                VerificarEspesor();
                //configurar
                M2_ConfigurarBarraCurve();
                //dibujar barra
                M3_DibujarBarraCurve_ConTrans();
                //configuracion espaciamiento
                //agregar parametro interno
                M4_ConfigurarAsignarParametrosRebarshapeREbarRefuerzo_COnTrans();
                M1_6_visualizar();
            }
            catch (Exception)
            {
                return IsOK = false;

            }
            return IsOK = true;
        }

        // ufncion si losa es de espesor variable
        private void VerificarEspesor()
        {
            if (espesorLosa == 0 && (ElementoHost is Floor))
                espesorLosa = ((Floor)ElementoHost).ObtenerEspesorConPtosFloor((EndPoint_offse + startPont_offset) / 2, false);//  ObtenerEspesorLosaVariable.ObtenerEspesorConPtos((Floor)ElementoHost, (EndPoint_offse + startPont_offset) / 2);
        }

        public bool generarBarra_SinTrans()
        {
            try
            {
                //datos inciiales
                M1_ConfigurarDatosIniciales();
                VerificarEspesor();
                //configurar
                M2_ConfigurarBarraCurve();
                //dibujar barra
                if (M3_DibujarBarraCurve_SinTrans() != Result.Succeeded) return false;
                //configuracion espaciamiento
                //agregar parametro interno
                if (!M4_ConfigurarAsignarParametrosRebarshapeREbarRefuerzo_SinTras()) return false;

                tipoColor = TipoCOlores.magenta;
                //M1_6_visualizar();
            }
            catch (Exception)
            {
                return IsOK = false;

            }
            return IsOK = true;

        }
        //obtener los datos de la cuarva
        public override void M2_ConfigurarBarraCurve()
        {
            XYZ normXY0 = Util.GetVectorPerpendicular2((EndPoint_offse - startPont_offset).Normalize());

            norm = -normXY0.CrossProduct((EndPoint_offse - startPont_offset).Normalize());
            switch (_calculoBarraRefuerzo.tipoBarraRefuerzo)
            {
                case TipoBarraRefuerzo.BarraRefPataInicial:
                    ObtenerBarraRefPataInicial();
                    break;
                case TipoBarraRefuerzo.BarraRefPataFinal:
                    ObtenerBarraRefPataFinal();
                    break;

                case TipoBarraRefuerzo.BarraRefPataAmbos:
                    ObtenerBarraRefPataAmbos();
                    break;
                case TipoBarraRefuerzo.buscar:
                case TipoBarraRefuerzo.NoBuscar:
                case TipoBarraRefuerzo.BarraRefPataAUTO:
                case TipoBarraRefuerzo.EstriboRef:
                case TipoBarraRefuerzo.BarraRefSinPatas:
                    ObtenerBarraRefSinPatas();
                    break;
                default:
                    ObtenerBarraRefSinPatas();
                    break;
            }
        }

        private void ObtenerBarraRefPataInicial()
        {
            //pata inicial
            //double largoPataInicial = startPont_offset.DistanceTo(_calculoBarraRefuerzo.pa_Orig);
            Line mcurve1 = Line.CreateBound(startPont_offset + _calculoBarraRefuerzo._direcciMueveBarra * _largoPataGeneral, startPont_offset);
            listcurve.Add(mcurve1);

            //centrol
            double largoCentrasl = startPont_offset.DistanceTo(EndPoint_offse);
            Line mcurve2 = Line.CreateBound(startPont_offset, EndPoint_offse);
            listcurve.Add(mcurve2);

            //norm = new XYZ(0, 0, -1);   //; Util.CrossProduct(new XYZ(0, 0, 1),((Line)curve).Direction)   con esto se obtiene una direccion perpendicular hacia dentro de la losa
            largo_Cm = (Math.Round(Util.FootToCm(largoCentrasl), 0) + Math.Round(+Util.FootToCm(_largoPataGeneral), 0)).ToString();
            largoParciales_CM = $"({Math.Round(Util.FootToCm(_largoPataGeneral), 0)}+ {Math.Round(Util.FootToCm(largoCentrasl), 0)})";
        }
        private void ObtenerBarraRefPataFinal()
        {

            //centrol
            double largoCentrasl = startPont_offset.DistanceTo(EndPoint_offse);
            Line mcurve2 = Line.CreateBound(startPont_offset, EndPoint_offse);
            listcurve.Add(mcurve2);

            //pata final
            double largoPataFinal = EndPoint_offse.DistanceTo(_calculoBarraRefuerzo.pd_Orig);
            Line mcurve3 = Line.CreateBound(EndPoint_offse, EndPoint_offse + _calculoBarraRefuerzo._direcciMueveBarra * _largoPataGeneral);
            listcurve.Add(mcurve3);


            //norm = new XYZ(0, 0, -1);   //; Util.CrossProduct(new XYZ(0, 0, 1),((Line)curve).Direction)   con esto se obtiene una direccion perpendicular hacia dentro de la losa

            largo_Cm = (Math.Round(Util.FootToCm(largoCentrasl), 0) + Math.Round(+Util.FootToCm(_largoPataGeneral), 0)).ToString();
            largoParciales_CM = $"({Math.Round(Util.FootToCm(largoCentrasl), 0)}+{Math.Round(Util.FootToCm(_largoPataGeneral), 0)})";
        }

        private void ObtenerBarraRefPataAmbos()
        {
            //pata inicial
            double largoPataInicial = startPont_offset.DistanceTo(_calculoBarraRefuerzo.pa_Orig);
            Line mcurve1 = Line.CreateBound(startPont_offset + _calculoBarraRefuerzo._direcciMueveBarra * _largoPataGeneral, startPont_offset);
            listcurve.Add(mcurve1);

            //centrol
            double largoCentrasl = startPont_offset.DistanceTo(EndPoint_offse);
            Line mcurve2 = Line.CreateBound(startPont_offset, EndPoint_offse);
            listcurve.Add(mcurve2);

            //pata final
            double largoPataFinal = EndPoint_offse.DistanceTo(_calculoBarraRefuerzo.pd_Orig);
            Line mcurve3 = Line.CreateBound(EndPoint_offse, EndPoint_offse + _calculoBarraRefuerzo._direcciMueveBarra * _largoPataGeneral);
            listcurve.Add(mcurve3);

            //norm = new XYZ(0, 0, -1);   //; Util.CrossProduct(new XYZ(0, 0, 1),((Line)curve).Direction)   con esto se obtiene una direccion perpendicular hacia dentro de la losa

            largo_Cm = (Math.Round(Util.FootToCm(largoCentrasl), 0) + Math.Round(+Util.FootToCm(_largoPataGeneral), 0) * 2).ToString();
            largoParciales_CM = $"({Math.Round(Util.FootToCm(_largoPataGeneral), 0)}+{Math.Round(Util.FootToCm(largoCentrasl), 0)}+{Math.Round(Util.FootToCm(_largoPataGeneral), 0)})";
        }

        private void ObtenerBarraRefSinPatas()
        {
            //centrol
            Line mcurve2 = Line.CreateBound(startPont_offset, EndPoint_offse);
            //norm = new XYZ(0, 0, -1);   //; Util.CrossProduct(new XYZ(0, 0, 1),((Line)curve).Direction)   con esto se obtiene una direccion perpendicular hacia dentro de la losa
            listcurve.Add(mcurve2);

            largo_Cm = Math.Round(Util.FootToCm(startPont_offset.DistanceTo(EndPoint_offse)), 0).ToString(); ;
            largoParciales_CM = "";
        }

        #endregion
        public void M4_ConfigurarAsignarParametrosRebarshapeREbarRefuerzo_COnTrans()
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("modificar parametros rebarrefuerzo-NH");
                    if (viewActual != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null) ParameterUtil.SetParaInt(_rebar, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"
                    if (ParameterUtil.FindParaByName(_rebar, "CuantiaRefuerzo") != null) ParameterUtil.SetParaInt(_rebar, "CuantiaRefuerzo", textREfuerzoBarra + numeroBarra);
                    if (ParameterUtil.FindParaByName(_rebar, "CantidadBarra") != null) ParameterUtil.SetParaInt(_rebar, "CantidadBarra", numeroBarra.ToString());  //"(2+2+2+2)"
                    if (ParameterUtil.FindParaByName(_rebar, "LargoParciales") != null) ParameterUtil.SetParaInt(_rebar, "LargoParciales", largoParciales_CM);//(30+100+30)
                    if (ParameterUtil.FindParaByName(_rebar, "LargoTotal") != null) ParameterUtil.SetParaInt(_rebar, "LargoTotal", largo_Cm);//(30+100+30)
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                //   TaskDialog.Show("Error", msj);
            }
        }

        public bool M4_ConfigurarAsignarParametrosRebarshapeREbarRefuerzo_SinTras()
        {
            try
            {
                if (ParameterUtil.FindParaByName(_rebar, "BarraTipo") != null && _BarraTipo != TipoRebar.NONE)
                    ParameterUtil.SetParaInt(_rebar, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo));  //"nombre de vista"

                if (viewActual != null && ParameterUtil.FindParaByName(_rebar, "NombreVista") != null) ParameterUtil.SetParaInt(_rebar, "NombreVista", viewActual.ObtenerNombreIsDependencia());  //"nombre de vista"
                if (ParameterUtil.FindParaByName(_rebar, "CuantiaRefuerzo") != null) ParameterUtil.SetParaInt(_rebar, "CuantiaRefuerzo", textREfuerzoBarra + numeroBarra);
                if (ParameterUtil.FindParaByName(_rebar, "CantidadBarra") != null) ParameterUtil.SetParaInt(_rebar, "CantidadBarra", numeroBarra.ToString());  //"(2+2+2+2)"
                if (ParameterUtil.FindParaByName(_rebar, "LargoParciales") != null) ParameterUtil.SetParaInt(_rebar, "LargoParciales", largoParciales_CM);//(30+100+30)
                if (ParameterUtil.FindParaByName(_rebar, "LargoTotal") != null) ParameterUtil.SetParaInt(_rebar, "LargoTotal", largo_Cm);//(30+100+30)
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }
            return true; ;
        }
    }
}
