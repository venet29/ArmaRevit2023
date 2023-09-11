using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.UpDate
{
    public class Manejador_UpdateRebar
    {
        private static Update_BarrasRebar updateopen;
        private static bool IsCARGADOUpdateREbar = false;


      
        //private UIApplication _uiapp;
        //private Document _doc;
        //private Update_BarrasRebar updateopen;
        //public Manejador_UpdateRebar(UIApplication _uiapp)
        //{
        //    this._uiapp = _uiapp;
        //    this._doc = _uiapp.ActiveUIDocument.Document;
        //}

        public static void CargarUpdateREbar(UIApplication _uiapp)
        {
            ConstNH.corte();//  // borrar lo de arriba(estatico) desconemtar el constructor y borrar variable 'IsCargarUpdateREbar'
            if (updateopen == null)
                updateopen = new Update_BarrasRebar(_uiapp, _uiapp.ActiveAddInId);
            
            if (IsCARGADOUpdateREbar) return;
            
            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    var _doc = _uiapp.ActiveUIDocument.Document;
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                    #region ELEMENTOS DISPARADORES
                    //BuiltInCategory.OST_IOSDetailGroups
                    ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);
                    //ElementCategoryFilter filtroGrupso = new ElementCategoryFilter(BuiltInCategory.OST_IOSDetailGroups);
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtromuros, Element.GetChangeTypeGeometry());
                    IsCARGADOUpdateREbar = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdateRebar' ex:" + ex.Message);
            }
        }


        public static void DesCargarUpdateREbar(UIApplication _uiapp)
        {
            if (updateopen == null)
                updateopen = new Update_BarrasRebar(_uiapp, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                    IsCARGADOUpdateREbar = false;
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdateRebar' ex:" + ex.Message);
            }
        }
    }
}
