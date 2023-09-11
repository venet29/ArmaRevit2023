using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF
{
    public interface ITagF
    {
        int numeroBarrasPrimaria { get; set; }
        bool IsBarraPrimaria { get; set; }
        int numeroBarrasSecundario { get; set; }
        bool IsBarraSecuandaria { get; set; }

        bool Ejecutar();


    }
}
