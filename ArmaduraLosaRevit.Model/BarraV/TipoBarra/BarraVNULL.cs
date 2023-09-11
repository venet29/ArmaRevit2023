using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra
{
    public class BarraVNULL : ABarrasElevV_ConTrans, IbarraBase
    {
        public bool IsOk { get; set; }
        public BarraVNULL(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) :base(uiapp,  interBArraDto,  _newGeometriaTag)
        {
            IsOk = false;
        }
        public override void M0_CalcularCurva()
        { }
   
    }
}
