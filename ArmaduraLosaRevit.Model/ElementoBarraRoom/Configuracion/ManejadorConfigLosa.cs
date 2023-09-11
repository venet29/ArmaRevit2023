using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Configuracion
{
    public class ManejadorConfigLosa
    {
        public ManejadorConfigLosa()
        {

        }

        public Result Execute()
        {
            try
            {

                // SingletonVariableSistema _SingletonVariableSistema = SingletonVariableSistema.GetInstance();
                if (VariablesSistemas.tipoPorF1 == null)
                    Util.ErrorMsg($" error null   tipoPorF1");
#pragma warning disable CS0472 // The result of the expression is always 'false' since a value of type 'double' is never equal to 'null' of type 'double?'
                if (VariablesSistemas.LargoBarras_cm == null)
#pragma warning restore CS0472 // The result of the expression is always 'false' since a value of type 'double' is never equal to 'null' of type 'double?'
                    Util.ErrorMsg($" error null   LargoBarras_cm");
#pragma warning disable CS0472 // The result of the expression is always 'false' since a value of type 'double' is never equal to 'null' of type 'double?'
                if (VariablesSistemas.LargoRecorrido_cm == null)
#pragma warning restore CS0472 // The result of the expression is always 'false' since a value of type 'double' is never equal to 'null' of type 'double?'
                    Util.ErrorMsg($" error null   LargoRecorrido_cm");
                if (VariablesSistemas.tipoPorF1 == null)
                    Util.ErrorMsg($" error null   tipoPorF1");
                if (VariablesSistemas.tipoPorF3 == null)
                    Util.ErrorMsg($" error null   tipoPorF3");
                if (VariablesSistemas.tipoPorF4 == null)
                    Util.ErrorMsg($" error null   tipoPorF4");

                VariablesSistemasDTO vdto = new VariablesSistemasDTO()
                {
                    IsConAhorro = VariablesSistemas.IsConAhorro,
                    IsDibujarS4 = VariablesSistemas.IsDibujarS4,
                    IsVerificarEspesor = VariablesSistemas.IsVerificarEspesor,
                    IsAjusteBarra_Recorrido = VariablesSistemas.IsAjusteBarra_Recorrido,
                    IsAjusteBarra_Largo = VariablesSistemas.IsAjusteBarra_Largo,

                    IsReSeleccionarPuntoRango = VariablesSistemas.IsReSeleccionarPuntoRango,

                    LargoBarras_cm = VariablesSistemas.LargoBarras_cm,
                    LargoRecorrido_cm = VariablesSistemas.LargoRecorrido_cm,
                    tipoPorF1 = VariablesSistemas.tipoPorF1.ToString(),
                    tipoPorF3 = VariablesSistemas.tipoPorF3.ToString(),
                    tipoPorF4 = VariablesSistemas.tipoPorF4.ToString()
                };


                ConfiLosa _confiLosa = new ConfiLosa(vdto);
                _confiLosa.ShowDialog();

                if (_confiLosa.IsOK)
                {
                    VariablesSistemas.IsConAhorro = _confiLosa.IsAhorro;
                    VariablesSistemas.IsVerificarEspesor = _confiLosa.IsConVerificar;
                    VariablesSistemas.IsDibujarS4 = _confiLosa.IsS4;
                    VariablesSistemas.IsAjusteBarra_Recorrido = _confiLosa.IsajusteBarra_Recorrido;
                    VariablesSistemas.IsAjusteBarra_Largo = _confiLosa.IsajusteBarra_Largo;
                    VariablesSistemas.IsReSeleccionarPuntoRango = _confiLosa.IsReSeleccionarPuntoRango;

                    VariablesSistemas.LargoBarras_cm = _confiLosa.LargoBarras;
                    VariablesSistemas.LargoRecorrido_cm = _confiLosa.LargoRecorrido;

                    VariablesSistemas.tipoPorF1 = _confiLosa.tipoPorF1;
                    VariablesSistemas.tipoPorF3 = _confiLosa.tipoPorF3;
                    VariablesSistemas.tipoPorF4 = _confiLosa.tipoPorF4;
                }

                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"ex :{ex.Message}");
                return Result.Failed; ;
            }
        }
    }
}
