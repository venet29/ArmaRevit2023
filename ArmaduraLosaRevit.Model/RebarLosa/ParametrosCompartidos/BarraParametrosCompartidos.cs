using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.ParametrosCompartidos
{
    public class BarraParametrosCompartidos
    {
        private Document _doc;
        private View _view;

        public List<ParametroBarra> listaParametroBarra { get; set; }
        public BarraParametrosCompartidos(Document doc)
        {
            this._doc = doc;
            this._view = doc.ActiveView; ;
            listaParametroBarra = new List<ParametroBarra>();
        }

        //public bool CopiarParametrosCompartidos(Element paraElem, TipoRebar _BarraTipo, string _Prefijo_F, TipoDireccionBarra TipoDireccionBarra_, UbicacionLosa ubicacionLosa)//UbicacionLosa ubicacionLosa)
        //{
        //    if (paraElem == null)
        //    {
        //        Util.ErrorMsg("Error al generar parametros rebar. Rebar igual null");
        //        return false;
        //    }
        //    try
        //    {
        //        using (Transaction t = new Transaction(_doc))
        //        {
        //            t.Start("modificar parametros rebarrefuerzo-NH");

        //            CasosGenerales(paraElem, _BarraTipo, _Prefijo_F, ubicacionLosa);

        //            t.Commit();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}


        // caso 2)
        public bool CopiarParametrosCompartidos_fundaciones(Element paraElem, TipoRebar _BarraTipo, string _Prefijo_F,
            TipoDireccionBarra TipoDireccionBarra_, RebarInferiorDTO _rebarInferiorDTO)//UbicacionLosa ubicacionLosa)
        {
            if (paraElem == null)
            {
                Util.ErrorMsg("Error al generar parametros rebar. Rebar igual null");
                return false;
            }
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("modificar parametros rebarrefuerzo-NH");

                    foreach (var item in listaParametroBarra)
                    {
                        switch (item.tipoParametro)
                        {
                            case TipoParametro.string_:
                                ParameterUtil.SetParaInt(paraElem, item.para.ToString(), item.valorString);
                                break;
                            case TipoParametro.double_:
                                ParameterUtil.SetParaInt(paraElem, item.para.ToString(), item.valorDouble);
                                break;
                            case TipoParametro.int_:
                                ParameterUtil.SetParaInt(paraElem, item.para.ToString(), item.valorint);
                                break;
                            default:
                                break;
                        }

                    }

                    ParameterUtil.SetParaStringNH(paraElem, "Prefijo_F", _Prefijo_F);

                    ParameterUtil.SetParaStringNH(paraElem, "NombreVista", _view.ObtenerNombreIsDependencia());

                    if (ParameterUtil.FindParaByName(paraElem, "BarraTipo") != null && _BarraTipo != TipoRebar.NONE)
                    {
                        string BarraTTipo = Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo);
                        ParameterUtil.SetParaInt(paraElem, "BarraTipo", BarraTTipo);  //"nombre de vista"

                    }


                    var largoTotal_aux = listaParametroBarra.Where(c => c.para.ToString() == "LL").FirstOrDefault();
                    if (largoTotal_aux != null)
                        ParameterUtil.SetParaStringNH(paraElem, "LargoTotal", Math.Round(Util.FootToCm(largoTotal_aux.valorDouble), 0).ToString());  //"nombre de vista"



                    if (ParameterUtil.FindParaByName(paraElem, "BarraOrientacion") != null && _rebarInferiorDTO.ubicacionLosa != UbicacionLosa.NONE)
                    {
                        // string BarraTTipo = Tipos_Barras.Buscar_nombreTipoBarras_porTipoRebar(ubicacionLosa);
                        ParameterUtil.SetParaInt(paraElem, "BarraOrientacion", _rebarInferiorDTO.ubicacionLosa.ToString());  //"nombre de vista"

                    }

                    if (paraElem is Rebar)
                    {
                        string diamString = ((Rebar)paraElem).ObtenerDiametroInt().ToString();

                        if (diamString != "" && ParameterUtil.FindParaByName(paraElem, "TipoDiametro") != null)
                            ParameterUtil.SetParaInt(paraElem, "TipoDiametro", diamString);//(30+100+30)
                    }

                    if (TipoDireccionBarra_ != TipoDireccionBarra.NONE)
                    {
                        string text = ((TipoDireccionBarra_ == TipoDireccionBarra.Primaria) ? ConstNH.NOMBRE_BARRA_PRINCIPAL : ConstNH.NOMBRE_BARRA_SECUNADARIA);
                        if (ParameterUtil.FindParaByName(paraElem, "TipoDireccionBarra") != null)
                            ParameterUtil.SetParaInt(paraElem, "TipoDireccionBarra", text);  //"nombre de vista"
                    }


                    if (ParameterUtil.FindParaByName(paraElem, "LargoParciales") != null && _rebarInferiorDTO.TexToLargoParciales != "")
                        ParameterUtil.SetParaInt(paraElem, "LargoParciales", _rebarInferiorDTO.TexToLargoParciales);  //"nombre de vista"

                    if (ParameterUtil.FindParaByName(paraElem, "LargoTotal") != null && _rebarInferiorDTO.LargoTotal > 0)
                        ParameterUtil.SetParaInt(paraElem, "LargoTotal", _rebarInferiorDTO.LargoTotal);  //"nombre de vista"



                    t.Commit();
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

    }
}
