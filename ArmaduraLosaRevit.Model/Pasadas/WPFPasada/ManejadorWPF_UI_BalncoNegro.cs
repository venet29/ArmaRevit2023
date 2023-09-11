#region Namespaces

using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Pasadas.Servicio;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.UI;

#endregion


namespace ArmaduraLosaRevit.Model.Pasadas.WPFPasada
{

    public class ManejadorWPF_UI_Pasadas
    {


        // ModelessForm instance
        private UI_Pasadas _mMyForm;
        private UIApplication _UIapp;

        public ManejadorWPF_UI_Pasadas(UIApplication _uiapp)
        {
            _UIapp = _uiapp;

        }

        //ManejadorWPF_UI_Pasadas _ManejadorWPF_UI_Pasadas = new ManejadorWPF_UI_Pasadas(commandData.Application);
        //_ManejadorWPF_UI_Pasadas.Execute();
        public Result Execute()
        {
            try
            {

                ServicioActivarFamiliaPasada _ServicioActivarFamiliaPasada = new ServicioActivarFamiliaPasada(_UIapp);

                if (!_ServicioActivarFamiliaPasada.Ejecutar())
                {
                    Util.ErrorMsg("Error al cargar familias de 'PASADA_ROJA' Y PASADA_VERDE'.");
                    return Result.Failed;
                }

                ShowForm(_UIapp);
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
            _mMyForm = new UI_Pasadas(_UIapp, evStr, evWpf);
            // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();
        }

    }
}