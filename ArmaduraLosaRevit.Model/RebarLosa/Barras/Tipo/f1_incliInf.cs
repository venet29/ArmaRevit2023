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
    public class f1_incliInf : ARebarLosa, IRebarLosa
    {
        //   private RebarInferiorDTO _RebarInferiorDTO;
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

        VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO;
        VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO;

        public double _largo { get; private set; }

        public f1_incliInf(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            //  this._uiapp = uiapp;
            //this._doc = _uiapp.ActiveUIDocument.Document;
            //  this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;

            ptoMouseAnivelVista = ptoMouseAnivelVista.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz1 = _rebarInferiorDTO.PtoDirectriz1.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz2 = _rebarInferiorDTO.PtoDirectriz2.AsignarZ(elevacionVIew);

            direccionBarra = (ptofin - ptoini).Normalize();

            double deltaMOverAbajo = offInferiorHaciaArribaLosa + _rebarInferiorDTO.diametroFoot / 2;
            _VectorMover = new XYZ(0, 0, (deltaMOverAbajo));

            TipoDireccionBarra_ = _rebarInferiorDTO.TipoDireccionBarra_;
            _Prefijo_F = "F=";
            this._uiapp = uiapp;
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

            _buscarLosaIncilnada = new BuscarLosaIncilnada(_uiapp, _rebarInferiorDTO.listaPtosPerimetroBarras, (Floor)_rebarInferiorDTO.floor, 10);

            dire_Pfin_Pini_XY0 = (ptofin - ptoini).Normalize().GetXY0();

            switch (_rebarInferiorDTO.ubicacionLosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:
                    if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaInicia(new XYZ(0, 0, -1)))
                    {
                        _vectorDireccionLosaInicialExternaInclinadaDTO = _buscarLosaIncilnada.ObtenerVectorDireccionLosaExternaInclinadaDTO(base._rebarInferiorDTO.floor);
                        if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
                            direPAtaINi = (-dire_Pfin_Pini_XY0).AsignarZ(_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.Z);
                    }
                    else
                        _vectorDireccionLosaInicialExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };
                    break;
                case UbicacionLosa.Izquierda:
                case UbicacionLosa.Inferior:
                    if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaFinal(new XYZ(0, 0, -1)))
                    {
                        _vectorDireccionLosaFinalExternaInclinadaDTO = _buscarLosaIncilnada.ObtenerVectorDireccionLosaExternaInclinadaDTO(base._rebarInferiorDTO.floor);

                        if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
                            direPAtaFin = dire_Pfin_Pini_XY0.AsignarZ(_vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa.Z);
                    }
                    else
                        _vectorDireccionLosaFinalExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };
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
                switch (_rebarInferiorDTO.ubicacionLosa)
                {
                    case UbicacionLosa.Superior:
                    case UbicacionLosa.Derecha:
                        M1_1_1_Configuracion_SupDer();
                        break;
                    case UbicacionLosa.Izquierda:
                    case UbicacionLosa.Inferior:
                        M1_1_1_Configuracion_InfIzq();
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


        private void M1_1_1_Configuracion_SupDer()
        {
            //a)
            if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
            {
                ladoDE = Line.CreateBound(ptoini, ptoini + _vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa * _largoPataInclinada);
            }

            //b
            ladoCD = Line.CreateBound(ptofin, ptoini);

            //c
            _buscarFaceSuperiorConPto_DereSup = new BuscarFaceSuperiorConPto(base._uiapp, _rebarInferiorDTO, direccionBarra);

            if (_buscarFaceSuperiorConPto_DereSup.ObtenerDirecionDeLosa(ptofin + -direccionBarra * _patabarra / 2 + LargoRecorrido * 0.5 * dirBarraPerpen, PosicionDeBusqueda.Fin))
            {
                direccionBarraSuperior_final = _buscarFaceSuperiorConPto_DereSup.DireccionBarraSuperior;
                EspesorRealDereSup = _buscarFaceSuperiorConPto_DereSup.EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam;
            }
            else if (_buscarFaceSuperiorConPto_DereSup.ObtenerDirecionDeLosa(_rebarInferiorDTO.ptoSeleccionMouse, PosicionDeBusqueda.Fin))
            {
                ConstNH.corte();// todo este if
                direccionBarraSuperior_final = _buscarFaceSuperiorConPto_DereSup.DireccionBarraSuperior;
                EspesorRealDereSup = _buscarFaceSuperiorConPto_DereSup.EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam;
            }
            else
            {
                direccionBarraSuperior_final = -direccionBarra;
                EspesorRealDereSup = new XYZ(0, 0, _rebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT);
            }

            ladoBC = Line.CreateBound(ptofin + EspesorRealDereSup, ptofin);
            ladoAB = Line.CreateBound(ptofin + EspesorRealDereSup + direccionBarraSuperior_final * _patabarra, ptofin + EspesorRealDereSup);

        }



        private void M1_1_1_Configuracion_InfIzq()
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
                ConstNH.corte();// todo este if
                direccionBarraSuperior_inicial = _buscarFaceSuperiorConPto_IzqInf.DireccionBarraSuperior;
                EspesorRealIzqInf = _buscarFaceSuperiorConPto_IzqInf.EspesorRealEnPtoFaceSuperior_SinRecub_SinDIam;
            }
            else
            {
                direccionBarraSuperior_inicial = direccionBarra;
                EspesorRealIzqInf = new XYZ(0, 0, _rebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT);
            }
            ladoAB = Line.CreateBound(ptoini + EspesorRealIzqInf + direccionBarraSuperior_inicial * _patabarra, ptoini + EspesorRealIzqInf);
            ladoBC = Line.CreateBound(ptoini + EspesorRealIzqInf, ptoini);

            //b
            ladoCD = Line.CreateBound(ptoini, ptofin);

            //c
            if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                ladoDE = Line.CreateBound(ptofin, ptofin + _vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa * _largoPataInclinada);
            }

        }
        #endregion


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {
            try
            {
                _deltaZ = CalcularDiferenciaMAxYMinZ();

                switch (_rebarInferiorDTO.ubicacionLosa)
                {
                    case UbicacionLosa.Superior:
                    case UbicacionLosa.Derecha:
                        M1_2_1_PAthSymbolFalso_DerSup();

                        break;
                    case UbicacionLosa.Izquierda:
                    case UbicacionLosa.Inferior:
                        M1_2_1_PAthSymbolFalso_IzqInf();

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
            return true;
        }

        private double CalcularDiferenciaMAxYMinZ()
        {
            _deltaZInicioFinconSigno = ptofin.Z - ptoini.Z;
            double AnguloEnZRad = Math.Abs((ptoini - ptofin).GetAngleEnZ_respectoPlanoXY());
            _largo = ptofin.DistanceTo(ptoini);

            return Math.Sin(AnguloEnZRad) * _largo;
        }


        private void M1_2_1_PAthSymbolFalso_IzqInf()
        {
            double espesorIzqInf = Util.CmToFoot(18);
            // double espesorDereSup = Util.CmToFoot(18);
            if (_buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorConRecub_foot > 100)
            {
                Util.ErrorMsg($"Espesor de losa no encontrado en tramo {_rebarInferiorDTO.ubicacionLosa.ToString()} de barra. Se utiliza espesor 18cm");
                _buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorConRecub_foot = espesorIzqInf;
            }
            //Espesore
            if (_buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorConRecub_foot != 0)
            {
                espesorIzqInf = _buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorConRecub_foot - 2 * ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT;
                //  espesorDereSup = _buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub - 2 * ConstantesGenerales.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT;
            }

            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = elevacionVIew; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo


            // pto inicio  y fin 
            XYZ ptoini_PthSymb = _rebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? -1 : 1) * Math.PI / 2, _deltaZ / 2);
            XYZ ptofin_PthSymb = _rebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);
            ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? 1 : -1) * Math.PI / 2, _deltaZ / 2);


            _direccionBarraPAthSymbol2D = (ptofin_PthSymb - ptoini_PthSymb).Normalize();
            //direccion del espesor
            //dirBarraPerpen = -Util.GetVectorPerpendicular2(_direccionBarraPAthSymbol2D);

            ladoAB_pathSym = Line.CreateBound((ptoini_PthSymb + dirBarraPerpen * espesorIzqInf + M1_2_2_GetDireccionBarraSuperior2d_INICIO(_buscarFaceSuperiorConPto_IzqInf) * _patabarra).AsignarZ(zRefencia)
                                            , (ptoini_PthSymb + dirBarraPerpen * espesorIzqInf).AsignarZ(zRefencia));
            ladoBC_pathSym = Line.CreateBound((ptoini_PthSymb + dirBarraPerpen * espesorIzqInf).AsignarZ(zRefencia), ptoini_PthSymb);

            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);

            if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                XYZ PD = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + direPAtaFin.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada).AsignarZ(zRefencia);
                ladoDE_pathSym = Line.CreateBound(ptofin_PthSymb, PD);
            }

            OBtenerListaFalsoPAthSymbol();
        }

        private void M1_2_1_PAthSymbolFalso_DerSup()
        {
            //   double espesorIzqInf = Util.CmToFoot(18);
            double espesorDereSup_foot = Util.CmToFoot(18);
            if (_buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub_foot > 100)
            {
                Util.ErrorMsg($"Espesor de losa no encontrado en tramo {_rebarInferiorDTO.ubicacionLosa} de barra. Se utiliza espesor 18cm");
                _buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub_foot = espesorDereSup_foot;
            }
            //Espesore
            if (_buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub_foot != 0)
            {
                //   espesorIzqInf = _buscarFaceSuperiorConPto_IzqInf.EspesorEnPtoFaceSuperiorConRecub - 2 * ConstantesGenerales.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT;
                espesorDereSup_foot = _buscarFaceSuperiorConPto_DereSup.EspesorEnPtoFaceSuperiorConRecub_foot - 2 * ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT;
            }

            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = elevacionVIew; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            // pto inicio  y fin 
            XYZ ptoini_PthSymb = _rebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? -1 : 1) * Math.PI / 2, _deltaZ / 2);
            XYZ ptofin_PthSymb = _rebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);
            ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? 1 : -1) * Math.PI / 2, _deltaZ / 2);

            _direccionBarraPAthSymbol2D = (ptofin_PthSymb - ptoini_PthSymb).Normalize();
            //direccion del espesor
            //dirBarraPerpen = -Util.GetVectorPerpendicular2(_direccionBarraPAthSymbol2D);


            if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
            {
                //XYZ PB = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, Math.PI + _rebarInferiorDTO.anguloBarraRad + -_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada);
                XYZ PB = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, Math.PI + _rebarInferiorDTO.anguloBarraRad + -direPAtaINi.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada).AsignarZ(zRefencia);
                ladoBC_pathSym = Line.CreateBound(PB, ptoini_PthSymb);
            }

            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ladoDE_pathSym = Line.CreateBound((ptofin_PthSymb + dirBarraPerpen * espesorDereSup_foot).AsignarZ(zRefencia), ptofin_PthSymb);
            ladoEF_pathSym = Line.CreateBound((ptofin_PthSymb + dirBarraPerpen * espesorDereSup_foot).AsignarZ(zRefencia),
                                              (ptofin_PthSymb + dirBarraPerpen * espesorDereSup_foot + M1_2_2_GetDireccionBarraSuperior2d_FIN(_buscarFaceSuperiorConPto_DereSup) * _patabarra).AsignarZ(zRefencia));

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
                _vectorDireccionLosaInicialExternaInclinadaDTO = _vectorDireccionLosaInicialExternaInclinadaDTO,
                _vectorDireccionLosaFinalExternaInclinadaDTO = _vectorDireccionLosaFinalExternaInclinadaDTO,
                _buscarFaceSuperiorConPto_IzqInf = this._buscarFaceSuperiorConPto_IzqInf,
                _buscarFaceSuperiorConPto_DereSup = this._buscarFaceSuperiorConPto_DereSup,
                deltaZ = _deltaZInicioFinconSigno,
                largoREcorridoDeltaZ = _largo

            });

        }




    }
}
