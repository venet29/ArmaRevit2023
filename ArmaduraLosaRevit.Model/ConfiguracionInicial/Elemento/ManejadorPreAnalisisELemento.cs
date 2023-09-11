using ArmaduraLosaRevit.Model.ConfiguracionInicial.model;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial.Elemento
{
    // clase para buscar si muros tiene coronocion, o fundaciones, o vigas tiene continudad al inicio y al final
    public class ManejadorPreAnalisisELemento
    {
        private readonly UIApplication _uiapp;
 
        public ManejadorPreAnalisisELemento(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            
        }

        public bool AnalisarMuros()
        {
            try
            {
                AnalisisMuros _analisisMuros = new AnalisisMuros(_uiapp);
                _analisisMuros.Inicio();
                if (_analisisMuros.BuscarMuros())
                {
                    _analisisMuros.AsignarCoronacion();
                  //  _analisisMuros.DesAsignar();
                }


              
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'AnalisarMuros'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
