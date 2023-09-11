using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
  public static class ExtensionRoom
    {

        public static string ObtenerNumero_Losa(this Room _room)
        {
            if (_room == null) return "";

            var nombrePara = ParameterUtil.FindParaByName(_room, "Numero Losa");
            if (nombrePara == null)
                return "";
            else
            {
                var result = nombrePara.AsString();

                if(result==null)
                    result = nombrePara.AsValueString();

                if (result == null)
                    return "";
                else
                    return result;

            }
        }
    }
}
