using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class ParametrosInternoRebarDTO
    {
        public string _texToCantidadoBArras { get; set; }
        public string _texToLargoParciales { get; set; }
        public string _texToLargoTotal { get; set; }
        public string _texToTipoDiam { get; set; }
        public bool _IsMalla { get; set; } = false;
        public string _cuantiaMalla { get; set; } = "";
        public string _IdMalla { get; set; } = "";
        public int _NUmeroLinea_paraTagRefuerzo { get; set; } = 0;
    }
}