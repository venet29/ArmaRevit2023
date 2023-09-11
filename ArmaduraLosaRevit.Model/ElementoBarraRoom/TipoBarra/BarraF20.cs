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
    public class BarraF20 : BarraBase, ICasoBarra
    {
        private BarraRoom _barraRoom;

        public BarraF20(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
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

                case "f20":
                    if (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior)
                    {
                        LargoAhorroBarraPrimaria = 0;
                        LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                        DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    }
                    else
                    {
                        LargoAhorroBarraPrimaria = 0;
                        LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                        DesplazamientoLargoBarraSecundaria = 0;
                    }                   
                    break;
                case "f20A_Dere_Tras":
                    LargoAhorroBarraPrimaria = 1;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;// PorcentajeLargoPath;
                    break;
                case "f20A_Izq_Tras":
                    LargoAhorroBarraPrimaria = 1;
                    LargoAhorroBarraSecundaria = 1;// - PorcentajeLargoPath;
                    DesplazamientoLargoBarraSecundaria = 0; //PorcentajeLargoPath;
                    break;
                case "f20B_Dere_Tras":
                    LargoAhorroBarraPrimaria = 1;
                    LargoAhorroBarraSecundaria = 1 - PorcentajeLargoPath;
                    DesplazamientoLargoBarraSecundaria = 0;
                    break;
                case "f20B_Izq_Tras":
                    LargoAhorroBarraPrimaria = 1;// - PorcentajeLargoPath;
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
