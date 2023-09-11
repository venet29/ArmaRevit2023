using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath
{
    public class BarraAreasPathBase
    {
        private UIApplication _uiapp;

        private Document _doc;
        private View _view;
        public AreaReinforcement m_createdAreaReinforcement = null;
        public List<AreaReinforcement> ListaAreaReinforcement = null;
        public string nombreSimboloAreaReinforcement = "";
        protected XYZ DireccionMayor;

        public List<XYZ> ListaPtosPerimetroBarras { get; set; }


        #region Hoook

        //HOOK barra Principal
        // star es el que tiene x menor   
        // si es vertical start y menor
        public static RebarHookType tipodeHookStartPrincipal { get; set; }
        public static RebarHookType tipodeHookEndPrincipal { get; set; }

        //Rebar shape Principal y secundaria
        public static RebarShape tipoRebarShapePrincipal { get; set; }
        public static RebarShape tipoRebarShapeAlternativa { get; set; }


        // HOOK barra alternativa
        // star es el que tiene x menor   
        // si es vertical start y menor
        public RebarHookType tipodeHookStarAlternativa { get; set; }
        public RebarHookType tipodeHookEndAlternativa { get; set; }
        public bool IsBarrAlternative { get; set; }
        #endregion


        protected Element ElementHost;
        public double EspaciamientoHorizontal { get; set; }
        public double EspaciamientoVertical { get; set; }


#pragma warning disable CS0649 // Field 'BarraAreasPathBase.diametroEnMM_Horizontal' is never assigned to, and will always have its default value null
        private string diametroEnMM_Horizontal;
#pragma warning restore CS0649 // Field 'BarraAreasPathBase.diametroEnMM_Horizontal' is never assigned to, and will always have its default value null
#pragma warning disable CS0169 // The field 'BarraAreasPathBase.diametroEnMM_Vertical' is never used
        private string diametroEnMM_Vertical;
#pragma warning restore CS0169 // The field 'BarraAreasPathBase.diametroEnMM_Vertical' is never used
        private static RebarHookType _tipodeHookStartPrincipalH;
        private static RebarHookType _tipodeHookStartPrincipalv;

        private static ElementId _areaReinforcementTypeId;
        private static ElementId _rebarBarTypeId;

        public BarraAreasPathBase(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _doc = uiapp.ActiveUIDocument.Document;
            _view = _doc.ActiveView;

            _tipodeHookStartPrincipalH = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
            _tipodeHookStartPrincipalv = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);

            ListaAreaReinforcement = new List<AreaReinforcement>();
        }

        #region 2.1)  3 Metodos Crear AreaReinforcement



        /// <summary>
        /// crea refuerzo de area 
        /// </summary>
        /// <param name="selecFloor">floor seleccionada</param>
        /// <param name="curves">lista con curvas de area a reforzar</param>
        protected AreaReinforcement CrearAreaRefuerzo(Element ElementHost, IList<Curve> curves, ElementId rebarHookTypeId)
        {

            try
            {
                AreaReinDataOnWall dataOnFloor = new AreaReinDataOnWall();
                //int aux_vect = 1;

                //si caso vertical si vector es positivo (hacia arriba)  symbol path area crea una barras patas arriba
                // si caso vertical si vector es negativo (hacia bajo)  symbol path area crea una barras patas abajo --> suple

                //si caso horizontal si vector es positivo (hacia adelante)  symbol path area crea una barras patas arriba
                // si caso horizontal si vector es negativo (hacia atras)  symbol path area crea una barras patas abajo --> suple

                //  majorDirection = majorDirection * aux_vect;

                //Create AreaReinforcement
                if (_areaReinforcementTypeId == null)
                    _areaReinforcementTypeId = AreaReinforcementType.CreateDefaultAreaReinforcementType(_uiapp.ActiveUIDocument.Document);

                if (_rebarBarTypeId == null)
                    _rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(_uiapp.ActiveUIDocument.Document);
                // ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_revit.Application.ActiveUIDocument.Document);
                AreaReinforcement areaRein = AreaReinforcement.Create(_uiapp.ActiveUIDocument.Document, ElementHost, curves, DireccionMayor, _areaReinforcementTypeId, _rebarBarTypeId, rebarHookTypeId);

                //set AreaReinforcement and it's AreaReinforcementCurves parameters
                dataOnFloor.FillIn(areaRein);

                return areaRein;

            }
            catch (Exception ex)
            {
                _areaReinforcementTypeId = null;
                _rebarBarTypeId = null;
                Util.ErrorMsg($"ex: {ex.Message}");
                return null;
            }
        }



        protected bool Configurar_LayoutRebar_AreaRefuerzo(DatosMallasAutoDTO datosMallasDTO, AreaReinforcement m_createdAreaReinforcement)
        {
            if (m_createdAreaReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a AreaDeRefuerzo null ");
                return false;
            }


            //horizontal  major
            RebarBarType _rebarBarTypeH = TiposRebarBarType.getRebarBarType("Ø" + datosMallasDTO.diametroH_mm, _doc, true);


            if (null == _rebarBarTypeH) { Util.ErrorMsg("Error al obtener la familiar Ø" + datosMallasDTO.diametroH_mm + " LayoutRebar_AreaRefuerzo"); ; return false; }

            ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BACK_DIR_1, _rebarBarTypeH.Id); //"Bottom Minor Bar Type" 
            ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_FRONT_DIR_1, _rebarBarTypeH.Id); //	"Top Minor Bar Type" 

            ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_BACK_DIR_1, Util.CmToFoot(datosMallasDTO.espaciemientoH_cm));
            ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_FRONT_DIR_1, Util.CmToFoot(datosMallasDTO.espaciemientoH_cm));

            if (_tipodeHookStartPrincipalH == null)
                _tipodeHookStartPrincipalH = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);


            if (_tipodeHookStartPrincipalH != null)
            {
                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BACK_DIR_1, _tipodeHookStartPrincipalH.Id);
                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_FRONT_DIR_1, _tipodeHookStartPrincipalH.Id);
            }


            //verticalMayor

            RebarBarType rebarBarTypeV = TiposRebarBarType.getRebarBarType("Ø" + datosMallasDTO.diametroV_mm, _doc, true);
            if (null == rebarBarTypeV) { Util.ErrorMsg("Error al obtener la familiar Ø" + datosMallasDTO.diametroV_mm + " LayoutRebar_AreaRefuerzo"); ; return false; }


            ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BACK_DIR_2, rebarBarTypeV.Id);// 	"Top Major Direction" 
            ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_FRONT_DIR_2, rebarBarTypeV.Id);// 	"Top Minor Direction"


            ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_BACK_DIR_2, Util.CmToFoot(datosMallasDTO.espaciemientoV_cm));
            ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_FRONT_DIR_2, Util.CmToFoot(datosMallasDTO.espaciemientoV_cm));

            //GANCHO


            if (_tipodeHookStartPrincipalv != null && false)
            {
                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BACK_DIR_2, _tipodeHookStartPrincipalv.Id);
                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_FRONT_DIR_2, _tipodeHookStartPrincipalv.Id); ;
            }
            return true;
        }


        protected bool Configurar_geometria_AreaRefuerzo(DatosMallasAutoDTO datosMallasDTO, AreaReinforcement m_createdAreaReinforcement)
        {
            if (m_createdAreaReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a AreaDeRefuerzo null ");
                return false;
            }
            // rebarsystem 
            var list = m_createdAreaReinforcement.GetRebarInSystemIds();
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    RebarInSystem rebarInSystem = _doc.GetElement(list[i]) as RebarInSystem;

                    if (Util.IsVertical(rebarInSystem.Normal))
                    {
                        int cantidad = rebarInSystem.Quantity;
                        rebarInSystem.SetBarHiddenStatus(_view, 0, true);
                        rebarInSystem.SetBarHiddenStatus(_view, 1, true);
                        rebarInSystem.SetBarHiddenStatus(_view, 2, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CAmbiarColor2   ->   ex:{ex.Message}");
                return false;
            }

            return true;
        }


        /// <summary>
        /// confugura AreaReinforcement
        /// a) desactva capa superior
        /// b) desactiva barras horizontaes o deverticales segun sea el caso
        /// </summary>
        /// <param name="Orientacion"></param>
        protected bool LayoutRebar_AreaRefuerzo(TipoOrientacionBarra Orientacion)
        {
            if (m_createdAreaReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a AreaDeRefuerzo null " + ListaPtosPerimetroBarras.Count);
                return false;
            }

            ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1, 0);// 	"Top Major Direction" 
            ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2, 0);// 	"Top Minor Direction" 

            if (Orientacion == TipoOrientacionBarra.Horizontal)
            {
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 0);// 	 	"Bottom Major Direction" 

                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2, 1);// 	 	"Bottom Minor Direction" 

                RebarBarType rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametroEnMM_Horizontal, _doc, true);
                if (null == rebarBarType) { Util.ErrorMsg("Error al obtener la familiar Ø" + diametroEnMM_Horizontal + " LayoutRebar_AreaRefuerzo"); ; return false; }

                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BOTTOM_DIR_2, rebarBarType.Id);
                ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2, EspaciamientoHorizontal);
            }
            else
            {
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 1);// 	 	"Bottom Major Direction" 
                                                                                                                           //1) asigna el tipo de la barra en funcion del diametro, que debe estar creados en la libreria
                RebarBarType rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametroEnMM_Horizontal, _doc, true);
                if (null == rebarBarType) { Util.ErrorMsg("Error al obtener la familiar Ø" + diametroEnMM_Horizontal + " LayoutRebar_AreaRefuerzo"); ; return false; }
                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BOTTOM_DIR_1, rebarBarType.Id);
                ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_1, EspaciamientoHorizontal);

                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2, 0);// 	 	"Bottom Minor Direction" }

            }

            string str = "";
            ParameterSet pars = m_createdAreaReinforcement.Parameters;

            var adas = ParameterUtil.FindParaByName(pars, "REBAR_ELEMENT_VISIBILITY");

            foreach (Parameter param in pars)
            {
                string val = "";
                string name = param.Definition.Name;
                Autodesk.Revit.DB.StorageType type = param.StorageType;
                switch (type)
                {
                    case Autodesk.Revit.DB.StorageType.Double:
                        val = param.AsDouble().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.ElementId:
                        Autodesk.Revit.DB.ElementId id = param.AsElementId();
                        Autodesk.Revit.DB.Element paraElem = _doc.GetElement(id);
                        if (paraElem != null)
                            val = paraElem.Name;
                        break;
                    case Autodesk.Revit.DB.StorageType.Integer:
                        val = param.AsInteger().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.String:
                        val = param.AsString();
                        break;
                    default:
                        break;
                }
                str = str + name + ": " + val + "\r\n";
            }


            return true;
        }

        #endregion

    }
}
