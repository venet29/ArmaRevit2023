using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo
{
    public class Tabla03_Info_Traslapos_Vigas
    {
        public string Tipo_Elemento { get; set; }
        public string Unique_Name_ETABS { get; set; }
        public string ID_Name_REVIT { get; set; }
        public string Label_Beam { get; set; }
        public string Fila_Barra { get; set; }
        public string Inicio_Fila { get; set; }
        public string Traslapo_1 { get; set; }
        public string Traslapo_2 { get; set; }
        public string Traslapo_3 { get; set; }
        public string Termino_Fila { get; set; }

        public (TraslapoEnSeccionNh, TipoTraslapoEnSeccionNh) OBtenerTIpo()
        {
            if (Inicio_Fila != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Inicio_Fila);
                return (TraslapoEnSeccionNh.TraslapoInicio, resulta);
            }
            else if (Traslapo_1 != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Traslapo_1);
                return (TraslapoEnSeccionNh.Traslpoa1_tercio, resulta);


            }
            else if (Traslapo_2 != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Traslapo_2);
                return (TraslapoEnSeccionNh.Traslpoa2_tercio, resulta);
            }
            else if (Traslapo_3 != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Traslapo_3);
                return (TraslapoEnSeccionNh.Traslpoa3_tercio, resulta);
            }
            else if (Termino_Fila != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Termino_Fila);
                return (TraslapoEnSeccionNh.TraslapoFinal, resulta);
            }
            else
                return (TraslapoEnSeccionNh.NONE, TipoTraslapoEnSeccionNh.NONE);
        }


        public List<CasosTraslapoDTO> OBtenerTIpov2()
        {
            List<CasosTraslapoDTO> ListaCasosTraslapos = new List<CasosTraslapoDTO>();

            if (Inicio_Fila != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Inicio_Fila);
                ListaCasosTraslapos.Add(new CasosTraslapoDTO(TraslapoEnSeccionNh.TraslapoInicio, resulta));
            }

            if (Traslapo_1 != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Traslapo_1);
                ListaCasosTraslapos.Add(new CasosTraslapoDTO(TraslapoEnSeccionNh.Traslpoa1_tercio, resulta));


            }
            if (Traslapo_2 != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Traslapo_2);
                ListaCasosTraslapos.Add(new CasosTraslapoDTO(TraslapoEnSeccionNh.Traslpoa2_tercio, resulta));
            }
            if (Traslapo_3 != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Traslapo_3);
                ListaCasosTraslapos.Add(new CasosTraslapoDTO(TraslapoEnSeccionNh.Traslpoa3_tercio, resulta));
            }
            if (Termino_Fila != "0")
            {
                var resulta = ObtenerTraslapoEnSeccionNh(Termino_Fila);
                ListaCasosTraslapos.Add(new CasosTraslapoDTO(TraslapoEnSeccionNh.TraslapoFinal, resulta));
            }


            return ListaCasosTraslapos;
        }

        private TipoTraslapoEnSeccionNh ObtenerTraslapoEnSeccionNh(string caso)
        {
            switch (caso)
            {
                case "1":
                    return TipoTraslapoEnSeccionNh.Tt_1;
                case "2":
                    return TipoTraslapoEnSeccionNh.Tt_izq_2;
                case "3":
                    return TipoTraslapoEnSeccionNh.Tt_dere_3;
                case "4":
                    return TipoTraslapoEnSeccionNh.Tc_4;
                case "5":
                    return TipoTraslapoEnSeccionNh.Tc_izq_5;
                case "6":
                    return TipoTraslapoEnSeccionNh.Tc_dere_6;
                case "7":
                    return TipoTraslapoEnSeccionNh.G_7;
                default:
                    return TipoTraslapoEnSeccionNh.NONE;
            }
        }

    }
}
