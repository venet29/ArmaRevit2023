using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS
{
    public class OcultarBarras_refuerzoLosa
    {
        private readonly Document _doc;
        private View _view;
        private List<View> listaView;

        public bool IsOK { get; set; }

        public OcultarBarras_refuerzoLosa(Document _doc)
        {
            this._doc = _doc;
            this._view = _doc.ActiveView;
        }



        private string ObtenerNombreCorto(string nameview)
        {
            var post = nameview.Split('(');

            if (post.Length > 1)
                return post[0].Trim();
            else
                return nameview.Replace(ConstNH.NOMBRE_PLANOLOSA_INF, "")
                                                                            .Replace(ConstNH.NOMBRE_PLANOLOSA_SUP, "")
                                                                            .Replace("(", "")
                                                                            .Replace(")", "").Trim();
        }

        public void Ocultar1BarraRefuerzo(Element Refuerzo, bool IsConTransaccion = true)
        {
            if (Refuerzo == null)
            {
                IsOK = false;
                return;
            }
            if (IsConTransaccion == true)
                OcultarVariosBarraRefuerzo_conTrans(new List<Element>() { Refuerzo });
            else
                OcultarVariosBarraRefuerzo_SINTrans(new List<Element>() { Refuerzo });
        }

        public void OcultarVariosBarraRefuerzo_SINTrans(Rebar rebar)
        {
            List<Element> ListaRefuerzo = new List<Element>();
            ListaRefuerzo.Add(rebar);
            OcultarVariosBarraRefuerzo_SINTrans(ListaRefuerzo);
        }

        public void OcultarVariosBarraRefuerzo_conTrans(List<Element> ListaRefuerzo)
        {
            if (!Validaciones(ListaRefuerzo)) return;
            if (listaView == null) ObtenerNiveles();

            if (listaView.Count == 0) listaView.Add(_view);

            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Ocultando Path");
                    MEtodoOcultar(ListaRefuerzo);                    
                    trans2.Commit();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
            }
        }

  

        public void OcultarVariosBarraRefuerzo_SINTrans(List<Element> ListaRefuerzo)
        {
            if (!Validaciones(ListaRefuerzo)) return;
            if (listaView == null) ObtenerNiveles();

            if (listaView.Count == 0) listaView.Add(_view);

            MEtodoOcultar(ListaRefuerzo);

        }

        private bool MEtodoOcultar(List<Element> ListaRefuerzo)
        {
            for (int k = 0; k < ListaRefuerzo.Count; k++)
            {
                Element refuerzolosa = ListaRefuerzo[k];
                if (!refuerzolosa.IsValidObject) continue;
                try
                {
                        // RebarInSystem rebInsyte = ListElemId[0];
                        for (int j = 0; j < listaView.Count; j++)
                        {
                            if (refuerzolosa.CanBeHidden(listaView[j]))
                            {
                                listaView[j].HideElements(new List<ElementId> { refuerzolosa.Id });
                            }
                        }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;

                    message = "Error al crear Path Symbol";

                }
            }
            return true;
        }

        private bool Validaciones(List<Element> Listarefuerzo)
        {
            if (Listarefuerzo == null) return IsOK = false;
            if (Listarefuerzo.Count == 0) return IsOK = false;
            return true;
        }
        private bool ObtenerNiveles()
        {
            try
            {
                char[] charsToTrim1 = { ConstNH.CARACTER_PTO };
                string nombreCorto = ObtenerNombreCorto(_view.Name);
                string nombreSinPto = _view.Name.TrimEnd(charsToTrim1);

                listaView = Util.GetListtViewCOntengaNombre(_doc, nombreCorto)
                                .Where(c => c.ViewType == ViewType.FloorPlan && !c.Name.Contains(nombreSinPto))
                                .ToList();
            }
            catch (Exception)
            {
                return IsOK = false;
            }

            if (listaView.Count == 0) return IsOK = false;
            return IsOK = true;
        }

  
    }
}
