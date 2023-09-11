using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.RebarLosa.ParametrosCompartidos
{
    public class ParametroBarra
    {
        public NombreParametros para  { get; set; }
        public TipoParametro tipoParametro { get; set; }
        public double valorDouble { get; set; }
        public string valorString { get; set; }
        public int valorint{ get; set; }
        public ParametroBarra(NombreParametros para, double valorDouble)
        {
            this.para = para;
            this.valorDouble = valorDouble;
            this.tipoParametro = TipoParametro.double_;
        }
        public ParametroBarra(NombreParametros para, string valorString)
        {
            this.para = para;
            this.valorString = valorString;
            this.tipoParametro = TipoParametro.string_;
        }
        public ParametroBarra(NombreParametros para, int valorInt)
        {
            this.para = para;
            this.valorint = valorInt;
            this.tipoParametro = TipoParametro.int_;
        }
    }
}
