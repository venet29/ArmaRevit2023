using ArmaduraLosaRevit.Model.BarraEstribo.Seleccion;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstriboV.Editar
{
    public class ManejarEditar
    {
        private  UIApplication _uiapp;
        private Document _doc;
#pragma warning disable CS0169 // The field 'ManejarEditar._view3D_BUSCAR' is never used
        private View3D _view3D_BUSCAR;
#pragma warning restore CS0169 // The field 'ManejarEditar._view3D_BUSCAR' is never used

        public ManejarEditar(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
        }

        public bool ReAjecutarEstribo()
        {

            try
            {
                EditarUbicacionEstribo _EditarUbicacionEstribo = new EditarUbicacionEstribo(_uiapp);
                _EditarUbicacionEstribo.ReAjecutarEstribo();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'ReAjecutarEstribo'. ex: {ex.Message}");
                return false;
            }
            return true;
        }

    }
}
