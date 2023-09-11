using Autodesk.Revit.UI;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.FAMILIA.Borrar;

namespace ArmaduraLosaRevit.Model.FAMILIA.CMD

{
    public class CMD_BorrarFamilias
    {


        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
        // [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
        public class CMD_BorrarFamiliasApp : IExternalCommand
        {

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                try
                {

                    BorrarFamilia borrarFamilia = new BorrarFamilia(commandData.Application);
                    borrarFamilia.BorrarTodasLasFamilias();
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
