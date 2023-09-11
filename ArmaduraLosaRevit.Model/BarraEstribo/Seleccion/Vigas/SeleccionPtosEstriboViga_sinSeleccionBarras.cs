using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.DTO;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Seleccion.Vigas
{
    public class SeleccionPtosEstriboViga_sinSeleccionBarras : SeleccionPtosEstriboViga
    {
        
        public SeleccionPtosEstriboViga_sinSeleccionBarras(UIApplication _uiapp, View3D _view3D_paraBuscar_, DatosConfinamientoAutoDTO configuracionInicialEstriboDTO)
            : base(_uiapp, _view3D_paraBuscar_, configuracionInicialEstriboDTO)
        {

        }


        public bool M1_Ejecutar_SeleccionViga()
        {
            IsOk = false;
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            //  if (!M1_2_SeleccionarBordeMuro()) return false;
            if (!M1_3_SeleccionarMuroElement()) return false;
            //if (!M1_4_BuscarPtoInicioBase()) return false;
            //  if (!M1_3_SeleccionarRebarElement()) return false;
            if (!M1_4_ObtenerPtoInicialYfinalViga()) return false;
            //if (!M1_3a_ObtenerRangoLevelSeleccionado()) return false;

            //if (!(M1_4_BuscarMuroPerpendicularVIew(_ptoSeleccionMouseCentroCaraMuro))) return false;
            _wallSeleccionado = _ElemetSelect;
            _espesorMuroVigaFoot = _ElemetSelect.ObtenerEspesorConPtos_foot(_ptoSeleccionMouseCaraMuro, _view.ViewDirection);

            if (!(M1_4_2_BuscarPuntosSegunMargen(_ptoSeleccionMouseCaraMuro))) return false;

            if (!M1_5_GenerarVectores()) return false;

            M1_6_NombreFamilaTAG();

            IsOk = true;
            return true;

        }

        public bool M1_Ejecutar_SeleccionViga(Element _elemetSelect)
        {

            var planarFrontal = _elemetSelect.ObtenerCaraSegun_IgualDireccion_MasCercamo(_view.ViewDirection);
            var  _ptoCentral = planarFrontal.ObtenerCenterDeCara();

            return M1_Ejecutar_SeleccionViga(_elemetSelect, _ptoCentral);
        }

            public bool M1_Ejecutar_SeleccionViga(Element _elemetSelect, XYZ _ptoseleccion)
        {
            IsOk = false;
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;
            _ptoSeleccionMouseCentroCaraMuro = _ptoseleccion;
            _ElemetSelect = _elemetSelect;

            m1_3_1_AuxObtenerMuros(_ElemetSelect);

            if (!M1_4_ObtenerPtoInicialYfinalViga()) return false;
            //if (!M1_3a_ObtenerRangoLevelSeleccionado()) return false;

            //if (!(M1_4_BuscarMuroPerpendicularVIew(_ptoSeleccionMouseCentroCaraMuro))) return false;
            _wallSeleccionado = _ElemetSelect;
            _espesorMuroVigaFoot = _ElemetSelect.ObtenerEspesorConPtos_foot(_ptoSeleccionMouseCentroCaraMuro, _view.ViewDirection);

            if (!(M1_4_2_BuscarPuntosSegunMargen(_ptoSeleccionMouseCentroCaraMuro))) return false;

            if (!M1_5_GenerarVectores()) return false;

            M1_6_NombreFamilaTAG();

            IsOk = true;
            return true;

        }
        private bool M1_4_ObtenerPtoInicialYfinalViga()
        {
            try
            {
                //ver imagenA
                var carasup = _ElemetSelect.ObtenerPLanarFAce_superior();
                var caraInf = _ElemetSelect.ObtenerCaraInferior();

                double espesor = _ElemetSelect.ObtenerEspesorConPtos_foot(_ptoSeleccionMouseCentroCaraMuro, _view.ViewDirection);
                _anchoEstribo1Foot_extremoAextremo = espesor - ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT * 2 + Util.MmToFoot(_configuracionInicialEstriboDTO.DiamtroEstriboMM);  // ancho viga - 2recub -2 * diam/2
                _anchoEstribo2Foot_extremoAextrem = _anchoEstribo1Foot_extremoAextremo;
                var carafInical = _ElemetSelect.ObtenerCaraSegun_IgualDireccion_MasLejano(-_view.RightDirection);

                var _ptoAux = carafInical.GetPtosIntersFaceUtilizarPlanoNh(_ptoSeleccionMouseCentroCaraMuro);
                //_ptoSeleccionMouseCaraMuro = carafInical.ObtenerPtosInterseccionFace(_ptoSeleccionMouseCaraMuro, -_view.RightDirection);

                _ptobarra1 = carafInical.ObtenerPtosInterseccionFace(_ptoAux, -_view.RightDirection, true).AsignarZ(caraInf.Origin.Z + ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT); 

                var carafinal = _ElemetSelect.ObtenerCaraSegun_IgualDireccion_MasLejano(_view.RightDirection);
                _ptobarra2 = carafinal.GetPtosIntersFaceUtilizarPlanoNh(_ptoAux).AsignarZ(carasup.Origin.Z - ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);

                M1_3_1_OrientacionSeleccion();
                M1_3_2_ReasignarSiP1MayorZqueP2();
                M1_3_3_ReasignarSiP2MasCercaOriengeViewSeccion();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'M1_4_ObtenerPtoInicialYfinalViga'.\n Ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public CoordenadasElementoDTO M1_5_ObtenerPtosDeElemento()
        {
            double recub = ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT;
            var carasup_paraPto1 = _ptobarra1.AsignarZ(_ptobarra2.Z);// _ElemetSelect.ObtenerPtoInterseccionCara_segunDireccion(_ptoSeleccionMouseCentroCaraMuro, new XYZ(0,0,1));
            var carainf_paraPto2 = _ptobarra2.AsignarZ(_ptobarra1.Z); ;// _ElemetSelect.ObtenerPtoInterseccionCara_segunDireccion(_ptobarra1, new XYZ(0, 0, 1));

         //   var aux_PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = (_ptobarra2 + carasup_paraPto1) / 2 + (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
            return new CoordenadasElementoDTO()
            {
                _ElemetSelect = _ElemetSelect,
                _ptobarra1Inf_conrecub = _ptobarra1,
                _ptobarra1Sup_conrecub = carasup_paraPto1,
                _ptobarra2Inf_conrecub = carainf_paraPto2,
                _ptobarra2Sup_conrecub = _ptobarra2,
                _ptoCentroElemento_conrecub = (_ptobarra2 + _ptobarra1) / 2,
   
            };
        }
    }
}
