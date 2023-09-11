using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public  class GeomeTagArgs
    {

        public double angulorad { get; set; }
        public double diferenciaZInicialFinal { get; set; }
        public VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaInicialExternaInclinadaDTO { get; set; }
        public VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaFinalExternaInclinadaDTO { get; set; }

        public BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto_IzqInf { get; set; }
        public BuscarFaceSuperiorConPto _buscarFaceSuperiorConPto_DereSup { get; set; }

        public double deltaZ { get; set; }
        public double largoREcorridoDeltaZ { get; set; }

        //solo para refuelro losa -(tipo vigs)
        public string tipoPosicionEstribo { get; set; }
        public int _numeroTramosbarra { get; internal set; }
        public DireccionPata _ubicacionPata { get; internal set; }

        // para metros para refurzo de viga
        public XYZ DesplaminetoDirectriz_soloRefuerzoVIga{ get; set; }=  XYZ.Zero;
        public TagOrientation HorientacionTag { get;  set; }

        public static GeomeTagArgs ValorDefaul() =>  new GeomeTagArgs() { angulorad = Util.GradosToRadianes(90) };
    }
}
