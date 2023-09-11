using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra
{
    public class BarraF22 : BarraBase, ICasoBarra
    {

        // private DatosNuevaBarraDTO _datosNuevaBarra;
        private SolicitudBarraDTO _solicitud;

        public BarraF22(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
        {
            this._solicitud = _solicitud;
            // this._datosNuevaBarra = _datosNuevaBarra;
        }

        // a) seleciona que barra se botton - inferior
        //b) activa barra alternativa de ser necesario
        //c) asigna largo de barra princiapales y alterniva si corresonde
        //b) asigna largos 
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
            //double Largoahorro=_datosNuevaBarra.LargoMininoLosa * 0.15;
            //double PorcentajeLargoPath = Math.Round((Largoahorro / _datosNuevaBarra.LargoPathreiforment), 2);

            switch (_tipoBarra.ToLower())
            {
                case "f22_dere":
                case "f22_izq":
                case "f22binv":
                case "f22ainv":
                case "f22b":
                case "f22a":
                case "f22":
                    if (_ubicacionEnlosa == UbicacionLosa.Izquierda || _ubicacionEnlosa == UbicacionLosa.Inferior)
                    {
                    
                        LargoAhorroBarraPrimaria = 0;
                        LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                        DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    }
                    else
                    {
                        // case "f16_izq":
                        LargoAhorroBarraPrimaria = 0;// _datosNuevaBarra.dimBarras._LargoAhorro_Dere_Sup;
                        LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                        DesplazamientoLargoBarraSecundaria = 0;

                    }
                    //  AsignacionDePArametrosLargoEspacificosBarra16();
                    break;
                case "f22tras":
                    LargoAhorroBarraPrimaria = 1;
                    LargoAhorroBarraSecundaria = 1;
                    DesplazamientoLargoBarraSecundaria = 0;
                    // AsignacionDePArametrosLargoEspacificosBarra16_DERE();
                    break;

                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }


    }
}
