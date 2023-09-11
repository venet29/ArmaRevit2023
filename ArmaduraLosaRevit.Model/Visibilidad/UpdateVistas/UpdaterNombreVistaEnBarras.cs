using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.UpdateVistas
{
    public class UpdaterNombreVistaEnBarras : IUpdater
    {
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;
#pragma warning disable CS0169 // The field 'UpdaterNombreVistaEnBarras._updateopen' is never used
        private UpdaterNombreVistaEnBarras _updateopen;
#pragma warning restore CS0169 // The field 'UpdaterNombreVistaEnBarras._updateopen' is never used

        public UpdaterNombreVistaEnBarras(Document doc, AddInId id)//codigo interno del Updater 145689
        {
            _appId = id;
            _doc = doc;
            _updaterId = new UpdaterId(_appId, new Guid("3b6ed4da-74c2-4a80-a18a-062e2266d1be"));//CAMBIAR CODIGO EN CADA UPDATER NUEVO

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

            
                ViewSection _viewSection = _doc.GetElement(id) as ViewSection;
                if (_viewSection == null) continue;
                M1_AnalizandoView(_viewSection, _doc);

            }

            
         //   VolverCargar();


        }

        private void M1_AnalizandoView(ViewSection viewSection,Document _doc)
        {
            Util.InfoMsg($" {viewSection.Name}");


           var nombre= ParameterUtil.FindParaByBuiltInParameter(viewSection, BuiltInParameter.VIEW_NAME, _doc);

            Util.InfoMsg($"Parametrer name: {nombre}");
        }



        #region Metodos eExecute




        #endregion



        public string GetUpdaterName()
        {
            return "UPDATER ViewSection";
        }

        public string GetAdditionalInformation()
        {
            return "Actualizar nombres de barras view";
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
