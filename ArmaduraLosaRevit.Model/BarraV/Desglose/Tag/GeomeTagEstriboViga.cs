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

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Tag
{
    public class GeomeTagEstriboViga : GeomeTagBaseV, IGeometriaTag
    {
        private Config_EspecialCorte Config_EspecialCorte;

        public GeomeTagEstriboViga(UIApplication _uiapp, RebarElevDTO _RebarElevDTO) :
            base( _uiapp,  _RebarElevDTO)
        {
            Config_EspecialCorte = _RebarElevDTO.Config_EspecialCorte;
        }


        public override void M3_DefinirRebarShape()
        {

            EstribosRectagularesOrtogonales_H _EstribosRectagularesHortogonales = new EstribosRectagularesOrtogonales_H(_rebarElevDTO);
            if (_EstribosRectagularesHortogonales.calcularUbiaciontexto())
            {
                //double Zrefe = CentroBarra.Z;
                CentroBarra = _EstribosRectagularesHortogonales.UbicacionDeF;//.AsignarZ(Zrefe);

                string familiaF = "_F_normal_";
                if (Config_EspecialCorte.TipoCOnfigCuantia == TipoCOnfCuantia.SegunPlano)
                    familiaF = "_F_segun_";

                TagP0_F = M1_1_ObtenerTAgBarra(CentroBarra, "FCorte", nombreDefamiliaBase + familiaF + escala, escala);
                listaTag.Add(TagP0_F);


                //largo
                LBarra = _EstribosRectagularesHortogonales.UbicacionDeL;//.AsignarZ(Zrefe);
                string familiaL = "_L_normal_";
               if (Config_EspecialCorte.TipoCOnfigLargo == TipoCOnfLargo.Aprox5)
                    familiaL = "_L_normal_";
                else if (Config_EspecialCorte.TipoCOnfigLargo == TipoCOnfLargo.Aprox10)
                    familiaL = "_L_normal_";
                TagP0_L = M1_1_ObtenerTAgBarra(LBarra, "LCorte", nombreDefamiliaBase + familiaL + escala, escala);
                listaTag.Add(TagP0_L);

                if (true)
                {
                    XYZ p0_sup = _EstribosRectagularesHortogonales.UbicacionSup;//.AsignarZ(Zrefe);
                    TagP0_ancho_ = M1_1_ObtenerTAgBarra(p0_sup, "Ancho", nombreDefamiliaBase + "_F_normal_" + escala, escala);//uso '_F_normal_' solo par acargar tl tag
                    TagP0_ancho_.valorTag = _EstribosRectagularesHortogonales.UbicacionSup_ValorLArgo;
                    listaTag.Add(TagP0_ancho_);


                    XYZ p0_izq = _EstribosRectagularesHortogonales.UbicacionIZq;//;.AsignarZ(Zrefe);
                    TagP0_Prof_ = M1_1_ObtenerTAgBarra(p0_izq, "Ancho", nombreDefamiliaBase + "_F_normal_" + escala, escala);//uso '_F_normal_' solo par acargar tl tag
                    TagP0_Prof_.valorTag = _EstribosRectagularesHortogonales.UbicacionIZq_ValorLArgo;
                    listaTag.Add(TagP0_Prof_);
                }
            }

            AsignarPArametros(this);
        }

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagEstriboViga> rutina)
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
