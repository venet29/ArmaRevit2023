using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo
{
    public class Tabla05_Objeto_con_todas_las_Tablas
    {

        public List<Tabla01_Info_Barras_Flexion> Lista_Tabla01_Info_Barras_Flexion { get;  set; }
        public List<Tabla02_Info_Barras_Corte> Lista_Tabla02_Info_Barras_Corte { get;  set; }
        public List<Tabla03_Info_Traslapos_Vigas> Lista_Tabla03_Info_Traslapos_Vigas { get;  set; }
        public List<Tabla04_Info_Geometria_Vigas> Lista_Tabla04_Info_Geometria_Vigas { get;  set; }

    }
}
