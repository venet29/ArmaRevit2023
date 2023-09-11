using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter.Rules
{
    class CambiarEstado
    {

        internal bool M1_CambiarEstadoVisibilidad_SinTrans(Document _doc,View _view,string nombreFiltro, bool IsVisible)
        {
            try
            {
                ParameterFilterElement parameterFilterElement = TiposFiltros.M1_GetFiltros_nh(nombreFiltro, _doc);
                if (parameterFilterElement == null)
                {
                    Util.InfoMsg($"No se encontro filtro {nombreFiltro}");
                    return false;
                }
                // Apply filter to view
                if (!_view.IsFilterApplied(parameterFilterElement.Id))
                    _view.AddFilter(parameterFilterElement.Id);

                _view.SetFilterVisibility(parameterFilterElement.Id, IsVisible);
            }
            catch (Exception ex)
            {
                Util.InfoMsg($"Error en 'CambiarEstadoVisibilidad' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        internal bool M2_BorrarVisibilidad_SinTrans(Document _doc, View _view, List<ElementId> ListaRemoverFiltroExitente, List<ElementId> ListaBorrarFiltroExitente)
        {
            try
            {
                try
                {
                    foreach (ElementId item in ListaRemoverFiltroExitente)
                    {
                        if (_view.IsFilterApplied(item))
                            _view.RemoveFilter(item);
                    }

                    if (ListaBorrarFiltroExitente.Count > 0)
                        _doc.Delete(ListaBorrarFiltroExitente);
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al remover y borrar filtros  \n  ex:{ex.Message}");

                }
            }
            catch (Exception ex)
            {
                Util.InfoMsg($"Error en 'CambiarEstadoVisibilidad' ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
