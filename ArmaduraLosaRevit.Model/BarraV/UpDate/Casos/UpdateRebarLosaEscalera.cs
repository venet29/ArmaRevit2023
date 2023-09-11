using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.BarraV.UpDate.Casos
{
    public class UpdateRebarLosaEscalera
    {
        private Document _doc;
        private Rebar _rebar;
        private readonly TipoRebar tipoBarraEspecifico;
        private CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo;

        public UpdateRebarLosaEscalera(Document doc, Rebar rebar, TipoRebar tipoBarraEspecifico)
        {
            _doc = doc;
            _rebar = rebar;
            this.tipoBarraEspecifico = tipoBarraEspecifico;
        }

        public void Ejecutar()
        {
            try
            {
                if (_rebar == null) return;

                RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                if (rebarShapeDrivenAccessor == null) return;

                _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero, tipoBarraEspecifico);
                if (!_CreadorListaWraperRebarLargo.Ejecutar()) return;

                M1_ActializarLargoParcialYTotal();

                M2_ModificarSegmentoRebar();

                M3_ModificarDiametroRebar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }


        private bool M1_ActializarLargoParcialYTotal()
        {
            try
            {

                List<WraperRebarLargo> ListaCurvaBarras = _CreadorListaWraperRebarLargo.ListaCurvaBarras;


                if (ListaCurvaBarras == null) return false;
                if (ListaCurvaBarras.Count==0) return false;

                string largoparciales = "";
                double largoTotal = 0;

                double diamCm2 = Convert.ToInt32(_rebar.LookupParameter("Bar Diameter").AsValueString().Replace("mm", "")) / 10.0f;
                double diamCm = Util.FootToCm(_rebar.LookupParameter("Bar Diameter").AsDouble());

                if (ListaCurvaBarras.Count > 1)
                {
                    foreach (WraperRebarLargo _wraperRebarLargo in ListaCurvaBarras)
                    {
                        if (largoparciales == "")
                        {
                            largoparciales = largoparciales + "" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                                                 + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                                                 + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);
                        }
                        else
                        {
                            largoparciales = largoparciales + "+" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                                                 + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                                                 + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);
                        }

                        largoTotal += +Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                        + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                     + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);
                    }
                    if (largoparciales != "") ParameterUtil.SetParaInt(_rebar, "LargoParciales", $"({largoparciales})");//(30+100+30)
                }
                else if (ListaCurvaBarras.Count == 1)
                {
                    largoTotal += Util.FootToCm(ListaCurvaBarras[0]._curve.Length);
                }
                ParameterUtil.SetParaInt(_rebar, "LargoTotal", $"{ Math.Round(largoTotal, 0)}");//(30+100+30)
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'M1_ActializarLargoParcialYTotal'  ex:{ex.Message}");
                return false;
            }

            return true;
        }


        private bool M2_ModificarSegmentoRebar()
        {

            try
            {
                List<parametrosRebar> ListaParametrosRebar = _CreadorListaWraperRebarLargo.ListaParametrosRebar;

                foreach (parametrosRebar item in ListaParametrosRebar)
                {
                    if (Util.IsSimilarValor(item.largo, 0, 0.001)) continue;
                    ParameterUtil.SetParaDoubleNH(_rebar, item.letraNH + "_", item.largo);//(30+100+30)v
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'M2_ModificarDiametroRebar'  ex:{ex.Message}");
                return false;
            }

            return true;
        }

        private bool M3_ModificarDiametroRebar()
        {
            try
            {

                ParameterSet pars = _rebar.Parameters;
                string tipoDiamAntiguo = ParameterUtil.FindParaByName(pars, "TipoDiametro").AsString();
                // if ParameterUtil.FindParaByName

                string diamString = _rebar.ObtenerDiametroInt().ToString();


                if (tipoDiamAntiguo == diamString) return false;
                if (!Util.IsNumeric(diamString))
                { Util.ErrorMsg($"Error UpdateRebar -> Diamtro Barra id:{_rebar.Id}, no numerico"); return false; ; }

                ParameterUtil.SetParaInt(_rebar, "TipoDiametro", diamString);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'M3_ModificarDiametroRebar'  ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
