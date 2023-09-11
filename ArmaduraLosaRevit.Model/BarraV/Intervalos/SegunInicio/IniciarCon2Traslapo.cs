using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio
{
    public class IniciarCon2Traslapo : IIniciarConXTraslapo
    {
        private SelecionarPtoSup _selecionarPtoSup;
        private readonly ConfiguracionIniciaWPFlBarraVerticalDTO _confiEnfierradoDTO;
        private List<double> _listaIntIntervalos;
        private int _posicionInicia;
        private bool IsUltimoTramoCOnMouse;
        public List<CoordenadasBarra> ListaCoordenadasBarra { get; set; }

        public IniciarCon2Traslapo(SelecionarPtoSup selecionarPtoSup, ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO)
        {
            this._selecionarPtoSup = selecionarPtoSup;
            this._confiEnfierradoDTO = confiEnfierradoDTO;
            this._listaIntIntervalos = selecionarPtoSup.ListaLevelIntervalo;
            this.ListaCoordenadasBarra = new List<CoordenadasBarra>();
            this.IsUltimoTramoCOnMouse = false;
        }

        public void CalcularIntervalo()
        {
            int intervaloInicial = 2;// _confiEnfierradoDTO.incial_ComoIniciarTraslapo_LineaPAr;

            M1_GenerarBArraInicial();

            //calculos
            int numeroIntervalos = _listaIntIntervalos.Count - intervaloInicial;
            int NumeroINtervalosSinPata = (int)(numeroIntervalos - 1) / _confiEnfierradoDTO.inicial_ComoTraslapo;
            double intervalofinal = (numeroIntervalos - 1) % _confiEnfierradoDTO.inicial_ComoTraslapo;

            // ConstantesGenerales.sbLog.AppendLine($"_listaIntIntervalos.Count:{_listaIntIntervalos.Count}  numeroIntervalos:{numeroIntervalos}  NumeroINtervalosSinPata:{NumeroINtervalosSinPata}  intervalofinal:{intervalofinal}");
            //ConstantesGenerales.sbLog.AppendLine($"Level inicial:{_listaIntIntervalos[0].Name}  elev:{_listaIntIntervalos[0].Elevation} ,  Level final:{_listaIntIntervalos[_listaIntIntervalos.Count-1].Name}  elev:{_listaIntIntervalos[_listaIntIntervalos.Count - 1].Elevation}  ");
            _posicionInicia = M2_CAlcularBArrasIntermedias(NumeroINtervalosSinPata, (intervalofinal == 0 ? true : false), _posicionInicia: intervaloInicial);

            //barraFinal
            if (intervalofinal != 0) M3_GenerarBarraFinal(_posicionInicia);

        }



        private void M1_GenerarBArraInicial()
        {
            try
            {


                TipoPataBarra tipobarraInicial;
                //priera barra
                CoordenadasBarra primeraBarra;

                if (_confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataInicial || _confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataAmbos)
                    tipobarraInicial = TipoPataBarra.BarraVPataInicial;
                else
                    tipobarraInicial = TipoPataBarra.BarraVSinPatas;

                if (_confiEnfierradoDTO.TipoSeleccionMousePtoInferior == TipoSeleccionMouse.nivel)
                {
                    primeraBarra = new CoordenadasBarra(new XYZ(0, 0, _listaIntIntervalos[0]),
                                                                       new XYZ(0, 0, _listaIntIntervalos[2]),
                                                                      tipobarraInicial);
                }
                else
                {
                    primeraBarra = new CoordenadasBarra(new XYZ(0, 0, _selecionarPtoSup._PtoInicioIntervaloBarra.Z),
                                                                 new XYZ(0, 0, _listaIntIntervalos[2]),
                                                                       tipobarraInicial);
                }
                ListaCoordenadasBarra.Add(primeraBarra);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ex:{ ex.Message}");


                throw;
            }


        }
        private int M2_CAlcularBArrasIntermedias(int NumeroINtervalosSinPata, bool IsBarraFinal, int _posicionInicia)
        {
            //barras intermedia
            TipoPataBarra tipobarraIntermedio = TipoPataBarra.BarraVSinPatas;

            for (int i = 0; i < NumeroINtervalosSinPata; i++)
            {
                double Zsup = _listaIntIntervalos[_posicionInicia + _confiEnfierradoDTO.inicial_ComoTraslapo];

                //si es ultima barra
                if ((NumeroINtervalosSinPata - 1) == i && IsBarraFinal)
                {
                    if (_confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataInicial || _confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataAmbos)
                        tipobarraIntermedio = TipoPataBarra.BarraVPataFinal;
                    //si es con mouse
                    if (_confiEnfierradoDTO.TipoSeleccionMousePtoSuperior == TipoSeleccionMouse.mouse)
                    {
                        Zsup = _selecionarPtoSup._PtoFinalIntervaloBarra.Z;
                        IsUltimoTramoCOnMouse = true;
                    }

                }

                CoordenadasBarra Intermedia = new CoordenadasBarra(new XYZ(0, 0, _listaIntIntervalos[_posicionInicia]),
                                                                     new XYZ(0, 0, Zsup),
                                                                     tipobarraIntermedio, IsUltimoTramoCOnMouse);
                ListaCoordenadasBarra.Add(Intermedia);
                _posicionInicia = _posicionInicia + _confiEnfierradoDTO.inicial_ComoTraslapo;

            }
            return _posicionInicia;
        }
        private void M3_GenerarBarraFinal(int posicionInicia)
        {

            TipoPataBarra tipobarraFinal;
            if (_confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataFinal || _confiEnfierradoDTO.inicial_tipoBarraV == TipoPataBarra.BarraVPataAmbos)
                tipobarraFinal = TipoPataBarra.BarraVPataFinal;
            else
                tipobarraFinal = TipoPataBarra.BarraVSinPatas;

            CoordenadasBarra FinalBarra;
            if (_confiEnfierradoDTO.TipoSeleccionMousePtoSuperior == TipoSeleccionMouse.nivel)
            {
                FinalBarra = new CoordenadasBarra(new XYZ(0, 0, _listaIntIntervalos[posicionInicia]),
                                                           new XYZ(0, 0, _listaIntIntervalos[_listaIntIntervalos.Count - 1]),
                                                          tipobarraFinal);
            }
            else
            {
                FinalBarra = new CoordenadasBarra(new XYZ(0, 0, _listaIntIntervalos[posicionInicia]),
                                                              new XYZ(0, 0, _selecionarPtoSup._PtoFinalIntervaloBarra.Z),
                                                              tipobarraFinal);
            }
            ListaCoordenadasBarra.Add(FinalBarra);

        }
    }
}
