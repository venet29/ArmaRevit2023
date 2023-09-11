using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ParametrosShare.Cambiar;
using System;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;

namespace ArmaduraLosaRevit.Model.ParametrosShare
{
    ///// Implements the Revit add-in interface IExternalCommand
    ///// </summary>
    //[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    //[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]

    //public class cmd_DefinicionManejador2 : IExternalCommand
    //{
    //    /// <summary>
    //    ///         /// Add two shared parameters to the rebar category instance elements:
    //    /// Updated: is used to start the regeneration
    //    /// CurveElementId: is used to store the id of a model curve
    //    /// </summary>
    //    /// <param name="commandData"></param>
    //    /// <param name="message"></param>
    //    /// <param name="elements"></param>
    //    /// <returns></returns>
    //    public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {
    //        try
    //        {
    //            Document doc = commandData.Application.ActiveUIDocument.Document;
    //            if (doc == null)
    //                return Result.Failed;

    //            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(commandData.Application, "ParametrosNH");

    //            using (Transaction tran = new Transaction(doc, "Add shared param"))
    //            {
    //                tran.Start();

    //                // _definicionManejador.Ejecutar();

    //                EntidadDefinition _entidadDefinition1 = new EntidadDefinition(commandData.Application, BuiltInCategory.OST_Rebar, ParameterType.YesNo, "Updated", "RebarTestParamGroup", false, false, true, "");
    //                EntidadDefinition _entidadDefinition2 = new EntidadDefinition(commandData.Application, BuiltInCategory.OST_Rebar, ParameterType.Integer, "CurveElementId", "RebarTestParamGroup", true, false, true, "");

    //                if (_definicionManejador.AddSharedTestParameter(_entidadDefinition1) && _definicionManejador.AddSharedTestParameter(_entidadDefinition2))
    //                {
    //                    tran.Commit();
    //                    return Result.Succeeded;
    //                }
    //                tran.RollBack();
    //                return Result.Failed;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Util.ErrorMsg(ex.Message);
    //            return Result.Failed;
    //        }
    //    }
    //}


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_DefinicionManejador_datosLOsas : IExternalCommand
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
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(commandData.Application, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarLosa()) return Result.Failed;
            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_DefinicionManejador_datosLOsElev : IExternalCommand
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
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(commandData.Application, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarElevacion()) return Result.Failed;
            return Result.Succeeded;
        }
    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_DefinicionManejador_datosLOsView : IExternalCommand
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
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(commandData.Application, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarView()) return Result.Failed;
            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_DefinicionManejador_CREARLISTA : IExternalCommand
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
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(commandData.Application, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.M1_ShareCrearLista()) return Result.Failed;
            return Result.Succeeded;
        }
    }





    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]

    public class cmd_DefinicionManejador_cambiarParametro : IExternalCommand
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
                UIDocument _uidoc = commandData.Application.ActiveUIDocument;
                Document doc = commandData.Application.ActiveUIDocument.Document;
                if (doc == null)
                    return Result.Failed;


                CambiarPArametro CambiarPArametro = new CambiarPArametro();
                CambiarPArametro.ShowDialog();

                if (CambiarPArametro.IsOK)
                {

                    try
                    {
                        Reference _ElementsRebarSeleccionado = _uidoc.Selection.PickObject(ObjectType.Element, "Seleccionar barra (Rebar)");


                        if (_ElementsRebarSeleccionado == null) return Result.Failed;
                        //obtiene una referencia floor con la referencia r
                        Element Element_pickobject_element = _uidoc.Document.GetElement(_ElementsRebarSeleccionado.ElementId);


                        using (Transaction tran = new Transaction(doc, "cambiar parametro"))
                        {
                            tran.Start();
                            ParameterUtil.SetParaInt(Element_pickobject_element, CambiarPArametro.nombreparametro, CambiarPArametro.valor);
                            tran.Commit();
                        }
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {
                        return Result.Failed;
                    }
                }


                return Result.Succeeded;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return Result.Failed;
            }
        }
    }


  



}
