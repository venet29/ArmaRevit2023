using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ParametrosShare.ConversorVersione
{
    public class AyudaConversor
    {


        public static bool nombreFuncion(UIApplication _uiapp)
        {
            try
            {
                bool IsmayorOIgual2022 = UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022);

                string s = IsmayorOIgual2022 ? "ExternalDefinitonCreationOptions" : "ExternalDefinitionCreationOptions";

                Type _external_definition_creation_options_type = System.Reflection.Assembly.GetExecutingAssembly().GetType(s);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
