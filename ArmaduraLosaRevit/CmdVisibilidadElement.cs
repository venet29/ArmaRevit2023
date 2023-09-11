using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.WPF;
using ArmaduraLosaRevit.Model.TablasSchedule;
using ArmaduraLosaRevit.Model.ViewFilter.WPF;

namespace ArmaduraLosaRevit
{


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdM1_MostrarPorOrientacion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM1_MostrarPorOrientacion(commandData.Application, commandData.View);

        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM1_MostrarPorOrientacion_H : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM1_MostrarPorOrientacion_H(commandData.Application, commandData.View);

        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM1_MostrarPorOrientacion_H_conPAthSym : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM1_MostrarPorOrientacion_MostarPAth(commandData.Application, commandData.View,AccionTipoBarraOrientacion.Horizontal);
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM1_MostrarPorOrientacion_V : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM1_MostrarPorOrientacion_V(commandData.Application, commandData.View);
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM1_MostrarPorOrientacion_V_conPAthSym : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM1_MostrarPorOrientacion_MostarPAth(commandData.Application, commandData.View, AccionTipoBarraOrientacion.Vertical);

        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM1_MostrarPorDireccion_1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM1_MostrarPorDireccion_1(commandData.Application, commandData.View);

        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM1_MostrarPorDireccion_2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM1_MostrarPorDireccion_2(commandData.Application, commandData.View);

        }


    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdM2_MostrarPAthPorDiametro : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            return ManejadorCmdVisibilidadElement.CmdM2_MostrarPAthPorDiametro(commandData.Application, commandData.View);
        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM3_MostrarPAthNormal : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return ManejadorCmdVisibilidadElement.CmdM3_MostrarPAthNormal(commandData.Application, commandData.View);

        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM4_MostrarPAthPorTipo : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Visualizacion Visualizacion = new Visualizacion();
            Visualizacion.ShowDialog();

            if (Visualizacion.DialogResult.HasValue && Visualizacion.DialogResult.Value)
            {
                TiposBarrasDTo tiposBarrasDTo = new TiposBarrasDTo() { tipofx = Visualizacion.fx, tiposx = Visualizacion.sx };

                SeleccionarPathReinfomentVisibilidad SeleccionarPathReinfomentVisibilidad = new SeleccionarPathReinfomentVisibilidad(commandData.Application, commandData.View);
                VisibilidadElementBase VisibilidadElement = new VisibilidadElementTipoBarra(commandData.Application, tiposBarrasDTo);

                ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(commandData.Application, VisibilidadElement, SeleccionarPathReinfomentVisibilidad);
                ManejadorVisibilidad.M4_CAmbiar_PorTipo();
            }

            return Result.Succeeded;

        }


    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM5_OcultarBArrasQueNoSeanVista : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            View _view = commandData.Application.ActiveUIDocument.ActiveView;
            ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(commandData.Application);
            _ManejadorVisibilidadElemenNoEnView.Ejecutar(_view, _view.Name);

            return Result.Succeeded;
        }


    }


    public class CmdM5_OcultarBArrasQueNoSeanVista_auxiliar
    {
        public Result Execute(UIApplication uiapp)
        {
            View _view = uiapp.ActiveUIDocument.ActiveView;
            ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(uiapp);
            _ManejadorVisibilidadElemenNoEnView.Ejecutar(_view, _view.Name);

            return Result.Succeeded;
        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM6_ActualizarNameVista : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

                ManejadorVisibilidadActualizarNombreVista _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadActualizarNombreVista(commandData.Application);
            _ManejadorVisibilidadElemenNoEnView.Ejecutar();

            return Result.Succeeded;

        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM7_FormatoParaEntrega : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            ManejadorWPF_FormatoEntrega _ManejadorWPF_BlancoNegro = new ManejadorWPF_FormatoEntrega(commandData.Application);
            return _ManejadorWPF_BlancoNegro.Execute();
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CmdM8_CreadorFiltro3D : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            ManejadorWPF_Filtro3D _ManejadorWPF_Filtro3D = new ManejadorWPF_Filtro3D(commandData.Application);
            return _ManejadorWPF_Filtro3D.Execute();
        }
    }

}