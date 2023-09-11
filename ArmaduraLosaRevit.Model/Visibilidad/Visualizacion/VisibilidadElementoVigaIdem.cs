using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista
{
    public class VisibilidadElementoVigaIdem : VisibilidadElementBase
    {
#pragma warning disable CS0108 // 'VisibilidadElementoVigaIdem._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        private Document _doc;
#pragma warning restore CS0108 // 'VisibilidadElementoVigaIdem._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        private View _view;
#pragma warning disable CS0108 // 'VisibilidadElementoVigaIdem._uiapp' hides inherited member 'VisibilidadElementBase._uiapp'. Use the new keyword if hiding was intended.
        private readonly UIApplication _uiapp;
#pragma warning restore CS0108 // 'VisibilidadElementoVigaIdem._uiapp' hides inherited member 'VisibilidadElementBase._uiapp'. Use the new keyword if hiding was intended.
        private readonly string nombreActualView;

        public VisibilidadElementoVigaIdem(UIApplication _uiapp, string nombreActualView) : base(_uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._uiapp = _uiapp;
            this.nombreActualView = nombreActualView;
        }




        public override void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view)
        {
            throw new NotImplementedException();
        }

        public List<Element> GetListaRebarIdemVIga(List<WrapperRebar> _lista_A_DeRebarVistaActual)
        {
            List<Element> listaVigaIdem = new List<Element>();
            if (_lista_A_DeRebarVistaActual.Count == 0)
            {
                Util.InfoMsg("A) No se encontraron Barras de vigas idem para ocultar");
                return listaVigaIdem;
            }

            listaVigaIdem = _lista_A_DeRebarVistaActual.Where(c => c.NombreView.Contains(nombreActualView) &&
                                                                             c.IdBarraCopiar != "0" &&
                                                                             c.NombreView != "" &&
                                                                             c.NombreView != null).Select(x => x.element).ToList();

            return listaVigaIdem;
        }
        public List<Element> GetListaRebarIdemVIgaOcultas(List<WrapperRebar> _lista_A_DeRebarVistaActual)
        {
            List<Element> listaVigaIdem = new List<Element>();
            if (_lista_A_DeRebarVistaActual.Count == 0)
            {
                Util.InfoMsg("A) No se encontraron Barras de vigas idem para ocultar");
                return listaVigaIdem;
            }

            listaVigaIdem = _lista_A_DeRebarVistaActual.Where(c => c.NombreView.Contains(nombreActualView) &&
                                                                             c.element.IsHidden(_view )== true &&
                                                                             c.IdBarraCopiar != "0" &&
                                                                             c.NombreView != "" &&
                                                                             c.NombreView != null).Select(x => x.element).ToList();

            return listaVigaIdem;
        }
    }
}
