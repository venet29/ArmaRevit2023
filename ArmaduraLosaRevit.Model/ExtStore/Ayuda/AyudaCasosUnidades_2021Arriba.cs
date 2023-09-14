using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ArmaduraLosaRevit.Model.ExtStore.Ayuda
{
    public class AyudaCasosUnidades_2021Arriba
    {
        public static UIApplication _uiapp { get; set; }
        private static PropertyInfo FeetProperty = default;
        private static object selectedValue = default;
        public static Entity Asignar_DUT_DECIMAL_FEET(Entity ent1, string nombreCampo, XYZ pto)
        {
            if (!ObtenerPropiedad()) return ent1;
            //version22 hacia arriba
            MethodInfo meth = ent1.GetType().GetMethod("Set", new Type[] { typeof(string), typeof(XYZ), FeetProperty.PropertyType });
            if (meth != null)
                meth.Invoke(ent1, new object[] { nombreCampo, pto, selectedValue });

            // propiedad para revit 2022 hacia arriba
            //ent1.Set(nombreCampo, pto, UnitTypeId.Feet); 
            return ent1;
        }

        private static bool ObtenerPropiedad()
        {
            var _doc = _uiapp.ActiveUIDocument.Document;

            List<Type> types = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "UnitTypeId").ToList();

            if (types.Count > 0)
            {
                Type specTypeId = types[0];
                FeetProperty = specTypeId.GetProperty("Feet");//"YesNo"
                selectedValue = FeetProperty.GetValue(null);
            }
            else
            {
                Util.ErrorMsg("No se puedo obtener para tipo 'Feet' A");
                return false;
            }
            return true;
        }

        public static XYZ Obtener_DUT_DECIMAL_FEET(Entity ent1, string nombreCampo)
        {
            try
            { 
                if (!ObtenerPropiedad()) return XYZ.Zero;

                MethodInfo meth = ent1.GetType().GetMethod("Get", new Type[] { typeof(string), FeetProperty.PropertyType });
                if (meth != null)
                {
                    MethodInfo meth1 = meth.MakeGenericMethod(typeof(XYZ));
                    return meth1.Invoke(ent1, new object[] { nombreCampo, selectedValue }) as XYZ;
                }
            }
            catch (Exception)
            {


            }
            return XYZ.Zero;
            //  return ent1.Get<XYZ>(nombreCampo, UnitTypeId.Feet);// UnitTypeId.Feet);
        }


        public static Entity Asignar_DUT_DECIMAL_FEET(Entity ent1, Field nombreField, XYZ pto)
        {
            try
            {
                if (_uiapp == null) return null;

                var _doc = _uiapp.ActiveUIDocument.Document;
                PropertyInfo FeetProperty = default;
                object selectedValue = default;
                List<Type> types = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "UnitTypeId").ToList();
                if (types.Count > 0)
                {
                    Type specTypeId = types[0];
                    FeetProperty = specTypeId.GetProperty("Feet");//"YesNo"
                    selectedValue = FeetProperty.GetValue(null);
                }
                else
                {
                    Util.ErrorMsg("No se puedo obtener para tipo 'Feet' b");
                    return null;
                }

                /*
                 A)ESTA FORMA SE OBTNEER  EL MTODO GENERA ERROR PORQUE TIENE 4 SOBRECARGAS EL METODO 'SET' APARTE DE SER UN GENERICO
                MethodInfo genericSetMethod = ent1.GetType().GetMethod("Set", new Type[] { typeof(Field), typeof(XYZ), typeof(ForgeTypeId) }); //FeetProperty.PropertyType;
                if (genericSetMethod != null)
                {
                    MethodInfo meth = genericSetMethod.MakeGenericMethod(typeof(XYZ));
                    meth.Invoke(ent1, new object[] { nombreField, pto, selectedValue });
                }
                */

                //* B) SE RECORRE TODOS LOS METODO Y SE PASA 'SET', SOLO SE PUEDO DIFERENCIAR EL CORRESPONDIENTE PQ 'eee1.LocalVariables.Count == 13', NO SE ENCONTRO OTRA
                // DIFERENCIAS ENTRE LOS 4 METODOS SET USANDO REFELXION
                Type entityType = ent1.GetType();
                foreach (var item in entityType.GetMethods())
                {
                    try
                    {
                        if (item.Name == "Set")
                        {
                            MethodInfo meth = item.MakeGenericMethod(typeof(XYZ));
                            var eee1 = meth.GetMethodBody();
                            if (eee1.LocalVariables.Count != 13) continue;
                            //MethodInfo meth = genericSetMethod.MakeGenericMethod(typeof(XYZ));
                            meth.Invoke(ent1, new object[] { nombreField, pto, selectedValue });
                            return ent1;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                Util.ErrorMsg("No se pudo agregar dato a entiddad 'Asignar_DUT_DECIMAL_FEET'");
            }
            catch (Exception)
            {
                Util.ErrorMsg("ERROR: al agregar dato a entiddad 'Asignar_DUT_DECIMAL_FEET'");
            }
            // ent1.Set(nombreField, pto, UnitTypeId.Feet);// UnitTypeId.Feet);
            return ent1;
        }


        public static FieldBuilder Obtener_UT_Length(FieldBuilder fb11)
        {
            try
            {
                if (_uiapp == null) return null;

                var _doc = _uiapp.ActiveUIDocument.Document;
                //Type parameterType = default;
                PropertyInfo FeetProperty = default;
                object selectedValue = default;
                List<Type> types = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "SpecTypeId").ToList();
                if (types.Count > 0)
                {
                    Type specTypeId = types[0];
                    FeetProperty = specTypeId.GetProperty("Length");//"YesNo"

                    selectedValue = FeetProperty.GetValue(null);
                }
                else
                {
                    Util.ErrorMsg("Util no se puedo obtener para tipo de parametro compartido A");
                    return null;
                }


                MethodInfo meth = fb11.GetType().GetMethod("SetSpec", new Type[] { FeetProperty.PropertyType }); //FeetProperty.PropertyType;
                if (meth != null)
                {
                    return meth.Invoke(fb11, new object[] { selectedValue }) as FieldBuilder;
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error al obtener FieldBuilder Up22");
            }
            //return fb11.SetSpec(SpecTypeId.Length);
            return null;
        }
        public static FieldBuilder Obtener_UT_Numero(FieldBuilder fb11)
        {
            //return fb11.SetSpec(SpecTypeId.Length);
            return Obtener_UT_Length(fb11);//.SetSpec(SpecTypeId.Length);
        }



        public static Entity Asignar_DUT_Numero_FEET(Entity ent, string nombreCampo, double numero)
        {
            ent.Set(nombreCampo, numero, UnitTypeId.Feet);// UnitTypeId.Feet);
            return ent;
        }

        public static double Obtener_DUT_Numero_FEET(Entity ent1, string nombreCampo)
        {
            return ent1.Get<double>(nombreCampo, UnitTypeId.Feet);// UnitTypeId.Feet);

        }

    }
}
