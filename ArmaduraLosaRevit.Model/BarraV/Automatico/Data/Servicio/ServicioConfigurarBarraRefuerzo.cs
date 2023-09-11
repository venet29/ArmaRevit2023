using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Data.Servicio
{
    public class ServicioConfigurarBarraRefuerzo
    {
        private List<BarraFlexionTramosDTO> listaDebarrasPorLine;

        public ServicioConfigurarBarraRefuerzo(List<BarraFlexionTramosDTO> listaDebarrasPorLine)
        {
            this.listaDebarrasPorLine = listaDebarrasPorLine;
        }

        public BarraFlexionTramosDTO Analizar(BarraFlexionTramosDTO barra)
        {
            barra.inicial_tipoBarraH = TipoPataBarra.buscar;
            barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.NONE;

            if (barra.Seccion_Inicio == 1)
            {
              //  barra.IsConLaeader = true;
                barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoVigas;
                barra.IsConLaeader = true;
                //buscar inicial
                double largoTraslapo_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(barra.diametro_Barras__mm));
                var resultLIsta = listaDebarrasPorLine.Where(b => b.ID_Name_REVIT_Inicio.ToString() != barra.ID_Name_REVIT_Inicio.ToString() && b.IsOk &&
                                                                            b.Seccion_Inicio == 4 && b.IdentiFIcadorParaTraslapo == barra.IdentiFIcadorParaTraslapo &&
                                                                           barra.p1_mm.X - largoTraslapo_mm < b.p2_mm.X + largoTraslapo_mm && b.p2_mm.X + largoTraslapo_mm < barra.p2_mm.X + largoTraslapo_mm
                                                                       ).OrderByDescending(c => c.p2_mm.X).ToList();

                barra.ListVigas.Add(new VigasPosicion() { Posicion = "DERE", IdVIga_string = barra.ID_Name_REVIT_Inicio.ToString() });
                //pto final
                barra.p2_mm.X = barra.p1_mm.X; // realmente es   : barra.p1_mm.X+ 25% LargoVIga

                barra.inicial_tipoBarraH = TipoPataBarra.BuscarSinExtender;
                //pto inicial
                if (resultLIsta.Count != 0)
                {
                    barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoVigas;
                    barra.p1_mm.X = resultLIsta[0].p2_mm.X;// realmente es   : resultLIsta[0].p2_mm.X- 25% LargoVIga
                    resultLIsta[0].IsOk = false;
                    barra.ListVigas.Add(new VigasPosicion() { Posicion = "IZQ", IdVIga_string = resultLIsta[0].ID_Name_REVIT_Inicio.ToString() });
                }
                else
                {
                    barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoBorde;
                    
                    barra.p1_mm.X = barra.p1_mm.X - largoTraslapo_mm;
                    barra.p2_mm.X = barra.p2_mm.X + largoTraslapo_mm;
                    barra.ListVigas.Add(new VigasPosicion() { Posicion = "DERE", IdVIga_string = barra.ID_Name_REVIT_Inicio.ToString() });
                }

            }
            else if (barra.Seccion_Inicio == 2)
            {
                barra.IsConLaeader = true;
                //double largoTraslapo_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(barra.diametro_Barras__mm));
                barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoCentral;
                barra.inicial_tipoBarraH = TipoPataBarra.BarraVSinPatas;
                //barra.TipoTraslapo_=UbicacionTraslapoNh.
            }
            else if (barra.Seccion_Inicio == 3)
            {
                Util.ErrorMsg($"Error al crear barras.  Id viga:{barra.ID_Name_REVIT_Inicio}");
                barra.IsOk = false;
            }
            else if (barra.Seccion_Inicio == 4)
            {
              //  barra.IsConLaeader = true;
              
                //buscar inicial
                double largoTraslapo_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(barra.diametro_Barras__mm));
                var resultLIsta = listaDebarrasPorLine.Where(b => b.ID_Name_REVIT_Inicio.ToString() != barra.ID_Name_REVIT_Inicio.ToString() && b.IsOk &&
                                                                            b.Seccion_Inicio == 1 && b.IdentiFIcadorParaTraslapo == barra.IdentiFIcadorParaTraslapo &&
                                                                           barra.p1_mm.X - largoTraslapo_mm < b.p1_mm.X - largoTraslapo_mm && b.p1_mm.X - largoTraslapo_mm < barra.p2_mm.X + largoTraslapo_mm
                                                                       ).OrderBy(c => c.p2_mm.X).ToList();
                barra.ListVigas.Add(new VigasPosicion() { Posicion = "IZQ", IdVIga_string = barra.ID_Name_REVIT_Inicio.ToString() });

                // pto inicial
                barra.p1_mm.X = barra.p2_mm.X; // realmente es   : barra.p2_mm.X- 25% LargoVIga

                barra.inicial_tipoBarraH = TipoPataBarra.BuscarSinExtender;
                barra.IsConLaeader = true;
                //pto final
                if (resultLIsta.Count != 0)
                {
                    barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoVigas;
                    barra.p2_mm.X = resultLIsta[0].p1_mm.X; //realmente es   : resultLIsta[0].p1_mm.X + 25 % LargoVIga
                    barra.ListVigas.Add(new VigasPosicion() { Posicion = "DERE", IdVIga_string = resultLIsta[0].ID_Name_REVIT_Inicio.ToString() });
                    resultLIsta[0].IsOk = false;
                }
                else
                {
                    barra.TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.RefuerzoBorde;
                    barra.p1_mm.X = barra.p1_mm.X - largoTraslapo_mm;
                    barra.p2_mm.X = barra.p2_mm.X + largoTraslapo_mm;
                    // realmente barra.p2_mm.X = barra.p2_mm.X + LArgoDesarrollo
                    barra.ListVigas.Add(new VigasPosicion() { Posicion = "IZQ", IdVIga_string = barra.ID_Name_REVIT_Inicio.ToString() });
                }
            }


            return barra;
        }
    }
}
