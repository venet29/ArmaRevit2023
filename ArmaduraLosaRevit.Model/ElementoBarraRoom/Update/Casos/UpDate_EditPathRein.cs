using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.CAsos
{

    /// <summary>
    /// reactor no utilizado pq al activar rebarInsystem tb se activan los rebar y se activa 'UpdaterBarrasRebar'
    /// y UpdaterBarrasRebar vulve a activar esta classe y se hacen llamadas recuerrentes ineficiente
    /// </summary>
    public class UpDate_EditPathRein : IUpdater
    {
        private UIApplication _uiapp;
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;
        private string _guid;

        public UpDate_EditPathRein(UIApplication _uiapp, AddInId id)//codigo interno del Updater 145689
        {
            this._uiapp = _uiapp;
            _appId = id;
            _doc = _uiapp.ActiveUIDocument.Document;
            _guid = "a57883c2-8c85-46fd-bb19-23ac2a7e09bc";
            _updaterId = new UpdaterId(_appId, new Guid(_guid));//CAMBIAR CODIGO EN CADA UPDATER NUEVO
 
        }
        public void Execute(UpdaterData data)
        {
            _doc = data.GetDocument();
            Stopwatch timeMeasure1 = Stopwatch.StartNew();
            // _updateopen = new UpdaterBarrasRebar(_doc.Application.ActiveAddInId);

            //Descargar();
            // RemoveAllTriggers  : no se puede remover disparadores durante le jecucion de un update
            //UnregisterUpdater : no se puede desactiuvar IUpdater dentro del mismo 
            foreach (ElementId id in data.GetAddedElementIds())
            {
                //	Wall muro = doc.GetElement(id) as Wall;

            }

            List<Element> ListaELementorein = new List<Element>();
            foreach (ElementId id in data.GetModifiedElementIds())
            {
                RebarInSystem reinSystem = _doc.GetElement(id) as RebarInSystem;
                if (reinSystem == null) continue;

                var ptha = _doc.GetElement(reinSystem.SystemId);
                ListaELementorein.Add(ptha);
            }

            if (ListaELementorein.Count > 0)
            {
                ActualizarnNumeroBarra _ActualizarnNumeroBarra = new ActualizarnNumeroBarra(_uiapp, _uiapp.ActiveUIDocument.Document.ActiveView);
                _ActualizarnNumeroBarra.Ejecutar_Sintrasn(ListaELementorein);
            }

            Debug.WriteLine($"UpDate_EditPathRein   VerificarDatos : {timeMeasure1.ElapsedMilliseconds } ms");
        }

        public string GetUpdaterName()
        {
            return "UPDATER UpDate_EditPathRein";
        }

        public string GetAdditionalInformation()
        {
            return "mantener texto pathReinf";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Rebar;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }
    }
}
