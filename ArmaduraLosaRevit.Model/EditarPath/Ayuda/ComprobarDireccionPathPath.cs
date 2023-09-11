using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.EditarPath.Ayuda
{
    public class ComprobarDireccionPathPath
    {
        private DireccionEdicionPathRein direccionEdicionPathRein_1;
        private DireccionEdicionPathRein direccionEdicionPathRein_2;

        public ComprobarDireccionPathPath(DireccionEdicionPathRein direccionEdicionPathRein_1, DireccionEdicionPathRein direccionEdicionPathRein_2)
        {
            this.direccionEdicionPathRein_1 = direccionEdicionPathRein_1;
            this.direccionEdicionPathRein_2 = direccionEdicionPathRein_2;
        }

        public bool VerificarDireccion()
        {

            if (direccionEdicionPathRein_1 == DireccionEdicionPathRein.Izquierda || direccionEdicionPathRein_1 == DireccionEdicionPathRein.Derecha)
            {
                if (direccionEdicionPathRein_2 == DireccionEdicionPathRein.Izquierda || direccionEdicionPathRein_2 == DireccionEdicionPathRein.Derecha)
                    return true;                
                else
                {
                    Util.ErrorMsg($"Error en seleccionar la direccion de alargamiento del segundo Patheinforment");
                    return false;
                }
            }
            else if (direccionEdicionPathRein_1 == DireccionEdicionPathRein.Inferior || direccionEdicionPathRein_1 == DireccionEdicionPathRein.Superior)
            {
                if (direccionEdicionPathRein_2 == DireccionEdicionPathRein.Inferior || direccionEdicionPathRein_2 == DireccionEdicionPathRein.Superior)
                    return true;
                else
                {
                    Util.ErrorMsg($"Error en seleccionar la direccion de alargamiento del segundo Patheinforment");
                    return false;
                }
            }

            return true;
        }
    }
}
