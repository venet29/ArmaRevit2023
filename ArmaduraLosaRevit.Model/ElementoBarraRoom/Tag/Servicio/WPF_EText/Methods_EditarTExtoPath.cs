using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.EditarTipoPath;
using ArmaduraLosaRevit.Model.WPF;
using System.Runtime.InteropServices.WindowsRuntime;
using ArmaduraLosaRevit.Model.RebarLosa;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Servicio.WPF_EText
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    public class Methods_EditarTExtoPath
    {
        public static void M1_EjecutarRutinas(Ui_EditarTExtoPath _ui_barraSuple, UIApplication _uiapp)
        {
            try
            {
                string tipoPosiicon = _ui_barraSuple.ImageOprimido;

                if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;

                _ui_barraSuple.Hide();

                if (tipoPosiicon == "Intercambio_F_L")
                {
                    ManejadorIntercambiar_F_L _ManejadorIntercambiar_F_L = new ManejadorIntercambiar_F_L(_uiapp);
                    _ManejadorIntercambiar_F_L.Ejecutar();
                }
                else if (tipoPosiicon == "ResetText")
                {
                    ManejadorResetText _ManejadorResetText = new ManejadorResetText(_uiapp);
                    _ManejadorResetText.Ejecutar();
                }
                _ui_barraSuple.Show();
            }
            catch (Exception)
            {

                _ui_barraSuple.Show();
                _ui_barraSuple.Close();
            }
        }


    }
}