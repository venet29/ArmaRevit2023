using Autodesk.Revit.UI;
//using planta_aux_C.Elemento_Losa;
using System.Diagnostics;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.EditarTipoPath.WPF;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.EditarTipoPath;
using ArmaduraLosaRevit.Model.Niveles.WPF;
//using ArmaduraLosaRevit.Model.AnalisisRoom;
namespace ArmaduraLosaRevit
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdEditarPathRein_datos : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            CargarEditPathReinforme _CargarPathReinforme = new CargarEditPathReinforme(commandData, TabEditarPath.Datos);
            return _CargarPathReinforme.Cargar();

        }


    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdEditarPathRein_forma : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            CargarEditPathReinforme _CargarPathReinforme = new CargarEditPathReinforme(commandData, TabEditarPath.Forma);
            return _CargarPathReinforme.Cargar();

        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CmdCrearNivelesEntre : IExternalCommand
    {
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            ManejadorWPF_Ui_Nivel _ManejadorWPF_Ui_Nivel = new ManejadorWPF_Ui_Nivel(commandData.Application);
            return _ManejadorWPF_Ui_Nivel.Execute();
        }


    }
}
