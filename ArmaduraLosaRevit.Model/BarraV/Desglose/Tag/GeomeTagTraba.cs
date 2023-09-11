using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Geometria;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
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
    public class GeomeTagTraba : GeomeTagBaseV, IGeometriaTag
    {
        private Config_EspecialCorte Config_EspecialCorte;

        public GeomeTagTraba(UIApplication _uiapp, RebarElevDTO _RebarElevDTO) :
            base(_uiapp, _RebarElevDTO)
        {

            Config_EspecialCorte = _RebarElevDTO.Config_EspecialCorte;
        }


        public override void M3_DefinirRebarShape()
        {


            Traba3ladosOrientadaOrtogonal_V _EstribosRectagularesHortogonales = new Traba3ladosOrientadaOrtogonal_V(_rebarElevDTO);
            if (_EstribosRectagularesHortogonales.calcularUbiaciontexto())
            {
                double Zrefe = CentroBarra.Z;
                CentroBarra = _EstribosRectagularesHortogonales.UbicacionDeF.AsignarZ(Zrefe);

                string familiaF = "_F_normal_";
                if (Config_EspecialCorte.TipoCOnfigCuantia == TipoCOnfCuantia.SegunPlano)
                    familiaF = "_F_segun_";

                TagP0_F = M1_1_ObtenerTAgBarra(CentroBarra, "FCorte", nombreDefamiliaBase + familiaF + escala, escala);
                listaTag.Add(TagP0_F);

                //largo
                LBarra = _EstribosRectagularesHortogonales.UbicacionDeL.AsignarZ(Zrefe);
                string familiaL = "_L_normal_";
                if (Config_EspecialCorte.TipoCOnfigLargo == TipoCOnfLargo.Aprox5)
                    familiaL = "_L_normal_";//_L_5aprox_
                else if (Config_EspecialCorte.TipoCOnfigLargo == TipoCOnfLargo.Aprox10)
                    familiaL = "_L_normal_";//_L_10aprox_
                TagP0_L = M1_1_ObtenerTAgBarra(LBarra, "LCorte", nombreDefamiliaBase + familiaL + escala, escala);
                listaTag.Add(TagP0_L);

                //parte
                XYZ textoSup = _EstribosRectagularesHortogonales.UbicacionSup;
                TagP0_ancho_ = M1_1_ObtenerTAgBarra(textoSup, "Ancho", nombreDefamiliaBase + "_F_normal_" + escala, escala);
                TagP0_ancho_.valorTag = _EstribosRectagularesHortogonales.UbicacionSup_ValorLArgo;
                listaTag.Add(TagP0_ancho_);
            }
            AsignarPArametros(this);
        }

        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagTraba> rutina)
        {
            rutina(this);
        }
        public void AsignarPArametros(GeomeTagBaseV _geomeTagBase)
        {

        }
    }
}
