using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas
{
    public class ObtenerCasoAoBDeAhorro
    {

        public static string ConversortoS4(string tipo)
        {
            string result = tipo;

            result = (result == "f1_SUP" ? "s4" : tipo);
            return result;
        }

        public  static string Obtenercaso_16_20_21_22_AoB(string tipobarra_, UbicacionLosa direccion)
        {
            string tipobarra = tipobarra_;
            if (tipobarra_ == "f16" && (direccion == UbicacionLosa.Inferior || direccion == UbicacionLosa.Izquierda))
                tipobarra = "f16a";
            else if (tipobarra_ == "f16" && (direccion == UbicacionLosa.Derecha || direccion == UbicacionLosa.Superior))
                tipobarra = "f16b";
            if (tipobarra_ == "f17" && (direccion == UbicacionLosa.Inferior || direccion == UbicacionLosa.Izquierda))
                tipobarra = "f17a";
            else if (tipobarra_ == "f17" && (direccion == UbicacionLosa.Derecha || direccion == UbicacionLosa.Superior))
                tipobarra = "f17b";
            else if (tipobarra_ == "f19")
                tipobarra = "f19";
            else if (tipobarra_ == "f20" && (direccion == UbicacionLosa.Inferior || direccion == UbicacionLosa.Izquierda))
                tipobarra = "f20a";
            else if (tipobarra_ == "f20" && (direccion == UbicacionLosa.Derecha || direccion == UbicacionLosa.Superior))
                tipobarra = "f20b";
            else if (tipobarra_ == "f20Inv" && (direccion == UbicacionLosa.Inferior || direccion == UbicacionLosa.Izquierda))
                tipobarra = "f20aInv";
            else if (tipobarra_ == "f20Inv" && (direccion == UbicacionLosa.Derecha || direccion == UbicacionLosa.Superior))
                tipobarra = "f20bInv";
            else if (tipobarra_ == "f21" && (direccion == UbicacionLosa.Inferior || direccion == UbicacionLosa.Izquierda))
                tipobarra = "f21a";
            else if (tipobarra_ == "f21" && (direccion == UbicacionLosa.Derecha || direccion == UbicacionLosa.Superior))
                tipobarra = "f21b";
            else if (tipobarra_ == "f22" && (direccion == UbicacionLosa.Inferior || direccion == UbicacionLosa.Izquierda))
                tipobarra = "f22a";
            else if (tipobarra_ == "f22Inv" && (direccion == UbicacionLosa.Inferior || direccion == UbicacionLosa.Izquierda))
                tipobarra = "f22aInv";
            else if (tipobarra_ == "f22" && (direccion == UbicacionLosa.Derecha || direccion == UbicacionLosa.Superior))
                tipobarra = "f22b";
            else if (tipobarra_ == "f22Inv" && (direccion == UbicacionLosa.Derecha || direccion == UbicacionLosa.Superior))
                tipobarra = "f22bInv";

            return tipobarra;
        }

        public static string Obtenercaso_INVERTIR_16_20_21_22_AoB(string tipobarra_)
        {
            string tipobarra = tipobarra_;
            if (tipobarra_ == "f16a" || tipobarra == "f16b")
                tipobarra = "f16";
            else if (tipobarra_ == "f17a" || tipobarra == "f17b")
                tipobarra = "f17";
            else if (tipobarra_ == "f19")
                tipobarra = "f19";
            else if (tipobarra_ == "f20a" || tipobarra == "f20b")
                tipobarra = "f20";
            else if (tipobarra_ == "f20aInv" || tipobarra == "f20bInv")
                tipobarra = "f20Inv";
            else if (tipobarra_ == "f21a" || tipobarra == "f21b")
                tipobarra = "f21";

            else if (tipobarra_ == "f22a" || tipobarra == "f22b")
                tipobarra = "f22";
            else if (tipobarra_ == "f22aInv" || tipobarra == "f22bInv")
                tipobarra = "f22Inv";

            return tipobarra;
        }


        public static bool IScaso_16_17_18_19_20_20Inv_21_22_AoB(string tipobarra_)
        {
            
            if (tipobarra_ == "f16" || tipobarra_ == "f16a" || tipobarra_ == "f16b" ||
                tipobarra_ == "f17" || tipobarra_ == "f17a" || tipobarra_ == "f17b" ||
                tipobarra_ == "f18" ||
                tipobarra_ == "f19" ||
                tipobarra_ == "f20" || tipobarra_ == "f20a" || tipobarra_ == "f20b" ||
                                       tipobarra_ == "f20aInv" || tipobarra_ == "f20bInv" ||
                tipobarra_ == "f21" || tipobarra_ == "f21a" || tipobarra_ == "f21b" ||
                tipobarra_ == "f22" || tipobarra_ == "f22a" || tipobarra_ == "f22b" ||
                                       tipobarra_ == "f22aInv" || tipobarra_ == "f22bInv")
                return true;
            else
                return false;

        }
    }
}
