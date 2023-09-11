using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF17B_Tras : GeomeTagF17, IGeometriaTag
    {
        public GeomeTagF17B_Tras(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
        base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        {


        }


    }
}
