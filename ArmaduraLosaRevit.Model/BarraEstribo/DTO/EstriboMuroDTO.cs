using ArmaduraLosaRevit.Model.BarraEstribo.Barras;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
    public class EstriboMuroDTO
    {
        public double largoRecorridoEstriboFoot { get;  set; }

        public TipoEstriboGenera tipoEstriboGenera { get; set; }
        //distancia paralela a la vistaseccion view
        public double AnchoVisibleFoot { get;  set; }
        public double EspaciamientoEntreEstriboFoot { get;  set; }
        public int DiamtroBarraEnMM { get;  set; }
        public XYZ OrigenEstribo { get;  set; }
        public XYZ direccionPerpenEntradoHaciaViewSecction { get;  set; }
        public XYZ direccionParalelaViewSecctioin { get; set; }
        public double _anchoEstribo1Foot { get; set; }
        public Element ElementHost { get;  set; }
        public double Espesor_ElementHostFoot  { get; set; }
        public string cantidadEstribo { get; set; }
        public XYZ Posi1TAg { get; internal set; }
        public string NombreFamilia { get; internal set; }
        public XYZ direccionBarra { get;  set; }

        public List<BarraLateralesDTO> ListaLateralesDTO { get;  set; }
        public List<BarraTrabaDTO> ListaTrabasDTO { get; set; }

        public DireccionSeleccionMouse DireccionSeleccionConMouse { get; internal set; }
        
        // se debe borrar  cuadno se implemente trabas
        public string TextoAUXTraba { get; set; }
        public XYZ direccionTag { get; internal set; }
    }
}
