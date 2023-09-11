﻿using ArmaduraLosaRevit.Model.Enumeraciones;
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

        public static string VersionR { get; set; } = "";
        public static string ObtenerVersionRevit(UIApplication _uiapp=null)
        {
            if(VersionR!="") return VersionR;

            if (_uiapp == null)
            {
                Util.ErrorMsg("No se puede obtener version de revit");
                return "";
            }

             VersionR = _uiapp.Application.VersionNumber;

            return VersionR;
        }

        public static bool IsMAyorOIgual(UIApplication _uiapp, VersionREvitNh _VersionREvitNh)
        {
            string _version = _uiapp.Application.VersionNumber;

            switch (_VersionREvitNh)
            {
                case VersionREvitNh.v2018:
                    return (_version.Contains("2018")  ||_version.Contains("2019") || _version.Contains("2020") || _version.Contains("2021") || _version.Contains("2022") || _version.Contains("2023") || _version.Contains("2024") || _version.Contains("2025"));
                case VersionREvitNh.v2019:
                    return ( _version.Contains("2019") || _version.Contains("2020") || _version.Contains("2021") || _version.Contains("2022") || _version.Contains("2023") || _version.Contains("2024") || _version.Contains("2025"));
                case VersionREvitNh.v2020:
                    return ( _version.Contains("2020") || _version.Contains("2021") || _version.Contains("2022") || _version.Contains("2023") || _version.Contains("2024") || _version.Contains("2025"));
                case VersionREvitNh.v2021:
                    return ( _version.Contains("2021") || _version.Contains("2022") || _version.Contains("2023") || _version.Contains("2024") || _version.Contains("2025"));
                case VersionREvitNh.v2022:
                    return (_version.Contains("2022") || _version.Contains("2023") || _version.Contains("2024") || _version.Contains("2025"));
                case VersionREvitNh.v2023:
                    return ( _version.Contains("2023") || _version.Contains("2024") || _version.Contains("2025"));
                case VersionREvitNh.v2024:
                    return ( _version.Contains("2024") || _version.Contains("2025"));
                case VersionREvitNh.v2025:
                    return (_version.Contains("2025"));
                default:
                    return false;
            }
        }

    }
}
