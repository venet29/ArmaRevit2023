using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace ArmaduraLosaRevit.Model.ParametosShare.Actualizar.Traba
{
    public class Actualizar_formatoPArametroTrabas
    {
        protected Document _doc;
        protected Element _barraAnalizada;
        protected UIApplication _uiapp;
        protected View _view;


        public Actualizar_formatoPArametroTrabas(UIApplication _uiapp, View _view)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
            this._view = _view;

        }


        public void Ejecutar_Trabas(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarNUmeroBarra-NH");
                    Ejecutarcomando_TRABAS(ElementosIDs);
                    trans2.Commit();
                    // uidoc.RefreshActiveView();nn
                } // fin trans 
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error en viewSection:" + _view.Name + $" ex:{ex.Message}");
            }
        }

   

        private void Ejecutarcomando_TRABAS(List<Element> ElementosIDs)
        {
            //view.IsolateElementsTemporary(ElementosIDs);
            for (int i = 0; i < ElementosIDs.Count; i++)
            {
                _barraAnalizada = ElementosIDs[i];
                if (_barraAnalizada == null) continue;

                var resultpara =ParameterUtil.FindParaByName(_barraAnalizada, "CantidadEstriboTRABA");
                if (resultpara == null) continue;
                var result = resultpara.AsString();
                if (!result.Contains("TR.Ø")) continue;

                if (ParameterUtil.FindParaByName(_barraAnalizada, "CantidadEstriboTRABA") != null)
                {
                    var lista = result.Split('Ø');
                    if(lista==null) continue;
                    if (lista.Length > 0)
                    {
                        ParameterUtil.SetParaStringNH(_barraAnalizada, "CantidadEstriboTRABA", lista[0]);  //"nombre de vista"
                    }
                    
                }


            }
        }


        internal void EjecutarLAterales(List<Element> ElementosIDs)
        {
            if (ElementosIDs == null) return;
            if (ElementosIDs.Count == 0) return;
            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("ActualizarNUmeroBarra-NH");
                    Ejecutarcomando_laTERALES(ElementosIDs);
                    trans2.Commit();
                    // uidoc.RefreshActiveView();nn
                } // fin trans 
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error en viewSection:" + _view.Name + $" ex:{ex.Message}");
            }
        }//***


        private void Ejecutarcomando_laTERALES(List<Element> ElementosIDs)
        {
            //view.IsolateElementsTemporary(ElementosIDs);
            for (int i = 0; i < ElementosIDs.Count; i++)
            {
                _barraAnalizada = ElementosIDs[i];
                if (_barraAnalizada == null) continue;

                var resultpara = ParameterUtil.FindParaByName(_barraAnalizada, "CantidadEstriboLAT");
                if (resultpara == null) continue;
                var result = resultpara.AsString();
                if (!result.Contains("LAT.")) continue;

                if (ParameterUtil.FindParaByName(_barraAnalizada, "CantidadEstriboLAT") != null)
                {
                    var lista = result.Split('Ø');
                    if (lista == null) continue;
                    if (lista.Length > 0)
                    {
                        ParameterUtil.SetParaStringNH(_barraAnalizada, "CantidadEstriboLAT", lista[0]);  //"nombre de vista"
                    }

                }


            }
        }
    }
}
