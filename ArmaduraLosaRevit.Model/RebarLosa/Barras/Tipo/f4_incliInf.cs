using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Servicio;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Tipo

{
    // para caso f1 con para hacia arriba o bajo, cuando se encuentra con rampa
    public class f4_incliInf : ARebarLosa, IRebarLosa
    {

        private RebarInferiorDTO _RebarInferiorDTO;
        private BuscarLosaIncilnada _buscarLosaIncilnada;

        private XYZ direccionBarraSuperior_inicial;
        private XYZ EspesorRealIzqInf;
        private XYZ direccionBarraSuperior_final;
        private XYZ EspesorRealDereSup;

        private double _deltaZInicioFinconSigno;
        private double _deltaZ;

        private BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto_IzqInf;
        private BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto_DereSup;
        private XYZ _direccionBarraPAthSymbol2D;

        public double _largo { get; private set; }

        public f4_incliInf(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._uiapp = uiapp;
            //  this._uiapp = uiapp;
            //this._doc = _uiapp.ActiveUIDocument.Document;
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;

            ptoMouseAnivelVista = ptoMouseAnivelVista.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz1 = _rebarInferiorDTO.PtoDirectriz1.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz2 = _rebarInferiorDTO.PtoDirectriz2.AsignarZ(elevacionVIew);
            double deltaMOverAbajo = offInferiorHaciaArribaLosa + _rebarInferiorDTO.diametroFoot / 2;
            _VectorMover = new XYZ(0, 0, (deltaMOverAbajo));
            direccionBarra = (ptofin - ptoini).Normalize();
            TipoDireccionBarra_ = _rebarInferiorDTO.TipoDireccionBarra_;
            _Prefijo_F = "F=";

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
                    else if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaInicia(new XYZ(0, 0, -1), _rebarInferiorDTO.ptoSeleccionMouse))
                    {
                        ConstNH.corte();// todo este if
                        _VectorDIreccionLosaInicialExternaInclinada = _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada;
                    }
                    else
                        return false;
                    break;
                case UbicacionLosa.Izquierda:
                case UbicacionLosa.Inferior:
                    if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaFinal(new XYZ(0, 0, -1)))
                        _VectorDIreccionLosaFinalExternaInclinada = _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada;
                    else if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaFinal(new XYZ(0, 0, -1), _rebarInferiorDTO.ptoSeleccionMouse))
                    {
                        ConstNH.corte();// todo este if
                        _VectorDIreccionLosaFinalExternaInclinada = _buscarLosaIncilnada.VectorDireccionLosaExternaInclinada;
                    }
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
            try
            {
                switch (_RebarInferiorDTO.ubicacionLosa)
                {
                    case UbicacionLosa.Superior:
                    case UbicacionLosa.Derecha:
                        M1_1_1_Configuracion();
                        break;
                    case UbicacionLosa.Izquierda:
                    case UbicacionLosa.Inferior:
                        M1_1_1_Configuracion();
                        break;
                    case UbicacionLosa.NONE:
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear Rebar(Barra3d) : { ex.Message}");
                return false;
            }
            return true;// CopiandoParametrosLado_COnALargoRebar();

        }


        private bool M1_1_1_Configuracion()
        {
            try
            {


                _buscarFaceSuperiorConPto_IzqInf = new BuscarFaceSuperiorConPto(base._uiapp, _rebarInferiorDTO, direccionBarra);

                //a)
                if (_buscarFaceSuperiorConPto_IzqInf.ObtenerDirecionDeLosa(ptoini + direccionBarra * _patabarra / 2 + LargoRecorrido * 0.5 * dirBarraPerpen, PosicionDeBusqueda.Inicio))
                {
                    direccionBarraSuperior_inicial = _buscarFaceSuperiorConPto_IzqInf.DireccionBarraSuperior;
                    EspesorRealIzqInf = _buscarFaceSuperiorConPto_IzqInf.EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam;
                }
                else if (_buscarFaceSuperiorConPto_IzqInf.ObtenerDirecionDeLosa(_rebarInferiorDTO.ptoSeleccionMouse, PosicionDeBusqueda.Inicio))
                {
                    direccionBarraSuperior_inicial = _buscarFaceSuperiorConPto_IzqInf.DireccionBarraSuperior;
                    EspesorRealIzqInf = _buscarFaceSuperiorConPto_IzqInf.EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam;
                }
                else
                {
                    direccionBarraSuperior_inicial = direccionBarra;
                    EspesorRealIzqInf = new XYZ(0, 0, _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT);
                }
                ladoAB = Line.CreateBound(ptoini + EspesorRealIzqInf + direccionBarraSuperior_inicial * _patabarra, ptoini + EspesorRealIzqInf);
                ladoBC = Line.CreateBound(ptoini + EspesorRealIzqInf, ptoini);
                //b
                ladoCD = Line.CreateBound(ptoini, ptofin);

                //c
                _buscarFaceSuperiorConPto_DereSup = new BuscarFaceSuperiorConPto(base._uiapp, _rebarInferiorDTO, direccionBarra);

                if (_buscarFaceSuperiorConPto_DereSup.ObtenerDirecionDeLosa(ptofin + -direccionBarra * _patabarra / 2 + LargoRecorrido * 0.5 * dirBarraPerpen, PosicionDeBusqueda.Fin))
                {
                    direccionBarraSuperior_final = _buscarFaceSuperiorConPto_DereSup.DireccionBarraSuperior;
                    EspesorRealDereSup = _buscarFaceSuperiorConPto_DereSup.EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam;
                }
                else if (_buscarFaceSuperiorConPto_DereSup.ObtenerDirecionDeLosa(_rebarInferiorDTO.ptoSeleccionMouse, PosicionDeBusqueda.Fin))
                {
                    direccionBarraSuperior_final = _buscarFaceSuperiorConPto_DereSup.DireccionBarraSuperior;
                    EspesorRealDereSup = _buscarFaceSuperiorConPto_DereSup.EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam;
                }
                else
                {
                    direccionBarraSuperior_final = -direccionBarra;
                    EspesorRealDereSup = new XYZ(0, 0, _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT);
                }

                ladoDE = Line.CreateBound(ptofin, ptofin + EspesorRealDereSup);
                ladoEG = Line.CreateBound(ptofin + EspesorRealDereSup, ptofin + EspesorRealDereSup + direccionBarraSuperior_final * _patabarra);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

            // norm = dirBarraPerpen.Normalize();
        }

        #endregion


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {
            try
            {
                _deltaZ = CalcularDiferenciaMAxYMinZ();

                switch (_RebarInferiorDTO.ubicacionLosa)
                {
                    case UbicacionLosa.Superior:
                    case UbicacionLosa.Derecha:
                        M1_2_1_PAthSymbolFalso();

                        break;
                    case UbicacionLosa.Izquierda:
                    case UbicacionLosa.Inferior:
                        M1_2_1_PAthSymbolFalso();

                        break;
                    case UbicacionLosa.NONE:
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear Rebar(Barra3d) : { ex.Message}");
                return false;
            }
            return true;// CopiandoParametrosLado_COnPAthSymbol();
        }

        private double CalcularDiferenciaMAxYMinZ()
        {
            _deltaZInicioFinconSigno = ptofin.Z - ptoini.Z;
            double AnguloEnZRad = Math.Abs((ptoini - ptofin).GetAngleEnZ_respectoPlanoXY());
            _largo = ptofin.DistanceTo(ptoini);

            return Math.Sin(AnguloEnZRad) * _largo;
        }


        private void M1_2_1_PAthSymbolFalso()
        {
            double espesorIzqInf = Util.CmToFoot(18);
            double espesorDereSup = Util.CmToFoot(18);
            //Espesore
            if (!Util.IsSimilarValor(_buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorSinRecub_foot, 0, 0.001))
            {
                espesorIzqInf = _buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorSinRecub_foot;
            }

            if (!Util.IsSimilarValor(_buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorSinRecub_foot, 0, 0.001))
            {
                espesorDereSup = _buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorSinRecub_foot;
            }


            // pto inicio  y fin 
            XYZ ptoini_PthSymb = _rebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? -1 : 1) * Math.PI / 2, _deltaZ / 2);
            XYZ ptofin_PthSymb = _rebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);
            ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? 1 : -1) * Math.PI / 2, _deltaZ / 2);

            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = ptoini_PthSymb.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            _direccionBarraPAthSymbol2D = (ptofin_PthSymb - ptoini_PthSymb).Normalize();
            //direccion del espesor
            

            XYZ Pa_aux = (ptoini_PthSymb + dirBarraPerpen * espesorIzqInf + M1_2_2_GetDireccionBarraSuperior2d_INICIO(_buscarFaceSuperiorConPto_IzqInf) * _patabarra).AsignarZ(zRefencia);
            XYZ Pb_aux = (ptoini_PthSymb + dirBarraPerpen * espesorIzqInf).AsignarZ(zRefencia);

            XYZ Pe_aux = (ptofin_PthSymb + dirBarraPerpen * espesorDereSup).AsignarZ(zRefencia);
            XYZ Pf_aux = (ptofin_PthSymb + dirBarraPerpen * espesorDereSup + M1_2_2_GetDireccionBarraSuperior2d_FIN(_buscarFaceSuperiorConPto_DereSup) * _patabarra).AsignarZ(zRefencia);


            ladoAB_pathSym = Line.CreateBound(Pa_aux, Pb_aux);

            ladoBC_pathSym = Line.CreateBound(Pb_aux, ptoini_PthSymb);

            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);

            ladoDE_pathSym = Line.CreateBound(ptofin_PthSymb, Pe_aux);
            ladoEF_pathSym = Line.CreateBound(Pe_aux, Pf_aux);

            OBtenerListaFalsoPAthSymbol();
        }

        public XYZ M1_2_2_GetDireccionBarraSuperior2d_INICIO(BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto)
        {
            double anguloz = _buscarFaceSuperiorConPto.DireccionBarraSuperior.GetAngleEnZ_respectoPlanoXY();
            double anguloORientacion = 0;// (_RebarInferiorDTO.ubicacionLosa == UbicacionLosa.Inferior || _RebarInferiorDTO.ubicacionLosa == UbicacionLosa.Superior ? Math.PI / 2 : 0);
            double anguloPathXY = _rebarInferiorDTO.anguloBarraRad;// Util.GetAnguloVectoresEnGrados_enPlanoXY(_direccionBarra);
            double angleTotal = anguloz + anguloORientacion + anguloPathXY;

            XYZ result = new XYZ(Math.Cos(angleTotal), Math.Sin(angleTotal), 0);
            return result;
        }

        public XYZ M1_2_2_GetDireccionBarraSuperior2d_FIN(BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto)
        {
            double anguloz = -_buscarFaceSuperiorConPto.DireccionBarraSuperior.GetAngleEnZ_respectoPlanoXY();
            double anguloORientacion = 0;// (_RebarInferiorDTO.ubicacionLosa == UbicacionLosa.Inferior || _RebarInferiorDTO.ubicacionLosa == UbicacionLosa.Superior ? Math.PI / 2 : 0);
            double anguloPathXY = _rebarInferiorDTO.anguloBarraRad;// Util.GetAnguloVectoresEnGrados_enPlanoXY(_direccionBarra);
            double angleTotal = anguloz + anguloORientacion + (anguloPathXY) + Math.PI;

            XYZ result = new XYZ(Math.Cos(angleTotal), Math.Sin(angleTotal), 0);
            return result;
        }

        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            try
            {
                ObtenerNuevoptoCirculo();

                ObtenerPAthSymbolTAG();
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al crear Rebar(Barra3d) : { ex.Message}");
                return false;
            }
            return true;
        }
        public override void ObtenerPAthSymbolTAG()
        {
            _newGeometriaTag.Ejecutar(new GeomeTagArgs()
            {
                angulorad = base._rebarInferiorDTO.anguloBarraRad,
                diferenciaZInicialFinal = _deltaZInicioFinconSigno,
                _buscarFaceSuperiorConPto_IzqInf = this._buscarFaceSuperiorConPto_IzqInf,
                _buscarFaceSuperiorConPto_DereSup = this._buscarFaceSuperiorConPto_DereSup,
                deltaZ = _deltaZInicioFinconSigno,
                largoREcorridoDeltaZ = _largo

            });

        }

        //public void M2A_GenerarBarra()
        //{
        //    M1_ConfigurarDatosIniciales();
        //    M3_DibujarBarraCurve();//en ibarra
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

        //    M12_MOverHaciaArriba();

        //}


    }
}
