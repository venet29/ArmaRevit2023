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
    public class BarraF19 : BarraBase, ICasoBarra
    {

        private BarraRoom _barraRoom;

        public BarraF19(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
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
            FactoresParaParametrosLargoEspacificosBarra();
            base.AsignacionDePArametrosLargoEspacificosBarra();
            return;
        }




        private void FactoresParaParametrosLargoEspacificosBarra()
        {

            switch (_tipoBarra)
            {
                case "f19":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    break;
                case "f19_Dere":
                    LargoAhorroBarraPrimaria = 0;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    break;
                case "f19_Izq":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = 0;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }
    }
}
