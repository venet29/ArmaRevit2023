using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ParametosShare.Actualizar
{
    public class ActualizarLargoBarras
    {
        private Document _doc;
        private Element _barraAnalizada;
        private UIApplication _uiapp;
        private View _view;
#pragma warning disable CS0169 // The field 'ActualizarLargoBarras.nombreActualView' is never used
        private string nombreActualView;
#pragma warning restore CS0169 // The field 'ActualizarLargoBarras.nombreActualView' is never used


        public static int cont { get; set; } = 0;
        public ActualizarLargoBarras(UIApplication _uiapp, View _view)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
            this._view = _view;

        }



        public void Ejecutar(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarNUmeroBarra-NH");
                    Ejecutarcomando(ElementosIDs);
                    trans2.Commit();
                    // uidoc.RefreshActiveView();nn
                } // fin trans 
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg("Error en viewSection:" + _view.Name);
            }
        }

        public void Ejecutar_SinTrans(PathReinforcement pathReinforcement)
        {
            List<Element> ElementosIDs = new List<Element>();
            ElementosIDs.Add(pathReinforcement);
            Ejecutar_SinTrans(ElementosIDs);
        }


        public void Ejecutar_SinTrans(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                Ejecutarcomando(ElementosIDs);

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg("Error en viewSection:" + _view.Name);
            }
        }

        private void Ejecutarcomando(List<Element> ElementosIDs)
        {
            //view.IsolateElementsTemporary(ElementosIDs);
            for (int i = 0; i < ElementosIDs.Count; i++)
            {
                _barraAnalizada = ElementosIDs[i];
                if (_barraAnalizada == null) continue;
                var largosPAthFoot = (_barraAnalizada as PathReinforcement).ObtenerLargoFoot_fund();
                if (largosPAthFoot == 0) continue;

                var largosPArciales = (_barraAnalizada as PathReinforcement).ObtenerLargoParcialFUndaciones_String();

                if (ParameterUtil.FindParaByName(_barraAnalizada, "LargoTotal") != null)
                    ParameterUtil.SetParaStringNH(_barraAnalizada, "LargoTotal", ((int)Util.FootToCm(largosPAthFoot)).ToString());  //"nombre de vista"


                if (ParameterUtil.FindParaByName(_barraAnalizada, "LargoTotal2") != null)
                    ParameterUtil.SetParaStringNH(_barraAnalizada, "LargoTotal2", ((int)Util.FootToCm(largosPAthFoot)).ToString());  //"nombre de vista"


                if (ParameterUtil.FindParaByName(_barraAnalizada, "LargoParciales") != null)
                    ParameterUtil.SetParaStringNH(_barraAnalizada, "LargoParciales", largosPArciales);  //"nombre de vista"

                //borrar solo se utiloz para el proyecto de devid 2020-029-  caceres
                if (ParameterUtil.FindParaByName(_barraAnalizada, "LargoParciales2") != null)
                    ParameterUtil.SetParaStringNH(_barraAnalizada, "LargoParciales2", largosPArciales);  //"nombre de vista"
            }
        }
    }
}
