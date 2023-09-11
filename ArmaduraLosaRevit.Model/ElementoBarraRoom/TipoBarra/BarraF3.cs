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
    public class BarraF3 : BarraBase1, ICasoBarraX
    { 

        public BarraF3( SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base( _solicitud, _datosNuevaBarra)
        {
    
        }

        //public void CambiarCaraSuperior(PathReinforcement _createdPathReinforcement)
        //{
        //    if (_datosNuevaBarra.TipoCaraObjeto_==TipoCaraObjeto.Superior)
        //        ParameterUtil.SetParaInt(_createdPathReinforcement, "Face", 0);
        //}

    }
}
