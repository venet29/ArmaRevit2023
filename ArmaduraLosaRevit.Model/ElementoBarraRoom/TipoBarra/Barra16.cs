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
    public class Barra16 : BarraBase, ICasoBarra
    {
       
       // private DatosNuevaBarraDTO _datosNuevaBarra;
        private SolicitudBarraDTO _solicitud;

        public Barra16(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
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
                case "f16a":
                case "f16":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                  //  AsignacionDePArametrosLargoEspacificosBarra16();
                    break;
                case "f16_dere":
                    LargoAhorroBarraPrimaria = 0;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    // AsignacionDePArametrosLargoEspacificosBarra16_DERE();
                    break;
                case "f16_izq":
                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    LargoAhorroBarraSecundaria = 0;
                    DesplazamientoLargoBarraSecundaria = 0;

                    //LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_Inf;
                    //LargoAhorroBarraSecundaria = 0;
                    //DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_Sup;

                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }


    }
}
