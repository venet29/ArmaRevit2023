
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.model;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.Contener;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.ServicioAgrupar
{
    public class AgrupadorBarras
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected View _view;


        protected List<BarraIng> ListBarraIng;
      

        public List<IndependentTag> listIndependentTag { get; set; }
        public List<AdministradorGrupos> ListAdministradorGrupos { get; set; }
        
        public bool IsContinuar { get; set; }

        public XYZ _PtoUbicacionTag { get; set; }
        public XYZ _direccionMoverTag { get; set; }
        public Orientacion _DireccionSeleccionMouse { get;  set; }
        public bool IsOK { get;  set; }
        public string Pier { get; private set; }

        public AgrupadorBarras(UIApplication uiapp, List<IndependentTag> listIndependentTag)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.listIndependentTag = listIndependentTag;
            ListBarraIng = new List<BarraIng>();
            ListAdministradorGrupos = new List<AdministradorGrupos>();
        }
        public AgrupadorBarras(UIApplication uiapp, List<BarraIng> ListBarraIng, XYZ _ptoUbicacionTag, Orientacion OrientacionTagGrupoBarras)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            if (ListBarraIng != null && ListBarraIng.Count > 0)
                this.Pier = ListBarraIng[0].Pier;
            this.ListBarraIng = ListBarraIng;
            this._PtoUbicacionTag = _ptoUbicacionTag;
            this._DireccionSeleccionMouse = OrientacionTagGrupoBarras;
            this._direccionMoverTag = (OrientacionTagGrupoBarras == Orientacion.izquierda ? _view.RightDirection : -_view.RightDirection);
            ListAdministradorGrupos = new List<AdministradorGrupos>();
        }
        public void GenerarListaBarras()
        {

            var _tiposRebarTagsEnView = new TiposRebarTagsEnView(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
            _tiposRebarTagsEnView.M0_BuscarRebarTagInView();

            foreach (IndependentTag item in listIndependentTag)
            {
                BarraIng barraIng = new BarraIng(item);
                barraIng.ObtenerTodosLosTag(_tiposRebarTagsEnView);
                barraIng.ObtenerDatos(_view.Origin);
                barraIng.ObtenerDatos_CantidadDiametr0();
                if (barraIng.IsOk)
                    ListBarraIng.Add(barraIng);
            }
        }


        public bool DireccionDeTagVertical(XYZ ptoMouse)
        {
            _PtoUbicacionTag = ptoMouse;
            IsContinuar = true;
            try
            {

                XYZ origenView = _view.Origin.GetXY0();
                double dist1 = origenView.DistanceTo(ptoMouse.GetXY0());
                if (dist1 < ListBarraIng[0].distaciaDesdeOrigen)
                {
                    _DireccionSeleccionMouse = Orientacion.izquierda;
                    _direccionMoverTag = _view.RightDirection;
                }
                else
                {
                    _DireccionSeleccionMouse = Orientacion.derecha;
                    _direccionMoverTag = -_view.RightDirection;
                }
            }
            catch (Exception ex)
            {
                IsContinuar = false;
                Debug.WriteLine($" ex:{ex.Message}");
                return IsContinuar;
            }
            return IsContinuar;

        }

        public bool DireccionDeTagHorizontal(XYZ ptoMouse)
        {
            _PtoUbicacionTag = ptoMouse;
            IsContinuar = true;
            try
            {

                XYZ origenView = _view.Origin.GetXY0();
                double dist1 = origenView.DistanceTo(ptoMouse.GetXY0());
                if (dist1 < ListBarraIng[0].distaciaDesdeOrigen)
                {
                    _DireccionSeleccionMouse = Orientacion.izquierda;
                    _direccionMoverTag = _view.RightDirection;
                }
                else
                {
                    _DireccionSeleccionMouse = Orientacion.derecha;
                    _direccionMoverTag = -_view.RightDirection;
                }
            }
            catch (Exception ex)
            {
                IsContinuar = false;
                Debug.WriteLine($" ex:{ex.Message}");
                return IsContinuar;
            }
            return IsContinuar;

        }

        public GenerarNuevaDirectizDTO ObtenerGenerarNuevaDirectizDTO()
        {
            return new GenerarNuevaDirectizDTO()
            {

                OrientacionSeleccion = _DireccionSeleccionMouse,
                DireccionMoverTag = _direccionMoverTag,
                ptoInserciontag = _PtoUbicacionTag

            };
        }

    }
}
