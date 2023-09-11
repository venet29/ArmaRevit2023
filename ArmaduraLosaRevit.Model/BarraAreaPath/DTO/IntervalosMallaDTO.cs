using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.DTO
{
    public class IntervalosMallaDTO
    {
        public DatosMallasAutoDTO _datosMallasDTO { get; set; }

        public List<Curve> ListaCurvaPAthArea { get;  set; }

        public Element _muroSeleccionado { get; set; }
        public List<XYZ> ListaPtos { get;  set; }
        public double EspesorMuroFoot { get;  set; }
        public double _largoMuroFoot { get;  set; }
        public List<XYZ> ListaPtos_MallaV { get;  set; }

        //Por algun motive al llegar al nivel 9 que esta agrupado , el muro host de indefine y queda como invalido.Entonces se crea varible ‘_muroSeleccionadoId’ para volver a obtener wall
        //solo para caso automatico elevacion
        public int _muroSeleccionadoId { get;  set; }
    }
}
