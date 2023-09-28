using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Windows.Forms;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES.FormFallasDialogo;
using Autodesk.Revit.DB.Events;
using System.Windows.Documents;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.UTILES
{



    /// <summary>
    /// Description of UtilitarioFallasAdvertencias.
    /// </summary>
    public class UtilitarioFallasAdvertencias_static
    {
        private static List<TipoFallaAdvertercias> ListaFalla;
        public static bool IsSalir;
        private static string IdCOnError;

        public static void resetVaribles()
        {
            // Util.InfoMsg("Resetear UtilitarioFallasAdvertencias_static");
            ListaFalla = new List<TipoFallaAdvertercias>();
            IsSalir = false;
        }
        public static void ProcesamientoFallas(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
          
            FailuresAccessor failuresAccessor = e.GetFailuresAccessor();
            var proce = e.GetProcessingResult();
            
            string transactionName = failuresAccessor.GetTransactionName();
            System.Diagnostics.Debug.Print("Nombre Transaction: " + transactionName);

            IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();

            if (fmas.Count == 0)
            {
                // FailureProcessingResult.Continue SI ES UNA FALLA QUE NO GENERA MENSAJES SE DEJA CONTINUAR EL CICLO
                e.SetProcessingResult(FailureProcessingResult.Continue);
                return;
            }

            var listaErrore = fmas.ToList();

       
   

            foreach (FailureMessageAccessor failure in fmas)
            {
                TipoFallaAdvertercias FallaActual = new TipoFallaAdvertercias(failure);

                if(false) // desactivado  obtener Elemento que genera error, dificil obtener
                    ObtenerElementoDeError(sender, failure);

                ObtenerMensajeError(failure);

                var Existe = ListaFalla.Where(c => c.FailID.Guid == failure.GetFailureDefinitionId().Guid).FirstOrDefault();

                if (Existe == null)
                {
                    ListaFalla.Add(FallaActual);
                }
                else
                {
                    Existe.Contador += 1;
                    FallaActual = Existe;
                }
                //*****************************
                var resolutionTypeList = failuresAccessor.GetAttemptedResolutionTypes(failure);
                if (resolutionTypeList.Count >= 3)
                {
                    Util.ErrorMsg("Cannot resolve failures - transaction will be rolled back.");



                    if (failuresAccessor.CanRollBackPendingTransaction())
                        failuresAccessor.RollBackPendingTransaction();
                    else
                        failuresAccessor.ResolveFailure(failure);
                }

                //**


                FailureDefinitionId failID = FallaActual.FailID;//  fma.GetFailureDefinitionId(); // EXISTE UN LISTADO DE FALLAS POSIBLES EN BUILTINFAILURES
                string descrip = FallaActual.Descripcion;// fma.GetDescriptionText();
                var _sever = FallaActual.Severidad;// fma.GetSeverity();


                if (failID == BuiltInFailures.RoomFailures.RoomNotEnclosed ||
                           descrip == "Two elements were not automatically joined because one or both is not editable." ||
                           descrip == "Highlighted room separation lines overlap. One of them may be ignored when Revit finds room boundaries. Delete one of the lines." ||
                           descrip == "A wall and a room separation line overlap. One of them may be ignored when Revit finds room boundaries. Shorten or delete the room separation line to remove the overlap." ||
                           descrip == "A wall and a room separation line overlap. One of them may be ignored when Revit finds room boundaries. Shorten or delete the room separation line to remove the overlap. You can tab-select one of the overlapping elements to exclude it from the group instance.") // SE USA PARA EVALUAR FALLAS ESPECIFICAS
                {
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }
                else if (_sever == FailureSeverity.Warning && descrip == "Line is slightly off axis and may cause inaccuracies.")
                {
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }
                else if (_sever == FailureSeverity.Warning && descrip == "A group has been changed outside group edit mode.  The change is being allowed because there is only one instance of the type.")
                {
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }
                else if (failID == BuiltInFailures.RebarFailures.OutSideOfHost || descrip == "Rebar is placed completely outside of its host.") // SE USA PARA EVALUAR FALLAS ESPECIFICAS
                {
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }
                else if (failID == BuiltInFailures.RebarFailures.OutSideOfHost || descrip == "Instance origin does not lie on host face. Instance will lose association to host.") // SE USA PARA EVALUAR FALLAS ESPECIFICAS
                {

                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }

                else if (failID == BuiltInFailures.EditingFailures.OwnedByOther || failID == BuiltInFailures.EditingFailures.OwnElementsOutOfDate) // SE USA PARA EVALUAR FALLAS ESPECIFICAS
                {
                    //Util.InfoMsg($"Error:\n  Descripcion de error:{descrip}");
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }
                else if (failID == BuiltInFailures.RebarFailures.OutSideOfHost) // SE USA PARA EVALUAR FALLAS ESPECIFICAS
                {
                    //Util.InfoMsg($"Error:\n  Barra completamente fuera de barra");
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }
                else if (descrip == "Line in Sketch is slightly off axis and may cause inaccuracies.")
                {
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                }
                else if (descrip == "There are identical rebar in the same place. This will result in double counting in schedules.")  // barra sobre otra                        
                {
                    failuresAccessor.ResolveFailure(failure);
                    Util.ErrorMsg("Error no considerado.Informar a programado");
                }
                else if (_sever == FailureSeverity.Warning)
                {

                    if (FallaActual.Contador < 3)
                    {
                        //Util.InfoMsg($"Error:\n  Descripcion de error:{descrip}");
                        e.SetProcessingResult(FailureProcessingResult.Continue);
                        continue;
                    }
                    else
                    {
                        if (FallaActual.Resolucion == "MensajeRevit")
                        {
                            //e.SetProcessingResult(FailureProcessingResult.WaitForUserInput);
                            failuresAccessor.ResolveFailure(failure);
                            Util.ErrorMsg("Error no considerado.Informar a programado");
                        }
                        else if (FallaActual.Resolucion == "RoolBack")
                        {

                            if (failuresAccessor.CanRollBackPendingTransaction())
                            {
                                //para limpiar despues del rollback
                                var failureHandlingOptions = failuresAccessor.GetFailureHandlingOptions();
                                failureHandlingOptions.SetClearAfterRollback(true);
                                failuresAccessor.SetFailureHandlingOptions(failureHandlingOptions);

                                if (failuresAccessor.CanRollBackPendingTransaction())
                                {
                                    var Result = failuresAccessor.RollBackPendingTransaction();
                                    Debug.Write($"Estatus del  RollBackPendingTransaction: {Result}  ");
                                }
                                else
                                    failuresAccessor.ResolveFailure(failure);

                            }
                            else
                                failuresAccessor.ResolveFailure(failure);

                            e.SetProcessingResult(FailureProcessingResult.ProceedWithRollBack);
                            return;
                        }


                    }


                    (bool result, int caso) = CerrarTransaccionForzosa(e, failuresAccessor, failure, descrip, IdCOnError);
                    if (result && caso == 1)
                    {
                        FallaActual.Resolucion = "MensajeRevit";
                        return;
                    }
                    else if (result && caso == 2)
                    {
                        FallaActual.Resolucion = "RoolBack";
                        continue;
                    }
                }
                else if (_sever == FailureSeverity.Error || _sever == FailureSeverity.DocumentCorruption)
                {
                    if (FallaActual.Contador < 3 && IsSalir == false)
                    {
                        e.SetProcessingResult(FailureProcessingResult.Continue);
                        return;
                    }
                    else
                    {
                        (bool result, int caso) = CerrarTransaccionForzosa(e, failuresAccessor, failure, descrip, IdCOnError);
                        if (result && caso == 1)
                        {
                            FallaActual.Resolucion = "MensajeRevit";
                            return;
                        }
                        else if (result && caso == 2)
                        {
                            FallaActual.Resolucion = "RoolBack";
                            continue;
                        }
                    }
                }
                else  // 18-05-2023 este caso se descarta  por ahora para casos automaticos 
                {

                    try { failuresAccessor.ResolveFailure(failure); } //SIMULA PRESIONAR EL BOTON DE RESOLVER FALLA
                    catch
                    {
                        if (FallaActual.Contador < 3)
                        {
                            e.SetProcessingResult(FailureProcessingResult.Continue);
                            continue;
                        }
                        // si se ha repetido envia mensaje de cerrar
                        (bool result, int caso) = CerrarTransaccionForzosa(e, failuresAccessor, failure, descrip, IdCOnError);

                        if (result && caso == 1)
                        {
                            FallaActual.Resolucion = "MensajeRevit";
                            return;
                        }
                        else if (result && caso == 2)
                        {
                            FallaActual.Resolucion = "RoolBack";
                            continue;
                        }

                        //   e.SetProcessingResult(FailureProcessingResult.Continue);
                    }
                }

                //try { failuresAccessor.DeleteWarning(fma); } //SIMULA EL BOTON DE BORRAR LA ADVERTENCIA O ACEPTARLA
                //catch { }
            }
            e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit); //para evitar que el error se envíe al usuario, el (pre)procesador de errores debe devolver ProceedWithCommit. Hará que se regeneren las fallas y que se reinicie el proceso de resolución de fallas
            e.SetProcessingResult(FailureProcessingResult.Continue); //CONTINUA EL PROCESO CON LA FALLA RESUELTA
            return;
        }

        //obtener  el objeto element que produce el error
        // obtener document y despues con elementID obtener 'element'
        private static void ObtenerElementoDeError(object sender, FailureMessageAccessor failure)
        {
            var _uiapp2 = sender as Autodesk.Revit.ApplicationServices.Application;
            List<ElementId> errorElementIds = failure.GetFailingElementIds().ToList(); //lista de elementId que  genera los mensajes de error
            
            //obtener lista de documento,  NOTA: aun nose como obtener documento actibo
            var _ListaDocum = _uiapp2.Documents;
            
            foreach (var f in _ListaDocum)
            {
                var elem = errorElementIds.FirstOrDefault();
                if (elem != null)
                {
                    var _documentoNH = (f as Document);
                    if (_documentoNH != null)
                    {
                        Element nuewElle = _documentoNH.GetElement(elem);
                    }
                }
            }
        }

        private static void ObtenerMensajeError(FailureMessageAccessor failure)
        {
            List<ElementId> errorElementIds = failure.GetFailingElementIds().ToList();
            IdCOnError = "";
            foreach (ElementId errorElementId in errorElementIds)
            {



                IdCOnError = (IdCOnError == "" ? errorElementId.IntegerValue.ToString() : IdCOnError + "," + errorElementId.IntegerValue);
                // Imprimir el Id del elemento en la consola de debug
                System.Diagnostics.Debug.Print("Elemento que causó el error: " + errorElementId.IntegerValue);
            }
        }

        private static (bool, int) CerrarTransaccionForzosa(FailuresProcessingEventArgs e, FailuresAccessor failuresAccessor, FailureMessageAccessor failure, string descrip,string idConError)
        {

            bool _IsAceptar = false;
            int _tipoTransaccion = 1;

            if (IsSalir == false)
            {

                UtilitarioFallasDialogosForm _UtilitarioFallasDialogosForm = new UtilitarioFallasDialogosForm(descrip,  idConError);
                _UtilitarioFallasDialogosForm.ShowDialog();
                _IsAceptar = _UtilitarioFallasDialogosForm.IsAceptar;
                _tipoTransaccion = _UtilitarioFallasDialogosForm.TipoTrasaccion;
            }
            else
            {
                _IsAceptar = true;
                _tipoTransaccion = 2;
            }
            bool IsReturn = false;

            if (_IsAceptar && _tipoTransaccion == 1)
            {
                try
                {
                    failuresAccessor.ResolveFailure(failure);
                }
                catch (Exception)
                {
                    failuresAccessor.ResolveFailure(failure);
                    e.SetProcessingResult(FailureProcessingResult.WaitForUserInput);
                    IsReturn = true;
                }
            }
            else if (_IsAceptar && _tipoTransaccion == 2)
            {
                IsSalir = true;

                //para limpiar despues del rollback
                var failureHandlingOptions = failuresAccessor.GetFailureHandlingOptions();
                failureHandlingOptions.SetClearAfterRollback(true);
                failuresAccessor.SetFailureHandlingOptions(failureHandlingOptions);

                if (failuresAccessor.CanRollBackPendingTransaction())
                {
                    TransactionStatus Result = failuresAccessor.RollBackPendingTransaction();
                    Debug.Write($"Estatus del  RollBackPendingTransaction: {Result}  ");
                }
                else
                    failuresAccessor.ResolveFailure(failure);

                e.SetProcessingResult(FailureProcessingResult.ProceedWithRollBack);
                IsReturn = true;

            }
            return (IsReturn, _tipoTransaccion);
        }
        /* estados del RollBackPendingTransaction()
                       case TransactionStatus.Uninitialized:
                        break;
                    case TransactionStatus.Started:
                        break;
                    case TransactionStatus.RolledBack:
                        break;
                    case TransactionStatus.Committed:
                        break;
                    case TransactionStatus.Pending:
                        break;
                    case TransactionStatus.Error:
                        Util.ErrorMsg("No se puedo anular comando");
                        break;
                    case TransactionStatus.Proceed:
         * */



        public static void SuprimirCuadroDialogo(object sender, Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs e)
        {
            //			Autodesk.Revit.UI.Events.MessageBoxShowingEventArgs e2 = e as Autodesk.Revit.UI.Events.MessageBoxShowingEventArgs;
            //			Autodesk.Revit.UI.Events.TaskDialogShowingEventArgs e2 = e as Autodesk.Revit.UI.Events.TaskDialogShowingEventArgs;
            Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs e2 = e as Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs;

            #region CANCELAR DIALOGO
            string s = string.Empty;
            if (null != e2)
            {
                s = "Dialogo ID " + e2.DialogId + " CANCELABLE: " + e2.Cancellable.ToString();
                System.Diagnostics.Debug.Print(s);
                try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.Abort); return; }
                catch { }
                try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.Cancel); return; }
                catch { }
                try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.Ignore); return; }
                catch { }
                try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.No); return; }
                catch { }
                try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.None); return; }
                catch { }
                try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.OK); return; }
                catch { }
                //try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.Retry); return; }
                //catch { }
                try { e2.OverrideResult((int)System.Windows.Forms.DialogResult.Yes); return; }
                catch { }
            }
            #endregion
        }
    }
}
