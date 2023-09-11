using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Traslapos.servicio
{
    public class ObtenerTraslaposView
    {
        private readonly UIApplication uiapp;
        private Document _doc;
        private View _view;
        private List<TraslapoDTO> ListaTraslapo;

        public ObtenerTraslaposView(UIApplication _uiapp)
        {
            uiapp = _uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.Document.ActiveView;
            ListaTraslapo = new List<TraslapoDTO>();
        }

        public bool ObtenerLista()
        {

            try
            {

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg($"Error en ObtenerLista");

                return false;
            }
            return true;
        }
    }
}
