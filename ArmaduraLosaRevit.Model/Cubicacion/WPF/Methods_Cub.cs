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

namespace ArmaduraLosaRevit.Model.Cubicacion.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_Cub
    {

        public static void M1_Cub(UI_Cub _ui, UIApplication _uiapp)
        {
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
            try
            {
                string tipoPosiicon = _ui.BotonOprimido;
                var lista_view = _ui.ListaLevel.ToList();
                var lista_Losa = _ui.ListaLosa.ToList();
                var lista_Elev = _ui.ListaElev.ToList();
               
                _ui.Hide();
                //  EditarPathReinMouse EditarPathReinMouse = new EditarPathReinMouse(_uiapp);
                if (tipoPosiicon == "Aceptar_Level")
                {
                    var _manejadorCubDTO = new ManejadorCubDTO()
                    {
                        nombreProyecto = _ui.NombreProyecto.Text,
                        numeroObra = _ui.NumeroObra.Text,
                        Nombre3d=_ui.SelectView3d,
                        lista_view = lista_view,
                        lista_Losa = lista_Losa,
                        lista_Elev = lista_Elev
                    };
                    ManejadorCub _ManejadorCub = new ManejadorCub(_uiapp, _manejadorCubDTO);
                    _ManejadorCub.Ejecutar();
                    //_ManejadorCub.Ejecutar(_uiapp.ActiveUIDocument.ActiveView);  // cubica el 3D
                }
                _ui.Show();
            }
            catch (System.Exception ex)
            {
                Util.DebugDescripcion(ex);
            }


            UpdateGeneral.M2_CargarBArras(_uiapp);
        }






    }
}