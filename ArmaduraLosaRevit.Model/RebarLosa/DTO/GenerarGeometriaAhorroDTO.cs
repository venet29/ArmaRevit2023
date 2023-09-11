using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.RebarLosa.DTO
{
    public class GenerarGeometriaAhorroDTO
    {
        public SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom { get;  set; }
        public ReferenciaRoomDatos _refereciaRoomDatos { get;  set; }
        public bool IsLuzSecuandiria { get;  set; }
        public TipoDireccionBarra TipoDireccionBarra_ { get;  set; }
        public UbicacionLosa ubicacionEnlosa { get;  set; }
        public string TipoBarraStr { get;  set; }
        public List<XYZ> ListaPtosPerimetroBarras { get;  set; }
    }
}
