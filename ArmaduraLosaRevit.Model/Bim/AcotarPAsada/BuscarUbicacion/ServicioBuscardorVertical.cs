using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion
{
    internal class ServicioBuscardorVertical : ServicioBuscardorBase
    {
        public bool IsSeguir { get; set; }

        public ServicioBuscardorVertical(UIApplication uiapp) : base(uiapp)
        {
            IsSeguir = false;
        }
        public void Resetear()
        {

        }



        public bool M1_Buscar_comenzarIzq(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.Largo < itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.Largo) //si superior es menos de 100 partir 'DERE_SUP'
                {
                    // **lado izq
                    if (M1_1_LAdoIzq_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_inf; }
                    else if (M1_2_LAdoIzq_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup; }
                    //***********lado derechop
                    else if (M1_3_LAdoDere_DImIZqIf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_inf; }
                    else if (M1_4_LAdoDere_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    {
                        itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_sup;
                    }
                    else //CASO FINAL
                    {
                        if (itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ == EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup;

                            itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = false;
                            itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = true;
                        }
                        else if (itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ == EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_inf;

                            itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = true;
                            itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = false;
                        }

                        itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaDere.IsOk = false;

                        itemSelect.CaraPasadaIzq.IsOk = true;
                    }
                }
                else // if (itemSelect.CaraPasadaIzq.Dimension_DereSUp.Largo < Util.CmToFoot(100)) //si superior es menos de 100 partir 'DERE_SUP'
                {
                    if (M1_2_LAdoIzq_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup; }
                    else if (M1_1_LAdoIzq_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    {
                        itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_inf;
                    }
                    //***********lado derechop
                    else if (M1_4_LAdoDere_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    {
                        itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_sup;
                    }
                    else if (M1_3_LAdoDere_DImIZqIf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_inf; }
                    else
                    {
                        if (itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ != EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup;

                            itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = false;
                            itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = true;
                        }
                        else if (itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ != EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_inf;

                            itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = true;
                            itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = false;
                        }

                        itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaIzq.IsOk = false;

                        IsSeguir = true;
                        itemSelect.CaraPasadaIzq.IsOk = true;

                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_Buscar_comenzarIzq'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_1_LAdoIzq_DimIzqInf(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                         !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf, ListaPAsadasCercanas))
                {
                    itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    //itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_inf;

                    itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaDere.IsOk = false;
                    //eleige cara izq
                    itemSelect.CaraPasadaIzq.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_1_LAdoIzq_DimIzqInf'. ex:{ex.Message}");
                return false;
            }
        }
        public bool M1_2_LAdoIzq_DImDereSup(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                           !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaIzq.Dimension_LadoDereSup, ListaPAsadasCercanas))
                {
                    itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    //itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup;

                    itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaDere.IsOk = false;
                    //eleige cara izq
                    itemSelect.CaraPasadaIzq.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_2_LAdoIzq_DImDereSup'. ex:{ex.Message}");
                return false;
            }
        }
        public bool M1_3_LAdoDere_DImIZqIf(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                    !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaDere.Dimension_LadoIzqInf, ListaPAsadasCercanas))
                {

                    itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    //itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_inf;

                    itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaIzq.IsOk = false;

                    itemSelect.CaraPasadaDere.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_3_LAdoDere_DImIZqIf'. ex:{ex.Message}");
                return false;
            }
        }
        public bool M1_4_LAdoDere_DImDereSup(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {
                if (itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ == EstadoIteracion.PermitidoIterar &&
                    !M0_1_BuscarInterseccion_general(itemSelect.CaraPasadaDere.Dimension_LadoDereSup, ListaPAsadasCercanas))
                {
                    itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                    // itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_sup;

                    itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                    itemSelect.CaraPasadaIzq.IsOk = false;

                    itemSelect.CaraPasadaDere.IsOk = true;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_4_LAdoDere_DImDereSup'. ex:{ex.Message}");
                return false;
            }
        }

        //*********

        public bool Buscar_comenzarDere(PasadasParaUbicDimensiones itemSelect, List<PasadasParaUbicDimensiones> ListaPAsadasCercanas)
        {
            try
            {

                if (itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.Largo < itemSelect.CaraPasadaDere.Dimension_LadoDereSup.Largo) //si superior es menos de 100 partir 'DERE_SUP'
                {
                    //***********lado derechop
                    if (M1_3_LAdoDere_DImIZqIf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_inf; }
                    else if (M1_4_LAdoDere_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    {
                        itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_sup;
                    }
                    // **lado izq
                    else if (M1_1_LAdoIzq_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_inf; }
                    else if (M1_2_LAdoIzq_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup; }
                    else //CASO FINAL
                    {
                        if (itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ != EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_sup;
                            itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = false;
                            itemSelect.CaraPasadaDere.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = true;
                        }
                        else if (itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ != EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_inf;
                            itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = true;
                            itemSelect.CaraPasadaDere.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = false;
                        }

                        itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaIzq.IsOk = false;

                        IsSeguir = true;
                        itemSelect.CaraPasadaDere.IsOk = true;
                    }
                }
                else // if (itemSelect.CaraPasadaIzq.Dimension_DereSUp.Largo < Util.CmToFoot(100)) //si superior es menos de 100 partir 'DERE_SUP'
                {
                    
                    if (M1_4_LAdoDere_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    {
                        itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_sup;
                    }
                    else if (M1_3_LAdoDere_DImIZqIf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_inf; }
                    else if (M1_2_LAdoIzq_DImDereSup(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup; }
                    else if (M1_1_LAdoIzq_DimIzqInf(itemSelect, ListaPAsadasCercanas))
                    { itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_inf; }
                    //***********lado derechop

                    else
                    {
                        if (itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.EstadoIteracion_ != EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_sup;

                            itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = false;
                            itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = true;
                        }
                        else if (itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ != EstadoIteracion.PermitidoIterar)
                        {
                            itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Derecha_inf;
                            itemSelect.CaraPasadaIzq.Dimension_LadoIzqInf.IsDIbujarRectaguloPorInterseccion = true;
                            itemSelect.CaraPasadaIzq.Dimension_LadoDereSup.IsDIbujarRectaguloPorInterseccion = false;
                        }

                        itemSelect.CaraPasadaDere.Dimension_LadoDereSup.EstadoIteracion_ = EstadoIteracion.NoIterrar;
                        itemSelect.CaraPasadaDere.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;

                        IsSeguir = true;
                        itemSelect.CaraPasadaIzq.IsOk = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Buscar_comenzarDere'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


    }
}
