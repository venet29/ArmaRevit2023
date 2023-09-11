using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ParametosShare.Actualizar
{
    public class ActualizarNUmeroTraba7_5
    {
        protected Document _doc;
        protected Element _barraAnalizada;
        protected UIApplication _uiapp;
        protected View _view;


        public ActualizarNUmeroTraba7_5(UIApplication _uiapp, View _view)
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
            catch (Exception ex)
            {
                Util.ErrorMsg("Error en viewSection:" + _view.Name + $" ex:{ex.Message}");
            }
        }


        private void Ejecutarcomando(List<Element> ElementosIDs)
        {
            //view.IsolateElementsTemporary(ElementosIDs);
            for (int i = 0; i < ElementosIDs.Count; i++)
            {
                _barraAnalizada = ElementosIDs[i];
                if (_barraAnalizada == null) continue;

                var resultpara =ParameterUtil.FindParaByName(_barraAnalizada, "CantidadEstriboTRABA");
                if (resultpara == null) continue;
                var result = resultpara.AsString();
                if (!result.Contains("7,5")) continue;
                if (ParameterUtil.FindParaByName(_barraAnalizada, "CantidadEstriboTRABA") != null)
                    ParameterUtil.SetParaStringNH(_barraAnalizada, "CantidadEstriboTRABA", result.Replace("7,5","7.5"));  //"nombre de vista"


            }
        }
    }
}
