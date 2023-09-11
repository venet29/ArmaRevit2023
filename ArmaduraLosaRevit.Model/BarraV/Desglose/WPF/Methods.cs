using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods
    {


        public static void M1_Desglose(UI_desglose _ui, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            string tipoPosiicon = _ui.BotonOprimido;
            EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);
            if (tipoPosiicon == "btnGenerar_Elev")
            {
                char ch = char.Parse(_ui.dtNombre.Text);

                if (!char.IsLetter(ch))
                {
                    Util.ErrorMsg("Lnombre de grupo debe ser un letra");
                    UpdateGeneral.M2_CargarBArras(_uiapp);
                    return;
                }


                _ui.Hide();

                Config_EspecialElev _Config_EspecialElv = _ui.ObtenerConfiguraEspecialElev();
                if (_Config_EspecialElv.TipoCasoAnalisis == Ayuda.CasoAnalisas.AnalsisVertical)
                {
                    ManejadorDesgloseV _ManejadorDesglose = new ManejadorDesgloseV(_uiapp, _ui);
                    _ManejadorDesglose.EjecutarDibujarBarrasEnElevacionV(_Config_EspecialElv);
                }
                else
                {
                    ManejadorDesgloseH _ManejadorDesglose = new ManejadorDesgloseH(_uiapp, _ui);
                    _ManejadorDesglose.EjecutarDibujarBarrasEnElevacionH(_Config_EspecialElv);
                }
                _ui.Show();
            }

            else if (tipoPosiicon == "GenCorteV")
            {
                _ui.Hide();


                Config_EspecialCorte _Config_EspecialCorte = _ui.ObtenerConfiguraEspecialCOrte();

                if (_Config_EspecialCorte.TipoCasoAnalisis == Ayuda.CasoAnalisas.AnalsisVertical)
                {
                    ManejadorDesgloseV _ManejadorDesglose = new ManejadorDesgloseV(_uiapp, _ui);
                    _ManejadorDesglose.EjecutarDibujarBarrasEncorteV(_Config_EspecialCorte);
                }
                else
                {
                    ManejadorDesgloseH _ManejadorDesglose = new ManejadorDesgloseH(_uiapp, _ui);
                    _ManejadorDesglose.EjecutarDibujarBarrasEncorteH(_Config_EspecialCorte);
                }
                _ui.Show();
            }

            else if (tipoPosiicon == "Bton_config")
            {
                _ui.Hide();

                ManejadorConfiguracionDesglose.cargar(_uiapp, true);
                _ui.Show();
            }


            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }






    }
}