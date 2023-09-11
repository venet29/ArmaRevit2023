using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Servicio;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Tipo

{
    public class f4_incli_esc : ARebarLosa, IRebarLosa
    {

        //private RebarInferiorDTO _rebarInferiorDTO;
        private double _extHorDentroEscalera;
        //  private double _largoPataInclinada;
        private double _deltaZ;   //no considera signo
        private double _deltaZInicioFinconSigno; //considera el signo



        VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO;
        VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO;
        private XYZ _direccionBarra;

        public f4_incli_esc(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            ConstNH.sbLog.AppendLine("----------------------------------------------------------------------------------------------------------------:");

            //  this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _extHorDentroEscalera = _rebarInferiorDTO.LargoPata;//UtilBarras.largo_traslapoFoot_diamMM(8);
            _largoPataInclinada = Util.CmToFoot(30);  //_rebarInferiorDTO.LargoPata;
            _patabarra = _rebarInferiorDTO.LargoPataF4;
            ptoMouseAnivelVista = ptoMouseAnivelVista.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz1 = _rebarInferiorDTO.PtoDirectriz1.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz2 = _rebarInferiorDTO.PtoDirectriz2.AsignarZ(elevacionVIew);

            if (_rebarInferiorDTO.ubicacionLosa == UbicacionLosa.Izquierda || _rebarInferiorDTO.ubicacionLosa == UbicacionLosa.Derecha)
            { _direccionBarra = (_rebarInferiorDTO.listaPtosPerimetroBarras[2].GetXY0() - _rebarInferiorDTO.listaPtosPerimetroBarras[1].GetXY0()).Normalize(); }
            else
            { _direccionBarra = (_rebarInferiorDTO.listaPtosPerimetroBarras[0].GetXY0() - _rebarInferiorDTO.listaPtosPerimetroBarras[1].GetXY0()).Normalize(); }


            // double deltaMOverAbajo = this._rebarInferiorDTO.espesorBarraEnLOsaFooT + offSuperiorhaciaBajoLosa + Util.CmToFoot(0.5);
            // ConstantesGenerales.sbLog.AppendLine($"deltaMOverAbajo : {Util.FootToCm(deltaMOverAbajo)}");


            _VectorMover = new XYZ(0, 0, ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT);

        }

        public bool M1A_IsTodoOK()
        {
            MoviendoP1P2HaciaCentro();
            M1_0_ObtenerPendienteLosaContigua();
            if (!M1_1_DatosBarra3d()) return false;
           // if (!M1_2_DatosBarra2d()) return false;
            // if (!M1_3_Recorrido()) return false;
            return true;
        }

        private void MoviendoP1P2HaciaCentro()
        {
            ptoini = ptoini + _direccionBarra * Util.CmToFoot(5);
            ptofin = ptofin + -_direccionBarra * Util.CmToFoot(5);
        }

        #region COmprobacion 1


        private void M1_0_ObtenerPendienteLosaContigua()
        {
            //ptoini
            //_buscarLosaIncilnada = new BuscarLosaIncilnada(_doc, base._rebarInferiorDTO.listaPtosPerimetroBarras, 10);
            //if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaInicia(new XYZ(0, 0, -1)))
            //{           
            //    _vectorDireccionLosaInicialExternaInclinadaDTO = _buscarLosaIncilnada.
            //                    ObtenerVectorDireccionLosaExternaInclinadaDTO(base._rebarInferiorDTO.floor);
            //}
            //else
            //    _vectorDireccionLosaInicialExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };
            _vectorDireccionLosaInicialExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO()
            {
                IsLosaEncontrada = true,
                direccionLosa = -_direccionBarra
            };

            //lado finv
            //_buscarLosaIncilnada = new BuscarLosaIncilnada(_doc, base._rebarInferiorDTO.listaPtosPerimetroBarras, 10);
            //if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaFinal(new XYZ(0, 0, -1)))
            //    _vectorDireccionLosaFinalExternaInclinadaDTO = _buscarLosaIncilnada.
            //                    ObtenerVectorDireccionLosaExternaInclinadaDTO(base._rebarInferiorDTO.floor);
            //else
            //    _vectorDireccionLosaFinalExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };
            _vectorDireccionLosaFinalExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO()
            {
                IsLosaEncontrada = true,
                direccionLosa = _direccionBarra
            };
        }

        public bool M1_1_DatosBarra3d()
        {
            switch (_rebarInferiorDTO.ubicacionLosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:
                    M1_1_1_ConfiguracionDereSup();
                    break;
                case UbicacionLosa.Izquierda:
                case UbicacionLosa.Inferior:
                    M1_1_1_ConfiguracionDereSup();
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

            XYZ desplaZ = (_rebarInferiorDTO.ServicioModificarCoordenadasEscalera._planarFaceMaxArea.FaceNormal * -1 * _rebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT).AsignarZ(zRefencia);

            ladoAB = Line.CreateBound((ptoini + direccionBarra * _patabarra + desplaZ).AsignarZ(zRefencia), (ptoini + desplaZ).AsignarZ(zRefencia));
            ladoBC = Line.CreateBound(ptoini + desplaZ, ptoini);
            ladoCD = Line.CreateBound(ptoini, ptofin);
            ladoDE = Line.CreateBound(ptofin, ptofin + desplaZ);
            ladoEG = Line.CreateBound(ptofin + desplaZ, (ptofin + -direccionBarra * _patabarra + desplaZ).AsignarZ(zRefencia));

        }

        #endregion


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {

            _deltaZ = CalcularDiferenciaMAxYMinZ();

            switch (_rebarInferiorDTO.ubicacionLosa)
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

        public override void ObtenerPAthSymbolTAG()
        {
            _newGeometriaTag.Ejecutar(new GeomeTagArgs()
            {
                angulorad = base._rebarInferiorDTO.anguloBarraRad,
                diferenciaZInicialFinal = _deltaZInicioFinconSigno,
                _vectorDireccionLosaInicialExternaInclinadaDTO = _vectorDireccionLosaInicialExternaInclinadaDTO,
                _vectorDireccionLosaFinalExternaInclinadaDTO = _vectorDireccionLosaFinalExternaInclinadaDTO
            });

        }

        private double CalcularDiferenciaMAxYMinZ()
        {
            _deltaZInicioFinconSigno = ptofin.Z - ptoini.Z;
            double AnguloEnZRad = Math.Abs((ptoini - ptofin).GetAngleEnZ_respectoPlanoXY());
            double largo = ptofin.DistanceTo(ptoini);

            return Math.Sin(AnguloEnZRad) * largo;
        }

        private void M1_2_1_PAthSymbolFalsoDereSup()
        {
            XYZ ptoini_PthSymb = _rebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? -1 : 1) * Math.PI / 2, _deltaZ / 2);
            XYZ ptofin_PthSymb = _rebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);
            ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? 1 : -1) * Math.PI / 2, _deltaZ / 2);

            if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
            {
                Line ladoAB = Line.CreateBound(ptoini_PthSymb + _vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa * _extHorDentroEscalera, ptoini_PthSymb);
                ListaFalsoPAthSymbol.Add(ladoAB);
            }

            Line ladoBC = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ListaFalsoPAthSymbol.Add(ladoBC);

            if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                Line ladoCD = Line.CreateBound(ptofin_PthSymb, ptofin_PthSymb + _vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa * _extHorDentroEscalera);
                ListaFalsoPAthSymbol.Add(ladoCD);
            }





        }


        private void M1_2_3_PAthSymbolFalsoIzqInfer()
        {
            ConstNH.sbLog.AppendLine($"f3_incli xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            ConstNH.sbLog.AppendLine($"  Antes");
            ConstNH.sbLog.AppendLine($" p1={_rebarInferiorDTO.barraIni}       p2 ={_rebarInferiorDTO.barraFin}     angulo={ _rebarInferiorDTO.anguloBarraRad}");

            XYZ ptoini_PthSymb = _rebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? -1 : 1) * Math.PI / 2, _deltaZ / 2);

            XYZ ptofin_PthSymb = _rebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);
            ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? 1 : -1) * Math.PI / 2, _deltaZ / 2);

            ConstNH.sbLog.AppendLine($" datos movidos");
            ConstNH.sbLog.AppendLine($" p1={ptoini_PthSymb}       p2 ={ptofin_PthSymb}");

            if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
            {
                XYZ PA = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, Math.PI + _rebarInferiorDTO.anguloBarraRad + -_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada);
                Line ladoAB = Line.CreateBound(PA, ptoini_PthSymb);
                //Line ladoAB = Line.CreateBound(ptoini_PthSymb + _vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa * _extHorDentroEscalera, ptoini_PthSymb);
                ListaFalsoPAthSymbol.Add(ladoAB);
            }

            Line ladoBC = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ListaFalsoPAthSymbol.Add(ladoBC);

            if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                XYZ PD = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + _vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada);
                Line ladoCD = Line.CreateBound(ptofin_PthSymb, PD);
                //Line ladoCD = Line.CreateBound(ptofin_PthSymb, ptofin_PthSymb + direccionBarra * _extHorDentroEscalera);
                ListaFalsoPAthSymbol.Add(ladoCD);
            }
        }

        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerNuevoptoCirculo();

            ObtenerPAthSymbolTAG();

            return true;
        }


        public override void M2A_GenerarBarra()
        {
            //metodo sin verificar, revisar si corresponde override o usar el metodo de la clase padre 'ARebarLosa.cs'
            Util.ErrorMsg("metodo sin verificar, revisar si corresponde override o usar el metodo de la clase padre 'ARebarLosa.cs'");
            M1_ConfigurarDatosIniciales();
            if (M3_DibujarBarraCurve() != Result.Succeeded) return;

            M3A_1_CopiarParametrosCOmpartidos();

            M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing();
            M6_visualizar();

            M12_MOverSegunVector();
        }


    }
}
