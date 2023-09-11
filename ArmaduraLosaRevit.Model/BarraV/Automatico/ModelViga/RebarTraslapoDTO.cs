using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    // objeto que contiene los datos con barras inicial y con barra final
    public class RebarTraslapo_paraDubujarTraslapoDimensionDTO
    {
        private static int InstanceNH = 0;
        public int InstanceNH_A { get; set; }
        public List<IbarraBase> _listaDebarra_Auto { get; set; }
        public BarraFlexion BarraFlexion_ { get; set; }
        public BarraFlexionTramosDTO BarraDTO { get; set; }

        public CasosTraslapoDTO TraslaFinal { get; set; }
        public CasosTraslapoDTO TraslaInicio { get; set; }


        // datos traslapo al inicio de barra
        public Rebar BarraAnterior_Inicial { get; set; }
        public Rebar BarrasPosterior_Inicial { get; set; } // deberia ser la misma barra

        //datos traslapo al final de la barra
        public Rebar BarraAnterior_Final { get; set; }// deberia ser la misma barra
        public Rebar BarrasPosterior_Final { get; set; }

        public bool Isok { get; set; }

        public RebarTraslapo_paraDubujarTraslapoDimensionDTO(List<IbarraBase> listaDebarra_Auto, BarraFlexion barra)
        {
            _listaDebarra_Auto = listaDebarra_Auto;
            BarraFlexion_ = barra;
            BarraDTO = barra.BarraFlexionTramosDTO_;

            Isok = false;
            InstanceNH = InstanceNH + 1;
            InstanceNH_A = InstanceNH;
        }

        public bool BUscarInicio(List<RebarTraslapo_paraDubujarTraslapoDimensionDTO> listaCOntendoraFinalParaTraslapo)
        {
            try
            {
                var trp_inicial = BarraFlexion_.BarraFlexionTramosDTO_.CasosTraslapoDTO_InicioTramo;
                if (trp_inicial != null)
                {
                    if (!trp_inicial.IsOK) return false;

                    var anteriorUAx = listaCOntendoraFinalParaTraslapo.Where(c => c.BarraDTO.ID_Name_REVIT_Inicio == trp_inicial.BarraTramosAnterior.ID_Name_REVIT_Inicio &&
                                                                                    c.BarraDTO.ID_Name_REVIT_Final == trp_inicial.BarraTramosAnterior.ID_Name_REVIT_Final &&
                                                                                    c.BarraDTO.IdentiFIcadorParaTraslapo == trp_inicial.BarraTramosAnterior.IdentiFIcadorParaTraslapo &&
                                                                                    c.BarraDTO.Seccion_Final == trp_inicial.BarraTramosAnterior.Seccion_Inicio)
                                            .OrderBy(c => c.BarraDTO.Seccion_Inicio).ToList();

                    if (anteriorUAx.Count > 0)
                    {
                        //anteriorUAx[0].BarraFlexion_.IsOK = false;
                        BarraAnterior_Inicial = (Rebar)anteriorUAx[0].BarraFlexion_.BarraCreadoElem;
                    }


                    var PosterriorAux = listaCOntendoraFinalParaTraslapo.Where(c => c.BarraDTO.ID_Name_REVIT_Inicio == trp_inicial.BarraTramosPosterior.ID_Name_REVIT_Inicio &&
                                                                                    c.BarraDTO.ID_Name_REVIT_Final == trp_inicial.BarraTramosPosterior.ID_Name_REVIT_Final &&
                                                                                    c.BarraDTO.IdentiFIcadorParaTraslapo == trp_inicial.BarraTramosPosterior.IdentiFIcadorParaTraslapo &&
                                                                                    c.BarraDTO.Seccion_Final == trp_inicial.BarraTramosPosterior.Seccion_Final)
                                    .OrderBy(c => c.BarraDTO.Seccion_Inicio).ToList();

                    if (PosterriorAux.Count > 0)
                    {
                        //PosterriorAux[0].BarraFlexion_.IsOK = false;
                        BarrasPosterior_Inicial = (Rebar)PosterriorAux[0].BarraFlexion_.BarraCreadoElem;
                    }
                    //TraslaFinal = trp_inicial.BarraTramosPosterior.TraslapoFinTramo;
                    //TraslaInicio = trp_inicial.BarraTramosAnterior.TraslapoInicioTramo;
                    trp_inicial.IsOK = false;
                    Isok = true;

                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool BUscarFin(List<RebarTraslapo_paraDubujarTraslapoDimensionDTO> listaCOntendoraFinalParaTraslapo)
        {
            try
            {
                var trp_final = BarraFlexion_.BarraFlexionTramosDTO_.CasosTraslapoDTO_FinTramo;
                if (trp_final != null)
                {
                    if (!trp_final.IsOK) return false;

                    var anteriorUAx = listaCOntendoraFinalParaTraslapo.Where(c => c.BarraDTO.ID_Name_REVIT_Inicio == trp_final.BarraTramosAnterior.ID_Name_REVIT_Inicio &&
                                                                                  c.BarraDTO.ID_Name_REVIT_Final == trp_final.BarraTramosAnterior.ID_Name_REVIT_Final &&
                                                                                c.BarraDTO.IdentiFIcadorParaTraslapo == trp_final.BarraTramosAnterior.IdentiFIcadorParaTraslapo &&
                                                                                  c.BarraDTO.Seccion_Final == trp_final.BarraTramosAnterior.Seccion_Final &&
                                                                                c.BarraFlexion_.IsOK
                                                                ).OrderBy(c => c.BarraDTO.Seccion_Inicio).ToList();

                    if (anteriorUAx.Count > 0)
                    {
                        //anteriorUAx[0].BarraFlexion_.IsOK = false;
                        BarraAnterior_Final = (Rebar)anteriorUAx[0].BarraFlexion_.BarraCreadoElem;
                    }

                    var PosterriorAux = listaCOntendoraFinalParaTraslapo.Where(c => c.BarraDTO.ID_Name_REVIT_Inicio == trp_final.BarraTramosPosterior.ID_Name_REVIT_Inicio &&
                                                                                    c.BarraDTO.ID_Name_REVIT_Final == trp_final.BarraTramosPosterior.ID_Name_REVIT_Final &&
                                                                                    c.BarraDTO.IdentiFIcadorParaTraslapo == trp_final.BarraTramosPosterior.IdentiFIcadorParaTraslapo &&
                                                                                    c.BarraDTO.Seccion_Final == trp_final.BarraTramosPosterior.Seccion_Final &&
                                                                                    c.BarraFlexion_.IsOK
                                                   ).OrderByDescending(c => c.BarraDTO.Seccion_Inicio).ToList();

                    if (PosterriorAux.Count > 0)
                    {
                        //PosterriorAux[0].BarraFlexion_.IsOK = false;
                        BarrasPosterior_Final = (Rebar)PosterriorAux[0].BarraFlexion_.BarraCreadoElem;
                    }
                    //TraslaFinal = trp_final.BarraTramosPosterior.TraslapoFinTramo;
                    //TraslaInicio = trp_final.BarraTramosAnterior.TraslapoInicioTramo;
                    trp_final.IsOK = false;
                    Isok = true;

                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
