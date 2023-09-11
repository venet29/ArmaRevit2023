using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion
{
    public class ServicioBuscardorBase
    {
        protected UIApplication _uiapp;

        public ServicioBuscardorBase(UIApplication uiapp)
        {
            this._uiapp = uiapp;
        }


        protected bool M0_1_BuscarInterseccion_general(DimensionParaUbicDimensiones dimension, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {

            try
            {
                //1) lado  izq o inferior

                if (dimension.IsOk)
                {
                    var listaPuntos = dimension.ListaPuntos;

                    for (int i = 0; i < ListaPAsadasCercanas.Count; i++)
                    {
                        var pasadaAnalizada = ListaPAsadasCercanas[i];

                        //casi izq inferiro
                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaBajo))
                        {
                            dimension.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }
                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaArriba))
                        {
                            dimension.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }

                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaIzq))
                        {
                            dimension.IsIntersectado = TipoInterseccion.Intersectado; ;
                            return true;
                        }

                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaDere))
                        {
                            dimension.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }

                    }

                    dimension.IsIntersectado = TipoInterseccion.SinInterseccion;
                    //caraPasadaAnalizada.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M0_1_BuscarInterseccion_general'. ex:{ex.Message}");
                return true;
            }

            return false;
        }

        protected bool M0_1_BuscarInterseccion_IzqInf(PlanoParaUbicDimensiones caraPasadaAnalizada, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {

            try
            {
                //1) lado  izq o inferior

                if (caraPasadaAnalizada.Dimension_LadoIzqInf.IsOk)
                {
                    var listaPuntos = caraPasadaAnalizada.Dimension_LadoIzqInf.ListaPuntos;

                    for (int i = 0; i < ListaPAsadasCercanas.Count; i++)
                    {
                        var pasadaAnalizada = ListaPAsadasCercanas[i];

                        //casi izq inferiro
                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaBajo))
                        {
                            caraPasadaAnalizada.Dimension_LadoIzqInf.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }
                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaArriba))
                        {
                            caraPasadaAnalizada.Dimension_LadoIzqInf.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }

                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaIzq))
                        {
                            caraPasadaAnalizada.Dimension_LadoIzqInf.IsIntersectado = TipoInterseccion.Intersectado; ;
                            return true;
                        }

                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaDere))
                        {
                            caraPasadaAnalizada.Dimension_LadoIzqInf.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }

                    }

                    caraPasadaAnalizada.Dimension_LadoIzqInf.IsIntersectado = TipoInterseccion.SinInterseccion;
                    //caraPasadaAnalizada.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M0_1_BuscarInterseccion_IzqInf'. ex:{ex.Message}");
                return true;
            }

            return false;
        }

        protected bool M0_1_BuscarInterseccion_DereSUp(PlanoParaUbicDimensiones caraPasadaAnalizada, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {

            try
            {
                //***********************************************************
                //2) lado  izq o inferior

                if (caraPasadaAnalizada.Dimension_LadoDereSup.IsOk)
                {
                    var listaPuntos = caraPasadaAnalizada.Dimension_LadoDereSup.ListaPuntos;

                    for (int i = 0; i < ListaPAsadasCercanas.Count; i++)
                    {
                        var pasadaAnalizada = ListaPAsadasCercanas[i];

                        //casi izq inferiro
                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaBajo))
                        {
                            caraPasadaAnalizada.Dimension_LadoDereSup.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }

                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaArriba))
                        {
                            caraPasadaAnalizada.Dimension_LadoDereSup.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }

                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaIzq))
                        {
                            caraPasadaAnalizada.Dimension_LadoDereSup.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }

                        if (M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(listaPuntos, pasadaAnalizada.CaraPasadaDere))
                        {
                            caraPasadaAnalizada.Dimension_LadoDereSup.IsIntersectado = TipoInterseccion.Intersectado;
                            return true;
                        }
                    }

                    // if (!caraPasadaAnalizada.Dimension_DereSUp.IsIntersectado)
                    caraPasadaAnalizada.Dimension_LadoDereSup.IsIntersectado = TipoInterseccion.SinInterseccion;
                    //caraPasadaAnalizada.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M0_1_BuscarInterseccion_DereSUp'. ex:{ex.Message}");
                return true;
            }

            return true;
        }


        protected static bool M0_1_1_BuscarInterseccionPOrCara_enAmbosLados(List<XYZ> listaPuntos, PlanoParaUbicDimensiones CaraPasada)
        {
            var listaPToIzqInf = CaraPasada.Dimension_LadoIzqInf.ListaPuntos;
            if (CaraPasada.Dimension_LadoIzqInf.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                IsInterseccion.IsInterseccionPoligonos_XY0(listaPuntos, listaPToIzqInf)) return true;

            //caso dere superio

            var listaPToDereSup = CaraPasada.Dimension_LadoDereSup.ListaPuntos;
            if (CaraPasada.Dimension_LadoDereSup.EstadoIteracion_ == EstadoIteracion.PermitidoIterar && 
                IsInterseccion.IsInterseccionPoligonos_XY0(listaPuntos, listaPToDereSup)) return true;

            return false;
        }
    }
}
