using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Servicio;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Tipo

{
    public class f3_incli : ARebarLosa, IRebarLosa
    {

        //private RebarInferiorDTO _rebarInferiorDTO;
        //   private double _extHorDentroEscalera;
        //  private double _largoPataInclinada;
        private double _deltaZ;   //no considera signo
        private double _deltaZInicioFinconSigno; //considera el signo
        private BuscarLosaIncilnada _buscarLosaIncilnada;


        VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO;
        VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO;
        private DireccionPata _ubicacionPata;
        private int _numeroTramosbarra;


        public f3_incli(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            ConstNH.sbLog.AppendLine("----------------------------------------------------------------------------------------------------------------:");

            //  this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            //  _extHorDentroEscalera = _largoPataInclinada;// Util.CmToFoot(30);//UtilBarras.largo_traslapoFoot_diamMM(8);


            ptoMouseAnivelVista = ptoMouseAnivelVista.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz1 = _rebarInferiorDTO.PtoDirectriz1.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz2 = _rebarInferiorDTO.PtoDirectriz2.AsignarZ(elevacionVIew);

            //  double deltaMOverAbajo = this._rebarInferiorDTO.espesorBarraEnLOsaFooT + offSuperiorhaciaBajoLosa + Util.CmToFoot(0.5);
            double deltaMOverAbajo = offInferiorHaciaArribaLosa + _rebarInferiorDTO.diametroFoot / 2;
            //deltaMOverAbajo = 0;
            _VectorMover = new XYZ(0, 0, (deltaMOverAbajo));

            TipoDireccionBarra_ = _rebarInferiorDTO.TipoDireccionBarra_;
            _Prefijo_F = "F=";
            this._uiapp = uiapp;
        }

        public bool M1A_IsTodoOK()
        {
            //  MoviendoP1P2HaciaCentro();
            if (!M1_0_ObtenerPendienteLosaContigua()) return false;
            if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;
            return true;
        }


        #region COmprobacion 1

        private bool M1_0_ObtenerPendienteLosaContigua()
        {
            try
            {
                //  dire_Pfin_Pini_XY0 = (ptofin.GetXY0() - ptoini.GetXY0()).Normalize();
                dire_Pfin_Pini_XY0 = (ptofin - ptoini).Normalize().GetXY0();

                //ptoini
                _buscarLosaIncilnada = new BuscarLosaIncilnada(_uiapp, base._rebarInferiorDTO.listaPtosPerimetroBarras, (Floor)base._rebarInferiorDTO.floor, 10);
                if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaInicia(new XYZ(0, 0, -1)))
                {
                    _vectorDireccionLosaInicialExternaInclinadaDTO = _buscarLosaIncilnada.ObtenerVectorDireccionLosaExternaInclinadaDTO(base._rebarInferiorDTO.floor);

                    if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
                        direPAtaINi = (-dire_Pfin_Pini_XY0).AsignarZ(_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.Z);
                }
                else
                    _vectorDireccionLosaInicialExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };

                //lado fin
                _buscarLosaIncilnada = new BuscarLosaIncilnada(_uiapp, base._rebarInferiorDTO.listaPtosPerimetroBarras, (Floor)base._rebarInferiorDTO.floor, 10);
                if (_buscarLosaIncilnada.obtenerPendienteLosaContiguaFinal(new XYZ(0, 0, -1)))
                {
                    _vectorDireccionLosaFinalExternaInclinadaDTO = _buscarLosaIncilnada.ObtenerVectorDireccionLosaExternaInclinadaDTO(base._rebarInferiorDTO.floor);

                    if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
                        direPAtaFin = dire_Pfin_Pini_XY0.AsignarZ(_vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa.Z);
                }
                else
                    _vectorDireccionLosaFinalExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en obtener pendiente de losa contiugua ex:{ex.Message}");
                return false;
            }

            return true;
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
            //  XYZ dire_Pfin_Pini_XY0 = (ptofin - ptoini).Normalize().GetXY0();
            // XYZ direPAtaINi = XYZ.Zero;
            // XYZ direPAtaFin = XYZ.Zero;
            if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada && _vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                //   direPAtaINi = dire_Pfin_Pini_XY0.AsignarZ(_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.Z);

                _ubicacionPata = DireccionPata.Ambos;
                _numeroTramosbarra = 3;
                ladoAB = Line.CreateBound(ptoini + direPAtaINi * _largoPataInclinada, ptoini);//_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa
                ladoBC = Line.CreateBound(ptoini, ptofin);
                ladoCD = Line.CreateBound(ptofin, ptofin + direPAtaFin * _largoPataInclinada);//_vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa 
            }
            else if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
            {
                //direPAtaINi = dire_Pfin_Pini_XY0.AsignarZ(_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.Z);
                //    direPAtaFin = dire_Pfin_Pini_XY0.AsignarZ(_vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa.Z);
                _ubicacionPata = DireccionPata.DereSup;
                _numeroTramosbarra = 2;
                ladoAB = Line.CreateBound(ptoini, ptofin);
                ladoBC = Line.CreateBound(ptofin, ptofin + direPAtaFin * _largoPataInclinada);//_vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa
            }
            else if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                //   direPAtaINi = dire_Pfin_Pini_XY0.AsignarZ(_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.Z);
                // direPAtaFin = dire_Pfin_Pini_XY0.AsignarZ(_vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa.Z);
                _ubicacionPata = DireccionPata.IzqInf;
                _numeroTramosbarra = 2;
                ladoAB = Line.CreateBound(ptoini + direPAtaINi * _largoPataInclinada, ptoini);//_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa
                ladoBC = Line.CreateBound(ptoini, ptofin);

            }
            else
            {
                _numeroTramosbarra = 1;
                _ubicacionPata = DireccionPata.SoloCentral;
                ladoAB = Line.CreateBound(ptoini, ptofin.AsignarZ(ptoini.Z));
            }

        }

        private void M1_1_1_ConfiguracionIzqInfer()
        {
            if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada && _vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {

                _ubicacionPata = DireccionPata.Ambos;
                _numeroTramosbarra = 3;
                ladoAB = Line.CreateBound(ptoini + direPAtaINi * _largoPataInclinada, ptoini);
                ladoBC = Line.CreateBound(ptoini, ptofin);
                ladoCD = Line.CreateBound(ptofin, ptofin + direPAtaFin * _largoPataInclinada);
            }
            else if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                _ubicacionPata = DireccionPata.DereSup;
                _numeroTramosbarra = 2;
                ladoAB = Line.CreateBound(ptofin + direPAtaFin * _largoPataInclinada, ptofin);
                ladoBC = Line.CreateBound(ptofin, ptoini);


            }
            else if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
            {
                _ubicacionPata = DireccionPata.IzqInf;
                _numeroTramosbarra = 2;
                ladoAB = Line.CreateBound(ptoini + direPAtaINi * _largoPataInclinada, ptoini);
                ladoBC = Line.CreateBound(ptoini, ptofin);

            }
            else
            {
                _numeroTramosbarra = 1;
                _ubicacionPata = DireccionPata.SoloCentral;
                ladoAB = Line.CreateBound(ptoini, ptofin);
            }

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
                ladoAB_pathSym = Line.CreateBound((ptoini_PthSymb + _vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa * _largoPataInclinada).AsignarZ(elevacionVIew), ptoini_PthSymb);
                ListaFalsoPAthSymbol.Add(ladoAB_pathSym);
            }

            ladoBC_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ListaFalsoPAthSymbol.Add(ladoBC_pathSym);

            if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                ladoCD_pathSym = Line.CreateBound(ptofin_PthSymb, (ptofin_PthSymb + _vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa * _largoPataInclinada).AsignarZ(elevacionVIew));
                ListaFalsoPAthSymbol.Add(ladoCD_pathSym);
            }





        }


        private void M1_2_3_PAthSymbolFalsoIzqInfer()
        {

            XYZ ptoini_PthSymb = _rebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? -1 : 1) * Math.PI / 2, _deltaZ / 2);

            XYZ ptofin_PthSymb = _rebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);
            ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? 1 : -1) * Math.PI / 2, _deltaZ / 2);

            if (_vectorDireccionLosaInicialExternaInclinadaDTO.IsLosaEncontrada)
            {
                XYZ PA = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, Math.PI + _rebarInferiorDTO.anguloBarraRad + -_vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada);
                ladoAB_pathSym = Line.CreateBound(PA.AsignarZ(elevacionVIew), ptoini_PthSymb);
                //Line ladoAB = Line.CreateBound(ptoini_PthSymb + _vectorDireccionLosaInicialExternaInclinadaDTO.direccionLosa * _extHorDentroEscalera, ptoini_PthSymb);
                ListaFalsoPAthSymbol.Add(ladoAB_pathSym);
            }

            ladoBC_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ListaFalsoPAthSymbol.Add(ladoBC_pathSym);

            if (_vectorDireccionLosaFinalExternaInclinadaDTO.IsLosaEncontrada)
            {
                XYZ PD = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + _vectorDireccionLosaFinalExternaInclinadaDTO.direccionLosa.GetAngleEnZ_respectoPlanoXY(), _largoPataInclinada);
                ladoCD_pathSym = Line.CreateBound(ptofin_PthSymb, PD.AsignarZ(elevacionVIew));
                //Line ladoCD = Line.CreateBound(ptofin_PthSymb, ptofin_PthSymb + direccionBarra * _extHorDentroEscalera);
                ListaFalsoPAthSymbol.Add(ladoCD_pathSym);
            }
        }

        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerNuevoptoCirculo();

            ObtenerPAthSymbolTAG();

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
                _numeroTramosbarra = _numeroTramosbarra,
                _ubicacionPata = _ubicacionPata
            });

        }


    }
}
