using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{
    public class TipoLetraCambiar
    {
        public static string ObtenerLetra(string tipo)
        {
            string result = "";
            try
            {
                if (ListaLetra.ContainsKey(tipo))
                    result = ListaLetra[tipo];
            }
            catch (Exception ex)
            {

                Util.DebugDescripcion(ex);
            }
            return result;
        }
        public static Dictionary<string, string> ListaLetra = new Dictionary<string, string>() {
                      { "f1_a","B_"},
                       { "f1_b","B_"},
                       { "f1_esc135_sinpata","B_"},
                       { "f1_SUP","B_"},
                        { "f9a","B_"},
                        { "f1","B_"},
                        { "f9","B_,D_"},
                        { "f4","B_,D_"},
                        { "s3","B_,D_"},
                        { "f7","B_,D_"},
                        { "s1","B_,D_"},

                        { "f10","A_,C_"},
                        { "f11","A_,C_"},

                        { "f10a","A_"},
                        { "f11a","A_"},

                        { "f17A_Tras","A_"},
                        { "f17B_Tras","A_"},
                        { "f17b","A_"},
            { "f17a","A_"},
            { "f17","A_"},
                        { "f18","A_"},

                        { "f19","B_"},
                        { "f20A_Izq_Tras","B_"},
                        { "f20B_Dere_Tras","B_"},
                        { "f20","B_"},

                        { "f19_Izq","B_"},
                        { "f19_Dere","B_"},
                        { "f21A_Izq_Tras","B_"},
                        { "f21B_Dere_Tras","B_"},
                        { "f21","B_"},

        };
    }
}
