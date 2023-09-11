using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Pasadas.Model
{

    public enum EstadoPasada
    {
        Sinrevision,
        Validado,
        NoValidado,
        ConError,
        Revision,
        Rechazar,
        Correcion,
        ShaftCreado
    }
    public enum InterseccionPipe
    {
        NoAnalizado,
        SeEncontroInterseccion,
        NoSeEncontroInterseccion
    }
    public enum BuscandoFamilia
    {
        NoSeEncontro,
        SeEncontro,
        SeEncontroCorrido
    }

    
    public enum Orientacion3D
    {
        NS,
        WE,
        NE,
        SE,
        Vertical

    }

    public enum TipoPasada
    {
        DosCaras,
        Unacara

    }
    public enum ColorTipoPasada
    {
        PASADA_VERDE,
        PASADA_AZUL,
        PASADA_ROJA,
        PASADA_AMARILLO,
        PASADA_NARANJO,
        PASADA_GRIS
    }


}
