using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ahorro
{
    public class ConfiguracionAhorro
    {
        private double largoAhorro_recorrido;
        private double largoAhorro_barra;
        private double largominimo;
#pragma warning disable CS0169 // The field 'ConfiguracionAhorro.v' is never used
        private string v;
#pragma warning restore CS0169 // The field 'ConfiguracionAhorro.v' is never used

        public string remplazoF1 { get; set; }
        public string remplazoF3 { get; set; }
        public string remplazoF11 { get; set; }

        public string remplazosx { get; set; }
        public TipoDiseño_F1 DISENO_TIPO_F1 { get; set; }
        public TipoValidarEspesor DISENO_VALIDAR_ESPESOR { get; set; }

        public ConfiguracionAhorro(bool IsCOnAhorro, double largoAhorro_recorrido, double largoAhorro_barra, double largominimo, bool _VerificarEspesorMenos15, string tipoF1,
                                    string remplazoF1 = "f20", string remplazoF3 = "f16", string remplazoF11 = "f17"
                                   )
        {
            this.largoAhorro_recorrido = largoAhorro_recorrido;
            this.largoAhorro_barra = largoAhorro_barra;
            this.largominimo = largominimo;
            this.remplazoF1 = remplazoF1;
            this.remplazoF3 = remplazoF3;
            this.remplazoF11 = remplazoF11;
            this.remplazosx = "s1";
            this.DISENO_VALIDAR_ESPESOR = (_VerificarEspesorMenos15 ? TipoValidarEspesor.VerificarEspesorMenor15 : TipoValidarEspesor.NOVerificarEspesorMenor15);
            if (IsCOnAhorro)
                this.DISENO_TIPO_F1 = (tipoF1.ToLower() == "f3" ? TipoDiseño_F1.f1_sup_conAhorro : TipoDiseño_F1.f1_conAhorro);
            else
                this.DISENO_TIPO_F1 = (tipoF1.ToLower() == "f3" ? TipoDiseño_F1.f1_sup : TipoDiseño_F1.f1);
        }

        public ConfiguracionAhorro()
        {
        }

        public ConfiguracionAhorro(string sx)
        {
            this.remplazosx = sx;
        }

        public SolicitudBarraDTO VerificarDiseñoConAHorro(SolicitudBarraDTO solicitudBarraDTO, List<XYZ> listaPtosPerimetroBarras)
        {
            if (largoAhorro_barra > listaPtosPerimetroBarras[1].DistanceTo(listaPtosPerimetroBarras[2]) ||
                largoAhorro_recorrido > listaPtosPerimetroBarras[3].DistanceTo(listaPtosPerimetroBarras[2]))
            {

                if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_sup_conAhorro)
                {
                    
                    if (solicitudBarraDTO.TipoBarra.ToLower() == "f16")
                    { solicitudBarraDTO.TipoBarra = "f3"; }
                    else if (solicitudBarraDTO.TipoBarra.ToLower() == "f18")
                    { solicitudBarraDTO.TipoBarra = "f11"; }
                    else if (solicitudBarraDTO.TipoBarra.ToLower() == "f17")
                    { solicitudBarraDTO.TipoBarra = "f11a"; }
                }
                else if (DatosDiseño.DISENO_TIPO_F1 == TipoDiseño_F1.f1_conAhorro)
                {
                    if (solicitudBarraDTO.TipoBarra.ToLower() == "f19")
                    { solicitudBarraDTO.TipoBarra = "f4"; }
                    else if (solicitudBarraDTO.TipoBarra.ToLower() == "f20" || solicitudBarraDTO.TipoBarra.ToLower() == "f21")
                    { solicitudBarraDTO.TipoBarra = "f1"; }
                    else if (solicitudBarraDTO.TipoBarra.ToLower() == "f16")
                    { solicitudBarraDTO.TipoBarra = "f3"; }
                }
            }
            else
            {

            }
            return solicitudBarraDTO;
        }


    
    internal SolicitudBarraDTO VerificarDiseñoConAHorrov2(SolicitudBarraDTO solicitudBarraDTO, List<XYZ> listaPtosPerimetroBarras)
    {
        if (largoAhorro_barra < listaPtosPerimetroBarras[1].DistanceTo(listaPtosPerimetroBarras[2]) &&
            largoAhorro_recorrido < listaPtosPerimetroBarras[3].DistanceTo(listaPtosPerimetroBarras[2]))
        {

            if (solicitudBarraDTO.TipoBarra == "f1")
            {
                solicitudBarraDTO.TipoBarra = remplazoF1;
            }
            else if (solicitudBarraDTO.TipoBarra == "f3")
            {
                solicitudBarraDTO.TipoBarra = remplazoF3;
            }
            else if (solicitudBarraDTO.TipoBarra == "f4")
            {
                solicitudBarraDTO.TipoBarra = remplazoF11;
                switch (solicitudBarraDTO.UbicacionEnlosa)
                {
                    case Enumeraciones.UbicacionLosa.Derecha:
                        solicitudBarraDTO.UbicacionEnlosa = Enumeraciones.UbicacionLosa.Izquierda;
                        break;
                    case Enumeraciones.UbicacionLosa.Izquierda:
                        solicitudBarraDTO.UbicacionEnlosa = Enumeraciones.UbicacionLosa.Derecha;
                        break;
                    case Enumeraciones.UbicacionLosa.Superior:
                        solicitudBarraDTO.UbicacionEnlosa = Enumeraciones.UbicacionLosa.Inferior;
                        break;
                    case Enumeraciones.UbicacionLosa.Inferior:
                        solicitudBarraDTO.UbicacionEnlosa = Enumeraciones.UbicacionLosa.Superior;
                        break;
                    case Enumeraciones.UbicacionLosa.NONE:
                        break;
                    default:
                        break;
                }
            }
        }

        return solicitudBarraDTO;
    }
    }

}