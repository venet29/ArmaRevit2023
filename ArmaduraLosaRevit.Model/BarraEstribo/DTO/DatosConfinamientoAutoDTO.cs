using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.BarraEstribo.DTO
{
    public class DatosConfinamientoAutoDTO
    {
        //estribo
        public int DiamtroEstriboMM { get; set; }
        public double espaciamientoEstriboCM { get; set; }
        public string cantidadEstribo { get; set; }
   

        //laterales
        public int DiamtroLateralEstriboMM { get; set; }
        public int cantidadLaterales { get; set; }

        //traba
        public int DiamtroTrabaEstriboMM { get; set; }
        public int cantidadTraba { get; set; }
        public double espaciamientoTrabaCM { get; set; }
        public double[] listaEspaciamientoTrabas { get; set; }


        //traba long
        public int DiamtroTrabaEstriboMM_long { get; set; }
        public int cantidadTraba_long { get; set; }
        public double espaciamientoTrabaCM_long { get; set; }


        public int _BarraInicialTrabaLong { get; set; }
        public int _BarraFinalTrabaLong { get; set; }

        public bool? IsLateral { get; set; } = false;
        public bool? IsEstribo { get; set; } = false;
        public bool? IsTraba { get; set; } = false;
        public bool? IsTrabaFalsa { get; set; } = false;


        // agregar dimensiones

        public bool IsDImensionPorBajarFUndacion { get; set; } = false;
        public XYZ ptoInicialDimension { get; set; }
        public XYZ ptoFinalDimension { get; set; }

        //crear
        public XYZnh centroPier { get; set; }
        public DireccionSeleccionMouse direccionSeleccionMOuse { get; set; }

        public TipoTraba_posicion tipoTraba_posicion { get; set; }

        public double espesor { get; set; }//cm

        public TipoEstriboGenera tipoEstriboGenera { get; set; }
        public TipoConfiguracionEstribo tipoConfiguracionEstribo { get; set; }

        public int opcionesConfinamiento { get; internal set; }
        public int CantidadBarrasPriemeraLinea { get; internal set; }
        public int CantidadLineasBarras { get; internal set; }

   
        public bool IsAgregarDetail { get; set; }
        public double largoVisible { get; set; }
        public TipoSeleccionMouse PtoInferior { get;  set; }
        public TipoSeleccionMouse PtoSuperior { get;  set; }
        public bool IsExtenderLatInicio { get;  set; } = true;
        public bool IsExtenderLatFin { get;  set; } = true;
        public TipoDisenoEstriboVIga TipoDiseñoEstriboViga { get; internal set; }
        public Element ElementoSeleccionado { get; internal set; }

        public string ObtenerTextBarra_Borrar()
        {
            return $"+{cantidadTraba+ cantidadTraba_long}TR.";

        }
    }
}
