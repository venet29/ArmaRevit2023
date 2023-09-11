using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class AyudaCurveRebar
    {
        private static double _diametro;

        //obtiene un desglose de todas las curvas y arc, determinadas por el numero de barras que  tiene el rebar
        public static List< List<Curve>> ListacurvesOriginal { get; set; }

        public static List<List<Curve>> ListacurvesSoloLineas { get; set; } //LARGOS PARCIALES DE BORDE EXTERIOR A BORDE EXTERIOR   -- omiter las curvas y las agrega segun su radio de gio


        public static List<Curve> curvaMedia { get; set; } // obtien la curva media


        static List<Curve> curvesOriginal;// { get; set; }
        static List<Curve> curvesSoloLineas;//{ get; set; }

       

        public static bool GetPrimeraRebarCurves(Rebar _rebar) => GetAllRebarCurves(_rebar, tipoSeleccion.Primero);
        public static bool GetUtimaRebarCurves(Rebar _rebar) => GetAllRebarCurves(_rebar, tipoSeleccion.Ultimo);
        public static bool GetTodasRebarCurves(Rebar _rebar) => GetAllRebarCurves(_rebar, tipoSeleccion.Todas);

        public static bool GetMitadRebarCurves(Rebar _rebar)
        {
            try
            {
                curvaMedia = null;
                if (!GetAllRebarCurves(_rebar, tipoSeleccion.Todas)) return false;

                if (ListacurvesSoloLineas.Count <= 2)
                {
                    curvaMedia = ListacurvesSoloLineas[0];
                }
                else if (ListacurvesSoloLineas.Count == 3) { 
                    curvaMedia = ListacurvesSoloLineas[2];
                }
           
                else
                {
                    int posi = (int)(ListacurvesSoloLineas.Count / 2.0);
                    curvaMedia = ListacurvesSoloLineas[posi];
                    
                }

                if (curvaMedia == null) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener punto medio para seccion  ex:{ex.Message}");
                return false;
            }
            return true;
        }
        

        //obtienes todas las curvas rebar  igual ala cantidad Quantity
        public static bool GetAllRebarCurves(Rebar _rebar , tipoSeleccion tipoSeleccion=tipoSeleccion.Todas)
        {

            if (_rebar == null) return false;
            if(!_rebar.IsValidObject) return false;
            if ( Util.IsSimilarValor(_rebar.TotalLength,0,0.01)) return false;

            _diametro = _rebar.ObtenerDiametroFoot();
            ListacurvesOriginal = new List<List<Curve>>();
            ListacurvesSoloLineas = new List<List<Curve>>();
   

            try
            {


                int n = 0;
                int inicio = 0;

                n = _rebar.NumberOfBarPositions;//Quantity

                switch (tipoSeleccion)
                {
                    case tipoSeleccion.Primero:
                        n = 1;                        
                        break;
                    case tipoSeleccion.Ultimo:
                        inicio = n - 1;
                        break;
                    default:
                        break;
                }

                for (int i = inicio; i < n; ++i)
                {
                    ObtenerCurvaConArc(_rebar, i);
                    //*************  solo caurvas
                    ObtenerSoloCurva(_rebar, i);                  
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener curva barra id:{_rebar.Id}   ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private static void ObtenerCurvaConArc(Rebar _rebar, int i)
        {
            curvesOriginal = new List<Curve>();
   
            // Retrieve the curves of i'th bar in the set.
            // In case of shape driven rebar, they will be 
            // positioned at the location of the first bar 
            // in set.

            IList<Curve> centerlineCurves = _rebar.GetCenterlineCurves(true, false, false, MultiplanarOption.IncludeAllMultiplanarCurves, i);

            if (_rebar.IsRebarShapeDriven())
            {
                RebarShapeDrivenAccessor accessor = _rebar.GetShapeDrivenAccessor();
                Transform trf = accessor.GetBarPositionTransform(i);

                foreach (Curve c in centerlineCurves)
                {
                    curvesOriginal.Add(c.CreateTransformed(trf));
                }
                //larogo de los extremos = largo_curve+redioARc + diam/2
                //larogo de los centros = largo_curve+ 2*redioARc + diam/2
            }
            else
            {
                // This is a Free Form Rebar
                foreach (Curve c in centerlineCurves)
                    curvesOriginal.Add(c);
            }
            ListacurvesOriginal.Add(curvesOriginal);
        }

        private static bool ObtenerSoloCurva(Rebar _rebar, int i)
        {
 
            curvesSoloLineas = new List<Curve>();
            // Retrieve the curves of i'th bar in the set.
            // In case of shape driven rebar, they will be 
            // positioned at the location of the first bar 
            // in set.

            if (curvesOriginal.Count == 1)
            {
                ListacurvesSoloLineas.Add(curvesOriginal);
                return false;
            }

            for (int j = 0; j < curvesOriginal.Count; j++)
            {
                if (j == 0)
                {
                    Line _curve0 = (Line)curvesOriginal[0];
                    Arc _Arc0 = (Arc)curvesOriginal[1];
                    if (_curve0 == null || _Arc0 == null)
                    {
                        Util.ErrorMsg("Erro al obtener 'Solocurva'");
                        return false;
                    }

                    //larogo de los extremos = largo_curve+redioARc + diam/2
                    Curve _curve0New =_curve0.ExtenderFin(_Arc0.Radius+_diametro/2);
                    curvesSoloLineas.Add(_curve0New);
                }
                else if (j == curvesOriginal.Count - 1)
                {
                    Arc _Arc0 = (Arc)curvesOriginal[curvesOriginal.Count - 2];
                    Line _curve0 = (Line)curvesOriginal[curvesOriginal.Count - 1];
                    
                    if (_curve0 == null || _Arc0 == null)
                    {
                        Util.ErrorMsg("Erro al obtener 'Solocurva'");
                        return false;
                    }

                    //larogo de los extremos = largo_curve+redioARc + diam/2
                    Curve _curve0New = _curve0.ExtenderInicio(_Arc0.Radius + _diametro / 2);
                    curvesSoloLineas.Add(_curve0New);
                }
                else
                {
                    if (curvesOriginal[j] is Arc) continue;
                    Arc _Arc_1 = (Arc)curvesOriginal[j - 1];
                    Line _curve0 = (Line)curvesOriginal[j];
                    Arc _Arc0 = (Arc)curvesOriginal[j+1];
                    if (_curve0 == null || _Arc0 == null || _Arc_1==null)
                    {
                        Util.ErrorMsg("Erro al obtener 'Solocurva'");
                        return false;
                    }

                    //larogo de los extremos = largo_curve+redioARc + diam/2
                    Curve _curve0New = _curve0.ExtenderInicioFin(_Arc_1.Radius + _diametro / 2,_Arc0.Radius + _diametro / 2);
                    curvesSoloLineas.Add(_curve0New);
                }
            }
            ListacurvesSoloLineas.Add(curvesSoloLineas);
            return true;
        }

        
    }

    public enum tipoSeleccion
    {
        Primero,
        PrimeroYUltimo,
        Ultimo,
        Todas
    }
}
