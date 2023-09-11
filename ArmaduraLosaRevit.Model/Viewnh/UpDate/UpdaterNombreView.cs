
using ArmaduraLosaRevit.Model.BarraV.UpDate.Casos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;

namespace ArmaduraLosaRevit.Model.Viewnh.UpDate
{
    public class UpdaterNombreView : IUpdater
    {
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;


        public UpdaterNombreView(Document doc, AddInId id)//codigo interno del Updater 145689
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
                View _view = _doc.GetElement(id) as View;
                if (_view == null) continue;
                if (_view.ViewType == ViewType.FloorPlan || _view.ViewType == ViewType.Section || _view.ViewType == ViewType.CeilingPlan)
                {
                    string viewnamaPara = _view.ObtenerNombre_ViewNombre();

                    string viewTipoEstructura = _view.ObtenerNombre_TipoEstructura();
                    if (viewTipoEstructura == "") continue;
                    if (viewnamaPara != _view.Name && (viewTipoEstructura.Contains("ARMADURA") ||  viewTipoEstructura.ToLower().Contains("ELEV") || viewTipoEstructura.Contains("ESTRUCTURA")))
                    {
                        Util.InfoMsg("Para cambiar nombre de vista utilizar comando 'Actualizar nombre vista'. Nombre de view cambiara a la version anterior.\n\n" +
                            $"NombrePArametro:{viewnamaPara}\n" +
                            $"Nombre View:{_view.Name}");
                        _view.Name = viewnamaPara;
                    }
                }
            }
            //   VolverCargar();
        }

        public string GetUpdaterName()
        {
            return "UPDATER NOMBRE VIEW";
        }

        public string GetAdditionalInformation()
        {
            return "Modificar nombre view";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Views;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }
    }
}
