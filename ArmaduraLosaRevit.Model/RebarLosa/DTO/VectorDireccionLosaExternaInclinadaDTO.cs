using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.DTO
{
    public class VectorDireccionLosaExternaInclinadaDTO
    {
        public XYZ direccionLosa { get; set; }
        public Floor Losa { get; set; }
        public PosicionDeBusqueda PosicionDeBusquedaEnu { get; set; }
        public bool IsLosaEncontrada { get; internal set; }
    }
}
