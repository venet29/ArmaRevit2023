using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class DatosMuroSeleccionadoDTO
    {
        // pto del borde de seleccion del elemento seleccionado
        public XYZ PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost { get; set; }
       
        public double EspesorElemetoHost { get; set; }
        public double LargoElemetoHost { get; set; }
        public XYZ NormalEntradoView { get;  set; } //vector normal entrando en la pantalla
        public XYZ DireccionRecorridoBarra { get; set; } //direccion en la qe se extiende el set de barrras
        public XYZ DireccionPataEnFierrado { get; set; }
        public XYZ DireccionLineaBarra { get; set; }
        public XYZ DireccionEnFierrado { get; set; }  // en la direccion del view  _seccionView.RightDirection o new XYZ(-_seccionView.RightDirection.X, -_seccionView.RightDirection.Y, _seccionView.RightDirection.Z)

        public Element elementoContenedor { get; set; }
        public ElementId IdelementoContenedor { get; set; }
        public BuscarViga vigaHost { get; set; }
        public BuscarMuros muroHost { get; set; }
        public XYZ DireccionMuro { get;  set; }
        public Orientacion orientacion { get; set; }
        public bool soloTag1 { get;  set; }
        public bool IsFundacion { get;  set; }
        public string Pier { get; internal set; }
        public string Story { get; internal set; }
        public bool IsLargoRecorrido { get;  set; }
  
        public double DesplazamientoVerticalFoot { get;  set; }
        public double LargoEspesorMalla_SinRecub_foot { get;  set; }
        public double _EspesorMuroFoot { get; internal set; }
        public bool IsCoronacion { get; internal set; }
        public XYZ ptoSeleccionMouseCentroCaraMuro6 { get; internal set; }
        public ElementoSeleccionado TipoElementoSeleccionado { get;  set; }
        public XYZ direccionBordeElemeto { get; internal set; }
    }
}
