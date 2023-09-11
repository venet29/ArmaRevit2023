using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    public class ScheduleFiltros
    {
        UIApplication _uiapp;
        private readonly ViewSchedule schedule;
        private Document _doc;
        private Dictionary<string, ElementId> listaParameter;

        public ScheduleFiltros(UIApplication _uiapp, ViewSchedule schedule)
        {
            this._uiapp = _uiapp;
            this.schedule = schedule;
            _doc = this._uiapp.ActiveUIDocument.Document;
        }

        public void Ejecutar(List<ScheduleFiltrosDTO> Lista_ScheduleFiltrosDTO)
        {
            listaParameter = AyudaBuscaParametrerShared.ObtenerListaPArameterShare(_doc);
            AddFilterToScheduleString(schedule, Lista_ScheduleFiltrosDTO);

        }


        public void AddFilterToScheduleString(ViewSchedule schedule, List<ScheduleFiltrosDTO> Lista_ScheduleFiltrosDTO)
        {
            // Find level field
            ScheduleDefinition definition = schedule.Definition;


            using (Transaction t = new Transaction(schedule.Document, "Add filter"))
            {
                t.Start();

                foreach (var _ScheduleFiltrosDTO in Lista_ScheduleFiltrosDTO)
                {
                    ElementId paramId = default;
                    if (_ScheduleFiltrosDTO.ElementIdPAra == null)
                        paramId = listaParameter.TryGetValue(_ScheduleFiltrosDTO.nombrePArameter, out var value) ? value : ElementId.InvalidElementId;
                    else
                        paramId = _ScheduleFiltrosDTO.ElementIdPAra;

                    if (paramId.IsInvalid() == true) return;

                    ScheduleField levelField = FindField(schedule, paramId);

                    // Add filter

                    // If field not present, add it
                    if (levelField == null)
                    {
                        levelField = definition.AddField(ScheduleFieldType.Instance, paramId);
                    }

                    // Set field to hidden
                    //levelField.IsHidden = true;
                    ScheduleFilter filter = default;
                    if (_ScheduleFiltrosDTO._ScheduleFilterType == ScheduleFilterType.HasNoValue)
                        filter = new ScheduleFilter(levelField.FieldId, _ScheduleFiltrosDTO._ScheduleFilterType);
                    else
                    {
                        if (_ScheduleFiltrosDTO.valor is string)
                            filter = new ScheduleFilter(levelField.FieldId, _ScheduleFiltrosDTO._ScheduleFilterType, (string)_ScheduleFiltrosDTO.valor);
                        else if (_ScheduleFiltrosDTO.valor is double)
                            filter = new ScheduleFilter(levelField.FieldId, _ScheduleFiltrosDTO._ScheduleFilterType, (double)_ScheduleFiltrosDTO.valor);
                        else if (_ScheduleFiltrosDTO.valor is int)
                            filter = new ScheduleFilter(levelField.FieldId, _ScheduleFiltrosDTO._ScheduleFilterType, (int)_ScheduleFiltrosDTO.valor);
                        else if (_ScheduleFiltrosDTO.valor is ElementId)
                            filter = new ScheduleFilter(levelField.FieldId, _ScheduleFiltrosDTO._ScheduleFilterType, (ElementId)_ScheduleFiltrosDTO.valor);
                    }

                    definition.AddFilter(filter);

                }
                t.Commit();
            }

        }


        /// 

        /// Finds an existing ScheduleField matching the given parameter
        /// 

        /// 
        /// 
        /// 
        public static ScheduleField FindField(ViewSchedule schedule, ElementId paramId)
        {
            ScheduleDefinition definition = schedule.Definition;
            ScheduleField foundField = null;

            foreach (ScheduleFieldId fieldId in definition.GetFieldOrder())
            {
                foundField = definition.GetField(fieldId);
                if (foundField.ParameterId == paramId)
                {
                    return foundField;
                }
            }

            return null;
        }


    }
}
