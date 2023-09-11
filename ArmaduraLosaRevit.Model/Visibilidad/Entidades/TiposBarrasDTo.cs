using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Entidades
{
    public enum AccionTipoBarra
    {
        Ver,
        Ocultar,
        Omitir
    }

    public enum AccionTipoBarraOrientacion
    {
        Horizontal,
        Vertical
    }


    public class TiposBarrasDTo
    {
        public AccionTipoBarra tipofx { get; set; }

        public AccionTipoBarra tiposx { get; set; }
        public AccionTipoBarra tiposRef { get; set; }
        public AccionTipoBarraOrientacion Orientacion { get; set; }

    }
}
