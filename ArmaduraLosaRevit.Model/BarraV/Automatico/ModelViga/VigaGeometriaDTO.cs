using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    public class VigaGeometriaDTO
    {

        public Tabla04_Info_Geometria_Vigas obj { get; }
        public string Eje_ETABS { get; private set; }
        public string Eje_REVIT { get; private set; }
        public string ID_Name_REVIT { get; private set; }
        public string Label_Beam { get; private set; }
        public string Tipo_Elemento { get; private set; }
        public string Unique_Name_ETABS { get; private set; }
        public XYZnh p1_cm { get; private set; }
        public XYZnh p2_cm { get; private set; }
        public XYZnh p3_cm { get; private set; }
        public XYZnh p4_cm { get; private set; }
        public XYZ P1_Revit_foot { get; private set; }
        public XYZ P2_Revit_foot { get; private set; }
        public XYZ P3_Revit_foot { get; private set; }
        public XYZ P4_Revit_foot { get; private set; }

        public VigaGeometriaDTO( Tabla04_Info_Geometria_Vigas tabla04_Info_Geometria_Vigas)
        {

            this.obj = tabla04_Info_Geometria_Vigas;
        }
        public bool Crear()
        {
            try
            {
                Eje_ETABS = obj.Eje_ETABS;
                Eje_REVIT = obj.Eje_REVIT;
                ID_Name_REVIT = obj.ID_Name_REVIT;
                Label_Beam = obj.Label_Beam;
                Tipo_Elemento = obj.Tipo_Elemento;
                Unique_Name_ETABS = obj.Unique_Name_ETABS;

                p1_cm = new XYZnh(Convert.ToDouble(obj.X1__m) * 100.0D, Convert.ToDouble(obj.Y1__m) * 100.0D, Convert.ToDouble(obj.Z1__m) * 100.0D);
                p2_cm = new XYZnh(Convert.ToDouble(obj.X2__m) * 100.0D, Convert.ToDouble(obj.Y2__m) * 100.0D, Convert.ToDouble(obj.Z2__m) * 100.0D);
                p3_cm = new XYZnh(Convert.ToDouble(obj.X3__m) * 100.0D, Convert.ToDouble(obj.Y3__m) * 100.0D, Convert.ToDouble(obj.Z3__m) * 100.0D);
                p4_cm = new XYZnh(Convert.ToDouble(obj.X4__m) * 100.0D, Convert.ToDouble(obj.Y4__m) * 100.0D, Convert.ToDouble(obj.Z4__m) * 100.0D);

    

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }
        public void PtosInvertirTrasformadas(ArmaduraTrasformada _armaduraTrasformada)
        {
            P1_Revit_foot = _armaduraTrasformada.Ejecutar(p1_cm.GetXYZ_cmTofoot());
            P2_Revit_foot = _armaduraTrasformada.Ejecutar(p2_cm.GetXYZ_cmTofoot());
            P3_Revit_foot = _armaduraTrasformada.Ejecutar(p3_cm.GetXYZ_cmTofoot());
            P4_Revit_foot = _armaduraTrasformada.Ejecutar(p4_cm.GetXYZ_cmTofoot());
        }
    }
}