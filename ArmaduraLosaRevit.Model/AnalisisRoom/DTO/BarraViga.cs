using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.AnalisisRoom;

using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;

namespace ArmaduraLosaRevit.Model.AnalisisRoom.DTO
{

    /// <summary>
    /// objeto que se utiliza cuando los borde de room  'BoundarySegmen' busca elemnto conttiguo  o vecino
    /// si encuntra una viga se crea objecto 'BarraViga' que contiene los objetos
    /// -  BoundarySegmentNH del borde
    /// -  la viga encontrada como vecino 'FamilyInstance'
    /// </summary>BoundarySegmen

    public class BarraViga
    {
        private WrapperBoundarySegment boundarySegmentNH;
        private FamilyInstance viga;

        #region 0)propiedades

        public Curve Curva1 { get; set; }
        public Element ElementoViga { get; set; }



        #endregion



        #region 1) Constructores
        public BarraViga(Curve Curva1, Element ElementoViga)
        {
            this.ElementoViga = ElementoViga;
            this.Curva1 = Curva1;

        }

        public BarraViga(WrapperBoundarySegment boundarySegmentNH, FamilyInstance viga)
        {
            this.boundarySegmentNH = boundarySegmentNH;
            this.viga = viga;
        }
        #endregion



    }
}
