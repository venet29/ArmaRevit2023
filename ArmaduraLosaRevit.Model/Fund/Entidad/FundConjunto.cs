using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.Entidad

{
    public class FundConjunto
    {
        private readonly UIApplication _uiapp;
        private readonly FundGeoDTO _fundGeoDTO;
        private XYZ _ptoMouse;
        public List<FundIndividual> ListaCaraAnalizada { get; set; }

        public FundIndividual FundacionUnicoSeleccoinado { get; set; }

        public FundConjunto(UIApplication uiapp, FundGeoDTO fundGeoDTO)
        {
            this._uiapp = uiapp;
            this._fundGeoDTO = fundGeoDTO;
            this._ptoMouse = fundGeoDTO.ptoSeleccionFund;
        }



        public bool Ejecutar()
        {
            if (!M2_OBtenerBordeOpeningSeleccionado()) return false;
            if (!M3_ObtenerShaftIndividual()) return false;

            return true;
        }

        public bool M2_OBtenerBordeOpeningSeleccionado()
        {
            try
            {
                if ((_fundGeoDTO.FaceAnalizada == null) || (_fundGeoDTO.FaceAnalizada == null))
                { Util.ErrorMsg("Una de las caras superio o inferior del Opening es NULL"); return false; }
                var listaAux = _fundGeoDTO.FaceAnalizada.GetEdgesAsCurveLoops();
                ListaCaraAnalizada = _fundGeoDTO.FaceAnalizada.GetEdgesAsCurveLoops().Select(c => new FundIndividual(_uiapp, c)).ToList();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'OBtenerBordeOpeningSeleccionado'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool M3_ObtenerShaftIndividual()
        {
            try
            {
                FundacionUnicoSeleccoinado = ListaCaraAnalizada.Where(shi => shi.IsPtoDentroShaf(_ptoMouse)).DefaultIfEmpty(new FundIndividual()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerShaftIndividual'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
