#region Namespaces

using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ParametrosShare.Actualizar.TipoBArra;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.ViewFilter.WPF
{

    public class ManejadorWPF_Filtro3D 
    {
      
        // ModelessForm instance
        private Ui_Filtro3D _mMyForm;
        private UIApplication _uiapp;
        private UbicacionVentana ubicacionVentana;

        public ManejadorWPF_Filtro3D(UIApplication uiapp)
        {
            this._uiapp = uiapp;
        }

        public  Result Execute()
        {
            try
            {
                ubicacionVentana = new UbicacionVentana(_uiapp);
                ubicacionVentana.ObtenerMOnitor();
                ShowForm(_uiapp);
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

            ManejadorActualizarTIpoBarraPorOtro.Ejecutar(uiapp);
            // The dialog becomes the owner responsible for disposing the objects given to it.
            _mMyForm = new Ui_Filtro3D(uiapp, evStr, evWpf);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }

    }
}