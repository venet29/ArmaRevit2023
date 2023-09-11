using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarPath.CambiarLetras
{
    public interface ILetraCambiar
    {
        bool Isok { get; set; }
        void Ejecutar();


    }
}
