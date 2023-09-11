using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
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
    public abstract class BarraBase1
    {
        private readonly SolicitudBarraDTO _solicitudBarra;
        protected readonly DatosNuevaBarraDTO _datosNuevaBarra;
        protected PathReinforcement _createdPathReinforcement;

        public BarraBase1(SolicitudBarraDTO _solicitud, DatosNuevaBarraDTO _datosNuevaBarra)
        {
            this._solicitudBarra = _solicitud;
            this._datosNuevaBarra = _datosNuevaBarra;
        }

        // a) seleciona que barra se botton - inferior
        //b) activa barra alternativa de ser necesario
        //c) asigna largo de barra princiapales y alterniva si corresonde
        //b) asigna largos 


        public bool LayoutRebar_PathReinforcement(PathReinforcement _createdPathReinforcement, DatosNuevaBarraDTO _datosNuevaBarraDTO)
        {
            try
            {
                double DiametroOrientacionPrincipal_mm = _datosNuevaBarraDTO.DiametroOrientacionPrincipal_mm;

                if (_createdPathReinforcement == null)
                {
                    TaskDialog.Show("Error", "Referencia a PathDeRefuerzo null ");
                    return false;
                }

                // "Face", 0  ->  activa barra superior  - Top ( viene por defecto=
                // "Face", 1  ->  activa barra inferior  - Botton
                if (_solicitudBarra.TipoBarra == "f9" || _solicitudBarra.TipoBarra == "f1_SUP" || _solicitudBarra.TipoBarra == "f9a" || _solicitudBarra.TipoBarra == "f10a" || _solicitudBarra.TipoBarra == "s1" || _solicitudBarra.TipoBarra == "s3" || _solicitudBarra.TipoBarra == "s2")
                    ParameterUtil.SetParaInt(_createdPathReinforcement, "Face", 0);
                else
                    ParameterUtil.SetParaInt(_createdPathReinforcement, "Face", 1);


                ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_LENGTH_1, _datosNuevaBarra.LargoPathreiforment);

                //ESPACIMEINTOSnn
                ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_SPACING, _datosNuevaBarra.EspaciamientoFoot);

                ParameterUtil.SetParaElementId(_createdPathReinforcement, BuiltInParameter.PATH_REIN_SHAPE_1, _datosNuevaBarra.tipoRebarShapePrincipal.Id);
                // si es luz princiapl sube el path 0.03'




                if (_datosNuevaBarraDTO._BarraTipo == TipoRebar.FUND_BA_INF || _datosNuevaBarraDTO._BarraTipo == TipoRebar.FUND_BA_SUP)
                {
                    double despla = Util.CmToFoot(3);

                    if (_datosNuevaBarra.IsLuzSecuandiria)
                        despla = despla + (DiametroOrientacionPrincipal_mm != 0 ? Util.MmToFoot(DiametroOrientacionPrincipal_mm) : Util.CmToFoot(1));
                    ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ADDL_OFFSET, despla);
                }
                else
                {
                    if (_datosNuevaBarra.IsLuzSecuandiria) //ParameterUtil.SetParaInt(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ADDL_OFFSET, Util.CmToFoot(1));
                    {
                        double despla = (DiametroOrientacionPrincipal_mm != 0 ? Util.MmToFoot(DiametroOrientacionPrincipal_mm) : Util.CmToFoot(1));
                        ParameterUtil.SetParaDouble(_createdPathReinforcement, BuiltInParameter.PATH_REIN_ADDL_OFFSET, despla);
                    }
                }


                ElementId startRebarHookTypeId = _datosNuevaBarraDTO.tipodeHookStartPrincipal;
                ElementId endRebarHookTypeId = _datosNuevaBarraDTO.tipodeHookEndPrincipal;

                if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.Sin)
                {
                    startRebarHookTypeId = _datosNuevaBarraDTO._InvalidrebarHookTypeId;
                    endRebarHookTypeId = _datosNuevaBarraDTO._InvalidrebarHookTypeId;
                }
                else if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.DereSup)
                { startRebarHookTypeId = _datosNuevaBarraDTO._InvalidrebarHookTypeId; }

                else if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.IzqInf)
                { endRebarHookTypeId = _datosNuevaBarraDTO._InvalidrebarHookTypeId; }


                ParameterUtil.SetParaElementId(_createdPathReinforcement, BuiltInParameter.PATH_REIN_HOOK_TYPE_1, endRebarHookTypeId);
                ParameterUtil.SetParaElementId(_createdPathReinforcement, BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1, startRebarHookTypeId); ;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ejecutar 'BarraBase1'  ex: {ex.Message}");
            }
            return true;
        }


        public bool CambiarCaraSuperior(PathReinforcement _createdPathReinforcement)
        {
            try
            {
                if (_createdPathReinforcement == null) return false;
                if (_createdPathReinforcement.IsValidObject == false) return false;
                if (_datosNuevaBarra.TipoCaraObjeto_ == TipoCaraObjeto.Superior)
                    ParameterUtil.SetParaInt(_createdPathReinforcement, "Face", 0);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al ejecutar 'BarraF11'  ex: {ex.Message}");
            }
            return true;
        }
    }
}
