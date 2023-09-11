
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class VisibilidadElemenCambiarColorElev : VisibilidadElementBase
    {


        List<TipoRebar> listaTipoRebar = new List<TipoRebar>();

        public VisibilidadElemenCambiarColorElev(UIApplication uiapp) : base(uiapp)
        {

            listaTipoRebar = new List<TipoRebar>();
        }

        public override void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view)
        {
            listaTipoRebar = new List<TipoRebar>();
        }

        public bool M9_Restablecer_Color_BarrasElevacion(List<WrapperRebar> _lista_A_DeRebarVistaActual)
        {
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("MostrarEstadoNormal-NH");
                    if (_lista_A_DeRebarVistaActual == null) return false;
                    if (_lista_A_DeRebarVistaActual.Count == 0) return false;
                    try
                    {
                        //1

                        //4)
                        try
                        {
                            using (Transaction tr = new Transaction(_doc, "actualizarVisualizacion normal"))
                            {
                                tr.Start();

                                //4)


                                //1
                                var listaBarraEstriBosMuro = _lista_A_DeRebarVistaActual
                                                                       .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES ||
                                                                                                c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_L ||
                                                                                                c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_T))
                                                                       .Select(c => c.element.Id).ToList();

                                CambiarColorBarras_Service CambiarColorBarras_Service_esrtiboMuro = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.EstriboMuro);
                                CambiarColorBarras_Service_esrtiboMuro.M1_2_CAmbiarColor_sintrans(listaBarraEstriBosMuro);

                                //2
                                var listaBarraEstriBosViga = _lista_A_DeRebarVistaActual
                                                  .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_V ||
                                                                           c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VL ||
                                                                           c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VT))
                                                  .Select(c => c.element.Id).ToList();

                                CambiarColorBarras_Service CambiarColorBarras_Service_EstriboViga = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.EstriboViga);
                                CambiarColorBarras_Service_EstriboViga.M1_2_CAmbiarColor_sintrans(listaBarraEstriBosViga);

                                //3
                                var listaBarraCOnfinamiento = _lista_A_DeRebarVistaActual
                                                            .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO ||
                                                                                     c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO_T))
                                                            .Select(c => c.element.Id).ToList();

                                CambiarColorBarras_Service CambiarColorBarras_Service_confina = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.Estribo);
                                CambiarColorBarras_Service_confina.M1_2_CAmbiarColor_sintrans(listaBarraCOnfinamiento);

                                //4
                                var listaBarraMalla = _lista_A_DeRebarVistaActual
                                                            .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_H ||
                                                                                     c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_V))
                                                            .Select(c => c.element.Id).ToList();

                                CambiarColorBarras_Service CambiarColorBarras_Service_Malla = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.ElevMallaH);
                                CambiarColorBarras_Service_Malla.M1_2_CAmbiarColor_sintrans(listaBarraMalla);


                                //5 rebar
                                var listaBarra = _lista_A_DeRebarVistaActual
                                                  .Where(c => c != null && (c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_H ||
                                                                           c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_V))
                                                  .Select(c => c.element.Id).ToList();

                                CambiarColorBarras_Service CambiarColorBarras_Service_barra = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.ELEV_BA_H);
                                CambiarColorBarras_Service_Malla.M1_2_CAmbiarColor_sintrans(listaBarraMalla);


                                tr.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($" ex:{ex.Message}");
                            t.RollBack();
                            return false;
                        }



                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error  en 'M3_Restablecer_PathRein_Symbol_Tag_aEstadoNormal'  ex:{ex.Message}");
                        return false;
                    }
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }


            return true;
        }


        public bool M9_Ocultar_Color_BarrasElevacion_Traba(List<WrapperRebar> _lista_A_DeRebarVistaActual, bool Isocultar = false)
        {
            try
            {
                if (_lista_A_DeRebarVistaActual == null) return false;
                if (_lista_A_DeRebarVistaActual.Count == 0) return false;

                //b
                listaTipoRebar.Clear();
                listaTipoRebar.Add(TipoRebar.ELEV_CO_T);

                var listaestriboCOnfinamiento = _lista_A_DeRebarVistaActual.Where(c => VeriificarSIcorresponde(c.ObtenerTipoBarra.TipoBarra_))
                                                              .Select(c => c.element.Id).ToList();

                 GenerarCambioCOlor(Isocultar, listaestriboCOnfinamiento, TipoConfiguracionEstribo.Estribo);

                //b
                listaTipoRebar.Clear();
                listaTipoRebar.Add(TipoRebar.ELEV_ES_T);
                listaTipoRebar.Add(TipoRebar.ELEV_ES_VT);

                var listaestriboMuro = _lista_A_DeRebarVistaActual.Where(c => VeriificarSIcorresponde( c.ObtenerTipoBarra.TipoBarra_))
                                                              .Select(c => c.element.Id).ToList();

                return GenerarCambioCOlor(Isocultar, listaestriboMuro, TipoConfiguracionEstribo.EstriboMuro);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }
            return true;
        }

        public bool VeriificarSIcorresponde(TipoRebar _TipoRebar) => listaTipoRebar.Exists(c => c == _TipoRebar);
     

        public bool M9_Ocultar_Color_BarrasElevacion_EStriboVigaLAterales(List<WrapperRebar> _lista_A_DeRebarVistaActual, bool Isocultar = false)
        {
            try
            {
                if (_lista_A_DeRebarVistaActual == null) return false;
                if (_lista_A_DeRebarVistaActual.Count == 0) return false;

                listaTipoRebar.Add(TipoRebar.ELEV_ES_V);
                listaTipoRebar.Add(TipoRebar.ELEV_ES_VL);

                var listaestribo = _lista_A_DeRebarVistaActual.Where(c => VeriificarSIcorresponde(c.ObtenerTipoBarra.TipoBarra_))
                                                              .Select(c => c.element.Id).ToList();
                return GenerarCambioCOlor(Isocultar, listaestribo, TipoConfiguracionEstribo.EstriboViga_Lateral);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex: {ex.Message}");
            }
            return true;
        }

        private bool GenerarCambioCOlor(bool Isocultar, List<ElementId> listaestribo, TipoConfiguracionEstribo _tipo)
        {

            try
            {
                using (Transaction tr = new Transaction(_doc, "actualizarVisualizacion normal"))
                {
                    tr.Start();
                    if (Isocultar)
                    {
                        CambiarColorBarras_Service CambiarColorBarras_Service_confina = new CambiarColorBarras_Service(_uiapp);
                        // CambiarColorBarras_Service_confina.M1_2_CAmbiarColor_sintrans(listaestribo);
                        CambiarColorBarras_Service_confina.M1_3_CAmbiarColorPorColor_sintrans(listaestribo, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.blanco), false);
                    }
                    else //restablecer
                    {
                        CambiarColorBarras_Service CambiarColorBarras_Service_confina = new CambiarColorBarras_Service(_uiapp, _tipo);
                        CambiarColorBarras_Service_confina.M1_2_CAmbiarColor_sintrans(listaestribo);
                    }
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'GenerarCambioColor' ex:{ex.Message}");

                return false;
            }

            return true;
        }
    }
}

