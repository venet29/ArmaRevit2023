﻿using ArmaduraLosaRevit.Model.WPFDefaul;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraV.WPFb
{
    /// <summary>
    /// This is an example of of wrapping a method with an ExternalEventHandler using a string argument.
    /// Any type of argument can be passed to the RevitEventWrapper, and therefore be used in the execution
    /// of a method which has to take place within a "Valid Revit API Context".
    /// </summary>
    public class EventHandlerWithStringArg : RevitEventWrapper<string>
    {
        /// <summary>
        /// The Execute override void must be present in all methods wrapped by the RevitEventWrapper.
        /// This defines what the method will do when raised externally.
        /// </summary>
        public override void Execute(UIApplication uiApp, string args)
        {
            // Do your processing here with "args"
            TaskDialog.Show("External Event", args);
        }
    }

    /// <summary>
    /// This is an example of of wrapping a method with an ExternalEventHandler using an instance of WPF
    /// as an argument. Any type of argument can be passed to the RevitEventWrapper, and therefore be used in
    /// the execution of a method which has to take place within a "Valid Revit API Context". This specific
    /// pattern can be useful for smaller applications, where it is convenient to access the WPF properties
    /// directly, but can become cumbersome in larger application architectures. At that point, it is suggested
    /// to use more "low-level" wrapping, as with the string-argument-wrapped method above.
    /// </summary>
    public class EventHandlerWithWpfArg : RevitEventWrapper<Ui_BarraV>
    {
        /// <summary>
        /// The Execute override void must be present in all methods wrapped by the RevitEventWrapper.
        /// This defines what the method will do when raised externally.
        /// </summary>
        public override void Execute(UIApplication uiApp, Ui_BarraV ui_BarraV)
        {
            // SETUP
     
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Methods_CrearBarra.M1_EjecutarRutinas(ui_BarraV, uiApp);

        }
    }

     
}
