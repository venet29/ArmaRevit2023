using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf.Servicios
{
   public class PathReinfVerificacion
    {


        public PathReinfVerificacion()
        {

        }


        public bool VerificarDatos(DatosNuevaBarraDTO datosNuevaBarraDTO)
        {
            if (datosNuevaBarraDTO.nombreFamiliaRebarShape == "")
            {
                Util.ErrorMsg($"sin nombreFamiliaRebarShape");
                return false;
            }

            if (datosNuevaBarraDTO.tipoRebarShapePrincipal == null)
            {
                Util.ErrorMsg($"sin tipoRebarShapePrincipal");
                return false;
            }
            if (datosNuevaBarraDTO.LargoPathreiforment < Util.CmToFoot(10))
            {
                Util.ErrorMsg($"LargoPathreiforment valor min (10cm) --> {datosNuevaBarraDTO.LargoPathreiforment}");
                return false;
            }
            if (datosNuevaBarraDTO.EspaciamientoFoot < Util.CmToFoot(5))
            {
                Util.ErrorMsg($"Espaciamiento de barra menor a 5cm --> {datosNuevaBarraDTO.EspaciamientoFoot}");
                return false;
            }

            return true;
        }
    }

}
