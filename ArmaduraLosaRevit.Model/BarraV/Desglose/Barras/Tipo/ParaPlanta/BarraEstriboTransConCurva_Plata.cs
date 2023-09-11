using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos.Tipo.ParaPlanta

{
    public class BarraEstriboTransConCurva_Plata : ARebarLosa_desglose, IRebarLosa_Desglose
    {
        private readonly UIApplication uiapp;
        private RebarElevDTO _RebarInferiorDTO;
        private XYZ _puntoInicialReferencia;
      

        public string _texToLargoParciales { get; private set; }
        public string _largoTotal { get; private set; }
        public double mayorDistancia { get; set; }

        public BarraEstriboTransConCurva_Plata(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this.uiapp = uiapp;
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _Prefijo_F = "F=";
            _puntoInicialReferencia = _RebarInferiorDTO.ptoini;

            if (_RebarInferiorDTO.Config_EspecialCorte.ListaPAraShare != null)
                listaPArametroSharenh = _RebarInferiorDTO.Config_EspecialCorte.ListaPAraShare;

          //  _configLargo = _RebarInferiorDTO.Config_EspecialCorte.TipoCOnfigLargo;
          //  _tolerancia = _RebarInferiorDTO.Config_EspecialCorte.tolerancia;
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
            //double pataSuperior = listaCuvas.Find(c=> !c.IsBarraPrincipal)._curve.Length;
            double zincial = listaCuvas[0].ptoInicial.Z;
            //var xprom = listaCuvas.Average(c => c.ptoMedio.X);
            //var yprom = listaCuvas.Average(c => c.ptoMedio.Y);

            XYZ centroDeestribo = _RebarInferiorDTO.ptocentroHost;// new XYZ(xprom, yprom, zincial);

          //  CrearTrasformadaSobreVector _Trasform = new CrearTrasformadaSobreVector(centroDeestribo, -90, _view.RightDirection);

            XYZ deltadesplaz = _puntoInicialReferencia - _RebarInferiorDTO.ptocentroHost;
            // mayorDistancia = 0;
            for (int i = 0; i < listaCuvas.Count; i++)
            {
                WraperRebarLargo item = listaCuvas[i];

                item.PtoInicialTransformada = (deltadesplaz + item.ptoInicial).AsignarZ(_puntoInicialReferencia.Z);
                item.PtoFinalTransformada = (deltadesplaz + item.ptoFinal).AsignarZ(_puntoInicialReferencia.Z);
                //if (item.TipoCurva == Ayuda.TipoCUrva.arco)
                item.PtoMedioTransformada = (deltadesplaz + item.ptoMedio).AsignarZ(_puntoInicialReferencia.Z);

            }

            ladoAB_pathSym = Line.CreateBound(listaCuvas[0].PtoInicialTransformada, listaCuvas[0].PtoFinalTransformada);
            ladoBC_pathSym = Arc.Create(listaCuvas[1].PtoInicialTransformada, listaCuvas[1].PtoFinalTransformada, listaCuvas[1].PtoMedioTransformada);
            ladoCD_pathSym = Line.CreateBound(listaCuvas[2].PtoInicialTransformada, listaCuvas[2].PtoFinalTransformada);
            ladoDE_pathSym = Arc.Create(listaCuvas[3].PtoInicialTransformada, listaCuvas[3].PtoFinalTransformada, listaCuvas[3].PtoMedioTransformada);
            ladoEF_pathSym = Line.CreateBound(listaCuvas[4].PtoInicialTransformada, listaCuvas[4].PtoFinalTransformada);

            ladoFG_pathSym = Arc.Create(listaCuvas[5].PtoInicialTransformada, listaCuvas[5].PtoFinalTransformada, listaCuvas[5].PtoMedioTransformada);
            ladoGH_pathSym = Line.CreateBound(listaCuvas[6].PtoInicialTransformada, listaCuvas[6].PtoFinalTransformada);
            ladoHI_pathSym = Arc.Create(listaCuvas[7].PtoInicialTransformada, listaCuvas[7].PtoFinalTransformada, listaCuvas[7].PtoMedioTransformada);
            ladoIJ_pathSym = Line.CreateBound(listaCuvas[8].PtoInicialTransformada, listaCuvas[8].PtoFinalTransformada);
            ladoJK_pathSym = Arc.Create(listaCuvas[9].PtoInicialTransformada, listaCuvas[9].PtoFinalTransformada, listaCuvas[9].PtoMedioTransformada);
            ladoKL_pathSym = Line.CreateBound(listaCuvas[10].PtoInicialTransformada, listaCuvas[10].PtoFinalTransformada);



            _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) })";

            _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0)
                            + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0)
                            + Math.Round(Util.FootToCm(ladoCD_pathSym.Length), 0)
                            + Math.Round(Util.FootToCm(ladoDE_pathSym.Length), 0)
                            + Math.Round(Util.FootToCm(ladoEF_pathSym.Length), 0)
                            + Math.Round(Util.FootToCm(ladoFG_pathSym.Length), 0)
                            + Math.Round(Util.FootToCm(ladoGH_pathSym.Length), 0)).ToString();

            if (_configLargo == TipoCOnfLargo.Aprox5)
                redonderar_5();
            else if (_configLargo == TipoCOnfLargo.Aprox10)
                redonderar_10();


            _ptoTexto = (_puntoInicialReferencia) / 2;//NO APLICA PQ MAL DEFINIDO _RebarInferiorDTO.ptofinal
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



    }
}
