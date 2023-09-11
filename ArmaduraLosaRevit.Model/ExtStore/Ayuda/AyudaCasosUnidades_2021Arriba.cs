using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ExtStore.Ayuda
{
   public class AyudaCasosUnidades_2021Arriba
    {

        public static Entity Asignar_DUT_DECIMAL_FEET(Entity ent1,string nombreCampo,XYZ pto)
        {
             ent1.Set<XYZ>(nombreCampo, pto, UnitTypeId.Feet);// UnitTypeId.Feet);
            return ent1;
        }

        public static XYZ Obtener_DUT_DECIMAL_FEET(Entity ent1, string nombreCampo)
        {
            return ent1.Get<XYZ>(nombreCampo, UnitTypeId.Feet);// UnitTypeId.Feet);
 
        }


        public static Entity Asignar_DUT_DECIMAL_FEET(Entity ent1, Field nombreField, XYZ pto)
        {
            ent1.Set<XYZ>(nombreField, pto, UnitTypeId.Feet);// UnitTypeId.Feet);
            return ent1;
        }


        public static FieldBuilder Obtener_UT_Length(FieldBuilder fb11)
        {
            return fb11.SetSpec(SpecTypeId.Length);
        }
        public static FieldBuilder Obtener_UT_Numero(FieldBuilder fb11)
        {
            return fb11.SetSpec(SpecTypeId.Length);
        }



        public static Entity Asignar_DUT_Numero_FEET(Entity ent, string nombreCampo, double numero)
        {
            ent.Set<double>(nombreCampo, numero, UnitTypeId.Feet);// UnitTypeId.Feet);
            return ent;
        }

        public static double Obtener_DUT_Numero_FEET(Entity ent1, string nombreCampo)
        {
            return ent1.Get<double>(nombreCampo, UnitTypeId.Feet);// UnitTypeId.Feet);

        }

    }
}
