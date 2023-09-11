using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo
{
    public class Tabla01_Info_Barras_Flexion
    {

        public string Tipo_Elemento { get; set; }
        public string Unique_Name_ETABS { get; set; }
        public string ID_Name_REVIT { get; set; }
        public string Label_Beam { get; set; }
        public string Eje_ETABS { get; set; }
        public string Eje_REVIT { get; set; }
        public string Ubicacion_Armado { get; set; }
        public string Seccion { get; set; }
        public string Fila_Barra { get; set; }
        public string Identificador_Barras { get; set; }
        public string n_Barras { get; set; }
        public string diametro_Barras__mm { get; set; }
        public string Pos_H_1__mm { get; set; }
        public string Pos_H_2__mm { get; set; }
        public string Pos_V_1__mm { get; set; }
        public string Pos_V_2__mm { get; set; }
        public string Pos_HOM_H_1__mm { get; set; }
        public string Pos_HOM_H_2__mm { get; set; }
        public string Pos_HOM_V_1__mm { get; set; }
        public string Pos_HOM_V_2__mm { get; set; }
        public string IDEM_a_Viga { get; set; }
        public string Barra_IDEM_a { get; set; }

    }

}
