using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.WPF.Component;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{


    public static class ExtensionRebarBarType
    {

        public static double ObtenerDiametroFoot(this RebarBarType _RebarBarType)
        {
            double diamfoot = 0;
            try
            {
                if (_RebarBarType == null)
                {
                    Util.ErrorMsg("RebarBarType igual null al obtener diamtro de barra ");
                    return 0;
                }

                //para buscar la propiedad 'BarNominalDiameter'  para versiones 2022 hacia arriba
                PropertyInfo prop = _RebarBarType.GetType().GetProperty("BarNominalDiameter");
                if (prop != null)
                {
                    diamfoot = (double)prop.GetValue(_RebarBarType, null);
                    return diamfoot;
                }
                // para buscar la propiedad 'BarNominalDiameter'  para versiones 2021 hacia abajo
                PropertyInfo prop2 = _RebarBarType.GetType().GetProperty("BarDiameter");
                if (prop2 != null)
                {
                    diamfoot = (double)prop2.GetValue(_RebarBarType, null);
                }

            }
            catch (Exception)
            {

                Util.ErrorMsg("Error al obtener Diamtro de barra 'Extension'");
            }

            return diamfoot;
        }



    }
}
