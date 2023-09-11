using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.CAsos;
using ArmaduraLosaRevit.Model.Enumeraciones;
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
    public class Manejador_UpDateMoverTagPathSymbol
    {
        private static UpDate_EditTagPathSymbol updateopen;
        private static bool IsCARGADOUpdateTagPathSymbol = false;

        //private UIApplication _uiapp;
        //private Document _doc;
        //private UpDate_EditTagPathSymbol updateopen;
        //public Manejador_UpDateMoverTagPathSymbol(UIApplication _uiapp)
        //{
        //    this._uiapp = _uiapp;
        //    this._doc = _uiapp.ActiveUIDocument.Document;
        //}

        public static void CargarUpdateTagPathSymbol(UIApplication _uiapp)
        {
            ConstNH.corte();//  // borrar lo de arriba(estatico) desconemtar el constructor y borrar variable 'IsCargarUpdateREbar'
            if (updateopen == null)
                updateopen = new UpDate_EditTagPathSymbol(_uiapp, _uiapp.ActiveAddInId);
            if (IsCARGADOUpdateTagPathSymbol) return;

            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    {
                        var _doc = _uiapp.ActiveUIDocument.Document;
                        UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                        #region ELEMENTOS DISPARADORES
                        ElementCategoryFilter filtroPathReinTags = new ElementCategoryFilter(BuiltInCategory.OST_PathReinTags);
                        UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtroPathReinTags, Element.GetChangeTypeAny());
                        IsCARGADOUpdateTagPathSymbol = true;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpDateMoverTagPathSymbol' ex:" + ex.Message);
            }
        }


        public static void DescargarTagUpdatePathSymbol(UIApplication _uiapp)
        {
            if (updateopen == null)
                updateopen = new UpDate_EditTagPathSymbol(_uiapp, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                    IsCARGADOUpdateTagPathSymbol = false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpDateMoverTagPathSymbol' ex:" + ex.Message);
            }
        }
    }
}
