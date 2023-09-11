using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Visibilidad.Entidades
{
    public class ElementoPathReinOriginal : IComparable
    {

        public PathReinforcement _pathReinforcement { get; set; }
        public List<Element> _lista_A_DeRebarInSystem { get; set; }
        public int DiametroBarra { get; }

        public Element pathSymbol { get; set; }
        public List<Element> tagpath { get; set; }
        public UbicacionLosa orientacionBarra { get; set; }
        public TipoDireccionBarra TipoDireccionBarra_ { get; set; }
        public TipoConfiguracionBarra tipoconfiguracionBarra { get; set; }
        public TipoBarra TipoBarra { get; set; }
        public TipoCaraObjeto TipoCaraObjeto_ { get; set; }

        public TipoConfiguracionBarra TipoBarraEnplano { get; set; }
        public static ElementoPathReinOriginal CrearVisibilidadElementoPathDTO(Document _doc, PathReinforcement _createdPathReinforcement, Element pathSymbol, List<Element> tagpath)
        {

            // tipo barra
            TipoBarra tipoBarra = M1_2_1_OBtenerTipoBarra(_createdPathReinforcement, _doc);

            TipoDireccionBarra tipoDireccionBarra_ = M1_2_2_OBtenerTipoDireccion(_createdPathReinforcement, _doc);
            //ubicacion
            UbicacionLosa ubicacionLosa = M1_2_3_OBtenerTipoBarraDireccion(_createdPathReinforcement, _doc);

            TipoConfiguracionBarra tipoConfiguracionBarra = ObtenerM1_2_4_TipoConfiguracionBarra(tipoBarra);
            int diametroBarra = M1_2_5_ObtenerDiametro(_createdPathReinforcement, _doc);

            #region obtener rebarInSystem -- puede ser 1 o 2 elemntos si tiene ahorro
            var Ilist_RebarInSystem = _createdPathReinforcement.GetRebarInSystemIds();
            List<ElementId> ListElemId = Ilist_RebarInSystem.ToList();

            List<Element> _lista_A_DeRebarInSystem = new List<Element>();
            foreach (var item in ListElemId) //ListElemId puede ser 1 o 2 elemntps RebarInSystem  __> pirncipal o secunedaria para el caso com ahorro
            {
                RebarInSystem rebarInSystem = (RebarInSystem)_doc.GetElement(item);
                _lista_A_DeRebarInSystem.Add(rebarInSystem);
            }
            #endregion

            return new ElementoPathReinOriginal(_createdPathReinforcement, ubicacionLosa, tipoConfiguracionBarra, tipoBarra, tipoDireccionBarra_, _lista_A_DeRebarInSystem, diametroBarra,
                                        pathSymbol, tagpath);
        }

        private static TipoBarra M1_2_1_OBtenerTipoBarra(PathReinforcement _pathReinforcement, Document _doc)
        {

            string result = ParameterUtil.FindValueParaByName(_pathReinforcement.Parameters, "IDTipo", _doc);
            if (result == null) return TipoBarra.NONE;
            return (result == "" ? TipoBarra.NONE : EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, result));
        }

        private static TipoDireccionBarra M1_2_2_OBtenerTipoDireccion(PathReinforcement _pathReinforcement, Document _doc)
        {
            string tipoDireccionBarra = ParameterUtil.FindParaByName(_pathReinforcement.Parameters, "TipoDireccionBarra").AsString();
            if (tipoDireccionBarra == null) return TipoDireccionBarra.NONE;
            if (tipoDireccionBarra.ToLower() == "i")
                return TipoDireccionBarra.Primaria;
            else if (tipoDireccionBarra.ToLower() == "s")
                return TipoDireccionBarra.Secundario;
            else
                return TipoDireccionBarra.NONE;



        }

        private static UbicacionLosa M1_2_3_OBtenerTipoBarraDireccion(PathReinforcement _pathReinforcement, Document _doc)
        {
            string result = ParameterUtil.FindValueParaByName(_pathReinforcement.Parameters, "IDTipoDireccion", _doc);
            return (result == "" ? UbicacionLosa.NONE : EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, result));
        }

        private static TipoConfiguracionBarra ObtenerM1_2_4_TipoConfiguracionBarra(TipoBarra tipoBarra)
        {
            TipoConfiguracionBarra resutlt = TipoConfiguracionBarra.NONE;
            if (tipoBarra.ToString().Contains("f") && ObtenerM1_2_4_1_IsDistintoBarraSUperior(tipoBarra))
            { resutlt = TipoConfiguracionBarra.refuerzoInferior; }
            else if (tipoBarra.ToString().Contains("s") || (!ObtenerM1_2_4_1_IsDistintoBarraSUperior(tipoBarra)))
            { resutlt = TipoConfiguracionBarra.suple; }

            return resutlt;
        }

        private static bool ObtenerM1_2_4_1_IsDistintoBarraSUperior(TipoBarra tipoBarra)
        {
            switch (tipoBarra)
            {
                case TipoBarra.f1_SUP:
                    return false;
                case TipoBarra.f9:
                    return false;
                case TipoBarra.f9a:
                    return false;
                case TipoBarra.f10:
                    return false;
                case TipoBarra.f10a:
                    return false;
            }
            return true;
        }

        private static int M1_2_5_ObtenerDiametro(PathReinforcement _pathReinforcement, Document _doc)
        {
            double aux_diametro = 0;

            Parameter elemntRebarType = ParameterUtil.FindParaByName(_pathReinforcement, "Primary Bar - Type");
            if (elemntRebarType == null) { Util.ErrorMsg("No se encuentra diametro barra"); return -1; }
            Element parametroBarType = _doc.GetElement2(elemntRebarType.AsElementId());
            bool result = double.TryParse(ParameterUtil.FindParaByBuiltInParameter(parametroBarType, BuiltInParameter.REBAR_BAR_DIAMETER, _doc), out aux_diametro);
            if (!result) { Util.ErrorMsg("No se encuentra diametro barra"); return -1; }
            int dimetroMM = UtilBarras.DimatrolDeFootaMM(aux_diametro);
            return dimetroMM;
        }



        public ElementoPathReinOriginal(PathReinforcement pathrein, UbicacionLosa orientacionBarra, TipoConfiguracionBarra tipoconfiguracionBarra,
                                        TipoBarra tipoBarra, TipoDireccionBarra tipoDireccionBarra_, List<Element> ListElemId, int diametroBarra,
                                        Element pathSymbol, List<Element> tagpath)
        {
            this.pathSymbol = pathSymbol;
            this.tagpath = tagpath;
            this.TipoDireccionBarra_ = tipoDireccionBarra_;
            this._pathReinforcement = pathrein;
            this.orientacionBarra = orientacionBarra;
            this.tipoconfiguracionBarra = tipoconfiguracionBarra;
            this.TipoBarra = tipoBarra;
            this._lista_A_DeRebarInSystem = ListElemId;
            this.DiametroBarra = diametroBarra;
        }




        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is ElementoPathReinOriginal)
            {

                ElementoPathReinOriginal otherPth = obj as ElementoPathReinOriginal;
                if (otherPth.tagpath != null)
                    return this._pathReinforcement.Id.IntegerValue.CompareTo(otherPth._pathReinforcement.Id.IntegerValue);
                else
                    throw new ArgumentException("El objeto no es ElementoPathRein");
            }
            else
            {

                throw new ArgumentException("El objeto comparado no es correcto");
            }
        }
    }

}
