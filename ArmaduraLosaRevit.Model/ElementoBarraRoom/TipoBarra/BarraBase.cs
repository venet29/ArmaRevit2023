using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra
{
    public abstract class BarraBase
    {
        protected string _tipoBarra;
        protected PathReinforcement _createdPathReinforcement;

        protected RebarShape _tipoRebarShapePrincipal;
        protected RebarShape _tipoRebarShapeAlternativa;
        protected double _largoPathreiforment;
        // protected double _espaciamiento;
        protected UbicacionLosa _ubicacionEnlosa;
        protected bool _IsLuzPrincipal;


        protected double LargoAhorroBarraPrimaria;
        protected double LargoAhorroBarraSecundaria;
        protected double DesplazamientoLargoBarraSecundaria;
#pragma warning disable CS0169 // The field 'BarraBase._solicitud' is never used
        private SolicitudBarraDTO _solicitud;
#pragma warning restore CS0169 // The field 'BarraBase._solicitud' is never used
        protected DatosNuevaBarraDTO _datosNuevaBarra;
        protected double PorcentajeLargoPath;
        private int DiametroOrientacionPrincipal_mm;

        public BarraBase(BarraRoom barraRoom, SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra)
        {
            // this._solicitud = _solicitud;
            this._datosNuevaBarra = _datosNuevaBarra;
            this._tipoBarra = barraRoom.TipoBarraStr;
            this._createdPathReinforcement = barraRoom.m_createdPathReinforcement;

            this._tipoRebarShapePrincipal = barraRoom.tipoRebarShapePrincipal;
            this._tipoRebarShapeAlternativa = barraRoom.tipoRebarShapeAlternativa;
            this._largoPathreiforment = barraRoom.LargoPathreiforment;

            this.DiametroOrientacionPrincipal_mm = _datosNuevaBarra.DiametroOrientacionPrincipal_mm;


            //this._espaciamiento = barraRoom.Espaciamiento;
            this._ubicacionEnlosa = barraRoom.ubicacionEnlosa;
            this._IsLuzPrincipal = barraRoom.IsLuzSecuandiria;
        }

        // a) seleciona que barra se botton - inferior
        //b) activa barra alternativa de ser necesario
        //c) asigna largo de barra princiapales y alterniva si corresonde
        //b) asigna largos 


        public void AsignacionDePArametrosGeneralesBarra()
        {
            // "Face", 0  ->  activa barra superior  - Top ( viene por defecto=
            // "Face", 1  ->  activa barra inferior  - Botton
            ParameterUtil.SetParaInt(_createdPathReinforcement, "Face", 1);

            //ESPACIMEINTOS
            ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_SPACING, _datosNuevaBarra.EspaciamientoFoot);

            // si es luz princiapl sube el path 0.03'
            if (_IsLuzPrincipal) //ParameterUtil.SetParaInt(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ADDL_OFFSET, Util.CmToFoot(1));
            {
                double despla = (DiametroOrientacionPrincipal_mm != 0 ? Util.MmToFoot(DiametroOrientacionPrincipal_mm) : Util.CmToFoot(1));
                ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ADDL_OFFSET, despla);
            }
            // PATH_REIN_SHAPE_1   "Primary Bar - Shape"    
            //asignar shape
            ParameterUtil.SetParaElementId(_createdPathReinforcement, BuiltInParameter.PATH_REIN_SHAPE_1, _datosNuevaBarra.tipoRebarShapePrincipal.Id);

            // PATH_REIN_SHAPE_2   "Alternating Bar - Shape"
            //activa la barra altenativa  value=1 --- si es cero solo estan activas las barras principal
            ParameterUtil.SetParaInt(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ALTERNATING, 1);

            //asignar shape
            ParameterUtil.SetParaElementId(_createdPathReinforcement, BuiltInParameter.PATH_REIN_SHAPE_2, _datosNuevaBarra.tipoRebarShapeAlternativa.Id);

            return;

        }


        protected void AsignacionDePArametrosLargoEspacificosBarra()
        {
            // PATH_REIN_SHAPE_1   "Primary Bar - Shape"    
            //activa la barra altenativa  value=1 --- si es cero solo estan activas las barras principal

            double largobarrasPrimaria = _datosNuevaBarra.LargoPathreiforment - LargoAhorroBarraPrimaria;
            if (_tipoBarra == "f16" || _tipoBarra == "f16a"
                || _tipoBarra == "f22a" || _tipoBarra == "f22b" || _tipoBarra == "f22aInv" || _tipoBarra == "f22bInv") //10-07-23  se agre caso 22 pq se vio el problema --> rvisar para caso 17,hasta 21
            {
                RedonderLargoBarras.RedondearFoot5_mascercano(largobarrasPrimaria);
                largobarrasPrimaria = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            else
            {
                RedonderLargoBarras.RedondearFoot1_mascercano(largobarrasPrimaria);
                largobarrasPrimaria = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, largobarrasPrimaria);

            // PATH_REIN_SHAPE_2   "Alternating Bar - Shape"
            ////asigna largo de barras Alternativas
            ///


            double largobarrasPSecundaria = _datosNuevaBarra.LargoPathreiforment - LargoAhorroBarraSecundaria;
            if (_tipoBarra == "f16" || _tipoBarra == "f16a"
                || _tipoBarra == "f22a" || _tipoBarra == "f22b" || _tipoBarra == "f22aInv" || _tipoBarra == "f22bInv")//10-07-23  se agre caso 22 pq se vio el problema --> rvisar para caso 17,hasta 21
            {
                RedonderLargoBarras.RedondearFoot5_mascercano(largobarrasPSecundaria);
                largobarrasPSecundaria = RedonderLargoBarras.NuevoLargobarraFoot;
            }
            else
            {
                RedonderLargoBarras.RedondearFoot1_mascercano(largobarrasPSecundaria);
                largobarrasPSecundaria = RedonderLargoBarras.NuevoLargobarraFoot;
            }

            ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_2, largobarrasPSecundaria);

            // desplazamiento de barra alternativa
            if (DesplazamientoLargoBarraSecundaria != 0)
                ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ALT_OFFSET, DesplazamientoLargoBarraSecundaria);
        }

        protected void PorcentajeLargoAhorro()
        {
            double Largoahorro = _datosNuevaBarra.LargoMininoLosa * ConstNH.CONST_PORCENTAJE_LARGOAHORRO;


            PorcentajeLargoPath = Math.Round((Largoahorro / _datosNuevaBarra.LargoPathreiforment), 2);


        }

    }
}
