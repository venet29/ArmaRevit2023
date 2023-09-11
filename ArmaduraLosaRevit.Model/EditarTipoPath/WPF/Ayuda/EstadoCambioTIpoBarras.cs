using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarTipoPath.WPF.Ayuda
{
    public class EstadoCambioTIpoBarras
    {
        //***********diamtro
        public string diametroActual { get; set; }
        public string diametroNuevo { get; set; }
        public bool IsCambioDiametro { get; set; }

        //****** espacieito
        public string EspacimeientoActual { get; set; }
        public string EspaciamientoNuevo { get; set; }
        public bool IsCambioEspacimeiento { get; set; }

        //******   tipo barra
        public string TipoBarraActual { get; set; }
        public string TipoBarraNuevo { get; set; }
        public bool IsCambioTipoBarra { get; set; }

        //****** orientacion
        public string OrientacionActual { get; set; }
        public string OrientacionNuevo { get; set; }
        public bool IsCambioOrientacion { get; set; }
        public EstadoCambioTIpoBarras()
        {
            IsCambioDiametro = false;
            IsCambioEspacimeiento = false;
            IsCambioTipoBarra = false;
            IsCambioOrientacion = false;
        }

        internal void ComprobarCambioTipoBarra() => IsCambioTipoBarra = (TipoBarraActual == TipoBarraNuevo ? false : true);
        internal void ComprobarCambioOrientacion() => IsCambioOrientacion = (OrientacionActual == OrientacionNuevo ? false : true);

        internal void ComprobarCambioEspacimeiento() => IsCambioEspacimeiento = (EspacimeientoActual == EspaciamientoNuevo ? false : true);
        internal void ComprobarCambiodiametro() => IsCambioDiametro = (diametroActual == diametroNuevo ? false : true);
    }
}
