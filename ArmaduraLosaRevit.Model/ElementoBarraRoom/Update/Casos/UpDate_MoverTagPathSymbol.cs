using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ExtStore;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.CAsos
{
    public class UpDate_EditTagPathSymbol : IUpdater
    {
        private UIApplication _uiapp;
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;
        private string _guid;

        public UpDate_EditTagPathSymbol(UIApplication _uiapp, AddInId id)//codigo interno del Updater 145689
        {
            this._uiapp = _uiapp;
            _appId = id;
            _doc = _uiapp.ActiveUIDocument.Document;
            _guid = "ecc365be-fe27-4822-ac05-060a942ad190";
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

            /*
            if (UtilVersionesRevit.IsMAyorOigual(_uiapp, VersionREvitNh.v2022))
            {
            }
            else if (UtilVersionesRevit.IsMAyorOigual(_uiapp, VersionREvitNh.v2021))
            {
                var _CreadorExtStoreDTO = FactoryExtStore.ObtnerPosicionTagLosa();
                CreadorExtStore _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);
                foreach (ElementId id in data.GetModifiedElementIds())
                {
                    var tasr = _doc.GetElement(id);
                    var _independentTag = _doc.GetElement(new ElementId(id.IntegerValue)) as IndependentTag;

                    _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(_independentTag, _independentTag.TagHeadPosition);

                }
            }
            else
            {
                var _CreadorExtStoreDTO = FactoryExtStore.ObtnerPosicionTagLosa();
                CreadorExtStore_2020Abajo _CreadorExtStore = new CreadorExtStore_2020Abajo(_uiapp, _CreadorExtStoreDTO);

                foreach (ElementId id in data.GetModifiedElementIds())
                {
                    var tasr = _doc.GetElement(id);
                    var _independentTag = _doc.GetElement(new ElementId(id.IntegerValue)) as IndependentTag;

                    _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(_independentTag, _independentTag.TagHeadPosition);
                }
            }
            */

            var _CreadorExtStoreDTO = FactoryExtStore.ObtnerPosicionTagLosa();
            CreadorExtStore _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);

            Debug.WriteLine($"UpDate_EditTagPathSymbol");
            foreach (ElementId id in data.GetModifiedElementIds())
            {
                Debug.WriteLine($"element Id :{id}");
                var _independentTag = _doc.GetElement(new ElementId(id.IntegerValue)) as IndependentTag;
                if (_independentTag == null) continue;
                _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(_independentTag, _independentTag.TagHeadPosition);

            }

            Debug.WriteLine($"UpdaterBarrasRebar   VerificarDatos : {timeMeasure1.ElapsedMilliseconds} ms");
        }

        #region Metodos eExecute



        #endregion
        public string GetUpdaterName()
        {
            return "UPDATER UpDate_EditTagPathSymbol";
        }

        public string GetAdditionalInformation()
        {
            return "mantener define como mover tag pathsymbol";
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
