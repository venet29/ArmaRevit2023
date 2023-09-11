using System;
using System.Linq;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.TablasSchedule.Factory;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    internal class ScheduleNH
    {
        private UIApplication _uiapp;
        private Document _doc;
        private ViewSchedule vs;
#pragma warning disable CS0169 // The field 'ScheduleNH.vs_resumen' is never used
        private ViewSchedule vs_resumen;
#pragma warning restore CS0169 // The field 'ScheduleNH.vs_resumen' is never used

        public ScheduleNH(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _doc = uiapp.ActiveUIDocument.Document;
        }

//        public void crear_CubicacionBarras()
//        {
//#pragma warning disable CS0219 // The variable 'agregarFiltro' is assigned but its value is never used
//            bool agregarFiltro = false;
//#pragma warning restore CS0219 // The variable 'agregarFiltro' is assigned but its value is never used
//            // PrteCAlcualo();
//            //CreateSingleCategoryScheduleWithGroupedColumnHeaders();
//            if (!CreateSingleCategorySchedule()) return;

//            //Editar_schemaGEneral();


//            if (!CreateSingleCategoryScheduleResumen()) return;

//          //  Editar_schemaResumen();

      
//        }

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

        private bool CreateSingleCategorySchedule()
        {
            try
            {


                View viewactualDepen = TiposView.ObtenerTiposView("Cubicacion Barras", _doc);
                if (viewactualDepen != null)
                {
                    Util.ErrorMsg($"Schedule 'Cubicacion Barras' ya esta creada ");
                    return false;
                }

                using (Transaction t = new Transaction(_doc, "CrearScheduleCubicacionBarras-NH"))
                {
                    t.Start();

                    // Create schedule
                    vs = ViewSchedule.CreateSchedule(_doc, new ElementId(BuiltInCategory.OST_Rebar));
                    vs.Name = "Cubicacion Barras";
                    _doc.Regenerate();


                    var listaParameter = AyudaBuscaParametrerShared.ObtenerListaPArameterShare(_doc);
                    // var BarraTipoObj= listaParameter.Where(c => c.Key == "BarraTipo").FirstOrDefault();

                    var BarraTipoObj = listaParameter.TryGetValue("BarraTipo", out var value) ? value : ElementId.InvalidElementId;
                    if (BarraTipoObj.IsInvalid() == true)
                    {
                        Util.ErrorMsg($"Schedule-> Error al obtener 'BarraTipo'");
                        return false;
                    }

                    var NombreVistaObj = listaParameter.TryGetValue("NombreVista", out var valueNombreVista) ? valueNombreVista : ElementId.InvalidElementId;
                    if (NombreVistaObj.IsInvalid() == true)
                    {
                        Util.ErrorMsg($"Schedule-> Error al obtener 'NombreVista'");
                        return false;
                    }

                    var NombreVistaOPesoBarrabj = listaParameter.TryGetValue("PesoBarra", out var valuePesoBarra) ? valuePesoBarra : ElementId.InvalidElementId;
                    if (NombreVistaOPesoBarrabj.IsInvalid() == true)
                    {
                        Util.ErrorMsg($"Schedule-> Error al obtener 'PesoBarra'");
                        return false;
                    }
  
                    // Add fields to the schedule
                    AddRegularFieldToSchedule(vs, BarraTipoObj);
                    AddRegularFieldToSchedule(vs, NombreVistaObj);
                    // AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.REBAR_ELEM_BAR_SPACING));
                    AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS));
                    AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.REBAR_ELEM_LENGTH));
                    AddRegularFieldToSchedule(vs, new ElementId(BuiltInParameter.REBAR_BAR_DIAMETER));

                    AddRegularFieldToSchedule(vs, NombreVistaOPesoBarrabj);




                    //agergar contador
                    ScheduleDefinition definition = vs.Definition;
                    definition.ShowGrandTotal = true;
                    definition.ShowGrandTotalTitle = true;
                    definition.ShowGrandTotalCount = true;


                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear 'CrearScheduleCubicacionBarras' ex:{ ex.Message}");
                return false;
            }

            return true;
        }

        //gropu  
        //https://twentytwo.space/2021/05/02/revit-api-schedule-creation/

        private bool CreateSingleCategoryScheduleResumen()
        {
            try
            {


                View viewactualDepen = TiposView.ObtenerTiposView("Cubicacion Barras Resumen", _doc);
                if (viewactualDepen != null)
                {
                    Util.ErrorMsg($"Schedule 'Cubicacion Barras' ya esta creada ");
                    return false;
                }

                using (Transaction t = new Transaction(_doc, "CrearScheduleCubicacionBarras-NH"))
                {
                    t.Start();

                    // Create schedule
                    vs = ViewSchedule.CreateSchedule(_doc, new ElementId(BuiltInCategory.OST_Rebar));
                    vs.Name = "Cubicacion Barras Resumen";
                    _doc.Regenerate();


                    var listaParameter = AyudaBuscaParametrerShared.ObtenerListaPArameterShare(_doc);
                    // var BarraTipoObj= listaParameter.Where(c => c.Key == "BarraTipo").FirstOrDefault();

                    var BarraTipoObj = listaParameter.TryGetValue("BarraTipo", out var value) ? value : ElementId.InvalidElementId;
                    if (BarraTipoObj.IsInvalid() == true)
                    {
                        Util.ErrorMsg($"Schedule-> Error al obtener 'BarraTipo'");
                        return false;
                    }

    

                    var NombreVistaOPesoBarrabj = listaParameter.TryGetValue("PesoBarra", out var valuePesoBarra) ? valuePesoBarra : ElementId.InvalidElementId;
                    if (NombreVistaOPesoBarrabj.IsInvalid() == true)
                    {
                        Util.ErrorMsg($"Schedule-> Error al obtener 'PesoBarra'");
                        return false;
                    }

                    // Add fields to the schedule
                    AddRegularFieldToSchedule(vs, BarraTipoObj);

                    AddRegularFieldToSchedule(vs, NombreVistaOPesoBarrabj);

                    //agergar contador
                    ScheduleDefinition definition = vs.Definition;

                    //obteher columnas 
                    ScheduleField field = definition.GetField(0);
                    ScheduleSortGroupField _fam = new ScheduleSortGroupField(field.FieldId,ScheduleSortOrder.Ascending);
                    definition.AddSortGroupField(_fam);
                    definition.SetSortGroupField(_fam.FieldId.IntegerValue, _fam);


                    definition.ShowGrandTotal = true;
                    definition.ShowGrandTotalTitle = true;
                    definition.ShowGrandTotalCount = true;
                    definition.IsItemized = false;

                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear 'CrearScheduleCubicacionBarras' ex:{ ex.Message}");
                return false;
            }

            return true;
        }


        /// Adds a single parameter field to the schedule
        /// 

        private void AddRegularFieldToSchedule(ViewSchedule schedule, ElementId paramId)
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

        //private bool Editar_schemaGEneral()
        //{
        //    try
        //    {
        //        using (Transaction t = new Transaction(_doc, "editar shedule"))
        //        {
        //            t.Start();
        //            // CreateSubtitle();


        //            customizar();
        //            //FormatSubtitle();
        //            FormatoDeColumnas();
        //            t.Commit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.DebugDescripcion(ex);
        //        return false;
        //    }
        //    return true;
        //}


        //private bool Editar_schemaResumen()
        //{
        //    try
        //    {
        //        using (Transaction t = new Transaction(_doc, "editar shedule resumen"))
        //        {
        //            t.Start();
        //            ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_ResumenDTO_2020BAjo.ObtenerDtoPesoBarra());
        //            t.Commit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Util.DebugDescripcion(ex);
        //        return false;
        //    }
        //    return true;
        //}
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

        private void customizar()
        {
            
            vs.GroupHeaders(0, 0, 0, 1, "NombreVista");
            // Get header section
            TableSectionData data = vs.GetTableData().GetSectionData(SectionType.Header);
            if (data == null) return;

            int rowNumber = data.LastRowNumber;
            int columnNumber = data.LastColumnNumber;

            // Get the overall width of the table so that the new columns can be resized properly
            double tableWidth = data.GetColumnWidth(columnNumber);

            data.InsertColumn(columnNumber);
            data.InsertColumn(columnNumber);

            // Refresh data to be sure that schedule is ready for text insertion
            vs.RefreshData();

            //Set text to the first header cell
            data.SetCellText(rowNumber, data.FirstColumnNumber, "(Bar Diameter / 1.273 cm)^2*Bar Length / 100 cm * Quantity");

            // Set width of first column
            data.SetColumnWidth(data.FirstColumnNumber, tableWidth / 3.0);

            //Set a different parameter to the second cell - the project name
            data.SetCellParamIdAndCategoryId(rowNumber, data.FirstRowNumber + 1, new ElementId(BuiltInParameter.PROJECT_NAME), new ElementId(BuiltInCategory.OST_ProjectInformation));
            data.SetColumnWidth(data.FirstColumnNumber + 1, tableWidth / 3.0);

            //Set the third column as the schedule view name - use the special category for schedule parameters for this
            data.SetCellParamIdAndCategoryId(rowNumber, data.LastColumnNumber, new ElementId(BuiltInParameter.VIEW_NAME),new ElementId(BuiltInCategory.OST_ScheduleViewParamGroup));
            data.SetColumnWidth(data.LastColumnNumber, tableWidth / 3.0);
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



        //private void FormatoDeColumnas()
        //{
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoNombreVista());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoCantiadad());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoLArgo());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoDiamtro());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoPesoBarra());

        //}

    }
}