using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Windows.Forms;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES.FormFallasDialogo;
using Autodesk.Revit.DB.Events;
using System.Windows.Documents;

namespace ArmaduraLosaRevit.Model.UTILES
{

    public class TipoFallaAdvertercias
    {
        private FailureMessageAccessor fma;
        public FailureSeverity Severidad { get; set; }
        public string Descripcion { get; set; }
        public int Contador { get; set; } = 0;
        public bool IsSalir { get; set; } = false;
        public FailureDefinitionId FailID { get; set; }
        public FailureResolutionType CurretntResolution { get; private set; }
        public string Resolucion { get; internal set; }

        public TipoFallaAdvertercias(FailureMessageAccessor fma)
        {
            this.fma = fma;
            FailID = fma.GetFailureDefinitionId(); // EXISTE UN LISTADO DE FALLAS POSIBLES EN BUILTINFAILURES
            Descripcion = fma.GetDescriptionText();
            Severidad = fma.GetSeverity();
            var _nnumeroResolution = fma.GetNumberOfResolutions();
            Resolucion = "None";

        }

        //internal void ResolverConCriterioDerevit()
        //{
        //    //con esto se envia el mensaje popup re revit mostrando mensaje de error
        //    // FUNCIONA PARA WARNING  , NO FUNCIONA PARA ERROR
            

        //    CurretntResolution = fma.GetCurrentResolutionType();

        //}
    }

    /// <summary>
    /// Description of UtilitarioFallasAdvertencias.
    /// </summary>
    public class UtilitarioFallasAdvertencias
    {
        private  List<TipoFallaAdvertercias> ListaFalla;
        public  bool IsSalir;
        private static string IdCOnError;
        public UtilitarioFallasAdvertencias()
        {
           // Util.InfoMsg("cargando clase");
            ListaFalla = new List<TipoFallaAdvertercias>();
            IsSalir = false;
        }
        public void ProcesamientoFallas(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
        {
            Util.InfoMsg("cargando ProcesamientoFallas--- no deberia");
            FailuresAccessor failuresAccessor = e.GetFailuresAccessor();

            String transactionName = failuresAccessor.GetTransactionName();
            System.Diagnostics.Debug.Print("Nombre Transaction: " + transactionName);

            IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();

            if (fmas.Count == 0)
            {
                // FailureProcessingResult.Continue SI ES UNA FALLA QUE NO GENERA MENSAJES SE DEJA CONTINUAR EL CICLO
                e.SetProcessingResult(FailureProcessingResult.Continue);
                return;
            }

            foreach (FailureMessageAccessor failure in fmas)
            {
                TipoFallaAdvertercias FallaActual = new TipoFallaAdvertercias(failure);

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

                FailureDefinitionId failID = FallaActual.FailID;//  fma.GetFailureDefinitionId(); // EXISTE UN LISTADO DE FALLAS POSIBLES EN BUILTINFAILURES
                string descrip = FallaActual.Descripcion;// fma.GetDescriptionText();
                var _sever = FallaActual.Severidad;// fma.GetSeverity();
                              

                if (failID == BuiltInFailures.RoomFailures.RoomNotEnclosed ||
                           descrip == "Two elements were not automatically joined because one or both is not editable." ||
                           descrip == "Highlighted room separation lines overlap. One of them may be ignored when Revit finds room boundaries. Delete one of the lines." ||
                           descrip == "A wall and a room separation line overlap. One of them may be ignored when Revit finds room boundaries. Shorten or delete the room separation line to remove the overlap.") // SE USA PARA EVALUAR FALLAS ESPECIFICAS
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
                else if (failID == BuiltInFailures.EditingFailures.OwnedByOther || failID == BuiltInFailures.EditingFailures.OwnElementsOutOfDate) // SE USA PARA EVALUAR FALLAS ESPECIFICAS
                {
                    Util.InfoMsg($"Error:\n  Descripcion de error:{descrip}");
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                }
                if (failID == BuiltInFailures.RebarFailures.OutSideOfHost) // SE USA PARA EVALUAR FALLAS ESPECIFICAS
                {
                    Util.InfoMsg($"Error:\n  Barra completamente fuera de barra");
                    failuresAccessor.DeleteWarning(failure);
                    e.SetProcessingResult(FailureProcessingResult.Continue);
                    continue;
                }
                else if (_sever == FailureSeverity.Warning)
                {

                    if (FallaActual.Contador < 3)
                    {
                        Util.InfoMsg($"Error:\n  Descripcion de error:{descrip}");
                        e.SetProcessingResult(FailureProcessingResult.Continue);
                        continue;
                    }
                
                    // si se ha repetido envia mensaje de cerrar

                    (bool result, int caso) = CerrarTransaccionForzosa(e, failuresAccessor, failure, descrip,"");
                    if (result && caso==1)
                        return;
                    else if (result && caso == 2)
                        continue;
                }
                else if (_sever == FailureSeverity.Error || _sever == FailureSeverity.DocumentCorruption)
                {
                    if (FallaActual.Contador < 3  && IsSalir==false)
                    {
                        e.SetProcessingResult(FailureProcessingResult.Continue);
                        return;
                    }
                    else
                    {
                        (bool result, int caso) = CerrarTransaccionForzosa(e, failuresAccessor, failure, descrip, "");
                        if (result && caso == 1)
                            return;
                        else if (result && caso == 2)
                            continue;
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
                        (bool result, int caso) = CerrarTransaccionForzosa(e, failuresAccessor, failure, descrip, "");
                        if (result && caso == 1)
                            return;
                        else if (result && caso == 2)
                            continue;

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

        private (bool, int) CerrarTransaccionForzosa(FailuresProcessingEventArgs e, FailuresAccessor failuresAccessor, FailureMessageAccessor fma, string descrip, string idConError)
        {

            bool _IsAceptar = false;
            int _tipoTransaccion = 1;

            if (IsSalir == false)
            {

                UtilitarioFallasDialogosForm _UtilitarioFallasDialogosForm = new UtilitarioFallasDialogosForm(descrip,idConError);
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
                    failuresAccessor.ResolveFailure(fma);
                }
                catch (Exception)
                {
                    e.SetProcessingResult(FailureProcessingResult.ProceedWithRollBack);
                    VolverAtras(e, failuresAccessor);
                    IsReturn = true;
                
                }

                //    continue;
            }
            else if (_IsAceptar && _tipoTransaccion == 2)
            {
                IsSalir = true;
                e.SetProcessingResult(FailureProcessingResult.ProceedWithRollBack);
                VolverAtras(e, failuresAccessor);
                IsReturn = true;
                
            }
            return (IsReturn, _tipoTransaccion);
        }

        private static void VolverAtras(FailuresProcessingEventArgs e, FailuresAccessor failuresAccessor)
        {
            if (failuresAccessor.CanRollBackPendingTransaction())
            {
                var resultRoll = failuresAccessor.RollBackPendingTransaction();
                switch (resultRoll)
                {
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
                        break;
                    default:
                        break;
                }
            }

            e.SetProcessingResult(FailureProcessingResult.ProceedWithRollBack); //OBLIGA A TERMINAR LA TRANSACCION
            return;
        }

    
        public void SuprimirCuadroDialogo(object sender, Autodesk.Revit.UI.Events.DialogBoxShowingEventArgs e)
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
