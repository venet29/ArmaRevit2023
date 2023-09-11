using ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar.Model;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Mirror.Ayuda
{
   public class CalculosPtos
    {
        private readonly Document _doc;
        private readonly List<WrapperBarrasLosa> ListaWrapperBarrasLosa;
        private readonly Line _EjeReferencia;

        public CoordenadaPath _CoordenadaPath { get; set; }
        private XYZ VectorDesplazamientoPAth;
        private XYZ VectorDesplazamiento1PathSymb;

        public CalculosPtos(Document _doc, List<WrapperBarrasLosa> ListaWrapperBarrasLosa,Line _EjeReferencia)
        {
            this._doc = _doc;
            this.ListaWrapperBarrasLosa = ListaWrapperBarrasLosa;
            this._EjeReferencia = _EjeReferencia;
        }

        public void M2_CalcularPtos()
        {


            for (int i = 0; i < ListaWrapperBarrasLosa.Count; i++)
            {
                WrapperBarrasLosa _wrapperBarrasLosa = ListaWrapperBarrasLosa[i];

                CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_wrapperBarrasLosa.pathReinforcement, _doc);
                _CoordenadaPath = pathReinformeCalculos.Calcular4PtosPathReinf();

                M2_1_ObtenerVectorPathRein(_wrapperBarrasLosa);

                M2_2_ObtenerVectorInicialPathSYmbol(_wrapperBarrasLosa);

                _wrapperBarrasLosa.VectorDesplazamiento = VectorDesplazamientoPAth * 2;
                _wrapperBarrasLosa.VectorDesplazamientoPathSymbol = VectorDesplazamiento1PathSymb;
            }
        }
        private bool M2_1_ObtenerVectorPathRein(WrapperBarrasLosa _wrapperBarrasLosa)
        {
            XYZ _ptoInterseccionEnEje = _EjeReferencia.ProjectExtendidaXY0(_CoordenadaPath.centro.AsignarZ(0));
            VectorDesplazamientoPAth = _ptoInterseccionEnEje - _CoordenadaPath.centro.AsignarZ(0);
            return true;
        }
        private void M2_2_ObtenerVectorInicialPathSYmbol(WrapperBarrasLosa _wrapperBarrasLosa)
        {
            XYZ _ptoInterseccionEnEjePAthsym = _EjeReferencia.ProjectExtendidaXY0(_wrapperBarrasLosa.pathReinSpanSymbol.TagHeadPosition.AsignarZ(0));
            VectorDesplazamiento1PathSymb = _wrapperBarrasLosa.pathReinSpanSymbol.TagHeadPosition.AsignarZ(0) - _ptoInterseccionEnEjePAthsym;
        }
    }
}
