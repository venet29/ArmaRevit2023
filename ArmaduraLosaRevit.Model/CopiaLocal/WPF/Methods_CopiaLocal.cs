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
using ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado;

namespace ArmaduraLosaRevit.Model.CopiaLocal.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_CopiaLocal
    {

        public static void M1_CrearCopiaLocal(UI_CopiaLocal uI_CopiaLocal, UIApplication _uiapp)
        {

            //if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            string tipoPosiicon = uI_CopiaLocal.BotonOprimido;
            uI_CopiaLocal.Hide();
            if (tipoPosiicon == "btn_CopiaLocal")
            {
                BuscadorRutasLocal _BuscadorRutaShare = new BuscadorRutasLocal(_uiapp);
                _BuscadorRutaShare.BuscarRutaLocalDeProyectoEnlaNube();
            }
            else if (tipoPosiicon == "btn_CopiaRespaldo")
            {
                BuscadorRutasLocal _BuscadorRutaShare = new BuscadorRutasLocal(_uiapp);
                _BuscadorRutaShare.COpiarArchivoREspaldo();
            }
            else if (tipoPosiicon == "btn_AppRequeriminetos")
            {
                System.Diagnostics.Process.Start("http://192.168.0.102:5010/");
            }
            
            else if (tipoPosiicon == "btn_CambiarEstado")
            {
                var lista =uI_CopiaLocal.EstadoCambio.Split('/');
                CambioEstadoProyecto _CambioEstadoProyecto = new CambioEstadoProyecto(_uiapp);
                _CambioEstadoProyecto.Ejecutar(lista[0].Trim());
            }
            uI_CopiaLocal.Show();
            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }






    }
}