
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades
{
    /*
     ElementoPath
     --- :ElementoPathRebar
     --- :ElementoPathRebarInSystem
     --- :ElementoPathRein
     --- :ElementoNoNe
    */
    public class ElementoPath : IComparable
    {


        public int DiametroBarra { get; }

        public Element pathSymbol { get; set; }
        public List<Element> ListTagpath { get; set; }
        public UbicacionLosa orientacionBarra { get; set; }
        public TipoDireccionBarra TipoDireccionBarra_ { get; set; }
        public TipoConfiguracionBarra tipoconfiguracionBarra { get; set; }
        public TipoBarra TipoBarra { get; set; }
        public TipoCaraObjeto TipoCaraObjeto_ { get; set; }

        public TipoBarraGeneral TipoBarraGeneral_ { get; set; }
        public TipoConfiguracionBarra TipoBarraEnplano { get; set; }
        public bool IsOK { get; set; } = true;

        protected static TipoBarra M1_2_1_OBtenerTipoBarra(Element _path, Document _doc,string tipoBarra)
        {

            string result = ParameterUtil.FindValueParaByName(_path.Parameters, "IDTipo", _doc);
            if (result == null) return TipoBarra.NONE;
            return (result == "" ? TipoBarra.NONE : EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, result));
        }

        protected static TipoDireccionBarra M1_2_2_OBtenerTipoDireccion(Element _path, Document _doc)
        {
            string tipoDireccionBarra = ParameterUtil.FindParaByName(_path.Parameters, "TipoDireccionBarra").AsString();
            if (tipoDireccionBarra == null) return TipoDireccionBarra.NONE;
            if (tipoDireccionBarra.ToLower() == "i")
                return TipoDireccionBarra.Primaria;
            else if (tipoDireccionBarra.ToLower() == "s")
                return TipoDireccionBarra.Secundario;
            else
                return TipoDireccionBarra.NONE;



        }

        protected static UbicacionLosa M1_2_3_OBtenerTipoBarraDireccion(Element _path, Document _doc,string nombreParametroTipo)
        {
            string result = ParameterUtil.FindValueParaByName(_path.Parameters, nombreParametroTipo, _doc);
            return (result == "" || result  ==null? UbicacionLosa.NONE : EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.NONE, result));
        }

        protected static TipoConfiguracionBarra ObtenerM1_2_4_TipoConfiguracionBarra(TipoBarra tipoBarra)
        {
            TipoConfiguracionBarra resutlt = TipoConfiguracionBarra.NONE;
            if (tipoBarra.ToString().Contains("f") && ObtenerM1_2_4_1_IsDistintoBarraSUperior(tipoBarra))
            { resutlt = TipoConfiguracionBarra.refuerzoInferior; }
            else if (tipoBarra.ToString().Contains("s") || (!ObtenerM1_2_4_1_IsDistintoBarraSUperior(tipoBarra)))
            { resutlt = TipoConfiguracionBarra.suple; }

            return resutlt;
        }

        internal IEnumerable<object> ObtenerListaIdPath()
        {
            throw new NotImplementedException();
        }

        protected static bool ObtenerM1_2_4_1_IsDistintoBarraSUperior(TipoBarra tipoBarra)
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

        protected static int M1_2_5_ObtenerDiametro(Element _path, Document _doc)
        {
            double aux_diametro = 0;

            Parameter elemntRebarType = ParameterUtil.FindParaByName(_path, "Primary Bar - Type");
            if (elemntRebarType == null) { Util.ErrorMsg("No se encuentra diametro barra"); return -1; }
            Element parametroBarType = _doc.GetElement2(elemntRebarType.AsElementId());
            bool result = double.TryParse(ParameterUtil.FindParaByBuiltInParameter(parametroBarType, BuiltInParameter.REBAR_BAR_DIAMETER, _doc), out aux_diametro);
            if (!result) { Util.ErrorMsg("No se encuentra diametro barra"); return -1; }
            int dimetroMM = UtilBarras.DimatrolDeFootaMM(aux_diametro);
            return dimetroMM;
        }



        public ElementoPath(List<Element> ListaTagPath,UbicacionLosa orientacionBarra, TipoConfiguracionBarra tipoconfiguracionBarra,
                                        TipoBarra tipoBarra, TipoDireccionBarra tipoDireccionBarra_, int diametroBarra, TipoBarraGeneral TipoBarraGeneral)
        {
            this.ListTagpath = ListaTagPath;
            this.TipoDireccionBarra_ = tipoDireccionBarra_;
            this.orientacionBarra = orientacionBarra;
            this.tipoconfiguracionBarra = tipoconfiguracionBarra;
            this.TipoBarra = tipoBarra;
            this.DiametroBarra = diametroBarra;
            this.TipoBarraGeneral_ = TipoBarraGeneral;
            this.IsOK = true;
        }

        public ElementoPath()
        {
            this.IsOK = false; 
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is ElementoPathReinOriginal)
            {

                ElementoPathReinOriginal otherPth = obj as ElementoPathReinOriginal;
                if (otherPth.tagpath != null)
                    return otherPth._pathReinforcement.Id.IntegerValue.CompareTo(otherPth._pathReinforcement.Id.IntegerValue);
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
