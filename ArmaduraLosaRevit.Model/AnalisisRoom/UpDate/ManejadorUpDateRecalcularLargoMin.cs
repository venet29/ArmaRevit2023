using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Tag.UpDate
{
    public class ManejadorUpDateRecalcularLargoMin
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorUpDateRecalcularLargoMin(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;

            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public void CargarRecalcularLargoMin()
        {
            UpDateRecalcularLargoMin updateopen = new UpDateRecalcularLargoMin(_doc,_uiapp.ActiveAddInId);
            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                    #region ELEMENTOS DISPARADORES
                    ElementCategoryFilter filtroPathReinTags = new ElementCategoryFilter(BuiltInCategory.OST_RoomTags);
                    List<ElementId> ListTag = TiposRoomTagEnView.M1_AllGetFamilySymbol(_doc, _doc.ActiveView).Select(c=> c.Id).ToList();
                    // UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtroPathReinTags, Element.GetChangeTypeGeometry());
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), _doc,ListTag, Element.GetChangeTypeAny());
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor UpDateRecalcularLargoMin ex:" + ex.Message);
            }
        }


        public void DesCargarRecalcularLargoMin()
        {
            UpDateRecalcularLargoMin updateopen = new UpDateRecalcularLargoMin(_doc, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor UpDateRecalcularLargoMin ex:" + ex.Message );
            }
        }
    }
}
