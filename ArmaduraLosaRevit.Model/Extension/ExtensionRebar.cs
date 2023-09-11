using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{

    public class RebarREferDto
    {

        public double Distance { get; set; }
        public Reference Reference { get; set; }
        public RebarREferDto(double distance, Reference reference)
        {
            this.Distance = distance;
            this.Reference = reference;
        }
    }

    public static class ExtensionRebar
    {

        public static double ObtenerDiametroFoot(this Rebar rebar)
        {
            string diamString = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsValueString().Replace("mm", "").Trim();
            double diamInt = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsDouble();
            //if (Util.IsNumeric(diamString))
            //{
            //    diamInt = Util.MmToFoot(Util.ConvertirStringInDouble(diamString));
            //}
            return diamInt;
        }


        public static double ObtenerLargoCm(this Rebar rebar) => Util.FootToCm(ObtenerLargoFoot(rebar));
        public static double ObtenerLargoFoot(this Rebar rebar) => rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_LENGTH).AsDouble();
        public static int ObtenerDiametroInt(this Rebar rebar)
        {
            string diamString = rebar.get_Parameter(BuiltInParameter.REBAR_BAR_DIAMETER).AsValueString().Replace("mm", "").Trim();
            int diamInt = 0;
            if (Util.IsNumeric(diamString))
            {
                diamInt = Util.ConvertirStringInInteger(diamString);
            }
            return diamInt;
        }


        public static double ObtenerEspaciento_cm(this Rebar rebar)
        {
            double espa = rebar.get_Parameter(BuiltInParameter.REBAR_ELEM_BAR_SPACING).AsDouble();

            return Util.FootToCm(espa);
        }
        public static XYZ ObtenerInicioCurvaMasLarga(this Rebar rebar)
        {
            var getdrive = rebar.GetCenterlineCurves(false, false, true, MultiplanarOption.IncludeOnlyPlanarCurves, 0).MinBy(c => -c.Length);

            return getdrive.GetEndPoint(0);
        }
        public static XYZ ObtenerFinCurvaMasLarga(this Rebar rebar)
        {
            var getdrive = rebar.GetCenterlineCurves(false, false, true, MultiplanarOption.IncludeOnlyPlanarCurves, 0).MinBy(c => -c.Length);

            return getdrive.GetEndPoint(1);
        }
        public static string ObtenerTipoBArra_string(this Rebar rebar)
        {
            var nombreBarraTipo = ParameterUtil.FindParaByName(rebar.Parameters, "BarraTipo")?.AsString();

            if (nombreBarraTipo == null)
            {
                Util.ErrorMsg("Error al obtener 'BarraTipo'");
                return "";
            }
            return nombreBarraTipo;
        }


        public static TipoRebar ObtenerTipoBArra_TipoRebar(this Rebar rebar)
        {
            var valorstring = ObtenerTipoBArra_string(rebar);
            var _barraTipo = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, valorstring);

            return _barraTipo;
        }


        public static double ObtenerPeso(this Rebar rebar)
        {
            double peso = 0;
            try
            {
                peso = (rebar.ObtenerDiametroInt() / 12.73) * (rebar.ObtenerDiametroInt() / 12.73) * (rebar.ObtenerLargoCm() / 100.0f) * rebar.Quantity;
            }
            catch (Exception)
            {
                return 0;
            }

            return peso;
        }

        public static Reference getReferenceForEndOfBar(this Rebar rebar, View _view, Line rebarSeg)
            => getReferenceForPointOfBar(rebar, _view, rebarSeg, 1);
        public static Reference getReferenceForStartOfBar(this Rebar rebar, View _view, Line rebarSeg)
         => getReferenceForPointOfBar(rebar, _view, rebarSeg, 0);
        public static Reference getReferenceForPointOfBar(this Rebar rebar, View _view, Line rebarSeg, int Point)
        {

#pragma warning disable CS0219 // The variable '_resul' is assigned but its value is never used
            Reference _resul = null;
#pragma warning restore CS0219 // The variable '_resul' is assigned but its value is never used
            Options options = new Options();
            // the view in which you want to place the dimension
            options.View = _view;
            options.ComputeReferences = true; // produce references
            options.IncludeNonVisibleObjects = true;

            GeometryElement geom = rebar.get_Geometry(options);
            int cont = 0;

            List<RebarREferDto> ListasRebarREferDto = new List<RebarREferDto>();

            //Debug.WriteLine($"--- Rebar : {rebar.Id.IntegerValue} )");
            foreach (GeometryObject geomObj in geom)
            {
                cont = cont + 1;
                Solid sld = geomObj as Solid;

                if (sld != null) continue;
                //Debug.WriteLine($" ");
                //Debug.WriteLine($"{cont}) tipo : {geomObj.GetType()} )");
                //Debug.WriteLine($"{cont}) tipo : {geomObj.GetHashCode()} )");
                //Debug.WriteLine($"{cont}) Id : {geomObj.Id} )");
                Line refLine = geomObj as Line;

                if (geomObj is Arc)
                {
                    var arc = (Arc)geomObj;
                    //Debug.WriteLine($"{cont}) Direction : {arc.Center.REdondearString(3)} )");
                    //Debug.WriteLine($"{cont}) Largo : {arc.ApproximateLength} )");
                    //Debug.WriteLine($"{cont}) Normal : {arc.Normal.REdondearString(3)} )");
                    //Debug.WriteLine($"{cont}) Radio : {arc.Radius} )");
                }

                if (refLine != null && refLine.Reference != null)
                {

                    // We found one reference. 
                    // Let's see if it is the correct one. 
                    // The correct referece need to be perpendicular 
                    // to rebar segement and the end point of rebar 
                    // segment should be on the reference curve.



                    var lin_Clone = (Line)refLine.Clone();
                    //Debug.WriteLine($"{cont})lin_Clone Direction : {lin_Clone.Direction.REdondearString(3)} )");
                    //Debug.WriteLine($"{cont})lin_Clone Length : {lin_Clone.Length} )");
                    //Debug.WriteLine($"{cont})lin_Clone Origin : {lin_Clone.Origin} )");
                    // refLine = lin_Clone;
                    double dotProd = lin_Clone.Direction.DotProduct(rebarSeg.Direction);

                    if (!Util.IsSimilarValor(Math.Abs(dotProd), 0.0, 0.001)) continue; // curves are not perpendicular.

                    XYZ endPointOfRebar = rebarSeg.GetEndPoint(Point);

                    IntersectionResult ir = lin_Clone.Project(endPointOfRebar);

                    if (ir == null) continue; // end point of rebar segment is not on the reference curve.

                    ListasRebarREferDto.Add(new RebarREferDto(ir.Distance, refLine.Reference));

                    if (!Util.IsSimilarValor(Math.Abs(ir.Distance), 0.0, 0.001)) continue; // end point of rebar segment is not on the reference curve.

                    return refLine.Reference;
                }

            }

            if (ListasRebarREferDto.Count > 0)
                return ListasRebarREferDto.OrderBy(c => c.Distance).FirstOrDefault()?.Reference;
            else
                return null;
        }



        public static bool getPrimerRebarSegment(this Rebar rebar, out Line rebarSeg)
        {
            rebarSeg = null;
            IList<Curve> rebarSegments = rebar.GetCenterlineCurves(false, true, true, MultiplanarOption.IncludeOnlyPlanarCurves, 0);
            var getdrive = rebar.GetCenterlineCurves(false, false, true, MultiplanarOption.IncludeOnlyPlanarCurves, 0).MinBy(c => -c.Length);
            if (rebarSegments.Count != 1) return false;

            rebarSeg = rebarSegments[0] as Line;

            if (rebarSeg == null) return false;

            return true;
        }

        public static bool getRebarSegmentMasLArgo(this Rebar rebar, out Line rebarSeg)
        {
            rebarSeg = null;

            var getdrive = rebar.GetCenterlineCurves(false, false, true, MultiplanarOption.IncludeOnlyPlanarCurves, 0).MinBy(c => -c.Length);
            if (getdrive == null) return false;

            rebarSeg = getdrive as Line;

            if (rebarSeg == null) return false;

            return true;
        }
        public static bool IsBarrarIntersectadaParalella(this Rebar rebar1, Rebar rebar2)
        {
            try
            {
                if (rebar1 == null) return false;
                if (rebar2 == null) return false;

                Line rebarSeg1 = null;
                bool bOk = rebar1.getRebarSegmentMasLArgo(out rebarSeg1);

                Line rebarSeg2 = null;
                bool bOk2 = rebar2.getRebarSegmentMasLArgo(out rebarSeg2);

                if ((bOk == false || bOk2 == false)) return false;

                if (!Util.IsParallel(rebarSeg1.Direction, rebarSeg2.Direction)) return false;


                XYZ Pto = default;
                bool IsIntersPto1 = false;
                bool IsIntersPto2 = false;

                XYZ P1_ref = rebarSeg2.GetEndPoint(0);
                XYZ P2_ref = rebarSeg2.GetEndPoint(1);

                XYZ ptointer1 = rebarSeg1.ProjectExtendida3D(P1_ref);
                XYZ ptointer2 = rebarSeg1.ProjectExtendida3D(P2_ref);

                if (Util.IsSimilarValor(ptointer1.DistanceTo(P1_ref), 0, 0.01) ||
                    Util.IsSimilarValor(ptointer2.DistanceTo(P2_ref), 0, 0.01))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
        }

    }
}
