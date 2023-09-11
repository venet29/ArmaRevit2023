#region Namespaces

using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.LosaEstructural.WPFp
{

    public class ManejadorWPF_pelotaLosa 
    {
      
        // ModelessForm instance
        private Ui_pelotaLosa _mMyForm;
        private UIApplication _UIapp;

        public ManejadorWPF_pelotaLosa(ExternalCommandData commandData)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
             
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
            _mMyForm = new Ui_pelotaLosa(uiapp, evStr, evWpf);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }

    }
}