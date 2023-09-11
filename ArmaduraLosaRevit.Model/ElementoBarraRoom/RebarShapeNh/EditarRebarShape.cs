using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh
{
    class EditarRebarShape
    {
        private UIApplication _uiapp;
        private Document _doc;

        public EditarRebarShape(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        /// <summary>
        /// metodo pueba para analizar rebarshapp
        /// </summary>
        /// <returns></returns>
        public bool analizar()
        {
            try
            {

                PathReinforcement rshap = _doc.GetElement(new ElementId(3917528)) as PathReinforcement;

                RebarShape shapeDefinition = _doc.GetElement(rshap.PrimaryBarShapeId) as RebarShape;
                RebarShape rshapAlter = _doc.GetElement(rshap.AlternatingBarShapeId) as RebarShape;
            
                RebarShapeDefinition _RebarShapeDefinition = shapeDefinition.GetRebarShapeDefinition();

                RebarShapeDefinitionBySegments shapeDefinitionBySegments = _RebarShapeDefinition as RebarShapeDefinitionBySegments;

                RebarShapeSegment sss = shapeDefinitionBySegments.GetSegment(0);
               

                var lsita = _RebarShapeDefinition.GetParameters();
                foreach (var item in lsita)
                {
                    Element rshapitemLE = _doc.GetElement(item);
                    SharedParameterElement _SharedParameterElement = rshapitemLE as SharedParameterElement; // parametros rebar shape

                }

               List<SharedParameterElement> listaSharedParameterElement= _RebarShapeDefinition.GetParameters()
                                                                                            .Select(c => _doc.GetElement(c) as SharedParameterElement).ToList();



            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);

            }

            return true;
        }
    }
}
