
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Traslapo.DTO;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;


namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO
{
    public class ContenedorDatosPathReinformeDTO
    {

        public double LargoPathreiforment;
        public CoordenadaPath coordenadas { get; set; }
        public IList<Curve> CurvesPathreiforment { get; }
        public ContenedorDatosLosaDTO datosLosaDTO { get; set; }
        public double AnguloP2toP3Rad { get; set; }
        public XYZ ptoTagSoloTraslapo { get; set; }

        public TipoPathReinfDTO tipoPathReinf { get; set; }
        public TipoConfiguracionBarra TipoConfiguracionBarra { get; internal set; }
        public List<XYZ> Lista4ptosPAth { get; internal set; }
        public double AnguloRoomRad { get; internal set; }
        public string Prefijo_F { get; internal set; }

        public ContenedorDatosPathReinformeDTO(ContenedorDatosLosaDTO datosLosaDTO, IList<Curve> CurvesPathreiforment, double LargoPathreiforment)
        {
            this.datosLosaDTO = datosLosaDTO;
            this.CurvesPathreiforment = CurvesPathreiforment;
            this.LargoPathreiforment = LargoPathreiforment;
            this.TipoConfiguracionBarra = TipoConfiguracionBarra.refuerzoInferior;
        }
        public ContenedorDatosPathReinformeDTO()
        {

        }
    }



}
