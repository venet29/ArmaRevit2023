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
    public class f3_esc135 : ARebarLosa, IRebarLosa
    {
        private RebarInferiorDTO _RebarInferiorDTO;


        private double _extHorizontalDentroEscalera;

#pragma warning disable CS0108 // 'f3_esc135._largoPataInclinada' hides inherited member 'ARebarLosa._largoPataInclinada'. Use the new keyword if hiding was intended.
        private double _largoPataInclinada;
#pragma warning restore CS0108 // 'f3_esc135._largoPataInclinada' hides inherited member 'ARebarLosa._largoPataInclinada'. Use the new keyword if hiding was intended.
        private double _espesorSinRecubEscalera;
        private double _pataEscalera;

        public f3_esc135(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;

          //  _RebarInferiorDTO.anguloTramoRad = -_RebarInferiorDTO.anguloTramoRad;

            // ptoini    --  ptofin
            _extHorizontalDentroEscalera = Util.CmToFoot(1);

            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _espesorSinRecubEscalera = Util.CmToFoot(11);
            _pataEscalera = Util.CmToFoot(11);

            _Prefijo_F = "F=";
            TipoDireccionBarra_ = _rebarInferiorDTO.TipoDireccionBarra_;
        }

        public bool M1A_IsTodoOK()
        {
            if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;
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
            M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(_RebarInferiorDTO.barraIni.AsignarZ(_RebarInferiorDTO.barraIni.Z - _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT), -direccionBarra);

            ptoini = Util.ExtenderPuntoRespectoVector3d(ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT), -direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);

            XYZ PA = Util.ExtenderPuntoRespectoVector3d(ptoini , -direccionBarra, _RebarInferiorDTO.anguloTramoRad, _largoPataInclinada);
           
            ladoAB = Line.CreateBound(PA,ptoini);
            ladoBC = Line.CreateBound(ptoini,ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT));

        }

        private void M1_1_1_ConfiguracionIzqInfer()
        {
            M1_1_1_1_ObtenerLargoExtensiondelPtoFinal(_RebarInferiorDTO.barraFin.AsignarZ(_RebarInferiorDTO.barraFin.Z - _RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT), direccionBarra);

            ptofin = Util.ExtenderPuntoRespectoVector3d(ptofin + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT), direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);
            // ptofin = Util.ExtenderPuntoRespectoVector3d(ptofin, direccionBarra, Util.GradosToRadianes(0), _extHorizontalDentroEscalera);
            XYZ PE = Util.ExtenderPuntoRespectoVector3d(ptofin , direccionBarra, _RebarInferiorDTO.anguloTramoRad, _largoPataInclinada);
            ladoBC = Line.CreateBound(ptofin,ptoini + new XYZ(0, 0, -_RebarInferiorDTO.espesorBarraEnLOsa_sinRecub_FooT));
            ladoAB = Line.CreateBound(PE,ptofin  );
      
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
            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = _RebarInferiorDTO.barraFin.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            XYZ ptoini_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraIni, Math.PI +- _RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera).AsignarZ(zRefencia); ; 
            XYZ ptofin_PthSymb = _RebarInferiorDTO.barraFin;
            //_RebarInferiorDTO.anguloTramoRad es negativo
            XYZ PE = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptoini_PthSymb, -(-_RebarInferiorDTO.anguloBarraRad + _RebarInferiorDTO.anguloTramoRad + Math.PI), _RebarInferiorDTO.LargoPata);
            PE = PE.AsignarZ(zRefencia);

            ladoAB_pathSym = Line.CreateBound(PE,ptoini_PthSymb);
            ladoBC_pathSym = Line.CreateBound(ptoini_PthSymb,ptofin_PthSymb);
       
            OBtenerListaFalsoPAthSymbol();
        }


        private void M1_2_3_PAthSymbolFalsoIzqInfer()
        {
            // borrar el parametro 'zRefencia' que aisgna un mismo valor  y borrar AsignarZ(zRefencia);
            ConstNH.corte();
            double zRefencia = _RebarInferiorDTO.barraIni.Z; // esto es porque se generar decimales y en z que prodeece q erro al dibujar pq no estan enl planode dibujo

            XYZ ptoini_PthSymb = _RebarInferiorDTO.barraIni;
            XYZ ptofin_PthSymb = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_RebarInferiorDTO.barraFin, _RebarInferiorDTO.anguloBarraRad, _extHorizontalDentroEscalera).AsignarZ(zRefencia);
            XYZ PE = Util.ExtenderPuntoRespectoOtroPtosConAngulo(ptofin_PthSymb, _RebarInferiorDTO.anguloBarraRad + _RebarInferiorDTO.anguloTramoRad, _RebarInferiorDTO.LargoPata).AsignarZ(zRefencia);

            ladoBC_pathSym = Line.CreateBound(ptofin_PthSymb,ptoini_PthSymb);
            ladoAB_pathSym = Line.CreateBound(PE,ptofin_PthSymb);

            OBtenerListaFalsoPAthSymbol();
        }


        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerPAthSymbolTAG();
            return true;
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
        //    // M7_OcultarBarraCreada();
        //    M12_MOverHaciaBajo();
        //}


    }
}
