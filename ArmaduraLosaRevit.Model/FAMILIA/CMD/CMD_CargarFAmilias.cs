using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.FAMILIA.Varios;

namespace ArmaduraLosaRevit.Model.FAMILIA.CMD

{
    public class CMD_CargarFAmilias
    {


        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
        // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
        public class CargarFamiliasApp : IExternalCommand
        {

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                try
                {

                    ManejadorCargarFAmilias cargarFAmilias_carga = new ManejadorCargarFAmilias(commandData.Application);
                    cargarFAmilias_carga.DuplicarFamilasReBarBarv2();
                    
                    cargarFAmilias_carga.cargarFamilias_run();

                    // cambiar pelota de directriz 
                    PelotaDedirectriz PelotaDedirectriz = new PelotaDedirectriz(commandData.Application);
                    PelotaDedirectriz.Ejecutar();
                    TaskDialog.Show("ok", "Elementos cargados correctamente");


                }
                catch (Exception ex)
                {
                    string msj = ex.Message;
                }
                return Result.Succeeded;
            }

        }




    }




}
