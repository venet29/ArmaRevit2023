using ArmaduraLosaRevit.Model.EditarPath;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ViewportnNH;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.GRIDS.CambiarNombre.Model;
using ArmaduraLosaRevit.Model.Visibilidad;

namespace ArmaduraLosaRevit.Model.GRIDS.WPF_CambiarNombreGrid
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_CambiarNombreGrid
    {
        public static void M1_Ejecutar(UI_CambiarNombreGrid _ui, UIApplication _uiapp)
        {
            UtilStopWatch _utilStopWatch = new UtilStopWatch();

            // if (BuscarIsNombreViewActualizado.IsError(_uiapp.ActiveUIDocument.ActiveView)) return;
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);


            List<View> lista_view = new List<View>() { _uiapp.ActiveUIDocument.ActiveView };
            string tipoPosiicon = _ui.BotonOprimido;
            //  EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);

            _ui.Hide();

            if (tipoPosiicon == "VisulaizarVIew")
            {
                CambiarVisibilidadViewSection _CambiarVisibilidadViewSection = new CambiarVisibilidadViewSection(_uiapp);
                _CambiarVisibilidadViewSection.CambiarVisibilidad(true, _ui.viewGeomSelec.viewDTO_.View_);
            }
            else if (tipoPosiicon == "OCultarVIew")
            {
                CambiarVisibilidadViewSection _CambiarVisibilidadViewSection = new CambiarVisibilidadViewSection(_uiapp);
                _CambiarVisibilidadViewSection.CambiarVisibilidad(false, _ui.viewGeomSelec.viewDTO_.View_);
            }
            else
            {
                var servicioViewCOntendioa = _ui._ServicioBuscarViewContendioEnGrid;

                List<EnvoltorioGrid_view> ListaNombre = new List<EnvoltorioGrid_view>();
                if (tipoPosiicon == "bton_Horizontal")
                {
                    ListaNombre = servicioViewCOntendioa.Lista_EnvoltorioGrid_view.Where(c => c.TipoGrid_ == TipoGrid.Horizonal).ToList();
                }
                else if (tipoPosiicon == "bton_cambiarVertical")
                {
                    ListaNombre = servicioViewCOntendioa.Lista_EnvoltorioGrid_view.Where(c => c.TipoGrid_ == TipoGrid.Vertical).ToList();
                }
                else if (tipoPosiicon == "bton_cambiarOtro")
                {
                    ListaNombre = servicioViewCOntendioa.Lista_EnvoltorioGrid_view.Where(c => c.TipoGrid_ == TipoGrid.Otros).ToList();
                }

                servicioViewCOntendioa.M3_CAmbiarNOmbre(ListaNombre);
            }
            _ui.Show();

            UpdateGeneral.M2_CargarBArras(_uiapp);
            //CargarCambiarPathReinfomenConPto_Wpf
        }

    }
}