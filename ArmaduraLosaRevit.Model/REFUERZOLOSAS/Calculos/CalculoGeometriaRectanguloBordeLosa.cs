using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{
    public class CalculoGeometriaRectanguloBordeLosa
    {
        private UIApplication _uiapp;
        private SeleccionarBordeLosa _seleccionarBordeLosa;
        private DatosBordeLosaDTO _datosBordeLosaDTO;

        public List<XYZ> ListaBorde1 { get; set; }
        public List<XYZ> ListaBorde2 { get; set; }
        private XYZ DireccionHaciaLosa;

#pragma warning disable CS0169 // The field 'CalculoGeometriaRectanguloBordeLosa._ptoSobreBorde' is never used
         private XYZ _ptoSobreBorde;
#pragma warning restore CS0169 // The field 'CalculoGeometriaRectanguloBordeLosa._ptoSobreBorde' is never used

        private XYZ _p1;
        private XYZ _p2;
        private XYZ _PtoMouseEspejo1;
        private XYZ _PtoMouseEspejo2;

        public CalculoGeometriaRectanguloBordeLosa(UIApplication uiapp, SeleccionarBordeLosa seleccionarBordeLosa, DatosBordeLosaDTO _DatosBordeLosaDTO)
        {
            this._uiapp = uiapp;
            this._seleccionarBordeLosa = seleccionarBordeLosa;
            this._datosBordeLosaDTO = _DatosBordeLosaDTO;
            ListaBorde1 = new List<XYZ>();
            ListaBorde2 = new List<XYZ>();
        }

        public void Ejecutar()
        {
            //DireccionHaciaLosa = _seleccionarLosaConMouse.DireccionHaciaLosa;
           // ObtenerPtoSobreBorde();
            GenerarListaConBordesDelRectangulo();
            PtoMouseSobreMuroFalso();
        }


        //private void ObtenerPtoSobreBorde()
        //{
        //    _ptoSobreBorde = _seleccionarLosaConMouse._curvaBordeLosa.Project(_seleccionarLosaConMouse._ptoSeleccionEnLosa).XYZPoint;
        //    DireccionHaciaLosa = (_seleccionarLosaConMouse._ptoSeleccionEnLosa - _ptoSobreBorde).GetXY0().Normalize();
        //}

        private void GenerarListaConBordesDelRectangulo()
        {
            DireccionHaciaLosa = _datosBordeLosaDTO.DireccionHaciaLosa;
            _p1 = _datosBordeLosaDTO.ptoInicial;
            _p2 = _datosBordeLosaDTO.ptoFinal;

            ListaBorde1.Add(_p1 + DireccionHaciaLosa * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL);
            ListaBorde1.Add(_p1 + DireccionHaciaLosa * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL + DireccionHaciaLosa * ConstNH.ESPACIA_BARRA_REFUERZO_BORDELOSA);

            ListaBorde2.Add(_p2 + DireccionHaciaLosa * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL);
            ListaBorde2.Add(_p2 + DireccionHaciaLosa * ConstNH.RECUBRIMIENTO_BORDE_LOSA_LATERAL + DireccionHaciaLosa * ConstNH.ESPACIA_BARRA_REFUERZO_BORDELOSA);
        }

        private void PtoMouseSobreMuroFalso()
        {
            _PtoMouseEspejo1 = (ListaBorde1[0] + ListaBorde1[1]) / 2 + (_p1 - _p2).Normalize() * 1;
            _PtoMouseEspejo2 = (ListaBorde2[0] + ListaBorde2[1]) / 2 + (_p2 - _p1).Normalize() * 1;
        }

        internal SeleccinarMuroRefuerzo ObtenerMuroMirror1()
        {

            SeleccinarMuroRefuerzo seleccinarMuroRefuerzoMirror = new SeleccinarMuroRefuerzo(_uiapp);
            seleccinarMuroRefuerzoMirror.ElementoSeleccionado = _seleccionarBordeLosa._FloorSeleccion;
            seleccinarMuroRefuerzoMirror.TipoElemto = TipoElemento.losa;
            seleccinarMuroRefuerzoMirror.pto1SeleccionadoConMouse = _PtoMouseEspejo1;
            seleccinarMuroRefuerzoMirror.ListaPtosBordeMuroIntersectado = ListaBorde1;
            seleccinarMuroRefuerzoMirror.DireccionEnfierrado = DireccionHaciaLosa;

            return seleccinarMuroRefuerzoMirror;
        }

        internal SeleccinarMuroRefuerzo ObtenerMuroMirror2()
        {
            SeleccinarMuroRefuerzo seleccinarMuroRefuerzoMirror = new SeleccinarMuroRefuerzo(_uiapp);
            seleccinarMuroRefuerzoMirror.ElementoSeleccionado = _seleccionarBordeLosa._FloorSeleccion;
            seleccinarMuroRefuerzoMirror.TipoElemto = TipoElemento.losa;
            seleccinarMuroRefuerzoMirror.pto1SeleccionadoConMouse = _PtoMouseEspejo2;
            seleccinarMuroRefuerzoMirror.ListaPtosBordeMuroIntersectado = ListaBorde2;
            seleccinarMuroRefuerzoMirror.DireccionEnfierrado = DireccionHaciaLosa;

            return seleccinarMuroRefuerzoMirror;
        }
    }
}

