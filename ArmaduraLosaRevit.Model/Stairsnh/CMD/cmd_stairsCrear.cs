using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Stairsnh.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Stairsnh.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;

namespace ArmaduraLosaRevit.Model.Stairsnh.CMD
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_ManejadorCreadorStair : IExternalCommand
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
            SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(commandData.Application);
            var listanivles = _seleccionarNivel.M2_ObtenerListaNivelOrdenadoPorElevacion(commandData.Application.ActiveUIDocument.ActiveView);
            CreadorStairInfo creadorStair = new CreadorStairInfo(commandData.Application);
            creadorStair.CreateStairs(listanivles[4], listanivles[6]);

            return Result.Succeeded;

        }

    }




 //   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_ElementosStair : IExternalCommand
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
            SeleccionarEscalera _seleccionarEscalera = new SeleccionarEscalera(commandData.Application);
            Stairs staris = _seleccionarEscalera.SeleccionarStairs(ObjectType.Element);

            if (staris != null)
            {
                ComponentStairs _componentStairs = new ComponentStairs(commandData.Application);
                _componentStairs.M2_GetStairInfo(staris);
                _componentStairs.M4_GetStairsType(staris);
                _componentStairs.M3_GetRunType(staris);
            }

            return Result.Succeeded;

        }

    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_GeometriaStair : IExternalCommand
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
            SeleccionarEscalera _seleccionarEscalera = new SeleccionarEscalera(commandData.Application);

            Stairs staris = _seleccionarEscalera.SeleccionarStairs(ObjectType.Element);

            //   Stairs staris = _seleccionarEscalera.SeleccionarStairs(ObjectType.Face);

            if (staris != null)
            {
                GeometrisStair _geometrisStair = new GeometrisStair(commandData.Application);
                _geometrisStair.M1_AsignarGeometriaObjecto(staris);

            }

            return Result.Succeeded;

        }

    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_GeometriaStairMaxArea : IExternalCommand
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
            SeleccionarEscalera _seleccionarEscalera = new SeleccionarEscalera(commandData.Application);

            Stairs staris = _seleccionarEscalera.SeleccionarStairs(ObjectType.Element);

            //buscar panarface de menor 
            if (staris != null)
            {
                GeometrisStairAreaMax _geometrisStairMaxArea = new GeometrisStairAreaMax(commandData.Application);
                //_geometrisStair.M1_GetGEom(staris);
                _geometrisStairMaxArea.M1_GetGEomPlanarFaceMAxiamaArea(staris);
            }         
            return Result.Succeeded;
        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_GeometriaStairMaxAreaConPtos : IExternalCommand
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
            Document _doc = commandData.Application.ActiveUIDocument.Document;
            ///a)buscar escalra con pto
            View3D elem3d = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return Result.Cancelled;
            }

            XYZ PtoBusuqedaInicial = new XYZ(-15.40, -3.034, 26.25);

            BuscarEscaleraHorizontalmente _buscarEscaleraHorizontalmente = new BuscarEscaleraHorizontalmente(commandData.Application);
            Stairs staris = _buscarEscaleraHorizontalmente.buscarEscaleraHorizontal(elem3d, PtoBusuqedaInicial, new XYZ(1, 0, 0), false);
            
            if (staris == null) return Result.Failed;
            
            //b)buscar panarface de menor 
            GeometrisStairAreaMax _geometrisStairMaxArea = new GeometrisStairAreaMax(commandData.Application);
            //_geometrisStair.M1_GetGEom(staris);
            _geometrisStairMaxArea.M1_GetGEomPlanarFaceMAxiamaArea(staris);

            PlanarFace faceInferior = _geometrisStairMaxArea.GetPlanarFaceMaxArea();
            if (faceInferior == null) return Result.Failed;
           
            //c) obtener espesor de losa
            //faltaImplementar

            //d) proyectar pto
            BuscarPtoProyeccionEnEscalera buscarPtbProyeccionEnEscalera = new BuscarPtoProyeccionEnEscalera(commandData.Application, faceInferior);
            buscarPtbProyeccionEnEscalera.BuscarProyeccionEnCaraInSuperior(PtoBusuqedaInicial, Util.CmToFoot(15));

            //e) crear vector SOLO TEMA VISUAL
            CrearLIneaAux CrearLIneaAux = new CrearLIneaAux(_doc);
            CrearLIneaAux.CrearLinea( PtoBusuqedaInicial.AsignarZ(buscarPtbProyeccionEnEscalera.PtoProyectadoCaraSuperior.Z)
                                                                , buscarPtbProyeccionEnEscalera.PtoProyectadoCaraSuperior);

            return Result.Succeeded;

        }
    }
}
