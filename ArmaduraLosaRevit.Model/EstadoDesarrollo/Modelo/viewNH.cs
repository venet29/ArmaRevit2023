using ArmaduraLosaRevit.Model.ParametrosShare;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EstadoDesarrollo.Modelo
{
    enum EstadoView
    {Terminado, Incompleto
    }

    public class viewNH
    {
        private static int cant = 0;

        public string NombreVista { get; set; }
        public string TipoEstructura { get; set; }
        public bool IsTerminado { get; set; }
        public int _viewid { get; set; }
        public string IsSheet { get; set; } //Si -NO
        public string TipoVista { get; set; } // Sheet - Vista
        public ViewType TipoView { get; set; }
   

        public viewNH(int _viewid)
        {
            this._viewid = _viewid;
        }
        public bool ObtenerDatos(Document _doc)
        {
            try
            {

                View _view = OBtenerVistaElement(_doc);

                if (_view == null) return false;
                TipoView = _view.ViewType;

                if (TipoView == ViewType.DrawingSheet)
                {
                    IsSheet = "Si";
                    TipoVista = "Sheet";
                    NombreVista = ((ViewSheet)_view).SheetNumber + " - " + _view.Name;
                }
                else
                {
                    TipoVista = "Vista";
                    IsSheet = "NO";
                    NombreVista = _view.Name;
                }

                TipoEstructura = ParameterUtil.FindValueParaByName(_view.Parameters, "TIPO DE ESTRUCTURA (VISTA)", _doc);

                if (TipoEstructura == null)
                    return false;

                var EstadoIsTerminado = ParameterUtil.FindValueParaByName(_view.Parameters, FactoryNombre.EstadoViewIsTerminado, _doc);
                if(EstadoIsTerminado== EstadoView.Terminado.ToString())
                    IsTerminado = true;
                else
                    IsTerminado = false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerLista. ex:{ex.Message}");

                return false;
            }
            return true;
        }


        public View OBtenerVistaElement(Document _doc)=> _doc.GetElement(new ElementId(_viewid)) as View;
    }
}
