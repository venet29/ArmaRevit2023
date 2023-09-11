using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI.Events;

namespace ArmaduraLosaRevit
{


    #region Eventos de direccion de barra


    [Transaction(TransactionMode.ReadOnly)]
    public class UIEventBarraIzquierda : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            VariablesSistemas.m_ubicacionBarra = UbicacionLosa.Izquierda;
            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.ReadOnly)]
    public class UIEventBarraDerecho : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            VariablesSistemas.m_ubicacionBarra = UbicacionLosa.Derecha;
            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.ReadOnly)]
    public class UIEventBarraInferior : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            VariablesSistemas.m_ubicacionBarra = UbicacionLosa.Inferior;
            return Result.Succeeded;
        }
    }



    [Transaction(TransactionMode.ReadOnly)]
    public class UIEventBarraSuperior : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            VariablesSistemas.m_ubicacionBarra = UbicacionLosa.Superior;
            return Result.Succeeded;
        }
    }


    #endregion
}
