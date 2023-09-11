using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Pasadas.WPF
{
    public class Manejador_PASADASBIM_en_elevaciones
    {

        public static bool Ejecutar(UIApplication _uiapp)
        {
            try
            {


                Ui_ElevacionesPasadaRegion _FormCub = new Ui_ElevacionesPasadaRegion(_uiapp, "PASADA_ROJA", "SOLIDO PASADAS");
                _FormCub.ShowDialog();

                if (_FormCub.casoTipo == "btn_crear1vista")
                {
                    MAnejadorCrearRegionConPAsada _MAnejadorCrearRegionConPAsada = new MAnejadorCrearRegionConPAsada(_uiapp, _FormCub.NombreFiilRegion, _FormCub.NombrePasada);
                    _MAnejadorCrearRegionConPAsada.Ejecutar();
                }
                else if (_FormCub.casoTipo == "btn_todasLASvistas")
                {
                    MAnejadorCrearRegionConPAsada _MAnejadorCrearRegionConPAsada = new MAnejadorCrearRegionConPAsada(_uiapp, _FormCub.NombreFiilRegion, _FormCub.NombrePasada);
                    _MAnejadorCrearRegionConPAsada.EjecutarTodasLasVista();
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

        public static bool EjecutarVistaActual(UIApplication _uiapp,View  _view )
        {
            try
            {
                MAnejadorCrearRegionConPAsada _MAnejadorCrearRegionConPAsada = new MAnejadorCrearRegionConPAsada(_uiapp, "SOLIDO PASADAS","PASADA_ROJA");
                _MAnejadorCrearRegionConPAsada.Ejecutar(_view);
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
