using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Tipos
{
    internal class CubicacionBarras : ScheduleNHBase
    {
        private readonly UIApplication _uiapp;
        private ViewSchedule vs;
        private Document _doc;
        private string _nombreschedule;

        public CubicacionBarras(UIApplication _uiapp, string nombreShedule) : base(_uiapp)
        {
            this._uiapp = _uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _nombreschedule = nombreShedule;
        }

        public bool CrearSchedule_CubicacionBarras()
        {
            try
            {
                // PrteCAlcualo();
                //CreateSingleCategoryScheduleWithGroupedColumnHeaders();
                if (!CreateSingleCategorySchedule()) return false;
                Editar_schemaGEneral();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear Schedule ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool agregarFiltro_CubicacionBarras(string tipo,object value, ScheduleFilterType _ScheduleFilterType, ElementId elId= null)
        {

            
            try
            {
                ScheduleFiltrosDTO _ScheduleFiltrosDTO = new ScheduleFiltrosDTO() {
                 nombrePArameter=   "BarraTipo",
                  valor= "LOSA_INF",
                  _ScheduleFilterType=ScheduleFilterType.Equal
                };

                ScheduleFiltrosDTO _ScheduleFiltrosDTO2 = new ScheduleFiltrosDTO()
                {
                    nombrePArameter = tipo,
                    valor = value,
                    _ScheduleFilterType = _ScheduleFilterType,
                    ElementIdPAra = elId

                };

                List<ScheduleFiltrosDTO> Lista_ScheduleFiltrosDTO = new List<ScheduleFiltrosDTO>();
                Lista_ScheduleFiltrosDTO.Add(_ScheduleFiltrosDTO2);

                ScheduleFiltros _ScheduleFiltros = new ScheduleFiltros(_uiapp, vs);
                _ScheduleFiltros.Ejecutar(Lista_ScheduleFiltrosDTO);

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al crear Schedule ex:{ex.Message}");
                return false;
            }
            return true;
        }


        private bool CreateSingleCategorySchedule()
        {
            try
            {


                View viewactualDepen = TiposView.ObtenerTiposView(_nombreschedule, _doc);
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
                    vs.Name = _nombreschedule;
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

        private bool Editar_schemaGEneral()
        {
            try
            {
                using (Transaction t = new Transaction(_doc, "editar shedule"))
                {
                    t.Start();
                    // CreateSubtitle();
                    customizar();

                    FormatoDeColumnas_2021Arriba();
                    //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                    //    FormatoDeColumnas_2021Arriba(); 
                    //else
                    //    FormatoDeColumnas_2020Bajo();

                    //FormatSubtitle();

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
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
            data.SetCellParamIdAndCategoryId(rowNumber, data.LastColumnNumber, new ElementId(BuiltInParameter.VIEW_NAME), new ElementId(BuiltInCategory.OST_ScheduleViewParamGroup));
            data.SetColumnWidth(data.LastColumnNumber, tableWidth / 3.0);
        }

        private void FormatoDeColumnas_2021Arriba()
        {
            ScheduleFormato_2021Arriba.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2021Arriba.ObtenerDtoNombreVista());
            ScheduleFormato_2021Arriba.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2021Arriba.ObtenerDtoCantiadad());
            ScheduleFormato_2021Arriba.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2021Arriba.ObtenerDtoLArgo());
            ScheduleFormato_2021Arriba.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2021Arriba.ObtenerDtoDiamtro());
            ScheduleFormato_2021Arriba.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2021Arriba.ObtenerDtoPesoBarra());

        }

        //private void FormatoDeColumnas_2020Bajo()
        //{
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoNombreVista());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoCantiadad());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoLArgo());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoDiamtro());
        //    ScheduleFormato_2020BAjo.ApplyFormattingToFieldSinTras(vs, FactoryScheduleFormato_generalDTO_2020Bajo.ObtenerDtoPesoBarra());

        //}
    }
}
