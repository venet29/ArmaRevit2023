using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.DTO
{
    public class CasosFundDTO
    {
        public bool SuperiorHorizontal { get; set; }
        public bool SuperiorVertical { get; set; }
        public bool InferiorHorizontal { get; set; }
        public bool InferiorVertical { get; set; }
        public string CasoTipoBArra { get; internal set; }
        public CasosFundDTO()
        {
            SuperiorHorizontal = false;
            SuperiorVertical = false;
            InferiorHorizontal = false;
            InferiorVertical = false;
        }
    }
}
