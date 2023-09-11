using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
   public  class ConfiguracionBarraTrabaDTO
    {
        public int DiamtroTrabaEstriboMM { get; set; }

        public int CantidadTrabasTranversal { get; set; }
        public XYZ Ptobarra1 { get; set; }
        public XYZ Ptobarra2 { get; set; }

        public string  textoTraba { get; set; }
        public XYZ DireccionEntradoHaciaView { get; set; }

        public double EspesroMuroOVigaFoot { get; set; }
        public DireccionTraba UbicacionTraba { get; set; }

        public TipoTraba_posicion tipoTraba_posicion { get; set; } = TipoTraba_posicion.A;
        
        public XYZ DireccionEnfierrrado { get;  set; }
        public double[] listaEspaciamientoTrabasTransversal { get;  set; }
        public int CantidadTrabasLongitudinal { get; internal set; }
    }
}
