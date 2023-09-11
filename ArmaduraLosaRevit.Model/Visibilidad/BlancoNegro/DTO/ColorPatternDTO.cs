using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO
{

    public class ColorPatternDTO
    {
        private Color color;
        private Element element;

        public string Nombre { get; }
        public Color color_ { get; set; }
        public Element Pattern_ { get; set; }
        public ColorPatternDTO(Element element)
        {
            this.element = element;
        }

        public ColorPatternDTO(Color color)
        {
            this.color = color;
        }

        public ColorPatternDTO(Color color, Element element)
        {
            this.color = color;
            this.element = element;
        }

  
    }
}
