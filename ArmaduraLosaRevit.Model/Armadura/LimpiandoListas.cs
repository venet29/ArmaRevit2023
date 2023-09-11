using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
   public class LimpiandoListas
    {

        public static void Limpiar()
        {

            ABuscarTipo.ListaFamilias = new Dictionary<string, Element>();

            BuscadorParametroCompartido.ListaFamilias = new Dictionary<string, Definition>();

            //  TipoDiametrosBarrasDTO.Limpiar();
            TipoRebarHookType.Limpiar();

            Tipos_Arrow.Limpiar();

            Tipos_ArrowFamily.Limpiar();

            Tipos_Barras.Limpiar();

            TiposLineaPattern.Limpiar();

            //TiposRebarTagsEnView ;  calse no estatica

            TiposTextNote.Limpiar();

            TiposViewFamily.Limpiar();

            TiposView.Limpiar();

            TiposAreaReinTags.ListaFamilias= new Dictionary<string, Element>();

            TiposGenericAnnotation.ListaFamilias = new Dictionary<string, FamilySymbol>();
            
            TiposPathReinSpanSymbol.ListaFamilias = new Dictionary<string, Element>();

            TiposPathReinTags.ListaFamilias = new Dictionary<string, Element>();

            TiposPathReinTagsEnView.ListaFamilias = new Dictionary<string, Element>();
            
            TiposPathReinTagsFamilia.ListaFamilias = new Dictionary<string, Family>();

            //falta TiposRebarBarType =

            TiposRebarShapeFamilia.ListaFamilias = new Dictionary<string, Family>();

            TiposRebarTag.ListaFamilias = new Dictionary<string, Element>();

            TiposRoomTagEnView.ListaFamilias = new Dictionary<string, Element>();

            TiposPathReinformentSymbolElement.pathReinforcementTypeId = null;

            TiposFormasRebarShape.ListaFamilias = new Dictionary<string, RebarShape>();

            TiposRebarBarType.ListaFamilias= new Dictionary<string, RebarBarType>();

            Tipos_FiilRegion.Limpiar();
        }

        internal static void ListaFamilias()
        {
            string listaPAra = $"  TiposAreaReinTags :{(TiposAreaReinTags.ListaFamilias == null ? 0 : TiposAreaReinTags.ListaFamilias?.Count)}\n             " +
                $"  TiposGenericAnnotation :{(TiposGenericAnnotation.ListaFamilias == null ? 0 : TiposGenericAnnotation.ListaFamilias?.Count)}\n             " +
                $"  TiposPathReinSpanSymbol :{(TiposPathReinSpanSymbol.ListaFamilias == null ? 0 : TiposPathReinSpanSymbol.ListaFamilias?.Count)} \n            " +
                $"  TiposPathReinTags :{(TiposPathReinTags.ListaFamilias == null ? 0 : TiposPathReinTags.ListaFamilias?.Count)} \n            " +
                $"  TiposPathReinTagsEnView :{(TiposPathReinTagsEnView.ListaFamilias == null ? 0 : TiposPathReinTagsEnView.ListaFamilias?.Count)} \n            " +
                $"  TiposPathReinTagsFamilia :{(TiposPathReinTagsFamilia.ListaFamilias == null ? 0 : TiposPathReinTagsFamilia.ListaFamilias?.Count)}  \n           " +
                $"  TiposRebarShapeFamilia :{(TiposRebarShapeFamilia.ListaFamilias == null ? 0 : TiposRebarShapeFamilia.ListaFamilias?.Count)} \n            " +
                $"  TiposRebarTag :{(TiposRebarTag.ListaFamilias == null ? 0 : TiposRebarTag.ListaFamilias?.Count)} \n            " +
                $"  TiposRoomTagEnView :{(TiposRoomTagEnView.ListaFamilias == null ? 0 : TiposRoomTagEnView.ListaFamilias?.Count)}  \n           " +
                $"  TiposFormasRebarShape :{(TiposFormasRebarShape.ListaFamilias == null ? 0 : TiposFormasRebarShape.ListaFamilias?.Count)}   \n          " +
                $"  TiposRebarBarType :{(TiposRebarBarType.ListaFamilias == null ? 0 : TiposRebarBarType.ListaFamilias?.Count)}   \n          ";


            Util.InfoMsg(listaPAra);
        }
    }
}
