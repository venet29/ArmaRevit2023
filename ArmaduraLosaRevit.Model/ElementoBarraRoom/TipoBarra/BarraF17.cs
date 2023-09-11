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
    public class BarraF17 : BarraBase, ICasoBarra
    {


        private BarraRoom _barraRoom;

        public BarraF17(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
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
                case "f17":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    break;
                case "f17a":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    break;
                case "f17b":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    break;
                case "f17A_Tras":
                    LargoAhorroBarraPrimaria = 0;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    break;
                case "f17B_Tras":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = 0;
                    DesplazamientoLargoBarraSecundaria = 0;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

     
        }


 


    }
}
