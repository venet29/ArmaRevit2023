using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ParametrosShare.Actualizar
{
    public class ManejadorActualizar
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorActualizar(UIApplication _uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
        }


        public void Ejecutar_ActualizarNUmeroBArraTOdasLAsvistas()
        {
           // View _view = _uiapp.ActiveUIDocument.ActiveView;
            try
            {

                var lista= SeleccionarView.ObtenerView_losa_elev_fund(_uiapp.ActiveUIDocument).Where(X=>X.ViewType == ViewType.FloorPlan).ToList();

                for (int i = 0; i < lista.Count; i++)
                {
                    Ejecutar_ActualizarNUmeroBArra(lista[i],false);
                }
         
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");

        }

        public void Ejecutar_ActualizarNUmeroBArra(View _viewIni=null,bool ISMensaje=true)
        {
            View _view = default;
            if (_viewIni == null)
                _view = _uiapp.ActiveUIDocument.ActiveView;
            else
                _view = _viewIni;
            try
            {
                SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view);
                _SelecPathReinVisibilidad.M1_ejecutar();

                ActualizarnNumeroBarra _ActualizarBarraTipo = new ActualizarnNumeroBarra(_uiapp, _view);
                _ActualizarBarraTipo.Ejecutar(_SelecPathReinVisibilidad._lista_A_DePathReinfNivelActual);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           if(ISMensaje) Util.InfoMsg($"Proceso Terminado. ");

        }


        public void Ejecutar_ActualizarLargoFundaciones()
        {
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            try
            {
                SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
                _SelecPathReinVisibilidad.M1_ejecutar();

                ActualizarLargoBarras _ActualizarBarraTipo = new ActualizarLargoBarras(_uiapp, _view);
                _ActualizarBarraTipo.Ejecutar(_SelecPathReinVisibilidad._lista_A_DePathReinfNivelActual);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");

        }


    }
}
