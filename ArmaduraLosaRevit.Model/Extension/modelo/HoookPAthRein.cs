using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension.modelo
{
    public class HookPAthRein
    {

        public RebarHookType rebarHookTypePrincipal_star { get; set; }
        public RebarHookType rebarHookTypeAlternativo_star { get; set; }
        public RebarHookType rebarHookTypePrincipal_end { get; set; }
        public RebarHookType rebarHookTypeAlternativo_end { get; set; }
        public bool isOk { get; set; }
        public HookPAthRein(RebarHookType rebarBarType1_star, RebarHookType rebarBarType2_star, RebarHookType rebarBarType1_end, RebarHookType rebarBarType2_end)
        {
            this.rebarHookTypePrincipal_star = rebarBarType1_star;
            this.rebarHookTypeAlternativo_star = rebarBarType2_star;
            this.rebarHookTypePrincipal_end = rebarBarType1_end;
            this.rebarHookTypeAlternativo_end = rebarBarType2_end;
            isOk = true;
        }

        public HookPAthRein()
        {
            isOk = false;
        }
    }

}
