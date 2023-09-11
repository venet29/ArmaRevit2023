using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra
{
    public class BarraF11 : BarraBase1, ICasoBarraX
    {



        public BarraF11(SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(_solicitud, _datosNuevaBarra)
        {

        }


    }
}
