#region Namespaces

using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.BarraV.Copiar.WPF
{

    public class ManejadorWPF_CopiarRebar
    {

        public bool IsOk { get; internal set; }
        // ModelessForm instance
        private UI_CopiarRebarElev _mMyForm;
        private UIApplication _UIapp;
        private readonly List<EnvoltoriLevel> listaLevel;

        public ManejadorWPF_CopiarRebar(UIApplication _uiapp, List<EnvoltoriLevel> listaLevel)
        {
            _UIapp = _uiapp;
            this.listaLevel = listaLevel;
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



            // The dialog becomes the owner responsible for disposing the objects given to it.
            _mMyForm = new UI_CopiarRebarElev(_UIapp, evStr, evWpf, listaLevel);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();

          
        }

    }
}