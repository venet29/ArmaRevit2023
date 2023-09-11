
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;


namespace ArmaduraLosaRevit.Model.Prueba
{
     public  class CargarBarraRoom2
    {
        public CargarBarraRoom2()
        {

        }
        //public static BarraRoom _newBarralosa { get; private set; }

        //public  Result Cargar(ExternalCommandData commandData, string tipobarra, UbicacionLosa ubicacion,bool _IsTest=false)
        //{
        //    TaskDialog.Show("ERRRO","SDFSDF");
        ////    //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
        //    try
        //    {
           
        // //       commandData.Application.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(utilfallas.ProcesamientoFallas);

        //        _newBarralosa = new BarraRoom(commandData.Application, tipobarra, ubicacion, false);   //f1_SUP

        //        if (_newBarralosa.statusbarra == Result.Succeeded)
        //        {
        //            // = newBvearralosa.Angle_1;nhsnhsnhsnhs
        //            _newBarralosa.CrearBarra(_newBarralosa.CurvesPathreiforment,
        //                                    _newBarralosa.LargoPathreiforment,
        //                                    _newBarralosa.nombreSimboloPathReinforcement,
        //                                    _newBarralosa.diametroEnMM,
        //                                    _newBarralosa.Espaciamiento,
        //                                    XYZ.Zero);

        //            if (_newBarralosa.statusbarra != Result.Succeeded)
        //            {

        //                _newBarralosa.message = "Error al crea barras";
        //                TaskDialog.Show("Error", _newBarralosa.message);
        //                return Result.Succeeded;
        //            }

        //            //crea barra a izq-inf y dere-sup
        //            if ((tipobarra == "f1_SUP") ||
        //                (tipobarra == "fauto" && (_newBarralosa.TipoBarra_izq_Inf != "" || _newBarralosa.TipoBarra_dere_sup != "")))
        //            {
        //                _newBarralosa.BuscarDireccion_F1SUP();
        //                _newBarralosa.CrearBarraExtremos();
        //            }

        //        }
        //        else
        //        {

        //            _newBarralosa.message = "Error al crea barras";
        //            TaskDialog.Show("Error", _newBarralosa.message);

        //        }

        //   //     commandData.Application.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(utilfallas.ProcesamientoFallas);
        //        return Result.Succeeded;
        //    }
        //    catch (Exception ex)
        //    {
        ////        commandData.Application.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(utilfallas.ProcesamientoFallas);
        //        // If there are something wrong, give error information and return failed

        //        TaskDialog.Show("Error", "Error al crear barra");
        //        return Autodesk.Revit.UI.Result.Failed;
        //    }

        //}


    }
}
