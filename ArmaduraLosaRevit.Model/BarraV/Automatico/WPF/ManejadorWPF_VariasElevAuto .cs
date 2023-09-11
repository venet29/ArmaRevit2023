#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using System.Linq;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ConfiguracionInicial.Elemento;
using ArmaduraLosaRevit.Model.UTILES;

#endregion


namespace ArmaduraLosaRevit.Model.BarraV.Automatico.WPF
{

    public class ManejadorWPF_VariasElevAuto
    {

        public bool IsOk { get; internal set; }
        // ModelessForm instance
        private UI_VariasElevAuto _mMyForm;
        private UIApplication _uiapp;
        private readonly List<ElevacionVAriosDto> listaElevacionVAriosDto;
        private readonly Ui_AutoElev ui_Barrav;
        private readonly string mpathDirecotrio;

        public ManejadorWPF_VariasElevAuto(UIApplication _uiapp, List<ElevacionVAriosDto> ListaElevacionVAriosDto,  Ui_AutoElev ui_Barrav, string mpathDirecotrio)
        {
            this._uiapp = _uiapp;
            listaElevacionVAriosDto = ListaElevacionVAriosDto;
            this.ui_Barrav = ui_Barrav;
            this.mpathDirecotrio = mpathDirecotrio;
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



            var listaTipo=listaElevacionVAriosDto.Select(c => c.TipoView).Distinct().Where(c=>c!="").ToList();

            // The dialog becomes the owner responsible for disposing the objects given to it.
            _mMyForm = new UI_VariasElevAuto(_uiapp, evStr, evWpf, listaElevacionVAriosDto, ui_Barrav, listaTipo, mpathDirecotrio);
           // _mMyForm.dtTipo.SelectedItem = TipoBarraTraslapoDereArriba.f1;
            _mMyForm.Show();

          
        }

    }
}