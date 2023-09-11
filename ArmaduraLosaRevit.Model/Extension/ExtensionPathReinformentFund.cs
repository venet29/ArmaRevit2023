using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static class ExtensionPathReinformentFund
    {

      
        public static string ObtenerLargoParcialFUndaciones_String(this PathReinforcement _pathReinforcement)
        {
            string Result = "";
            var _doc = _pathReinforcement.Document;
            try
            {
                double largo = (int)Util.FootToCm(_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_1).AsDouble());


                double LargoPataFinal = 0;
                double LargoPataInicial = 0;
                bool IShookInicial = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1).AsValueString() != "None" ? true : false);
                bool IShookFinal  = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1).AsValueString() != "None" ? true : false);
               

                // var parar1 = ParameterUtil.FindParaByName(_pathReinforcement.Parameters, "Primary Bar - End Hook Type");
                //var parar2= ParameterUtil.FindParaByName(_pathReinforcement.Parameters, "Alternating Bar - End Hook Type");

     

                if (IShookInicial)
                {
                    var pata2 = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1).AsElementId();
                    var rebaHook2 = _doc.GetElement(pata2) as RebarHookType;
                    if (rebaHook2 != null)
                        LargoPataInicial = rebaHook2.StraightLineMultiplier;
                }

                if (IShookFinal)
                {
                    var pata1 = _pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1).AsElementId();
                    var rebaHook1 = _doc.GetElement(pata1) as RebarHookType;
                    if (rebaHook1 != null)
                        LargoPataFinal = rebaHook1.StraightLineMultiplier;
                }



                int diamtro = (int)_pathReinforcement.ObtenerDiametro_mm();
                //bool IShookInicial = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1).AsValueString() != "None" ? true : false);
                //bool IShookFinal = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1).AsValueString() != "None" ? true : false);

               int LargoSegunDiamtro = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(diamtro);

                if (IShookInicial && IShookFinal)
                {
                    Result = $"({LargoPataFinal}+{largo}+{LargoPataInicial})";
                }
                else if (IShookFinal)
                {
                    Result = $"({LargoPataFinal}+{largo})";
                }
                else if (IShookInicial)
                {
                    Result = $"({largo}+{LargoPataInicial})";
                }

            }
            catch (Exception)
            {

            }

            return Result;
        }
        public static double ObtenerLargoParcialFUndaciones_double(this PathReinforcement _pathReinforcement)
        {
            double Result = 0;

            try
            {
                double largo = (int)Util.FootToCm(_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_LENGTH_1).AsDouble());

                int diamtro = (int)_pathReinforcement.ObtenerDiametro_mm();
                bool IShookInicial = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_END_HOOK_TYPE_1).AsValueString() != "None" ? true : false);
                bool IShookFinal = (_pathReinforcement.get_Parameter(BuiltInParameter.PATH_REIN_HOOK_TYPE_1).AsValueString() != "None" ? true : false);

                double LargoSegunDiamtro_foot = UtilBarras.largo_gancho_diamMM_soloFUndaciones_cm(diamtro);

                if (IShookInicial && IShookFinal)
                {
                    Result = LargoSegunDiamtro_foot+largo+LargoSegunDiamtro_foot;
                }
                else if (IShookInicial)
                {
                    Result = LargoSegunDiamtro_foot+largo;
                }
                else if (IShookFinal)
                {
                    Result = largo+LargoSegunDiamtro_foot;
                }
                else
                    Result = largo;
            }
            catch (Exception)
            {

            }

            return Result;
        }

    }
}
