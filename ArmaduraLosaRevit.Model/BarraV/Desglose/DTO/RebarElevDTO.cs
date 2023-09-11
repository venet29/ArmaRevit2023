using ArmaduraLosaRevit.Model.AnalisisRoom.Servicios;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Stairsnh.Servicio;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.DTO
{
    //ver obs 1
    public class RebarElevDTO
    {

        public UIApplication uiapp { get; set; }
        public Element floor { get; set; }

        public ServicioModificarCoordenadasEscalera ServicioModificarCoordenadasEscalera { get; set; }

      
        public XYZ PtoDirectriz1 { get; set; }
        public XYZ PtoDirectriz2 { get; set; }
        public XYZ ptoini { get; set; }
        public XYZ ptofinal { get; set; }

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

        public double espesorBarraEnLOsaFooT { get; set; }

        public UbicacionLosa ubicacionLosa { get; set; }
        public TipoRebarElev tipoBarra { get; set; }
        public int numeroBarra { get; set; }
        public double largo_recorridoFoot { get; set; }
        public double anguloBarraGrados { get; set; }
        public double anguloBarraRad { get; set; }
        public double anguloTramoRad { get; set; }
        public double LargoPata { get; set; }
        public double largomin_1 { get; internal set; }
        public int AcortamientoEspesorSecundario { get; set; }
        public bool IsOK { get; internal set; }
        public double LargoPataF4 { get; set; }
        public TipoDireccionBarra TipoDireccionBarra_ { get; set; }
        public XYZ DireccionPataEnFierrado { get;  set; }
        public string Clasificacion { get; internal set; }
        public List<WraperRebarLargo> listaCUrvas { get; internal set; }
        public int Id { get; internal set; }
        public XYZ ptoMedio { get; internal set; }  // solo para corte transversal
        public TipoRebar TipoBarraEspecifico { get; internal set; }
        public List<WraperRebarLargo> ListaCurvaBarrasFinal_conCurva { get; internal set; }// // solo para corte transversal
        public XYZ ptocentroHost { get; internal set; }
        public View  _View { get;  set; }
        public Rebar Rebar_ { get; internal set; }
        public View _viewOriginal { get; internal set; }
        public Config_EspecialCorte Config_EspecialCorte { get; internal set; }
        public List<ParametroShareNhDTO> listaPArametroSharenh { get;  set; }
        public Config_EspecialElev Config_EspecialElv { get; internal set; }
        public double LargoTotalSumaParcialesFoot { get; internal set; }

        public RebarElevDTO(UIApplication _uiapp)
        {
            uiapp = _uiapp;           
            IsOK = false;
        }
    }
}