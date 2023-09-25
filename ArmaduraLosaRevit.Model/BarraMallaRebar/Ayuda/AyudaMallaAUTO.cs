using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar.Ayuda
{
    public class AyudaMallaAUTO
    {

        public static ConfiguracionIniciaWPFlBarraVerticalDTO ObtenerConfiguracionIniciaWPF(Document _doc, CasoAnalisasBarrasElevacion _CasoAnalisasBarrasElevacion)
        {


            ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO = new ConfiguracionIniciaWPFlBarraVerticalDTO()
            {
                Inicial_Cantidadbarra = ObtenerNUmeroMallas("E.D."),//tipo_mallaV.Text   pq hay dos mallas si fueran 3 mallas serian  '2+2+2', 
                incial_ComoIniciarTraslapo_LineaPAr = 1,
                incial_ComoIniciarTraslapo_LineaImpar = 1,//barra incio
                inicial_ComoTraslapo = 1,
                inicial_diametroMM = 8,//Util.ConvertirStringInInteger(ui_Malla.diam_mallaV.Text),
                Document_ = _doc,
                inicial_tipoBarraV = Enumeraciones.TipoPataBarra.BarraVSinPatas,
                CasoAnalisasBarrasElevacion_ = _CasoAnalisasBarrasElevacion,
                inicial_IsDirectriz = false,
                inicial_ISIntercalar = false,
                Inicial_espacienmietoCm_EntreLineasBarras = "20",// ui_Malla.espa_mallaV.Text,
                IsDibujarTag = false,
                TipoBarraRebar_ = TipoBarraVertical.MallaV,
                TipoBarraRebarHorizontal_ = TipoBarraVertical.MallaH,
                TipoSelecion =TipoSeleccion.ConMouse //se ocupa esta seleccio pq es la que siempre se utilizo
            };

            return confiWPFEnfierradoDTO;
        }

        public static DatosMallasAutoDTO ObtenerDatosMallasDTO()
        {
            DatosMallasAutoDTO datosMallasDTO = new DatosMallasAutoDTO()
            {
                diametroH = 8,
                diametroV = 8,
                paraCantidadLineasV = ObtenerNUmeroMallas("E.D."),//tipo_mallaV.Text
                paraCantidadLineasH = ObtenerNUmeroMallas("E.D."),
                espaciemientoH = 20,
                espaciemientoV = 20,
                tipoMallaH = ObtenerTipo("E.D."),
                tipoMallaV = ObtenerTipo("E.D."),//tipo_mallaV.Text
                tipoSeleccionInf = TipoSeleccionMouse.nivel,
                tipoSeleccionSup = TipoSeleccionMouse.nivel
            };

            return datosMallasDTO;
        }

        public static string ObtenerNUmeroMallas(string txt)
        {
            switch (txt)
            {
                case "E.":
                    return "2";
                case "E.D.":
                    return "2+2";
                case "E.T.":
                    return "2+2+2";
                case "E.C.":
                    return "2+2+2+2";
                default:
                    return "2";
            }
        }

        public static TipoMAllaMuro ObtenerTipo(string tipo)
        {
            switch (tipo)
            {
                case "E.":
                    return TipoMAllaMuro.SM;
                case "E.D.":
                    return TipoMAllaMuro.DM;

                case "E.T.":
                    return TipoMAllaMuro.TM;

                case "E.C.":
                    return TipoMAllaMuro.CM;
                default:
                    return TipoMAllaMuro.SM;
            }
        }
    }
}
