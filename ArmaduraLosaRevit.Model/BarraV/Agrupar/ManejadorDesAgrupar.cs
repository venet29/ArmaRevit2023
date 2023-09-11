using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Contener;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.model;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.ServicioAgrupar;
using ArmaduraLosaRevit.Model.Extension;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar
{
    public class ManejadorDesAgrupar
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public ManejadorDesAgrupar(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }


        public Result Ejecutar()
        {
          
            try
            {
                AlmacenerNiveles.ListaNiveles = new List<double>();
                AlmacenerNiveles.UltimoPtoTag = XYZ.Zero;

                SeleccionarTagRebarVerticales seleccionarTagRebarVerticales = new SeleccionarTagRebarVerticales(_uiapp);
                if (!seleccionarTagRebarVerticales.M1_EjecutarVertical()) return Result.Failed;


                DesAgrupadorBarrasManual _DesAgrupadorBarrasManual = new DesAgrupadorBarrasManual(_uiapp, seleccionarTagRebarVerticales.ListIndependentTag);
                _DesAgrupadorBarrasManual.GenerarListaBarras();//solo caso manual

                if (!_DesAgrupadorBarrasManual.DireccionDeTagVertical(seleccionarTagRebarVerticales.ptoMouse.AsignarZ(seleccionarTagRebarVerticales.ptoMouse.Z - ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT))) //solo manual
                    return Result.Failed;
                else
                {
                    _DesAgrupadorBarrasManual.ConfigurarDesAgrupar();
                    _DesAgrupadorBarrasManual.DibujarTagDeGrupo();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }
            return Result.Succeeded;
        }

 
    }
}
