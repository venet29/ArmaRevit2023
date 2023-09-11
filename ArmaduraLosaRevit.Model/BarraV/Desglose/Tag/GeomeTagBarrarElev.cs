using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Geometria;
using ArmaduraLosaRevit.Model.BarraV.TipoTag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Tag
{
    public class GeomeTagBarrarElev : GeomeTagBaseV, IGeometriaTag
    {


        public GeomeTagBarrarElev(UIApplication _uiapp, RebarElevDTO _RebarElevDTO) :
            base( _uiapp,  _RebarElevDTO)
        {
            listaTag = new List<TagBarra>();
        }


        public override void M3_DefinirRebarShape()
        {
            double defaseTag = 1.6;
            if (_rebarElevDTO.Config_EspecialElv.IsAgregarId)
                defaseTag = defaseTag + 1.2;

            XYZ direccionBArra = (_p2 - _p1).Normalize();
            //XYZ _ptoTexto = (_rebarElevDTO.ptoini + _rebarElevDTO.ptofinal) / 2
            //                 - new XYZ(0, 0, defaseTag) - _view.RightDirection * 0.25;
            XYZ _ptoTexto = (_rebarElevDTO.ptoini + _rebarElevDTO.ptofinal) / 2
                                 - direccionBArra* defaseTag - _rebarElevDTO.Config_EspecialElv.direccionMuevenBarrasFAlsa * 0.25;


            TagP0_Tipo = M1_1_ObtenerTAgBarra(_ptoTexto, "MLB", nombreDefamiliaBase + "_MLB_" + escala, escala);
            listaTag.Add(TagP0_Tipo);

            AsignarPArametros(this);
        }

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagBarrarElev> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseV _geomeTagBase)
        {

            //_geomeTagBase.TagP0_A.IsOk = false;
            //_geomeTagBase.TagP0_B.IsOk = false;
            //_geomeTagBase.TagP0_D.IsOk = false;
            //_geomeTagBase.TagP0_E.IsOk = false;
            // _geomeTagBase.TagP0_F_SIN.IsOk = false;

            //_geomeTagBase.TagP0_C.IsOk = false;
            //_geomeTagBase.TagP0_L.IsOk = false;
            //_geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_A);
        }
    }
}
