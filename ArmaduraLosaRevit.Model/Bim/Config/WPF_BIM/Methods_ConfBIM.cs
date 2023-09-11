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
using ArmaduraLosaRevit.Model.Bim.BimWorkSet.WPF_WorkSet;

namespace ArmaduraLosaRevit.Model.Bim.Config.WPF_BIM
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_ConfBIM
    {

        public static void M1_CrearConfBim(UI_ConfBIM uI_CopiaLocal, UIApplication _uiapp)
        {

            //if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            string tipoPosiicon = uI_CopiaLocal.BotonOprimido;
          
            if (tipoPosiicon == "CrearWorkSet")
            {
                ManejadorWPF_WorkSetNH _ManejadorWPF_WorkSetNH = new ManejadorWPF_WorkSetNH(_uiapp);
                _ManejadorWPF_WorkSetNH.Execute();
            }

            uI_CopiaLocal.Close();
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }






    }
}