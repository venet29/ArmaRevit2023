using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra
{
    public partial class DatosNuevaBarraDTO
    {
        //solo fundaciones
        public XYZ PtoCodoDireztriz { get; set; }
        public XYZ PtoDireccionDirectriz { get; set; }
        public bool IsLibre { get; set; }
        public XYZ LeaderEnd { get; set; }

        public double LargoPAtaIzqHook_cm { get; set; }
        public double LargoPAtaDereHook_cm { get; set; }
        public  string  TipoBarra { get; set; }

        // para automatico
        public double DiametroMM_fundV { get; set; }
        public double Espaciamiento_fundV_Foot { get; set; }
        public double DiametroMM_fundH{ get; set; }
        public double Espaciamiento_fundH_Foot { get; set; }


        public string LargoTotal_Fund(double largoBarra)
        {

            largoBarra = Math.Round(Util.FootToCm(largoBarra), 0);
            switch (TipoPataFun)
            {
                case TipoPataFund.Sin:
                    return (largoBarra).ToString();
                case TipoPataFund.DereSup:
                    return (LargoPAtaDereHook_cm + largoBarra).ToString();
                case TipoPataFund.IzqInf:
                    return (LargoPAtaIzqHook_cm + largoBarra).ToString();
                case TipoPataFund.Ambos:
                    return (LargoPAtaIzqHook_cm + LargoPAtaDereHook_cm + largoBarra).ToString();
            }
            return "";
        }

        public string LargoParciales_Fund(double largoBarra_foot)
        {
            
            largoBarra_foot = Math.Round(Util.FootToCm(largoBarra_foot), 0);
            switch (TipoPataFun)
            {
                case TipoPataFund.IzqInf:
                    return $"({LargoPAtaIzqHook_cm}+{largoBarra_foot})";
                case TipoPataFund.DereSup:
                    return $"({largoBarra_foot}+{LargoPAtaDereHook_cm})";
                case TipoPataFund.Ambos:
                    return $"({LargoPAtaIzqHook_cm}+{largoBarra_foot}+{LargoPAtaDereHook_cm})";
            }
            return "";
        }


    }
}
