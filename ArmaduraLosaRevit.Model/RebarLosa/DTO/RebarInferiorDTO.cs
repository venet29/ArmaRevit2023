using ArmaduraLosaRevit.Model.AnalisisRoom.Servicios;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Stairsnh.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.RebarLosa.DTO
{
    //ver obs 1
    public class RebarInferiorDTO
    {

        public UIApplication uiapp { get; set; }
        public Element floor { get; set; }

   
        public ServicioModificarCoordenadasEscalera ServicioModificarCoordenadasEscalera { get; set; }

        public List<XYZ> listaPtosPerimetroBarras { get; set; }
        public XYZ PtoDirectriz1 { get; set; }
        public XYZ PtoDirectriz2 { get; set; }
        public XYZ barraIni { get; set; }
        public XYZ barraFin { get; set; }

        public XYZ ptoSeleccionMouse { get; set; }


        public double diametroFoot { get; set; }


        private int _diametroMM; // field
        public int diametroMM   // property
        {
            get { return _diametroMM; }   // get method
            set
            {
                _diametroMM = value;
                diametroFoot = Util.MmToFoot(_diametroMM);
            }  // set method
        }

        public int largorecorrido { get; set; }

        public double espaciamientoFoot { get; set; }
        public int cantidadBarras { get; set; }
        public double espesorLosaFoot { get; set; }

        public double espesorBarraEnLOsa_sinRecub_FooT { get; set; }
        

        public UbicacionLosa ubicacionLosa { get; set; }
        public TipoBarra tipoBarra { get; set; }
        public int numeroBarra { get; set; }
        public double largo_recorridoFoot { get; set; }
        public double anguloBarraGrados { get; set; }
        public double anguloBarraRad { get; set; }
        public double anguloTramoRad { get; set; }
        public double LargoPata { get; set; }
        public double largomin_1 { get; internal set; }
        public int AcortamientoEspesorSecundario { get;  set; }
        public bool IsOK { get; internal set; }
        public double LargoPataF4 { get; set; }
        public TipoDireccionBarra TipoDireccionBarra_ { get;  set; }

        //**** solo fundaciones
        public PlanarFace planarfaceAnalizada { get; set; } // solo para fundaciones
        public TipoCaraObjeto TipoUbicacionFund { get; set; }// solo para fundaciones
        public DatosNuevaBarraDTO DatosNuevaBarraDTO { get;  set; }

        #region solo para barras rebar creadas en lfundaciones 31-08-2023
        public double LargoTotal { get; set; } = 0;
        public string TexToLargoParciales { get; set; } = "";
        #endregion

        //***

        public RebarInferiorDTO(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            listaPtosPerimetroBarras = new List<XYZ>();
            IsOK = false;
        }


        public SolicitudBarraDTO Obtener_solicitudBarraDTO()
        {
            return new SolicitudBarraDTO(uiapp, tipoBarra.ToString(), ubicacionLosa, TipoConfiguracionBarra.refuerzoInferior);
        }
    }
}