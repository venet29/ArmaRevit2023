using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Enumeraciones
{
    public static class VariablesSistemas
    {
        public static UbicacionLosa m_ubicacionBarra { get; set; } = UbicacionLosa.Izquierda;
        public static bool IsConAhorro { get; set; } = false;
        public static bool IsDibujarS4 { get; set; } = false;

        public static bool IsVerificarEspesor{ get; set; } = false;

        public static double LargoBarras_cm { get; set; } = 400;
        public static double LargoRecorrido_cm { get; set; } = 200;

        public static string tipoPorF1 { get; set; } = "F20";

        public static string tipoPorF3 { get; set; } = "F16";

        public static string tipoPorF4 { get; set; } = "F19";
        public static bool IsAjusteBarra_Recorrido { get; set; } = true;
        public static bool IsAjusteBarra_Largo { get; set; } = true;
        public static bool IsReSeleccionarPuntoRango { get; set; } = true;

        public static string CONST_NOMBRE_3D_PARA_BUSCAR { get; set; } = "3D_NoEditar";

        public static bool IS_MENSAJE_BUSCAR_ROOM { get; set; } = true; //SOLO ES FALSE CUANDO SE EDITA FUNDACIONES
        //para elvciones

        public static bool IsUsarTraslapoDimension_sinMoverBarras { get; set; } = true;
        public static bool IsCambiarLEtraArial { get; set; } = false; // 22-09 -2022 solo se usa para cambiar letra de tag
    }
}
