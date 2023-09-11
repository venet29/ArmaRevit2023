using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using System.Linq;
using ArmaduraLosaRevit.Model.BarraV.Copiar.Helper;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar.WPF
{
    /// <summary>
    /// Create methods here that need to be wrapped in a valid Revit Api context.
    /// Things like transactions modifying Revit Elements, etc.
    /// </summary>
    internal class Methods_CopiarRebar
    {

        public static void M1_Cub(UI_CopiarRebarElev _ui, UIApplication _uiapp)
        {
            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M3_DesCargarBarras(_uiapp);
            try
            {
                string BotonOprimido = _ui.BotonOprimido;
                var lista_view = _ui.ListaLevel.ToList();

                var _view = _uiapp.ActiveUIDocument.ActiveView;
                _ui.Hide();

    
                if (_ui.IsOk)
                {
                    if (BotonOprimido == "Aceptar_Level")
                    {
                        List<double> listaLevelZz = _ui.ObtenerListaLevelZ();
                        string nombreLevelRef = _ui.ObtenerTexto() ;
                        //listaLevelZ.Add(Util.CmToFoot(200));
                        if (listaLevelZz.Count != 0)
                        {
                            ManejadorCopiaElevBarra _ManejadorCopiaElevBarra = new ManejadorCopiaElevBarra(_uiapp, listaLevelZz, nombreLevelRef);
                            _ManejadorCopiaElevBarra.Ejecutar();
                        }
              
                    }
                    else if (BotonOprimido == "Mostrar_Level")
                    {
                        SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                        ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                        ManejadorVisibilidad.M8_DesOcultarBarrasVigaIdem();
                    }
                    else if (BotonOprimido == "Ocultar_Level")
                    {

                        SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                        ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                        ManejadorVisibilidad.M7_OcultarBarrasVigaIdem();

                    }

                    else if (BotonOprimido == "Seleccionar_Level")
                    {

                        SeleccionarBarrasREbar_InfoComppleta _seleccionarBarrasRebar_InfoCompleta = new SeleccionarBarrasREbar_InfoComppleta(_uiapp);
                        _seleccionarBarrasRebar_InfoCompleta.SeleccionarBarrasViga_paraCOpiar();


                    }
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