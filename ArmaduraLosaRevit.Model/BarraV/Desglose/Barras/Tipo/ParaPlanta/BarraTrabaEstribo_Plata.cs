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
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos.Tipo.ParaPlanta

{
    public class BarraTrabaEstribo_Plata : ARebarLosa_desglose, IRebarLosa_Desglose
    {
        private RebarElevDTO _RebarInferiorDTO;
        public string _texToLargoParciales { get; private set; }
        public string _largoTotal { get; private set; }
        public double mayorDistancia { get; set; }


        public BarraTrabaEstribo_Plata(UIApplication uiapp, RebarElevDTO _rebarInferiorDTO, IGeometriaTag newGeometriaTag) : base(_rebarInferiorDTO)
        {
            this._RebarInferiorDTO = _rebarInferiorDTO;
            _newGeometriaTag = newGeometriaTag;
            _largoPataInclinada = _rebarInferiorDTO.LargoPata;
            _Prefijo_F = "F=";
            if (_RebarInferiorDTO.Config_EspecialCorte.ListaPAraShare != null)
                listaPArametroSharenh = _RebarInferiorDTO.Config_EspecialCorte.ListaPAraShare;

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
            double pataSuperior = listaCuvas.Find(c => !c.IsBarraPrincipal)._curve.Length;

            CrearTrasformadaSobreVector _Trasform = new CrearTrasformadaSobreVector(listaCuvas[0].ptoInicial, 90, _view.RightDirection);

            for (int i = 0; i < listaCuvas.Count; i++)
            {
                WraperRebarLargo item = listaCuvas[i];
                // _RebarInferiorDTO.ptoini : es el pto seleccion con mouse
                item.PtoInicialTransformada = _RebarInferiorDTO.ptoini + _Trasform.EjecutarTransform(item.ptoInicial);
                item.PtoFinalTransformada = _RebarInferiorDTO.ptoini + _Trasform.EjecutarTransform(item.ptoFinal);

            }




            ladoAB_pathSym = Line.CreateBound(listaCuvas[0].PtoInicialTransformada, listaCuvas[0].PtoFinalTransformada);
            ladoBC_pathSym = Line.CreateBound(listaCuvas[1].PtoInicialTransformada, listaCuvas[1].PtoFinalTransformada);
            ladoCD_pathSym = Line.CreateBound(listaCuvas[2].PtoInicialTransformada, listaCuvas[2].PtoFinalTransformada);

            _texToLargoParciales = $"({ Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) }+{ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) })";

             _largoTotal = (Math.Round(Util.FootToCm(ladoAB_pathSym.Length), 0) + Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0)+ Math.Round(Util.FootToCm(ladoBC_pathSym.Length), 0) ).ToString();

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
