using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Seleccionar;
using formNH = System.Windows.Forms;
using System.Linq;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.Pin.WPF_pin
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    public class Methods_pin
    {
        private static bool IsNivelActual;
        private static bool IsPinner;
        private static tipoVIewNh tipoView;
        private static Document _doc;
        private static List<View> listaView;

        public static void M1_EjecutarRutinas(Ui_pin _ui_barraSuple, UIApplication _uiapp)
        {
            try
            {

                if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;

                IsNivelActual = _ui_barraSuple.IsNivelActual;
                IsPinner = _ui_barraSuple.IsPinner;
                tipoView = _ui_barraSuple.viewModificados;
                _doc = _uiapp.ActiveUIDocument.Document;
                if (!ObtenerNiveles(_uiapp, tipoView)) return;

                _ui_barraSuple.Hide();
                foreach (var item_view in listaView)
                {
                    if (_uiapp.ActiveUIDocument.ActiveView.Name != item_view.Name)
                    {
                        _uiapp.ActiveUIDocument.ActiveView = item_view;
                    }

                    ManejadorPin _ManejadorPin = new ManejadorPin(_uiapp, item_view);
                    _ManejadorPin.EjecutarParaBArras(IsPinner);
                }


                if (IsNivelActual)
                    Util.InfoMsg($"Proceso Terminado. Vista Actual con Pin :{(IsPinner?"ON":"OFF")}");
                else
                    Util.InfoMsg($"Proceso Terminado. { listaView.Count} Vistas con Pin :{(IsPinner ? "ON" : "OFF")}");

                _ui_barraSuple.Show();
            }
            catch (Exception)
            {

                _ui_barraSuple.Show();
                _ui_barraSuple.Close();
            }
        }



        private static bool ObtenerNiveles(UIApplication _uiapp, tipoVIewNh tipoView)
        {
            listaView = new List<View>();

            if (!IsNivelActual)
            {
                listaView = SeleccionarView.ObtenerView_losa_elev_fund(_uiapp.ActiveUIDocument);

                if (tipoView != tipoVIewNh.Todas_las_View)
                    listaView = listaView.Where(c => c.ViewType == (tipoView == tipoVIewNh.Solo_Elevaciones
                                                                             ? ViewType.Section
                                                                             : ViewType.FloorPlan))
                                         .ToList();


                formNH.DialogResult result = formNH.MessageBox.Show($"Seguro que desea ejecutar { listaView.Count} View. Proceso demora unos minutos.", "Salir", formNH.MessageBoxButtons.YesNoCancel);

                if (result != formNH.DialogResult.Yes)
                {
                    listaView.Clear();
                    return false;
                }
            }
            else
            {
                listaView.Add(_uiapp.ActiveUIDocument.ActiveView);
            }
            return true;
        }

    }
}