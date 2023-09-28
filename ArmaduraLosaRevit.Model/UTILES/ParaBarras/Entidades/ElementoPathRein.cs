using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades
{
    public class ElementoPathRein : ElementoPath
    {

        public PathReinforcement _pathReinforcement { get; set; }
        public List<Element> _lista_A_DeRebarInSystem { get; set; }
        List<ElementoPathRebarInSystem> _lista_A_DeRebarInSystemv2 { get; set; }
        //public List<ElementoPathRebarInSystem> _lista_A_DeRebarInSystem { get; set; }


        public static ElementoPathRein ObtenerElementoPathReinConElement(Document _doc, ElementId elemId)
        {
            if (elemId == null) return null;
            var elem = _doc.GetElement(elemId);
            return ObtenerElementoPathReinConPathReinSpanSymbol(_doc, elem);
        }
        public static ElementoPathRein ObtenerElementoPathReinConPathReinSpanSymbol(Document _doc, Element _elem)
        {
            if (!(_elem is PathReinSpanSymbol)) return null;

            PathReinSpanSymbol _independentTag = _elem as PathReinSpanSymbol;
            if (_independentTag == null) return null;

            PathReinforcement _PathRein = _independentTag.Obtener_GetTaggedLocalElement(null) as PathReinforcement; ;  //_doc.GetElement(_independentTag.Obtener_GetTaggedLocalElement()) as PathReinforcement;
            if (_PathRein == null) return null;

            List<IndependentTag> listaIndependentTag = TiposPathReinTagsEnView.M1_GetFamilySymbol_ConPathReinforment(_PathRein.Id, _doc, _doc.ActiveView.Id);
            if (listaIndependentTag.Count == 0)
            {
                Util.InfoMsg($"PathReinfome Id:{_PathRein.Id} \n\nNO cuenta con ningun tag que indique cuantias o largo de barras.");
                return null;
            }
            var _elementoPathRein = ObtenerElementoPathRein(_doc, _PathRein, _independentTag, listaIndependentTag.Select(c => (Element)c).ToList());
            return _elementoPathRein;
        }

        public static ElementoPathRein ObtenerElementoPathRein(Document _doc, PathReinforcement _createdPathReinforcement, Element pathSymbol, List<Element> tagpath)
        {

            // tipo barra
            TipoBarra tipoBarra = M1_2_1_OBtenerTipoBarra(_createdPathReinforcement, _doc, "IDTipo");

            TipoDireccionBarra tipoDireccionBarra_ = M1_2_2_OBtenerTipoDireccion(_createdPathReinforcement, _doc);
            //ubicacion
            UbicacionLosa ubicacionLosa = M1_2_3_OBtenerTipoBarraDireccion(_createdPathReinforcement, _doc, "IDTipoDireccion");

            TipoConfiguracionBarra tipoConfiguracionBarra = ObtenerM1_2_4_TipoConfiguracionBarra(tipoBarra);
            int diametroBarra = M1_2_5_ObtenerDiametro(_createdPathReinforcement, _doc);

            #region obtener rebarInSystem -- puede ser 1 o 2 elemntos si tiene ahorro
            var Ilist_RebarInSystem = _createdPathReinforcement.GetRebarInSystemIds();
            List<ElementId> ListElemId = Ilist_RebarInSystem.ToList();

            List<Element> _lista_A_DeRebarInSystem = new List<Element>();
            List<ElementoPathRebarInSystem> _lista_A_DeRebarInSystemv2 = new List<ElementoPathRebarInSystem>();

            foreach (var item in ListElemId) //ListElemId puede ser 1 o 2 elemntps RebarInSystem  __> pirncipal o secunedaria para el caso com ahorro
            {
                RebarInSystem rebarInSystem = (RebarInSystem)_doc.GetElement(item);
                _lista_A_DeRebarInSystem.Add(rebarInSystem);

                ElementoPathRebarInSystem visibilidadElementoPathDTO =
                    ElementoPathRebarInSystem.CrearVisibilidadElementoRebarhDTO(_doc, rebarInSystem, _createdPathReinforcement, new List<Element>());
                _lista_A_DeRebarInSystemv2.Add(visibilidadElementoPathDTO);
            }
            #endregion
            TipoBarraGeneral TipoBarraGeneral_ = Tipos_Barras.M2_Buscar_TipoGrupoBarras_pornombre(tipoBarra.ToString());
            //ObtenerTipoBarra _tipoBarraGeneral = AyudaObtenerParametros.ObtenerTipoBarraRebra(_createdPathReinforcement);

            return new ElementoPathRein(_createdPathReinforcement, pathSymbol, _lista_A_DeRebarInSystem, _lista_A_DeRebarInSystemv2, tagpath, ubicacionLosa, tipoConfiguracionBarra, tipoBarra, tipoDireccionBarra_, diametroBarra,
               TipoBarraGeneral_);
        }




        public ElementoPathRein(PathReinforcement _pathReinforcement, Element pathSymbol, List<Element> ListElem, List<ElementoPathRebarInSystem> _lista_A_DeRebarInSystemv2, List<Element> tagpath
                                        , UbicacionLosa orientacionBarra, TipoConfiguracionBarra tipoconfiguracionBarra,
                                        TipoBarra tipoBarra, TipoDireccionBarra tipoDireccionBarra_, int diametroBarra, TipoBarraGeneral TipoBarraGeneral)
            : base(tagpath, orientacionBarra, tipoconfiguracionBarra, tipoBarra, tipoDireccionBarra_, diametroBarra, TipoBarraGeneral)
        {
            this._pathReinforcement = _pathReinforcement;
            this.pathSymbol = pathSymbol;
            this._lista_A_DeRebarInSystem = ListElem;
            this._lista_A_DeRebarInSystemv2 = _lista_A_DeRebarInSystemv2;

        }


        public List<ElementId> ObtenerListaIdPath()

        {
            List<ElementId> ele = new List<ElementId>();
            ele.Add(_pathReinforcement.Id);
            ele.Add(pathSymbol.Id);
            if (ListTagpath.Count > 0)
                ele.AddRange(ListTagpath.Select(c => c.Id).ToList());
            return ele;
        }


    }

}
