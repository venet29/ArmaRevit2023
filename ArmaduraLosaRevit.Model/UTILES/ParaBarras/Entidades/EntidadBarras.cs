using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades
{
    public class EntidadBarras
    {


        public EntidadBarras(TipoRebar tipoRebar, string nombre, TipoBarraGeneral grupo,string nombreParaCub, Orientacion_Cub orientacion_Cub_, Elemento_cub _elemento_cub)
        {
            this.tipoRebar = tipoRebar;
            this.nombre = nombre;
            this.grupo = grupo;
            this.TipoParaCub = nombreParaCub;
            this.Orientacion_Cub_ = orientacion_Cub_;
            this.Elemento_Cub = _elemento_cub;
        }

        public TipoRebar tipoRebar { get; set; }
        public string nombre { get; set; }
        public TipoBarraGeneral grupo { get; set; }
        public string TipoParaCub { get; set; }
        public Orientacion_Cub Orientacion_Cub_ { get; }
        public Elemento_cub Elemento_Cub { get; }
    }
}
