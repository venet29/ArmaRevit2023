using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.CAsos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
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
    public class Manejador_UpDateMoverPathSymbol
    {
        private static UpDate_MoverPathSymbol updateopen;
        private static bool IsCARGADOUpdatePathSymbol = false;

        //private UIApplication _uiapp;
        //private Document _doc;
       // private UpDate_MoverPathSymbol updateopen;
        //public Manejador_UpDateMoverPathSymbol(UIApplication _uiapp)
        //{
        //    this._uiapp = _uiapp;
        //    this._doc = _uiapp.ActiveUIDocument.Document;
        //}

        public static void CargarUpdatePathSymbol(UIApplication _uiapp)
        {
            ConstNH.corte();//  // borrar lo de arriba(estatico) desconemtar el constructor y borrar variable 'IsCargarUpdateREbar'
            UtilVersionesRevit.ObtenerVersionRevit(_uiapp);
            if (updateopen == null)
                updateopen = new UpDate_MoverPathSymbol(_uiapp, _uiapp.ActiveAddInId);

            if (IsCARGADOUpdatePathSymbol) return;

            try
            {
                if (!UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    var _doc = _uiapp.ActiveUIDocument.Document;
                    UpdaterRegistry.RegisterUpdater(updateopen, _doc, true);//PREGUNTAR AL USUARIO: TRUE ES OPCIONAL. FALSE ES OBLIGATORIO.

                    #region ELEMENTOS DISPARADORES
                    ElementCategoryFilter filtroPathReinTags = new ElementCategoryFilter(BuiltInCategory.OST_PathReinSpanSymbol);
                    UpdaterRegistry.AddTrigger(updateopen.GetUpdaterId(), filtroPathReinTags, Element.GetChangeTypeGeometry());
                    IsCARGADOUpdatePathSymbol = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpDatePathSymbol' ex:" + ex.Message);
            }
        }


        public static void DesCargarUpdatePathSymbol(UIApplication _uiapp)
        {
            UtilVersionesRevit.ObtenerVersionRevit(_uiapp);
            if (updateopen == null)
                updateopen = new UpDate_MoverPathSymbol(_uiapp, _uiapp.ActiveAddInId);

            try
            {
                if (UpdaterRegistry.IsUpdaterRegistered(updateopen.GetUpdaterId()))
                {
                    UpdaterRegistry.UnregisterUpdater(updateopen.GetUpdaterId());
                    IsCARGADOUpdatePathSymbol = false;
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error Descargar Reactor 'UpDatePathSymbol' ex:" + ex.Message );
            }
        }
    }
}
