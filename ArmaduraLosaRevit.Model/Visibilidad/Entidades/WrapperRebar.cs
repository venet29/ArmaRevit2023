
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Entidades
{
   public  class WrapperRebar: IEnumerable
    {
        public Element element { get; set; }
        public string NombreView { get; internal set; }
        public string BarraTipo { get; internal set; }
        public List<ElementId> ListaRebarInsistem { get; set; }
        public List<WrapperRebarInSystem> ListaRebarInsystemV2 { get; set; }
        public string IdBarraCopiar { get;  set; }
        public ObtenerTipoBarra ObtenerTipoBarra { get; internal set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class WrapperRebarInSystem : IEnumerable
    {
        public Element element { get; set; }
        public string NombreView { get; internal set; }
        public string BarraTipo { get; internal set; }
        public ObtenerTipoBarra ObtenerTipoBarra { get; internal set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}
