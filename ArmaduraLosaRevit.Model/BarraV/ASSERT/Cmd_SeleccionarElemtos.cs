#region Namespaces
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Enumeraciones;
#endregion // Namespaces


namespace ArmaduraLosaRevit.Model.BarraV.ASSERT
{


    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd_SeleccionarElemtos : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {


            SeleccionarElementosV _seleccionarNivel = new SeleccionarElementosV(commandData.Application);

            _seleccionarNivel.M1_ObtenerPtoinicio();
            DireccionRecorrido _DireccionRecorrido = new DireccionRecorrido(commandData.Application.ActiveUIDocument.ActiveView, DireccionRecorrido_.PerpendicularEntradoVista);
            var resul = _seleccionarNivel.M5_OBtenerElementoREferenciaDTO(_DireccionRecorrido);
            //ConstantesGenerales.sbLog.AgregarListaPtos(ListaPtosPerimetroBarras, "ListaPtosPerimetroBarras");
            ConstNH.sbLog.AppendLine($"resul: ptoFAceMuroInicial: {resul.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost} \nespesorElemeto:{ resul.EspesorElemetoHost}\nDireccionEnFierrado:{resul.DireccionEnFierrado}\nDireccionPerpendicularElemeto:  {resul.NormalEntradoView}\n elemetoID:{resul.IdelementoContenedor}");
#if DEBUG
            LogNH.Guardar_registro_ConStringBuilder(ConstNH.sbLog, ConstNH.rutaLogNh);
#endif 


            return Result.Succeeded;
            
        }
    }
}

