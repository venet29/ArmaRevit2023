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
    public class f4_incli : ARebarLosa, IRebarLosa
    {
#pragma warning disable CS0108 // 'f4_incli._uiapp' hides inherited member 'ARebarLosa._uiapp'. Use the new keyword if hiding was intended.
        private readonly UIApplication _uiapp;
#pragma warning restore CS0108 // 'f4_incli._uiapp' hides inherited member 'ARebarLosa._uiapp'. Use the new keyword if hiding was intended.
        private RebarInferiorDTO _RebarInferiorDTO;
        private BuscarLosaIncilnada _buscarLosaIncilnada;
        private XYZ direccionBarraSuperior_inicial;
        private XYZ direccionBarraSuperior_final;
        private double _deltaZInicioFinconSigno;
        private double _deltaZ;

        public f4_incli(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._uiapp = uiapp;
            //  this._uiapp = uiapp;
            //this._doc = _uiapp.ActiveUIDocument.Document;
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;

            ptoMouseAnivelVista = ptoMouseAnivelVista.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz1 = _rebarInferiorDTO.PtoDirectriz1.AsignarZ(elevacionVIew);
            _rebarInferiorDTO.PtoDirectriz2 = _rebarInferiorDTO.PtoDirectriz2.AsignarZ(elevacionVIew);
          //  double deltaMOverAbajo = this._rebarInferiorDTO.espesorBarraEnLOsaFooT + offInferiorHaciaArribaLosa + Util.CmToFoot(0.5);
           // _VectorMover = new XYZ(0, 0, (deltaMOverAbajo));
            direccionBarra = (ptofin - ptoini).Normalize();
            _Prefijo_F = "F=";

        }

        public bool M1A_IsTodoOK()
        {

          //  if (!M0_ObtenerPendienteLosaContigua()) return false;
            if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;
            return true;
        }

        private bool M0_ObtenerPendienteLosaContigua()
        {

            _buscarLosaIncilnada = new BuscarLosaIncilnada(base._uiapp, _rebarInferiorDTO.listaPtosPerimetroBarras, (Floor)_rebarInferiorDTO.floor, 100);

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

            return CopiandoParametrosLado_COnPAthSymbol();

        }


        private void M1_1_1_Configuracion()
        {

            BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto = new BuscarFaceSuperiorConPto(base._uiapp, _rebarInferiorDTO, direccionBarra);

            XYZ DireccionEspesor;
           
            if (_buscarFaceSuperiorConPto.ObtenerDirecionDeLosa(ptoini + direccionBarra * _patabarra / 2+ LargoRecorrido*0.5* dirBarraPerpen, PosicionDeBusqueda.Inicio))
            {
                direccionBarraSuperior_inicial = _buscarFaceSuperiorConPto.DireccionBarraSuperior;
                DireccionEspesor = -1 * _buscarFaceSuperiorConPto.CaraSup.FaceNormal * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT;
            }
            else  if (_buscarFaceSuperiorConPto.ObtenerDirecionDeLosa(_rebarInferiorDTO.ptoSeleccionMouse, PosicionDeBusqueda.Inicio))
            {
                ConstNH.corte();// todo este if
                direccionBarraSuperior_inicial = _buscarFaceSuperiorConPto.DireccionBarraSuperior;
                DireccionEspesor = -1 * _buscarFaceSuperiorConPto.CaraSup.FaceNormal * _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT;
            }
            else
            {
                direccionBarraSuperior_inicial = direccionBarra;
                DireccionEspesor = new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT);
            }
            ladoAB = Line.CreateBound(ptoini + direccionBarraSuperior_inicial * _patabarra, ptoini);

            ladoBC = Line.CreateBound(ptoini, ptoini + DireccionEspesor);
            ladoCD = Line.CreateBound(ptoini + DireccionEspesor, ptofin + DireccionEspesor);
            ladoDE = Line.CreateBound(ptofin + DireccionEspesor, ptofin);

            if (_buscarFaceSuperiorConPto.ObtenerDirecionDeLosa(ptofin + -direccionBarra * _patabarra / 2 +LargoRecorrido * 0.5 * dirBarraPerpen, PosicionDeBusqueda.Fin))
                direccionBarraSuperior_final = _buscarFaceSuperiorConPto.DireccionBarraSuperior;
            else
                direccionBarraSuperior_final = -direccionBarra;
            ladoEG = Line.CreateBound(ptofin, ptofin + direccionBarraSuperior_final * _patabarra);

        }

        #endregion


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {
            _deltaZ = CalcularDiferenciaMAxYMinZ();

            switch (_RebarInferiorDTO.ubicacionLosa)
            {
                case UbicacionLosa.Superior:
                case UbicacionLosa.Derecha:
                    M1_2_3_PAthSymbolFalso();

                    break;
                case UbicacionLosa.Izquierda:
                case UbicacionLosa.Inferior:
                    M1_2_3_PAthSymbolFalso();

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


        private void M1_2_3_PAthSymbolFalso()
        {

            //XYZ ptoini_PthSymb = _RebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            //XYZ ptofin_PthSymb = _RebarInferiorDTO.barraFin.AsignarZ(elevacionVIew); //Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraFin, _RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera);

            XYZ ptoini_PthSymb = _rebarInferiorDTO.barraIni.AsignarZ(elevacionVIew);
            ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? -1 : 1) * Math.PI / 2, _deltaZ / 2);
            XYZ ptofin_PthSymb = _rebarInferiorDTO.barraFin.AsignarZ(elevacionVIew);
            ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _rebarInferiorDTO.anguloBarraRad + (_deltaZInicioFinconSigno > 0 ? 1 : -1) * Math.PI / 2, _deltaZ / 2);

            XYZ direccionBarra = (ptofin_PthSymb - ptoini_PthSymb).Normalize();
            dirBarraPerpen = -Util.GetVectorPerpendicular2(direccionBarra);

            ladoAB_pathSym = Line.CreateBound(ptoini_PthSymb + direccionBarra * _patabarra + dirBarraPerpen * Util.CmToFoot(18), ptoini_PthSymb + dirBarraPerpen * Util.CmToFoot(18));
            ladoBC_pathSym = Line.CreateBound(ptoini_PthSymb + dirBarraPerpen * Util.CmToFoot(18), ptoini_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ladoDE_pathSym = Line.CreateBound(ptofin_PthSymb + dirBarraPerpen * Util.CmToFoot(18), ptofin_PthSymb);
            ladoEF_pathSym = Line.CreateBound(ptofin_PthSymb - direccionBarra * _patabarra + dirBarraPerpen * Util.CmToFoot(18), ptofin_PthSymb + dirBarraPerpen * Util.CmToFoot(18));
            OBtenerListaFalsoPAthSymbol();
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
                diferenciaZInicialFinal = _deltaZInicioFinconSigno
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

        //    M12_MOverHaciaBajo();

        //}


    }
}
