using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cubicacion.model
{
    public enum visibi
    {
        no_dibujado,
        oculto,
        visible,
        no_analizado
    }
    public enum CasoRabar
    {
        Path_,
        Rebar_
    }
    public enum Caso_cub
    {
        Elev,
        Losa,
        Fund,
        None,
        ESC
    }

    public enum Orientacion_Cub
    {
        H,
        V,
        inf,
        sup,
        NONE,
        ESC
    }


    public enum Elemento_cub
    {
        LOSA,
        VIGA,
        MURO,
        FUND,
        NONE,
        ESC,
    }
}
