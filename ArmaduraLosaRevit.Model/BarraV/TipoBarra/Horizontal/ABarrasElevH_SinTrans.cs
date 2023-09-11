using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Horizontal
{
    public class ABarrasElevH_SinTrans : ABarrasBaseElev_SinTrans
    {


        public ABarrasElevH_SinTrans(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag):base( uiapp,  interBArraDto,  _newGeometriaTag)
        {
            

            _BarraTipo = interBArraDto.BarraTipo;


        }

        public bool M1_DibujarBarra()
        {
            try
            {
            
                M1_1_CalculosIniciales();

                M0_CalcularCurva();

                M1_3_DibujarBarraCurve();

                if (_rebar == null) return false;
                // IMPLENTAR ALFINA
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
        public bool M1_1_DibujarBarraCOnfiguracion() {

            return true;
        }
        public virtual void M0_CalcularCurva()
        { }
    }
}
