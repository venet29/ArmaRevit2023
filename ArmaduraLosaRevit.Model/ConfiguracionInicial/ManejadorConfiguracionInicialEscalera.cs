using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.Viewnh;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Prueba;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial
{
    public class ManejadorConfiguracionInicialEscalera
    {

        public static void cargar(UIApplication _uiapp)
        {
            if (_uiapp == null) return;
            try
            {

                View _view = _uiapp.ActiveUIDocument.ActiveView;
                ConfiguracionInicialParametros configuracionInicial = new ConfiguracionInicialParametros(_uiapp);
                configuracionInicial.AgregarParametrosShareEscalera();

                ManejadorVisibilidadElemenNoEnView _ManejadorVisibilidadElemenNoEnView = new ManejadorVisibilidadElemenNoEnView(_uiapp);
                _ManejadorVisibilidadElemenNoEnView.Ejecutar(_view, _view.Name);

                // esta opcion mas quenada es para resetear  3D_noEditar
                CreadorView CreadorView = new CreadorView(_uiapp);
                CreadorView.M2_CrearVIew3D(ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR);

                Util.InfoMsg("Datos cargados correctamente");


            }
            catch (Exception)
            {

                Util.InfoMsg("Error al cargar parametros");
            }
        }
    }
}
