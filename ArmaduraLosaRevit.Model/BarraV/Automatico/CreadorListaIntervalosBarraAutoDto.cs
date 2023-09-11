using ArmaduraLosaRevit.Model.BarraAreaPath;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.Creation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico
{
    public class CreadorListaIntervalosBarraAutoDto
    {
        private Autodesk.Revit.DB.Document  _doc;
        private readonly List<IntervalosBarraAutoDtoIMPORTAR> _listaIntervalosBarraAutoDtoIMPORTAR;
        private readonly ArmaduraTrasformada _armaduraTrasformada;


        public List<IntervalosBarraAutoDto> ListaIntervalosBarraAutoDto { get; set; }

        public List<IntervalosMallaDTOAuto> ListaIntervalosMAllaAutoDto { get; set; }
        public List<IntervalosConfinaDTOAuto> ListaIntervalosConfinaminetoAutoDto { get; set; }
        public List<IntervalosConfinaDTOAuto> ListaIntervalosEstriboMuroAutoDto { get; set; }

        public CreadorListaIntervalosBarraAutoDto( Autodesk.Revit.DB.Document doc, List<IntervalosBarraAutoDtoIMPORTAR> listaIntervalosBarraAutoDtoIMPORTAR, ArmaduraTrasformada armaduraTrasformada)
        {
            this._doc = doc;
            this._listaIntervalosBarraAutoDtoIMPORTAR = listaIntervalosBarraAutoDtoIMPORTAR;
            this._armaduraTrasformada = armaduraTrasformada;
            //barra
            ListaIntervalosBarraAutoDto = new List<IntervalosBarraAutoDto>();
            //malla
            ListaIntervalosMAllaAutoDto = new List<IntervalosMallaDTOAuto>();
            //estribo
            ListaIntervalosEstriboMuroAutoDto = new List<IntervalosConfinaDTOAuto>();
            //confinamiento
            ListaIntervalosConfinaminetoAutoDto = new List<IntervalosConfinaDTOAuto>();
        }



        public void Ejecutar()
        {
            foreach (IntervalosBarraAutoDtoIMPORTAR item in _listaIntervalosBarraAutoDtoIMPORTAR)
            {

                if (item.Tipo == TipoElementoElevancion.barra)
                {
                    ListaIntervalosBarraAutoDto.Add(item.M1_ObtenerIntervalosBarraAutoDto(_armaduraTrasformada, _doc));
                }
                else if (item.Tipo == TipoElementoElevancion.malla)
                {
                    ListaIntervalosMAllaAutoDto.Add(item.M2_ObtenerIntervalosMallaAutoDto(_armaduraTrasformada, _doc));
                }
                else if (item.Tipo == TipoElementoElevancion.estriboMuro)
                {
                    ListaIntervalosEstriboMuroAutoDto.Add(item.M3_ObtenerIntervalosEstriboAutoDto(_armaduraTrasformada, _doc));
                }
                else if (item.Tipo == TipoElementoElevancion.confinamiento)
                {
                    ListaIntervalosConfinaminetoAutoDto.Add(item.M4_ObtenerIntervalosConfinaminetoAutoDto(_armaduraTrasformada, _doc));
                }

            }


            // coso que no tiene traba pero hay solo traba de confinaineto
            var listaSoloSIntrabas = ListaIntervalosEstriboMuroAutoDto.Where(c => c._datosConfinaDTO.IsTraba == false).ToList();
            for (int i = 0; i < listaSoloSIntrabas.Count; i++)
            {
                var EstriboSinTraba = listaSoloSIntrabas[i];
                var encontrado = ListaIntervalosConfinaminetoAutoDto.Where(c => c.Pier == EstriboSinTraba.Pier && TrabaCOnf_cercaP1Estribo(EstriboSinTraba, c)).FirstOrDefault();
                if (encontrado != null)
                    EstriboSinTraba._datosConfinaDTO.IsTrabaFalsa = true;

            }

            // coso que no tiene traba ni lateral pero hay solo traba de confinaineto
            var listaSoloSIntrabasSinLaterals = ListaIntervalosEstriboMuroAutoDto.Where(c => c._datosConfinaDTO.IsLateral == false &&  c._datosConfinaDTO.IsTraba == false).ToList();
            for (int i = 0; i < listaSoloSIntrabasSinLaterals.Count; i++)
            {
                var EstriboSinTrabaSinLAteral = listaSoloSIntrabasSinLaterals[i];
                var encontrado = ListaIntervalosConfinaminetoAutoDto.Where(c => c.Pier == EstriboSinTrabaSinLAteral.Pier && TrabaCOnf_cercaP1Estribo(EstriboSinTrabaSinLAteral, c)).FirstOrDefault();
                if (encontrado != null)
                    EstriboSinTrabaSinLAteral._datosConfinaDTO.IsTrabaFalsa = true;

            }

        }

        private bool TrabaCOnf_cercaP1Estribo(IntervalosConfinaDTOAuto estriboSinTraba, IntervalosConfinaDTOAuto conf)
        {
            if (estriboSinTraba.ListaPtos == null || conf.ListaPtos == null) return false;
            if (estriboSinTraba.ListaPtos.Count != 2) return false;
            if (conf.ListaPtos.Count != 2) return false;


            var pto1Estribo = estriboSinTraba.ListaPtos[0].GetXY0();
            var pto2Estribo = estriboSinTraba.ListaPtos[1].GetXY0();

            var pto1Conf = conf.ListaPtos[0].GetXY0();
            double zmedioCOnf = (conf.ListaPtos[0].Z + conf.ListaPtos[1].Z) / 2;

            if ( (estriboSinTraba.ListaPtos[1].Z> zmedioCOnf && zmedioCOnf>estriboSinTraba.ListaPtos[0].Z)
                && (pto1Estribo.DistanceTo(pto1Conf) < pto2Estribo.DistanceTo(pto1Conf)))
                return true;
            else
                return false;

        }
    }
}
