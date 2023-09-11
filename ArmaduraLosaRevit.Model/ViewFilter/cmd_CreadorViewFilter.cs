using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ParametrosShare.Cambiar;
using System;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using ArmaduraLosaRevit.Model.ViewFilter;

namespace ArmaduraLosaRevit.Model.ParametrosShare
{
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]

    public class cmd_CreadorViewFilter : IExternalCommand
    {
        /// <summary>
        ///         /// Add two shared parameters to the rebar category instance elements:
        /// Updated: is used to start the regeneration
        /// CurveElementId: is used to store the id of a model curve
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //Document doc = commandData.Application.ActiveUIDocument.Document;
                CreadorViewFilter _CreadorViewFilter = new CreadorViewFilter(commandData.Application);
                _CreadorViewFilter.M2_CreateViewFilter(commandData.Application.ActiveUIDocument.ActiveView);
                //_CreadorViewFilter.CreateViewFilter_manual(commandData.Application.ActiveUIDocument.ActiveView);
                return Result.Succeeded;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg(ex.Message);
                return Result.Failed;
            }
        }
    }


    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class cmd_CreadorViewFilterAllView : IExternalCommand
    {
        /// <summary>
        ///         /// Add two shared parameters to the rebar category instance elements:
        /// Updated: is used to start the regeneration
        /// CurveElementId: is used to store the id of a model curve
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //Document doc = commandData.Application.ActiveUIDocument.Document;
                CreadorViewFilter _CreadorViewFilter = new CreadorViewFilter(commandData.Application);
                _CreadorViewFilter.M1_CreateViewFilterEnTodasVistas();
                // _CreadorViewFilter.CreateViewFilter_manual(commandData.Application.ActiveUIDocument.ActiveView);
                return Result.Succeeded;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg(ex.Message);
                return Result.Failed;
            }
        }
    }

}
