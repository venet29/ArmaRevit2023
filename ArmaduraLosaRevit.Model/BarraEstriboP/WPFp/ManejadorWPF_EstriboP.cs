#region Namespaces

using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.BarraEstriboP.WPFp
{

    public class ManejadorWPF_EstriboP
    {
      
        // ModelessForm instance
        private Ui_EstriboP _mMyForm;
        private UIApplication _uiapp;

        public ManejadorWPF_EstriboP(ExternalCommandData commandData)
        {
            _uiapp = commandData.Application;
        }

        public  Result Execute()
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
            _mMyForm = new Ui_EstriboP(uiapp, evStr, evWpf);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }

    }
}