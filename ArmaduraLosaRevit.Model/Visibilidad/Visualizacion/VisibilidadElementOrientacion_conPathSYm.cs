
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class VisibilidadElementOrientacion_conPathSYm : VisibilidadElementBase
    {
#pragma warning disable CS0108 // 'VisibilidadElementOrientacion_conPathSYm._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        protected readonly Document _doc;
#pragma warning restore CS0108 // 'VisibilidadElementOrientacion_conPathSYm._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        private readonly TiposBarrasDTo _tiposBarrasDTo;

        public VisibilidadElementOrientacion_conPathSYm(UIApplication _uiapp, TiposBarrasDTo tiposBarrasDTo) : base(_uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._tiposBarrasDTo = tiposBarrasDTo;
        }


        // no se ejecuta pq no se 
        public override void Ejecutar(List<List<ElementoPath>> lista_ElementoPath, View _view)
        {

            List<ElementoPath> lista_ElementoPathVisibles = lista_ElementoPath[0];
            List<ElementoPath> lista_ElementoPathOculto = lista_ElementoPath[1];

               M1_EjecutarOrientacion(_view, lista_ElementoPathVisibles, lista_ElementoPathOculto);



        }

        private void M1_EjecutarOrientacion(View _view, List<ElementoPath> lista_ElementoPathVisibles, List<ElementoPath> lista_ElementoPathOculto)
        {
            if (_tiposBarrasDTo.Orientacion == AccionTipoBarraOrientacion.Horizontal)
            {
                //ocultar


                //linea parche  pq como elemento se deberia usar  'ElementoPathRein' en vez de 'ElementoPath'
                List<Element> listaAPth_aux_visible = lista_ElementoPathVisibles.Where(tt => tt.orientacionBarra == UbicacionLosa.Inferior || tt.orientacionBarra == UbicacionLosa.Superior)
                                                                                .Select(ee => ((PathReinSpanSymbol)ee.pathSymbol).Obtener_TaggedLocalElement(_uiapp)).ToList();

                List<Element> listaSymbol_aux_visible = lista_ElementoPathVisibles.Where(tt => tt.orientacionBarra == UbicacionLosa.Inferior || tt.orientacionBarra == UbicacionLosa.Superior).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux_visible = lista_ElementoPathVisibles.Where(tt => tt.orientacionBarra == UbicacionLosa.Inferior || tt.orientacionBarra == UbicacionLosa.Superior).SelectMany(ee => ee.ListTagpath).ToList();

                vi1_OcultarElemento(listaAPth_aux_visible, _view);
                vi1_OcultarElemento(listaSymbol_aux_visible, _view);
                vi1_OcultarElemento(listatag_aux_visible, _view);


                //mostrar
                //linea parche  pq como elemento se deberia usar  'ElementoPathRein' en vez de 'ElementoPath'
                List<Element> listaAPth_aux = lista_ElementoPathOculto.Where(tt => tt.orientacionBarra == UbicacionLosa.Derecha || tt.orientacionBarra == UbicacionLosa.Derecha)
                                                                                .Select(ee => ((PathReinSpanSymbol)ee.pathSymbol).Obtener_TaggedLocalElement(_uiapp)).ToList();

                List<Element> listaSymbol_aux = lista_ElementoPathOculto.Where(tt => tt.orientacionBarra == UbicacionLosa.Derecha || tt.orientacionBarra == UbicacionLosa.Izquierda).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux = lista_ElementoPathOculto.Where(tt => tt.orientacionBarra == UbicacionLosa.Derecha || tt.orientacionBarra == UbicacionLosa.Izquierda).SelectMany(ee => ee.ListTagpath).ToList();

                Vi2_DesOcultarElemento_conTrans(listaAPth_aux, _view);
                Vi2_DesOcultarElemento_conTrans(listaSymbol_aux, _view);
                Vi2_DesOcultarElemento_conTrans(listatag_aux, _view);
            }
            else if (_tiposBarrasDTo.Orientacion == AccionTipoBarraOrientacion.Vertical)
            {

                //ocultar
                //linea parche  pq como elemento se deberia usar  'ElementoPathRein' en vez de 'ElementoPath'
                List<Element> listaAPth_aux_visible = lista_ElementoPathVisibles.Where(tt => tt.orientacionBarra == UbicacionLosa.Derecha || tt.orientacionBarra == UbicacionLosa.Izquierda)
                                                                                .Select(ee => ((PathReinSpanSymbol)ee.pathSymbol).Obtener_TaggedLocalElement(_uiapp)).ToList();

                List<Element> listaSymbol_aux_visible = lista_ElementoPathVisibles.Where(tt => tt.orientacionBarra == UbicacionLosa.Derecha || tt.orientacionBarra == UbicacionLosa.Izquierda).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux_visible = lista_ElementoPathVisibles.Where(tt => tt.orientacionBarra == UbicacionLosa.Derecha || tt.orientacionBarra == UbicacionLosa.Izquierda).SelectMany(ee => ee.ListTagpath).ToList();

                vi1_OcultarElemento(listaAPth_aux_visible, _view);
                vi1_OcultarElemento(listaSymbol_aux_visible, _view);
                vi1_OcultarElemento(listatag_aux_visible, _view);

                //mostar
                //linea parche  pq como elemento se deberia usar  'ElementoPathRein' en vez de 'ElementoPath'
                List<Element> listaAPth_aux = lista_ElementoPathOculto.Where(tt => tt.orientacionBarra == UbicacionLosa.Inferior || tt.orientacionBarra == UbicacionLosa.Superior)
                                                                                .Select(ee => ((PathReinSpanSymbol)ee.pathSymbol).Obtener_TaggedLocalElement(_uiapp)).ToList();
                List<Element> listaSymbol_aux = lista_ElementoPathOculto.Where(tt => tt.orientacionBarra == UbicacionLosa.Inferior || tt.orientacionBarra == UbicacionLosa.Superior).Select(ee => ee.pathSymbol).ToList();
                List<Element> listatag_aux = lista_ElementoPathOculto.Where(tt => tt.orientacionBarra == UbicacionLosa.Inferior || tt.orientacionBarra == UbicacionLosa.Superior).SelectMany(ee => ee.ListTagpath).ToList();

                Vi2_DesOcultarElemento_conTrans(listaAPth_aux, _view);
                Vi2_DesOcultarElemento_conTrans(listaSymbol_aux, _view);
                Vi2_DesOcultarElemento_conTrans(listatag_aux, _view);
            }
        }

    }
}
