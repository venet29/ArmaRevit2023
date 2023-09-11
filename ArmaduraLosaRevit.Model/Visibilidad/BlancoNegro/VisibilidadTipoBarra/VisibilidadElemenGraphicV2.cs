
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public abstract class VisibilidadElemenGraphicV2
    {
        protected Document _doc;
        protected UIApplication _uiapp;

        public View _viewAnalizado { get; }

        public VisibilidadElemenGraphicV2(UIApplication _uiapp, View _viewAnalizado)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
            this._viewAnalizado = _viewAnalizado;
        }
   

    
        //***
        /// <summary>
        /// princiapl y mas generico
        /// </summary>
        /// <param name="Listid"></param>
        /// <param name="ogs"></param>
        public void ChangeListElementsColorSinTrans(List<ElementId> Listid, OverrideGraphicSettings ogs)
        {
            if (Listid == null) return;
            if (Listid.Count == 0) return;
            try
            {
                for (int i = 0; i < Listid.Count; i++)
                {

                    _viewAnalizado.SetElementOverrides(Listid[i], ogs);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex :{ex.Message}");

            }
        }

        public void ChangeListaElementColorConTrans(List<ElementId> Listid, OverrideGraphicSettings ogs)
        {
            if (Listid == null) return;
            if (Listid.Count == 0) return;
            if (ogs==null) return;

            try
            {
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("Change Element Colo-NHr");
                    for (int i = 0; i < Listid.Count; i++)
                    {
                        _viewAnalizado.SetElementOverrides(Listid[i], ogs);
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }
    }
}
