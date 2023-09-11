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
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra
{
    public class Barra16B : BarraBase, ICasoBarra
    {

        // private DatosNuevaBarraDTO _datosNuevaBarra;
        private SolicitudBarraDTO _solicitud;

        public Barra16B(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra) : base(barraRoom, _solicitud, _datosNuevaBarra)
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
            AsignacionDePArametrosLargoEspacificosBarra();

            return;
        }



        private void FactoresParaParametrosLargoEspacificosBarra()
        {
            //double Largoahorro=_datosNuevaBarra.LargoMininoLosa * 0.15;
            //double PorcentajeLargoPath = Math.Round((Largoahorro / _datosNuevaBarra.LargoPathreiforment), 2);

            switch (_tipoBarra.ToLower())
            {
                case "f16b":

                    LargoAhorroBarraPrimaria = _datosNuevaBarra.dimBarras._LargoAhorro_Izq_;
                    LargoAhorroBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    DesplazamientoLargoBarraSecundaria = _datosNuevaBarra.dimBarras._LargoAhorro_Dere_;
                    //  AsignacionDePArametrosLargoEspacifi-cosBarra16();
                    break;

                default:
                    Console.WriteLine("Default case");
                    break;
            }

        }

#pragma warning disable CS0108 // 'Barra16B.AsignacionDePArametrosLargoEspacificosBarra()' hides inherited member 'BarraBase.AsignacionDePArametrosLargoEspacificosBarra()'. Use the new keyword if hiding was intended.
        protected void AsignacionDePArametrosLargoEspacificosBarra()
#pragma warning restore CS0108 // 'Barra16B.AsignacionDePArametrosLargoEspacificosBarra()' hides inherited member 'BarraBase.AsignacionDePArametrosLargoEspacificosBarra()'. Use the new keyword if hiding was intended.
        {
            // PATH_REIN_SHAPE_1   "Primary Bar - Shape"    
            //activa la barra altenativa  value=1 --- si es cero solo estan activas las barras principal

            double largobarrasPrimaria = _datosNuevaBarra.LargoPathreiforment - LargoAhorroBarraPrimaria;

            RedonderLargoBarras.RedondearFoot5_mascercano(largobarrasPrimaria);
            largobarrasPrimaria = RedonderLargoBarras.NuevoLargobarraFoot;

            ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, largobarrasPrimaria);

            // PATH_REIN_SHAPE_2   "Alternating Bar - Shape"
            ////asigna largo de barras Alternativas
            ///


            double largobarrasPSecundaria = _datosNuevaBarra.LargoPathreiforment - LargoAhorroBarraSecundaria;

            RedonderLargoBarras.RedondearFoot5_mascercano(largobarrasPSecundaria);
            largobarrasPSecundaria = RedonderLargoBarras.NuevoLargobarraFoot;


            ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, largobarrasPSecundaria);

            // desplazamiento de barra alternativa
            if (DesplazamientoLargoBarraSecundaria != 0)
                ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, DesplazamientoLargoBarraSecundaria);
        }
    }
}
