using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Traslapo.Help
{
    public class FactoryITraslapo
    {



        public static iCalculoDatosParaReinforment CreateNewPathReinformentV2(CoordenadaPath _4PtosPathReinf, XYZ puntoSeleccionMouse,
                                                           ContenedorDatosLosaDTO datosLosaYpathInicialesDTO, ICalculoTiposTraslapos calculoTiposTraslapos)
        {
            ContenedorDatosLosaDTO _datosLosaPath1DTO = null;
            ContenedorDatosLosaDTO _datosLosaPath2DTO = null;
            try
            {
                _datosLosaPath1DTO = new ContenedorDatosLosaDTO(calculoTiposTraslapos.TipoPathReinf_IzqBajo, datosLosaYpathInicialesDTO);
                _datosLosaPath2DTO = new ContenedorDatosLosaDTO(calculoTiposTraslapos.TipoPathReinf_DerArriba, datosLosaYpathInicialesDTO);
                return new CalculoDatosParaReinformentV2(_4PtosPathReinf, puntoSeleccionMouse, _datosLosaPath1DTO, _datosLosaPath2DTO);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'CreateNewPathReinformentV2' ex:{ex.Message}");
                return new CalculoDatosParaReinformentV2(_4PtosPathReinf, puntoSeleccionMouse, null, null, false);
            }
        }



    }
}
