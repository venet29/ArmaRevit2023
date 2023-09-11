using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using System.Diagnostics;
using System.Collections;
using ArmaduraLosaRevit.Model.Elementos_viga;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model;
//using planta_aux_C.Elemento_Losa;


namespace ArmaduraLosaRevit.Model.LosaArmadura.Geometria
{
    // CLASE QUE BUSCA LAS VIGAS EN LA VISTA (VIEW) ANALIZADA, OBTIENE SU GEOMETRIA
    // Y REDIBUJA LA VIGA CON SeparacicionRoom AL NIVEL DE VIEW

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Cmd_LosaGeometrias : IExternalCommand
    {


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            ISeleccionarLosaConMouse seleccionarLosaConMouse = new SeleccionarLosaConMouse(uiapp);
            LosaGeometrias LosaGeometrias = new LosaGeometrias(uiapp, seleccionarLosaConMouse);
            LosaGeometrias.Ejecutar();

            return Result.Succeeded;
        }
    }
}
