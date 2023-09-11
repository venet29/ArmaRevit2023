using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion
{
    internal class ServicioBuscardorHorizontal : ServicioBuscardorBase
    {
        public bool IsSeguir { get; set; }

        public ServicioBuscardorHorizontal(UIApplication uiapp) : base(uiapp)
        {
            IsSeguir = false;
        }
        public void Resetear()
        {

        }

        public bool M1_1_LadoArriba_DimIzqInf(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                         !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf, ListaPAsadasCercanas))
                {
                    itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    //itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Arriba_izq;

                    itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaBajo.IsOk = false;
                    //eleige cara izq
                    itemSelect.CaraPasadaArriba.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_1_LadoArriba_DimIzqInf'. ex:{ex.Message}");
                return false;
            }
        }

        public bool M1_2_LAdoArriba_DimDereSup(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                           !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaArriba.Dimension_LadoDereSup, ListaPAsadasCercanas))
                {
                    itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    //itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Arriba_dere;

                    itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaBajo.IsOk = false;
                    //eleige cara izq
                    itemSelect.CaraPasadaArriba.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_2_LAdoArriba_DimDereSup'. ex:{ex.Message}");
                return false;
            }
        }
        //**

        public bool M1_3_LAdoBajo_DimIzqInf(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                    !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf, ListaPAsadasCercanas))
                {

                    itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    // itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Bajo_izq;

                    itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaArriba.IsOk = false;

                    itemSelect.CaraPasadaBajo.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_2_LAdoArriba_DimDereSup'. ex:{ex.Message}");
                return false;
            }
        }

        public bool M1_4_LAdoBajo_DimDereSup(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                    !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaBajo.Dimension_LadoDereSup, ListaPAsadasCercanas))
                {
                    itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    //itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Bajo_dere;

                    itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaArriba.IsOk = false;

                    itemSelect.CaraPasadaBajo.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_4_LAdoBajo_DimDereSup'. ex:{ex.Message}");
                return false;
            }
        }



        public bool Buscar_comenzarArriba(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.Largo < itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.Largo) //si superior es menos de 100 partir 'DERE_SUP'
                {
                    //caso cara arriba  bajo
                    if (M1_1_LadoArriba_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_izq; }
                    else if (M1_2_LAdoArriba_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere; }
                    // **lado izq
                    else if (M1_3_LAdoBajo_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_izq; }
                    else if (M1_4_LAdoBajo_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_dere; }
                    else
                    {
                        IsSeguir = true;

                        itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere;

                        itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.PermitidoIterar;
                        itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                        itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaBajo.IsOk = false;

                        itemSelect.CaraPasadaArriba.IsOk = true;
                    }
                }
                else
                {
                    if (M1_2_LAdoArriba_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere; }
                    else if (M1_1_LadoArriba_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_izq; }

                    else if (M1_4_LAdoBajo_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_dere; }
                    // **lado izq
                    else if (M1_3_LAdoBajo_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_izq; }
                    else
                    {
                        itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere;

                        itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.PermitidoIterar; // esto es pq puede aparecer desactivado
                        itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        
                        itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaBajo.IsOk = false;

                        IsSeguir = true;
                        itemSelect.CaraPasadaArriba.IsOk = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Buscar_comenzarArriba'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        //**************************************************************** BAJO

        public bool Buscar_comenzarBAJO(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.Largo < itemSelect. CaraPasadaBajo.Dimension_LadoDereSup.Largo) //si superior es menos de 100 partir 'DERE_SUP'
                {
                    if (M1_3_LAdoBajo_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_izq; }
                    else if (M1_4_LAdoBajo_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_dere; }
                    //caso cara arriba  bajo
                    else if (M1_1_LadoArriba_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_izq; }
                    else if (M1_2_LAdoArriba_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere; }
                    // **lado izq

                    else
                    {
                        IsSeguir = true;

                        itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere;

                        
                        itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.PermitidoIterar  ;// esto pq previamete puede estar  desactualizado
                        itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;


                        itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaArriba.IsOk = false;


                        itemSelect.CaraPasadaBajo.IsOk = true;
                    }
                }
                else
                {


                    if (M1_4_LAdoBajo_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_dere; }
                    // **lado izq
                    else if (M1_3_LAdoBajo_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_izq; }
                    else if (M1_2_LAdoArriba_DimDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere; }
                    else if (M1_1_LadoArriba_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_izq; }

                    else
                    {
                        itemSelect.EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Arriba_dere;

                        itemSelect.CaraPasadaBajo.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaBajo.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.PermitidoIterar;

                        //itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Bajo_dere;

                        itemSelect.CaraPasadaArriba.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaArriba.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaArriba.IsOk = false;

                        IsSeguir = true;
                        itemSelect.CaraPasadaBajo.IsOk = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Buscar_comenzarBAJO'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
