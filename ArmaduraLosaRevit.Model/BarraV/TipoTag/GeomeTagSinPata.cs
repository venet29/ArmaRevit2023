using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoTag
{
    public class GeomeTagSinPata : GeomeTagBaseV, IGeometriaTag
    {
   

        public GeomeTagSinPata(Document doc,  XYZ ptoIni, XYZ ptoFin, XYZ posiciontag) :
            base(doc,  ptoIni, ptoFin,  posiciontag)
        { }


  
        public override void M3_DefinirRebarShape()=>  AsignarPArametros(this); 
    
        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagSinPata> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseV _geomeTagBase)
        {

            //_geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            //_geomeTagBase.TagP0_E.IsOk = false;
            _geomeTagBase.TagP0_F.IsOk = false;

            _geomeTagBase.TagP0_C.IsOk = false;
            _geomeTagBase.TagP0_L.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
