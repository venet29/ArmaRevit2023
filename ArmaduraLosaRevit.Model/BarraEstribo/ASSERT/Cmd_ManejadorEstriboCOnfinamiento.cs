#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion;
using ArmaduraLosaRevit.Model.ConfiguracionInicial;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;
using System;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraEstribo.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_ManejadorEstriboCOnfinamiento : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication _uiapp = commandData.Application;


            try
            {

                Document _doc = commandData.Application.ActiveUIDocument.Document;
                var view3D = TiposFamilia3D.Get3DBuscar(_doc);
                if (view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial");
                    return Result.Failed;
                }

                //1)datos de form
                DatosConfinamientoAutoDTO configuracionInicialEstriboDTO =
                    new DatosConfinamientoAutoDTO()
                    {
                        DiamtroEstriboMM = 8,
                        espaciamientoEstriboCM = 20,
                        cantidadEstribo = "E.",
                        tipoConfiguracionEstribo = TipoConfiguracionEstribo.EstriboViga
                    };

                ManejadorEstriboCOnfinamiento manejadorEstriboCOnfinamiento = new ManejadorEstriboCOnfinamiento(commandData.Application, configuracionInicialEstriboDTO);
                manejadorEstriboCOnfinamiento.M1_Ejecutar();


            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");

            }


            return Result.Succeeded;

        }
    }

  


}


