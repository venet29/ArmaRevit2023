using ArmaduraLosaRevit.Model.BarraV.Copiar.Helper;
using ArmaduraLosaRevit.Model.BarraV.Copiar.wpf;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Seleccionar;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar
{
    public class CArgar_CopiarElev
    {
        public static Result Ejecutar(UIApplication _uiapp)
        {
            try
            {

                View _view = _uiapp.ActiveUIDocument.ActiveView;

                Dictionary<string, double> listaLevelZ = _CalculoValorZ.ObtenerLista_Z_Level(_uiapp, _view);

                SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                List<EnvoltoriLevel> listaLevel = _seleccionarNivel.ObtenerListaEnvoltoriLevelOrdenadoPorElevacion(_view);

                CopiarRebarWpf _CopiarRebarWpf = new CopiarRebarWpf(listaLevel);
                _CopiarRebarWpf.ShowDialog();

                if (_CopiarRebarWpf.IsOk)
                {
                    if (_CopiarRebarWpf.casoEjecutar == TipoCaso.Copiar)
                    {
                        ManejadorCopiaElevBarra _ManejadorCopiaElevBarra = new ManejadorCopiaElevBarra(_uiapp, _CopiarRebarWpf);
                        _ManejadorCopiaElevBarra.Ejecutar();
                    }
                    else if (_CopiarRebarWpf.casoEjecutar == TipoCaso.Desocultar)
                    {
                        SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                        ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                        ManejadorVisibilidad.M8_DesOcultarBarrasVigaIdem();
                    }
                    else if (_CopiarRebarWpf.casoEjecutar == TipoCaso.ocultar)
                    {

                        SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                        ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                        ManejadorVisibilidad.M7_OcultarBarrasVigaIdem();

                    }


                }

            }
            catch (System.Exception)
            {
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }



}
