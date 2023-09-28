using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarra
{
    public class EditarBarraV
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected View _view;
        protected EditarBarraDTO _editarBarraDTO;
        protected  SeleccionarRebarElemento seleccionarRebarElemento;
        protected SeleccionarTagRebar seleccionarTagRebar;
        protected IndependentTag _independeTag;

        private double EspesorElementoHost;
        private double espaciamientoFoot;
        private double recorridoBarrar;
        protected Curve curvaMAyorlargo;

        public Rebar RebarSeleccion { get; set; }
        public XYZ ptoini { get; set; }
        public XYZ ptofinal { get; set; }

        public XYZ ptoPosicionTAg { get; set; }

        public XYZ DireccionEnFierrado { get; set; }

        public int Diametro_mm { get; set; }
        public int cantidadBarras { get; set; }
        public Element ElementMuroHost { get; set; }
        public IntervaloBarrasDTO intervaloBarrasDTO { get; set; }

        public bool IsOk { get; set; }

        public EditarBarraV(UIApplication uiapp, EditarBarraDTO editarBarraDTO, SeleccionarTagRebar seleccionarTagRebar)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._editarBarraDTO = editarBarraDTO;
            this.seleccionarTagRebar = seleccionarTagRebar;
            this.IsOk = true;


        }
        public EditarBarraV(UIApplication uiapp, EditarBarraDTO editarBarraDTO, SeleccionarRebarElemento seleccionarRebarElemento)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._editarBarraDTO = editarBarraDTO;
            this.seleccionarRebarElemento = seleccionarRebarElemento;
            this.RebarSeleccion = seleccionarRebarElemento._RebarSeleccion;
            this.IsOk = true;


        }
        public bool ObtenerRebarYTag()
        {
            try
            {
                _independeTag = seleccionarTagRebar.independentTag;
                RebarSeleccion = _independeTag.Obtener_GetTaggedLocalElement(_uiapp) as Rebar;

                if (RebarSeleccion.Pinned)
                {
                    Util.ErrorMsg($"Barra tiene Pin asigando, no es posible editar. ");
                    return false;
                }
                ptoPosicionTAg = _independeTag.TagHeadPosition;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:  {ex.Message}  ");
                return false;
            }
            return true;
        }

    

        public bool A_ObtenerDatosDebarra()
        {
            try
            {
                Diametro_mm = (_editarBarraDTO.IsCambiarDiametroYEspacia ? _editarBarraDTO.diametro : Convert.ToInt32(RebarSeleccion.LookupParameter("Bar Diameter").AsValueString().Replace("mm", "")));

               
                //  cantidadBarras = (_editarBarraDTO.IsCambiarDiametroYEspacia ? _editarBarraDTO.cantidad : Convert.ToInt32(RebarSeleccion.LookupParameter("CantidadBarra").AsString()));
                cantidadBarras = (_editarBarraDTO.IsCambiarDiametroYEspacia ? _editarBarraDTO.cantidad : RebarSeleccion.LookupParameter("Quantity").AsInteger());
                ElementMuroHost = _doc.GetElement(RebarSeleccion.GetHostId());

                if (!M1_ObtenerEspesorHost()) return IsOk = false;
                if (!M2_ObtenerPtoInicialYfinal()) return IsOk = false;
                M3_ObtenerDireccionENfierrado();
                M4_ObtenerPosiciontag();// solo sirve para barra horizontal

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'ObtenerDatosDebarra' ->  ex:{ex.Message}");
                return IsOk = false;
            }

            return true;
        }


        #region Metodos ObtenerDatosDebarra

        private bool M1_ObtenerEspesorHost()
        {
            try
            {


                if (ElementMuroHost is Wall)
                    EspesorElementoHost = (ElementMuroHost as Wall).ObtenerEspesorMuroFoot(_doc);
                else if (ElementMuroHost is Floor)
                    EspesorElementoHost = Util.CmToFoot((ElementMuroHost as Floor).ObtenerEspesorLosaCm(_doc));
                else if (ElementMuroHost is FamilyInstance)
                    EspesorElementoHost = (ElementMuroHost as FamilyInstance).ObtenerEspesorConCaraVerticalVIsible_foot(_view);
                else
                {
                    Util.ErrorMsg($"ObtenerEspesorHost -> no se encontro tipo ");
                    return false;
                }

                if (cantidadBarras == 1)
                    espaciamientoFoot = -1;
                else
                    espaciamientoFoot = (EspesorElementoHost - Util.CmToFoot(ConstNH.RECUBRIMIENTO_MURO_CM + (Diametro_mm) / 10) * 2.0) / (cantidadBarras - 1);
                recorridoBarrar = (EspesorElementoHost - Util.CmToFoot(ConstNH.RECUBRIMIENTO_MURO_CM + (Diametro_mm) / 10) * 2.0);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"ObtenerEspesorHost  ex:{ex.Message}");
                return true;
            }
            return true;
        }

        private bool M2_ObtenerPtoInicialYfinal()
        {
            try
            {

                curvaMAyorlargo = RebarSeleccion.GetCenterlineCurves(false, false, true, MultiplanarOption.IncludeOnlyPlanarCurves, 0).MinBy(c => -c.Length);

                if (curvaMAyorlargo.GetEndPoint(0).Z > curvaMAyorlargo.GetEndPoint(1).Z)
                {
                    ptofinal = curvaMAyorlargo.GetEndPoint(0);
                    ptoini = curvaMAyorlargo.GetEndPoint(1);
                }
                else
                {
                    ptofinal = curvaMAyorlargo.GetEndPoint(1);
                    ptoini = curvaMAyorlargo.GetEndPoint(0);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" 'ObtenerPtoInicialYfinal'  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void M3_ObtenerDireccionENfierrado()
        {

            if (_editarBarraDTO.TipoCasobarra == TipoCasobarra.BarraVertical)
            {
                XYZ vectorSeleccion = ptoini.GetXY0() - ptoPosicionTAg.GetXY0();
                double resul = Util.GetProductoEscalar(vectorSeleccion.Normalize(), _view.RightDirection.GetXY0());
                DireccionEnFierrado = (resul > 0 ?
                                           _view.RightDirection :
                                           new XYZ(-_view.RightDirection.X, -_view.RightDirection.Y, _view.RightDirection.Z));
            }
            else
            {
                if (ptoPosicionTAg != null)
                    DireccionEnFierrado = (ptoPosicionTAg.Z > ptoini.Z ? new XYZ(0, 0, -1) : new XYZ(0, 0, 1));

            }

        }
        private void M4_ObtenerPosiciontag()
        {
            //si es horizontal se cambio
            if (_editarBarraDTO.TipoCasobarra == TipoCasobarra.BarrasHorizontal || _editarBarraDTO.TipoCasobarra == TipoCasobarra.BarraRefuerzoLosa)
            {

                ptoPosicionTAg = (ptofinal + ptoini) / 2;// + -DireccionEnFierrado * (DireccionEnFierrado.Z == 1 ? 0.8 : 0.2);
            }
        }

        #endregion


        #region Metodos Obtener DTO

        public IntervaloBarrasDTO GenerarIntervaloBarrasDTO(XYZ ViewDirectionEntradoView, XYZ DireccionRecorridoBarra)
        {
            return new IntervaloBarrasDTO()
            {
                diametroMM = Diametro_mm,
                DireccionPataEnFierrado = DireccionEnFierrado.RedondearSIHAYCERO(),
                ElementoHost = ElementMuroHost,
                ptoini = ptoini.Redondear8(),
                ptofinal = ptofinal.Redondear8(),
                ptoPosicionTAg = ptoPosicionTAg,
                tipobarraV = _editarBarraDTO.tipobarraV,
                view3D_paraVisualizar = _editarBarraDTO.view3D_paraVisualizar,
                _view3D_paraBuscar = _editarBarraDTO.view3D_paraBuscar,
                _viewActual = _editarBarraDTO.viewActual,
                IsbarraIncial = false,
                _parametrosInternoRebarDTO = new ParametrosInternoRebarDTO()
                {

                    _texToCantidadoBArras = cantidadBarras.ToString(),
                    _texToTipoDiam = Diametro_mm.ToString()

                },
                ViewDirectionEntradoView = ViewDirectionEntradoView,
                DireccionRecorridoBarra = DireccionRecorridoBarra,
                EspesorElementoHost = EspesorElementoHost,
                espaciamientoRecorridoBarrasFoot = espaciamientoFoot,
                recorridoBarrar = recorridoBarrar,
                _nuevaLineaCantidadbarra = cantidadBarras,
                BarraTipo = RebarSeleccion.ObtenerTipoBArra_TipoRebar() // ObtenerBArraTipo()

            };
        }

        private TipoRebar ObtenerBArraTipo()
        {
            //BarraTipo.ELEV_BA_V
            switch (_editarBarraDTO.TipoCasobarra)
            {
                case TipoCasobarra.BarrasHorizontal:
                    return TipoRebar.ELEV_BA_H;
                case TipoCasobarra.BarraVertical:
                    return TipoRebar.ELEV_BA_V;
                case TipoCasobarra.BarraRefuerzoLosa:
                    return RebarSeleccion.ObtenerTipoBArra_TipoRebar(); 
                default:
                    return TipoRebar.NONE; ;
            }
        }

        #endregion



    }
}
