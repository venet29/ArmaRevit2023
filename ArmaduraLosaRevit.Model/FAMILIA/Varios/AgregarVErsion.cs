using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FAMILIA.Varios
{
   public class AgregarVErsion
    {
        public static string Ejecutar(string _VerSIon)
        {

                if (_VerSIon.Contains("2018")) return @"2018\familias\";

                if (_VerSIon.Contains("2019")) return @"2019\familias\";

                if (_VerSIon.Contains("2020")) return @"2020\familias\";

                if (_VerSIon.Contains("2021")) return @"2021\familias\";

                if (_VerSIon.Contains("2022")) return @"2022\familias\";

                if (_VerSIon.Contains("2023")) return @"2023\familias\";

                if (_VerSIon.Contains("2024")) return @"2024\familias\";

                if (_VerSIon.Contains("2025")) return @"2025\familias\";

                if (_VerSIon.Contains("2026")) return @"2026\familias\";
                return "";

        }

    }
}
