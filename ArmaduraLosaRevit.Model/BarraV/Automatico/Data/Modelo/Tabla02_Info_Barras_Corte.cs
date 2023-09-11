using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo
{
    public class Tabla02_Info_Barras_Corte
    {
        public string Tipo_Elemento { get;  set; }
        public string Unique_Name_ETABS { get;  set; }
        public string ID_Name_REVIT { get;  set; }
        public string Label_Beam { get;  set; }
        public string Eje_ETABS { get;  set; }
        public string Eje_REVIT { get;  set; }
        public string Seccion { get;  set; }
        public string n_Estribos { get;  set; }
        public string diametro_Estribos_mm { get;  set; }
        public string espaciamiento_Estribos__cm { get;  set; }
        public string n_Laterales { get;  set; }
        public string diametro_Laterales_mm { get;  set; }
        public string X1__m { get;  set; }
        public string Y1__m { get;  set; }
        public string Z1__m { get;  set; }
        public string X2__m { get;  set; }
        public string Y2__m { get;  set; }
        public string Z2__m { get;  set; }
        public string X3__m { get;  set; }
        public string Y3__m { get;  set; }
        public string Z3__m { get;  set; }
        public string X4__m { get;  set; }
        public string Y4__m { get;  set; }
        public string Z4__m { get;  set; }
        public string IDEM_a_Viga { get; set; }
        public string Barra_IDEM_a { get; set; }

    }
}
