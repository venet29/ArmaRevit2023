using ArmaduraLosaRevit.Model.BarraMallaRebar.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Intervalos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public class FactoryGenerarIntervalos
    {

        public static IGenerarIntervalosV CrearGeneradorDeIntervalosV(UIApplication uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            if (selecionarPtoSup.ListaLevelIntervalo == null)
                return new GenerarIntervalosNULL(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);


            // horquilla
            //if(confiWPFEnfierradoDTO.TipoBarraRebar_== TipoBarraVertical.Cabeza_Horquilla)
            //    return new GenerarIntervalos1Nivel_Cabeza_Horq(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);

            // elev sin traslapo
            if (confiWPFEnfierradoDTO.inicial_ComoTraslapo == 0)
            {
                selecionarPtoSup.ListaLevelIntervalo.Clear();
                selecionarPtoSup.ListaLevelIntervalo.Add(selecionarPtoSup._PtoInicioIntervaloBarra.Z);
                selecionarPtoSup.ListaLevelIntervalo.Add(selecionarPtoSup._PtoFinalIntervaloBarra.Z);
                return new GenerarIntervalos1Nivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
            }

            //elev
            switch (selecionarPtoSup.ListaLevelIntervalo.Count)
            {
                case 2:
                    {                    //mallas
                        if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH)
                            return new GenerarIntervalos1Nivel_MuroH(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        else if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV)
                            return new GenerarIntervalos1Nivel_MuroV(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        else
                            return new GenerarIntervalos1Nivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                    }
                case 3:
                    return new GenerarIntervalos2Nivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                case 4:
                    return new GenerarIntervalos3Nivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                default:
                    return new GenerarIntervalosVariosNivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
            }

        }


        public static IGenerarIntervalosV CrearGeneradorDeIntervalosV_mallas(UIApplication uiapp, ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            if (selecionarPtoSup.ListaLevelIntervalo == null)
                return new GenerarIntervalosNULL(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);


            //elev
            switch (selecionarPtoSup.ListaLevelIntervalo.Count)
            {

                case 2:
                    {                    //mallas
                        if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH)
                            return new GenerarIntervalos1Nivel_MuroH(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        else if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH_Horq)
                        { 
                            // no implementados
                            return new GenerarIntervalos1Nivel_Muro_Horq(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        }
                        else if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.Cabeza_Horquilla)
                            return new GenerarIntervalos1Nivel_Cabeza_Horq(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        else //if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV)
                            return new GenerarIntervalos1Nivel_MuroV(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                    }
                case 3:
                    {                    //mallas
                        if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH)
                        { } //  falta implementar para varios nivels--> return new GenerarIntervalos1Nivel_MuroH(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        else// if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV)
                        { }   //  falta implementar para varios nivels--> return new GenerarIntervalos1Nivel_MuroV(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);

                        return new GenerarIntervalos2Nivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                    }

                case 4:
                    {                    //mallas
                        if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH)
                        { } //  falta implementar para varios nivels--> return new GenerarIntervalos1Nivel_MuroH(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        else //if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV)
                        { }   //  falta implementar para varios nivels--> return new GenerarIntervalos1Nivel_MuroV(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
  
                        return new GenerarIntervalos3Nivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                    }

                default:
                    {                    //mallas
                        if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaH)
                        { } //  falta implementar para varios nivels--> return new GenerarIntervalos1Nivel_MuroH(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        else// if (confiWPFEnfierradoDTO.TipoBarraRebar_ == TipoBarraVertical.MallaV)
                        { }   //  falta implementar para varios nivels--> return new GenerarIntervalos1Nivel_MuroV(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                        
                        return new GenerarIntervalosVariosNivel(uiapp, confiWPFEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
                    }
            }

        }

  

        public static IGenerarIntervalosH CrearGeneradorDeIntervalosH(UIApplication uiapp,
            ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO,
            SelecionarPtoHorizontal selecionarPtoSup,
            DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {

            return new GenerarIntervalosSINNivel(uiapp, confiEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
        }


        public static IGenerarIntervalosH CrearGeneradorDeIntervalosH_RefuerzoVIga(UIApplication uiapp,
       ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO,
       SelecionarPtoHorizontal selecionarPtoSup,
       DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {

            return new GenerarIntervalosSINNivel_RefuerzoVIga(uiapp, confiEnfierradoDTO, selecionarPtoSup, muroSeleccionadoDTO);
        }

        public static IGenerarIntervalosV CrearGeneradorDeIntervalosVAUTO(UIApplication uiapp,
            ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO,
             IntervalosBarraAutoDto newIntervaloBarraAutoDto,
            DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {


            if (confiEnfierradoDTO.ListaIntervaloBarraAutoDto.Count > 0)
            { return new GenerarIntervalosAuto(uiapp, confiEnfierradoDTO, newIntervaloBarraAutoDto, muroSeleccionadoDTO); }
            else
            { return new GenerarIntervalosNULL(uiapp, confiEnfierradoDTO, null, muroSeleccionadoDTO); }

        }

    }
}
