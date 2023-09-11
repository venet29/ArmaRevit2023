
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class ManejadorVisibilidadElemenNoEnView
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ManejadorVisibilidadElemenNoEnView(UIApplication _uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
        }

        public string NombreAntiguoVista { get; private set; }

        public void Ejecutar(View _view, string nombreView)
        {
            try
            {


                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                SeleccionarAreaPath seleccionarAreaPath = new SeleccionarAreaPath(_uiapp, _view);
                VisibilidadElemenNoEnView VisibilidadElement = new VisibilidadElemenNoEnView(_uiapp);

                ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, seleccionarRebar, seleccionarAreaPath, _view, nombreView);

                ManejadorVisibilidad.M5_OcultarBarraNoElevacion();
                NombreAntiguoVista = ManejadorVisibilidad.NombreAntiguoVista;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
    }
}
