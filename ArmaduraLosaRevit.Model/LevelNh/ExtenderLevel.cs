using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.LevelNh
{
    public class ExtenderLevel
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ExtenderLevel(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.ActiveView;
        }

        public bool Extender()
        {

            try
            {
                SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                List<EnvoltoriLevel> listaLevel = _seleccionarNivel.ObtenerListaEnvoltoriLevelOrdenadoPorElevacion();

                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();

                    for (int i = 0; i < listaLevel.Count; i++)
                    {
                        var lvl = listaLevel[i];
                        var box = lvl._Level.get_BoundingBox(_view);
                        var pmax = box.Max;
                        var pmin = box.Min;

                        pmax = pmax + new XYZ(0, 100, 0);
                        //pmin = pmin + new XYZ(100, 0, 0);

                        var cc = Line.CreateBound(pmin, pmax);
                        var getc=lvl._Level.GetCurvesInView(DatumExtentType.ViewSpecific,_view);

                        //lvl._Level.SetDatumExtentType(DatumEnds.End0, _view, DatumExtentType.ViewSpecific);
                        //var getc2=lvl._Level.GetCurve();
                        var propag = lvl._Level.GetPropagationViews(_view);
                        lvl._Level.SetCurveInView(DatumExtentType.ViewSpecific, _view, cc);
                   
                    }

                    tr.Commit();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }
    }
}
