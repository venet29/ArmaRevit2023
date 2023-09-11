using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa
{
   public class AyudaObtenerLetraNH
    {
        private static Dictionary<string, string> DiccionarioLetras_LOSA_ESC_F1_135_SINPATA;

        private static Dictionary<string, string> DiccionarioLetras_LOSA_ESC_F1_45_CONPATA;

        public static Dictionary<string, string> getDiccionarioLetras_LOSA_ESC_F1_135_SINPATA()
        {
            if (DiccionarioLetras_LOSA_ESC_F1_135_SINPATA == null)
            {
                DiccionarioLetras_LOSA_ESC_F1_135_SINPATA = new Dictionary<string, string>() {
                    { "B","A"},
                    { "C","B"},
                    { "D","C"},
                    { "E","D"}
                };

            }

                return DiccionarioLetras_LOSA_ESC_F1_135_SINPATA;
        }

        public static Dictionary<string, string> getDiccionarioLetras_LOSA_ESC_F1_45_CONPATA()
        {
            if (DiccionarioLetras_LOSA_ESC_F1_45_CONPATA == null)
            {
                DiccionarioLetras_LOSA_ESC_F1_45_CONPATA = new Dictionary<string, string>() {
                    { "A","A"},
                    { "B","B"},
                    { "C","C"},
                    { "D","D"},
                    { "G","E"},
                    { "K","G"}
                };

            }

            return DiccionarioLetras_LOSA_ESC_F1_45_CONPATA;
        }


        public static Dictionary<string, string> getDiccionarioLetras_LOSA_INCLI_F1()
        {
            if (DiccionarioLetras_LOSA_ESC_F1_45_CONPATA == null)
            {
                DiccionarioLetras_LOSA_ESC_F1_45_CONPATA = new Dictionary<string, string>() {
                    { "B","A"},
                    { "C","B"},
                    { "D","C"},
                    { "E","D"}                    
                };

            }

            return DiccionarioLetras_LOSA_ESC_F1_45_CONPATA;
        }


        //            LOSA_ESC_F3_45,
        //LOSA_ESC_F3_135,
        //LOSA_ESC_F3B_45,
        //LOSA_ESC_F3B_135,
        //LOSA_ESC_F1_45,
        //LOSA_ESC_F1_45_CONPATA,
        //LOSA_ESC_F1_135_SINPATA,

    }
}
