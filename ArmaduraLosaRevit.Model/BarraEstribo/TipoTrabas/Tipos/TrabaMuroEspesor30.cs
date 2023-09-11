using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas.Servicios;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas.Tipos
{
    public class TrabaMuroEspesor30: ATrabaBase, ITipoTraba
    {
        private ObtenerIntervalos _ObtenerIntervalos;
        public TrabaMuroEspesor30(ConfiguracionBarraTrabaDTO _configuracionBarraTrabaDTO) :base(_configuracionBarraTrabaDTO)
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

            if (TipoTraba_Posicion == TipoTraba_posicion.B)
            {
                switch (_cantidadLineas)
                {
    
                    case 4:
                        ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(0), RebarHookOrientation.Left, TipoTraba.Longitudinal, DiametroTrabasTrasversales_mm, Util.CmToFoot(45+3)));
                        break;
                    case 5:
                        ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(0), RebarHookOrientation.Left, TipoTraba.Longitudinal, DiametroTrabasTrasversales_mm, Util.CmToFoot(60+3)));
                        break;
                    default:
                        ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(0), RebarHookOrientation.Left, TipoTraba.Longitudinal, DiametroTrabasTrasversales_mm, Util.CmToFoot(30+3)));
                        break;
                }
              
            }


            double[] ListaIntervalo = (_configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal !=null
                                                    ? _configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal 
                                                    : _ObtenerIntervalos.ObtenerSeparacion(_cantidadLineas));
           
            if (ListaIntervalo == null) return ListaTrabaDTO;

            for (int i = 0; i < ListaIntervalo.Length; i++)
            {
                double desplaza = (Util.IsPar(i) ? Util.CmToFoot(3): 0);
                RebarHookOrientation rb = (Util.IsPar(i) ? RebarHookOrientation.Left : RebarHookOrientation.Right);
                ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(ListaIntervalo[i]) + desplaza, rb, TipoTraba.Transversal, DiametroTrabasTrasversales_mm));
            }

            return ListaTrabaDTO;
        }
    
        private List<BarraTrabaDTO> M2_ObtenerTipoTrabaDer()
        {
            if (TipoTraba_Posicion == TipoTraba_posicion.B)
            {
                switch (_cantidadLineas)
                {

                    case 4:
                        ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(0), RebarHookOrientation.Left, TipoTraba.Longitudinal, DiametroTrabasTrasversales_mm, Util.CmToFoot(45 + 3)));
                        break;
                    case 5:
                        ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(0), RebarHookOrientation.Left, TipoTraba.Longitudinal, DiametroTrabasTrasversales_mm, Util.CmToFoot(60 + 3)));
                        break;
                    default:
                        ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(0), RebarHookOrientation.Left, TipoTraba.Longitudinal, DiametroTrabasTrasversales_mm, Util.CmToFoot(30 + 3)));
                        break;
                }

            }


            double[] ListaIntervalo = (_configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal != null
                                              ? _configuracionBarraTrabaDTO.listaEspaciamientoTrabasTransversal
                                              : _ObtenerIntervalos.ObtenerSeparacion(_cantidadLineas));

            if (ListaIntervalo==null) return ListaTrabaDTO;

            for (int i = 0; i < ListaIntervalo.Length; i++)
            {
                double desplaza = (Util.IsPar(i) ? Util.CmToFoot(3) : 0);
                RebarHookOrientation rb = (Util.IsPar(i) ? RebarHookOrientation.Right : RebarHookOrientation.Left);
                ListaTrabaDTO.Add(new BarraTrabaDTO(Util.CmToFoot(ListaIntervalo[i])+ desplaza, rb, TipoTraba.Transversal, DiametroTrabasTrasversales_mm));
            }


            return ListaTrabaDTO;
        }
    
    }
}