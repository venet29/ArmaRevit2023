using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraMallaRebar.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.DTO
{
    public class IntervalosMallaDTOAuto
    {
        public DatosMallasAutoDTO _datosMallasDTO { get; set; }

        public List<Curve> ListaCurvaPAthArea { get;  set; }

        public Element _muroSeleccionado { get; set; }
        public List<XYZ> ListaPtos { get;  set; }
        public bool IsOk { get;  set; }
        public List<XYZ> ListaPtos_mallaVertical { get;  set; }

        public bool IsBuscarCororonacion { get; set; }
        public string Pier { get; internal set; }
        public string Story { get; internal set; }

        public  DatosMallasAutoDTO ObtenerDatosMallasDTO()
        {
            DatosMallasAutoDTO datosMallasDTO = new DatosMallasAutoDTO()
            {
                diametroH = _datosMallasDTO.diametroH,
                diametroV = _datosMallasDTO.diametroV,
                paraCantidadLineasV = _datosMallasDTO.ObtenerNUmeroMallas_tipoMallaV(), //   AyudaMallaAUTO.ObtenerNUmeroMallas("E.D."),//tipo_mallaV.Text
                paraCantidadLineasH = _datosMallasDTO.ObtenerNUmeroMallas_tipoMallaH(),//AyudaMallaAUTO.ObtenerNUmeroMallas("E.D."),
                espaciemientoH = _datosMallasDTO.espaciemientoH,
                espaciemientoV = _datosMallasDTO.espaciemientoV,
                tipoMallaH = _datosMallasDTO.tipoMallaH,
                tipoMallaV = _datosMallasDTO.tipoMallaH,
                tipoSeleccionInf = TipoSeleccionMouse.nivel,
                tipoSeleccionSup = TipoSeleccionMouse.nivel,
                IsBuscarCororonacion = _datosMallasDTO.IsBuscarCororonacion

            };

            return datosMallasDTO;
        }
    }
}
