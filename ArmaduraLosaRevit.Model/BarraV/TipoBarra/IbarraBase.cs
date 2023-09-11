using ArmaduraLosaRevit.Model.BarraV.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public interface IbarraBase
    {
        bool IsOk { get; set; }
        bool IsSoloTag { get; set; }
        void M0_CalcularCurva();
        bool M1_DibujarBarra();
        bool M1_1_DibujarBarraCOnfiguracion();
        bool M2_DibujarTags(ConfiguracionTAgBarraDTo confBarraTag);



      
        ElementId M3_ObtenerIdRebar();
      //  Rebar M3_a_ObtenerRebar();

        IbarraBaseResultDTO GetResult();

     //   bool GetIsNoProloganLosaArriba();

      //  XYZ GetIsPo_P2paraTag();
    }
}
