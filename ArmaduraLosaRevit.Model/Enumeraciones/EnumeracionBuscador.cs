using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Enumeraciones
{
    public class EnumeracionBuscador
    {
        public static T ObtenerEnumGenerico<T>(T valor, string v, bool IsMaje=true )
        {
            try
            { 
                T temp = valor;
                T result = (T)System.Enum.Parse(typeof(T), v);
                return result;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
             if(IsMaje)  Util.ErrorMsg($"No se puedo obtener nombre generico de {v}. Rutina:'ObtenerEnumGenerico'. Se devuelve valor de referencia ");
                return valor;
            }
        }
    }
}
