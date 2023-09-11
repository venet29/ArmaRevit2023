using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos.Tipo.ParaElev

{
    //CLASE QUE OBTIENE LA  FORMA DE LOS ESTROBOS Y TRABAS PARA DIBUJAR UNA VIEW DE ELEVACION
    public class BarraTrabaEstriboConCurva_Elev : ARebarLosa_desglose, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;
        private XYZ _puntoInicialReferencia;

        public string _texToLargoParciales { get; private set; }
        public string _largoTotal { get; private set; }
        public double mayorDistancia { get; set; }
        private double angleRADNormalHostYEJeZ;

        public BarraTrabaEstriboConCurva_Elev(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _Prefijo_F = "F=";
            _puntoInicialReferencia = _RebarInferiorDTO.ptoini;
        }



        public bool M1A_IsTodoOK()
        {
            //if (!M1_1_DatosBarra3d()) return false;
            if (!M1_2_DatosBarra2d()) return false;
            if (!M1_3_PAthSymbolTAG()) return false;

            return true;
        }


        #region comprobacion2

        public bool M1_2_DatosBarra2d()
        {

            List<WraperRebarLargo> listaCuvas = _RebarInferiorDTO.ListaCurvaBarrasFinal_conCurva;

            mayorDistancia = listaCuvas.Max(c => c._curve.Length);

            XYZ centroDeestribo = _RebarInferiorDTO.ptocentroHost;// new XYZ(xprom, yprom, zincial);
            XYZ deltadesplaz = _puntoInicialReferencia - _RebarInferiorDTO.ptocentroHost;
            ObtenerAnguloCaracorte(listaCuvas);


            for (int i = 0; i < listaCuvas.Count; i++)
            {
                WraperRebarLargo item = listaCuvas[i];
                item.PtoInicialTransformada = _view.NH_ObtenerPtoSObreVIew(deltadesplaz + item.ptoInicial);

                item.PtoFinalTransformada = _view.NH_ObtenerPtoSObreVIew(deltadesplaz + item.ptoFinal);

                item.PtoMedioTransformada = _view.NH_ObtenerPtoSObreVIew(deltadesplaz + item.ptoMedio);
            }




            ladoAB_pathSym = Line.CreateBound(listaCuvas[0].PtoInicialTransformada, listaCuvas[0].PtoFinalTransformada);
            ladoBC_pathSym = Arc.Create(listaCuvas[1].PtoInicialTransformada, listaCuvas[1].PtoFinalTransformada, listaCuvas[1].PtoMedioTransformada);
            ladoCD_pathSym = Line.CreateBound(listaCuvas[2].PtoInicialTransformada, listaCuvas[2].PtoFinalTransformada);
            ladoDE_pathSym = Arc.Create(listaCuvas[3].PtoInicialTransformada, listaCuvas[3].PtoFinalTransformada, listaCuvas[3].PtoMedioTransformada);
            ladoEF_pathSym = Line.CreateBound(listaCuvas[4].PtoInicialTransformada, listaCuvas[4].PtoFinalTransformada);

            _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) })";

            _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0)).ToString();

            _ptoTexto = (_puntoInicialReferencia) / 2; //NO APLICA PQ MAL DEFINIDO _RebarInferiorDTO.ptofinal
            //if (_RebarInferiorDTO.Id == -1)
            //    _textoBArras = $" {_RebarInferiorDTO.Clasificacion} {_RebarInferiorDTO.cantidadBarras}Ø{_RebarInferiorDTO.diametroMM} L={_largoTotal}\n {_texToLargoParciales} ";
            //else
            //    _textoBArras = $" {_RebarInferiorDTO.Clasificacion} {_RebarInferiorDTO.cantidadBarras}Ø{_RebarInferiorDTO.diametroMM} L={_largoTotal} id:{_RebarInferiorDTO.Id}\n {_texToLargoParciales} ";

            OBtenerListaFalsoPAthSymbol();

            return true;

        }
        #endregion


        public bool M1_3_PAthSymbolTAG()
        {
            ObtenerPAthSymbolTAG();
            return true;
        }


        private bool ObtenerAnguloCaracorte(List<WraperRebarLargo> listaCuvas)
        {
            try
            {

                angleRADNormalHostYEJeZ = 0;
                return true;
                //revisar si viga tiene angulo posito o negatico
#pragma warning disable CS0162 // Unreachable code detected
                WraperRebarLargo curvaPrinciplar = listaCuvas.Find(c => c.IsBarraPrincipal);
#pragma warning restore CS0162 // Unreachable code detected
                if (curvaPrinciplar == null)
                {
                    Util.ErrorMsg($"No se puedo obtener de coordenadas planos de host");
                    return false;
                }

                angleRADNormalHostYEJeZ = Math.PI / 2 - curvaPrinciplar.direccion.GetAngleEnZ_respectoPlanoXY(false);

            }
            catch (Exception ex)
            {
                UtilDesglose.ErrorMsg($"Error al obtener angulo del plano de corte  ex:{ex.Message}");
                return false;
            }
#pragma warning disable CS0162 // Unreachable code detected
            return true;
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
