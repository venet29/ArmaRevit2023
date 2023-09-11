using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.model
{
   public class VisibilizacionFilterDTO
    {
        public bool IsRebar { get; set; }
        public bool IsPAth { get; set; }

        public bool SectionBox { get; set; }
        public bool CropRegion { get; set; }
        
        public bool Muros { get; set; }
        public bool vigas { get; set; }
        public bool Losa{ get; set; }
        public bool Pilares { get; set; }
        public bool Fund { get; set; }

        internal static VisibilizacionFilterDTO TodoActivo()
        {
            return new VisibilizacionFilterDTO()
            {
                IsRebar = true,
                IsPAth = true,
                SectionBox = true,
                CropRegion = true
            };
        }
    }
}
