using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using System;

namespace ArmaduraLosaRevit.Model.Armadura.Dimensiones
{
    public partial class DimensionesBarras
    {
        public class dimensiones
        {
            public string nombre { get; set; }
            public double valor { get; set; }
            public double valorCM { get; set; }
            public double factorSegunFamilia { get; internal set; }
            public bool IsOk { get; set; }

            public dimensiones(string nombre, double valor)
            {                this.nombre = nombre;
                this.valor = valor;
                if (RedonderLargoBarras.RedondearFoot1_mascercano(valor))
                    this.valorCM = RedonderLargoBarras.NuevoLargobarracm;
                else
                    this.valorCM = Math.Round(Util.FootToCm(valor));
                this.IsOk = (valor == -1? false:true);
            }
            public dimensiones()
            {
                this.nombre = "";
                this.valor = 0;
                this.valorCM = 0;
                this.IsOk = false;
            }
        }

    }


}


