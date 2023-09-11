using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;

namespace ArmaduraLosaRevit.Model.EditarPath.Seleccion
{
    public class SelecionarPathSeleccionadoParteSuperior
    {
        private readonly UIApplication _uiapp;
        private readonly PathReinSpanSymbol _PathReinSpanSymbol;
        private SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto;

        public SelecionarPathSeleccionadoParteSuperior(UIApplication uiapp ,PathReinSpanSymbol _PathReinSpanSymbol)
        {
            this._uiapp = uiapp;
            this._PathReinSpanSymbol = _PathReinSpanSymbol;
        }

        public void Ejecutar()
        {
            if (_PathReinSpanSymbol == null)
            {
                Util.ErrorMsg("Error en'SelecionarPathSeleccionadoParteSuperior' -> _PathReinSpanSymbol==null ");
                return;
            }

            if(!Obtener_SeleccionarPathReinfomentConPto()) return;

            AsignarPtoSuperiorMouse();
            List<ObjectSnapTypes> listaSnap = new List<ObjectSnapTypes>() { ObjectSnapTypes.Points,ObjectSnapTypes.Nearest};

            EditarPathReinMouse_ExtederPathA2punto EditarPathReinMouse_ExtederPathA2punto = new EditarPathReinMouse_ExtederPathA2punto(_uiapp, listaSnap);
            EditarPathReinMouse_ExtederPathA2punto.M1_ExtederPathApunto(_seleccionarPathReinfomentConPto);

        }

        private bool Obtener_SeleccionarPathReinfomentConPto()
        {
            _seleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(_uiapp);

            if (!_seleccionarPathReinfomentConPto.AsignarPathReinformentSymbol_sinSelecRoom(_PathReinSpanSymbol))
            {
                //Util.ErrorMsg("Error al obtener parametros del PathReinforment previamente selecionado");
                _seleccionarPathReinfomentConPto = null;
                return false;
            }

            if (_seleccionarPathReinfomentConPto.PathReinforcement == null) _seleccionarPathReinfomentConPto = null;

            return (_seleccionarPathReinfomentConPto==null? false:true);
        }

        private bool AsignarPtoSuperiorMouse()
        {
            CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_seleccionarPathReinfomentConPto.PathReinforcement, _uiapp.ActiveUIDocument.Document);
            CoordenadaPath _CoordenadaPath= pathReinformeCalculos.Calcular4PtosPathReinf();
            XYZ ptoSuperior = (_CoordenadaPath.p1 + _CoordenadaPath.p4) / 2;
            XYZ direccionncentro = (_CoordenadaPath.centro - ptoSuperior).Normalize();

            _seleccionarPathReinfomentConPto.PuntoSeleccionMouse= (_CoordenadaPath.p1 + _CoordenadaPath.p4)/2 + direccionncentro*0.5; 

            return true;
        }
    }
}
