using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarra.EditarRebarShape
{
    public class EditarBarraRebarShape
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private RebarShape _newRebarShape;
        private readonly SeleccionarTagRebar seleccionarTagRebar;
        private IndependentTag _independeTag;

#pragma warning disable CS0169 // The field 'EditarBarraRebarShape.EspesorElementoHost' is never used
        private double EspesorElementoHost;
#pragma warning restore CS0169 // The field 'EditarBarraRebarShape.EspesorElementoHost' is never used
#pragma warning disable CS0169 // The field 'EditarBarraRebarShape.espaciamientoFoot' is never used
        private double espaciamientoFoot;
#pragma warning restore CS0169 // The field 'EditarBarraRebarShape.espaciamientoFoot' is never used
#pragma warning disable CS0169 // The field 'EditarBarraRebarShape.recorridoBarrar' is never used
        private double recorridoBarrar;
#pragma warning restore CS0169 // The field 'EditarBarraRebarShape.recorridoBarrar' is never used

        public Rebar RebarSeleccion { get; set; }
        public XYZ ptoini { get; set; }
        public XYZ ptofinal { get; set; }

        public XYZ ptoPosicionTAg { get; set; }

        public XYZ DireccionEnFierrado { get; set; }

        public int Diametro_mm { get; set; }
        public int cantidadBarras { get; set; }
        public Element ElementMuroHost { get; set; }

        private RebarShape _actualRebraShape;

        public IntervaloBarrasDTO intervaloBarrasDTO { get; set; }
        public bool IsOK { get; private set; }

        public EditarBarraRebarShape(UIApplication uiapp, RebarShape _rebarShape, SeleccionarTagRebar seleccionarTagRebar)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._newRebarShape = _rebarShape;
            this.seleccionarTagRebar = seleccionarTagRebar;
        }

        public void ObtenerDatosDebarra()
        {
            _independeTag = seleccionarTagRebar.independentTag;
            Element elem = _independeTag.Obtener_GetTaggedLocalElement(_uiapp);
            if (elem==null)
            {
                IsOK = false;
                return;
            }
            RebarSeleccion = elem as Rebar;

            ptoPosicionTAg = _independeTag.TagHeadPosition;
            ElementMuroHost = _doc.GetElement(RebarSeleccion.GetHostId());
            _actualRebraShape = _doc.GetElement(RebarSeleccion.GetShapeId()) as RebarShape;

            var _newRebarShape_aux = ObtenesCorrectoRebarShape.NombreRebarShape(_newRebarShape, _actualRebraShape, _doc);

            if (_newRebarShape_aux == null)
                IsOK = false;
            else
                IsOK = true;

            _newRebarShape = _newRebarShape_aux;
        }

        public void CambiarRebarShape(RebarShape rebarshapeNew)
        {
            try
            {
                // RebarShape rebarshape = TiposFormasRebarShape.getRebarShape("M0_17", _doc);
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create  nuevo refuerzo-NH");
                    RebarShapeDrivenAccessor _drivenAccessor = RebarSeleccion.GetShapeDrivenAccessor();
                    _drivenAccessor.SetRebarShapeId(rebarshapeNew.Id);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
        }

    }
}
