
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class VisibilidadElemenNoEnView : VisibilidadElementBase
    {
#pragma warning disable CS0108 // 'VisibilidadElemenNoEnView._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        private readonly Document _doc;
#pragma warning restore CS0108 // 'VisibilidadElemenNoEnView._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        private List<Element> _listaAreaREain;

        public VisibilidadElemenNoEnView(UIApplication _uiapp) : base(_uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            _listaAreaREain = new List<Element>();
        }
        public override void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view)
        {

        }

        public void OcultarElemento(List<WrapperAreaRein> _lista_A_DeAreaReinVistaActual, View view)
        {
            if (_lista_A_DeAreaReinVistaActual == null) return;
            if (_lista_A_DeAreaReinVistaActual.Count==0) return;
            CAmbiaAElement(_lista_A_DeAreaReinVistaActual, view);
            vi1_OcultarElemento(_listaAreaREain, view);
        }
        private void CAmbiaAElement(List<WrapperAreaRein> _lista_A_DeAreaReinVistaActual, View _view)
        {
            var listaId = _lista_A_DeAreaReinVistaActual.Where(c => !c.NombreView.Contains(_view.Name) &&
                                                                    c.NombreView != "" &&
                                                                    c.NombreView != null)
                                                                .SelectMany(x => x.ListaRebarInsistem);

            _listaAreaREain = new List<Element>();
            foreach (var item in listaId) _listaAreaREain.Add(_doc.GetElement(item));        
        }

 
    }
}
