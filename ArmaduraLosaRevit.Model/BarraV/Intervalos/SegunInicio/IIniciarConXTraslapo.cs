using ArmaduraLosaRevit.Model.BarraV.DTO;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos.SegunInicio
{
    public interface IIniciarConXTraslapo
    {
      //  ConfiguracionInicialBarraHorizontalDTO _confiEnfierradoDTO { get; set; }
        List<CoordenadasBarra> ListaCoordenadasBarra { get; set; }

        void CalcularIntervalo();
    }
}