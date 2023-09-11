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
    public class f1_esc45_conpata : ARebarLosa, IRebarLosa
    {
        private RebarInferiorDTO _RebarInferiorDTO;

        private double _extHorizontalDentroEscalera;
        private double _espesorSinRecubEscalera;
        private double _pataEscalera;

        public f1_esc45_conpata(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;


            _RebarInferiorDTO.anguloTramoRad = -_RebarInferiorDTO.anguloTramoRad;

            // ptoini    --  ptofin

            _extHorizontalDentroEscalera = Util.CmToFoot(1);

            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _espesorSinRecubEscalera = Util.CmToFoot(11);
            _pataEscalera = Util.CmToFoot(11);

            _Prefijo_F = "F'=";
            TipoDireccionBarra_ = _rebarInferiorDTO.TipoDireccionBarra_;
        }

        public bool M1A_IsTodoOK()
        {
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

            return true;// CopiandoParametrosLado_COnALargoRebar();

        }


        private void M1_1_1_ConfiguracionDereSup()
        {
            M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(_RebarInferiorDTO.barraIni, -direccionBarra);

            ptoini = Util.ExtenderPuntoRespectoVector3d(ptoini, -direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);

            XYZ PE = Util.ExtenderPuntoRespectoVector3d(ptoini, -direccionBarra, _RebarInferiorDTO.anguloTramoRad, _largoPataInclinada);
            ladoAB = Line.CreateBound((ptofin + -direccionBarra * _patabarra + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT))
                                        , ( ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)));
            ladoBC = Line.CreateBound((ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)), ptofin);
            ladoCD = Line.CreateBound(ptofin, ptoini);
            ladoDE = Line.CreateBound(ptoini, PE);
            XYZ PF = Util.ExtenderPuntoRespectoVector3d(PE, direccionBarra, _RebarInferiorDTO.anguloTramoRad + 0, _espesorSinRecubEscalera);
            ladoEG = Line.CreateBound(PE, PF);
            XYZ PG = Util.ExtenderPuntoRespectoVector3d(PF, direccionBarra, -_RebarInferiorDTO.anguloTramoRad, _pataEscalera);
            ladoGH = Line.CreateBound(PF, PG);
        }

        private void M1_1_1_ConfiguracionIzqInfer()
        {
            M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(_RebarInferiorDTO.barraFin, direccionBarra);

            ptofin = Util.ExtenderPuntoRespectoVector3d(ptofin, direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);

            // ptofin = Util.ExtenderPuntoRespectoVector3d(ptofin, direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);
            XYZ PE = Util.ExtenderPuntoRespectoVector3d(ptofin, direccionBarra, _RebarInferiorDTO.anguloTramoRad, _largoPataInclinada);
            ladoAB = Line.CreateBound((ptoini + direccionBarra * _patabarra + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)),
                                      (ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)));
            ladoBC = Line.CreateBound((ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT)), ptoini);
            ladoCD = Line.CreateBound(ptoini, ptofin);
            ladoDE = Line.CreateBound(ptofin, PE);
            XYZ PG = Util.ExtenderPuntoRespectoVector3d(PE, direccionBarra, _RebarInferiorDTO.anguloTramoRad - Math.PI / 2, _espesorSinRecubEscalera);
            ladoEG = Line.CreateBound(PE, PG);
            XYZ PH = Util.ExtenderPuntoRespectoVector3d(PG, -direccionBarra, -_RebarInferiorDTO.anguloTramoRad, _pataEscalera);
            ladoGH = Line.CreateBound(PG, PH);
        }

        /// <summary>
        /// obtiene la distancia que se extiende el pto final
        /// </summary>
        private void M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(XYZ ptoExtendida,XYZ direccionBarra)
        {
            BuscarPtoProyectadoDentroEscalera _buscarPtoProyectadoDentroEscalera = new BuscarPtoProyectadoDentroEscalera(_uiapp);
            //pto desplazado 2.5 cm hacia abajo
            XYZ _PtoInicioBusquedaEscalera_haciaDereSup = ptoExtendida.AsignarZ(ptoExtendida.Z - ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT);

            if (_buscarPtoProyectadoDentroEscalera.M1_BuscarPtoProyectadoEnCaraSuperiorEScalera(_PtoInicioBusquedaEscalera_haciaDereSup, direccionBarra))
            {
                //obtiene la direccion del pto encontrado  con respecto al pto de refericnia
                XYZ vectorDIreccionPtoENonctrado = _buscarPtoProyectadoDentroEscalera._buscarPtbProyeccionEnEscalera.PtoProyectadoCaraSuperior.GetXY0() - _PtoInicioBusquedaEscalera_haciaDereSup.GetXY0();

                //si direccion estan en contra de la direccion de la barra pq esta mas mas atras (hacia la losa)
                //si direccion estan a favor de la direccion de la barra pq esta mas hacia exterior (hacia interior de la escalera)
                _extHorizontalDentroEscalera = (Util.GetProductoEscalar(vectorDIreccionPtoENonctrado.Normalize(), direccionBarra.Normalize()) > 0 ? _buscarPtoProyectadoDentroEscalera._buscarPtbProyeccionEnEscalera.DistanciaHorizontalCaraSuperior :
                                                                                -_buscarPtoProyectadoDentroEscalera._buscarPtbProyeccionEnEscalera.DistanciaHorizontalCaraSuperior);
                _RebarInferiorDTO.anguloTramoRad = -_buscarPtoProyectadoDentroEscalera.anguloDeCaraInferior;
            }
        }

        #endregion

        #region comprobacion2 pathSymbol

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


            return true;// CopiandoParametrosLado_COnPAthSymbol();
        }

        private void M1_2_1_PAthSymbolFalsoDereSup()
        {
            XYZ ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraIni, Math.PI + -_RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera);
            XYZ ptofin_PthSymb = _RebarInferiorDTO.barraFin;

            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = ptofin_PthSymb.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo
            //_RebarInferiorDTO.anguloTramoRad es negativo
            XYZ PE = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, -(-_RebarInferiorDTO.anguloBarraRad + _RebarInferiorDTO.anguloTramoRad + Math.PI), _RebarInferiorDTO.LargoPata).AsignarZ(zRefencia);

            ladoAB_pathSym = Line.CreateBound((ptofin_PthSymb - direccionBarra * _patabarra - dirBarraPerpen * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT).AsignarZ(zRefencia),
                                                (ptofin_PthSymb - dirBarraPerpen * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT).AsignarZ(zRefencia));
            ladoBC_pathSym = Line.CreateBound((ptofin_PthSymb - dirBarraPerpen * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT).AsignarZ(zRefencia), ptofin_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptofin_PthSymb, ptoini_PthSymb);
            ladoDE_pathSym = Line.CreateBound(ptoini_PthSymb, PE);

            XYZ PF = Util.ExtenderPuntoRespectoOtroPtosConAngulo(PE, -(-_RebarInferiorDTO.anguloBarraRad + _RebarInferiorDTO.anguloTramoRad + Math.PI) + Math.PI / 2, _espesorSinRecubEscalera).AsignarZ(zRefencia);
            ladoEF_pathSym = Line.CreateBound(PE, PF);

            XYZ PG = Util.ExtenderPuntoRespectoOtroPtosConAngulo(PF, -(-_RebarInferiorDTO.anguloBarraRad + _RebarInferiorDTO.anguloTramoRad + Math.PI) + Math.PI, _pataEscalera).AsignarZ(zRefencia);
            ladoFG_pathSym = Line.CreateBound(PF, PG);

            OBtenerListaFalsoPAthSymbol();
        }
        private void M1_2_3_PAthSymbolFalsoIzqInfer()
        {
            XYZ ptoini_PthSymb = _RebarInferiorDTO.barraIni;
            XYZ ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraFin, _RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera);

            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = ptoini_PthSymb.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            XYZ PE = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _RebarInferiorDTO.anguloBarraRad + _RebarInferiorDTO.anguloTramoRad, _RebarInferiorDTO.LargoPata).AsignarZ(zRefencia);

            ladoAB_pathSym = Line.CreateBound((ptoini_PthSymb + direccionBarra * _patabarra - dirBarraPerpen * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT).AsignarZ(zRefencia),
                                             (ptoini_PthSymb - dirBarraPerpen * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT).AsignarZ(zRefencia));
            ladoBC_pathSym = Line.CreateBound((ptoini_PthSymb - dirBarraPerpen * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT).AsignarZ(zRefencia), ptoini_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ladoDE_pathSym = Line.CreateBound(ptofin_PthSymb, PE);

            XYZ PF = Util.ExtenderPuntoRespectoOtroPtosConAngulo(PE, _RebarInferiorDTO.anguloBarraRad + _RebarInferiorDTO.anguloTramoRad - Math.PI / 2, _espesorSinRecubEscalera).AsignarZ(zRefencia);
            ladoEF_pathSym = Line.CreateBound(PE, PF);

            XYZ PG = Util.ExtenderPuntoRespectoOtroPtosConAngulo(PF, _RebarInferiorDTO.anguloBarraRad + Math.PI + _RebarInferiorDTO.anguloTramoRad, _pataEscalera).AsignarZ(zRefencia);
            ladoFG_pathSym = Line.CreateBound(PF, PG);

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
