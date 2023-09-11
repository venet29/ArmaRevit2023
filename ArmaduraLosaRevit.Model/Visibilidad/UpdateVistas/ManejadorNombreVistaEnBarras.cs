using ArmaduraLosaRevit.Model.BarraV.UpDate;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.UpdateVistas
{
    public class ManejadorNombreVistaEnBarras
    {
        //private UIApplication _uiapp;
        //private Document _doc;
        private static UpdaterNombreVistaEnBarras updateopen;
        private static bool IsCargarUpdateView = false;

        //public ManejadorNombreVistaEnBarras(UIApplication _uiapp)
        //{
        //    this._uiapp = _uiapp;
        //    this._doc = _uiapp.ActiveUIDocument.Document;
        //}

        public static void CargarUpdateView(UIApplication _uiapp)
        {
            var _doc = _uiapp.ActiveUIDocument.Document;
            if (updateopen == null)
                updateopen = new UpdaterNombreVistaEnBarras(_doc,_uiapp.ActiveAddInId);

            if (IsCargarUpdateView) return;

            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                    #region ELEMENTOS DISPARADORES
                    ElementCategoryFilter filtromuros = new ElementCategoryFilter(BuiltInCategory.OST_Views);
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtromuros, Element.GetChangeTypeAny());
                    IsCargarUpdateView = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdaterNombreVistaEnBarras'. ex:" + ex.Message);
            }
        }


        public static void DesCargarUpdateView(UIApplication _uiapp)
        {
            var _doc = _uiapp.ActiveUIDocument.Document;
            if (updateopen == null)
                updateopen = new UpdaterNombreVistaEnBarras(_doc, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                    IsCargarUpdateView = false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpdaterNombreVistaEnBarras'. ex:" + ex.Message );
            }
        }
    }
}
