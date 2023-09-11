
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    //modifica los elementos seleccionados
    public class VisibilidadTipoBarraLosas : VisibilidadElemenGraphicV2

    {
          private SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad;
        private readonly SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad;

        public VisibilidadTipoBarraLosas(UIApplication _uiapp, SeleccionarRebarVisibilidad seleccionarRebarElemento,
            SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad,View _viewAnalizado) : base(_uiapp, _viewAnalizado)
        {
            this._SeleccionarRebarVisibilidad = seleccionarRebarElemento;
            this._SelecPathReinVisibilidad = _SelecPathReinVisibilidad;
        }

        public  bool Ejecutar( View _view)
        {
            try
            {
                // para malla
                CamabiarBArrasREfuezoLosa();
                CamabiarBArrasEstribo();

            }
            catch (Exception Ex)
            {
                Util.ErrorMsg($"Error al cambiar formato View :{_view.Name}   \n ex:{Ex.Message} ");
                return false;
            }

            return true;
        }

        private void CamabiarBArrasREfuezoLosa()
        {
            List<ElementId> Listid = _SeleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion
                                                                          .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_BA_REF_LO
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_BA_CAB_MURO
                                                                                || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_BA_BORDE
                                                                                || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_BA)
                                                                        .Select(c => c.element.Id).ToList();

            GraphicSettingDTO ss = FActoryGraphicSettingsBarrasElevLosa.ObtenerbarrasRefuerzoLosa(_doc);
            OverrideGraphicSettings ogs_malla = Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(ss);

            ChangeListElementsColorSinTrans(Listid, ogs_malla);
        }

        private void CamabiarBArrasEstribo()
        {
            List<ElementId> Listid = _SeleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion
                                                                          .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_REF_LO ||
                                                                                       c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.REFUERZO_EST_BORDE )
                                                                        .Select(c => c.element.Id).ToList();

            GraphicSettingDTO ss = FActoryGraphicSettingsBarrasElevLosa.ObtenerbarrasEstriboLosa();
            OverrideGraphicSettings ogs_malla = Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(ss);

            ChangeListElementsColorSinTrans(Listid, ogs_malla);
        }


    }
}
