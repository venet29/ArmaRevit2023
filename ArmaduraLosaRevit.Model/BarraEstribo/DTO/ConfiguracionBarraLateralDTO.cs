using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
   public  class ConfiguracionBarraLateralDTO
    {
        public int DiamtroTrabaEstriboMM { get; set; }

        
        public XYZ Ptobarra1 { get; set; }
        public XYZ Ptobarra2 { get; set; }

        public double EspesroMuroOVigaFoot { get; set; }
        public string  textoTraba { get; set; }
        public XYZ DireccionEntradoHaciaView { get; set; }
        public Element  ElementoSeleccionado { get; set; }

        public double LargoElementoSeleccionadoFoot { get; set; }
        public XYZ DireccionEnfierrrado { get;  set; }
        public double[] listaEspaciamientoTrabasTransversal { get;  set; }
        public XYZ DireccionMuro { get; internal set; }
        public XYZ PtoSeleccionMouseCentroCaraMuro { get; internal set; }
    }
}
