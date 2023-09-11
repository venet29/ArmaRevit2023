using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.TablasSchedule.Factory;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Tipos
{
    internal class CubicacionBarrasResumen : ScheduleNHBase
    {
        private readonly UIApplication _uiapp;
        private ViewSchedule vs;
        private Document _doc;
        private string _nombreschedule;

        public CubicacionBarrasResumen(UIApplication _uiapp, string nombreShedule) :base(_uiapp)
        {
            this._uiapp = _uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _nombreschedule = nombreShedule;
        }

        public bool CrearSchedule_CubicacionBarrasResumen()
        {

            try
            {
                if (!CreateSingleCategoryScheduleResumen()) return false;

                Editar_schemaResumen_2021Arriba();
                //if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                //    Editar_schemaResumen_2021Arriba();
                //else
                //    Editar_schemaResumen_2020Bajo();

                //Util.InfoMsg($"Schedule 'cubiacion de barras' creado");
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al crear Schedule ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private bool CreateSingleCategoryScheduleResumen()
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
                    ScheduleSortGroupField _fam = new ScheduleSortGroupField(field.FieldId, ScheduleSortOrder.Ascending);
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
        private bool Editar_schemaResumen_2021Arriba()
        {
            try
            {
                using (Transaction t = new Transaction(_doc, "editar shedule resumen"))
                {
                    t.Start();
                    ScheduleFormato_2021Arriba.ApplyFormattingToFieldSinTras(vs, FactoryScheFormato_ResumenDTO_2021Arriba.ObtenerDtoPesoBarra());
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
        //private bool Editar_schemaResumen_2020Bajo()
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


    }
}
