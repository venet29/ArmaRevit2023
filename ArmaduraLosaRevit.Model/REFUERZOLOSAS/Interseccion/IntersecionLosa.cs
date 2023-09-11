using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Buscar;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Interseccion
{
    public class IntersecionLosa
    {
        private readonly UIApplication _uiapp;
        private View3D _view3D;
        private readonly EmpotramientoPatasLosaDTO _empotramientoPatasDTO;
#pragma warning disable CS0169 // The field 'IntersecionLosa.view3D' is never used
        private readonly View3D view3D;
#pragma warning restore CS0169 // The field 'IntersecionLosa.view3D' is never used
        public XYZ _ptoini { get; set; }
        public XYZ _pto1NevelAntesfinal { get; set; }
        public XYZ _ptofinal { get; set; }

        private XYZ _ptoMedio;
        public XYZ _direccionBarra;


        public TipoBarraRefuerzo ResulttTpobarraDerecha;
        public TipoBarraRefuerzo ResultTipobarraIzquierda;
        private BuscarLosaShaft _buscarElementosDerechaSup;
        private BuscarLosaShaft _buscarElementosIzqBajo;
#pragma warning disable CS0649 // Field 'IntersecionLosa._diametroMM' is never assigned to, and will always have its default value 0
        private int _diametroMM;
#pragma warning restore CS0649 // Field 'IntersecionLosa._diametroMM' is never assigned to, and will always have its default value 0

        public TipoBarraRefuerzo ResultTipoBarraRef { get; set; }

        public IntersecionLosa(UIApplication uiapp, View3D _view3D, EmpotramientoPatasLosaDTO _empotramientoPatasDTO, XYZ p1, XYZ p2)
        {
            this._uiapp = uiapp;
            this._view3D = _view3D;
            this._empotramientoPatasDTO = _empotramientoPatasDTO;
            this._ptoini = p1;
            this._ptofinal = p2;
            this._ptoMedio = (p2 + p1) / 2;

        }

        public void BuscarInterseccion()
        {
            _direccionBarra = (_ptofinal - _ptoini).Normalize();

            ResulttTpobarraDerecha = TipoBarraRefuerzo.BarraRefSinPatas;
            ResultTipobarraIzquierda = TipoBarraRefuerzo.BarraRefSinPatas;
            ResultTipoBarraRef = TipoBarraRefuerzo.BarraRefSinPatas;

            M1_BuscarIntersecionHaciaDerechaSuperior();
            M2_BuscarIntersecionHaciaIzquierdaInferior();
            M3_ObternerResultTipoBarraRef();
        }

        private void M2_BuscarIntersecionHaciaIzquierdaInferior()
        {
            if (_empotramientoPatasDTO.TipoPataIzq == TipoBarraRefuerzo.BarraRefPataInicial ||
             _empotramientoPatasDTO.TipoPataIzq == TipoBarraRefuerzo.BarraRefPataAmbos)
            {
                PataAlInicioIzqInf();
                return;
            }
            double LargoBusuqeda = _ptoini.DistanceTo(_ptoMedio);
            _buscarElementosIzqBajo = new BuscarLosaShaft(_uiapp, LargoBusuqeda, _view3D);
            _buscarElementosIzqBajo.OBtenerRefrenciaLosaSahft(_ptoMedio, -_direccionBarra);

            if (_buscarElementosIzqBajo.ListaShaftLosaEncontrado.Count == 0 || _empotramientoPatasDTO.TipoPataDere == TipoBarraRefuerzo.NoBuscar)
            {
                M1_0_LadoLibreIzqInf();
                return;
            }
            else
            {
                PataAlInicioIzqInf();
            }
        }

        private void M1_0_LadoLibreIzqInf()
        {
            _ptoini = _ptoini - _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_diametroMM) * _empotramientoPatasDTO.factorLargoIni;
            ResultTipobarraIzquierda = TipoBarraRefuerzo.BarraRefSinPatas;
        }
        private void PataAlInicioIzqInf()
        {
            if (M1_1_1_IntersectaShafIzqInf()) return;
            if (M1_1_2_IntersectaLosaIzqInf()) return;

            M1_0_LadoLibreIzqInf();
        }
        private bool M1_1_1_IntersectaShafIzqInf()
        {
            var losaMaxDistancia = _buscarElementosIzqBajo.ListaShaftLosaEncontrado.MinBy(c => c.distancia);
            if (losaMaxDistancia._TipoElementoBArraV != TipoElementoBArraV.shaft) return false;
            _ptoini = losaMaxDistancia._PtoSObreFaceLosaShaft + _direccionBarra * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL;
            ResultTipobarraIzquierda = TipoBarraRefuerzo.BarraRefPataInicial;
            return true;
        }
        private bool M1_1_2_IntersectaLosaIzqInf()
        {


            var losaMaxDistancia = _buscarElementosIzqBajo.ListaShaftLosaEncontrado.MinBy(c => c.distancia);
            if (losaMaxDistancia._TipoElementoBArraV != TipoElementoBArraV.losa) return false;
            _ptoini = losaMaxDistancia._PtoSObreFaceLosaShaft + _direccionBarra * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL;
            ResultTipobarraIzquierda = TipoBarraRefuerzo.BarraRefPataInicial;
            return true;
        }



        #region 2) metodos Derecha Superior

        private void M1_BuscarIntersecionHaciaDerechaSuperior()
        {
            if (_empotramientoPatasDTO.TipoPataDere == TipoBarraRefuerzo.BarraRefPataFinal ||
                _empotramientoPatasDTO.TipoPataDere == TipoBarraRefuerzo.BarraRefPataAmbos)
            {
                M1_1_pataAlfinalDerSup();
                return;
            }
            double LargoBusuqeda = _ptofinal.DistanceTo(_ptoMedio);
            _buscarElementosDerechaSup = new BuscarLosaShaft(_uiapp, LargoBusuqeda, _view3D);
            _buscarElementosDerechaSup.OBtenerRefrenciaLosaSahft(_ptoMedio, _direccionBarra);

            if (_buscarElementosDerechaSup.ListaShaftLosaEncontrado.Count == 0 || _empotramientoPatasDTO.TipoPataDere == TipoBarraRefuerzo.NoBuscar)
            {
                M1_0_LadoLibreDereSup();
                return;
            }
            else
            {
                M1_1_pataAlfinalDerSup();
            }

        }

        private void M1_0_LadoLibreDereSup()
        {
            _ptofinal = _ptofinal + _direccionBarra * UtilBarras.largo_L9_DesarrolloFoot_diamMM(_diametroMM) * _empotramientoPatasDTO.factorLargoFin; ;
            ResulttTpobarraDerecha = TipoBarraRefuerzo.BarraRefSinPatas;
        }

        private void M1_1_pataAlfinalDerSup()
        {
            if (M1_1_1_IntersectaShaf()) return;
            if (M1_1_2_IntersectaLosa()) return;
            M1_0_LadoLibreDereSup();
        }

        private bool M1_1_1_IntersectaShaf()
        {
            var losaMaxDistancia = _buscarElementosDerechaSup.ListaShaftLosaEncontrado.MinBy(c => c.distancia);
            if (losaMaxDistancia._TipoElementoBArraV != TipoElementoBArraV.shaft) return false;
            _ptofinal = losaMaxDistancia._PtoSObreFaceLosaShaft + -_direccionBarra * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL;
            ResulttTpobarraDerecha = TipoBarraRefuerzo.BarraRefPataFinal;
            return true;
        }
        private bool M1_1_2_IntersectaLosa()
        {


            var losaMaxDistancia = _buscarElementosDerechaSup.ListaShaftLosaEncontrado.MinBy(c => c.distancia);
            if (losaMaxDistancia._TipoElementoBArraV != TipoElementoBArraV.losa) return false;

            var listaOrdenada = _buscarElementosDerechaSup.ListaShaftLosaEncontrado.OrderByDescending(c => c.distancia).ToList();

            if (listaOrdenada.Count > 1)
            {
                double distaUltimo = listaOrdenada.Last().distancia;
                if (listaOrdenada[listaOrdenada.Count - 2]._TipoElementoBArraV != TipoElementoBArraV.losa) return false;
                double distaPenUltimo = listaOrdenada[listaOrdenada.Count - 2].distancia;
                if (Util.IsEqual(distaUltimo, distaPenUltimo, Util.CmToFoot(2))) return false;
            }

            _ptofinal = losaMaxDistancia._PtoSObreFaceLosaShaft + -_direccionBarra * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL;
            ResulttTpobarraDerecha = TipoBarraRefuerzo.BarraRefPataFinal;
            return true;
        }


        #endregion



        private void M3_ObternerResultTipoBarraRef()
        {
            if (ResulttTpobarraDerecha == TipoBarraRefuerzo.BarraRefPataFinal &&
                ResultTipobarraIzquierda == TipoBarraRefuerzo.BarraRefPataInicial)
            {
                ResultTipoBarraRef = TipoBarraRefuerzo.BarraRefPataAmbos;
            }
            else if (ResulttTpobarraDerecha == TipoBarraRefuerzo.BarraRefPataFinal)
            {
                ResultTipoBarraRef = TipoBarraRefuerzo.BarraRefPataFinal;
            }
            else if (ResultTipobarraIzquierda == TipoBarraRefuerzo.BarraRefPataInicial)
            {
                ResultTipoBarraRef = TipoBarraRefuerzo.BarraRefPataInicial;
            }
            else
            { ResultTipoBarraRef = TipoBarraRefuerzo.BarraRefSinPatas; }
        }
    }
}
