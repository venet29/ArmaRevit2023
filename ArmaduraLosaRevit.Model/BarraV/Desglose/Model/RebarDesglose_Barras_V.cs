using ApiRevit;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GEOM;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Model
{

    //solo  barras verticales
    public class RebarDesglose_Barras_V
    {
        static int nextId = 0;
        public int contBarra { get; set; }
        public RebarDesglose _rebarDesglose { get; set; }
        public TipoRebar _tipoBarraEspecifico { get; private set; }
        public bool analizadasuperior { get; set; }
        public bool IsTraslapable { get; set; }
        public XYZ ptoInicial { get; set; }
        public XYZ ptoMedio { get; set; }
        public XYZ ptoFinal { get; set; }
        public Line curvePrincipal { get; set; }
        public int diametroMM { get; set; }
        public Reference refenciaInicial { get; private set; }
        public Reference refenciaFinal { get; private set; }
        public direccionBarra _direccion { get; set; }

        public RebarDesglose_Barras_V(RebarDesglose _RebarDesglose)
        {
            nextId += 1;
            contBarra = nextId;
            diametroMM = _RebarDesglose._rebar.ObtenerDiametroInt();
            this._rebarDesglose = _RebarDesglose;
            this._tipoBarraEspecifico = _RebarDesglose._tipoBarraEspecifico;
            analizadasuperior = false;
            IsTraslapable = true;
        }


        public bool Ordenar_UltimaCurvaMayorZ()
        {
            try
            {
                if (_tipoBarraEspecifico == TipoRebar.ELEV_BA_V)
                {
                    if (_rebarDesglose.ListaCurvaBarras[0].ptoFinal.Z > _rebarDesglose.ListaCurvaBarras.Last().ptoFinal.Z)
                        _rebarDesglose.ListaCurvaBarras.Reverse();
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'UltimaCurvaMayorZ'   ex: {ex.Message}");
                return false;
            }
            return true;
        }


        public RebarElevDTO ObtenerRebarElevDTO(XYZ ptoB, UIApplication _uiapp, bool isId, Config_EspecialElev _config_EspecialElv)
        {
            View _view = _uiapp.ActiveUIDocument.Document.ActiveView;
            RebarElevDTO _newRebarElevDTO = null;
            try
            {
                _newRebarElevDTO = new RebarElevDTO(_uiapp)
                {
                    tipoBarra = _rebarDesglose.C_obtenerTipobarra(),
                    LargoPata = ptoFinal.DistanceTo(ptoFinal),
                    ptoini = ptoB.AsignarZ(ptoInicial.Z),
                    ptofinal = ptoB.AsignarZ(ptoFinal.Z),
                    DireccionPataEnFierrado = -_view.RightDirection,
                    diametroMM = diametroMM,
                    listaCUrvas = _rebarDesglose.ListaCurvaBarras,
                    cantidadBarras = contBarra,
                    Rebar_ = _rebarDesglose._rebar,
                    Id = (isId ? _rebarDesglose._rebar.Id.IntegerValue : -1),
                    TipoBarraEspecifico = _tipoBarraEspecifico,
                    Config_EspecialElv = _config_EspecialElv

                };
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al  'ObtenerRebarElevDTO'   ex: {ex.Message}");
                return null;
            }
            return _newRebarElevDTO;
        }



        public RebarElevDTO ObtenerRebarCorteDTO(XYZ posicionAUX,XYZ ptocentroHost,UIApplication _uiapp, View _view, View _viewOriginal, Config_EspecialCorte _Config_EspecialCorte)
        {
            //View _view = _uiapp.ActiveUIDocument.Document.ActiveView;
            //var _ParametroShareNhDTO = new ParametroShareNhDTO() {
            //    Isok=true,
            //    NombrePAra= "LargoSumaParciales",
            //    valor= Util.FootToCm( _rebarDesglose.LargoTotalSumaParciales).ToString()
            //};

            //_Config_EspecialCorte.ListaPAraShare.Add(_ParametroShareNhDTO);

            RebarElevDTO _newRebarElevDTO = null;
            try
            {
                _newRebarElevDTO = new RebarElevDTO(_uiapp)
                {
                    tipoBarra = _rebarDesglose.C_obtenerTipoEstribo_V(),
                    LargoPata = ptoFinal.DistanceTo(ptoFinal),
                    ptoini = posicionAUX,                   
                    ptofinal = posicionAUX + (ptoFinal - ptoInicial).Normalize() * 1,//valor no se  utiliza y se agrega  new XYZ(0,0,1)  para evitar error
                    DireccionPataEnFierrado = -_view.RightDirection,
                    diametroMM = diametroMM,
                    listaCUrvas = _rebarDesglose.ListaCurvaBarras,
                    cantidadBarras = contBarra,
                    Rebar_= _rebarDesglose._rebar,
                    Id = _rebarDesglose._rebar.Id.IntegerValue,
                    TipoBarraEspecifico = _tipoBarraEspecifico,

                    _View = _view,
                    _viewOriginal= _viewOriginal,//valido solo arco de estribo y traba para dibujo tranversal
                    ptoMedio = ptoMedio, //valido solo arco de estribo y traba para dibujo tranversal
                    ListaCurvaBarrasFinal_conCurva = _rebarDesglose.ListaCurvaBarrasFinal_conCurva_Estribo, //solo para dijar corte real re estribo
                    ptocentroHost = ptocentroHost,//solo para dijar corte real re estribo
                    Config_EspecialCorte= _Config_EspecialCorte,//valido solo arco de estribo y traba para dibujo tranversal
                    LargoTotalSumaParcialesFoot=_rebarDesglose.LargoTotalSumaParcialesFoot//valido solo arco de estribo y traba para dibujo tranversal
                };
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al  'ObtenerRebarElevDTO'   ex: {ex.Message}");
                return null;
            }
            return _newRebarElevDTO;
        }

        public bool Ordenar_AnalizarEstribo()
        {
            try
            {
                GemetrieLine _GemetrieLIne = new GemetrieLine(_rebarDesglose._uiapp, _rebarDesglose._rebar);
                if (_GemetrieLIne.ObtenerLine())
                {
                    refenciaInicial = _GemetrieLIne.ListaResult[0];
                    refenciaFinal = _GemetrieLIne.ListaResult.Last();
                }

                var curveMasFinal = _rebarDesglose.ListaCurvaBarrasFinal_Estribo[0];

                var curveMasInicial = _rebarDesglose.ListaCurvaBarras[0];

                Getrefere();
                _direccion = direccionBarra.NONE;
                if (curveMasFinal.ptoInicial.Z > curveMasInicial.ptoInicial.Z)
                {
                    curvePrincipal = (Line)curveMasInicial._curve;
                    ptoInicial = curveMasInicial.ptoInicial;
                    ptoFinal = curveMasInicial.ptoFinal;
                    IsTraslapable = true;

                    //reasiganr z del estribo mas alto
                    XYZ PtocurveMasFinal = curveMasFinal.ptoInicial;
                    ptoFinal = ptoFinal.AsignarZ(PtocurveMasFinal.Z);
                }
                else
                {
                    curvePrincipal = (Line)curveMasFinal._curve;
                    ptoInicial = curveMasFinal.ptoInicial;
                    ptoFinal = curveMasFinal.ptoFinal;
                    IsTraslapable = true;

                    //reasiganr z del estribo mas alto
                    XYZ PtocurveMasFinal = curveMasInicial.ptoInicial;
                    ptoFinal = ptoFinal.AsignarZ(PtocurveMasFinal.Z);
                }

                ptoMedio = (ptoFinal + ptoInicial) / 2;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'analizar'  barraId:{_rebarDesglose._rebar.Id}  ex: {ex.Message}");
                return false;
            }
            return true;
        }

        private void Getrefere()
        {
            IList<RebarConstrainedHandle> dd = _rebarDesglose._rebar.GetRebarConstraintsManager().GetAllHandles();

            RebarConstrainedHandle s = null;
            RebarConstrainedHandle e = null;
            foreach (RebarConstrainedHandle rbh in dd)
            {
                if (rbh.GetHandleName().ToString() == "Start of Bar")
                {
                    s = rbh;
                }
                if (rbh.GetHandleName().ToString() == "End of Bar")
                {
                    e = rbh;
                }
            }
        }

        public bool Ordenar_Analizar()
        {
            try
            {
                var curveMasLArga = (Line)_rebarDesglose.ListaCurvaBarras.OrderByDescending(c => c._curve.Length).FirstOrDefault()._curve;

                if (UtilDesglose.IsmasVertical(curveMasLArga))
                    _direccion = direccionBarra.Vertical;
                else
                {
                    _direccion = direccionBarra.Horizontal;
                    return true;
                }


                if (_rebarDesglose.ListaCurvaBarras[0].IsBarraPrincipal)
                {
                    curvePrincipal = (Line)_rebarDesglose.ListaCurvaBarras[0]._curve;
                    ptoInicial = _rebarDesglose.ListaCurvaBarras[0].ptoInicial;
                    ptoFinal = _rebarDesglose.ListaCurvaBarras[0].ptoFinal;

                    IsTraslapable = (UtilDesglose.IsmasVertical((ptoFinal - ptoInicial).Normalize()) ? true : false);
                }
                else if (_rebarDesglose.ListaCurvaBarras.Last().IsBarraPrincipal)
                {
                    curvePrincipal = (Line)_rebarDesglose.ListaCurvaBarras.Last()._curve;
                    ptoInicial = _rebarDesglose.ListaCurvaBarras.Last().ptoInicial;
                    ptoFinal = _rebarDesglose.ListaCurvaBarras.Last().ptoFinal;
                    IsTraslapable = (UtilDesglose.IsmasVertical((ptoFinal - ptoInicial).Normalize()) ? true : false);
                }
                else
                {

                    WraperRebarLargo _masLargo = _rebarDesglose.ListaCurvaBarras.OrderByDescending(c => c._curve.Length).FirstOrDefault();
                    curvePrincipal = (Line)_masLargo._curve;
                    ptoInicial = _masLargo.ptoInicial;
                    ptoFinal = _masLargo.ptoFinal;
                    IsTraslapable = false;
                }
                ptoMedio = (ptoFinal + ptoInicial) / 2;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'analizar'  barraId:{_rebarDesglose._rebar.Id}  ex: {ex.Message}");
                return false;
            }
            return true;
        }


        public bool SepuedeTraslaparSUperior()
        {
            try
            {
                WraperRebarLargo utimoCUrva = _rebarDesglose.ListaCurvaBarras.Last();
                return (utimoCUrva.IsBarraPrincipal ? true : false);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'UltimaCurvaMayorZ'   ex: {ex.Message}");
                return false;
            }
        }
    }
}
