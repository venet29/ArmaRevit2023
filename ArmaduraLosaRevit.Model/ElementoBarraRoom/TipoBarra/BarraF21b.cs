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
    public class BarraF21b : BarraBase, ICasoBarra
    {

        private BarraRoom _barraRoom;

        public BarraF21b(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
        {
            this._barraRoom = barraRoom;
        }



        public void LayoutRebar_PathReinforcement()
        {
            if (_createdPathReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a PathDeRefuerzo null ");
                return;
            }
            base.AsignacionDePArametrosGeneralesBarra();
            base.PorcentajeLargoAhorro();
            this.FactoresParaParametrosLargoEspacificosBarra();
            base.AsignacionDePArametrosLargoEspacificosBarra();
            return;
        }


        private void FactoresParaParametrosLargoEspacificosBarra()
        {
            LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
            LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
            DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
        }



    }
}
