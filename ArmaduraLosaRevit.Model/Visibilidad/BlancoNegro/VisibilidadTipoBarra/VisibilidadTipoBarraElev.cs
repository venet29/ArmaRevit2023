
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
    public class VisibilidadTipoBarraElev : VisibilidadElemenGraphicV2
    {

        private SeleccionarRebarVisibilidad _SeleccionarRebarVisibilidad;

 
        public VisibilidadTipoBarraElev(UIApplication _uiapp, SeleccionarRebarVisibilidad seleccionarRebarElemento, View _viewAnalizado) : base(_uiapp, _viewAnalizado)
        {
            this._SeleccionarRebarVisibilidad = seleccionarRebarElemento;
        }

        public  bool Ejecutar( View _view)
        {
            try
            {
                // para malla
                CamabiarMAllas();
                CamabiarEstribo();
            }
            catch (Exception Ex)
            {
                Util.ErrorMsg($"Error al cambiar formato View :{_view.Name}   \n ex:{Ex.Message} ");
                return false;
            }

            return true;
        }

        private void CamabiarMAllas()
        {
            List<ElementId> Listid = _SeleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion
                                                                          .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_H
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_T
                                                                                || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_MA_V)
                                                                        .Select(c => c.element.Id).ToList();

            GraphicSettingDTO ss = FActoryGraphicSettingsDTO_BarrasElevEntrega.ObtenerCOnfMAllas();
            OverrideGraphicSettings ogs_malla = Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(ss);

            ChangeListElementsColorSinTrans(Listid, ogs_malla);
        }

        private void CamabiarEstribo()
        {
            List<ElementId> Listid = _SeleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion
                                                                          .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_CO_T
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_L
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_T
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_V
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VL
                                                                               || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_ES_VT)
                                                                        .Select(c => c.element.Id).ToList();

            GraphicSettingDTO ss = FActoryGraphicSettingsDTO_BarrasElevEntrega.ObtenerCOnfEstrivo_CO();
            OverrideGraphicSettings ogs_malla = Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(ss);

            ChangeListElementsColorSinTrans(Listid, ogs_malla);
        }

        private void CamabiarBArras()
        {
            List<ElementId> Listid = _SeleccionarRebarVisibilidad._lista_A_DeRebarVistaActualElevacion
                                                                          .Where(c => c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_V
                                                                                   || c.ObtenerTipoBarra.TipoBarra_ == TipoRebar.ELEV_BA_H)
                                                                        .Select(c => c.element.Id).ToList();

            GraphicSettingDTO ss = FActoryGraphicSettingsDTO_BarrasElevEntrega.ObtenerCOnfBArrasElev();
            OverrideGraphicSettings ogs_barras = Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(ss);

            ChangeListaElementColorConTrans(Listid, ogs_barras);
        }
    }
}
