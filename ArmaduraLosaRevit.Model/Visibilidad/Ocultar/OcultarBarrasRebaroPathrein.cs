using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Ocultar
{
    public class OcultarBarrasRebaroPathrein
    {
        private readonly Document _doc;
        private View _view;
        private List<View> listaView;

        public bool IsOK { get; set; }

        public OcultarBarrasRebaroPathrein(Document _doc)
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

        public void Ocultar1BarraCreada_(Element _ElementosRebarOPathReinforment, bool IsConTransaccion = true)
        {
            if (_ElementosRebarOPathReinforment == null)
            {
                IsOK = false;
                return;
            }
            if (IsConTransaccion == true)
                OcultarListaBarraEnViewDistintaActual_ConTrans(new List<Element>() { _ElementosRebarOPathReinforment });
            else
                OcultarListaBarraCreada_SinTrans(new List<Element>() { _ElementosRebarOPathReinforment });
        }



        public void OcultarListaBarraEnViewDistintaActual_ConTrans(List<Element> _ElementosRebarOPathReinforment)
        {
            if (ViewType.FloorPlan != _view.ViewType) return;

            if (!Validaciones(_ElementosRebarOPathReinforment)) return;

            if (listaView == null) ObtenerNivelesDistintoViewActual();

            if (listaView.Count == 0) return;

            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Ocultando Path");
                    MEtodoOcultar(_ElementosRebarOPathReinforment);

                    trans2.Commit();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
            }
        }


        public void OcultarListaBarraCreada_SinTrans(List<Element> _ElementosRebarOPathReinforment)
        {
            if (!Validaciones(_ElementosRebarOPathReinforment)) return;
            if (listaView == null) ObtenerNivelesDistintoViewActual();

            if (listaView.Count == 0) listaView.Add(_view);

            MEtodoOcultar(_ElementosRebarOPathReinforment);

        }

        private bool MEtodoOcultar(List<Element> _ElementosRebarOPathReinforment)
        {
            for (int k = 0; k < _ElementosRebarOPathReinforment.Count; k++)
            {
                if (_ElementosRebarOPathReinforment[k] is PathReinforcement)
                {
                    PathReinforcement m_createdPathReinforcement = (PathReinforcement)_ElementosRebarOPathReinforment[k];
                    if (!m_createdPathReinforcement.IsValidObject) continue;
                    var Ilist_RebarInSystem = m_createdPathReinforcement.GetRebarInSystemIds();

                    if (Ilist_RebarInSystem == null) continue;

                    try
                    {

                        for (int j = 0; j < listaView.Count; j++)
                        {
                            if (m_createdPathReinforcement.CanBeHidden(listaView[j]))
                            {
                                listaView[j].HideElements(new List<ElementId> { m_createdPathReinforcement.Id });
                            }
                        }

                        List<ElementId> ListElemId = Ilist_RebarInSystem.ToList();

                        for (int i = 0; i < ListElemId.Count; i++)
                        {
                            RebarInSystem rebarInSystem = (RebarInSystem)_doc.GetElement(ListElemId[i]);
                            // RebarInSystem rebInsyte = ListElemId[0];
                            for (int j = 0; j < listaView.Count; j++)
                            {
                                if (rebarInSystem.CanBeHidden(listaView[j]))
                                {
                                    listaView[j].HideElements(new List<ElementId> { rebarInSystem.Id });
                                }
                            }
                        }

                        // Agregar parametris compartidos a share         
                        //foreach (var item in Ilist_RebarInSystem)
                        //{
                        //    Element elm = _doc.GetElement2(item);
                        //    if (_view != null && ParameterUtil.FindParaByName(elm, "NombreVista") != null) ParameterUtil.SetParaInt(elm, "NombreVista", _view.Name);  //"nombre de vista"
                        //}


                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;

                        message = "Error al crear Path Symbol";

                    }
                }
                else if (_ElementosRebarOPathReinforment[k] is Rebar)
                {
                    Element refuerzolosa = _ElementosRebarOPathReinforment[k];
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
            }



            return true;
        }


        public bool AgregarNombreVista_SinTrasn(List<Element> _ElementosRebarOPathReinforment)
        {
            for (int k = 0; k < _ElementosRebarOPathReinforment.Count; k++)
            {
                if (_ElementosRebarOPathReinforment[k] is PathReinforcement)
                {
                    PathReinforcement m_createdPathReinforcement = (PathReinforcement)_ElementosRebarOPathReinforment[k];
                    if (!m_createdPathReinforcement.IsValidObject) continue;
                    var Ilist_RebarInSystem = m_createdPathReinforcement.GetRebarInSystemIds();
                    if (Ilist_RebarInSystem == null) continue;
                    try
                    {
                      
                        // Agregar parametris compartidos a share         
                        foreach (var item in Ilist_RebarInSystem)
                        {
                            Element elm = _doc.GetElement2(item);
                            if (_view != null && ParameterUtil.FindParaByName(elm, "NombreVista") != null) ParameterUtil.SetParaInt(elm, "NombreVista", _view.ObtenerNombreIsDependencia());  //"nombre de vista"
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        message = "Error al crear Path Symbol";

                    }
                }
                else if (_ElementosRebarOPathReinforment[k] is Rebar)
                {
                    Element refuerzolosa = _ElementosRebarOPathReinforment[k];
                    if (!refuerzolosa.IsValidObject) continue;
                    try
                    {
                        if (_view != null && ParameterUtil.FindParaByName(refuerzolosa, "NombreVista") != null)
                            ParameterUtil.SetParaInt(refuerzolosa, "NombreVista", _view.ObtenerNombreIsDependencia());  //"nombre de vista"
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;

                        message = "Error al crear Path Symbol";

                    }

                }
            }



            return true;
        }



        private bool Validaciones(List<Element> ListaPathRein)
        {
            if (ListaPathRein == null) return IsOK = false;
            if (ListaPathRein.Count == 0) return IsOK = false;
            return true;
        }
        private bool ObtenerNivelesDistintoViewActual()
        {
            try
            {
                string nombreCorto = ObtenerNombreCorto(_view.Name);
                char[] charsToTrim1 = { ConstNH.CARACTER_PTO };
                string nombreCompletoSinPto = _view.Name.TrimEnd(charsToTrim1);

                // contiene todos los view con nombre raiz igual a nombre corto, menos los del nombre original con pto
                listaView = Util.GetListtViewCOntengaNombre(_doc, nombreCorto)
                     .Where(c => c.ViewType == ViewType.FloorPlan && !c.Name.Contains(nombreCompletoSinPto))
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
