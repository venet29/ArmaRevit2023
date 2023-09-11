using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Tipo

{
    public class f3_fund : ARebarLosa, IRebarLosa
    {
        private RebarInferiorDTO _RebarInferiorDTO;


        private double _extHorizontalDentroEscalera;

#pragma warning disable CS0108 // 'f3_esc135._largoPataInclinada' hides inherited member 'ARebarLosa._largoPataInclinada'. Use the new keyword if hiding was intended.
        private double _largoPataInclinada;
#pragma warning restore CS0108 // 'f3_esc135._largoPataInclinada' hides inherited member 'ARebarLosa._largoPataInclinada'. Use the new keyword if hiding was intended.
        private double _espesorSinRecubEscalera;
        private double _pataEscalera;

        public f3_fund(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;

          //  _RebarInferiorDTO.anguloTramoRad = -_RebarInferiorDTO.anguloTramoRad;

            // ptoini    --  ptofin
            _extHorizontalDentroEscalera = Util.CmToFoot(1);

            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _espesorSinRecubEscalera = Util.CmToFoot(11);
            _pataEscalera = Util.CmToFoot(11);

            _Prefijo_F = "F=";
            TipoDireccionBarra_ = _rebarInferiorDTO.TipoDireccionBarra_;
        }

        public bool M1A_IsTodoOK()
        {
            if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;
            return true;
        }

        #region COmprobacion 1

        public bool M1_1_DatosBarra3d()
        {   
            M1_1_1_Configuracion();
            return CopiandoParametrosLado_COnPAthSymbol();
        }


        private void M1_1_1_Configuracion() => ladoBC = Line.CreateBound(ptoini, ptofin);


        #endregion

        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {
            M1_2_1_PAthSymbolFalsoDereSup();
            return true;
        }

        private void M1_2_1_PAthSymbolFalsoDereSup()
        {
            XYZ ptoini_PthSymb = _RebarInferiorDTO.barraIni;
            XYZ ptofin_PthSymb = _RebarInferiorDTO.barraFin;

            ladoBC_pathSym = Line.CreateBound(ptoini_PthSymb,ptofin_PthSymb);
       
            OBtenerListaFalsoPAthSymbol();
        }

        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerPAthSymbolTAG();
            return true;
        }

    }
}
