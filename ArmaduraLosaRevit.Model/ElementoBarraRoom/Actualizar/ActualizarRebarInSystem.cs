using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Actualizar
{
    public class ActualizarRebarInSystem
    {

        public static void ActualizarTipoPAthDiferenteRebarSystem_ConTrans(Document _doc, List<WrapperRebar> _lista_A_DePathReinfVistaActual)
        {
            try
            {


                var listaPathDistintoRebarInsyskmete = _lista_A_DePathReinfVistaActual.Where(c => c.ObtenerTipoBarra.TipoBarra_ != c.ListaRebarInsystemV2.First()?.ObtenerTipoBarra.TipoBarra_ ||  //si tipobarra path distinto al tipo primer rebarInSystem
                                                           c.ListaRebarInsystemV2.GroupBy(b => b.ObtenerTipoBarra.TipoBarra_).Select(b => b.Key).Count() > 1)  // si al agropar lista rebarInsystem mayor de 1 grupo, significa diferente nombre de tipode barra
                                                             .ToList();

                if (listaPathDistintoRebarInsyskmete.Count == 0) return;

                try
                {
                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("Crear TipoTextNote-NH");

                        for (int i = 0; i < listaPathDistintoRebarInsyskmete.Count; i++)
                        {
                            var item = listaPathDistintoRebarInsyskmete[i];
                            List<PathReinforcement> ListaparaEle = new List<PathReinforcement>() { item.element as PathReinforcement };
                            string _NombreVista = item.NombreView;
                            string _barraTipo = item.ObtenerTipoBarra.TipoBarra_.ToString();

                            AgregarParametroRebarSystem_sinTrans(ListaparaEle, _NombreVista, _barraTipo);
                        }

                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al copiar datos de view en RebarInSYtem ex:{ex.Message}");
                }



            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al copiar datos de view en RebarInSYtem ex:{ex.Message}");
            }
        }

        public static void AgregarParametroRebarSystem_ConTrans(Document _doc, List<PathReinforcement> ListaparaElem, string _NombreVista, string _barraTipo = "")
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TipoTextNote-NH");
                    AgregarParametroRebarSystem_sinTrans(ListaparaElem, _NombreVista, _barraTipo);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al copiar datos de view en RebarInSYtem ex:{ex.Message}");
            }
        }
        public static void AgregarParametroRebarSystem_sinTrans(List<PathReinforcement> ListaparaElem, string _NombreVista, string _barraTipo = "")
        {
            foreach (PathReinforcement item in ListaparaElem)
            {
                AgregarParametroRebarSystem_sinTrans(item, _NombreVista, _barraTipo);
            }

        }

        public static void AgregarParametroRebarSystem_sinTrans(PathReinforcement paraElem, string _NombreVista, string _barraTipo = "")
        {
            Document _doc = paraElem.Document;
            if (paraElem == null) return;

            //a)
            if (_NombreVista == "")
            {
                var resiltpara = ParameterUtil.FindParaByName(paraElem, "NombreVista");
                if (resiltpara == null)
                {
                    Util.ErrorMsg("Error al obtener 'NombreVista' del pathreinforment");
                    return;
                }
                _NombreVista = resiltpara.AsString();
            }
            //b)
            Parameter paraBarraTipo = null;
            if (_barraTipo == "")
            {
                paraBarraTipo = ParameterUtil.FindParaByName(paraElem, "BarraTipo");
                if (paraBarraTipo != null) _barraTipo = paraBarraTipo.AsString();
            }


            //c) analsis
            if (_barraTipo == "FUND_BA")
                CasoFundaciones_FUND_BA(paraElem, _barraTipo);
            else
            {
                List<ElementId> ListElemId = paraElem.GetRebarInSystemIds().ToList();

                for (int i = 0; i < ListElemId.Count; i++)
                {
                    ElementId item = ListElemId[i];

                    Element elm = _doc.GetElement2(item);
                    if (ParameterUtil.FindParaByName(elm, "NombreVista") != null)
                        ParameterUtil.SetParaInt(elm, "NombreVista", _NombreVista);  //"nombre de vista"


                    if (ParameterUtil.FindParaByName(elm, "BarraTipo") != null && _barraTipo != "")
                        ParameterUtil.SetParaInt(elm, "BarraTipo", _barraTipo);  //"nombre de vista"
                }
            }

        }

        private static string CasoFundaciones_FUND_BA(PathReinforcement paraElem, string _barraTipo)
        {
            if (_barraTipo == "FUND_BA")
            {
                string _tipoFace = "";
                var face = ParameterUtil.FindParaByName(paraElem, "Face");
                _tipoFace = face.AsValueString();
                if (_tipoFace == "Bottom")
                    _barraTipo = _barraTipo + "_INF";
                else
                    _barraTipo = _barraTipo + "_SUP";
                ParameterUtil.SetParaInt(paraElem, "BarraTipo", _barraTipo);  //"nombre de vista"
            }

            return _barraTipo;
        }
    }
}
