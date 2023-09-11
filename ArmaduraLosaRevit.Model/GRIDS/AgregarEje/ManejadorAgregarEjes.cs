using System;
using ArmaduraLosaRevit.Model.GRIDS.AgregarEje.Servicios;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.GRIDS.AgregarEje
{
    class ManejadorAgregarEjes
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ManejadorAgregarEjes(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _view = _uiapp.ActiveUIDocument.Document.ActiveView;
        }

        public  bool Ejecutar(string nombreGridPrincipal)
        {
            try
            {
                ServicioObtenerInterseccionEntreGrid _ServicioObtenerInterseccionEntreGrid = new ServicioObtenerInterseccionEntreGrid(_uiapp);
                if (!_ServicioObtenerInterseccionEntreGrid.M2_Ejecutar()) return false;

                ServicioObtenerGridDeVIew _ServicioObtenerGridDeVIew = new ServicioObtenerGridDeVIew(_uiapp, _ServicioObtenerInterseccionEntreGrid.ListaEnvoltorioGrid);
                _ServicioObtenerGridDeVIew.Buscar(_view, nombreGridPrincipal);
                _ServicioObtenerGridDeVIew.CrearLineas(_view);

                //LogNH.Limpiar_sbuilder();
                //_ServicioObtenerInterseccionEntreGrid.ListaEnvoltorioGrid.ForEach(c => LogNH.Agregar_registro(c.Nombre + " -  pMax:" + c.MaximumOint.REdondearString_foot(2) + " -  PMin:" + c.MinimumOint.REdondearString_foot(2)+" ptoInter:"+ c.ListaGridIntersectado.Count));
                //LogNH.MostarVentaConDatos();
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
