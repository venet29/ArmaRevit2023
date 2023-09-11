
using ArmaduraLosaRevit.Model.BarraV.UpDate.Casos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;

namespace ArmaduraLosaRevit.Model.RebarLosa.UpDate
{
    public class UpdaterRebarPath : IUpdater
    {
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;


        public UpdaterRebarPath(Document doc, AddInId id)//codigo interno del Updater 145689
        {
            _appId = id;
            _doc = doc;
            _updaterId = new UpdaterId(_appId, Guid.NewGuid());//CAMBIAR CODIGO EN CADA UPDATER NUEVO

            //se desactiva pq se hacen los registros en clase 'ManejadorUpdateRebar'
            // RegistraUpdate();
            //  RegisterDisparadores();

        }
        public void Execute(UpdaterData data)
        {
            _doc = data.GetDocument();

            // _updateopen = new UpdaterBarrasRebar(_doc.Application.ActiveAddInId);

            //Descargar();
            // RemoveAllTriggers  : no se puede remover disparadores durante le jecucion de un update
            //UnregisterUpdater : no se puede desactiuvar IUpdater dentro del mismo 
            foreach (ElementId id in data.GetAddedElementIds())
            {
                //	Wall muro = doc.GetElement(id) as Wall;

            }
            foreach (ElementId id in data.GetModifiedElementIds())
            {


                Group _rebar = _doc.GetElement(id) as Group;
                if (_rebar == null) continue;


                //ObtenerTipoBarra _newObtenerTipoBarra = new ObtenerTipoBarra(_rebar);
                //if (!_newObtenerTipoBarra.Ejecutar()) return;



            }

            //   VolverCargar();


        }

 


        public string GetUpdaterName()
        {
            return "UPDATER GROUP REBAR";
        }

        public string GetAdditionalInformation()
        {
            return "Modificar GROUP rebar";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.DetailComponents;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }
    }
}
