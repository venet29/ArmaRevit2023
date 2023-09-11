using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.ParametrosShare.Seleccionar
{
    public class GetListaRebarPathRein
    {
        private UIApplication _uiapp;
        private Document _doc;

        public GetListaRebarPathRein(UIApplication _uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
        }


        public void Ejecutar_Seleccionar()
        {
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            try
            {
                SeleccionarRebarOPathRein _seleccionarRebarOPathRein = new SeleccionarRebarOPathRein(_uiapp);
                _seleccionarRebarOPathRein.GetListaRebarPathReinV2();


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Util.InfoMsg($"Proceso Terminado. ");

        }

    }
}
