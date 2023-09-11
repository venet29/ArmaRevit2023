using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EstadoDesarrollo.Modelo
{
    public class ContenedorProyectos
    {

        public List<Proyecto> ListaPRoyectosTotal { get; set; }
        public ContenedorProyectos()
        {
            ListaPRoyectosTotal = new List<Proyecto>();
        }
    }
}
