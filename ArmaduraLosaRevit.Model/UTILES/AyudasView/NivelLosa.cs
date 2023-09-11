using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.AyudasView
{
   public class NivelLosa
    {
        public Level Nivel_ { get; set; }
        public double COtaSuperior { get; set; }
        public double CotaReal { get; set; }
        public double CotaInferior { get; set; }
    }
}
