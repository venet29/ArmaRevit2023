using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.Borrar
{
   public class BorrarAreaPAthRectangulo
    {

        public static  void EjecutarBorrarAreaPath(UIApplication _uiapp)
        {

            try
            {
                SeleccionarAreaPath seleccionarAreaPathRectangulo = new SeleccionarAreaPath(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
                seleccionarAreaPathRectangulo.GetRoomSeleccionadosConRectaguloYFiltros();
                seleccionarAreaPathRectangulo.BorrarAreaPathSeleccionado();

                //  visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            catch (Exception e)
            {
             Util.ErrorMsg($"Error al borrar:{e.Message}" );
                
            }
            finally
            {
                //visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
        }
    }
}
