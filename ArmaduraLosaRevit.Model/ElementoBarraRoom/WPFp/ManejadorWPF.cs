#region Namespaces

using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.WPFp
{

    public class ManejadorWPF 
    {
      
        // ModelessForm instance
        private Ui_barra _mMyForm;
        private UIApplication _UIapp;

        public ManejadorWPF(ExternalCommandData commandData)
        {
            _UIapp = commandData.Application;
        }

        public  Result Execute()
        {
            try
            {
                var ubicacionVentana = new UbicacionVentana(_UIapp);
                ubicacionVentana.ObtenerMOnitor();
                ShowForm(_UIapp);
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

        public void ShowForm(UIApplication _uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (_mMyForm != null && _mMyForm == null) return;
            //EXTERNAL EVENTS WITH ARGUMENTS
            EventHandlerWithStringArg evStr = new EventHandlerWithStringArg();
            EventHandlerWithWpfArg evWpf = new EventHandlerWithWpfArg();

            ResetearColorCategoria _ResetearColorCategoria = new ResetearColorCategoria(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
            _ResetearColorCategoria.ResetearLineaBArrasMagenta();

            // The dialog becomes the owner responsible for disposing the objects given to it.
            _mMyForm = new Ui_barra(_uiapp, evStr, evWpf);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }

    }
}