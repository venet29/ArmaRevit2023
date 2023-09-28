using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{
   public class UtilVersionesRevit
    {

        public static string VersionRvt { get; set; } = "";
        public static string ObtenerVersionRevit(UIApplication _uiapp=null)
        {
            if(VersionRvt!="" && _uiapp==null) return VersionRvt;

            if (_uiapp == null)
            {
                Util.ErrorMsg("No se puede obtener version de revit");
                return "";
            }

             VersionRvt = _uiapp.Application.VersionNumber;

            return VersionRvt;
        }

        public static bool IsMAyorOIgual(UIApplication _uiapp, VersionREvitNh _VersionREvitNh)
        {
            VersionRvt = ObtenerVersionRevit(_uiapp);

            switch (_VersionREvitNh)
            {
                case VersionREvitNh.v2018:
                    return (VersionRvt.Contains("2018")  ||VersionRvt.Contains("2019") || VersionRvt.Contains("2020") || VersionRvt.Contains("2021") || VersionRvt.Contains("2022") || VersionRvt.Contains("2023") || VersionRvt.Contains("2024") || VersionRvt.Contains("2025"));
                case VersionREvitNh.v2019:
                    return ( VersionRvt.Contains("2019") || VersionRvt.Contains("2020") || VersionRvt.Contains("2021") || VersionRvt.Contains("2022") || VersionRvt.Contains("2023") || VersionRvt.Contains("2024") || VersionRvt.Contains("2025"));
                case VersionREvitNh.v2020:
                    return ( VersionRvt.Contains("2020") || VersionRvt.Contains("2021") || VersionRvt.Contains("2022") || VersionRvt.Contains("2023") || VersionRvt.Contains("2024") || VersionRvt.Contains("2025"));
                case VersionREvitNh.v2021:
                    return ( VersionRvt.Contains("2021") || VersionRvt.Contains("2022") || VersionRvt.Contains("2023") || VersionRvt.Contains("2024") || VersionRvt.Contains("2025"));
                case VersionREvitNh.v2022:
                    return (VersionRvt.Contains("2022") || VersionRvt.Contains("2023") || VersionRvt.Contains("2024") || VersionRvt.Contains("2025"));
                case VersionREvitNh.v2023:
                    return ( VersionRvt.Contains("2023") || VersionRvt.Contains("2024") || VersionRvt.Contains("2025"));
                case VersionREvitNh.v2024:
                    return ( VersionRvt.Contains("2024") || VersionRvt.Contains("2025"));
                case VersionREvitNh.v2025:
                    return (VersionRvt.Contains("2025"));
                default:
                    return false;
            }
        }

    }
}
