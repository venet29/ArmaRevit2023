using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Intervalos
{
    public interface IGenerarIntervalosH
    {
        List<IbarraBase> ListaIbarraHorizontal { get; }
        List<IntervaloBarrasDTO> ListaIntervaloBarrasDTO { get; }


        void M1_ObtenerIntervaloBarrasDTO();
        List<IbarraBase> M2_GenerarListaBarraHorizontal();

    }
}