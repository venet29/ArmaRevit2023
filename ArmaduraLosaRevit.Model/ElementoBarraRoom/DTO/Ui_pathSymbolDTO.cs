using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO
{
    public class Ui_pathSymbolDTO
    {
        public FormaDibujarPAth FormaDibujar_ { get; internal set; }
        public double Largo_Foot { get; internal set; }
        public double LargoIzq_foot { get; internal set; }
        public double LargoDere_foot { get; internal set; }


        public string tipobarra { get; set; }
        public UbicacionLosa ubicacion { get; set; }
        public bool IsOk { get;  set; }
        public XYZ ptoMouse { get; internal set; }

        public Ui_pathSymbolDTO()
        {
            IsOk = false;
            FormaDibujar_ = FormaDibujarPAth.Normal;
        }
    }
}
