using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.UpDate
{
    public class ManejadorUpdaterRebarPath
    {

        private static bool IsCARGADOUpdateREbarPath = false;
        private static UpdaterRebarPath updateopen;


        //private UIApplication _uiapp;
        //private Document _doc;
        //private UpdaterRebarPath updateopen;
        //public ManejadorUpdaterRebarPath(UIApplication _uiapp)
        //{
        //    this._uiapp = _uiapp;
        //    this._doc = _uiapp.ActiveUIDocument.Document;
        //}

        public static void CargarUpdateREbarPath(UIApplication _uiapp)
        {
            var _doc = _uiapp.ActiveUIDocument.Document;

            ConstNH.corte();//  // borrar lo de arriba(estatico) desconemtar el constructor y borrar variable 'IsCargarUpdateREbar'
            if (updateopen == null)
                updateopen = new UpdaterRebarPath(_doc,_uiapp.ActiveAddInId);

            if (IsCARGADOUpdateREbarPath) return;

            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                    #region ELEMENTOS DISPARADORES
                    ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_IOSDetailGroups);
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtromuros, Element.GetChangeTypeAny());
                    IsCARGADOUpdateREbarPath = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdaterRebarPath' ex:" + ex.Message);
            }
        }


        public static void DesCargarUpdateREbarPAth(UIApplication _uiapp)
        {
            var _doc = _uiapp.ActiveUIDocument.Document;

            if (updateopen == null)
                updateopen = new UpdaterRebarPath(_doc, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                    IsCARGADOUpdateREbarPath=false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdaterRebarPath' ex:" + ex.Message );
            }
        }
    }
}
