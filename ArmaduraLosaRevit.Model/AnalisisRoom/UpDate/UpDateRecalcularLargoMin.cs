using ArmaduraLosaRevit.Model.AnalisisRoom.Servicios;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.Tag.UpDate
{
    public class UpDateRecalcularLargoMin : IUpdater
    {
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;


        public UpDateRecalcularLargoMin(Document doc, AddInId id)//codigo interno del Updater 145689
        {
            _appId = id;
            _doc = doc;
            _updaterId = new UpdaterId(_appId, new Guid("bff783c2-209a-463a-b931-021bf900950f"));//CAMBIAR CODIGO EN CADA UPDATER NUEVO

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
                var tasr = _doc.GetElement(id);
                RoomTag _roomTag = _doc.GetElement(new ElementId( id.IntegerValue)) as RoomTag;
                if (_roomTag == null) continue;

                List<Element> ListaRooms = new List<Element>();
                //borrar elemtos si existen
                ServicioaCambiarLargoMinV2 servicioaCambiarLargoMin = new ServicioaCambiarLargoMinV2(_doc);
                //borrar elemtos
                servicioaCambiarLargoMin.BorrarLargosMinUpdate(_roomTag.Room);


                //dibujar
              //  servicioaCambiarLargoMin.DibujarLargosMin();
            }         
        }




        #region Metodos eExecute

        private void M2_ModificarSegmentoRebar(PathReinSpanSymbol _independentTag)
        {
            Util.ErrorMsg($"name:{_independentTag.Name}  , {_independentTag.Location.ToString()}   ");
        }

        #endregion
        public string GetUpdaterName()
        {
            return "UPDATER UpDateRecalcularLargoMin";
        }

        public string GetAdditionalInformation()
        {
            return "mantener texto pathsymbol";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Annotations;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }
    }
}
