using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Seleccionar.Model;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Mirror.DireccionMirror
{
    class MirrorParaleloBarras
    {
        private readonly Document _doc;
        private readonly Line ejeReferencia;
        private XYZ VectorDesplazamiento2PathSymb;
        private List<ElementId> liscopaida;

        public bool Isok { get; internal set; }
        public List<WrapperBarrasLosa> ListaWrapperBarrasLosa { get; set; }
        public List<PathReinforcement> ListaFinalCopiados { get; set; }
        public MirrorParaleloBarras(Document _doc, List<WrapperBarrasLosa> listaWrapperBarrasLosa, Line EjeReferencia)
        {
            this._doc = _doc;
            this.ListaWrapperBarrasLosa = listaWrapperBarrasLosa;
            this.ejeReferencia = EjeReferencia;
            ListaFinalCopiados = new List<PathReinforcement>();
        }

  

        public void Mirror()
        {
            Isok = true;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("MirrorPAth-NH");


                    for (int i = 0; i < ListaWrapperBarrasLosa.Count; i++)
                    {
                        WrapperBarrasLosa _wrapperBarrasLosa = ListaWrapperBarrasLosa[i];
                        M1_CopiarMirror(_wrapperBarrasLosa);

                        M2_AnalisasPAthsymbol(_wrapperBarrasLosa);

                        M3_AnalizarPathReinforment();
                    }

                    t.Commit();
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puede mover barra");
                Isok= false;
            }
  
        }

        private void M1_CopiarMirror(WrapperBarrasLosa _wrapperBarrasLosa)
        {
            List<ElementId> ele = _wrapperBarrasLosa.ObtenerListaIdPath();
            liscopaida = ElementTransformUtils.CopyElements(_doc, ele, _wrapperBarrasLosa.VectorDesplazamiento).ToList();
        }

        private void M3_AnalizarPathReinforment()
        {
            var pathREinf = liscopaida.Where(c => _doc.GetElement(c) is PathReinforcement).FirstOrDefault();
            if (pathREinf != null)
                ListaFinalCopiados.Add(_doc.GetElement(pathREinf) as PathReinforcement);
        }

        private void M2_AnalisasPAthsymbol(WrapperBarrasLosa _wrapperBarrasLosa)
        {
            var pathsymbId = liscopaida.Where(c => _doc.GetElement(c) is PathReinSpanSymbol).FirstOrDefault();
            if (pathsymbId != null)
            {
                var pathsymcopiado = _doc.GetElement(pathsymbId) as PathReinSpanSymbol;

                XYZ VectorDesplazamiento2PathSymb = ObtenerVectorInicialPathSYmbol(pathsymcopiado);
                XYZ VectorDesplazamiento3PathSymb = -(VectorDesplazamiento2PathSymb + _wrapperBarrasLosa.VectorDesplazamientoPathSymbol);
                ElementTransformUtils.MoveElement(_doc, pathsymbId, VectorDesplazamiento3PathSymb);
            }
        }

        private XYZ ObtenerVectorInicialPathSYmbol(PathReinSpanSymbol _pathReinSpanSymbol)
        {
            XYZ _ptoInterseccionEnEjePAthsym = ejeReferencia.ProjectExtendidaXY0(_pathReinSpanSymbol.TagHeadPosition.AsignarZ(0));
           return VectorDesplazamiento2PathSymb = _pathReinSpanSymbol.TagHeadPosition.AsignarZ(0) - _ptoInterseccionEnEjePAthsym;
        }
    }
}
