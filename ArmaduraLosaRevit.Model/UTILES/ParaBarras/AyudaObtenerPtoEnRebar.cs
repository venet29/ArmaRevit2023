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
    class AyudaObtenerPtoEnRebar
    {
        public static XYZ ptoMASCercano { get; private set; }

        public static bool Obtener(Rebar _rebar,XYZ ptosele)
        {
            double distMin = 100000;
             ptoMASCercano = XYZ.Zero;
            try
            {
                if (AyudaCurveRebar.GetTodasRebarCurves(_rebar))
                {

                    var listaTotal = AyudaCurveRebar.ListacurvesSoloLineas.SelectMany(c=>c).ToList();
                    foreach (Line item in listaTotal)
                    {
                        XYZ ptoProject = item.ProjectExtendida3D(ptosele);

                        if (distMin > ptoProject.DistanceTo(ptosele))
                        {
                            distMin = ptoProject.DistanceTo(ptosele);
                            ptoMASCercano = ptoProject;
                        }
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Punto proyecto en rebar ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
