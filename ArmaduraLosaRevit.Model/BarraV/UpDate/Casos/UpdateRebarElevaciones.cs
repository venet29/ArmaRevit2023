using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class UpdateRebarElevaciones
    {
        private Document _doc;
        private Rebar _rebar;
        private readonly TipoRebar tipoBarraEspecifico;
        private CreadorListaWraperRebarLargo _CreadorListaWraperRebarLargo;

        public UpdateRebarElevaciones(Document doc, Rebar rebar, TipoRebar tipoBarraEspecifico)
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
                if (!_rebar.IsValidObject) return;

                //  RebarShapeDrivenAccessor rebarShapeDrivenAccessor = _rebar.GetShapeDrivenAccessor();
                //   if (rebarShapeDrivenAccessor == null) return;
                Stopwatch timeMeasure = Stopwatch.StartNew();
                _CreadorListaWraperRebarLargo = new CreadorListaWraperRebarLargo(_rebar, XYZ.Zero);
                if (!_CreadorListaWraperRebarLargo.Ejecutar()) return;
                Debug.WriteLine($"   VerificarDatos : {timeMeasure.ElapsedMilliseconds } ms");
                M1_ActializarLargoParcialYTotal();

                M2_ModificarSegmentoRebarV2();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }



        private bool M1_ActializarLargoParcialYTotal()
        {
            try
            {
                List<WraperRebarLargo> ListaCurvaBarras = _CreadorListaWraperRebarLargo.ListaCurvaBarras;

                if (ListaCurvaBarras == null) return false; ;

                string largoparciales = "";
                double largoTotal = 0;

                // double diamCm2 = Convert.ToInt32(_rebar.LookupParameter("Bar Diameter").AsValueString().Replace("mm", "")) / 10.0f;
                // double diamCm = Util.FootToCm(_rebar.LookupParameter("Bar Diameter").AsDouble());

                if (ListaCurvaBarras.Count > 1)
                {
                    foreach (WraperRebarLargo _wraperRebarLargo in ListaCurvaBarras)
                    {
                        if (largoparciales == "")
                        {
                            /* largoparciales = largoparciales + "" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                                                  + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                                                  + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);*/
                            largoparciales = largoparciales + "" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length));
                        }
                        else
                        {
                            /*largoparciales = largoparciales + "+" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                                                 + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                                                 + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);*/
                            largoparciales = largoparciales + "+" + Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length));
                        }

                        /* largoTotal += +Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length)
                                         + (_wraperRebarLargo.FijacionInicial == FijacionRebar.fijo ? diamCm / 2.0f : 0)
                                      + (_wraperRebarLargo.FijacionFinal == FijacionRebar.fijo ? diamCm / 2.0f : 0), 0);*/
                        largoTotal += +Math.Round(Util.FootToCm(_wraperRebarLargo._curve.Length));
                    }
                    ParameterUtil.SetParaInt(_rebar, "CantidadBarra", _rebar.Quantity);//(30+100+30)
                    if (largoparciales != "") ParameterUtil.SetParaInt(_rebar, "LargoParciales", $"({largoparciales})");//(30+100+30)
                }
                else if (ListaCurvaBarras.Count == 1)
                {
                    largoTotal += Util.FootToCm(ListaCurvaBarras[0]._curve.Length);
                }
                ParameterUtil.SetParaInt(_rebar, "LargoTotal", $"{ Math.Round(largoTotal, 0)}");//(30+100+30)
                //ParameterUtil.SetParaInt(_rebar, "BBOpt", "532");
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
                ParameterSet pars = _rebar.Parameters;
                string tipoDiamAntiguo = ParameterUtil.FindParaByName(pars, "TipoDiametro").AsString();
                // if ParameterUtil.FindParaByName

                string diamString = _rebar.ObtenerDiametroInt().ToString();


                if (tipoDiamAntiguo == diamString) return false;
                if (!Util.IsNumeric(diamString))
                { Util.ErrorMsg($"Diamtro Barra id:{_rebar.Id}, no numerico"); return false; }

                int diamInt = Util.ConvertirStringInInteger(diamString);
                double largoDesarrollo = UtilBarras.largo_ganchoFoot_diamMM(diamInt);


                ParameterUtil.SetParaInt(_rebar, "B_", largoDesarrollo);//(30+100+30)v
                ParameterUtil.SetParaInt(_rebar, "D_", largoDesarrollo);//(30+100+30)
                ParameterUtil.SetParaInt(_rebar, "TipoDiametro", diamString);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'M3_ModificarDiametroRebar'  ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private bool M2_ModificarSegmentoRebarV2()
        {
            try
            {
                return true;
#pragma warning disable CS0162 // Unreachable code detected
                ParameterSet pars = _rebar.Parameters;
#pragma warning restore CS0162 // Unreachable code detected
                string tipoDiamAntiguo = ParameterUtil.FindParaByName(pars, "TipoDiametro").AsString();
                // if ParameterUtil.FindParaByName
                string diamString = _rebar.ObtenerDiametroInt().ToString();

                if (tipoDiamAntiguo != diamString)
                {
                    if (!Util.IsNumeric(diamString))
                    {
                        Util.ErrorMsg($"Diamtro Barra id:{_rebar.Id}, no numerico");
                    }
                    else
                    {
                        ParameterUtil.SetParaInt(_rebar, "TipoDiametro", diamString);
                    }
                    return false;
                }

                //NOTA:al aprecer el problema espq el parametro es visible y se debe actualizar en la interfase

                for (int i = 0; i < _CreadorListaWraperRebarLargo.ListaParametrosRebar.Count; i++)
                {
                    var para = _CreadorListaWraperRebarLargo.ListaParametrosRebar[0];
                    if (para.largo == 0) continue;

                    ParameterUtil.SetParaInt(_rebar, "BBOpt", $"{ Math.Round(Util.FootToCm(para.largo))}");//(30+100+30)v
//                    ParameterUtil.SetParaInt(_rebar, "BBOpt", diamString);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error UpdateRebar -> 'M3_ModificarDiametroRebar'  ex:{ex.Message}");
                return false;
            }
#pragma warning disable CS0162 // Unreachable code detected
            return true;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
