using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension.modelo;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionPathReinforment
    {

        //reobtiene el origen del pathreinforme, pq no lo actualiza al cambiar la geometris manualmente del path
        // busca en las lines que conforman su geoemtria
        // busca la que tenga igual origen del modelcurve del pathrein
        // y que tenga igual valor de coordenada Z
        public static XYZ ObtenerOrigenModificado(this PathReinforcement _pathReinforcement, Line _lineDeCurveModel)
        {
            XYZ _direccion = _lineDeCurveModel.Direction;
            double coord_Z = _lineDeCurveModel.GetEndPoint(0).Z;
            //buscar la otra linea
            Options gOptions = new Options();
            gOptions.ComputeReferences = true;
            gOptions.DetailLevel = ViewDetailLevel.Undefined;
            gOptions.IncludeNonVisibleObjects = false;

            GeometryElement ConjuntoDeElemtosDelPathReinforment = _pathReinforcement.get_Geometry(gOptions);
            if (ConjuntoDeElemtosDelPathReinforment == null) return XYZ.Zero;

            var listapTOS = ConjuntoDeElemtosDelPathReinforment.
                                             Where(obj => obj.IsIgualDireccionYCoordenadaZ(_direccion, coord_Z)).
                                             Select(obj => (obj as Line).Origin).FirstOrDefault();
            return listapTOS;
        }

        public static bool IsIgualDireccionYCoordenadaZ(this GeometryObject Gobj, XYZ _direccion, double coord_Z)
        {
            if (!(Gobj is Line)) return false;

            Line _line = (Gobj as Line);
            XYZ pt1 = _line.GetPoint2(0);
            XYZ pt2 = _line.GetPoint2(1);
            bool result = _line.Direction.IsAlmostEqualTo(_direccion) && Math.Abs(coord_Z - pt1.Z) < 0.01;




            return result;
        }


        public static bool EsRefuerzoSUperiorSx(this string _tipoPath)
        {
            return false;
#pragma warning disable CS0162 // Unreachable code detected
            return (_tipoPath == "s1" || _tipoPath == "s2" || _tipoPath == "s3" ? true : false);
#pragma warning restore CS0162 // Unreachable code detected
        }

        public static int[] ObtenerNumeroBarras(this PathReinforcement _pathReinforcement, Document _doc)
        {

            int numeroBarrasPrimaria = 0;
            int numeroBarrasSecundario = 0;
            int[] valores = new int[2];
            try
            {
                var ListElemId = _pathReinforcement.GetRebarInSystemIds();

                if (ListElemId.Count > 0)
                {
                    RebarInSystem rebarInSystem1 = (RebarInSystem)_doc.GetElement(ListElemId[0]);
                    numeroBarrasPrimaria = ParameterUtil.FindParaByName(rebarInSystem1, "Quantity").AsInteger();
                }

                if (ListElemId.Count > 1)
                {
                    RebarInSystem rebarInSystem2 = (RebarInSystem)_doc.GetElement(ListElemId[1]);
                    numeroBarrasSecundario = ParameterUtil.FindParaByName(rebarInSystem2, "Quantity").AsInteger();
                }

                valores[0] = numeroBarrasPrimaria;
                valores[1] = numeroBarrasSecundario;
            }
            catch (Exception)
            {

                valores[0] = 0;
                valores[1] = 0;
            }

            return valores;
        }

        public static double ObtenerLargoFoot_fund(this PathReinforcement _pathReinforcement)
        {
            if (_pathReinforcement == null) return 0;
            var _doc = _pathReinforcement.Document;
            double largo = 0;
            try
            {
                largo = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_1).AsDouble();
                double LargoPata1 = 0;
                double LargoPata2 = 0;
                bool IShookInicial = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1).AsValueString() != "None" ? true : false);
                bool IShookFinal = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1).AsValueString() != "None" ? true : false);

                // var parar1 = ParameterUtil.FindParaByName(_pathReinforcement.Parameters, "Primary Bar - End Hook Type");
                //var parar2= ParameterUtil.FindParaByName(_pathReinforcement.Parameters, "Alternating Bar - End Hook Type");

                if (IShookInicial)
                {
                    var pata1 = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1).AsElementId();
                    var rebaHook1 = _doc.GetElement(pata1) as RebarHookType;
                    if (rebaHook1 != null)
                        LargoPata1 = Util.CmToFoot(rebaHook1.StraightLineMultiplier);
                }

                if (IShookFinal)
                {
                    var pata2 = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1).AsElementId();
                    var rebaHook2 = _doc.GetElement(pata2) as RebarHookType;
                    if (rebaHook2 != null)
                        LargoPata2 = Util.CmToFoot(rebaHook2.StraightLineMultiplier);
                }

                largo = largo + LargoPata1 + LargoPata2;
            }
            catch (Exception)
            {

            }

            return largo;
        }

        public static double ObtenerLargoFoot_SinPAtas(this PathReinforcement _pathReinforcement)
        {
            double largo = 0;
            try
            {
                largo = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_1).AsDouble();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }

            return largo;
        }
        public static double ObtenerLargoAlternativo(this PathReinforcement _pathReinforcement)
        {
            double largo = 0;
            try
            {
                if (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_ALTERNATING).AsValueString() == "No")
                    return 0;

                largo = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_2).AsDouble();
            }
            catch (Exception)
            {

            }
            return largo;
        }

        public static double ObtenerLargoOffSet(this PathReinforcement _pathReinforcement)
        {
            double largo = 0;
            try
            {
                if (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_ALTERNATING).AsValueString() == "No")
                    return 0;

                largo = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_ALT_OFFSET).AsDouble();
            }
            catch (Exception)
            {

            }
            return largo;
        }
        public static double ObtenerEspaciamiento_cm(this PathReinforcement _pathReinforcement) => Util.FootToCm(ObtenerEspaciamiento_foot(_pathReinforcement));
        public static double ObtenerEspaciamiento_foot(this PathReinforcement _pathReinforcement)
        {
            double _espaciamiento = 0;
            try
            {
                Parameter aux_espaciamiento = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_SPACING);
                if (aux_espaciamiento != null)
                    _espaciamiento = aux_espaciamiento.AsDouble();
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }

            return _espaciamiento;
        }

        public static int ObtenerDiametro_mm(this PathReinforcement _pathReinforcement)
        {
            int _diametro = 0;
            try
            {
                _diametro = (int)Math.Round(Util.FootToMm(ObtenerDiametro_foot(_pathReinforcement)), 0);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }

            return _diametro;
        }
        public static double ObtenerDiametro_foot(this PathReinforcement _pathReinforcement)
        {
            double _diametro = 0;
            Document _doc = _pathReinforcement.Document;
            try
            {
                Parameter aux_diametro = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_TYPE_1);
                if (aux_diametro != null)
                {
                    RebarBarType _RebarBarType = _doc.GetElement(aux_diametro.AsElementId()) as RebarBarType;
                    _diametro = _RebarBarType.ObtenerDiametroFoot();

                   // ConstNH.VersionSObre2022();
             
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }

            return _diametro;
        }


        public static int ObtenerNumeroBarras(this PathReinforcement _pathReinforcement)
        {
            int numeroBarras = 0;
            try
            {
                Document _doc = _pathReinforcement.Document;
                int aux_numerobarra = 0;
                bool result = int.TryParse(ParameterUtil.FindParaByBuiltInParameter(_pathReinforcement, BuiltInParameter.PATH_REIN_NUMBER_OF_BARS, _doc), out aux_numerobarra);
                if (!result) Util.ErrorMsg("No se pudo obtener el numero de barras del path");
                numeroBarras = aux_numerobarra;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Erro ala 'ObtenerNumeroBarras' pathRein ex:{ex.Message}");
                numeroBarras = 0;
            }
            return numeroBarras;
        }


        public static HookPAthRein ObtenerHooks(this PathReinforcement _pathReinforcement)
        {

            Document _doc = _pathReinforcement.Document;
            HookPAthRein _HoookPAthRein = new HookPAthRein();
            try
            {
                RebarHookType _RebarHookTypePrimary_star = null;
                RebarHookType _RebarHookTypeSecondary_star = null;
                RebarHookType _RebarHookTypePrimary_end = null;
                RebarHookType _RebarHookTypeSecondary_end = null;

                Parameter aux_parametro = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1);
                if (aux_parametro != null) _RebarHookTypePrimary_star = _doc.GetElement(aux_parametro.AsElementId()) as RebarHookType;

                aux_parametro = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1);
                if (aux_parametro != null) _RebarHookTypePrimary_end = _doc.GetElement(aux_parametro.AsElementId()) as RebarHookType;

                //altrnativo
                aux_parametro = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_2);
                if (aux_parametro != null) _RebarHookTypeSecondary_star = _doc.GetElement(aux_parametro.AsElementId()) as RebarHookType;

                aux_parametro = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_2);
                if (aux_parametro != null) _RebarHookTypeSecondary_end = _doc.GetElement(aux_parametro.AsElementId()) as RebarHookType;



                _HoookPAthRein = new HookPAthRein(_RebarHookTypePrimary_star, _RebarHookTypeSecondary_star, _RebarHookTypePrimary_end, _RebarHookTypeSecondary_end);

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al obtener hooks  ex:{ex.Message}");
            }

            return _HoookPAthRein;
        }


        public static HookPAthRein ObtenerHooksOP2(this PathReinforcement _pathReinforcement)
        {

            Document _doc = _pathReinforcement.Document;
            HookPAthRein _HoookPAthRein = new HookPAthRein();
            try
            {
                RebarHookType _RebarHookTypePrimary_star = null;
                RebarHookType _RebarHookTypeSecondary_star = null;
                RebarHookType _RebarHookTypePrimary_end = null;
                RebarHookType _RebarHookTypeSecondary_end = null;

                Parameter aux_parametro = ParameterUtil.FindParaByName(_pathReinforcement.Parameters, "Primary Bar - Start Hook Type");//   ParameterUtil.FindParaByName(_PathReinforcement.Parameters, "Alternating Bars"); _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1);
                if (aux_parametro != null) _RebarHookTypePrimary_star = _doc.GetElement(aux_parametro.AsElementId()) as RebarHookType;

                Parameter aux_parametro_prima_end = ParameterUtil.FindParaByName(_pathReinforcement.Parameters, "Primary Bar - End Hook Type");
                if (aux_parametro_prima_end != null) _RebarHookTypePrimary_end = _doc.GetElement(aux_parametro_prima_end.AsElementId()) as RebarHookType;

                //altrnativo
                aux_parametro = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_2);
                if (aux_parametro != null) _RebarHookTypeSecondary_star = _doc.GetElement(aux_parametro.AsElementId()) as RebarHookType;

                aux_parametro = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_2);
                if (aux_parametro != null) _RebarHookTypeSecondary_end = _doc.GetElement(aux_parametro.AsElementId()) as RebarHookType;



                _HoookPAthRein = new HookPAthRein(_RebarHookTypePrimary_star, _RebarHookTypeSecondary_star, _RebarHookTypePrimary_end, _RebarHookTypeSecondary_end);

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al obtener hooks  ex:{ex.Message}");
            }

            return _HoookPAthRein;
        }


        public static string ObtenerTipoBarra(this PathReinforcement PathRein) => ParameterUtil.FindValueParaByName(PathRein, "IDTipo", PathRein.Document);
        public static string ObtenerDireccionBarra(this PathReinforcement PathRein) => ParameterUtil.FindValueParaByName(PathRein, "IDTipoDireccion", PathRein.Document);
        public static string ObtenerDiBarraTipo(this PathReinforcement PathRein) => ParameterUtil.FindValueParaByName(PathRein, "BarraTipo", PathRein.Document);


        public static double EspesorLosa(this PathReinforcement PathRein)
        {
            double result = 0;
            Document _doc = PathRein.Document;
            ElementId elementId = PathRein.GetHostId();
            var _losa = _doc.GetElement2(elementId) as Floor;
            result = _losa.ObtenerEspesorLosaCm();
            return result;

        }

        /// <summary>
        /// obtiene la posicion real de la barra en el 3D,  agregando recubrimiento y mitad de diamtro
        /// </summary>
        /// <param name="PathRein"></param>
        /// <returns></returns>
        public static List<XYZ> ObtenerPtoPerimetroReal(this PathReinforcement PathRein)
        {
            try
            {
                double diamFoot = PathRein.ObtenerDiametro_foot();
                Document _doc = PathRein.Document;
                ElementId elementHost = PathRein.GetHostId();
                var _losaoFund = _doc.GetElement2(elementHost);

                string cara = PathRein.get_Parameter(BuiltInParameter.PATH_REIN_FACE_SLAB).AsValueString();


                double result = 0;
                ElementId elementId = PathRein.GetCurveElementIds()[0];
                var _modelline = _doc.GetElement2(elementId) as ModelLine;
                XYZ P3 = _modelline.GetCurve().GetEndPoint(1);
                XYZ P4 = _modelline.GetCurve().GetEndPoint(0);

                if (cara == "Bottom")
                {
                    ElementId recubIdInfId = _losaoFund.get_Parameter(BuiltInParameter.CLEAR_COVER_BOTTOM).AsElementId();
                    RebarCoverType recuInf = _doc.GetElement(recubIdInfId) as RebarCoverType;
                    double DistRecubInf = recuInf.CoverDistance;

                    var p3_int = _losaoFund.ObtenerPtosInterseccionFaceInferior(P3) + XYZ.BasisZ * (DistRecubInf + diamFoot / 2);
                    var p4_int = _losaoFund.ObtenerPtosInterseccionFaceInferior(P4) + XYZ.BasisZ * (DistRecubInf + diamFoot / 2);
                    P3 = p3_int;
                    P4 = p4_int;

                }
                else
                {
                    ElementId recubIdInfId = _losaoFund.get_Parameter(BuiltInParameter.CLEAR_COVER_TOP).AsElementId();
                    RebarCoverType recubSup = _doc.GetElement(recubIdInfId) as RebarCoverType;
                    double DistRecubSup = recubSup.CoverDistance;


                    var p3_Sup = _losaoFund.ObtenerPtosInterseccionFaceSuperior(P3) - XYZ.BasisZ * (DistRecubSup + diamFoot / 2);
                    var p4_sup = _losaoFund.ObtenerPtosInterseccionFaceSuperior(P4) - XYZ.BasisZ * (DistRecubSup + diamFoot / 2);
                    P3 = p3_Sup;
                    P4 = p4_sup;
                }


                var largo_foot = ObtenerLargoFoot_SinPAtas(PathRein);
                var largoOFsset = ObtenerLargoOffSet(PathRein);
                largo_foot += largoOFsset;

                var caraSup = _losaoFund.ObtenerCaraSuperior();
                XYZ direccion = caraSup.FaceNormal.CrossProduct((P4 - P3).Normalize());
                XYZ P1 = P4 + direccion * largo_foot;
                XYZ P2 = P3 + direccion * largo_foot;
                List<XYZ> Lista = new List<XYZ>();
                Lista.Add(P1);
                Lista.Add(P2);
                Lista.Add(P3);
                Lista.Add(P4);
                return Lista;
            }
            catch (Exception)
            {

                return new List<XYZ>(); ;
            }
        }


        public static List<XYZ> ObtenerPtoPerimetro_NivelCaraHost(this PathReinforcement PathRein)
        {
            try
            {
                double diamFoot = PathRein.ObtenerDiametro_foot();
                Document _doc = PathRein.Document;
                ElementId elementHost = PathRein.GetHostId();
                var _losaoFund = _doc.GetElement2(elementHost);

                string cara = PathRein.get_Parameter(BuiltInParameter.PATH_REIN_FACE_SLAB).AsValueString();


                double result = 0;
                ElementId elementId = PathRein.GetCurveElementIds()[0];
                var _modelline = _doc.GetElement2(elementId) as ModelLine;
                XYZ P3 = _modelline.GetCurve().GetEndPoint(1);
                XYZ P4 = _modelline.GetCurve().GetEndPoint(0);

                if (cara == "Bottom")
                {


                    var p3_int = _losaoFund.ObtenerPtosInterseccionFaceInferior(P3);
                    var p4_int = _losaoFund.ObtenerPtosInterseccionFaceInferior(P4);
                    P3 = p3_int;
                    P4 = p4_int;

                }
                else
                {
                    var p3_Sup = _losaoFund.ObtenerPtosInterseccionFaceSuperior(P3);
                    var p4_sup = _losaoFund.ObtenerPtosInterseccionFaceSuperior(P4);
                    P3 = p3_Sup;
                    P4 = p4_sup;
                }


                var largo_foot = ObtenerLargoFoot_SinPAtas(PathRein);
                var largoOFsset = ObtenerLargoOffSet(PathRein);
                largo_foot += largoOFsset;

                var caraSup = _losaoFund.ObtenerCaraSuperior();
                XYZ direccion = caraSup.FaceNormal.CrossProduct((P4 - P3).Normalize());
                XYZ P1 = P4 + direccion * largo_foot;
                XYZ P2 = P3 + direccion * largo_foot;
                List<XYZ> Lista = new List<XYZ>();
                Lista.Add(P1);
                Lista.Add(P2);
                Lista.Add(P3);
                Lista.Add(P4);
                return Lista;
            }
            catch (Exception)
            {

                return new List<XYZ>(); ;
            }
        }

        public static List<RebarInSystem> ObtenerRebarInsystem(this PathReinforcement listaPathReinforcement)
        {

            List<RebarInSystem> lista = new List<RebarInSystem>();



            try
            {
                if (!listaPathReinforcement.IsValidObject) return lista;
                var Ilist_RebarInSystem = listaPathReinforcement.GetRebarInSystemIds();

                if (Ilist_RebarInSystem == null) return lista;

                Document _doc = listaPathReinforcement.Document;

                List<ElementId> ListElemId = Ilist_RebarInSystem.ToList();

                for (int i = 0; i < ListElemId.Count; i++)
                {
                    RebarInSystem rebarInSystem = (RebarInSystem)_doc.GetElement(ListElemId[i]);

                    lista.Add(rebarInSystem);
                    // RebarInSystem rebInsyte = ListElemId[0];
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
            }

            return lista;
        }
    }
}
