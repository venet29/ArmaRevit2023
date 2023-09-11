using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Dto
{
    public class ListaPlanosDTO
    {

        public string Plano { get; set; }
        public string contenido { get; set; }
        public string revision { get; set; }
        public string descripcion { get; set; }
        public string dibujo { get; set; }
        public string reviso { get; set; }




        public object[] ObtenerDato_array() =>
            new object[] {
                    Plano,
               contenido,
               revision,
               descripcion,
               dibujo,
               reviso
                    };
    }
}

