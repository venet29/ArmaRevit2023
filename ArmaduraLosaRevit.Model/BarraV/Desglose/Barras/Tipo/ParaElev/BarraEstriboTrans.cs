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

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos.Tipo.ParaElev

{
    //CLASE QUE OBTIENE LA  FORMA DE LOS ESTROBOS Y TRABAS PARA DIBUJAR UNA VIEW DE ELEVACION
    public class BarraEstriboTrans : ARebarLosa_desglose, IRebarLosa_Desglose
    {
        private readonly UIApplication uiapp;
        private RebarElevDTO _RebarInferiorDTO;
        public string _texToLargoParciales { get; private set; }
        public string _largoTotal { get; private set; }
        public double mayorDistancia { get;  set; }

        public BarraEstriboTrans(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this.uiapp = uiapp;
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _Prefijo_F = "F=";

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

            List<WraperRebarLargo> listaCuvas = _RebarInferiorDTO.listaCUrvas;
            double pataSuperior = listaCuvas.Find(c=> !c.IsBarraPrincipal)._curve.Length;
            double zincial = listaCuvas[0].ptoInicial.Z;
            var xprom = listaCuvas.Average(c => c.ptoMedio.X);
            var yprom = listaCuvas.Average(c => c.ptoMedio.Y);

            XYZ centroDeestribo = new XYZ(xprom, yprom, zincial);

            CrearTrasformadaSobreVector _Trasform = new CrearTrasformadaSobreVector(centroDeestribo, -90, _view.RightDirection);


            mayorDistancia = 0;
            for (int i = 0; i < listaCuvas.Count; i++)
            {
                WraperRebarLargo item = listaCuvas[i];

                XYZ auxIni = _Trasform.EjecutarTransform(item.ptoInicial.AsignarZ(zincial));
                mayorDistancia = Math.Max(mayorDistancia, Util.ModuloVector(auxIni.AsignarZ(0)));
                item.PtoInicialTransformada = _RebarInferiorDTO.ptoini + auxIni;

                XYZ auxFin = _Trasform.EjecutarTransform(item.ptoFinal.AsignarZ(zincial));
                mayorDistancia = Math.Max(mayorDistancia, Util.ModuloVector(auxFin.AsignarZ(0)));
                item.PtoFinalTransformada = _RebarInferiorDTO.ptoini + auxFin;

            }


            ladoAB_pathSym = Line.CreateBound(listaCuvas[0].PtoInicialTransformada, listaCuvas[0].PtoFinalTransformada);
            ladoBC_pathSym = Line.CreateBound(listaCuvas[1].PtoInicialTransformada, listaCuvas[1].PtoFinalTransformada);
            ladoCD_pathSym = Line.CreateBound(listaCuvas[2].PtoInicialTransformada, listaCuvas[2].PtoFinalTransformada);

            ladoDE_pathSym = Line.CreateBound(listaCuvas[3].PtoInicialTransformada, listaCuvas[3].PtoFinalTransformada);
            ladoEF_pathSym = Line.CreateBound(listaCuvas[4].PtoInicialTransformada, listaCuvas[4].PtoFinalTransformada);
            ladoFG_pathSym = Line.CreateBound(listaCuvas[5].PtoInicialTransformada, listaCuvas[5].PtoFinalTransformada);

            _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) })";

             _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0)).ToString();

            _ptoTexto = (_RebarInferiorDTO.ptoini + _RebarInferiorDTO.ptofinal) / 2;
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
