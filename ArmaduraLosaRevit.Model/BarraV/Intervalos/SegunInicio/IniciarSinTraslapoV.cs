using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio
{
    public class IniciarSinTraslapoV : IIniciarConXTraslapo
    {
        private UIApplication _uiapp;
        private SelecionarPtoSup _selecionarPtoSup;
        private readonly ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO;
      //  private readonly MuroSeleccionadoDTO _muroSeleccionadoDTO;
        private List<double> _listaIntIntervalos;
        private bool IsUltimoTramoCOnMouse;
        private View3D _view3D;

        public List<CoordenadasBarra> ListaCoordenadasBarra { get; set; }
         
        public IniciarSinTraslapoV(SelecionarPtoSup selecionarPtoSup, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO)
        {
            this._uiapp = selecionarPtoSup._uiapp;
            this._view3D = confiEnfierradoDTO.view3D_paraBuscar;
            this._selecionarPtoSup = selecionarPtoSup;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
          //  this._muroSeleccionadoDTO = muroSeleccionadoDTO;
            this._listaIntIntervalos = selecionarPtoSup.ListaLevelIntervalo;
            this.ListaCoordenadasBarra = new List<CoordenadasBarra>();
            this.IsUltimoTramoCOnMouse = false;
        }

        public void CalcularIntervalo()
        {
            //solo una barra
            M1_GenerarBArraInicial();
        }


        private void M1_GenerarBArraInicial()
        {
            try
            {
                TipoPataBarra tipobarraInicial = TipoPataBarra.buscar;

                if (_confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataAmbos ||
                    _confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataInicial ||
                    _confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataFinal ||
                    _confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataAmbos_Horquilla)
                    tipobarraInicial = _confiEnfierradoDTO.inicial_tipoBarraV;



                double Zsup = 0;
                if (_confiEnfierradoDTO.TipoSeleccionMousePtoSuperior == TipoSeleccionMouse.nivel)
                    Zsup = _listaIntIntervalos.Last();
                else
                {
                    Zsup = _selecionarPtoSup._PtoFinalIntervaloBarra.Z;
                    IsUltimoTramoCOnMouse = true;
                }

                    double Zini = 0;
                if (_confiEnfierradoDTO.TipoSeleccionMousePtoInferior == TipoSeleccionMouse.nivel)
                    Zini = _listaIntIntervalos[0];
                else
                {
                    Zini = _selecionarPtoSup._PtoInicioIntervaloBarra.Z;
                 
                }

                CoordenadasBarra primeraBarra = new CoordenadasBarra(new XYZ(0, 0, Zini),
                                                               new XYZ(0, 0, Zsup),
                                                              tipobarraInicial, IsUltimoTramoCOnMouse);
                ListaCoordenadasBarra.Add(primeraBarra);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex:{ ex.Message}");

                ListaCoordenadasBarra.Clear();
            }


        }


    }
}
