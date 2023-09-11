using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Ahorro
{
    public class FactoryAhorro
    {


        public static ConfiguracionAhorro ObtenerAhorro(bool isConAhorro, bool IsConF1_SUp)
        {

            if (isConAhorro)
            {
                if (IsConF1_SUp) //nunca se utilza
                    return new ConfiguracionAhorro(true,Util.CmToFoot(VariablesSistemas.LargoRecorrido_cm),
                                                                      Util.CmToFoot(VariablesSistemas.LargoBarras_cm),
                                                                      Util.CmToFoot(60), false, "f1_SUP", "f16", "f16", "f16");
                else
                    return new ConfiguracionAhorro(VariablesSistemas.IsConAhorro,
                                                        Util.CmToFoot(VariablesSistemas.LargoRecorrido_cm),
                                                        Util.CmToFoot(VariablesSistemas.LargoBarras_cm),
                                                        Util.CmToFoot(60), false, "f1", VariablesSistemas.tipoPorF1, VariablesSistemas.tipoPorF3, VariablesSistemas.tipoPorF4);
            }
            else
            {
                return new ConfiguracionAhorro(false,1000000, 10000000, 115,false, "", "", "", "");
            }
        }
    }
}
