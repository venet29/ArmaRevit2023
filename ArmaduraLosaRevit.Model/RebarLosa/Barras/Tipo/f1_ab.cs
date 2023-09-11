using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Servicio;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Tipo

{
    // para caso f1 con para hacia arriba o bajo, cuando se encuentra con rampa
    public class f1_ab : ARebarLosa, IRebarLosa
    {
#pragma warning disable CS0108 // 'f1_ab._uiapp' hides inherited member 'ARebarLosa._uiapp'. Use the new keyword if hiding was intended.
        private readonly UIApplication _uiapp;
#pragma warning restore CS0108 // 'f1_ab._uiapp' hides inherited member 'ARebarLosa._uiapp'. Use the new keyword if hiding was intended.
        private RebarInferiorDTO _RebarInferiorDTO;
        private BuscarLosaIncilnada _buscarLosaIncilnada;

        public f1_ab(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._uiapp = uiapp;
            //  this._uiapp = uiapp;
            //this._doc = _uiapp.ActiveUIDocument.Document;
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;

            ptoMouseAnivelVista = ptoMouseAnivelVista.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz1 = _rebarInferiorDTO.PtoDirectriz1.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz2 = _rebarInferiorDTO.PtoDirectriz2.AsignarZ(elevacionVIew);


        }

        public bool M1A_IsTodoOK()
        {

            if (!M0_ObtenerPendienteLosaContigua()) return false;
            if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;
            return true;
        }

        private bool M0_ObtenerPendienteLosaContigua()
        {

            _buscarLosaIncilnada = new BuscarLosaIncilnada(_uiapp, _rebarInferiorDTO.listaPtosPerimetroBarras, (Floor)_rebarInferiorDTO.floor, 100);

            switch (_RebarInferiorDTO.ubicacionLosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:
                    if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaInicia(new XYZ(0, 0, -1)))
                        _VectorDIreccionLosaInicialExternaInclinada = _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada;
                    else
                        return false;
                    break;
                case UbicacionLosa.Izquierda:
                case UbicacionLosa.Inferior:
                    if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaFinal(new XYZ(0, 0, -1)))
                        _VectorDIreccionLosaFinalExternaInclinada = _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada;
                    else
                        return false;
                    break;
                default:
                    return false;
            }
            return true;
        }

        #region COmprobacion 1

        public bool M1_1_DatosBarra3d()
        {
            switch (_RebarInferiorDTO.ubicacionLosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:
                    M1_1_1_ConfiguracionDereSup();
                    break;
                case UbicacionLosa.Izquierda:
                case UbicacionLosa.Inferior:
                    M1_1_1_ConfiguracionIzqInfer();
                    break;
                case UbicacionLosa.NONE:
                    return false;
                default:
                    return false;
            }

            return CopiandoParametrosLado_COnPAthSymbol();

        }

        private void M1_1_1_ConfiguracionDereSup()
        {
            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = ptoini.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            XYZ PE = (ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT) + _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada.Normalize() * _largoPataInclinada).AsignarZ(zRefencia);
            ladoAB = Line.CreateBound((ptofin + -direccionBarra * _patabarra).AsignarZ(zRefencia), ptofin);
            ladoBC = Line.CreateBound(ptofin, (ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia));
            ladoCD = Line.CreateBound((ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia), (ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia));
            ladoDE = Line.CreateBound((ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia), PE);
        }

        private void M1_1_1_ConfiguracionIzqInfer()
        {         
            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = ptoini.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo
            //  XYZ PE = Util.ExtenderPuntoRespectoVector3d(ptofin, direccionBarra, _RebarInferiorDTO.anguloTramoRad, _largoPataInclinada);
            ladoAB = Line.CreateBound((ptoini + direccionBarra * _patabarra).AsignarZ(zRefencia), ptoini);
            ladoBC = Line.CreateBound(ptoini, (ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia));
            ladoCD = Line.CreateBound((ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia), (ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia));

            XYZ PE = (ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT) + _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada.Normalize() * _largoPataInclinada).AsignarZ(zRefencia);
            ladoDE = Line.CreateBound((ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)).AsignarZ(zRefencia), PE);
        }

        #endregion


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {

            switch (_RebarInferiorDTO.ubicacionLosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:
                    M1_2_1_PAthSymbolFalsoDereSup();

                    break;
                case UbicacionLosa.Izquierda:
                case UbicacionLosa.Inferior:
                    M1_2_3_PAthSymbolFalsoIzqInfer();

                    break;
                case UbicacionLosa.NONE:
                    return false;
                default:
                    return false;
            }



            return true;
        }



        private void M1_2_1_PAthSymbolFalsoDereSup()
        {
            XYZ ptoini_PthSymb = _RebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            XYZ ptofin_PthSymb = _RebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);

            XYZ PE = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, Math.PI + _RebarInferiorDTO.anguloBarraRad + -_buscarLosaIncilnada.VectorDireccionLosaExternaInclinada.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada);

            ladoAB_pathSym = Line.CreateBound(ptofin_PthSymb - direccionBarra * _patabarra + dirBarraPerpen * Util.CmToFoot(18), ptofin_PthSymb + dirBarraPerpen * Util.CmToFoot(18));
            ladoBC_pathSym = Line.CreateBound(ptofin_PthSymb + dirBarraPerpen * Util.CmToFoot(18), ptofin_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptofin_PthSymb, ptoini_PthSymb);
            ladoDE_pathSym = Line.CreateBound(ptoini_PthSymb, PE);
            OBtenerListaFalsoPAthSymbol();
        }


        private void M1_2_3_PAthSymbolFalsoIzqInfer()
        {

            XYZ ptoini_PthSymb = _RebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            XYZ ptofin_PthSymb = _RebarInferiorDTO.barraFin.AsignarZ(elevacionVIew); //Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraFin, _RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera);

            XYZ PE = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _RebarInferiorDTO.anguloBarraRad + _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada);

            ladoAB_pathSym = Line.CreateBound(ptoini_PthSymb + direccionBarra * _patabarra + dirBarraPerpen * Util.CmToFoot(18), ptoini_PthSymb + dirBarraPerpen * Util.CmToFoot(18));
            ladoBC_pathSym = Line.CreateBound(ptoini_PthSymb + dirBarraPerpen * Util.CmToFoot(18), ptoini_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ladoDE_pathSym = Line.CreateBound(ptofin_PthSymb, PE);

            OBtenerListaFalsoPAthSymbol();
        }


        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerNuevoptoCirculo();

            ObtenerPAthSymbolTAG();
            return true;
        }


        //public void M2A_GenerarBarra()
        //{
        //    M1_ConfigurarDatosIniciales();
        //    if (M3_DibujarBarraCurve() != Result.Succeeded) return;
        //    if (_rebar == null)
        //    {
        //        Util.ErrorMsg("Error al crear rebar. Rebar igual null");
        //        return;
        //    }

        //    M3A_1_CopiarParametrosCOmpartidos();
        //    //parametros no son correctos
        //    // M4_ConfigurarAsignarParametrosRebarshape();
        //    M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing();
        //    M6_visualizar();
        //    M8_CrearPatSymbolFalso();
        //    M9_CreaTAg();
        //    M10_CreaDimension();
        //    M11_CreaCirculo();
        //    M11_CrearGrupo();
        //    M12_MOverHaciaBajo();

        //}


    }
}
