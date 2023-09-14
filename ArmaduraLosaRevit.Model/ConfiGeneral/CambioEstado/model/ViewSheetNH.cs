using ArmaduraLosaRevit.Model.BarraV.Copiar.Helper;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
//using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado.model
{

    public class ViewSheetNH
    {

        public string NombreVista { get; set; } = "";
        public Revision revisionActual { get; private set; }
        public string Sheetnombre { get; set; }
        public string SheetNumber { get; set; }
        public List<View> listaPortInSheet { get; private set; }
        public string TipoEstructura { get; set; }
        public string EstadoDeAvance { get; set; }
        public string CurrentRevision { get; set; }
        public string CODIGOESTADODEAVANCE { get; set; }
        public string ESPECIALIDAD { get; set; }
        public string CODIGOESPECIALIDAD { get; set; }

        // public int _viewid { get; set; }
        public string IsSheet { get; set; } //Si -NO
        public string TipoVista { get; set; } // Sheet - Vista
        public ViewType TipoView { get; set; }

        public bool IsTieneTemplateNh { get; set; }

        public List<ElementId> IdTemplateAsignado { get; private set; }
        public ElementId IdTemplateAsignadov2 { get; private set; }
        public View _view { get; set; }
        public ExtensionViewDTO ValorZ { get; private set; }

        //     private Mutex _mutex;
        private UIApplication _uiapp;

        private Document _doc;


        public ViewSheetNH(UIApplication uiapp, View _viewSheet)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            //  this._view = _doc.ActiveView;

            this._view = _viewSheet;
            IsTieneTemplateNh = false;
        }




        public bool ObtenerDatos()
        {
            try
            {
                TipoView = _view.ViewType;

                IdTemplateAsignadov2 = _view.ObtenerTemplate();
                if (IdTemplateAsignadov2 != null)
                    IsTieneTemplateNh = true;

                if (TipoView == ViewType.DrawingSheet)
                {
                    IsSheet = "Si";
                    TipoVista = "Sheet";
                    NombreVista = ((ViewSheet)_view).SheetNumber + " - " + _view.Name;
                }
                else if (TipoView == ViewType.Schedule)
                {
                    TipoVista = "Schedule";
                    IsSheet = "NO";
                }
                else
                {
                    TipoVista = "Vista";
                    IsSheet = "NO";
                    NombreVista = _view.Name;
                }

                if (_view is ViewSheet)
                {
                    var revisionActualId = ((ViewSheet)_view).GetCurrentRevision();
                    var revisionActualEle = _doc.GetElement(revisionActualId);// as Revision;

                    Sheetnombre = _view.Name; //ParameterUtil.FindValueParaByName(_viewSheet.Parameters, "Sheet Name", _doc);
                    SheetNumber = ((ViewSheet)_view).SheetNumber;// ParameterUtil.FindValueParaByName(_viewSheet.Parameters, "Sheet Number", _doc);

                    listaPortInSheet = ((ViewSheet)_view).ObteneListaVIew();
                }
                if(!IsTieneTemplateNh)
                    ValorZ = _view.Obtener_Z_SoloPLantas(false);
                TipoEstructura = ParameterUtil.FindValueParaByName(_view, "TIPO DE ESTRUCTURA (VISTA)");
                EstadoDeAvance = ParameterUtil.FindValueParaByName(_view, "ESTADO DE AVANCE");
                CurrentRevision = ParameterUtil.FindValueParaByName(_view, "Current Revision");
                CODIGOESPECIALIDAD = ParameterUtil.FindValueParaByName(_view, "CODIGO ESPECIALIDAD");
                CODIGOESTADODEAVANCE = ParameterUtil.FindValueParaByName(_view, "CODIGO ESTADO DE AVANCE");
                ESPECIALIDAD = ParameterUtil.FindValueParaByName(_view, "ESPECIALIDAD");

                //if (TipoEstructura == null)
                //    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerLista. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool CambiarEstado(string estadoAvance)
        {
            try
            {
                if (_view == null) return false;
                if (!_view.IsValidObject) return false;

                var EstadoDatos = FactoryEstados.ListaEstadosProyecto.Where(c => c.CurrentRevision == estadoAvance).FirstOrDefault();

                if (EstadoDatos != null)
                {
                    CODIGOESTADODEAVANCE = EstadoDatos.CODIGOESTADODEAVANCE;

                    EstadoDeAvance = EstadoDatos.CODIGOESTADODEAVANCE + ": " + EstadoDatos.CODIGOESPECIALIDAD;
                    //    _mutex.WaitOne();
                    ParameterUtil.SetParaStringNH(_view, "CODIGO ESTADO DE AVANCE", CODIGOESTADODEAVANCE);

                    if (_view is ViewSheet)
                    {
                        CurrentRevision = EstadoDatos.CurrentRevision;
                        //revisionActual.RevisionNumber= EstadoDatos.CurrentRevision; ;
                        ParameterUtil.SetParaStringNH(_view, "Current Revision", CurrentRevision);
                    }

                    var resul = ParameterUtil.SetParaStringNH(_view, "ESTADO DE AVANCE", EstadoDeAvance);
                    //_mutex.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CambiarEstado'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        internal string ObtenerNUmeroSheet() {

            if (_view == null) return "";
            if (!_view.IsValidObject) return "";
            return ((ViewSheet)_view).SheetNumber;
        }
    }
}
