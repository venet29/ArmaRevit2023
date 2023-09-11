using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Contener
{
    public class ContendorRebar
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private static ElementId defaultRebarContainerTypeId;
        private RebarContainer _container;

        public ContendorRebar(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        public bool CrearContenedor(Element vigaMuro, List<Rebar> ListRebar1)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("creando contenedor-NH");
                    ElementId defaultRebarContainerTypeId = RebarContainerType.GetOrCreateRebarContainerType(_doc,"COntainerNH3");
                    _container = RebarContainer.Create(_doc, vigaMuro, defaultRebarContainerTypeId);
                
                    // Any items for this container should be presented in schedules and tags as separate subelements
                    //_container.PresentItemsAsSubelements = true;

                    foreach (var item in ListRebar1)
                    {
                        RebarContainerItem item_RC = _container.AppendItemFromRebar(item);
                        if (null != item_RC)
                        {
                            // set specific layout for new rebar as fixed number, with 10 bars, distribution path length of 1.5'
                            // with bars of the bar set on the same side of the rebar plane as indicated by normal
                            // and both first and last bar in the set are shown
                          //  item_RC.SetLayoutAsFixedNumber(10, 1.5, true, true, true);
                        }

                        // Hide the new item in the active view
                        //_container.SetItemHiddenStatus(_container.Document.ActiveView, item_RC.ItemIndex, true);
                    }


                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;

                return false;
            }


    
            return true;
        }

        public bool CrearContenedorSinTRAS(Element vigaMuro, List<Rebar> ListRebar1)
        {
            try
            {
                    if (defaultRebarContainerTypeId==null)defaultRebarContainerTypeId = RebarContainerType.CreateDefaultRebarContainerType(_doc);

                    _container = RebarContainer.Create(_doc, vigaMuro, defaultRebarContainerTypeId);

                    // Any items for this container should be presented in schedules and tags as separate subelements
                    _container.PresentItemsAsSubelements = true;

                    foreach (var item in ListRebar1)
                    {
                       var itemSelut= _container.AppendItemFromRebar(item);
                    }
             
            }
            catch (Exception ex)
            {
                string msj = ex.Message;

                return false;
            }



            return true;
        }

    }
}
