using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class CambiarVisibilidadViewSection
    {
        private readonly UIApplication uiapp;
        private View _viewSeccion;

        public CambiarVisibilidadViewSection(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            _viewSeccion = uiapp.ActiveUIDocument.ActiveView;
        }


        public void CambiarVisibilidad()
        {
            if (_viewSeccion is ViewSection)
            {
                int valor = _viewSeccion.get_Parameter(BuiltInParameter.SECTION_COARSER_SCALE_PULLDOWN_METRIC).AsInteger();
                if (valor == 1)
                    valor = 200;
                else
                    valor = 1;


                using (Transaction t = new Transaction(uiapp.ActiveUIDocument.Document, "cambiar visibilidad sectionView"))
                {
                    t.Start();
                    _viewSeccion.get_Parameter(BuiltInParameter.SECTION_COARSER_SCALE_PULLDOWN_METRIC).Set(valor);
                    t.Commit();
                }

            }
        }

        public void CambiarVisibilidad(bool IsOn, View _viewSeccion)
        {
            if (_viewSeccion == null) return;
            if (_viewSeccion is ViewSection)
            {
                int valor = _viewSeccion.get_Parameter(BuiltInParameter.SECTION_COARSER_SCALE_PULLDOWN_METRIC).AsInteger();
                if (valor == 1)
                    valor = 200;
                else
                    valor = 1;

                if (!IsOn && valor != 1)
                    return;

                if (IsOn && valor == 1)
                    return;

                using (Transaction t = new Transaction(uiapp.ActiveUIDocument.Document, "cambiar visibilidad sectionView"))
                {
                    t.Start();
                    _viewSeccion.get_Parameter(BuiltInParameter.SECTION_COARSER_SCALE_PULLDOWN_METRIC).Set(valor);
                    t.Commit();
                }

            }
        }

    }
}
