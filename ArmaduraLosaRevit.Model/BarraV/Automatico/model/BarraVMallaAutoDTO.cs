using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model
{
    public class BarraVMallaAutoDTO
    {
        public ConfiguracionIniciaWPFlBarraVerticalDTO ConfiWPFEnfierradoDTO { get; set; }
        public DatosMallasAutoDTO DatosMallasAutoDTO { get; set; }
        public DireccionRecorrido DireccionRecorrido { get; set; }
        public SelecionarPtoSup SelecionarPtoSup { get; set; }
        public DatosMuroSeleccionadoDTO DatosmuroSeleccionadoDTO { get; set; }


        public BarraVMallaAutoDTO(ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO, 
            DatosMallasAutoDTO datosMallasAutoDTO,
            DireccionRecorrido direccionRecorrido, 
            SelecionarPtoSup selecionarPtoSup,
            DatosMuroSeleccionadoDTO datosmuroSeleccionadoDTO)
        {
            this.ConfiWPFEnfierradoDTO = confiWPFEnfierradoDTO;
            this.DatosMallasAutoDTO = datosMallasAutoDTO;
            this.DireccionRecorrido = direccionRecorrido;
            this.SelecionarPtoSup = selecionarPtoSup;
            this.DatosmuroSeleccionadoDTO = datosmuroSeleccionadoDTO;
        }

    }
}
