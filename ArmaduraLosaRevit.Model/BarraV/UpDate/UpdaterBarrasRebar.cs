
using ArmaduraLosaRevit.Model.BarraV.UpDate.Casos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;

using System.Diagnostics;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraV.UpDate
{
    public class Update_BarrasRebar : IUpdater
    {
        private readonly UIApplication _uiapp;
        static AddInId _appId;
        static UpdaterId _updaterId;
        private Document _doc;
#pragma warning disable CS0169 // The field 'Update_BarrasRebar._updateopen' is never used
        private Update_BarrasRebar _updateopen;
#pragma warning restore CS0169 // The field 'Update_BarrasRebar._updateopen' is never used
        public static bool IsMjs { get; set; } = false;
        public Update_BarrasRebar(UIApplication _uiapp, AddInId id)//codigo interno del Updater 145689
        {
            this._uiapp = _uiapp;
            _appId = id;
            _doc = _uiapp.ActiveUIDocument.Document;
            _updaterId = new UpdaterId(_appId, new Guid("afe1cab7-f37d-4acc-931b-653a3bdbddeb"));//CAMBIAR CODIGO EN CADA UPDATER NUEVO

            //se desactiva pq se hacen los registros en clase 'ManejadorUpdateRebar'
            // RegistraUpdate();
            //  RegisterDisparadores();

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

            List<Element> ListaELementoRebaiInSystem = new List<Element>();


            foreach (ElementId id in data.GetModifiedElementIds())
            {
                var elem = _doc.GetElement(id);

                if (elem is Rebar)
                {
                    Rebar _rebar = elem as Rebar;
                    if (_rebar == null) continue;

                    ObtenerTipoBarra _newObtenerTipoBarra = new ObtenerTipoBarra(_rebar);
                    if (!_newObtenerTipoBarra.Ejecutar()) return;

                    if (_newObtenerTipoBarra.TipoBarraGeneral == TipoBarraGeneral.Elevacion || _newObtenerTipoBarra.TipoBarraGeneral == TipoBarraGeneral.Losa)
                    {
                        UpdateRebarElevaciones _newUpdateRebarElevaciones = new UpdateRebarElevaciones(_doc, _rebar, _newObtenerTipoBarra.TipoBarra_);
                        _newUpdateRebarElevaciones.Ejecutar();
                    }
                    else if (_newObtenerTipoBarra.TipoBarraGeneral == TipoBarraGeneral.LosaInclinida)
                    {
                        UpdateRebarLosaInclinadas _newUpdateRebarLosaEscalera = new UpdateRebarLosaInclinadas(_doc, _rebar, _newObtenerTipoBarra.TipoBarra_);
                        _newUpdateRebarLosaEscalera.Ejecutar();
                    }
                    else if (_newObtenerTipoBarra.TipoBarraGeneral == TipoBarraGeneral.LosaEscalera)
                    {
                        UpdateRebarLosaEscalera _newUpdateRebarLosaEscalera = new UpdateRebarLosaEscalera(_doc, _rebar, _newObtenerTipoBarra.TipoBarra_);
                        _newUpdateRebarLosaEscalera.Ejecutar();
                    }
                    else
                    {

                        if (IsMjs)
                        {

                            var result = Util.ErrorMsgConDesctivar($"Barras  id{_rebar.Id.IntegerValue} sin 'TipoBarraGeneral'. No se puede actualizar ");
                            if (result == System.Windows.Forms.DialogResult.Cancel)
                            {
                                IsMjs = false;
                            }
                        }

                    }
                }
                else if (elem is RebarInSystem)
                {
                    //para cuendo se modifica la geometra del pathreinforme
                    RebarInSystem reinSystem = elem as RebarInSystem;
                    if (reinSystem == null) continue;

                    var ptha = _doc.GetElement(reinSystem.SystemId);
                    if(!(ListaELementoRebaiInSystem.Exists(c=> c.Id.IntegerValue== ptha.Id.IntegerValue)))
                        ListaELementoRebaiInSystem.Add(ptha);
                }
            }


            if (ListaELementoRebaiInSystem.Count > 0)
            {
                ActualizarnNumeroBarra _ActualizarnNumeroBarra = new ActualizarnNumeroBarra(_uiapp, _uiapp.ActiveUIDocument.Document.ActiveView);
                _ActualizarnNumeroBarra.Ejecutar_Sintrasn(ListaELementoRebaiInSystem);
            }


            Debug.WriteLine($"UpdaterBarrasRebar   VerificarDatos : {timeMeasure1.ElapsedMilliseconds } ms");
        }



        //#endregion


        public string GetUpdaterName()
        {
            return "UPDATER REBAR";
        }

        public string GetAdditionalInformation()
        {
            return "Modificar texto rebar";
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
