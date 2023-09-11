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
using ArmaduraLosaRevit.Model.Bim.BimWorkSet.Factory;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.WPF_Pasada
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_AcotarPasada
    {

        public static void M1_CrearCopiaLocal(UI_AcotarPasadasNH uI_CopiaLocal, UIApplication _uiapp)
        {

            //if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);

      
            string tipoBOton = uI_CopiaLocal.BotonOprimido;
            // ManajeadorWorkSet _ManajeadorWorkSet = new ManajeadorWorkSet(_uiapp);
            uI_CopiaLocal.Hide();

            if (tipoBOton == "btn_CrearIndividual")
            {

                ManejadorAcotarPAsada _ManejadorAcotarPAsada = new ManejadorAcotarPAsada(_uiapp);                
                _ManejadorAcotarPAsada.M3_EjecutarAcotarPasadasDeUNo_selecconadaMouse_2doPtoAutomatico(uI_CopiaLocal.DimesionHorizonal, uI_CopiaLocal.DimesionVertical);
            }
            else if (tipoBOton == "btn_CrearMultiples")
            {
                ManejadorAcotarPAsada _ManejadorAcotarPAsada = new ManejadorAcotarPAsada(_uiapp); 
                if(uI_CopiaLocal.cbx_CrearMultiplo.Text== "Segun Usuario")
                    _ManejadorAcotarPAsada.M4_EjecutarAcotarPasadasD_conRectagulo_2doPtoAutomatico_segunUsuario(uI_CopiaLocal.DimesionHorizonal, uI_CopiaLocal.DimesionVertical);
                else if (uI_CopiaLocal.cbx_CrearMultiplo.Text == "Automatico")
                    _ManejadorAcotarPAsada.M5_EjecutarAcotarPasadasD_conRectagulo_2doPtoAutomatico();
            }
            else if (tipoBOton == "btn_CrearBorrar")
            {
                ManejadorAcotarPAsada _ManejadorAcotarPAsada = new ManejadorAcotarPAsada(_uiapp);
                _ManejadorAcotarPAsada.M6_BorrarDimensiones();
                
            }


            uI_CopiaLocal.Show();
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }






    }
}