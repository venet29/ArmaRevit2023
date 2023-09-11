using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.RebarLosa.Geom;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Barras;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Fund.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Fund.Entidad;
using ArmaduraLosaRevit.Model.RebarFundaciones.Geom;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.PathReinf.DTO;

namespace ArmaduraLosaRevit.Model.RebarLosa
{

    public class DatosBarrasDTO
    {
        public double diamtro_mm { get; set; }
        public double espacimiento_foot { get; set; }
        public string tipoBArras { get;  set; } // "f11" -> inferior  //   "f10"  superior
    }

    public class ManejadorRebarFund: ManejadorRebarBase
    {

        private DatosNuevaBarraDTO _datosNuevaBarraDTO;
        private SeleccionarFundConMouse _seleccionarFundConMouse;
        private PathReinfSeleccionDTO _PathReinfSeleccionDTO;

        public ManejadorRebarFund(UIApplication uiapp,  DatosNuevaBarraDTO _DatosNuevaBarraDTO, SeleccionarFundConMouse seleccionarFundConMouse) : base(uiapp)
        {
 
            this._datosNuevaBarraDTO = _DatosNuevaBarraDTO;
            this._seleccionarFundConMouse = seleccionarFundConMouse;
        }
        public bool DibujarBarra(FundIndividual fundIndividuas, UbicacionLosa _UbicacionLosa, TipoCaraUbicacion TipoUbicacionFund)
        {     
            try
            {
                //2)
                GenerarGeometriaFund _GenerarGeometriaSimple = new GenerarGeometriaFund(_uiapp, fundIndividuas, _seleccionarFundConMouse);
                if (!_GenerarGeometriaSimple.Ejecutar( UbicacionPtoMouse.inferior, _datosNuevaBarraDTO, _UbicacionLosa)) return false;
                
                ObtenerGEometriaDTO _ObtenerGEometriaDTO = new ObtenerGEometriaDTO()
                {
                    ListaPtosPerimetroBarras = fundIndividuas.ListaVertices,
                    TipoUbicacionFund = _datosNuevaBarraDTO.TipoCaraObjeto_ ,
                    barraMenos = 0
                };

                RebarInferiorDTO rebarInferiorDTO1 = _GenerarGeometriaSimple.ObtenerGEometria(_ObtenerGEometriaDTO);
                if (rebarInferiorDTO1.IsOK == false) return false;

                //rebarInferiorDTO1.barraIni = rebarInferiorDTO1.barraIni + XYZ.BasisZ * ConstNH.RECUBRIMIENTO_FUNDACIONES_foot;
                //rebarInferiorDTO1.barraFin = rebarInferiorDTO1.barraFin + XYZ.BasisZ * ConstNH.RECUBRIMIENTO_FUNDACIONES_foot;
                rebarInferiorDTO1.espesorBarraEnLOsa_sinRecub_FooT = rebarInferiorDTO1.espesorBarraEnLOsa_sinRecub_FooT
                                                                    - Util.MmToFoot( rebarInferiorDTO1.diametroMM)/2
                                                                    -(ConstNH.RECUBRIMIENTO_FUNDACIONES_foot - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm));

                rebarInferiorDTO1.DatosNuevaBarraDTO = _datosNuevaBarraDTO;
                _PathReinfSeleccionDTO = new PathReinfSeleccionDTO()
                {
                    ListaPtosPerimetroBarras = fundIndividuas.ListaVertices,
                    ptoConMouse = _datosNuevaBarraDTO.PtoMouse,
                    PtoCodoDireztriz = _datosNuevaBarraDTO.PtoCodoDireztriz,
                    PtoDireccionDireztriz = _datosNuevaBarraDTO.PtoDireccionDirectriz,
                    PtoLadoLibre = _datosNuevaBarraDTO.LeaderEnd,
                    IsLadoLibre = true,
                    PtoTag = _datosNuevaBarraDTO.PtoTag,
                    

                };

                GenerarBarra_Contras_fund(rebarInferiorDTO1);

                if (_ListIRebarLosa.Count == 0) return false;
                
                GenerarBarras_ConTras_SegundaPArte();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");

            }
            return true;
        }


        protected bool GenerarBarra_Contras_fund(RebarInferiorDTO rebarInferiorDTO1)
        {
            try
            {
                XYZ DireccionEfierrado= (rebarInferiorDTO1.PtoDirectriz2- rebarInferiorDTO1.PtoDirectriz1).Normalize();
                XYZ puntotag = (rebarInferiorDTO1.barraIni + rebarInferiorDTO1.barraFin) / 2;
                IGeometriaTag _newIGeometriaTag = FactoryGeomTag.
                    CrearGeometriaTag_RebarFundacionesAUto(_uiapp, _datosNuevaBarraDTO.TipoPataFun, _datosNuevaBarraDTO.PtoMouse, _PathReinfSeleccionDTO);
                if (_newIGeometriaTag == null) return false;

                //4)barra
                IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
                if (!rebarLosa.M1A_IsTodoOK()) return false;

                _ListIRebarLosa.Add(rebarLosa);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

    }
}
