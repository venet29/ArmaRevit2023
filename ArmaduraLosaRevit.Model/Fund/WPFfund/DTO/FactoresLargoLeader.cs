using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.WPFfund.DTO
{
    public class FactoresLargoLeader
    {
        public static double FactorDesplazaminetoPotFree_foot { get; set; } = Util.CmToFoot(15);

        public static double FactorLargoCOdo_foot { get; set; } = -1;
    

        //  public static double FactorDesplazaminetoTag { get; set; } = Util.CmToFoot(80);

        static double factorDesplazaminetoTag_cm = 20;
        public static double FactorDesplazaminetoTag_foot
        {
            get
            {
                return Util.CmToFoot(factorDesplazaminetoTag_cm + 60);
            }
            set
            {
                factorDesplazaminetoTag_cm = Util.FootToCm(value);
            }
        }

        public static bool IsDefinirLargoCOdo { get; set; } = false;


        public static double LargoPaTaIzq_cm { get; set; } = 25;

        public static double LargoPaTaDere_cm { get; set; } = 25;
        public static string TipoFundacion { get; internal set; }
    }
}
