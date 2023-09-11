using ArmaduraLosaRevit.Model.BarraV.ColorRebar;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades
{
    public class ElementoRebar_Elev
    {
        private View _view;

        public List<IndependentTag> Listatag { get; set; }
        public Rebar _rebar { get; set; }

        private Document _doc;
        private double LevelHostRebar;

        public XYZ VectorDesplazamiento { get; set; }
        public XYZ nivelReferencia { get; set; }

        public XYZ OrigenRebar { get; set; }
        public XYZ OrigenHostRebar { get; set; }

        public Color colorBarras { get; set; }
        public string nombreElevacion { get; set; }
        public string barraTipo { get; set; }
        public bool IsOk { get; private set; }

        public ElementoRebar_Elev(Rebar _rebar, List<IndependentTag> listaTag, View _view)
        {
            this._rebar = _rebar;
            this._doc = _rebar.Document;
            this.Listatag = listaTag;
            this._view = _view;
            this.IsOk = true;
        }

        public List<ElementId> ObtenerListaIdPath()
        {
            List<ElementId> ele = new List<ElementId>();
            ele.Add(_rebar.Id);
            if (Listatag.Count > 0)
                ele.AddRange(Listatag.Select(c => c.Id).ToList());
            return ele;
        }

        public void ObtenerPArametros(double _valorZ)
        {
            try
            {
                //color
                OverrideGraphicSettings grhapOverRRider = _view.GetElementOverrides(_rebar.Id);
                colorBarras = grhapOverRRider.ProjectionLineColor;

                var _driverAccesor = _rebar.GetShapeDrivenAccessor();

                var listapto1 = _rebar.GetShapeDrivenAccessor().ComputeDrivingCurves().ToList();
                var primerRebar = (Line)listapto1[0];

                //a) ubicacion
                OrigenRebar = primerRebar.Origin;
                nivelReferencia = ObtenerNivelReferencia();
                double valorDesplazamientoZ = _valorZ - nivelReferencia.Z;

                if (valorDesplazamientoZ < 0)
                {
                    Util.ErrorMsg($"Error al obtener delta de desplazamiento. Valor negativo : {valorDesplazamientoZ}");
                    IsOk = false;
                }

                VectorDesplazamiento = new XYZ(0, 0, valorDesplazamientoZ);

                //barratipo
                var paraNombreVista = ParameterUtil.FindParaByName(_rebar, "NombreVista");
                if (paraNombreVista != null) nombreElevacion = paraNombreVista.AsString();
                
                //nombre elevacion
                var paraBarraTipo = ParameterUtil.FindParaByName(_rebar, "BarraTipo");
                if (paraBarraTipo != null) barraTipo = paraBarraTipo.AsString();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Obtener Parametros Internos\nex : {ex.Message} ");
            }
        }

        private XYZ ObtenerNivelReferencia()
        {
            Element elementHostRebar = _doc.GetElement(_rebar.GetHostId());
            if (elementHostRebar == null)
            {
                Util.ErrorMsg($"Error al  Obtener Nivel Referencia");
                IsOk = false;
            }
            

            if (elementHostRebar is Wall)
            {
                Level aux_level  = (elementHostRebar as Wall).ObtenerLevel();
                if (aux_level == null)
                {
                    Util.ErrorMsg($"Error al  Obtener Nivel Referencia de wall");
                    IsOk = false;
                }
                LevelHostRebar = aux_level.ProjectElevation;
            }
            else if (elementHostRebar.Category.Name == "Structural Framing")
            {
                LevelHostRebar = (elementHostRebar as FamilyInstance).ObtenerLevel_valorZ();
            }
 
            else if (elementHostRebar.Category.Name == "Floors" || elementHostRebar.Category.Name == "Structural Foundations")
            {
                Level aux_levelfloor = (elementHostRebar as Floor).ObtenerLevel();
                if (aux_levelfloor == null)
                {
                    Util.ErrorMsg($"Error al  Obtener Nivel Referencia de losa");
                    IsOk = false;
                }

                LevelHostRebar = aux_levelfloor.ProjectElevation;
            }

            return new XYZ(0, 0, LevelHostRebar);
        }
    }
}

