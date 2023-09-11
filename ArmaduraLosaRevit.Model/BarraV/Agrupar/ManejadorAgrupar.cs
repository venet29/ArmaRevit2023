using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.ServicioAgrupar;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.model;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar
{
    public class ManejadorAgrupar
    {
        private UIApplication _uiapp;
        private readonly List<Level> _listaLevel;
        private Document _doc;
        private View _view;
        private int _escale;
        private List<AgrupadorBarrasAuto> ListaAgrupadorBarrasAutomatico;

        public ManejadorAgrupar(UIApplication uiapp, List<Level> listaLevel)//para atuomatico
        {
            this._uiapp = uiapp;
            this._listaLevel = listaLevel;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._escale = _view.Scale;
        }

        public ManejadorAgrupar(UIApplication uiapp) //para manual
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._escale = _view.Scale;
  

        }
        public Result M1_EjecutarVerticales()

        {
            try
            {

                AlmacenerNiveles.ListaNiveles = new List<double>();
                AlmacenerNiveles.UltimoPtoTag = XYZ.Zero;

                SeleccionarTagRebarVerticales seleccionarTagRebarVerticales = new SeleccionarTagRebarVerticales(_uiapp);
                if (!seleccionarTagRebarVerticales.M1_EjecutarVertical()) return Result.Failed;


                AgrupadorBarrasManual AgrupadorBarrasManual = new AgrupadorBarrasManual(_uiapp, seleccionarTagRebarVerticales.ListIndependentTag);
                AgrupadorBarrasManual.GenerarListaBarras();//solo caso manual

                if (!AgrupadorBarrasManual.DireccionDeTagVertical(seleccionarTagRebarVerticales.ptoMouse.AsignarZ(seleccionarTagRebarVerticales.ptoMouse.Z - ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT))) //solo manual
                    return Result.Failed;
                else
                {
                    AgrupadorBarrasManual.AgruparVertical();
                    AgrupadorBarrasManual.DibujarTagDeGrupoVertical();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        public Result M2_EjecutarHorizontal()

        {
            try
            {

                AlmacenerNiveles.ListaNiveles = new List<double>();
                AlmacenerNiveles.UltimoPtoTag = XYZ.Zero;

                SeleccionarTagRebarVerticales seleccionarTagRebarVerticales = new SeleccionarTagRebarVerticales(_uiapp);
                if (!seleccionarTagRebarVerticales.EjecutarHorizontal()) return Result.Failed;


                AgrupadorBarrasManual AgrupadorBarrasManual = new AgrupadorBarrasManual(_uiapp, seleccionarTagRebarVerticales.ListIndependentTag);
                AgrupadorBarrasManual.GenerarListaBarras();//solo caso manual

                if (!AgrupadorBarrasManual.DireccionDeTagHorizontal(seleccionarTagRebarVerticales.ptoMouse)) //solo manual
                    return Result.Failed;
                else
                {
                    AgrupadorBarrasManual.AgruparHorizontal();
                    AgrupadorBarrasManual.DibujarTagDeGrupoHorizontal();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        public bool M3_EjecutarAutomatico(List<BarraIng> ListBarraIng)
        {
            try
            {
                ListaAgrupadorBarrasAutomatico = new List<AgrupadorBarrasAuto>();
                var Grupos_BArrasPorPierYOrientacion = ListBarraIng
                             .GroupBy(c => new { c.Pier, c.orientacion }).ToList();

                foreach (var Grupo_BArrasPorPierYOrientacion in Grupos_BArrasPorPierYOrientacion)
                {
                    List<BarraIng> Lista_BArrasPorPierYOrientacion = Grupo_BArrasPorPierYOrientacion.Where(c => c.IsOk).ToList();
                    if (Lista_BArrasPorPierYOrientacion.Count == 0) continue;
                    //Orientacion orientacion = itemGrup.Key.orientacion;


                    AlmacenerNiveles.ListaNiveles = new List<double>();
                    AlmacenerNiveles.UltimoPtoTag = XYZ.Zero;

                    // agrupar por pto de partida
                    var Gruopo_BarrasPorPtoPartida_PierYOrientacion = Lista_BArrasPorPierYOrientacion
                                          .GroupBy(c => new { c.P2.Z, c.largoFoot })
                                          .OrderBy(g => g.Key.Z);

                    // aqui el error
                    foreach (var BarrasPorPtoPartida_PierYOrientacion in Gruopo_BarrasPorPtoPartida_PierYOrientacion)
                    {

                        List<BarraIng> _ListaIBarrasPorPtoPartida_PierYOrientacion = BarrasPorPtoPartida_PierYOrientacion.ToList();

                        Orientacion _OrientacionBarrasPorPtoPartida_PierYOrientacion = Lista_BArrasPorPierYOrientacion[0].OrientacionTagGrupoBarras;
                        //XYZ _ptoUbicacionTag = AyudaGruposBarrasIguaoP2.ObtenerPtoInsersiontag( _ListaIBarrasPorPtoPartida_PierYOrientacion) +
                        //                (_OrientacionBarrasPorPtoPartida_PierYOrientacion == Orientacion.izquierda ? -1 * Util.CmToFoot(35) : Util.CmToFoot(50)) * _view.RightDirection;

                        XYZ _ptoUbicacionTag = AyudaGruposBarrasIguaoP2.ObtenerPtoInsersiontag(_ListaIBarrasPorPtoPartida_PierYOrientacion, _view.RightDirection, _view.Scale);

                        AgrupadorBarrasAuto agrupadorBarrasPorLargoYDiametro = new AgrupadorBarrasAuto(_uiapp, _ListaIBarrasPorPtoPartida_PierYOrientacion, _ptoUbicacionTag,
                                                                                                        _OrientacionBarrasPorPtoPartida_PierYOrientacion);
                        agrupadorBarrasPorLargoYDiametro.M1_AgruparAuto();
                        ListaAgrupadorBarrasAutomatico.Add(agrupadorBarrasPorLargoYDiametro);
                    }
                }

                //ordenar tag por piso
                ListaAgrupadorBarrasAutomatico = ListaAgrupadorBarrasAutomatico.Where(c => c.IsOK == true).ToList();
                foreach (AgrupadorBarrasAuto itemPorPtoPartida in ListaAgrupadorBarrasAutomatico)
                {
                    itemPorPtoPartida.M2_ObtenerPtoInserccionPorPiso();
                }

                //reordenar coordenadas
                M3_a_ReaordenarPtoInserccion2Agrupar();
                //ReaordenarPtoInserccion();

                //dibujarTag
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Agrupar directriz auto-NH");
                    foreach (AgrupadorBarrasAuto itemPorPtoPartida in ListaAgrupadorBarrasAutomatico)
                    {
                        if (itemPorPtoPartida.IsOK)
                            itemPorPtoPartida.M3_DibujarTagDeGrupoAutomatico();
                    }
                    trans.Commit();
                }


            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }



        private void M3_a_ReaordenarPtoInserccion2Agrupar()
        {



            var listaPierYOrientacion = ListaAgrupadorBarrasAutomatico.GroupBy(c => new { c.Pier, c._DireccionSeleccionMouse });
            foreach (var itemGrup in listaPierYOrientacion)
            {
                List<AgrupadorBarrasAuto> ListaAgrupadorBarrasAutomatico_aux = itemGrup.ToList();
                Orientacion orientacion = itemGrup.Key._DireccionSeleccionMouse;



                for (int i = 0; i < _listaLevel.Count - 1; i++)
                {
                    try
                    {
                        if (ListaAgrupadorBarrasAutomatico_aux.Count == 0) continue;
                        List<AdministradorGrupos> ListaPorNivel = ListaAgrupadorBarrasAutomatico_aux
                                                                            .SelectMany(c => c.ListAdministradorGrupos)
                                                                            .Where(p => _listaLevel[i].ProjectElevation < p._ptoInsertar.Z && p._ptoInsertar.Z < _listaLevel[i + 1].ProjectElevation)
                                                                            .OrderByDescending(p => p.distanciaMasLejosDeBorde)
                                                                            .ToList();

                        if (ListaPorNivel.Count == 0) continue;
                        int NumeroLineas = ListaPorNivel.Count(c => c.NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale) * 2 + ListaPorNivel.Count(c => c.NombreFamiliaPrincipal == "MRA Rebar_SIN_" + _escale);

                        NumeroLineas = NumeroLineas - (ListaPorNivel.Last().NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale ? 1 : 0);// si barra mas cerca del borde usa dos lines se resta 1 para evitar obs1)

                        int contPosicionTexto = 0;
                        for (int j = 0; j < ListaPorNivel.Count; j++)
                        {

                            if (ListaPorNivel[j]._orientacion == Orientacion.izquierda)
                            {
                                ListaPorNivel[j]._ptoInsertar = ListaPorNivel[j]._ptoInsertar + new XYZ(0, 0, -Util.CmToFoot(20)) * (j) + -ListaPorNivel[j]._direccionEnfierrado * Util.CmToFoot(20) * (NumeroLineas - 1 - contPosicionTexto);

                                contPosicionTexto += (ListaPorNivel[j].NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale ? 2 : 1);
                            }
                            else
                            {
                                if (ListaPorNivel.Count > 1) contPosicionTexto += (ListaPorNivel[j].NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale ? 1 : 0);
                                ListaPorNivel[j]._ptoInsertar = ListaPorNivel[j]._ptoInsertar + new XYZ(0, 0, -Util.CmToFoot(20)) * (j) + -ListaPorNivel[j]._direccionEnfierrado * Util.CmToFoot(20) * (NumeroLineas - 1 - contPosicionTexto);

                                contPosicionTexto += 1;
                            }
                        }

                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ex :{ex.Message}");
                    }

                }
            }





        }

        private void ReaordenarPtoInserccion()
        {

            for (int i = 0; i < _listaLevel.Count - 1; i++)
            {
                try
                {
                    Orientacion[] direccion = new Orientacion[2] { Orientacion.izquierda, Orientacion.derecha };

                    foreach (Orientacion orientacion in direccion)// anazlizar por orientacion
                    {
                        List<AgrupadorBarrasAuto> ListaAgrupadorBarrasAutomatico_aux = ListaAgrupadorBarrasAutomatico.Where(c => c._DireccionSeleccionMouse == orientacion).ToList();
                        if (ListaAgrupadorBarrasAutomatico_aux.Count == 0) continue;
                        List<AdministradorGrupos> ListaPorNivel = ListaAgrupadorBarrasAutomatico_aux
                                                                            .SelectMany(c => c.ListAdministradorGrupos)
                                                                            .Where(p => _listaLevel[i].ProjectElevation < p._ptoInsertar.Z && p._ptoInsertar.Z < _listaLevel[i + 1].ProjectElevation)
                                                                            .OrderByDescending(p => p.distanciaMasLejosDeBorde)
                                                                            .ToList();

                        if (ListaPorNivel.Count == 0) continue;
                        int NumeroLineas = ListaPorNivel.Count(c => c.NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale) * 2 + ListaPorNivel.Count(c => c.NombreFamiliaPrincipal == "MRA Rebar_SIN_" + _escale);

                        NumeroLineas = NumeroLineas - (ListaPorNivel.Last().NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale ? 1 : 0);// si barra mas cerca del borde usa dos lines se resta 1 para evitar obs1)

                        int contPosicionTexto = 0;
                        for (int j = 0; j < ListaPorNivel.Count; j++)
                        {

                            if (ListaPorNivel[j]._orientacion == Orientacion.izquierda)
                            {
                                ListaPorNivel[j]._ptoInsertar = ListaPorNivel[j]._ptoInsertar + new XYZ(0, 0, -Util.CmToFoot(20)) * (j) + -ListaPorNivel[j]._direccionEnfierrado * Util.CmToFoot(20) * (NumeroLineas - 1 - contPosicionTexto);

                                contPosicionTexto += (ListaPorNivel[j].NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale ? 2 : 1);
                            }
                            else
                            {
                                if (ListaPorNivel.Count > 1) contPosicionTexto += (ListaPorNivel[j].NombreFamiliaPrincipal == "MRA Rebar_F_" + _escale ? 1 : 0);
                                ListaPorNivel[j]._ptoInsertar = ListaPorNivel[j]._ptoInsertar + new XYZ(0, 0, -Util.CmToFoot(20)) * (j) + -ListaPorNivel[j]._direccionEnfierrado * Util.CmToFoot(20) * (NumeroLineas - 1 - contPosicionTexto);

                                contPosicionTexto += 1;
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    Debug.WriteLine($"ex :{ex.Message}");
                }

            }
        }


    }
}
