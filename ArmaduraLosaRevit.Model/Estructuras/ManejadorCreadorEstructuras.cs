using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Estructuras
{
    class ManejadorCreadorEstructuras
    {
        private  UIApplication _uiapp;

        public ManejadorCreadorEstructuras(UIApplication uiapp)
        {
            this._uiapp = uiapp;
        }


        public bool CrearVigasConSacados()
        {

            try
            {
                CrearVigasConSacados _crearVigasConSacados = new CrearVigasConSacados(_uiapp);
                _crearVigasConSacados.Crear();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearVigasConSacados'. Ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
