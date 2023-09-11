using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales
{
    public class ABarrasElevV_ConTrans: ABarrasBaseElev_ConTrans
    {

        public ABarrasElevV_ConTrans(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag):base( uiapp,  interBArraDto,  _newGeometriaTag)
        {
     
            _BarraTipo = interBArraDto.BarraTipo;
        }
       public bool GetIsNoProloganLosaArriba() {

            return true;
        }
        public virtual bool M1_DibujarBarra()
        {
            try
            {
                if(!M1_1_CalculosIniciales()) return false;
                //if (!ObtenerTipoRedondero()) return false;
                M0_CalcularCurva();
                M1_3_DibujarBarraCurve();
                _interBArraDto._parametrosInternoRebarDTO._texToCantidadoBArras = _interBArraDto._nuevaLineaCantidadbarra.ToString();
                M1_4_ConfigurarAsignarParametrosRebarshape(_interBArraDto._parametrosInternoRebarDTO, "");
                M1_5__ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing();
                M1_6_visualizar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;

        }
        public bool M1_1_DibujarBarraCOnfiguracion()
        {

            return true;
        }

        public virtual void M0_CalcularCurva()
        { }
    }
}
