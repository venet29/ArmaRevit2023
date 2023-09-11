using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial.WPF
{
    class Manejador_COnf_Elevaciones
    {

        public static bool Ejecutar(UIApplication _uiapp)
        {
            try
            {


                Ui_Elevaciones _FormCub = new Ui_Elevaciones(_uiapp, ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV);
                _FormCub.ShowDialog();

                if (_FormCub.casoTipo == "btn_crear1vista")
                {
                    ManejadorConfiguracionInicialElevacion _ManejadorConfiguracionInicialElevacion = new ManejadorConfiguracionInicialElevacion(_uiapp, _FormCub.NombreTEmplete);
                    _ManejadorConfiguracionInicialElevacion.cargar();
                }
                else if (_FormCub.casoTipo == "btn_todasLASvistas")
                {
                    ManejadorConfiguracionInicialElevacion _ManejadorConfiguracionInicialElevacion = new ManejadorConfiguracionInicialElevacion(_uiapp, _FormCub.NombreTEmplete);
                    _ManejadorConfiguracionInicialElevacion.cargarTodos();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }
    }
}
