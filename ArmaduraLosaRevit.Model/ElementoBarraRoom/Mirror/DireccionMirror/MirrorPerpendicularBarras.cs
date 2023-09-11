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
    class MirrorPerpendicularBarras
    {
        private readonly Document _doc;
        private readonly Line ejeReferencia;
#pragma warning disable CS0169 // The field 'MirrorPerpendicularBarras.VectorDesplazamiento2PathSymb' is never used
        private XYZ VectorDesplazamiento2PathSymb;
#pragma warning restore CS0169 // The field 'MirrorPerpendicularBarras.VectorDesplazamiento2PathSymb' is never used
        private List<ElementId> liscopaida;

        public bool Isok { get; internal set; }
        public List<WrapperBarrasLosa> ListaWrapperBarrasLosa { get; set; }
        public List<PathReinforcement> ListaFinalCopiados { get; set; }
        public MirrorPerpendicularBarras(Document _doc, List<WrapperBarrasLosa> listaWrapperBarrasLosa, Line EjeReferencia)
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
                Isok = false;
            }

        }

        private void M1_CopiarMirror(WrapperBarrasLosa _wrapperBarrasLosa)
        {
            List<ElementId> ele = _wrapperBarrasLosa.ObtenerListaIdPath();
            if (ElementTransformUtils.CanMirrorElements(_doc, ele))
            {

                XYZ vectorDesp = ejeReferencia.Direction.CrossProduct(new XYZ(0, 0, -1));
                Plane plano = Plane.CreateByNormalAndOrigin(vectorDesp.Redondear8(), ejeReferencia.Origin);

                liscopaida = ElementTransformUtils.MirrorElements(_doc, ele, plano, true).ToList();
            }
        }

        private void M3_AnalizarPathReinforment()
        {
            var pathREinf = liscopaida.Where(c => _doc.GetElement(c) is PathReinforcement).FirstOrDefault();
            if (pathREinf != null)
                ListaFinalCopiados.Add(_doc.GetElement(pathREinf) as PathReinforcement);
        }

        private void M2_AnalisasPAthsymbol(WrapperBarrasLosa _wrapperBarrasLosa)
        {
            var ListaTagPath = liscopaida.Where(c => _doc.GetElement(c) is IndependentTag).Select(p => _doc.GetElement(p) as IndependentTag).ToList(); ;

            if (ListaTagPath == null) return;

            if (ListaTagPath.Count>0)
            {
                var tafF=ListaTagPath.Where(c => c.Name.Contains("_F_")).FirstOrDefault();
                var tafL = ListaTagPath.Where(c => c.Name.Contains("_L_")).FirstOrDefault();

                if (tafF == null) return;
                if (tafL == null) return;

                XYZ Vector = tafF.TagHeadPosition - tafL.TagHeadPosition;

                ElementTransformUtils.MoveElement(_doc, tafL.Id, Vector);
                ElementTransformUtils.MoveElement(_doc, tafF.Id, -Vector);

            }
        }


    }
}
