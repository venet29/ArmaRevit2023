using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.RebarLosa;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades
{
    public class parametrosRebar
    {
        private readonly TipoRebar tipoBarraEspecifico;

        public static int indice { get; set; }
        public string letraOriginal { get; set; }
        public string letraNH { get;  set; }
        public double largo { get; set; }
        public parametrosRebar(string letra, double largo )
        {
            indice += 1;
            this.letraOriginal = letra;
            this.letraNH = letra;
            this.largo = largo;
            this.tipoBarraEspecifico = TipoRebar.NONE;
        }

        public parametrosRebar(string letra, double largo, TipoRebar tipoBarraEspecifico)
        {
            indice += 1;
            this.letraOriginal = letra;
            this.letraNH = letra;
            this.largo = largo;
            this.tipoBarraEspecifico = tipoBarraEspecifico;
        }

        public bool ObtenerLetraNH()
        {
            if (tipoBarraEspecifico == TipoRebar.NONE) return true;

            try
            {
                if (tipoBarraEspecifico == TipoRebar.LOSA_ESC_F1_135_SINPATA)
                {
                    
                    if (AyudaObtenerLetraNH.getDiccionarioLetras_LOSA_ESC_F1_135_SINPATA().ContainsKey(letraOriginal))
                        letraNH = AyudaObtenerLetraNH.getDiccionarioLetras_LOSA_ESC_F1_135_SINPATA()[letraOriginal];
                    else
                        return false;
                }
                else if (tipoBarraEspecifico == TipoRebar.LOSA_ESC_F1_45_CONPATA)
                {
                    if (AyudaObtenerLetraNH.getDiccionarioLetras_LOSA_ESC_F1_45_CONPATA().ContainsKey(letraOriginal))
                        letraNH = AyudaObtenerLetraNH.getDiccionarioLetras_LOSA_ESC_F1_45_CONPATA()[letraOriginal];
                    else
                        return false;

                }
                else if (tipoBarraEspecifico == TipoRebar.LOSA_INCLI_F1)
                {
                    if (AyudaObtenerLetraNH.getDiccionarioLetras_LOSA_INCLI_F1().ContainsKey(letraOriginal))
                        letraNH = AyudaObtenerLetraNH.getDiccionarioLetras_LOSA_INCLI_F1()[letraOriginal];
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                Util.ErrorMsg($"Error --> 'ObtenerLetraNH'    \ntipoBarraEspecifico:{tipoBarraEspecifico}  - \n letraOriginal:{letraOriginal}  ex:{ex.Message}");
                return false; ;
            }
            return true;
        }
    }
}
