using Autodesk.Revit.UI;
//using planta_aux_C.Elemento_Losa;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.Agrupar;
using ArmaduraLosaRevit.Model.BarraEstribo.WPFc;
using ArmaduraLosaRevit.Model.BarraEstriboV.WPFv;
using ArmaduraLosaRevit.Model.BarraEstriboP.WPFp;
using ArmaduraLosaRevit.Model.BarraV.WPFb;
using ArmaduraLosaRevit.Model.BarraAreaPath.WPFm;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.WPFEdB;
using ArmaduraLosaRevit.Model.Pasadas.WPF;
using ArmaduraLosaRevit.Model.BarraV.Copiar.Helper;
using ArmaduraLosaRevit.Model.BarraV.Copiar.wpf;
using ArmaduraLosaRevit.Model.BarraV.Copiar;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.RefuerzoSupleMuro.WPFp;
using ArmaduraLosaRevit.Model.BarraV.Borrar;
using ArmaduraLosaRevit.Model.GRIDS.NombreEje.WPFGrid;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using System.Linq;
using System;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.WPF;
using ArmaduraLosaRevit.Model.GRIDS.WPF_AgregraEJE;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_AgregarEjes : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            ManejadorWPF_AgregarEje _ManejadorWPF_AgregarEje = new ManejadorWPF_AgregarEje(commandData.Application);
            _ManejadorWPF_AgregarEje.Execute();
            return Result.Succeeded;

        }


    }
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_borrarElevaciones : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            BorrarRebarRectangulo.EjecutarElevaciones(commandData.Application, "todos");

            return Result.Succeeded;

        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_Ui_BarraV : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            ManejadorWPF_BarraV manejadorWPF = new ManejadorWPF_BarraV(commandData);
            return manejadorWPF.Execute();

        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_Ui_BarraV_Agrupar : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            ManejadorAgrupar ManejadorAgrupar = new ManejadorAgrupar(commandData.Application);
            return ManejadorAgrupar.M1_EjecutarVerticales();

        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_Ui_BarraV_DesAgrupar : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            ManejadorDesAgrupar ManejadorAgrupar = new ManejadorDesAgrupar(commandData.Application);
            return ManejadorAgrupar.Ejecutar();

        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_Ui_BarraVAutomatico : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            //ManejadorBarrasAutomaticas manejadorBarrasAutomaticas = new ManejadorBarrasAutomaticas(commandData.Application);

            //return (manejadorBarrasAutomaticas.EjecutarImportacion()==true?Result.Succeeded:Result.Cancelled) ;
            ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto.ManejadorWPF_AutoElev manejadorWPF = new ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto.ManejadorWPF_AutoElev(commandData);
            return manejadorWPF.Execute();

        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class cmd_Ui_MAlla : IExternalCommand
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian


        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            ManejadorWPF_Malla manejadorWPF_Malla = new ManejadorWPF_Malla(commandData);
            return manejadorWPF_Malla.Execute();

        }


    }
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_Ui_BarraEstriboC : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            ManejadorWPF_EstriboC manejadorWPF_EstriboC = new ManejadorWPF_EstriboC(commandData);
            return manejadorWPF_EstriboC.Execute();

        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_Ui_BarraEstriboP : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            ManejadorWPF_EstriboP manejadorWPF_EstriboP = new ManejadorWPF_EstriboP(commandData);
            return manejadorWPF_EstriboP.Execute();

        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_Ui_BarraEstriboV : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            //return Result.Succeeded;
            Debug.Print("probando debug");

            ManejadorWPF_EstriboV manejadorWPF_Estribov = new ManejadorWPF_EstriboV(commandData);
            return manejadorWPF_Estribov.Execute();

        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_editarBarraRebar : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            ManejadorWPF_EditarBarraLargo manejadorWPF_EstriboV = new ManejadorWPF_EditarBarraLargo(commandData);
            return manejadorWPF_EstriboV.Execute();

        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_CopiarElev : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            CArgar_CopiarElev_WPF.Ejecutar(commandData.Application);
            return Result.Succeeded;
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_RefuerzoSupleMuro : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            ManejadorWPF_RefuerzoSupleMuro manejadorWPF = new ManejadorWPF_RefuerzoSupleMuro(commandData);
            return manejadorWPF.Execute();

        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_ActualizarColoresElev : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {

            View _view = commandData.Application.ActiveUIDocument.ActiveView;
            SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(commandData.Application, _view);

            //   VisibilidadElement VisibilidadElement = new VisibilidadElemen(commandData.Application);


            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(commandData.Application, seleccionarRebar);
            ManejadorVisibilidad.M9_Restablecer_Color_BarrasElevacion();
            return Result.Succeeded;

        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_TextoEjesElev : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {


            ManejadorWPF_Ui_UIGrid manejadorWPF_EstriboV = new ManejadorWPF_Ui_UIGrid(commandData);
            return manejadorWPF_EstriboV.Execute();

        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_ConfiguracionesOcultarTraba : IExternalCommand
    {
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            try
            {
                UIApplication _uiapp = commandData.Application;
                var lista1 = _uiapp.GetRibbonPanels("Diseño Elevaciones").SelectMany(pa => pa.GetItems()).First(it => it.Name == "GridElev");


                string valorActual = lista1.ItemText;

                View _view = _uiapp.ActiveUIDocument.ActiveView;
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);

                //   VisibilidadElement VisibilidadElement = new VisibilidadElemen(commandData.Application);


                ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, seleccionarRebar);
                ManejadorVisibilidad.M10_Ocultar_Color_BarrasElevacion_traba_estrivovigaYLaterales((valorActual == "On"? false : true));
                lista1.ItemText = (valorActual=="Off" ? "On" : "Off");

                //TaskDialog.Show("ok", "Visibilidad" + lista1.ItemText);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_CrearHorquilla : IExternalCommand
    {
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            try
            {
                UIApplication _uiapp = commandData.Application;

                ManejadorWPF_Horquilla _manejadorWPF_Horquilla = new ManejadorWPF_Horquilla(_uiapp);
                return _manejadorWPF_Horquilla.Execute();

            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_GeneraPasadas : IExternalCommand
    {
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            try
            {

                Manejador_PASADASBIM_en_elevaciones.Ejecutar(commandData.Application);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // If there are something wrong, give error information and return failed
                message = ex.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
        }

    }

}
