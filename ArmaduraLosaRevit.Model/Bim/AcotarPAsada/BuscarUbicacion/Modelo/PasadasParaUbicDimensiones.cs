using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo
{
    public enum DireccionDimensionEnum
    {
 
        BAJO_IZQ,
        ARRIBA_IZQ,
        BAJO_DERE,
        ARRIBA_DERE,
        IZQ_ARRIBA,
        DERE_ARRIBA,
        IZQ_BAJO,
        DERE_BAJO
    }
    public class PasadasParaUbicDimensiones
    {
        private UIApplication _uiapp;        
        public EnvoltoriPasada EnvoltoriPasada_ { get; set; }
        //public DireccionDimensionEnum DireccionDimension_CaraDireIzqDere { get; set; } // REPRESENTEA LA DIRECCION DE LA DIMENSION VERTICAL
        //public DireccionDimensionEnum DireccionDimension_CaraArribaBAjo { get; set; } // RESPRESENTA LA DIRECION DE LAS DIMENSION HORIZONTAL

        public bool IsDibujadoDimensiones { get; set; }
        public double RadioBusqueda { get; set; }
        internal PlanoParaUbicDimensiones CaraPasadaArriba { get; private set; }
        internal PlanoParaUbicDimensiones CaraPasadaBajo { get; private set; }
        internal PlanoParaUbicDimensiones CaraPasadaIzq { get; private set; }
        internal PlanoParaUbicDimensiones CaraPasadaDere { get; private set; }


        public PasadasParaUbicDimensiones(UIApplication uiapp, EnvoltoriPasada envoltoriPasada)
        {
            this._uiapp = uiapp;
            this.EnvoltoriPasada_ = envoltoriPasada;
            this.IsDibujadoDimensiones = false;
        }


        public bool ObtenerDatos()
        {
            try
            {
                for (int i = 0; i < EnvoltoriPasada_.ListaPLanosPAsadas.Count; i++)
                {
                    var _planoPasada = EnvoltoriPasada_.ListaPLanosPAsadas[i];

                    //_planoPasada.M0_GenerarDIreccion_DerechaSuperior();
                    //_planoPasada.M1_BuscarPosicionCara();

                    if (_planoPasada.TipoCara_ == TipoCara.Arriba)
                    {
                        CaraPasadaArriba = new PlanoParaUbicDimensiones(_uiapp,_planoPasada, EnvoltoriPasada_._pasada.Id);
                        CaraPasadaArriba.ObtenerDatos();
                    }
                    else if (_planoPasada.TipoCara_ == TipoCara.Bajo)
                    {
                        CaraPasadaBajo = new PlanoParaUbicDimensiones(_uiapp, _planoPasada, EnvoltoriPasada_._pasada.Id);
                        CaraPasadaBajo.ObtenerDatos();
                    }
                    else if (_planoPasada.TipoCara_ == TipoCara.Izquierdo)
                    {
                        CaraPasadaIzq = new PlanoParaUbicDimensiones(_uiapp, _planoPasada, EnvoltoriPasada_._pasada.Id);
                        CaraPasadaIzq.ObtenerDatos();
                    }
                    else if (_planoPasada.TipoCara_ == TipoCara.Derecha)
                    {
                        CaraPasadaDere = new PlanoParaUbicDimensiones(_uiapp, _planoPasada, EnvoltoriPasada_._pasada.Id);
                        CaraPasadaDere.ObtenerDatos();
                    }

                }

                RadioBusqueda = Math.Max(CaraPasadaArriba.MaximoLargo, Math.Max(CaraPasadaBajo.MaximoLargo, Math.Max(CaraPasadaIzq.MaximoLargo, CaraPasadaDere.MaximoLargo)));

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerDatos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        internal void DibujarRectagulo()
        {
            if(CaraPasadaArriba!= null && CaraPasadaArriba.IsOk)
                CaraPasadaArriba.DIbujarRectagulo("LINEA TABLA DE REVISIONES");

            if (CaraPasadaBajo != null && CaraPasadaBajo.IsOk)
                CaraPasadaBajo.DIbujarRectagulo("LINEA TABLA DE REVISIONES");

            if (CaraPasadaIzq != null && CaraPasadaIzq.IsOk)
                CaraPasadaIzq.DIbujarRectagulo("Lines");

            if (CaraPasadaDere != null && CaraPasadaDere.IsOk)
                CaraPasadaDere.DIbujarRectagulo("Lines");
        }

        internal void ConfigInicial()
        {
            EnvoltoriPasada_.DimesionVerticalDireccion = EnumPasadasConGrilla.Izquieda_sup;
            EnvoltoriPasada_.DimesionHorizonalDireccion = EnumPasadasConGrilla.Bajo_izq;
            CaraPasadaIzq.IsOk = true;
            CaraPasadaDere.IsOk = true;
            CaraPasadaBajo.IsOk = true;
            CaraPasadaArriba.IsOk = true;
        }
    }
}
