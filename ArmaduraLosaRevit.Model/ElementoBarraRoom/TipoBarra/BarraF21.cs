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
    public class Barra11 : BarraBase, ICasoBarra
    {

        private BarraRoom _barraRoom;

        public Barra11(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
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

            switch (_tipoBarra)
            {
                case "f21":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    break;
                case "f21A_Dere_Tras":
                    LargoAhorroBarraPrimaria = 1;
                    LargoAhorroBarraSecundaria = 1 - PorcentajeLargoPath;
                    DesplazamientoLargoBarraSecundaria = PorcentajeLargoPath;
                    break;
                case "f21A_Izq_Tras":
                    LargoAhorroBarraPrimaria = 1;
                    LargoAhorroBarraSecundaria = 1 - PorcentajeLargoPath;
                    DesplazamientoLargoBarraSecundaria =  PorcentajeLargoPath;
                    break;
                case "f21B_Dere_Tras":
                    LargoAhorroBarraPrimaria = 1;
                    LargoAhorroBarraSecundaria = 1 - PorcentajeLargoPath;
                    DesplazamientoLargoBarraSecundaria = PorcentajeLargoPath;
                    break;
                case "f21B_Izq_Tras":
                    LargoAhorroBarraPrimaria = 1 - PorcentajeLargoPath;
                    LargoAhorroBarraSecundaria = 1;
                    DesplazamientoLargoBarraSecundaria = 0;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

      
        }



    }
}
