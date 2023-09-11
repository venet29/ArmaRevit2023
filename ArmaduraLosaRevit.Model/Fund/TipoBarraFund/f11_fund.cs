using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Barras;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Servicio;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.Fund.TipoBarraFund

{
    // para caso f1 con para hacia arriba o bajo, cuando se encuentra con rampa
    public class f11_fund : ARebarLosa, IRebarLosa
    {
#pragma warning disable CS0108 // 'f4_incli._uiapp' hides inherited member 'ARebarLosa._uiapp'. Use the new keyword if hiding was intended.
        private readonly UIApplication _uiapp;
#pragma warning restore CS0108 // 'f4_incli._uiapp' hides inherited member 'ARebarLosa._uiapp'. Use the new keyword if hiding was intended.
        private RebarInferiorDTO _RebarInferiorDTO;

        private XYZ direccionBarraSuperior_inicial;
        private XYZ direccionBarraSuperior_final;
        private double _deltaZInicioFinconSigno;
        private double _deltaZ;
        XYZ DireccionEspesor;
        private double LArgoPataHOok_foot;

        private double LArgoPataHOokIzq_foot;
        private double LArgoPataHOokDere_foot;
        private XYZ DireccionEspesor_izq;
        private XYZ DireccionEspesor_Dere;

        public f11_fund(UIApplication uiapp, RebarInferiorDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._uiapp = uiapp;
            //  this._uiapp = uiapp;
            //this._doc = _uiapp.ActiveUIDocument.Document;
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            LArgoPataHOokIzq_foot = Util.CmToFoot(_RebarInferiorDTO.DatosNuevaBarraDTO.LargoPAtaIzqHook_cm - _RebarInferiorDTO.diametroMM / 20.0D);
            LArgoPataHOokDere_foot = Util.CmToFoot(_RebarInferiorDTO.DatosNuevaBarraDTO.LargoPAtaDereHook_cm - _RebarInferiorDTO.diametroMM / 20.0D);
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

            mitadDiam_foot = _RebarInferiorDTO.diametroFoot / 2;


            ptoini = ptoini + XYZ.BasisZ * (ConstNH.RECUBRIMIENTO_FUNDACIONES_foot + _RebarInferiorDTO.diametroFoot / 2);
            ptofin = ptofin + XYZ.BasisZ * (ConstNH.RECUBRIMIENTO_FUNDACIONES_foot + _RebarInferiorDTO.diametroFoot / 2);

            var planarface = _rebarInferiorDTO.planarfaceAnalizada;
            DireccionEspesor = -planarface.FaceNormal * LArgoPataHOok_foot;

            DireccionEspesor_izq = -planarface.FaceNormal * LArgoPataHOokIzq_foot;
            DireccionEspesor_Dere = -planarface.FaceNormal * LArgoPataHOokDere_foot;

            ladoAB = Line.CreateBound(ptoini + DireccionEspesor_izq, ptoini);
            ladoBC = Line.CreateBound(ptoini, ptofin);
            ladoCD = Line.CreateBound(ptofin, ptofin + DireccionEspesor_Dere);

            _rebarInferiorDTO.TexToLargoParciales = $"({Math.Round(Util.FootToCm(ladoAB.Length + mitadDiam_foot), 0)}+{Math.Round(Util.FootToCm(ladoBC.Length + mitadDiam_foot * 2), 0)}+{Math.Round(Util.FootToCm(ladoCD.Length + mitadDiam_foot), 0)})";
            _rebarInferiorDTO.LargoTotal = Math.Round(Util.FootToCm(ladoAB.Length + mitadDiam_foot), 0) + Math.Round(Util.FootToCm(ladoBC.Length + mitadDiam_foot * 2), 0) + Math.Round(Util.FootToCm(ladoCD.Length + mitadDiam_foot));
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

            //ladoAB_pathSym = Line.CreateBound(ptoini_PthSymb + direccionBarra * _patabarra + dirBarraPerpen * Util.CmToFoot(18), ptoini_PthSymb + dirBarraPerpen * Util.CmToFoot(18));
            ladoBC_pathSym = Line.CreateBound(ptoini_PthSymb + dirBarraPerpen * LArgoPataHOokIzq_foot, ptoini_PthSymb);
            ladoCD_pathSym = Line.CreateBound(ptoini_PthSymb, ptofin_PthSymb);
            ladoDE_pathSym = Line.CreateBound(ptofin_PthSymb + dirBarraPerpen * LArgoPataHOokDere_foot, ptofin_PthSymb);
            //  ladoEF_pathSym = Line.CreateBound(ptofin_PthSymb - direccionBarra * _patabarra + dirBarraPerpen * Util.CmToFoot(18), ptofin_PthSymb + dirBarraPerpen * Util.CmToFoot(18));

            // _texToLargoParciales = $"({Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0)}+{Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0)}+{Math.Round(Util.FootToCm(ladoCD_pathSym.Length), 0)})";

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
                diferenciaZInicialFinal = _deltaZInicioFinconSigno,
                HorientacionTag = (_rebarInferiorDTO.ubicacionLosa == UbicacionLosa.Izquierda ? TagOrientation.Horizontal : TagOrientation.Vertical)
            });

        }



    }
}
