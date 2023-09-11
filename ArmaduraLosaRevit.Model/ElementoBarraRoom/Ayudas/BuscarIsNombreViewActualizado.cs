using Autodesk.Revit.DB;
using System;
using ArmaduraLosaRevit.Model.Extension;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    public class BuscarIsNombreViewActualizado
    {

        public static bool IsError(View _view,bool IsMostrarMje=true)
        {

            try
            {
                //1) dependencia
                string nombreActua = _view.ObtenerNombreIsDependencia();

                //2)nombre de parametro
                Parameter _para = _view.GetParameter2("ViewNombre");
                if (_para == null) return false;
                string _paraViewNombre = _para.AsString();

                //3) verificar
                if (_paraViewNombre != nombreActua)
                {
                    if(IsMostrarMje) Util.ErrorMsg("Nombre de vista no actualizado. Actualizar con comando 'Actualizar Nombre View");
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                
            }
            return true; 

        }
        public static bool IsError_SOloconfiguracionInicial(View _view, TipoBarraGeneral _tipoBarraGeneral)
        {
            try
            {
                //3) verificar
                if (_tipoBarraGeneral == TipoBarraGeneral.Losa && _view.ViewType != ViewType.FloorPlan)
                {
                    Util.ErrorMsg("Comando sebe ser ejecutada en una vista de Armadura de losa");
                    return true;
                }

                if (_tipoBarraGeneral == TipoBarraGeneral.Elevacion && _view.ViewType != ViewType.Section)
                {
                    Util.ErrorMsg("Comando sebe ser ejecutada en una vista de Armadura de elevaciones");
                    return true;
                }


                //1) dependencia
                string nombreActua = _view.ObtenerNombreIsDependencia();

                //2)nombre de parametro
                Parameter _para = _view.GetParameter2("ViewNombre");
                if (_para == null) return false;
                string _paraViewNombre = _para.AsString();

                // esta linea es para el caso en que se creal alguna vista y esste parametro esta como nulo
                if (_paraViewNombre == null) return false;


                //solo caso losas 
                if (_tipoBarraGeneral == TipoBarraGeneral.Losa &&  _view.ViewType == ViewType.FloorPlan && _paraViewNombre == ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA) return false;
                //solo caso elevacion 
                if (_tipoBarraGeneral == TipoBarraGeneral.Elevacion &&  _view.ViewType == ViewType.Section && _paraViewNombre == ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV) return false;

                //3) verificar
                if (_paraViewNombre != nombreActua)
                {
                    Util.ErrorMsg("Nombre de vista no actualizado. Actualizar con comando 'Actualizar Nombre View");
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            return true;

        }


    }
}
