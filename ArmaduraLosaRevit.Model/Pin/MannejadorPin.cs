using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Pin
{

    public class ManejadorPin
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _View;
        private SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad;
        private SeleccionarRebarElemento _SeleccionarRebarElemento;
        private SeleccionarRebarVisibilidad _seleccionarRebarVisibilidad;

        public ManejadorPin(UIApplication _uiapp, View _view)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._View = _view;
            this._SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _View);
            this._SeleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _View);            
            this._seleccionarRebarVisibilidad = new SeleccionarRebarVisibilidad(_uiapp, _View);
        }
        public ManejadorPin(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;

        }
        public bool EjecutarParaBArras(bool IsPinner)
        {
            try
            {
                //if (!_SelecPathReinVisibilidad.M1_ejecutar()) return false;
                //if (!_seleccionarRebarVisibilidad.M1_Ejecutar()) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("MostrarPorDiametro-NH");
                    EjecutarSinTran(IsPinner);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        public bool EjecutarSinTran(bool IsPinner)
        {
            if (!_SelecPathReinVisibilidad.M1_ejecutar()) return false;
            if (!_seleccionarRebarVisibilidad.M1_Ejecutar_ConrebarSystem()) return false;



            PinnearLista(_SelecPathReinVisibilidad._lista_A_DePathReinfNivelActual, IsPinner);
            PinnearLista(_SelecPathReinVisibilidad._lista_B_DePathSymbolNivelActual.Select(c => c.element).ToList(), IsPinner);
            PinnearLista(_SelecPathReinVisibilidad._lista_C_DePathTagNivelActual.Select(c => c.element).ToList(), IsPinner);

            PinnearLista(_SelecPathReinVisibilidad._lista_A_DeRebar, IsPinner);


            //rebar
            PinnearLista(_seleccionarRebarVisibilidad._lista_A_TodasRebarNivelActual_MENOSRebarSystem, IsPinner);
            PinnearLista(_seleccionarRebarVisibilidad._lista_C_DeRebarTagNivelActual.Select(c => c.element).ToList(), IsPinner);

            return false;
        }


        public bool EjecutarParaLIstaElemeto(bool IsPinner, List<Element> listaElem)
        {
            try
            {
                //if (!_SelecPathReinVisibilidad.M1_ejecutar()) return false;
                //if (!_seleccionarRebarVisibilidad.M1_Ejecutar()) return false;

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("MostrarPorDiametro-NH");
                    PinnearLista(listaElem,IsPinner);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        public void PinnearLista(List<Element> listaElem, bool IsPinner)
        {
            if (listaElem == null) return;
            if (listaElem.Count == 0) return;
            foreach (Element item in listaElem)
            {
                if (!item.IsValidObject) continue;
                if (item.Pinned == IsPinner) continue;
                item.Pinned = IsPinner;
            }
        }
    }
}
