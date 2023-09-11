using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GRIDS.model;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
namespace ArmaduraLosaRevit.Model.Pasadas.Model
{
    public class EnvoltorioBase
    {
        //protected Element _elemento;
        protected Category _tipo;
        protected ConnectorManager conect;
       // public XYZ _OrigenRevitLinkInstance { get; set; }
        public CurveArray profile { get; set; }

        public Element _elemento { get; set; }

        protected Document _doc;

        public Transform _transform { get; set; }
        public string NombreDucto { get; set; }
        public bool IsOK { get; protected set; }

        public string NombreId { get; set; }
        public bool IsSelected { get; set; }

        public InterseccionPipe InterseccionPipe_ { get; set; }

        public BuscandoFamilia BuscandoFamilia_ { get; set; }

        public string EstadoShaft { get; set; }
        public string ColorEstadoShaft { get; set; }
        public string ColorEstadoShaft_letra { get; set; } = "Black";
        public Line CurvaPipe { get; set; }

        public List<EnvoltorioShaft> ListaPasadas { get; set; }

        //************* dimensiones
        public string nivel { get; set; }
        public string ejesGrilla { get; set; }
        public string pt1_string { get; protected set; }
        public string pt2_string { get; protected set; }
        public XYZ PMin { get; set; }
        public XYZ PMax { get; protected set; }

        public XYZ Pto1 { get; set; }
        public XYZ Pto2 { get; set; }
        public XYZ Dire { get; set; }

        public XYZ PuntoCentro { get; set; }
        public XYZ ptoInf_diseñoEnMuro { get; set; }
        public XYZ ptoSup_diseñoEnMuro { get; set; }

        public string Orientacion3D_ { get; set; }
        public TipoPasada TipoPasada_ { get; set; }

        public string Comentario { get; set; }
        //************* largos

        public double LargoAncho_DibujarPasada_foot { get; set; }
        public double LargoAlto_DibujarPasada_foot { get; set; }
        public double LargoPipe { get; set; }
        public double LargoPipeCm { get; set; }
        public int PasadaId { get;  set; } = -1;
        public string ElementoId { get; set; } = "-1";
        public EnvoltorioBase(Element c, Transform transform)
        {
            this._elemento = c;
            this._doc = c.Document;
            this._transform = transform;
            this._tipo = c.Category;
            //this._OrigenRevitLinkInstance = _origenRevitLinkInstance;
            ListaPasadas = new List<EnvoltorioShaft>();
            profile = new CurveArray();

        }


        public virtual bool ObtenerDatos(View _view)
        {
            try
            {
                IsOK = true;
                //var curva = Ducto.GetCurve();
                var curva = _elemento.GetCurve();

                if (curva == null) return IsOK = false;
                NombreId = _elemento.Id.ToString();

                Pto1 = _transform.OfPoint(curva.GetEndPoint(0));
                Pto2 = _transform.OfPoint(curva.GetEndPoint(1));
                LargoPipe = curva.Length;
                LargoPipeCm = Math.Round(Util.FootToCm(curva.Length), 1);
                Dire = ((Line)curva).Direction;
                InterseccionPipe_ = InterseccionPipe.NoAnalizado;
                BuscandoFamilia_ = BuscandoFamilia.NoSeEncontro;

                var bx = _elemento.get_BoundingBox(_view);

                PMin = _transform.OfPoint(bx.Min);//; bx.Min + _OrigenRevitLinkInstance;
                PMax = _transform.OfPoint(bx.Max);// bx.Max + _OrigenRevitLinkInstance;

                EstadoShaft = "SinShaft";
                ColorEstadoShaft = "White";

                // ObtenerLeveYgrilla(); 
                ObtnerDireccion();
            }

            catch (Exception )

            {

                IsOK = false;
            }
            return IsOK;
        }

        public bool ObtenerLevelYgrilla(List<EnvoltoriLevel> listaLevel, List<EnvoltorioGrid> listaEnvoltorioGrid)
        {
            try
            {
                //level
                XYZ ptoMedio = (Pto1 + Pto2) / 2;
                var result = listaLevel.OrderBy(c => Math.Abs(c.ElevacionProjectadaRedondeada - ptoMedio.Z)).FirstOrDefault();
                if (result != null)
                    nivel = result.NombreLevel;


                //griilla
                var grilla2 = listaEnvoltorioGrid.Select(c => new { nombre = c.Nombre, distancia = ((Line)c.Curva).ProjectExtendidaXY0(ptoMedio).DistanceTo(ptoMedio.GetXY0()) }).OrderBy(c => c.distancia).ToList();
                var grilla = listaEnvoltorioGrid.OrderBy(c => ((Line)c.Curva).ProjectExtendidaXY0(ptoMedio).DistanceTo(ptoMedio.GetXY0())).FirstOrDefault();
                if (grilla != null)
                    ejesGrilla = grilla.Nombre;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool ObtnerDireccion()
        {

            try
            {
                if (Util.IsMASVertical(Dire))
                    Orientacion3D_ = "Vertical";
                else
                {
                    Orientacion3D_ = Math.Round(Util.AnguloEntre2PtosGrado90(Pto1, Pto2, true), 0).ToString();
                }
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

                LargoAncho_DibujarPasada_foot = LargoAncho_DibujarPasada_foot + Util.CmToFoot(1);
                LargoAlto_DibujarPasada_foot = LargoAlto_DibujarPasada_foot + Util.CmToFoot(1);

                var result = conect.Lookup(0);
                PuntoCentro = _transform.OfPoint(result.Origin);//; + _OrigenRevitLinkInstance;
                var transf = result.CoordinateSystem;

                var trasENx = transf.BasisX.RedondearSIHAYCERO();
                var trasENy = transf.BasisY.RedondearSIHAYCERO();

                XYZ pto1 = PuntoCentro - trasENx * LargoAncho_DibujarPasada_foot / 2 - trasENy * LargoAlto_DibujarPasada_foot / 2;
                XYZ pto2 = PuntoCentro - trasENx * LargoAncho_DibujarPasada_foot / 2 + trasENy * LargoAlto_DibujarPasada_foot / 2;
                XYZ pto3 = PuntoCentro + trasENx * LargoAncho_DibujarPasada_foot / 2 + trasENy * LargoAlto_DibujarPasada_foot / 2;
                XYZ pto4 = PuntoCentro + trasENx * LargoAncho_DibujarPasada_foot / 2 - trasENy * LargoAlto_DibujarPasada_foot / 2;

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
            catch (Exception )
            {
                IsOK = false;
                return false;
            }
            IsOK = true;
            return true;
        }


        public EnvoltorioBase Clone()
        {
            ConstNH.corte();
            EnvoltorioBase  nuevo= (EnvoltorioBase)this.MemberwiseClone();
            nuevo.ListaPasadas = new List<EnvoltorioShaft>();
            nuevo.ListaPasadas.AddRange(ListaPasadas);

            return nuevo;
        }
    }
}

