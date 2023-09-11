#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.ViewportnNH.WPF
{

    public class ManejadorWPF_ViewPortNH
    {


        // ModelessForm instance
        private UI_ViewPOrtNH _mMyForm;
        private UIApplication _uiapp;
        private List<ViewSheetNH> listaViewPort;

        public ManejadorWPF_ViewPortNH(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;

        }

        public Result Execute()
        {
            try
            {
                var ubicacionVentana = new UbicacionVentana(_uiapp);
                ubicacionVentana.ObtenerMOnitor();
                ShowForm(_uiapp);
                _mMyForm.Left = ubicacionVentana.X + 500;
                _mMyForm.Top = ubicacionVentana.Y + 250;
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Result.Failed;
            }
        }

        public void ShowForm(UIApplication uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (_mMyForm != null && _mMyForm == null) return;
            //EXTERNAL EVENTS WITH ARGUMENTS
            EventHandlerWithStringArg evStr = new EventHandlerWithStringArg();
            EventHandlerWithWpfArg evWpf = new EventHandlerWithWpfArg();

            // The dialog becomes the owner responsible for disposing the objects given to it.
            _mMyForm = new UI_ViewPOrtNH(_uiapp, evStr, evWpf);
            // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }


    }
}