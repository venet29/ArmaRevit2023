using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Actualizar;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ocultar
{
    public class OcultarBarras
    {
        private readonly Document _doc;
        private View _view;
        private List<View> listaView;
        private string _NombreVista;
        private string _barraTipo;

        public bool IsOK { get; set; }

        public OcultarBarras(Document _doc)
        {
            this._doc = _doc;
            this._view = _doc.ActiveView;
        }

        public bool ObtenerNiveles()
        {
            try
            {
                listaView = new List<View>();
              //  View viewactual = TiposView.ObtenerTiposView(_view.Name,_doc);
                if (_view != null) listaView.Add(_view);

                if (_view.IsDependencia())
                {
                    View viewactualDepen = TiposView.ObtenerTiposView(_view.ObtenerNombreIsDependencia(), _doc);
                    if (viewactualDepen != null) listaView.Add(viewactualDepen);
                }

                //Util.ge
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return IsOK = false;
            }

            if (listaView.Count == 0) return IsOK = false;
            return IsOK = true;
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

        public void Ocultar1BarraCreada_AgregarParametroARebarSystem(PathReinforcement _PathReinforcement, string _barraTipo_ = "", bool IsConTransaccion = true)
        {
            if (_PathReinforcement == null)
            {
                IsOK = false;
                return;
            }

            _NombreVista = _view.ObtenerNombreIsDependencia();
            _barraTipo = _barraTipo_;

            if (IsConTransaccion == true)
                OcultarListaBarraCreada_AgregarParametroARebarSystemConTrasn(new List<PathReinforcement>() { _PathReinforcement });
            else
                OcultarListaBarraCreada_AgregarParametroARebarSystem(new List<PathReinforcement>() { _PathReinforcement });
        }

        public void OcultarListaBarraCreada_AgregarParametroARebarSystemConTrasn(List<PathReinforcement> ListaPathRein)
        {
            if (!Validaciones(ListaPathRein)) return;
            if (listaView == null) ObtenerNiveles();

            if (listaView.Count == 0) listaView.Add(_view);

            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Ocultando Path");
                    MEtodoOcultar(ListaPathRein);
                    ActualizarRebarInSystem.AgregarParametroRebarSystem_sinTrans(ListaPathRein, _NombreVista, _barraTipo);
                    trans2.Commit();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";
            }
        }


        public void OcultarListaBarraCreada_AgregarParametroARebarSystem(List<PathReinforcement> ListaPathRein)
        {
            if (!Validaciones(ListaPathRein)) return;
            if (listaView == null) ObtenerNiveles();

            if (listaView.Count == 0) listaView.Add(_view);

            MEtodoOcultar(ListaPathRein);
            ActualizarRebarInSystem.AgregarParametroRebarSystem_sinTrans(ListaPathRein, _NombreVista, _barraTipo);

        }

        private bool MEtodoOcultar(List<PathReinforcement> ListaPathRein)
        {
            for (int k = 0; k < ListaPathRein.Count; k++)
            {
                PathReinforcement m_createdPathReinforcement = ListaPathRein[k];
                if (!m_createdPathReinforcement.IsValidObject) continue;
                var Ilist_RebarInSystem = m_createdPathReinforcement.GetRebarInSystemIds();

                if (Ilist_RebarInSystem == null) continue;

                try
                {
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
                    //    if (_view != null && ParameterUtil.FindParaByName(elm, "NombreVista") != null) 
                    //        ParameterUtil.SetParaInt(elm, "NombreVista", _NombreVista);  //"nombre de vista"
                    //}



                    //**** path
                    for (int j = 0; j < listaView.Count; j++)
                    {
                        if (listaView[j].Name!= _view.Name &&  m_createdPathReinforcement.CanBeHidden(listaView[j]))
                        {
                            listaView[j].HideElements(new List<ElementId> { m_createdPathReinforcement.Id });
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

        private bool Validaciones(List<PathReinforcement> ListaPathRein)
        {
            if (ListaPathRein == null) return IsOK = false;
            if (ListaPathRein.Count == 0) return IsOK = false;
            return true;
        }
    }
}
