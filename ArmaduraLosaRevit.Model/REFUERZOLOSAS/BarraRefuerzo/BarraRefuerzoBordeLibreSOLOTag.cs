using Autodesk.Revit.DB;
using System;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo
{
    public class BarraRefuerzoBordeLibreSOLOTag : ARebarRefuerzo
    {
       // private readonly UIApplication _uiapp;
        private readonly SeleccionarRebarElemento _seleccionarRebarElemento;

        private readonly IGeometriaTag _iGeometriaTagRefuerzo;
        private readonly int _numeroBarra;
        #region 0)propiedades


        #endregion

        #region 1) Constructores




        public BarraRefuerzoBordeLibreSOLOTag(UIApplication uiapp, SeleccionarRebarElemento seleccionarRebarElemento, IGeometriaTag _iGeometriaTagRefuerzo, int numeroBarra) : base( uiapp, _iGeometriaTagRefuerzo)
        {
          //  this._uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            this._seleccionarRebarElemento = seleccionarRebarElemento;
            _rebar = seleccionarRebarElemento._RebarSeleccion;
            viewActual = _doc.ActiveView;
            this._iGeometriaTagRefuerzo = _iGeometriaTagRefuerzo;
            this._numeroBarra = numeroBarra;
        }

        public override void M2_ConfigurarBarraCurve()
        {
            throw new NotImplementedException();
        }

        public void Ejecutar(ConfiguracionTAgBarraDTo _configuracionTAgEstriboDTo)
        {

            cambiarParametrosCantidadBarra();
            DibujarTagRebarRefuerzoLosa(_configuracionTAgEstriboDTo);
        }

        public bool cambiarParametrosCantidadBarra()
        {


            string diamString = _rebar.ObtenerDiametroInt().ToString();  //get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsValueString().Replace("mm", "").Trim();

            Parameter ParameterCuantiaRefuerzo = ParameterUtil.FindParaByName(_rebar, "CuantiaRefuerzo");
            if (ParameterCuantiaRefuerzo == null) return false;

            string Antiguacuantia = ParameterCuantiaRefuerzo.AsString(); ;
            //            string nuevaCuantia = "F'=" + _numeroBarra + "Ø" + diamString;
            string nuevaCuantia = "F=F'=" + _numeroBarra;
            if (Antiguacuantia == nuevaCuantia) return true;



            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("modificar parametros rebarrefuerzo-NH");
                    if (ParameterUtil.FindParaByName(_rebar, "CuantiaRefuerzo") != null) ParameterUtil.SetParaInt(_rebar, "CuantiaRefuerzo", nuevaCuantia);

                    t.Commit();
                }
            }
            catch (Exception ex)

            {

                string msj = ex.Message;
                TaskDialog.Show("Error", msj);
                return false;
            }




            return true;

        }



        #endregion



    }
}
