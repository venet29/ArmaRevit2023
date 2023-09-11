
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
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
    public class VisibilidadElementTipoBarra : VisibilidadElementBase
    {
#pragma warning disable CS0108 // 'VisibilidadElementTipoBarra._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        protected readonly Document _doc;
#pragma warning restore CS0108 // 'VisibilidadElementTipoBarra._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        private readonly TiposBarrasDTo _tiposBarrasDTo;

        public VisibilidadElementTipoBarra(UIApplication _uiapp, TiposBarrasDTo tiposBarrasDTo) : base(_uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._tiposBarrasDTo = tiposBarrasDTo;
        }

        public override void Ejecutar(List<List<ElementoPath>> lista_ElementoPath, View _view)
        {

            List<ElementoPath> lista_ElementoPathVisibles = lista_ElementoPath[0];
            List<ElementoPath> lista_ElementoPathOculto = lista_ElementoPath[1];

            M1_EjecutarCasosFx(_view, lista_ElementoPathVisibles, lista_ElementoPathOculto);

            M2_EjecutarCasosSx(_view, lista_ElementoPathVisibles, lista_ElementoPathOculto);

        }

        private void M1_EjecutarCasosFx(View _view, List<ElementoPath> lista_ElementoPathVisibles, List<ElementoPath> lista_ElementoPathOculto)
        {
            if (_tiposBarrasDTo.tipofx == AccionTipoBarra.Ver)
            {
                List<Element> listaSymbol_aux = lista_ElementoPathOculto.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.refuerzoInferior).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux = lista_ElementoPathOculto.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.refuerzoInferior).SelectMany(ee => ee.ListTagpath).ToList();

                Vi2_DesOcultarElemento_conTrans(listaSymbol_aux, _view);
                Vi2_DesOcultarElemento_conTrans(listatag_aux, _view);
            }
            else if (_tiposBarrasDTo.tipofx == AccionTipoBarra.Ocultar)
            {
                List<Element> listaSymbol_aux = lista_ElementoPathVisibles.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.refuerzoInferior).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux = lista_ElementoPathVisibles.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.refuerzoInferior).SelectMany(ee => ee.ListTagpath).ToList();

                vi1_OcultarElemento(listaSymbol_aux, _view);
                vi1_OcultarElemento(listatag_aux, _view);
            }
        }
        private void M2_EjecutarCasosSx(View _view, List<ElementoPath> lista_ElementoPathVisibles, List<ElementoPath> lista_ElementoPathOculto)
        {
            if (_tiposBarrasDTo.tiposx == AccionTipoBarra.Ver)
            {
                List<Element> listaSymbol_aux = lista_ElementoPathOculto.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.suple).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux = lista_ElementoPathOculto.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.suple).SelectMany(ee => ee.ListTagpath).ToList();

                Vi2_DesOcultarElemento_conTrans(listaSymbol_aux, _view);
                Vi2_DesOcultarElemento_conTrans(listatag_aux, _view);
            }
            else if (_tiposBarrasDTo.tiposx == AccionTipoBarra.Ocultar)
            {
                List<Element> listaSymbol_aux = lista_ElementoPathVisibles.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.suple).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux = lista_ElementoPathVisibles.Where(tt => tt.tipoconfiguracionBarra == TipoConfiguracionBarra.suple).SelectMany(ee => ee.ListTagpath).ToList();

                vi1_OcultarElemento(listaSymbol_aux, _view);
                vi1_OcultarElemento(listatag_aux, _view);
            }
        }
    }
}
