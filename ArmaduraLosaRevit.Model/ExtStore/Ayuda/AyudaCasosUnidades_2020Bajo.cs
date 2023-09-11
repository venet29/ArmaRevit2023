using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ExtStore.Ayuda
{
    class AyudaCasosUnidades_2020Bajo
    {
        public static Entity Asignar_DUT_DECIMAL_FEET(Entity ent1, string nombreCampo, XYZ pto)
        {
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'Entity.Set<FieldType>(string, FieldType, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Set(string, FieldType, ForgeTypeId)` overload instead.'
            ent1.Set<XYZ>(nombreCampo, pto, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
#pragma warning restore CS0618 // 'Entity.Set<FieldType>(string, FieldType, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Set(string, FieldType, ForgeTypeId)` overload instead.'
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
            return ent1;
        }

        public static XYZ Obtener_DUT_DECIMAL_FEET_(Entity ent1,string nombreCampo )
        {
#pragma warning disable CS0618 // 'Entity.Get<FieldType>(string, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Get(string, ForgeTypeId)` overload instead.'
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
            return ent1.Get<XYZ>(nombreCampo, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning restore CS0618 // 'Entity.Get<FieldType>(string, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Get(string, ForgeTypeId)` overload instead.'
            
        }

        public static Entity Asignar_DUT_DECIMAL_FEET(Entity ent1, Field nombreField, XYZ pto)
        {
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'Entity.Set<FieldType>(Field, FieldType, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Set(Field, FieldType, ForgeTypeId)` overload instead.'
            ent1.Set(nombreField, pto, DisplayUnitType.DUT_DECIMAL_FEET); ;
#pragma warning restore CS0618 // 'Entity.Set<FieldType>(Field, FieldType, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Set(Field, FieldType, ForgeTypeId)` overload instead.'
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
          //  ent1.Set<XYZ>(nombreField, pto, UnitTypeId.Feet);// UnitTypeId.Feet);
            return ent1;
        }
  
        internal static FieldBuilder Obtener_UT_Length(FieldBuilder fb11)
        {
#pragma warning disable CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'FieldBuilder.SetUnitType(UnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `SetSpec(ForgeTypeId)` method instead.'
            return fb11.SetUnitType(UnitType.UT_Length);
#pragma warning restore CS0618 // 'FieldBuilder.SetUnitType(UnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `SetSpec(ForgeTypeId)` method instead.'
#pragma warning restore CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
        }


        internal static FieldBuilder Obtener_UT_Numero(FieldBuilder fb11)
        {
#pragma warning disable CS0618 // 'FieldBuilder.SetUnitType(UnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `SetSpec(ForgeTypeId)` method instead.'
#pragma warning disable CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
            return fb11.SetUnitType(UnitType.UT_Length);
#pragma warning restore CS0618 // 'UnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `SpecTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning restore CS0618 // 'FieldBuilder.SetUnitType(UnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `SetSpec(ForgeTypeId)` method instead.'
        }
        //*********

        public static Entity Asignar_DUT_NUMERO_FEET(Entity ent1, string nombreCampo, double numero)
        {
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'Entity.Set<FieldType>(string, FieldType, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Set(string, FieldType, ForgeTypeId)` overload instead.'
            ent1.Set<double>(nombreCampo, numero, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
#pragma warning restore CS0618 // 'Entity.Set<FieldType>(string, FieldType, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Set(string, FieldType, ForgeTypeId)` overload instead.'
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
            return ent1;
        }

        public static double Obtener_DUT_NUMERO_FEET_(Entity ent1, string nombreCampo)
        {
#pragma warning disable CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'
#pragma warning disable CS0618 // 'Entity.Get<FieldType>(string, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Get(string, ForgeTypeId)` overload instead.'
            return ent1.Get<double>(nombreCampo, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
#pragma warning restore CS0618 // 'Entity.Get<FieldType>(string, DisplayUnitType)' is obsolete: 'This method is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `Get(string, ForgeTypeId)` overload instead.'
#pragma warning restore CS0618 // 'DisplayUnitType' is obsolete: 'This enumeration is deprecated in Revit 2021 and may be removed in a future version of Revit. Please use the `ForgeTypeId` class instead. Use constant members of the `UnitTypeId` class to replace uses of specific values of this enumeration.'

        }

    }
}
