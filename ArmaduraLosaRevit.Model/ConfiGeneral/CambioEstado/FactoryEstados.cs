using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ConfiGeneral.CambioEstado
{
    public class EstadosProyectoNH
    {
        public int posicion { get; set; }
        public string CurrentRevision { get; set; }
        public string CODIGOESTADODEAVANCE { get; set; }
        public string CODIGOESPECIALIDAD { get; set; }
        public EstadosProyectoNH(int posicion,string CODIGOESTADODEAVANCE, string CurrentRevision, string CODIGOESPECIALIDAD)
        {
            this.posicion = posicion;
            this.CODIGOESTADODEAVANCE = CODIGOESTADODEAVANCE;
            this.CurrentRevision = CurrentRevision;
            this.CODIGOESPECIALIDAD = CODIGOESPECIALIDAD;
        }


    }
    public class FactoryEstados
    {
        public static List<EstadosProyectoNH> ListaEstadosProyecto = new List<EstadosProyectoNH>() {
        new EstadosProyectoNH(1,"IP","A","INICIO DE PROYECTO"),
        new EstadosProyectoNH(2,"DA","B","DISEÑO ANTEPROYECTO"),
        new EstadosProyectoNH(3,"DB","C","DISEÑO BASICO"),
        new EstadosProyectoNH(4,"DD","D","DISEÑO DE DETALLES"),
        new EstadosProyectoNH(5,"RE","E","REVISION"),
        new EstadosProyectoNH(6,"AP","F","APTO PARA PRESUPUESTO"),
        new EstadosProyectoNH(7,"AC","0","APTO PARA CONSTRUCCION"),
        };
    }
}
