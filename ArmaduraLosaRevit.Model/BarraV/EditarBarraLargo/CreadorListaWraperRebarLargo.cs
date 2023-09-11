using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo
{
    public class CreadorListaWraperRebarLargo
    {
        private Rebar _rebar;
        private TipoRebar tipoBArras;
        private readonly XYZ _ptoSeleccionEnrebar;
        private readonly TipoRebar _tipoBarraEspecifico;

        public List<parametrosRebar> ListaParametrosRebar { get; set; }
        public List<WraperRebarLargo> ListaCurvaBarras { get; set; }
        public XYZ _normal { get; private set; }

        public CreadorListaWraperRebarLargo(Rebar _rebar, XYZ ptoseleccionEnrebar)
        {
            this._rebar = _rebar;
            this._ptoSeleccionEnrebar = ptoseleccionEnrebar;
            this.ListaParametrosRebar = new List<parametrosRebar>();
            this.ListaCurvaBarras = new List<WraperRebarLargo>();
            this._tipoBarraEspecifico = TipoRebar.NONE;
        }
        public CreadorListaWraperRebarLargo(Rebar _rebar, XYZ ptoseleccionEnrebar, TipoRebar tipoBarraEspecifico)
        {
            this._rebar = _rebar;
            this._ptoSeleccionEnrebar = ptoseleccionEnrebar;
            this._tipoBarraEspecifico = tipoBarraEspecifico;
            this.ListaParametrosRebar = new List<parametrosRebar>();
            this.ListaCurvaBarras = new List<WraperRebarLargo>();
        }
        public bool Ejecutar()
        {
            try
            {
                if (IsExcluir()) return false;

                if (!A_ObtenerListaParametros()) return false;

                if (!B_ObtenerListaCurvas()) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'CreadorListaWraperRebarLargo'   ex:{ ex.Message} ");
                return false;
            }
            return true;
        }

        private bool IsExcluir()
        {
            try
            {
                tipoBArras = _rebar.ObtenerTipoBArra_TipoRebar();
                if (tipoBArras == TipoRebar.ELEV_BA_H || tipoBArras == TipoRebar.ELEV_BA_V ||
                    tipoBArras == TipoRebar.ELEV_ES_V || tipoBArras == TipoRebar.ELEV_ES ||
                    tipoBArras == TipoRebar.ELEV_CO || tipoBArras == TipoRebar.REFUERZO_BA_CAB_MURO ||
                    tipoBArras == TipoRebar.REFUERZO_BA || tipoBArras == TipoRebar.REFUERZO_BA_CAB_MURO ||
                    tipoBArras == TipoRebar.REFUERZO_BA_REF_LO || tipoBArras == TipoRebar.REFUERZO_BA_BORDE)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {

                return true;
            }

        }

        public bool EjecutarSoloListaCurva()
        {
            try
            {
                //if (!A_ObtenerListaParametros()) return false;

                if (!B_ObtenerListaCurvas()) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'CreadorListaWraperRebarLargo'   ex:{ ex.Message} ");
                return false;
            }
            return true;
        }
        public bool A_ObtenerListaParametros()
        {

            try
            {
                string[] listaLetra = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "K" };
                ParameterSet pars = _rebar.Parameters;
                foreach (var letra in listaLetra)
                {
                    var parabet = ParameterUtil.FindParaByName(pars, letra);
                    if (parabet == null) continue;
                    double largo = parabet.AsDouble();
                    if (largo != 0)
                    {

                        parametrosRebar _newparametrosRebar = (_tipoBarraEspecifico == TipoRebar.NONE
                                                                ? new parametrosRebar(letra, largo)
                                                                : new parametrosRebar(letra, largo, _tipoBarraEspecifico));
                        if (_newparametrosRebar.ObtenerLetraNH())
                            ListaParametrosRebar.Add(_newparametrosRebar);


                    };
                }
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public bool B_ObtenerListaCurvas()
        {
            try
            {
                var _driverAccesor = _rebar.GetShapeDrivenAccessor();
                _normal = _driverAccesor.Normal;

                if (!AyudaCurveRebar.GetPrimeraRebarCurves(_rebar)) return false;

                List<Curve> listapto1 = AyudaCurveRebar.ListacurvesSoloLineas[0];
                int cont = 0;
                foreach (Curve item in listapto1)
                {
                    // bool IsBarraPrincipal = curvaMAyorlargo.IsEqual((Line)item);
                    WraperRebarLargo newCurbaBarras = new WraperRebarLargo(item, ListaParametrosRebar[cont], false);
                    newCurbaBarras.DatosIniciales();
                    if (!(_ptoSeleccionEnrebar.IsAlmostEqualTo(XYZ.Zero))) newCurbaBarras.Generar(_ptoSeleccionEnrebar);
                    ListaCurvaBarras.Add(newCurbaBarras);
                    cont += 1;
                }

                // revisar si s
                if (!ListaCurvaBarras.Exists(c => c.IsCurvaSeleccionada))
                {
                    var result = ListaCurvaBarras.OrderBy(c => c.distanciaPtoSeleccion).FirstOrDefault();
                    if (result == null)
                    {
                        Util.ErrorMsg($"Error en 'B_ObtenerListaCurvas' ");
                        return false;
                    }
                    result.ReGenerar(_ptoSeleccionEnrebar);
                }


                if (ListaCurvaBarras.Count > 2)
                {
                    for (int i = 1; i < ListaCurvaBarras.Count - 1; i++)
                    {
                        ListaCurvaBarras[i].IsBarraPrincipal = true;
                    }
                    ListaCurvaBarras[0].FijacionInicial = FijacionRebar.movil;
                    ListaCurvaBarras.Last().FijacionFinal = FijacionRebar.movil;
                }
                else if (ListaCurvaBarras.Count == 2)
                {
                    if (ListaCurvaBarras[1].ParametrosRebar.largo > ListaCurvaBarras[0].ParametrosRebar.largo)
                        ListaCurvaBarras[1].IsBarraPrincipal = true;
                    else
                        ListaCurvaBarras[0].IsBarraPrincipal = true;

                    ListaCurvaBarras[0].FijacionInicial = FijacionRebar.movil;
                    ListaCurvaBarras.Last().FijacionFinal = FijacionRebar.movil;
                }
                else if (ListaCurvaBarras.Count == 1)
                {
                    ListaCurvaBarras[0].IsBarraPrincipal = true;
                    ListaCurvaBarras[0].FijacionInicial = FijacionRebar.movil;
                    ListaCurvaBarras[0].FijacionFinal = FijacionRebar.movil;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'B_ObtenerListaCurvas'. TIpo Barra: {tipoBArras }    id:{_rebar.Id} \n ex:{ex.Message} ");
                return false;
            }
            return true;
        }
    }
}
