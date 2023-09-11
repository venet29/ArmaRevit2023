using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
namespace ArmaduraLosaRevit.Model.Pasadas.Model
{
    public class EnvoltorioPipesAntigua
    {
        //protected Element _elemento;
        protected Category _tipo;
        protected ConnectorManager conect;
        public XYZ _OrigenRevitLinkInstance { get; set; }
        public CurveArray profile { get; set; }
        public Pipe Piper_ { get; set; }
        public Element _elemento { get; set; }
        public string NombreDucto { get; set; }
        public bool IsOK { get; protected set; }

        public string NombreId { get; set; }
        public bool IsSelected { get; set; }

        public InterseccionPipe InterseccionPipe_ { get; set; }

        public BuscandoFamilia BuscandoFamilia_ { get; set; }

        public string EstadoShaft { get; set; }
        public string ColorEstadoShaft { get; set; }
        public Line CurvaPipe { get; set; }

        public List<EnvoltorioShaft> ListaPasadas { get; set; }

        //************* dimensiones
        public string pt1_string { get; protected set; }
        public string pt2_string { get; protected set; }
        public XYZ PMin { get; internal set; }
        public XYZ PMax { get; protected set; }

        public XYZ Pto1 { get; private set; }
        public XYZ Pto2 { get; private set; }
        public XYZ Dire { get; private set; }

        public XYZ PuntoCentro { get; private set; }
        public XYZ ptoInf_diseñoEnMuro { get; private set; }
        public XYZ ptoSup_diseñoEnMuro { get; private set; }

        //************* largos

        public double LargoAncho_DibujarPasada_foot { get; set; }
        public double LargoAlto_DibujarPasada_foot { get; set; }
        public double LargoPipe { get; private set; }
        public double LargoPipeCm { get; set; }


        public EnvoltorioPipesAntigua(Pipe c, XYZ _origenRevitLinkInstance)
        {
            this.Piper_ = c;
            this._elemento = c;
            this._tipo = c.Category;
            this._OrigenRevitLinkInstance = _origenRevitLinkInstance;
            this.NombreDucto = Piper_.Name;
            ListaPasadas = new List<EnvoltorioShaft>();
            profile = new CurveArray();

        }


        public EnvoltorioPipesAntigua(Element c, XYZ _origenRevitLinkInstance)
        {
            this._elemento = c;

            this._tipo = c.Category;
            this._OrigenRevitLinkInstance = _origenRevitLinkInstance;
            this.NombreDucto = Piper_.Name;
            ListaPasadas = new List<EnvoltorioShaft>();
            profile = new CurveArray();

        }

        public EnvoltorioPipesAntigua()
        {
            ListaPasadas = new List<EnvoltorioShaft>();
            profile = new CurveArray();
        }

        public bool ObtenerDatos(View _view)
        {
            try
            {
                IsOK = true;
                //var curva = Ducto.GetCurve();
                var curva = _elemento.GetCurve();

                if (curva == null) return IsOK = false;
                NombreId = _elemento.Id.ToString();
                Pto1 = curva.GetEndPoint(0) + _OrigenRevitLinkInstance;
                Pto2 = curva.GetEndPoint(1) + _OrigenRevitLinkInstance;
                LargoPipe = curva.Length;
                LargoPipeCm = Math.Round(Util.FootToCm(curva.Length), 1);
                Dire = ((Line)curva).Direction;
                InterseccionPipe_ = InterseccionPipe.NoAnalizado;
                BuscandoFamilia_ = BuscandoFamilia.NoSeEncontro;

                var bx = _elemento.get_BoundingBox(_view);

                PMin = bx.Min + _OrigenRevitLinkInstance;
                PMax = bx.Max + _OrigenRevitLinkInstance;

                EstadoShaft = "SinShaft";
                ColorEstadoShaft = "White";
                conect = ((Pipe)_elemento).ConnectorManager;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                IsOK = false;
            }
            return IsOK;
        }

        public virtual bool ObtenerLArgoAncho()
        {
            try
            {
                double largo = Piper_.Diameter + Util.CmToFoot(0);

                LargoAncho_DibujarPasada_foot = largo;
                LargoAlto_DibujarPasada_foot = largo;


            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public virtual bool ObtenerGeometria()
        {

            try
            {


                if (conect == null)
                {
                    IsOK = false;
                    return false;
                }
                var result = conect.Lookup(0);
                PuntoCentro = result.Origin+ _OrigenRevitLinkInstance;
                var transf = result.CoordinateSystem;

                XYZ pto1 = PuntoCentro - transf.BasisX * LargoAncho_DibujarPasada_foot / 2 - transf.BasisY * LargoAlto_DibujarPasada_foot / 2;
                XYZ pto2 = PuntoCentro - transf.BasisX * LargoAncho_DibujarPasada_foot / 2 + transf.BasisY * LargoAlto_DibujarPasada_foot / 2;
                XYZ pto3 = PuntoCentro + transf.BasisX * LargoAncho_DibujarPasada_foot / 2 + transf.BasisY * LargoAlto_DibujarPasada_foot / 2;
                XYZ pto4 = PuntoCentro + transf.BasisX * LargoAncho_DibujarPasada_foot / 2 - transf.BasisY * LargoAlto_DibujarPasada_foot / 2;

                ptoInf_diseñoEnMuro = pto1;
                ptoSup_diseñoEnMuro = pto3;

                profile.Append(Line.CreateBound(pto1, pto2));
                profile.Append(Line.CreateBound(pto2, pto3));
                profile.Append(Line.CreateBound(pto3, pto4));
                profile.Append(Line.CreateBound(pto4, pto1));

                var tipo = _elemento.Location;
                if (tipo is LocationCurve)
                {
                    LocationCurve lc = tipo as LocationCurve;
                    CurvaPipe = (Line)lc.Curve;

                    pt1_string = Pto1.REdondearString_foot(2);
                    pt2_string = Pto2.REdondearString_foot(2);
                }
                else
                {
                    IsOK = false;
                    return false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                IsOK = false;
                return false;
            }
            IsOK = true;
            return true;
        }

    }
}

