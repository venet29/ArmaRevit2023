using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.CAsos;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update
{
    /// <summary>
    /// reactor no utilizado pq al activar rebarInsystem tb se activan los rebar y se activa 'UpdaterBarrasRebar'
    /// y UpdaterBarrasRebar vulve a activar esta classe y se hacen llamadas recuerrentes ineficiente
    /// </summary>
    public class Manejador_UpDateEditPathReinf
    {
        private UIApplication _uiapp;
        private Document _doc;

        public Manejador_UpDateEditPathReinf(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public void CargarUpdateEditPathReinf()
        {
            UpDate_EditPathRein updateopen = new UpDate_EditPathRein(_uiapp, _uiapp.ActiveAddInId);
            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.


                    var para = new ElementId(BuiltInParameter.PATH_REIN_NUMBER_OF_BARS);
                    #region ELEMENTOS DISPARADORES
                    ElementCategoryFilter filtroPathReinTags = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);
                    // UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtroPathReinTags, Element.GetChangeTypeAny());
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtroPathReinTags, Element.GetChangeTypeGeometry());
                    //UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtroPathReinTags, Element.GetChangeTypeAny());
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpDatePathSymbol' ex:" + ex.Message);
            }
        }


        public void DesCargarUpdatePathReinf()
        {
            UpDate_EditPathRein updateopen = new UpDate_EditPathRein(_uiapp, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpDate_MoverEditPathRein' ex:" + ex.Message );
            }
        }
    }
}
