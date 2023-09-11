using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.Pasadas.Servicio;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Pasadas.Model
{


    public class EnvoltorioShaft
    {

        public XYZ puntoInsercion { get; set; }
        public ObjetosEncontradosDTO _ObjetosEncontradosDTO { get; set; }
        public Opening OpeningCreado { get; set; }
        public FamilyInstance Pasada { get; set; }
        public EstadoPasada Estado { get; set; } = EstadoPasada.Sinrevision;
        public PAsadasCuandradaDTO DatosPasadas { get; internal set; }

        public EnvoltorioShaft(ObjetosEncontradosDTO obj)
        {
            this._ObjetosEncontradosDTO = obj;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
