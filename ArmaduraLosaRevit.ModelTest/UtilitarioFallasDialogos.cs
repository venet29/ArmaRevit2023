using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Windows.Forms ;

namespace UTILITARIOS
{
	/// <summary>
	/// Description of UtilitarioFallasAdvertencias.
	/// </summary>
	public class UtilitarioFallasAdvertencias
	{
		
		public void ProcesamientoFallas(object sender, Autodesk.Revit.DB.Events.FailuresProcessingEventArgs e)
		{
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

			foreach (FailureMessageAccessor fma in fmas)
			{
				FailureDefinitionId failID = fma.GetFailureDefinitionId(); // EXISTE UN LISTADO DE FALLAS POSIBLES EN BUILTINFAILURES
//				if (failID == BuiltInFailures.RoomFailures.RoomNotEnclosed) // SE USA PARA EVALUAR FALLAS ESPECIFICAS
//				{
//					failuresAccessor.DeleteWarning(fma);
//				}
				
				try { failuresAccessor.ResolveFailure(fma); } //SIMULA PRESIONAR EL BOTON DE RESOLVER FALLA
				catch { }

				try { failuresAccessor.DeleteWarning(fma); } //SIMULA EL BOTON DE BORRAR LA ADVERTENCIA O ACEPTARLA
				catch { }
			}
			e.SetProcessingResult(FailureProcessingResult.ProceedWithCommit); //OBLIGA A TERMINAR LA TRANSACCION
			e.SetProcessingResult(FailureProcessingResult.Continue); //CONTINUA EL PROCESO CON LA FALLA RESUELTA
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
				s = "Dialogo ID "+e2.DialogId+ " CANCELABLE: "+e2.Cancellable.ToString ();
				System.Diagnostics .Debug .Print (s);
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
