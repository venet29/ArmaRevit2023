using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.CAsos
{
    public class UpDate_MoverPathSymbol : IUpdater
    {
        private UIApplication _uiapp;
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;


        private TipoMoverTag _TipoMoverTag;
        public UpDate_MoverPathSymbol(UIApplication _uiapp, AddInId id)//codigo interno del Updater 145689
        {
            this._uiapp = _uiapp;
            _appId = id;
            _doc = _uiapp.ActiveUIDocument.Document;
            _updaterId = new UpdaterId(_appId, new Guid("2f0738a8-1731-4d94-84ce-d62d51170fd7"));//CAMBIAR CODIGO EN CADA UPDATER NUEVO
            _TipoMoverTag = TipoMoverTag.MoverMantenerTODO;
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
            foreach (ElementId id in data.GetModifiedElementIds())
            {
                var tasr = _doc.GetElement(id);
                PathReinSpanSymbol _independentTag = _doc.GetElement(new ElementId(id.IntegerValue - 1)) as PathReinSpanSymbol;
                if (_independentTag == null) continue;

                PathReinforcement PathRein = _doc.GetElement(_independentTag.Obtener_GetTaggedLocalElementID()) as PathReinforcement;
                if (PathRein == null) continue;

                List<IndependentTag> listaIndependentTag = TiposPathReinTagsEnView.M1_GetFamilySymbol_ConPathReinforment(PathRein.Id, _doc, _doc.ActiveView.Id);
                if (listaIndependentTag.Count == 0) continue;

                if (_TipoMoverTag == TipoMoverTag.MoverConfiguracionInicial)
                {
                    //mantener tag como configuracion inicial
                    ManejarEditTagUpdate_MoverConfiguracionInicial _ManejarEditTagUpdate_move = new ManejarEditTagUpdate_MoverConfiguracionInicial(_uiapp, PathRein, _independentTag.TagHeadPosition, listaIndependentTag);
                    _ManejarEditTagUpdate_move.Ejecutar_SinTrans();
                }
                else if (_TipoMoverTag == TipoMoverTag.MoverMantenerTODO)
                {
                    ManejarEditTagUpdate_MoverMantenerTODO _ManejarEditTagUpdate_MoverMantenerTODO = new ManejarEditTagUpdate_MoverMantenerTODO(_uiapp, PathRein, _independentTag.TagHeadPosition, listaIndependentTag);
                    _ManejarEditTagUpdate_MoverMantenerTODO.Ejecutar();
                }
                else if (_TipoMoverTag == TipoMoverTag.MoverMantenerSOLOExtremos)
                {
                    ManejarEditTagUpdate_MoverMantenerSOLOExtremos _ManejarEditTagUpdate_MoverMantenerSOLOExtremos = new ManejarEditTagUpdate_MoverMantenerSOLOExtremos(_uiapp, PathRein, _independentTag.TagHeadPosition, listaIndependentTag);
                    _ManejarEditTagUpdate_MoverMantenerSOLOExtremos.Ejecutar();
                }
            }

            Debug.WriteLine($"UpDate_MoverPathSymbol   VerificarDatos : {timeMeasure1.ElapsedMilliseconds } ms");
        }

        #region Metodos eExecute

        private void M2_ModificarSegmentoRebar(PathReinSpanSymbol _independentTag)
        {
            Util.ErrorMsg($"name:{_independentTag.Name}  , {_independentTag.Location.ToString()}   ");
        }

        #endregion
        public string GetUpdaterName()
        {
            return "UPDATER UpDate_MoverPathSymbol";
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
