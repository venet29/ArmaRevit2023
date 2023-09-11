using System;
using System.Linq;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.TablasSchedule;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model
{
    internal class ScheduleNHBase
    {
        private UIApplication _uiapp;
        private Document _doc;
#pragma warning disable CS0649 // Field 'ScheduleNHBase.vs' is never assigned to, and will always have its default value null
        private ViewSchedule vs;
#pragma warning restore CS0649 // Field 'ScheduleNHBase.vs' is never assigned to, and will always have its default value null
#pragma warning disable CS0169 // The field 'ScheduleNHBase.vs_resumen' is never used
        private ViewSchedule vs_resumen;
#pragma warning restore CS0169 // The field 'ScheduleNHBase.vs_resumen' is never used

        public ScheduleNHBase(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _doc = uiapp.ActiveUIDocument.Document;
        }  

        private void PrteCAlcualo()
        {
            var vs_ = TiposView.ObtenerTiposView("pruebaBarra", _doc);

            ViewSchedule vs = vs_ as ViewSchedule;
            ScheduleDefinition definition = vs.Definition;
            var listaFieldOrder = definition.GetFieldOrder().ToList();

            foreach (ScheduleFieldId id in listaFieldOrder)
            {
                var resul = vs.Definition.GetField(id).GetSchedulableField();
            }


        }

        //gropu  
        //https://twentytwo.space/2021/05/02/revit-api-schedule-creation/

   

        /// Adds a single parameter field to the schedule
        /// 

        protected void AddRegularFieldToSchedule(ViewSchedule schedule, ElementId paramId)
        {
            ScheduleDefinition definition = schedule.Definition;

            // Find a matching SchedulableField
            SchedulableField schedulableField = definition.GetSchedulableFields().FirstOrDefault(sf => sf.ParameterId == paramId);

            if (schedulableField != null)
            {
                // Add the found field
                definition.AddField(schedulableField);
            }
        }

  

   

        public void CreateSingleCategoryScheduleWithGroupedColumnHeaders()
        {
            using (Transaction t = new Transaction(_doc, "Create single-category with grouped column headers"))
            {
                // Build the schedule
                t.Start();
                ViewSchedule vs = ViewSchedule.CreateSchedule(_doc, new ElementId(BuiltInCategory.OST_Windows));

                AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.WINDOW_HEIGHT));
                AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.WINDOW_WIDTH));
                AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.ALL_MODEL_MARK));
                AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.ALL_MODEL_COST));

                _doc.Regenerate();

                // Group the headers in the body section using ViewSchedule methods
                vs.GroupHeaders(0, 0, 0, 1, "Size");
                vs.GroupHeaders(0, 2, 0, 3, "Other");
                vs.GroupHeaders(0, 0, 0, 3, "All");

                t.Commit();
            }
        }

        private void CreateSubtitle()
        {
            var tableBody = vs.GetTableData().GetSectionData(SectionType.Body);
            tableBody.SetCellText(0, 0, "MY TEXT");



            TableData colTableData = vs.GetTableData();

            TableSectionData tsd = colTableData.GetSectionData(SectionType.Header);
            tsd.InsertRow(tsd.FirstRowNumber + 1);
            tsd.SetCellText(tsd.FirstRowNumber + 1, tsd.FirstColumnNumber, "Schedule of column top and base levels with offsets");

        }

        private void FormatSubtitle()
        {
            TableData colTableData = vs.GetTableData();

            TableSectionData tsd = colTableData.GetSectionData(SectionType.Header);

            int rowNumber = tsd.LastRowNumber;
            int columnNumber = tsd.LastColumnNumber;

            // Subtitle is second row, first column
            if (tsd.AllowOverrideCellStyle(tsd.FirstRowNumber, tsd.FirstColumnNumber))
            {
                TableCellStyle tcs = new TableCellStyle();
                TableCellStyleOverrideOptions options = new TableCellStyleOverrideOptions();
                options.FontSize = true;
                options.Bold = true;
                //  options.FontColor = true;
                options.BackgroundColor = true;
                tcs.SetCellStyleOverrideOptions(options);
                tcs.IsFontBold = true;
                tcs.TextSize = 20;
                tcs.BackgroundColor = new Color(255, 0, 0);
                //  tcs.TextColor = new Color(255, 0, 0);
                tsd.SetCellStyle(tsd.FirstRowNumber + 1, tsd.FirstColumnNumber, tcs);
            }
        }





    }
}