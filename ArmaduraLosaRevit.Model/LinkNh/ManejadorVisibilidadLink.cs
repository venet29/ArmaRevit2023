using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LinkNh
{
    public class ManejadorVisibilidadLink
    {
        UIApplication _uiApp;
        private Document _doc;
        private ICollection<ElementId> elementIdSet;

        public ManejadorVisibilidadLink(UIApplication uiapp)
        {
            this._uiApp = uiapp;
            this._doc = this._uiApp.ActiveUIDocument.Document;
        }


        public bool Ejecutar(ICollection<ElementId> elementIdSet_, bool IsVisibildad)
        {
            try
            {
                if (elementIdSet_.Count == 0)
                    Seleccionar();
                else
                {
                    elementIdSet = elementIdSet_;
                }
                EjecutarCambioVisivilidad(IsVisibildad);

                return true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en Ejecutar visibilidad. ex:{ex.Message}");

                return false;
            }

        }

        private void Seleccionar()
        {
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            elementIdSet = collector.OfCategory(BuiltInCategory.OST_RvtLinks).OfClass(typeof(RevitLinkInstance)).ToElementIds();
        }

        private void EjecutarCambioVisivilidad(bool IsVisibildad)
        {
            using (Transaction trans = new Transaction(_doc, "LinkedFileVisibility"))
            {

                trans.Start();
                foreach (ElementId linkedFileId in elementIdSet)
                {
                    if (linkedFileId != null)
                    {
                        if (IsVisibildad)
                        {
                            if (true == _doc.GetElement(linkedFileId).IsHidden(_doc.ActiveView))
                            {
                                if (true == _doc.GetElement(linkedFileId).CanBeHidden(_doc.ActiveView))
                                {
                                    _doc.ActiveView.UnhideElements(elementIdSet);
                                }
                            }
                        }
                        else
                        {

                            if (false == _doc.GetElement(linkedFileId).IsHidden(_doc.ActiveView))
                                _doc.ActiveView.HideElements(elementIdSet);

                        }
                    }
                }
                trans.Commit();

            }
        }


        //*

        private void EjecutarCambioVisivilidadIicial(bool IsVisibildad)
        {
            using (Transaction trans = new Transaction(_doc, "LinkedFileVisibility"))
            {

                trans.Start();
                foreach (ElementId linkedFileId in elementIdSet)
                {
                    if (linkedFileId != null)
                    {

                        if (true == _doc.GetElement(linkedFileId).IsHidden(_doc.ActiveView))
                        {
                            if (true == _doc.GetElement(linkedFileId).CanBeHidden(_doc.ActiveView))
                            {
                                _doc.ActiveView.UnhideElements(elementIdSet);
                            }
                        }
                        else
                        {
                            _doc.ActiveView.HideElements(elementIdSet);
                        }

                    }
                }
                trans.Commit();

            }
        }
    }
}
