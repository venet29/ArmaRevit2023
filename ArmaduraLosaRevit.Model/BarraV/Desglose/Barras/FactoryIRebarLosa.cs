using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos.Tipo;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos.Tipo.ParaElev;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos.Tipo.ParaPlanta;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos
{
     public class FactoryIRebarDesglose
    {

        public static IRebarLosa_Desglose CrearIRebarLosa(UIApplication _uiapp, RebarElevDTO _RebarElevDTO, IGeometriaTag _newIGeometriaTag)
        {


            switch (_RebarElevDTO.tipoBarra)
            {
                case TipoRebarElev.Sinpata:
                    return new BarraSinPatas(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataInferior:
                    return new BarraPataInicial(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.PataSuperior:
                    return new BarraPataSuperior(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.AmbasPata:
                    return new BarraPataAmbos(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.Estribo:
                    return new BarraEstriboTransConCurva_Plata(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboTraba:
                    return new BarraTrabaEstriboConCurva_Plata(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboViga:
                    return new BarraEstriboTransConCurva_Elev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                case TipoRebarElev.EstriboVigaTraba:
                    return new BarraTrabaEstriboConCurva_Elev(_uiapp, _RebarElevDTO, _newIGeometriaTag);
                default:
                    return new fx_null();

            }

    }



}
}
