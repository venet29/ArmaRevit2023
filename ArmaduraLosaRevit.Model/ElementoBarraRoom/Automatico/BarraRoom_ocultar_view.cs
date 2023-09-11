using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ocultar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Automatico
{
    public class BarraRoom_ocultar_view
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private OcultarBarras _OcultarBarras;
#pragma warning disable CS0649 // Field 'BarraRoom_ocultar_view.listaView' is never assigned to, and will always have its default value null
        private List<View> listaView;
#pragma warning restore CS0649 // Field 'BarraRoom_ocultar_view.listaView' is never assigned to, and will always have its default value null

        public BarraRoom_ocultar_view(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;

            _OcultarBarras = new OcultarBarras(_doc);
          //  _OcultarBarras.Ocultar1BarraCreada_AgregarParametroARebarSystem(m_createdPathReinforcement);

        }
        public void Ejecutar(List<BarraRoom> ListaBarraRoom)
        {

            if (!_OcultarBarras.ObtenerNiveles()) return;

            try
            {
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Crear CreatePathReinforcement2-NH");

                    for (int i = 0; i < ListaBarraRoom.Count; i++)
                    {
                        BarraRoom newBarraRoom = ListaBarraRoom[i];


                        if (newBarraRoom.m_createdPathReinforcement != null)
                        {
                            TipoRebar auxTipoRebar = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, newBarraRoom.TipoBarraStr);
                            var aux_BarraTipo = Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(auxTipoRebar);

                            _OcultarBarras.Ocultar1BarraCreada_AgregarParametroARebarSystem(newBarraRoom.m_createdPathReinforcement, aux_BarraTipo, IsConTransaccion: false);
                            ITagF newITagF = FactoryITagF_.ObtenerICasoyBarraX(_doc, newBarraRoom.m_createdPathReinforcement, newBarraRoom.TipoBarraStr);
                            if (newITagF!=null)newITagF.Ejecutar();
                        }

                        if (newBarraRoom.m_createdPathReinforcement_izq != null)
                        {
                            TipoRebar auxTipoRebar_Izq = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, newBarraRoom.TipoBarra_izq_Inf);
                            var aux_BarraTipo_Izq = Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(auxTipoRebar_Izq);

                            _OcultarBarras.Ocultar1BarraCreada_AgregarParametroARebarSystem(newBarraRoom.m_createdPathReinforcement_izq, aux_BarraTipo_Izq, IsConTransaccion: false);
                            ITagF newITagF = FactoryITagF_.ObtenerICasoyBarraX(_doc, newBarraRoom.m_createdPathReinforcement_izq, newBarraRoom.TipoBarra_izq_Inf);
                            if (newITagF != null) newITagF.Ejecutar();
                        }
                        if (newBarraRoom.m_createdPathReinforcement_dere != null)
                        {
                            TipoRebar auxTipoRebar_Der = EnumeracionBuscador.ObtenerEnumGenerico(TipoRebar.NONE, newBarraRoom.TipoBarra_dere_sup);
                            var aux_BarraTipo_Der = Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(auxTipoRebar_Der);

                            _OcultarBarras.Ocultar1BarraCreada_AgregarParametroARebarSystem(newBarraRoom.m_createdPathReinforcement_dere, aux_BarraTipo_Der, IsConTransaccion: false);
                            ITagF newITagF = FactoryITagF_.ObtenerICasoyBarraX(_doc, newBarraRoom.m_createdPathReinforcement_dere, newBarraRoom.TipoBarra_dere_sup);
                            if (newITagF != null) newITagF.Ejecutar();
                        }
                      
                    }
                    trans2.Commit();
                }

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"BarraRoom_ocultar_view  ex: {ex}");
            }

        }


        private void OcultarBarraCreada_AgregarParametroARebarSystem_v2(PathReinforcement listaPathReinforcement)
        {





            if (!listaPathReinforcement.IsValidObject) return;
            var Ilist_RebarInSystem = listaPathReinforcement.GetRebarInSystemIds();

            if (Ilist_RebarInSystem == null) return;


            try
            {
                //segunda trasn

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
                foreach (var item in Ilist_RebarInSystem)
                {
                    Element elm = _doc.GetElement2(item);
                    if (_view != null && ParameterUtil.FindParaByName(elm, "NombreVista") != null)
                        ParameterUtil.SetParaInt(elm, "NombreVista", _view.ObtenerNombreIsDependencia());  //"nombre de vista"
                }
                //  doc.Regenerate();

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                message = "Error al crear Path Symbol";

            }



        }

    }
}
