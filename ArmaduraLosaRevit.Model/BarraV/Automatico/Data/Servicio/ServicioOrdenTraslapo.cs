using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.BarraV.Automatico.Servicios;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Data.Servicio
{
    class ServicioOrdenTraslapo
    {

        private readonly List<Tabla03_Info_Traslapos_Vigas> listaTraslapoEnBruto;
        private List<BarraFlexionTramosDTO> listaBarrasVigas_secciones;
        public List<TraslapoNH> ListaTraslapo { get; set; }
        public ServicioOrdenTraslapo(List<Tabla03_Info_Traslapos_Vigas> listaTraslapoEnBruto, List<BarraFlexionTramosDTO> listaBarrasVigas_secciones)
        {

            this.listaTraslapoEnBruto = listaTraslapoEnBruto;
            this.listaBarrasVigas_secciones = listaBarrasVigas_secciones;
            ListaTraslapo = new List<TraslapoNH>();
        }

        internal bool Ejecutar()
        {
            try
            {
                foreach (Tabla03_Info_Traslapos_Vigas TraslpoSeccion in listaTraslapoEnBruto)
                {
                    TraslapoNH _newTraslapoNH = new TraslapoNH(TraslpoSeccion);
                    if (!_newTraslapoNH.ObtenerDatos()) continue;

                    if (!ObtenerSeccionesAnteriorYPosterior(listaBarrasVigas_secciones, _newTraslapoNH)) continue;

                    ListaTraslapo.Add(_newTraslapoNH);
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private bool ObtenerSeccionesAnteriorYPosterior(List<BarraFlexionTramosDTO> listaBarrasVigas_secciones, TraslapoNH newTraslapoNH)
        {
            //var posicion = (newTraslapoNH.Fila_Barra.Contains("SUP_") ? TipoCaraObjeto.Superior : TipoCaraObjeto.Inferior);
            try
            {
                for (int i = 0; i < newTraslapoNH.ListaCasosTraslapos.Count; i++)
                {

                    CasosTraslapoDTO CasosTraslapoDTO_new = newTraslapoNH.ListaCasosTraslapos[i];

                    ListaTraslapoState.Lista.Add(CasosTraslapoDTO_new);

                    List<BarraFlexionTramosDTO> ListaBarraFlexionTramoDTO_inicial = new List<BarraFlexionTramosDTO>();
                    List<BarraFlexionTramosDTO> ListaBarraFlexTramoDTO_final = new List<BarraFlexionTramosDTO>();
                    int section_Inic = 0;
                    int section_Fin = 0;
                    switch (CasosTraslapoDTO_new.TraslapoEnSeccionNh_)
                    {
                        case TraslapoEnSeccionNh.TraslapoInicio:
                            ListaBarraFlexionTramoDTO_inicial = listaBarrasVigas_secciones.Where(b => b.ID_Name_REVIT_Inicio.ToString() == newTraslapoNH.ID_Name_REVIT && b.IsOk &&
                                               b.Seccion_Inicio == 1 && b.IdentiFIcadorParaTraslapo == newTraslapoNH.Fila_Barra).ToList();
                            if (ListaBarraFlexionTramoDTO_inicial.Count != 1) continue;

                            // implemetar buscar la barra en l la viga a la izq y asignar  tralapo
                            //trasapo final
                            double largoTraslapo_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(ListaBarraFlexionTramoDTO_inicial[0].diametro_Barras__mm));
                            ListaBarraFlexTramoDTO_final = listaBarrasVigas_secciones.Where(b => b.ID_Name_REVIT_Inicio.ToString() != newTraslapoNH.ID_Name_REVIT && b.IsOk &&
                                                                                        b.Seccion_Inicio == 4 && b.IdentiFIcadorParaTraslapo == newTraslapoNH.Fila_Barra &&
                                                                                    ISBarrasTraslapadaIncial(ListaBarraFlexionTramoDTO_inicial[0],b, largoTraslapo_mm)
                                                                                   ).ToList();
                            if (ListaBarraFlexTramoDTO_final.Count != 1) continue;

                            //rellenar informacion
                            CasosTraslapoDTO_new.BarraTramosAnterior = ListaBarraFlexTramoDTO_final[0];                                               
                            CasosTraslapoDTO_new.BarraTramosPosterior = ListaBarraFlexionTramoDTO_inicial[0];

                            ListaBarraFlexionTramoDTO_inicial[0].CasosTraslapoDTO_InicioTramo = CasosTraslapoDTO_new;
                            ListaBarraFlexTramoDTO_final[0].CasosTraslapoDTO_FinTramo = CasosTraslapoDTO_new;

                            continue;

                        case TraslapoEnSeccionNh.Traslpoa1_tercio:
                            section_Inic = 1;
                            section_Fin = 2;
                            break;

                        case TraslapoEnSeccionNh.Traslpoa2_tercio:
                            section_Inic = 2;
                            section_Fin = 3;
                            break;

                        case TraslapoEnSeccionNh.Traslpoa3_tercio:
                            section_Inic = 3;
                            section_Fin = 4;
                            break;

                        case TraslapoEnSeccionNh.TraslapoFinal:

                            ListaBarraFlexionTramoDTO_inicial = listaBarrasVigas_secciones.Where(b => b.ID_Name_REVIT_Inicio.ToString() == newTraslapoNH.ID_Name_REVIT && b.IsOk &&
                                                                                b.Seccion_Inicio == 4 && b.IdentiFIcadorParaTraslapo == newTraslapoNH.Fila_Barra).ToList();
                            if (ListaBarraFlexionTramoDTO_inicial.Count != 1) continue;
                         
                            // implemetar buscar la barra en l la viga a la izq y asignar  tralapo
                            //trasapo inciial
                            double AuxlargoTraslapo_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(ListaBarraFlexionTramoDTO_inicial[0].diametro_Barras__mm));
                             ListaBarraFlexTramoDTO_final = listaBarrasVigas_secciones.Where(b => b.ID_Name_REVIT_Inicio.ToString() != newTraslapoNH.ID_Name_REVIT && b.IsOk &&
                                                                                         b.Seccion_Inicio == 1 && b.IdentiFIcadorParaTraslapo == newTraslapoNH.Fila_Barra &&
                                                                                         ISBarrasTraslapadasFinal(ListaBarraFlexionTramoDTO_inicial[0], b, AuxlargoTraslapo_mm)
                                                                                    ).ToList();
                            if (ListaBarraFlexTramoDTO_final.Count != 1) continue;

                            CasosTraslapoDTO_new.BarraTramosAnterior = ListaBarraFlexTramoDTO_final[0];
                            CasosTraslapoDTO_new.BarraTramosPosterior = ListaBarraFlexionTramoDTO_inicial[0];

                            ListaBarraFlexionTramoDTO_inicial[0].CasosTraslapoDTO_FinTramo = CasosTraslapoDTO_new;
                            ListaBarraFlexTramoDTO_final[0].CasosTraslapoDTO_InicioTramo = CasosTraslapoDTO_new;
                        

                            continue;

                        case TraslapoEnSeccionNh.NONE:
                            continue;
                        default:
                            continue;
                    }

                    ListaBarraFlexionTramoDTO_inicial = listaBarrasVigas_secciones.Where(b => b.ID_Name_REVIT_Inicio.ToString() == newTraslapoNH.ID_Name_REVIT && b.IsOk &&
                                                     (b.Seccion_Inicio == section_Inic || b.Seccion_Inicio == section_Fin) && b.IdentiFIcadorParaTraslapo == newTraslapoNH.Fila_Barra).OrderBy(b => b.Seccion_Inicio).ToList();
                    if (ListaBarraFlexionTramoDTO_inicial.Count != 2) return false;

                    CasosTraslapoDTO_new.BarraTramosAnterior = ListaBarraFlexionTramoDTO_inicial[0];
                    CasosTraslapoDTO_new.BarraTramosPosterior = ListaBarraFlexionTramoDTO_inicial[1];
                    //           seccion i                         secc i+1
                    ListaBarraFlexionTramoDTO_inicial[0].CasosTraslapoDTO_FinTramo = CasosTraslapoDTO_new;//(a)           // resultLIsta[0].TraslapoFinTramo(a)  //   resultLIsta[1].TraslapoInicioTramo (b)
                    ListaBarraFlexionTramoDTO_inicial[1].CasosTraslapoDTO_InicioTramo = CasosTraslapoDTO_new;//b)

                    // si estoy en seccion  y quiero ver  si hay traslapo inicial     busco:  TraslapoInicioTramo(b)
                    // si estoy en seccion y quiero ver si hay traslapo fin           busco:  TraslapoFinTramo(a)
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerLista . ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool ISBarrasTraslapadasFinal(BarraFlexionTramosDTO barraFlexionTramosDTO, BarraFlexionTramosDTO b, double AuxlargoTraslapo_mm)
        {
            return barraFlexionTramosDTO.p1_mm.X < b.p1_mm.X - AuxlargoTraslapo_mm && 
                        b.p1_mm.X - AuxlargoTraslapo_mm < barraFlexionTramosDTO.p2_mm.X + AuxlargoTraslapo_mm;
        }

        private bool ISBarrasTraslapadaIncial(BarraFlexionTramosDTO barraFlexionTramosDTO, BarraFlexionTramosDTO b, double largoTraslapo_mm)
        {
            return barraFlexionTramosDTO.p1_mm.X - largoTraslapo_mm < b.p2_mm.X + largoTraslapo_mm &&
                    b.p2_mm.X + largoTraslapo_mm < barraFlexionTramosDTO.p2_mm.X + largoTraslapo_mm;
        }
    }
}
