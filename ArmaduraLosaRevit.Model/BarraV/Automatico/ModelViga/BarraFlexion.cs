using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    public class BarraFlexion
    {
        public int Nombreviga { get; }
        public TipoCaraObjeto Ubicacion { get; }
        public int Linea { get; }
        public BarraFlexionTramosDTO BarraFlexionTramosDTO_ { get; set; }
        public bool IsOK { get;  set; }
        public TipoPataBarra inicial_tipoBarraH { get;  set; }
        public Element BarraCreadoElem { get; set; }
        public BarraFlexion BarraIdem { get; set; }

        public BarraFlexion(int nombreviga, TipoCaraObjeto ubicacion, int linea, BarraFlexionTramosDTO barraviga)
        {
            Nombreviga = nombreviga;
            Ubicacion = ubicacion;
            Linea = linea;
            BarraFlexionTramosDTO_ = barraviga;
            inicial_tipoBarraH = barraviga.inicial_tipoBarraH;
            IsOK = true;
        }

        internal void PtosInvertirTrasformadas(ArmaduraTrasformada _armaduraTrasformada)
        {            
            BarraFlexionTramosDTO_.P1_Revit_foot = _armaduraTrasformada.Ejecutar(BarraFlexionTramosDTO_.p1_mm.GetXYZ_mmTofoot());
            BarraFlexionTramosDTO_.P2_Revit_foot = _armaduraTrasformada.Ejecutar(BarraFlexionTramosDTO_.p2_mm.GetXYZ_mmTofoot());
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}