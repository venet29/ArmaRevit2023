#region Namespaces

using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.EditarTipoPath.WPF
{

    public class ManejadorWPF 
    {
        private readonly SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto;
        private readonly TabEditarPath tabEditarPath;

        // ModelessForm instance
        private Ui _mMyForm;
        private UIApplication _UIapp;

        public ManejadorWPF(ExternalCommandData commandData, SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto, TabEditarPath tabEditarPath)
        {
            _UIapp = commandData.Application;
            this.seleccionarPathReinfomentConPto = seleccionarPathReinfomentConPto;
            this.tabEditarPath = tabEditarPath;
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

        public void ShowForm(UIApplication uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (_mMyForm != null && _mMyForm == null) return;
            //EXTERNAL EVENTS WITH ARGUMENTS
            EventHandlerWithStringArg evStr = new EventHandlerWithStringArg();
            EventHandlerWithWpfArg evWpf = new EventHandlerWithWpfArg();

            // The dialog becomes the owner responsible for disposing the objects given to it.
            _mMyForm = new Ui(uiapp, evStr, evWpf, seleccionarPathReinfomentConPto, tabEditarPath);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }

    }
}