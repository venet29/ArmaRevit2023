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
    public class IniciarConAuto : IIniciarConXTraslapo
    {
        private readonly IntervalosBarraAutoDto _newIntervaloBarraAutoDto;

        public List<CoordenadasBarra> ListaCoordenadasBarra { get; set; }

        public IniciarConAuto(IntervalosBarraAutoDto newIntervaloBarraAutoDto)
        {
            this._newIntervaloBarraAutoDto = newIntervaloBarraAutoDto;
            this.ListaCoordenadasBarra = new List<CoordenadasBarra>();
        }

        public void CalcularIntervalo()
        {
            ListaCoordenadasBarra.AddRange(_newIntervaloBarraAutoDto.ListaCoordenadasBarra);
        }
     


    }
}
