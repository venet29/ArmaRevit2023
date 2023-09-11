using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using System.Collections.ObjectModel;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model;
using System;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto;
using System.IO;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_VariasElevAuto
    {

        public static void M1_Cub(UI_VariasElevAuto _ui, UIApplication _uiapp)
        {
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
            try
            {
                string tipoPosiicon = _ui.BotonOprimido;
                string tipoView=_ui.tipo_View.Text;


                _ui.Hide();
                //  EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);
                if (tipoPosiicon == "Aceptar")
                {
                    List<VariasElevAutoDTO> lista = _ui.ListaElev.Where(c=> c.IsSelected && IsTipoView(c, tipoView) && c.NombreJson!= "NoEncontrado").Select(r=> new VariasElevAutoDTO(r.ViewElev,r.ArchJsonSeleccion)).ToList();

                    var propAutoelev = _ui.Ui_AutoElev;
                    string rutaGuardarArchivos= _ui.rutaGuardarArchivos;
                    if(!Directory.Exists( rutaGuardarArchivos))
                        rutaGuardarArchivos= Util.InfoRutaMisdocumentos();
                    // ejecutar exportacion
                    ManejadorBarrasAutomaticas manejadorBarrasAutomaticas = new ManejadorBarrasAutomaticas(_uiapp,ref propAutoelev, rutaGuardarArchivos);
                    manejadorBarrasAutomaticas.EjecutarVariasImportacion(lista);
                }
                _ui.Show();
            }
            catch (System.Exception ex)
            {
                Util.DebugDescripcion(ex);
            }


            UpdateGeneral.M2_CargarBArras(_uiapp);
        }

        private static bool IsTipoView(ElevacionVAriosDto c, string tipoView)
        {
            if (tipoView.ToLower() == "todos") return true;

            if (tipoView == c.TipoView)
                return true;
            else
                return false;
        }
    }
}