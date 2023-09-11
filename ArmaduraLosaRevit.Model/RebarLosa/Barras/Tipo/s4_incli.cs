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
    public class s4_incli : ARebarLosa, IRebarLosa
    {
        private RebarInferiorDTO _RebarInferiorDTO;
        private double _extHorizontalDentroEscalera;
        private BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto_IzqInf;
        private double _espesorEnPuntoSeleccion_sinRecb_SinDiam_foot;
        private double _espesorEnPuntoSeleccion_sinRecb_foot;

        public s4_incli(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {

            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _extHorizontalDentroEscalera = Util.CmToFoot(1);

            _espesorEnPuntoSeleccion_sinRecb_SinDiam_foot = 0;
            _espesorEnPuntoSeleccion_sinRecb_foot = 0;

            _largoPataInclinada = Util.CmToFoot(ConstNH.CONST_PATA_SX); // Util.MmToFoot(20 * _rebarInferiorDTO.diametroMM + 100)

            _Prefijo_F = "F'=";
            TipoDireccionBarra_ = _rebarInferiorDTO.TipoDireccionBarra_;
        }

        public bool M1A_IsTodoOK()
        {
            //la para la define a 15 cm, inicalmente el '_patabarra' viene con 15%
            _patabarra = _largoPataInclinada;

            if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;
            return true;
        }

        #region COmprobacion 1 rebar

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

            return true;

        }


        private void M1_1_1_ConfiguracionDereSup()
        {
            M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(_RebarInferiorDTO.barraIni, -direccionBarra);

            BUScandoEspesorEnPutnoSeleciondo();

            ptoini = Util.ExtenderPuntoRespectoVector3d(ptoini, -direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);


            // ladoAB = Line.CreateBound(ptofin + -direccionBarra * _patabarra + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsaFooT), ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsaFooT));
            ladoAB = Line.CreateBound(ptofin + -direccionBarra * _largoPataInclinada + new XYZ(0, 0, -_espesorEnPuntoSeleccion_sinRecb_SinDiam_foot), ptofin + new XYZ(0, 0, -_espesorEnPuntoSeleccion_sinRecb_SinDiam_foot));
            ladoBC = Line.CreateBound(ptofin + new XYZ(0, 0, -_espesorEnPuntoSeleccion_sinRecb_SinDiam_foot), ptofin);
            ladoCD = Line.CreateBound(ptofin, ptoini);
        }

        private void M1_1_1_ConfiguracionIzqInfer()
        {
            M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(_RebarInferiorDTO.barraFin, direccionBarra);
            BUScandoEspesorEnPutnoSeleciondo();

            ptofin = Util.ExtenderPuntoRespectoVector3d(ptofin, direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);
            //ladoAB = Line.CreateBound(ptoini + direccionBarra * _patabarra + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsaFooT), ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsaFooT));
            ladoAB = Line.CreateBound(ptoini + direccionBarra * _largoPataInclinada + new XYZ(0, 0, -_espesorEnPuntoSeleccion_sinRecb_SinDiam_foot), ptoini + new XYZ(0, 0, -_espesorEnPuntoSeleccion_sinRecb_SinDiam_foot));
            ladoBC = Line.CreateBound(ptoini + new XYZ(0, 0, -_espesorEnPuntoSeleccion_sinRecb_SinDiam_foot), ptoini);
            ladoCD = Line.CreateBound(ptoini, ptofin);
        }

        private void BUScandoEspesorEnPutnoSeleciondo()
        {
            var _auxEspesorLosaConPunto = _rebarInferiorDTO.floor.ObtenerEspesorConPtos_foot(_rebarInferiorDTO.ptoSeleccionMouse, -XYZ.BasisZ);

            if (_auxEspesorLosaConPunto != 0)
            {
                _espesorEnPuntoSeleccion_sinRecb_foot = _auxEspesorLosaConPunto - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_SUP_cm) - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm);
                _espesorEnPuntoSeleccion_sinRecb_SinDiam_foot = _espesorEnPuntoSeleccion_sinRecb_foot - _rebarInferiorDTO.diametroFoot;

            }
            else
            {
                _espesorEnPuntoSeleccion_sinRecb_foot = _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT;
                _espesorEnPuntoSeleccion_sinRecb_SinDiam_foot = _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT - _rebarInferiorDTO.diametroFoot;
            }


            // _buscarFaceSuperiorConPto_IzqInf = new BuscarFaceSuperiorConPto(base._uiapp, _rebarInferiorDTO, direccionBarra);
            // //a)
            //if (_buscarFaceSuperiorConPto_IzqInf.ObtenerDirecionDeLosa(_rebarInferiorDTO.ptoSeleccionMouse, PosicionDeBusqueda.Inicio))
            // {
            //     _espesorEnPuntoSeleccion_foot = _buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorSinRecub_foot;               
            // }

        }
        private void M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(XYZ ptoExtendida, XYZ direccionBarra)
        {


            BuscarPtoProyectadoDentroEscalera _buscarPtoProyectadoDentroEscalera = new BuscarPtoProyectadoDentroEscalera(_uiapp);
            //pto desplazado 2.5 cm hacia abajo
            XYZ _PtoInicioBusquedaEscalera_haciaDereSup = ptoExtendida.AsignarZ(ptoExtendida.Z - ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT);

            if (_buscarPtoProyectadoDentroEscalera.M1_BuscarPtoProyectadoEnCaraInferiorEScalera(_PtoInicioBusquedaEscalera_haciaDereSup, direccionBarra))
            {
                //obtiene la direccion del pto encontrado  con respecto al pto de refericnia
                XYZ vectorDIreccionPtoENonctrado = _buscarPtoProyectadoDentroEscalera._buscarPtbProyeccionEnEscalera.PtoProyectadoCaraInferior.GetXY0() - _PtoInicioBusquedaEscalera_haciaDereSup.GetXY0();

                //si direccion estan en contra de la direccion de la barra pq esta mas mas atras (hacia la losa)
                //si direccion estan a favor de la direccion de la barra pq esta mas hacia exterior (hacia interior de la escalera)
                _extHorizontalDentroEscalera = (Util.GetProductoEscalar(vectorDIreccionPtoENonctrado.Normalize(), direccionBarra.Normalize()) > 0 ? _buscarPtoProyectadoDentroEscalera._buscarPtbProyeccionEnEscalera.DistanciaHorizontalCaraInferior :
                                                                                -_buscarPtoProyectadoDentroEscalera._buscarPtbProyeccionEnEscalera.DistanciaHorizontalCaraInferior);
                _RebarInferiorDTO.anguloTramoRad = _buscarPtoProyectadoDentroEscalera.anguloDeCaraInferior;
            }
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

            XYZ ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraIni, Math.PI + -_RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera);
            XYZ ptofin_PthSymb = _RebarInferiorDTO.barraFin;


            // borrar el parametro 'zRefencia' que aisgna un mismo valor
            ConstNH.corte();
            double zRefencia = ptoini_PthSymb.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            //_RebarInferiorDTO.anguloTramoRad es negativo

            XYZ Pa_aux = (ptofin_PthSymb - direccionBarra * _patabarra - dirBarraPerpen * _espesorEnPuntoSeleccion_sinRecb_foot).AsignarZ(zRefencia);
            XYZ Pb_aux = (ptofin_PthSymb - dirBarraPerpen * _espesorEnPuntoSeleccion_sinRecb_foot).AsignarZ(zRefencia);

            ladoAB_pathSym = Line.CreateBound(Pa_aux, Pb_aux);
            ladoBC_pathSym = Line.CreateBound(Pb_aux, ptofin_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptofin_PthSymb, ptoini_PthSymb);

            OBtenerListaFalsoPAthSymbol();
        }
        private void M1_2_3_PAthSymbolFalsoIzqInfer()
        {
            XYZ ptoini_PthSymb = _RebarInferiorDTO.barraIni;
            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = ptoini_PthSymb.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            XYZ ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraFin, _RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera);


            XYZ Pa_aux = (ptoini_PthSymb + direccionBarra * _patabarra - dirBarraPerpen * _espesorEnPuntoSeleccion_sinRecb_foot).AsignarZ(zRefencia);
            XYZ Pb_aux = (ptoini_PthSymb - dirBarraPerpen * _espesorEnPuntoSeleccion_sinRecb_foot).AsignarZ(zRefencia);

            ladoAB_pathSym = Line.CreateBound(Pa_aux, Pb_aux);
            ladoBC_pathSym = Line.CreateBound(Pb_aux, ptoini_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);

            OBtenerListaFalsoPAthSymbol();

            //ListaFalsoPAthSymbol.Add(ladoAB);
            //ListaFalsoPAthSymbol.Add(ladoBC);
            //ListaFalsoPAthSymbol.Add(ladoCD);

        }
        #endregion

        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerPAthSymbolTAG();
            return true;
        }

    }
}
