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
using ArmaduraLosaRevit.Model.GRIDS.AgregarEje;

namespace ArmaduraLosaRevit.Model.GRIDS.WPF_AgregraEJE
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_AgregarEje
    {

        public static void M1_CrearAgregarEje(UI_AgregarEjes UI_Horquilla, UIApplication _uiapp)
        {

            if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return ;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
  

            string tipoPosiicon = UI_Horquilla.BotonOprimido;
            string textoGrid = UI_Horquilla.Textogrid.Trim();

            if (tipoPosiicon == "btnDibujar")
            {
                UI_Horquilla.Hide();


                ManejadorAgregarEjes _ManejadorAgregarEjes = new ManejadorAgregarEjes(_uiapp);
                if (_ManejadorAgregarEjes.Ejecutar(textoGrid))
                {

                }

                UI_Horquilla.Show();
            }

            UpdateGeneral.M2_CargarBArras(_uiapp);
                //CargarCambiarPathReinfomenConPto_Wpf
        }



 


    }
}