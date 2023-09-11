using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales
{
  

    public class ABarrasElevV_SinTrans : ABarrasBaseElev_SinTrans
    {

        public ABarrasElevV_SinTrans(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {

            _BarraTipo = interBArraDto.BarraTipo;
       
        }

        public virtual bool M1_DibujarBarra()
        {
            try
            {
               // _interBArraDto.DireccionRecorridoBarra = -XYZ.BasisZ.CrossProduct(_interBArraDto.DireccionPataEnFierrado);

                if (!M1_1_CalculosIniciales()) return false;
               // if (!ObtenerTipoRedondero()) return false;
                M0_CalcularCurva();
                var result = M1_3_DibujarBarraCurve();

                _interBArraDto._parametrosInternoRebarDTO._texToCantidadoBArras = _interBArraDto._nuevaLineaCantidadbarra.ToString();
                if (result != Result.Succeeded) return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error  ex:{ex.Message}");
                return false;
            }
            return true;

        }

        public virtual bool M1_1_DibujarBarraCOnfiguracion()
        {
            try
            {
                if (_rebar == null) return false;
                if(!_rebar.IsValidObject) return false;
                M1_4_ConfigurarAsignarParametrosRebarshape(_interBArraDto._parametrosInternoRebarDTO, "");
                M1_5__ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing();
                M3_VerificarBarraMenor15mt();
                M1_6_visualizar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error  ex:{ex.Message}");
                return false;
            }
            return true;

        }

        public virtual void M0_CalcularCurva()
        { }

     
    }
}
