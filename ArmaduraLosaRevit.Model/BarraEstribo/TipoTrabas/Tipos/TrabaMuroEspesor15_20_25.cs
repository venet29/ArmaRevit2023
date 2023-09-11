using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas.Servicios;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas.Tipos
{
    public class TrabaMuroEspesor15_20_25 : ATrabaBase, ITipoTraba
    {
#pragma warning disable CS0169 // The field 'TrabaMuroEspesor15_20_25.configuracionBarraTrabaDTO' is never used
        private readonly ConfiguracionBarraTrabaDTO configuracionBarraTrabaDTO;
#pragma warning restore CS0169 // The field 'TrabaMuroEspesor15_20_25.configuracionBarraTrabaDTO' is never used
        private ObtenerIntervalos _ObtenerIntervalos;

        public TrabaMuroEspesor15_20_25(ConfiguracionBarraTrabaDTO _configuracionBarraTrabaDTO) : base(_configuracionBarraTrabaDTO)
        {

            _ObtenerIntervalos = new ObtenerIntervalos(_configuracionBarraTrabaDTO);
        }

        public List<BarraTrabaDTO> ObtenerListaEspacimientoTrabas()
        {
            if (_direccionTraba == DireccionTraba.Izquierda)
            { return M1_ObtenerTipoTrabaIzq(); }
            else
            { return M2_ObtenerTipoTrabaDer(); }

        }

        private List<BarraTrabaDTO> M1_ObtenerTipoTrabaIzq()
        {
            double[] ListaIntervalo = (_configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal != null
                                        ? _configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal
                                        : _ObtenerIntervalos.ObtenerSeparacion(cantidadTrabasTrasversales));

            if (ListaIntervalo == null) return ListaTrabaDTO;

            for (int i = 0; i < ListaIntervalo.Length; i++)
            {
                double desplaza = (Util.IsPar(i) ? Util.CmToFoot(3) : 0);
                RebarHookOrientation rb = (Util.IsPar(i) ? RebarHookOrientation.Left : RebarHookOrientation.Right);
                ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(ListaIntervalo[i]) + desplaza, rb, TipoTraba.Transversal, DiametroTrabasTrasversales_mm));
            }

            return ListaTrabaDTO;
        }

        private List<BarraTrabaDTO> M2_ObtenerTipoTrabaDer()
        {

            double[] ListaIntervalo = (_configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal != null
                                        ? _configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal
                                        : _ObtenerIntervalos.ObtenerSeparacion(cantidadTrabasTrasversales));

            if (ListaIntervalo == null) return ListaTrabaDTO;

            for (int i = 0; i < ListaIntervalo.Length; i++)
            {
                double desplaza = (Util.IsPar(i) ? Util.CmToFoot(3) : 0);
                RebarHookOrientation rb = (Util.IsPar(i) ? RebarHookOrientation.Right : RebarHookOrientation.Left);
                ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(ListaIntervalo[i]) + desplaza, rb, TipoTraba.Transversal, DiametroTrabasTrasversales_mm));
            }



            return ListaTrabaDTO;
        }
  

    }
}