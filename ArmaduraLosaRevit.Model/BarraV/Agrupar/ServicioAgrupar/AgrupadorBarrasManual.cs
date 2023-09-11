using ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.model;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.ServicioAgrupar
{
    public class AgrupadorBarrasManual : AgrupadorBarras
    {
        public AgrupadorBarrasManual(UIApplication uiapp, List<IndependentTag> listIndependentTag) : base(uiapp, listIndependentTag)
        {

        }

        public void AgruparVertical()
        {
            IsOK = true;
            try
            {
                double DeltaLargo = Util.CmToFoot(2);
                double DeltaZ = Util.CmToFoot(2);
                var listaAgrupadaDiamtro = ListBarraIng
                                   .GroupBy(c => new { c.diametroInt })
                                   .OrderByDescending(g => g.Key.diametroInt);


                var confBarraTag = ConfiguracionTAgBarraDTo.OBtenercasoVertical();

                foreach (var itemKey in listaAgrupadaDiamtro)
                {
                    List<BarraIng> listDiam = itemKey.ToList();


                    //analzia agupado por diametro
                    for (int i = 0; i < listDiam.Count; i++)
                    {
                        //analiza caso puntoal
                        List<BarraIng> listGrupoDiam = new List<BarraIng>();
                        for (int j = 0; j < listDiam.Count; j++)
                        {
                            BarraIng BarraIngAnal = listDiam[j];
                            if (BarraIngAnal.IsAgrupado) continue;

                            listGrupoDiam = listDiam.Where(c => c.IsAgrupado == false &&
                                                    Util.IsSimilarValor(c.largoFoot, BarraIngAnal.largoFoot, DeltaLargo) &&
                                                    Util.IsSimilarValor(c.P2.Z, BarraIngAnal.P2.Z, DeltaZ)).ToList();

                            listGrupoDiam.ForEach(x => x.IsAgrupado = true);

                            if (listGrupoDiam.Count == 0) continue;

                            AdministradorGrupos newAdministradorGrupos =
                                new AdministradorGrupos(_uiapp, listGrupoDiam[0].diametroInt, listGrupoDiam[0].largoFoot, listGrupoDiam[0].P2.Z, listGrupoDiam, confBarraTag);
                            ListAdministradorGrupos.Add(newAdministradorGrupos);
                        }
                    }
                }
                 
                /*
                var listaAgrupada = ListBarraIng
                                .GroupBy(c => new { c.largoFoot, c.diametroInt, c.P2.Z })
                                .OrderBy(g => g.Key.Z);

                foreach (var itemKey in listaAgrupada)
                {
                    List<BarraIng> list = itemKey.OrderBy(c => c.distaciaDesdeOrigen).ToList();
                    AdministradorGrupos newAdministradorGrupos = new AdministradorGrupos(_uiapp, itemKey.Key.diametroInt, itemKey.Key.largoFoot, itemKey.Key.Z, list);
                    ListAdministradorGrupos.Add(newAdministradorGrupos);
                }
                */
            }
            catch (Exception)
            {
                IsOK = false; ;
            }
        }

        public void DibujarTagDeGrupoVertical()
        {
            GenerarNuevaDirectizDTO _generarNuevaDirectizDTO = ObtenerGenerarNuevaDirectizDTO();
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Agrupar directriz-NH");
                    foreach (AdministradorGrupos administradorGrupos in ListAdministradorGrupos)
                    {
                        //agregar tag
                        administradorGrupos.M1_ObtenerDatosPreDibujo(_generarNuevaDirectizDTO);
                        administradorGrupos.M4_ObtenerPuntoInsercionTagVertical();
                        administradorGrupos.M2_GenerarNuevaDirectrizVertical();
                        //agregar contenedor hay que agrupar
                        //ContendorRebar ContendorRebar = new ContendorRebar(_uiapp);
                        //Element muroVigaContanedor = _doc.GetElement(administradorGrupos.ListaBarraIng[0]._Rebar.GetHostId());
                        //ContendorRebar.CrearContenedorSinTRAS(muroVigaContanedor, administradorGrupos.ListaBarraIng.Select(c => c._Rebar).ToList());
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }

        public void AgruparHorizontal()
        {
            IsOK = true;
            try
            {
                double DeltaLargo = Util.CmToFoot(2);
                double DeltaZ = Util.CmToFoot(2);
                var listaAgrupadaDiamtro = ListBarraIng
                                   .GroupBy(c => new { c.diametroInt })
                                   .OrderByDescending(g => g.Key.diametroInt);



                var confBarraTag = ConfiguracionTAgBarraDTo.OBtenercasoHorizontal(-_view.RightDirection);

                foreach (var itemKey in listaAgrupadaDiamtro)
                {
                    List<BarraIng> listDiam = itemKey.ToList();


                    //analzia agupado por diametro
                    for (int i = 0; i < listDiam.Count; i++)
                    {
                        //analiza caso puntoal
                        List<BarraIng> listGrupoDiam = new List<BarraIng>();
                        for (int j = 0; j < listDiam.Count; j++)
                        {
                            BarraIng BarraIngAnal = listDiam[j];
                            if (BarraIngAnal.IsAgrupado) continue;

                            listGrupoDiam = listDiam.Where(c => c.IsAgrupado == false &&
                                                    Util.IsSimilarValor(c.largoFoot, BarraIngAnal.largoFoot, DeltaLargo)).ToList();

                            listGrupoDiam.ForEach(x => x.IsAgrupado = true);

                            if (listGrupoDiam.Count == 0) continue;

                     

                            AdministradorGrupos newAdministradorGrupos =
                                new AdministradorGrupos(_uiapp, listGrupoDiam[0].diametroInt, listGrupoDiam[0].largoFoot, listGrupoDiam[0].P2.Z, listGrupoDiam, confBarraTag);
                            ListAdministradorGrupos.Add(newAdministradorGrupos);
                        }
                    }
                }
            }
            catch (Exception)
            {
                IsOK = false; ;
            }
        }

        public void DibujarTagDeGrupoHorizontal()
        {
            GenerarNuevaDirectizDTO _generarNuevaDirectizDTO = ObtenerGenerarNuevaDirectizDTO();
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Agrupar directriz-NH");
                    foreach (AdministradorGrupos administradorGrupos in ListAdministradorGrupos)
                    {
                        //agregar tag
                        administradorGrupos.M1_ObtenerDatosPreDibujo(_generarNuevaDirectizDTO);
                        //administradorGrupos.M3_ObtenerPuntoInsercionTagVertical();
                        administradorGrupos.M3_GenerarNuevaDirectrizHorizontal();
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }
    }
}
