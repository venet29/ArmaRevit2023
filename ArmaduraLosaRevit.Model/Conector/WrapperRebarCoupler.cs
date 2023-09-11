using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Conector
{
    //https://knowledge.autodesk.com/es/support/revit-products/learn-explore/caas/CloudHelp/cloudhelp/2017/ESP/Revit-API/files/GUID-F9EA1C58-86D1-4085-9FDC-57D15E895A99-htm.html
    //http://help.autodesk.com/view/RVT/2017/ESP/?guid=GUID-F9EA1C58-86D1-4085-9FDC-57D15E895A99
    class WrapperRebarCoupler
    {
        private Document _doc;
        private List<Rebar> _bars;

        public WrapperRebarCoupler(Document doc, List<Rebar> bars)
        {
            _doc = doc;
            _bars = bars;
        }



        public RebarCoupler CreateCoupler()
        {
            ElementId defaultTypeId3 = _doc.GetDefaultFamilyTypeId(new ElementId(BuiltInCategory.OST_Coupler));
            RebarCoupler coupler = null;
            // if we have at least 2 bars, create a coupler between them
            if (_bars.Count > 1)
            {
                // get a type id for the Coupler
                ElementId defaultTypeId = _doc.GetDefaultFamilyTypeId(new ElementId(3327888));

                if (defaultTypeId != ElementId.InvalidElementId)
                {
                    // Specify the rebar and ends to couple
                    RebarReinforcementData rebarData1 = RebarReinforcementData.Create(_bars[0].Id, 0);
                    RebarReinforcementData rebarData2 = RebarReinforcementData.Create(_bars[1].Id, 1);

                    RebarCouplerError error;
                    coupler = RebarCoupler.Create(_doc, defaultTypeId, rebarData1, rebarData2, out error);

                     var lsiat=coupler.GetPointsForPlacement();

                    if (error != RebarCouplerError.ValidationSuccessfuly)
                    {
                        TaskDialog.Show("Revit", "Create Coupler failed: " + error.ToString());
                    }

                    // Use a coupler to cap the other end of the first bar
                    RebarReinforcementData rebarData = RebarReinforcementData.Create(_bars[0].Id, 1);
                    RebarCoupler.Create(_doc, defaultTypeId, rebarData, null, out error);
                    if (error != RebarCouplerError.ValidationSuccessfuly)
                    {
                        TaskDialog.Show("Revit", "Create Coupler failed: " + error.ToString());
                    }
                }
            }

            return coupler;
        }

        //Code Region: Change EndTreatmentType for RebarCoupler
        private void NewEndTreatmentForCouplerType(Document doc, ElementId couplerTypeId)
        {
            EndTreatmentType treatmentType = EndTreatmentType.Create(doc, "Custom");
            FamilySymbol couplerType = doc.GetElement(couplerTypeId) as FamilySymbol;
            Parameter param = couplerType.get_Parameter(BuiltInParameter.COUPLER_MAIN_ENDTREATMENT);
            param.Set(treatmentType.Id);
        }

        //Code Region: Set and EndTreatmentType for a RebarShape
        private bool SetEndTreatmentType(Document doc, RebarShape rebarShape)
        {
            bool set = false;
            // check if end treatments are defined by rebar shape
            ReinforcementSettings settings = ReinforcementSettings.GetReinforcementSettings(doc);
            if (!settings.RebarShapeDefinesEndTreatments)
            {
                try
                {
                    // can only be changed if document contains no rebars, area reinforcement or path reinforcement
                    settings.RebarShapeDefinesEndTreatments = true;
                }
                catch (Exception e)
                {
                    // cannot change the settings value
                    TaskDialog.Show("Revit", e.Message);
                }
            }
            if (settings.RebarShapeDefinesEndTreatments)
            {
                EndTreatmentType treatmentType = EndTreatmentType.Create(doc, "Flame Cut");
                rebarShape.SetEndTreatmentTypeId(treatmentType.Id, 0);

                ElementId treatmentTypeId = EndTreatmentType.CreateDefaultEndTreatmentType(doc);
                rebarShape.SetEndTreatmentTypeId(treatmentTypeId, 1);

                set = true;
            }

            return set;
        }
    }
}
