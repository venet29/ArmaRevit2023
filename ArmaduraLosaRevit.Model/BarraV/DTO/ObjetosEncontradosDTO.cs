using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class ObjetosEncontradosDTO
    {

        public Element elemt { get; set; }
        public int elemtid { get; set; }
        public TipoElementoBArraV nombreTipo { get; set; }
        public double distancia { get; set; }
        public XYZ ptoInterseccion { get; set; }
        public XYZ PtoSObreCara { get; set; }
        public XYZ ptoSObreCaraInferiorLosa { get; set; }

        public double EspesorMuro { get; set; }
        public XYZ OrigenMuro { get; set; }
        public XYZ DireccionMuro { get; set; }


        public double EspesorViga { get; set; }
        public XYZ OrigenViga { get; set; }
        public XYZ DireccionViga { get; set; }
        public PlanarFace PlanarfaceIntersectada { get;  set; }
        public XYZ NormalFace { get; internal set; }
        public double EspesorLosa { get; internal set; }
    }




}
