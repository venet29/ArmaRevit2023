using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.EditarPath.TiposSeleccion;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_Horquilla
    {

        public static void M1_CrearHorquilla(UI_Horquilla UI_Horquilla, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return ;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
  

            string tipoPosiicon = UI_Horquilla.BotonOprimido;
 
            if (tipoPosiicon == "btnDibujar")
            {
                UI_Horquilla.Hide();
                ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);

                ConfiguracionIniciaWPFlBarraVerticalDTO confiEnfierradoDTO = UI_Horquilla.ObtenerConfiguracionInicialBarraVerticalVDTO();
                ConfiguracionInicialBarraVerticalHorqDTO confiEnfierradoHorqDTO = UI_Horquilla.ObtenerConfiguracionInicialBarraVerticalHorqDTO();
                //configuracion barra verticales y horizontal  malla  y Horquilla 
                DireccionRecorrido _DireccionRecorridoHorqulla = new DireccionRecorrido(_uiapp.ActiveUIDocument.ActiveView, DireccionRecorrido_.ParaleloDerechaVista, 100);
                DireccionRecorrido _DireccionRecorridoBarra = new DireccionRecorrido(_uiapp.ActiveUIDocument.ActiveView, DireccionRecorrido_.PerpendicularEntradoVista, 100);

                ManejadorBarraV_Horq _ManejadorBarraH = new ManejadorBarraV_Horq(_uiapp, _seleccionarNivel, confiEnfierradoDTO,
                                                        _DireccionRecorridoHorqulla, _DireccionRecorridoBarra, confiEnfierradoHorqDTO);
                _ManejadorBarraH.CrearBArra();

                UI_Horquilla.Show();
            }

            UpdateGeneral.M2_CargarBArras(_uiapp);
                //CargarCambiarPathReinfomenConPto_Wpf
        }



 


    }
}