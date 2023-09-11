using ArmaduraLosaRevit.Model.ViewFilter.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ViewFilter.Rules;
using ArmaduraLosaRevit.Model.ViewFilter.Analisis;

namespace ArmaduraLosaRevit.Model.ViewFilter
{
    public class CreadorViewFilter_Borrar : CreadorViewFilter_Base
    {
        public CreadorViewFilter_Borrar(UIApplication _uiapp, View view3D) :base(_uiapp)
        {
            _view3D = view3D;
            nombreFiltro = "";
        }

        public void M3_BorrarFiltros(List<string> ListaNombreIgualExcluir,List<string> ListaNombreContein)
        {
            if (_view3D == null) return;
            // if (!(_view3D is View3D)) return;
            RestearParametrosSoloRebar();
            M3_BorrarBase(_view3D,ListaNombreIgualExcluir, ListaNombreContein);
        }

        public void M3_BorrarFiltros_RebarYPAth(List<string> ListaNombreIgualExcluir, List<string> ListaNombreContein)
        {
            if (_view3D == null) return;
            // if (!(_view3D is View3D)) return;
            RestearParametros();
            M3_BorrarBase(_view3D, ListaNombreIgualExcluir, ListaNombreContein);
        }
        private void M3_BorrarBase(View _view3D, List<string> ListaNombreIgualExcluir, List<string> ListaNombreContein)
        {
            try
            {
                BuscarBorrarRemoverFiltro _BuscarBorrarRemoverFiltro = new BuscarBorrarRemoverFiltro(_doc);
                if (!_BuscarBorrarRemoverFiltro.C_BuscarSiExisteFiltro_NombreVistaYTipoBarras(_view3D, ListaNombreIgualExcluir, ListaNombreContein)) return;

                ListaBorrarFiltroExitente = _BuscarBorrarRemoverFiltro.ListaBorrarFiltroExitente;
                ListaRemoverFiltroExitente = _BuscarBorrarRemoverFiltro.ListaRemoverFiltroExitente;
                D_EjecutarREmoverBorrar_ConTras(_view3D);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }
    }
}
