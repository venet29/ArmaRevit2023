using System;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.EditarPath;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.Fund.Editar
{
    internal class CambiarDatosFund
    {
        private readonly Autodesk.Revit.UI.UIApplication _uiapp;
        private Document _doc;
        private DatosEditarFundacionesDTO _datosEditarFundacionesDTO;
        private RebarBarType _rebarBarType;
        public int CantidadBarra { get; set; }
        public CambiarDatosFund(Autodesk.Revit.UI.UIApplication _uiapp, DatosEditarFundacionesDTO datosEditarFundacionesDTO)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._datosEditarFundacionesDTO = datosEditarFundacionesDTO;
        }

        public bool M1_ObtenerNuevoTipoDIametro()
        {
            try
            {
                _rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + _datosEditarFundacionesDTO.Diametro_mm, _doc, true);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        public void M2_Editar()
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Borrar pathreiforment por traslapo-NH");

                    var aux_espaciamiento = _datosEditarFundacionesDTO._PathReinforcement.ObtenerDiametro_mm();
                    if (!Util.IsSimilarValor(_datosEditarFundacionesDTO.Diametro_mm, aux_espaciamiento, 0.1))
                        ParameterUtil.SetParaElementId(_datosEditarFundacionesDTO._PathReinforcement, BuiltInParameter.PATH_REIN_TYPE_1, _rebarBarType.Id);


                    var aux_espacia = _datosEditarFundacionesDTO._PathReinforcement.ObtenerEspaciamiento_foot();

                    if (!Util.IsSimilarValor(_datosEditarFundacionesDTO._Espaciamiento_foot, aux_espacia, 0.1))
                    {
                        _datosEditarFundacionesDTO._IsCambioEspaciamiento = true;
                        ParameterUtil.SetParaDouble(_datosEditarFundacionesDTO._PathReinforcement, BuiltInParameter.PATH_REIN_SPACING, _datosEditarFundacionesDTO._Espaciamiento_foot);
                        ParameterUtil.SetParaStringNH(_datosEditarFundacionesDTO._PathReinforcement, "NumeroPrimario", Util.ParteEnteraInt(CantidadBarra).ToString());
                    }
                    else
                        _datosEditarFundacionesDTO._IsCambioEspaciamiento = false;
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                _datosEditarFundacionesDTO._IsCambioEspaciamiento = false;
                string msj = ex.Message;
            }
        }



        public bool M3_RedefinirPathPorEspaciamiento()
        {
            try
            {
                if (!_datosEditarFundacionesDTO._IsCambioEspaciamiento) return false;

                double DeltaDesplazamieto = ObtenerDeltaDesplazamieto();
                if (DeltaDesplazamieto == 0) return false;

                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("ExtederPathApunto-NH");
                    EditarPathRein editarPathRein = new EditarPathRein(_uiapp, _datosEditarFundacionesDTO._seleccionarPathReinfomentConPto);

                    editarPathRein.EditarPath(-DeltaDesplazamieto, -DeltaDesplazamieto, Enumeraciones.DireccionEdicionPathRein.Superior);

                    transGroup.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private double ObtenerDeltaDesplazamieto()
        {
            double delta = 0;
            try
            {
                double EspaciamientoFoot = _datosEditarFundacionesDTO._Espaciamiento_foot;

                CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_datosEditarFundacionesDTO._PathReinforcement, _doc);
                pathReinformeCalculos.Calcular4PtosPathReinf();
                CoordenadaPath _coordenadaPath = pathReinformeCalculos.Obtener4pointPathReinf();

                double LargoRecorridoFoot = _coordenadaPath.p4.DistanceTo(_coordenadaPath.p3);

                double _nuevacantidadBArras = Math.Round(LargoRecorridoFoot / EspaciamientoFoot, 5);

                long CantidadBarra_ = (long)_nuevacantidadBArras;

                if (CantidadBarra_ < 1)
                    CantidadBarra_ = 1;
                delta = (LargoRecorridoFoot - (CantidadBarra_ * EspaciamientoFoot + Util.MmToFoot(_datosEditarFundacionesDTO.Diametro_mm))) / 2;

                CantidadBarra = (int)CantidadBarra_ + 1;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ObtenerDeltaDesplazamieto'  ex:{ex.Message}");
                return 0;
            }
            return delta;
        }
    }
}