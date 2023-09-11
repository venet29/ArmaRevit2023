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

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.WPFp_suple
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    public class Methods_barraSuple
    {
        public static void M1_EjecutarRutinas(Ui_barraSuple _ui_barraSuple, UIApplication uiapp)
        {
            try
            {
                string tipoPosiicon = _ui_barraSuple.ImageOprimido;

                if (BuscarIsNombreViewActualizado.IsError(uiapp.ActiveUIDocument.ActiveView)) return;

                _ui_barraSuple.Hide();

                int diamtroBarraSupleMM = Util.ConvertirStringInInteger(_ui_barraSuple.dtDiaSUple.Text);
                double EspaciemientoCm = Util.ConvertirStringInInteger(_ui_barraSuple.espaSuple.Text);
                UbicacionLosa ubicacion = VariablesSistemas.m_ubicacionBarra;
                CargarBarraRoom.Cargar(uiapp, tipoPosiicon, ubicacion, diamtroBarraSupleMM, EspaciemientoCm);

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