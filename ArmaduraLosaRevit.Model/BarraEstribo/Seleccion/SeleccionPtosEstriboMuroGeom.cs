using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Buscar;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Seleccion
{
    public class SeleccionPtosEstriboMuroGeom : SeleccionPtosEstriboMuro
    {
        public Element MuroParaleloView { get; set; }
        public SeleccionPtosEstriboMuroGeom(UIApplication _uiapp, View3D _view3D_paraBuscar,
                                        DatosConfinamientoAutoDTO configuracionInicialEstriboDTO, ISeleccionarNivel _seleccionarNivel)
            : base(_uiapp, _view3D_paraBuscar, configuracionInicialEstriboDTO, _seleccionarNivel)
        {

        }

        public override bool M1_Ejecutar()
        {
            IsOk = false;
            if (!M1_1_CrearWorkPLane_EnCentroViewSecction()) return false;

            if (!M1_3a_SeleccionarRebarElement()) return false;

            if (!M1_4b_1_OrientacionSeleccion()) return false;

            //if (!M1_4_BuscarMuroPerpendicularVIew()) return false;

            if (!M1_40_ObtenerAnchoEstribo()) return false;

            if (!M1_4a_ReCalcularP1P2_conRecubrimieto()) return false;

            if (!M1_4b_SeleccionarRebarElement()) return false;

            if (!M1_4c_ObtenerRangoLevelSeleccionado()) return false;

            if (!M1_5_GenerarVectores()) return false;

            M1_6_NombreFamilaTAG();

            IsOk = true;
            return true;

        }

        public override bool M1_3a_SeleccionarRebarElement()
        {
            try
            {
                _confiEnfierradoDTO = new ConfiguracionIniciaWPFlBarraVerticalDTO();
                _confiEnfierradoDTO.TipoSelecion = TipoSeleccion.ConElemento;

                //1) pto inicial
                IsSalirSeleccionPilarMuro = true;
                M2_SeleccionarPtoInicio();
                if (IsSalirSeleccionPilarMuro == false) return false;
                _ptobarra1 = _PtoInicioIntervaloBarra;
                _ptoSeleccionMouseCentroCaraMuro = _PtoInicioIntervaloBarra;
                _anchoEstribo1Foot = 0; ;


                //2) pto final)
                SelecionarPtoSup selecionarPtoSup = new SelecionarPtoSup(_uiapp, _confiEnfierradoDTO, new List<Level>());
                selecionarPtoSup._PtoInicioIntervaloBarra = _PtoInicioIntervaloBarra;
                //selecionarPtoSup.SeleccionarPtoSuperior(_PtoInicioBaseBordeMuro_ProyectadoCaraMuroHost, _ptoSeleccionMouseCentroCaraMuro);

                if (selecionarPtoSup.SeleccionarPtoFin(_ptoSeleccionMouseCentroCaraMuro, new FiltroVIga_Muro_Rebar_Columna())) return false;
                _ptobarra2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(NormalCaraElemento, _ptoSeleccionMouseCentroCaraMuro, selecionarPtoSup._PtoFinalIntervaloBarra);
                _anchoEstribo2Foot = 0;

                _direccionBarra = (_ptobarra2.GetXY0() - _ptoSeleccionMouseCentroCaraMuro.GetXY0()).Normalize();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }

        private bool M1_40_ObtenerAnchoEstribo()
        {
            try
            {
                var XpuntoBUScar = ((_ptobarra1 + _ptobarra2) / 2) + _view.ViewDirection * Util.CmToFoot(5);

                if (Math.Abs(_ptobarra2.Z - _ptobarra1.Z) > Util.CmToFoot(300))
                {
                    XpuntoBUScar = XpuntoBUScar.AsignarZ(_ptobarra1.Z + Util.CmToFoot(200));
                }

                BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, Util.CmToFoot(40));
                XYZ ptoSobreMuro = XYZ.Zero;
                (_wallSeleccionado, _espesorMuroVigaFoot, ptoSobreMuro)
                    = BuscarMuros.OBtenerRefrenciaMuro(_view3D_paraBuscar, XpuntoBUScar, -_view.ViewDirection);

                double recub = ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT;


                XYZ VectorFaceNormal = _view.ViewDirection6();
                if (_wallSeleccionado == null)
                {
                    Util.ErrorMsg("No se encontro Muro de referencia, seleccionar muro para enfierrar con estribo ");
                    M1_3_SeleccionarMuroElement();
                    _espesorMuroVigaFoot = _ElemetSelect.ObtenerAnchoConPtos(_ptoSeleccionMouseCentroCaraMuro, -_view.ViewDirection);
                    ptoSobreMuro = _ptoSeleccionMouseCentroCaraMuro;
                    if (!AyudaObtenerNormarPlanoVisisible.Obtener(_ElemetSelect, _view)) return false;
                    VectorFaceNormal = AyudaObtenerNormarPlanoVisisible.FaceNormal;

                    _wallSeleccionado = _ElemetSelect;
                    _espesorMuroVigaFoot = _wallSeleccionado.ObtenerEspesorConCaraVerticalVIsible_foot(_view);
                
                }

                XYZ ptoSobreMuro_menosRecub = ptoSobreMuro - _view.ViewDirection * recub;

                _ptobarra1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(VectorFaceNormal, ptoSobreMuro_menosRecub, _ptobarra1);
               // _ptobarra1 = _ptoSeleccionMouseCentroCaraMuro;
                _ptobarra2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(VectorFaceNormal, ptoSobreMuro_menosRecub, _ptobarra2);

                _anchoEstribo1Foot = _espesorMuroVigaFoot - recub * 2;
                _anchoEstribo2Foot = _espesorMuroVigaFoot - recub * 2;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }

    }
}
