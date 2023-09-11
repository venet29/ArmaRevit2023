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
    public class IniciarSinTraslapoH : IIniciarConXTraslapo
    {
        private UIApplication _uiapp;
        private SelecionarPtoHorizontal _selecionarPtoHorizontal;
        //private readonly ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO;
      //  private readonly MuroSeleccionadoDTO _muroSeleccionadoDTO;
        private List<Level> _listaIntIntervalos;
//#pragma warning disable CS0169 // The field 'IniciarSinTraslapoH._posicionInicia' is never used
//        private int _posicionInicia;
//#pragma warning restore CS0169 // The field 'IniciarSinTraslapoH._posicionInicia' is never used
        private View3D _view3D;

        public ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO { get; set; }
        public List<CoordenadasBarra> ListaCoordenadasBarra { get; set; }

        public IniciarSinTraslapoH(SelecionarPtoHorizontal selecionarPtoHorizontal, ConfiguracionInicialBarraHorizontalDTO confiEnfierradoDTO)
        {
            this._uiapp = selecionarPtoHorizontal._uiapp;
            this._view3D = confiEnfierradoDTO.view3D_paraBuscar;
            this._selecionarPtoHorizontal = selecionarPtoHorizontal;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
          //  this._muroSeleccionadoDTO = muroSeleccionadoDTO;
            this._listaIntIntervalos = selecionarPtoHorizontal.ListaLevelIntervalo;
            this.ListaCoordenadasBarra = new List<CoordenadasBarra>();
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
                TipoPataBarra tipobarraInicial = TipoPataBarra.BarraVSinPatas;
                CoordenadasBarra primeraBarra = new CoordenadasBarra(_selecionarPtoHorizontal._PtoInicioIntervaloBarra6,
                                                                     _selecionarPtoHorizontal._PtoFinalIntervaloBarra, 
                                                                     tipobarraInicial);
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
