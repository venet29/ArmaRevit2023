


using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.Model
{
    public class BarraIng
    {
        private readonly IntervalosBarraAutoDto _newIntervaloBarraAutoDto;
 
        public int id { get; private set; }
        public string Story { get; set; }

        public XYZ ptoBordeMuro { get; set; }
        public XYZ ptoCentroMuro { get; set; }
        public string Pier { get; set; }
        public int linea { get; set; }
        public int cantidadBarra { get; set; }
        public string cantidadBarraString { get; set; }
        public int qualityBarra { get;  set; }
        public int diametroInt { get; set; }
        public double espesor { get; set; }
        public double espaciamiento { get; set; }
        public double largoFoot { get; set; }
        public double distanciaRespectoBorde { get;  set; }
        public double distaciaDesdeOrigen { get; set; }
        public XYZnh P1 { get; set; } //p1 es el con Z mayor

        public XYZnh ptoInserccionTag_nivelLosa { get; set; } //p1 es el con Z mayor

        public XYZnh P2 { get; set; } //p2 es el con Z menor
        public XYZnh xyCentro_Pier_inferior { get; set; }
        public XYZnh xyCentro_Pier_superior { get; set; }
        public bool AuxIsbarraIncial { get; set; }
        public bool IsNoProloganLosaArriba { get; set; }
        public Orientacion orientacion { get; set; }
        public Orientacion OrientacionTagGrupoBarras { get;  set; }
        public UbicacionEnPier ubicacionEnPier { get; set; }
        public IndependentTag _IndependentTag_soloParaBorrarTag { get; }
        public Rebar _rebar { get; }

        private int _escale;

        public bool IsOk { get; set; }
        public bool IsAgrupado { get; set; }
        public TipoPataBarra _TipoBarraV { get;  set; }
        public string NOmbreFamiliaTag { get;  set; }
        public List<IndependentTag> ListaTodosTagRebar { get;  private set; }

        public BarraIng(IndependentTag independentTag)
        {
            this._IndependentTag_soloParaBorrarTag = independentTag;
            this._rebar = _IndependentTag_soloParaBorrarTag.Obtener_GetTaggedLocalElement(null) as Rebar;
            this._escale = 50; // _Rebar.Document.ActiveView.Scale; ;
            this.IsAgrupado = false;
            this.ListaTodosTagRebar = new List<IndependentTag>();
        }

        public BarraIng(IbarraBase item , IntervalosBarraAutoDto newIntervaloBarraAutoDto,XYZ ptoTag)
        {
            IbarraBaseResultDTO resulIpm = item.GetResult();
            this._IndependentTag_soloParaBorrarTag = null;
            this._rebar = resulIpm._rebar;// item.M3_a_ObtenerRebar();
            this._escale = 50;// _Rebar.Document.ActiveView.Scale; ;
            this._newIntervaloBarraAutoDto = newIntervaloBarraAutoDto;
            this.IsNoProloganLosaArriba = IsNoProloganLosaArriba;
            this.IsAgrupado = false;
            this.ptoInserccionTag_nivelLosa=   new XYZnh(ptoTag.X, ptoTag.Y, ptoTag.Z);
            this.Pier = newIntervaloBarraAutoDto.Pier;
            this.Story = newIntervaloBarraAutoDto.Story;
            this.ptoBordeMuro = newIntervaloBarraAutoDto.PtoBordeMuro;
            this.ptoCentroMuro = newIntervaloBarraAutoDto.PtoCentralSobreMuro;
            this.ubicacionEnPier = newIntervaloBarraAutoDto.ubicacionEnPier;
            this.orientacion = newIntervaloBarraAutoDto.orientacion;
            this.OrientacionTagGrupoBarras = resulIpm.OrientacionTagGrupoBarras;//;  item. newIntervaloBarraAutoDto.OrientacionTagGrupoBarras;
            this.cantidadBarra = newIntervaloBarraAutoDto.Inicial_Cantidadbarra;
            this.diametroInt = newIntervaloBarraAutoDto.Inicial_diametroMM;
            this.IsOk = true;
            this.ListaTodosTagRebar = new List<IndependentTag>();
        }


        public bool ObtenerTodosLosTag(TiposRebarTagsEnView _listaTAg)
        {
            try
            {

                ListaTodosTagRebar=_listaTAg.M1_BuscarEnColecctorPorRebar(_rebar.Id);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerTodosLosTag'. ex:{ex.Message}");
                return false;
            }
            return true;
        }



        public void ObtenerDatos(XYZ OrigenView)
        {
            try
            {
                if (!AyudaCurveRebar.GetPrimeraRebarCurves(_rebar))
                {
                    Util.ErrorMsg("Error al obtener largos parciales de barra");
                    return;
                }

                List<Curve> listapto1 = AyudaCurveRebar.ListacurvesSoloLineas[0];


                // var listapto1 = _Rebar.GetShapeDrivenAccessor().ComputeDrivingCurves().ToList();
                var curvaMAyorLargo = listapto1.MinBy(c => -c.ApproximateLength);
                largoFoot = Math.Round(listapto1.Sum(c => c.Length), 4);
                _TipoBarraV = AyudaRebarVertical.obtenerTipoBarras(listapto1);

                if (!Util.IsVertical(((Line)curvaMAyorLargo).Direction))
                {
                    Debug.WriteLine("Barra1 Seleccionada No es completamente Vertical");
                    IsOk = false;
                }

                if (curvaMAyorLargo.GetEndPoint(0).Z > curvaMAyorLargo.GetEndPoint(1).Z)
                {
                    P1 = new XYZnh(curvaMAyorLargo.GetEndPoint(0).DedondearZA4());
                    P2 = new XYZnh(curvaMAyorLargo.GetEndPoint(1).DedondearZA4());
                }
                else
                {
                    P1 = new XYZnh(curvaMAyorLargo.GetEndPoint(1).DedondearZA4());
                    P2 = new XYZnh(curvaMAyorLargo.GetEndPoint(0).DedondearZA4());
                }

                
                distanciaRespectoBorde =(ptoBordeMuro==null?0: ptoBordeMuro.GetXY0().DistanceTo(P1.GetXYZ().GetXY0()));
                distaciaDesdeOrigen = P1.GetXYZ().GetXY0().DistanceTo(OrigenView.GetXY0());
                IsOk = true;
            }
            catch (Exception)
            {

                IsOk = false;
            }
        }


        public void ObtenerDatos_CantidadDiametr0()
        {
            try
            {
                var cantBarrastt = _rebar.LookupParameter("CantidadBarra").AsString();

                if (Util.IsNumeric(cantBarrastt))
                    cantidadBarra = Convert.ToInt32(cantBarrastt);
                else
                    cantidadBarra = _rebar.Quantity;

                qualityBarra = _rebar.Quantity;
                diametroInt = _rebar.ObtenerDiametroInt();
                IsOk = true;
            }
            catch (Exception)
            {

                IsOk = false;
            }
        }

        internal string ObtenerNOmbreFamiliaTagPOrFormaV()
        {
            string NOmbreFamiliaTag = "";
            if (_TipoBarraV == TipoPataBarra.BarraVSinPatas)
                NOmbreFamiliaTag = "MRA Rebar_F_SIN_"+ _escale;
            else
                NOmbreFamiliaTag = "MRA Rebar_F_"+ _escale;

            return NOmbreFamiliaTag;

        }
    }
}
