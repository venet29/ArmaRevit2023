using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;

namespace ArmaduraLosaRevit.Model.EstadoDesarrollo.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class MethodsBlancoNegro
    {


        public static void M1_BlancoNegro(UI_EstadoDesarrollo _ui, UIApplication _uiapp)
        {
            UtilStopWatch _utilStopWatch = new UtilStopWatch();
        
          
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);

            List<View> lista_view = new List<View>() { _uiapp.ActiveUIDocument.ActiveView };
            string BotonOprimido = _ui.BotonOprimido;
            //  EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);
            if(BotonOprimido.Contains("GuardarInfo"))
            {
                _ui.Hide();

                // 13-03-2023  borrar mas adelante
                ConfiguracionInicialParametros configuracionInicialview = new ConfiguracionInicialParametros(_uiapp);
                configuracionInicialview.AgregarParametrosShareView();

                ManejadorEstadoDesarrollo _ManejadorEstadoDesarrollo = _ui.ManejadorEstadoDesarrollo_;
                _ManejadorEstadoDesarrollo.GuardarDatosInternoVIewa();
                _ManejadorEstadoDesarrollo.Exportar();

                _ui.CargarTotales();

                _ui.Show();
            }     
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }






    }
}