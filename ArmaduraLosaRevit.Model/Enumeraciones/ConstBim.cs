using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Enumeraciones
{
   public class ConstBim
    {

        #region generar dimensiones de pasadas respectp a grilla
        public static double CONST_ANCHO_DIMENSIONES = Util.CmToFoot(50);
        public static string CONST_NOMBRE_COTA = "COTAS";

        public static double CONST_LargoComprobarLargoMinimo_foot = Util.CmToFoot(50);
        public static double CONST_DesplazamientoDimExtremo_foot = Util.CmToFoot(30);
        public static double CONST_DesplazamientoDimCentral_foot = Util.CmToFoot(45);

        #endregion

        #region AgruparConductos //J:\_revit\PROYECTOS_REVIT\REVITArmaduraLosaRevit\ArmaduraLosaRevit_2022\ArmaduraLosaRevit.Model\Bim\AgruparConductos
        public static string nombreFlecha = "Flecha 15 grados rellenada";

        public static string BuitInCategoryDUct = "Conduits";

        public static string NombreDUctos = "Tubo metálico eléctrico (EMT)";

        public static string NombreFamiliaTAg = "M_Etiqueta GrupoConduit";
        #endregion


        #region SUmarLArgos
        public static string BuitInCategoryPipes = "Pipes";
        internal static string NombreFamiliaTAgPipes="Piepes";
        public static string BuitInCategory_shaftOpening = "Shaft Openings";

        
        #endregion
    }
}
