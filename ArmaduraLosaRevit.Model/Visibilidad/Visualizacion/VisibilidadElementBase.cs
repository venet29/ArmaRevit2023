
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
    public abstract class VisibilidadElementBase
    {
        protected Document _doc;
        protected UIApplication _uiapp;

        public VisibilidadElementBase(UIApplication _uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
        }
        public VisibilidadElementBase(Document doc)
        {
            this._doc = doc;
        }

        #region  1) Ocultar Elemento

        public void vi1_OcultarElemento(IList<Element> Elementos, View view, bool IsCOnstrans = true)
        {
            if (Elementos == null) return;
            if (Elementos.Count == 0) return;
            List<ElementId> ElementosIDs = Elementos.Where(c => c.IsHidden(view) == false).Select(el => el.Id).ToList();

            if (IsCOnstrans)
                Vi_1_OcultarElementID_conTrans(ElementosIDs, view);
            else
                Vi1_2_OcultarElementID_SinTrans(ElementosIDs, view);

        }

        private void Vi1_2_OcultarElementID_SinTrans(List<ElementId> ElementosIDs, View view)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                view.HideElements(ElementosIDs);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                message = "Error al crear Path Symbol";
            }
        }

        public void Vi_1_OcultarElementID_conTrans(List<ElementId> ElementosIDs, View view)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Ocultando elementos-NH");
                    //view.IsolateElementsTemporary(ElementosIDs);
                    view.HideElements(ElementosIDs);
                    trans2.Commit();
                    // uidoc.RefreshActiveView();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }
        }

        #endregion

        #region 2) DesOcultar Elemento
        public void Vi2_DesOcultarElemento_conTrans(IList<Element> Elementos, View view)
        {
            if (Elementos == null) return;
            if (Elementos.Count == 0) return;
            List<ElementId> ElementosIDs = Elementos.Select(el => el.Id).ToList();
            try
            {
                //segunda trasnREV

                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Ocultando elementos-NH");
                    view.UnhideElements(ElementosIDs);
                    trans2.Commit();
                    // uidoc.RefreshActiveView();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }

        }

        public void Vi3_DesOcultarElemento_SinTrans(IList<Element> Elementos, View view)
        {
            if (Elementos == null) return;
            if (Elementos.Count == 0) return;

            List<ElementId> ElementosIDs = Elementos.Select(el => el.Id).ToList();
            try
            {
                //segunda trasnREV


                view.UnhideElements(ElementosIDs);


            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }

        }
        #endregion

        #region 3) cambiar color


        public void ChangeElementColorCONtrans(ElementId elemtid, Color color_, bool _Halftone = false)
        {
            if (elemtid == null) return;
            List<ElementId> Listid = new List<ElementId>();
            Listid.Add(elemtid);

            ChangeListaElementColorConTrans(Listid, color_, _Halftone);
        }
        public void ChangeListaElementColorConTrans(List<ElementId> Listid, Color color_, bool _Halftone = false)
        {
            if (Listid == null) return;
            if (Listid.Count == 0) return;

            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ogs.SetProjectionLineColor(color_);
            ogs.SetCutLineColor(color_);
            if (_Halftone) ogs.SetHalftone(true);
            try
            {

                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("Change Element Colo-NHr");
                    for (int i = 0; i < Listid.Count; i++)
                    {

                        _doc.ActiveView.SetElementOverrides(Listid[i], ogs);
                    }
                    tx.Commit();
                }

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }


        public void ChangeElementColorSinTrans(ElementId elid, Color color_, bool _Halftone = false)
        {
            if (elid == null) return;
            List<ElementId> Listid = new List<ElementId>();
            Listid.Add(elid);

            ChangeListElementsColorSinTrans(Listid, color_, _Halftone);
        }

    

        public void ChangeListElementsColorSinTrans(List<ElementId> Listid, Color color_, bool _Halftone = false)
        {
            if (Listid == null) return;
            if (Listid.Count == 0) return;
            try
            {
                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                ogs.SetProjectionLineColor(color_);
                ogs.SetCutLineColor(color_);
                if (_Halftone) ogs.SetHalftone(true);

                for (int i = 0; i < Listid.Count; i++)
                {

                    _doc.ActiveView.SetElementOverrides(Listid[i], ogs);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex :{ex.Message}");

            }
        }

        #endregion


        public void ChangePresentationModeRebarSinTrans(List<Rebar> Listid, RebarPresentationMode _rebarPresentationMode, View viewActual)
        {
            if (Listid == null) return;
            if (Listid.Count == 0) return;
            foreach (Rebar _rebar in Listid)
            {
                int test = _rebar.NumberOfBarPositions;
                _rebar.SetPresentationMode(viewActual, _rebarPresentationMode);
            }

        }
        public void ChangePresentationModeRebarCONTrans(List<Rebar> listRebar, RebarPresentationMode _rebarPresentationMode, View viewActual)
        {
            if (listRebar == null) return;
            if (listRebar.Count == 0) return;
            try
            {
                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("PresentationModeRebar-NHr");
                    foreach (Rebar _rebar in listRebar)
                    {
                        _rebar.SetPresentationMode(viewActual, _rebarPresentationMode);
                    }

                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }



        public abstract void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view);

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

                    _doc.ActiveView.SetElementOverrides(Listid[i], ogs);
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
                        _doc.ActiveView.SetElementOverrides(Listid[i], ogs);
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
