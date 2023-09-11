using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class CrearWorkPLane
    {
        private static Document _doc;
        private static bool _ConTransaccionAlCrearSketchPlane;
        private static View _view;
        //  private static XYZ _ViewNormalDirection6;
        public static XYZ _ViewNormalDirection6 { get; set; }
        public static bool Ejecutar(Document doc, View view, bool ConTransaccionAlCrearSketchPlane = true)
        {
            _doc = doc;
            _ConTransaccionAlCrearSketchPlane = ConTransaccionAlCrearSketchPlane;
            _view = view;
            try
            {
                var _origenSeccionView = _view.Origin;
                var _RightDirection = _view.RightDirection.Redondear8();
                _ViewNormalDirection6 = _view.ViewDirection.Redondear8();

                double AnchoView = _view.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).AsDouble();
                XYZ NuevoOrigen = _origenSeccionView + -_ViewNormalDirection6 * AnchoView / 2;

                if (!M1_CrearOAsignarSketchPlane(NuevoOrigen)) return false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }


        private static bool M1_CrearOAsignarSketchPlane(XYZ NuevoOrigen)
        {
            if (_view == null) return false;

            bool isCrearPlane = false;


            Plane skInicial = null;

            if (_view.SketchPlane == null)
            { isCrearPlane = true; }
            else
            {
                skInicial = _view.SketchPlane.GetPlane();
                if (skInicial == null)
                { isCrearPlane = true; }
                else if ((!skInicial.Origin.IsAlmostEqualTo(NuevoOrigen, ConstNH.CONST_FACT_TOLERANCIA_SketchPlane)) || (!skInicial.Normal.IsAlmostEqualTo(_ViewNormalDirection6, ConstNH.CONST_FACT_TOLERANCIA_SketchPlane)))
                {
                    isCrearPlane = true;
                }
            }

            if (isCrearPlane)
            {
                try
                {
                    if (_ConTransaccionAlCrearSketchPlane)
                    {
                        using (Transaction t = new Transaction(_doc))
                        {
                            var result = t.GetStatus();
                            t.Start("CreandoSketchPlane-NH");

                            M1_1_CrearSketchPlane(NuevoOrigen);
                            t.Commit();
                        }
                    }
                    else
                    {
                        M1_1_CrearSketchPlane(NuevoOrigen);
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error en 'CrearOAsignarSketchPlane'    \n\n  ex: {ex.Message}");
                    return false;
                }

            }
            return true;
        }



        private static void M1_1_CrearSketchPlane(XYZ NuevoOrigen)
        {
            Plane plano = Plane.CreateByNormalAndOrigin(_ViewNormalDirection6, NuevoOrigen);
            SketchPlane sk = SketchPlane.Create(_doc, plano); ;
            _view.SketchPlane = sk;
        }
    }
}
