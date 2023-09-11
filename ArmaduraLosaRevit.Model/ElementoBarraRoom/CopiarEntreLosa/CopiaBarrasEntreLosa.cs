using ArmaduraLosaRevit.Model.BarraV.Copiar;
using ArmaduraLosaRevit.Model.BarraV.Copiar.model;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa.Servicios;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa
{
    internal class CopiaBarrasEntreLosa
    {
        private UIApplication _uiapp;
        private readonly View _viewActual;
        // private readonly SeleccionarPathReinformentParaCopiar _seleccionarPathReinformentParaCopiar;
        private Document _doc;
        private List<ElementoPathRein> _listaElementoPathRein;
        private readonly List<ElementoPath> _listaElementoRebar;
        private List<Element> _listaElementoGroup;

        public bool Isok { get; private set; }

#pragma warning disable CS0169 // The field 'CopiaBarrasEntreLosa.view3D_Visualizar' is never used
        private View3D view3D_Visualizar;
#pragma warning restore CS0169 // The field 'CopiaBarrasEntreLosa.view3D_Visualizar' is never used

        public List<WrapperFormatoRebar_final> _listaWrapperFormatoRebar_final { get; set; }
        public List<ElementId> ListaElementoCopiados_id { get; set; }

        public CopiaBarrasEntreLosa(UIApplication uiapp, View _viewActual,
            List<ElementoPathRein> ListaElementoPathRein,
            List<ElementoPath> ListaElementoRebar,
            List<Element> listaElementoGroup)
        {
            this._uiapp = uiapp;
            this._viewActual = _viewActual;
            // this._seleccionarPathReinformentParaCopiar = _SeleccionarPathReinformentParaCopiar;
            this._doc = uiapp.ActiveUIDocument.Document;

            this._listaElementoPathRein = ListaElementoPathRein;
            this._listaElementoRebar = ListaElementoRebar;
            this._listaElementoGroup = listaElementoGroup;
            _listaWrapperFormatoRebar_final = new List<WrapperFormatoRebar_final>();
            ListaElementoCopiados_id = new List<ElementId>();
        }





        public bool CopiarConTrasnAll(View _viewDestino)
        {
            Isok = true;
             //view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            List<ElementId> lisIdAll_ = new List<ElementId>();
            List<ElementId> lisIdAll_path = _listaElementoPathRein.SelectMany(c => c.ObtenerListaIdPath()).ToList();
            List<ElementId> lisIdAll_rebar = _listaElementoRebar.SelectMany(c => ((ElementoPathRebar)c).ObtenerListaIdPath()).ToList();
            List<ElementId> lisIdAll_Group = _listaElementoGroup.Select(c => c.Id).ToList();
            lisIdAll_.AddRange(lisIdAll_path);
            lisIdAll_.AddRange(lisIdAll_rebar);
            lisIdAll_.AddRange(lisIdAll_Group);

            if (lisIdAll_.Count == 0) return true;

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start($"copiarBarrasSinColor-NH");
                    var ListaCopiada = ElementTransformUtils.CopyElements(_viewActual, lisIdAll_, _viewDestino, null, new CopyPasteOptions()).ToList();
                    ListaElementoCopiados_id.AddRange(ListaCopiada);
                    CopiarParametrosBarrasCopiadas.M4_CopiarParametros(_doc,_viewDestino, ListaElementoCopiados_id);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Util.ErrorMsg("No se puede mover barra");
                Isok = false;
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return Isok;
        }

      




    }
}