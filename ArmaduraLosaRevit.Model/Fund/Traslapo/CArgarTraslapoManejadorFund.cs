using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.Traslapo
{
    public  class CArgarTraslapoManejadorFund
    {
        private  UIApplication _uiapp;

        public CArgarTraslapoManejadorFund(UIApplication uiapp)
        {
            this._uiapp = uiapp;
        }

        public Result Execute()
        {
 

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            try
            {
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiapp);
                if (!seleccionarPathReinfomentConPto.SeleccionarPathReinformentFund()) return Result.Failed;


                CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO = new CalcularLargoTraslapoPAthDTO()
                {

                    pathDefinir = TipoPAthDefinir.Mitad,
                    largopathFoot = Util.CmToFoot(200),
                    IsDefinirLargo = false
                };
                //cargardatos
                PathReinformeTraslapoManejadorFund pathReinformeTraslapo = new PathReinformeTraslapoManejadorFund(_uiapp, seleccionarPathReinfomentConPto, _CalcularLargoPAthDTO);
                pathReinformeTraslapo.M0_EjecutarTraslapoFund();


                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                string msje = ex.Message;
                return Result.Failed;
            }


        }
    }
}
