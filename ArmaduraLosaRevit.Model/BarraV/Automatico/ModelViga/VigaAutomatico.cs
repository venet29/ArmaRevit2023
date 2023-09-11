using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    public class VigaAutomatico
    {

        public int IdentificadorViga_revit { get; set; }
        public string NombreEje { get; }
        public VigaGeometriaDTO VigaGeometriaDTO { get; set; }
        public List<BarraFlexion> ListaBArras { get; set; }
        public List<BarraCorteTramos> ListaEstribos { get;  set; }
        public string VigaIdem { get;  set; }

   
        public VigaAutomatico(int nombreviga, string nombreEje, List<BarraFlexion> listaBArras)
        {
            this.IdentificadorViga_revit = nombreviga;
            this.NombreEje = nombreEje;
            this.ListaBArras = listaBArras;
            this.ListaEstribos = new List<BarraCorteTramos>();
            this.VigaIdem = "None";
        }

        public bool ReAjustarBarras(List<VigaGeometriaDTO> listaVigas)
        {
            try
            {
                double largoVIga_mm = Math.Abs(VigaGeometriaDTO.p1_cm.X - VigaGeometriaDTO.p2_cm.X) * 10;
                if (VigaGeometriaDTO.p1_cm.X == 0)
                    largoVIga_mm = Math.Abs(VigaGeometriaDTO.p1_cm.Y - VigaGeometriaDTO.p2_cm.Y) * 10;
                else
                    largoVIga_mm = Math.Abs(VigaGeometriaDTO.p1_cm.X - VigaGeometriaDTO.p2_cm.X) * 10;

                for (int i = 0; i < ListaBArras.Count; i++)
                {
                    var barra = ListaBArras[i].BarraFlexionTramosDTO_;


                    barra.VigaContenedor = VigaGeometriaDTO;

                    AsignarVigaIdem(barra);

                    if (barra.TipoBarraRefuerzoViga == TipoBarraRefuerzoViga.RefuerzoCentral)
                    {
                        if (VigaGeometriaDTO.p1_cm.X == 0)
                        {
                            barra.p1_mm.X = VigaGeometriaDTO.p1_cm.Y * 10 + largoVIga_mm * 0.1;
                            barra.p2_mm.X = VigaGeometriaDTO.p2_cm.Y * 10 - largoVIga_mm * 0.1;
                        }
                        else
                        {
                            barra.p1_mm.X = VigaGeometriaDTO.p1_cm.X * 10 + largoVIga_mm * 0.1;
                            barra.p2_mm.X = VigaGeometriaDTO.p2_cm.X * 10 - largoVIga_mm * 0.1;
                        }
                        barra.TipoPataIzqInf = TipoPataBarra.BarraVSinPatas;
                        barra.TipoPataDereSup = TipoPataBarra.BarraVSinPatas;
                    }
                    else if (barra.TipoBarraRefuerzoViga == TipoBarraRefuerzoViga.RefuerzoVigas && barra.ListVigas.Count == 2)
                    {
                        var vigaIZq = barra.ListVigas.Where(c => c.Posicion == "IZQ").FirstOrDefault()?.IdVIga_string;
                        var vigaDere = barra.ListVigas.Where(c => c.Posicion == "DERE").FirstOrDefault()?.IdVIga_string;

                        if (vigaIZq == null || vigaDere == null) continue;

                        var vigaGeo1 = listaVigas.Where(c => c.ID_Name_REVIT == vigaIZq).FirstOrDefault();
                        var vigaGeo2 = listaVigas.Where(c => c.ID_Name_REVIT == vigaDere).FirstOrDefault();
                        if (vigaGeo1 == null || vigaGeo2 == null) continue;



                        if (VigaGeometriaDTO.p1_cm.X == 0)
                        {
                            double largoIzq_mm = Math.Abs(vigaGeo1.p1_cm.Y - vigaGeo1.p2_cm.Y) * 10;
                            double largoDere_mm = Math.Abs(vigaGeo2.p1_cm.Y - vigaGeo2.p2_cm.Y) * 10;

                            barra.p1_mm.X = vigaGeo1.p2_cm.Y * 10 - largoIzq_mm * 0.25;
                            barra.p2_mm.X = vigaGeo2.p1_cm.Y * 10 + largoDere_mm * 0.25;
                        }
                        else
                        {
                            double largoIzq_mm = Math.Abs(vigaGeo1.p1_cm.X - vigaGeo1.p2_cm.X) * 10;
                            double largoDere_mm = Math.Abs(vigaGeo2.p1_cm.X - vigaGeo2.p2_cm.X) * 10;

                            barra.p1_mm.X = vigaGeo1.p2_cm.X * 10 - largoIzq_mm * 0.25;
                            barra.p2_mm.X = vigaGeo2.p1_cm.X * 10 + largoDere_mm * 0.25;
                        }


                        barra.TipoPataIzqInf = TipoPataBarra.BarraVSinPatas;
                        barra.TipoPataDereSup = TipoPataBarra.BarraVSinPatas;

                    }
                    else if (barra.TipoBarraRefuerzoViga == TipoBarraRefuerzoViga.RefuerzoBorde && barra.ListVigas.Count == 2)
                    {
                        var viga = barra.ListVigas.FirstOrDefault()?.IdVIga_string;


                        if (viga == null) continue;
                        double LargoDesarrollo_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(barra.diametro_Barras__mm));
                        var vigaGeo = listaVigas.Where(c => c.ID_Name_REVIT == viga).FirstOrDefault();
                        if (vigaGeo != null)
                        {

                            if (VigaGeometriaDTO.p1_cm.X == 0)
                            {
                                if (barra.Seccion_Inicio == 1)
                                {
                                    barra.p1_mm.X = vigaGeo.p1_cm.Y * 10 - LargoDesarrollo_mm;
                                    barra.p2_mm.X = vigaGeo.p1_cm.Y * 10 + LargoDesarrollo_mm;
                                }
                                else
                                {
                                    barra.p1_mm.X = vigaGeo.p2_cm.Y * 10 - LargoDesarrollo_mm;
                                    barra.p2_mm.X = vigaGeo.p2_cm.Y * 10 + LargoDesarrollo_mm;
                                }
                            }
                            else
                            {
                                if (barra.Seccion_Inicio == 1)
                                {
                                    barra.p1_mm.X = vigaGeo.p1_cm.X * 10 - LargoDesarrollo_mm;
                                    barra.p2_mm.X = vigaGeo.p1_cm.X * 10 + LargoDesarrollo_mm;
                                }
                                else
                                {
                                    barra.p1_mm.X = vigaGeo.p2_cm.X * 10 - LargoDesarrollo_mm;
                                    barra.p2_mm.X = vigaGeo.p2_cm.X * 10 + LargoDesarrollo_mm;
                                }
                            }
                        }

                        barra.TipoPataIzqInf = TipoPataBarra.BarraVSinPatas;
                        barra.TipoPataDereSup = TipoPataBarra.BarraVSinPatas;

                    }
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al Reajustar datos de barras en viga:{IdentificadorViga_revit}.\nEx:{ex.Message}");
                return false;
            }
            return true;
        }

        private void AsignarVigaIdem(BarraFlexionTramosDTO barra)
        {
            if (VigaIdem != barra.VigaIdem || VigaIdem.ToLower() == "none")
            {
          
                if (VigaIdem.ToLower() == "none")
                    VigaIdem = barra.VigaIdem;
                else
                {
                    Util.ErrorMsg($"Viga con barras que tiene diferentes viga idem  vigaId:{VigaGeometriaDTO.Eje_REVIT}");
                    VigaIdem = VigaIdem + "_" + barra.VigaIdem;
                }
            }
        }
    }
}