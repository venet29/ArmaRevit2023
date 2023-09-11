using ArmaduraLosaRevit.Model.ViewportnNH.model;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewportnNH.Servicios
{
    public class ServicioGenerarGoemtria
    {
        private UIApplication _uiapp;
        private List<ViewDTO> listaMismoSheet;
        public List<ViewGeom> ListaViewGeom { get; set; }
        public string NombreViewSheet { get; }

        public ServicioGenerarGoemtria(UIApplication uiapp,string nombreViewSheet, List<ViewDTO> listaMismoSheet)
        {
            this._uiapp = uiapp;
            this.NombreViewSheet = nombreViewSheet;
            this.listaMismoSheet = listaMismoSheet;
            this.ListaViewGeom = new List<ViewGeom>();
        }

        internal bool Calcular(double delta_AnnotationCrop_foot=0)
        {
            try
            {
                for (int i = 0; i < listaMismoSheet.Count; i++)
                {
                    var view_ = listaMismoSheet[i];
                    ViewGeom _ViewGeom = new ViewGeom(_uiapp,view_);
                    if (_ViewGeom.CAlcularGeometria(delta_AnnotationCrop_foot))
                        ListaViewGeom.Add(_ViewGeom);
                }

                double X_auxDespla = 0;
                for (int i = 0; i < ListaViewGeom.Count; i++)
                {
                    var viewGeo = ListaViewGeom[i];
                    viewGeo.X_sheet = viewGeo.PtoMin.X + X_auxDespla;
                    viewGeo.Y_sheet = viewGeo.PtoMin.Y + Util.CmToFoot(10);
                    X_auxDespla += viewGeo.Ancho + Util.CmToFoot(10);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ServicioGenerarGoemtria'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
