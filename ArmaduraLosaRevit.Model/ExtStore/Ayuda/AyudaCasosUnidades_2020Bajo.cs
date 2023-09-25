using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ArmaduraLosaRevit.Model.ExtStore.Ayuda
{
    class AyudaCasosUnidades_2020Bajo
    {
        public static UIApplication _uiapp { get; set; }
        private static PropertyInfo FeetProperty = default;
        private static object selectedValue = default;
        private static Type parameterType = default;
        private static bool ObtenerPropiedad(string nombreEnum)
        {
            var _doc = _uiapp.ActiveUIDocument.Document;

            //***

            List<Type> Tipo_ParameterType = _doc.GetType().Assembly.GetTypes().Where(a => a.IsEnum && a.Name == "DisplayUnitType").ToList();
            if (Tipo_ParameterType.Count > 0)
            {
                // 2. Busca el tipo ParameterType
                parameterType = Tipo_ParameterType.FirstOrDefault();// assembly.GetType("Autodesk.Revit.DB.ParameterType");
            }

            object selectedValue = default;
            // 3. Verifica si es un enum
            if (parameterType != null && parameterType.IsEnum)
            {
                // 4. Obtiene los valores del enum
                Array values = Enum.GetValues(parameterType);

                foreach (var name in values)
                {
                    if (name.ToString() == nombreEnum)
                    {
                        selectedValue = name;
                        break;
                    }
                }
                // 5. Selecciona un valor del enum (por ejemplo, el primer valor)
                //  selectedValue = values.GetValue(0);
            }
            else
                return false;

            return true;
        }

        public static Entity Asignar_DUT_DECIMAL_FEET(UIApplication _uiapp_,Entity ent1, string nombreCampo, XYZ pto)
        {

            //ent1.Set<XYZ>(nombreCampo, pto, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
            try
            {
                _uiapp = _uiapp_;
                if (!ObtenerPropiedad("DUT_DECIMAL_FEET")) return ent1;

                Type entityType = ent1.GetType();
                foreach (var metodod in entityType.GetMethods())
                {
                    try
                    {
                        if (metodod.Name == "Set")
                        {
                            var listaPAtametros = metodod.GetParameters();
                            MethodInfo meth = metodod.MakeGenericMethod(typeof(XYZ));
                            var listaPAtametros2 = metodod.GetParameters();
                            if (listaPAtametros2.Length != 3) continue;
                            if (listaPAtametros2[0].ParameterType.Name != "String") continue;
                            //MethodInfo meth = genericSetMethod.MakeGenericMethod(typeof(XYZ));
                            meth.Invoke(ent1, new object[] { nombreCampo, pto, selectedValue });
                            return ent1;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puedo asignar tipo 'Asignar_DUT_DECIMAL_FEET' ");
            }
            //caso corrector
            // propiedad para revit 2022 hacia arriba
            //ent1.Set(nombreCampo, pto, UnitTypeId.Feet); 
            return ent1;
        }

        public static XYZ Obtener_DUT_DECIMAL_FEET_(UIApplication _uiapp_, Entity ent1,string nombreCampo )
        {

            
            try
            {
                _uiapp = _uiapp_;
                if (!ObtenerPropiedad("DUT_DECIMAL_FEET")) return XYZ.Zero;

                MethodInfo meth = ent1.GetType().GetMethod("Get", new Type[] { typeof(string), parameterType });
                if (meth != null)
                {
                    MethodInfo meth1 = meth.MakeGenericMethod(typeof(XYZ));
                    return meth1.Invoke(ent1, new object[] { nombreCampo, selectedValue }) as XYZ;
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puedo obtener tipo 'Obtener_DUT_DECIMAL_FEET' ");

            }
            return XYZ.Zero;

            //caso corrector
            //return ent1.Get<XYZ>(nombreCampo, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
        }

        public static Entity Asignar_DUT_DECIMAL_FEET(UIApplication _uiapp_, Entity ent1, Field nombreField, XYZ pto)
        {
            try
            {
                _uiapp = _uiapp_;
                if (_uiapp == null) return null;


                if (!ObtenerPropiedad("DUT_DECIMAL_FEET")) return null;

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
                            var listaPAtametros2 = item.GetParameters();
                            if (listaPAtametros2.Length != 3) continue;
                            if (listaPAtametros2[0].ParameterType.Name != "Field") continue;
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

            return ent1;

            //caso corrector
            // ent1.Set(nombreField, pto, DisplayUnitType.DUT_DECIMAL_FEET); ;
            //return ent1;
        }

        internal static FieldBuilder Obtener_UT_Length(UIApplication _uiapp_, FieldBuilder fb11)
        {
            try
            {
                _uiapp = _uiapp_;
                if (_uiapp == null) return null;

                var _doc = _uiapp.ActiveUIDocument.Document;
                //Type parameterType = default;
             //   PropertyInfo FeetProperty = default;
                object selectedValue = default;
                Type parameterType = default;
                int gg = 0;
                foreach (var item in _doc.GetType().Assembly.GetTypes().ToList())
                {
                    if (item.Name.Contains("UnitType"))
                        Debug.WriteLine($"{gg = +1})  nombre: {item.Name}   tipo:{item.BaseType}");
                }
                List<Type> types = _doc.GetType().Assembly.GetTypes().Where(a => a.IsEnum && a.Name == "UnitType").ToList();
                if (types.Count > 0)
                {
                    parameterType = types[0];
                  //  FeetProperty = specTypeId.GetProperty("UT_Length");//"YesNo"

                    // 3. Verifica si es un enum
                    if (parameterType != null && parameterType.IsEnum)
                    {
                        // 4. Obtiene los valores del enum
                        Array values = Enum.GetValues(parameterType);

                        foreach (var name in values)
                        {
                            if (name.ToString() == "UT_Length")
                            {
                                selectedValue = name;
                                break;
                            }
                        }
                        // 5. Selecciona un valor del enum (por ejemplo, el primer valor)
                        //  selectedValue = values.GetValue(0);
                    }
                    else
                        return null;
                }
                else
                {
                    Util.ErrorMsg("Util no se puedo obtener para tipo de parametro compartido A");
    
                    return null;
                }


                MethodInfo meth = fb11.GetType().GetMethod("SetUnitType", new Type[] { parameterType }); //FeetProperty.PropertyType;
                if (meth != null)
                {
                    return meth.Invoke(fb11, new object[] { selectedValue }) as FieldBuilder;
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error al obtener FieldBuilder Up22");
            }

            return null;

            //caso corrector
           // return fb11.SetUnitType(UnitType.UT_Length);

        }


        internal static FieldBuilder Obtener_UT_Numero(UIApplication _uiapp_, FieldBuilder fb11)
        {
            return Obtener_UT_Length(_uiapp_, fb11);

            //caso corrector
            // return fb11.SetUnitType(UnitType.UT_Length);
        }
        //*********

        public static Entity Asignar_DUT_NUMERO_FEET(UIApplication _uiapp_, Entity ent1, string nombreCampo, double numero)
        {
            try
            {
                _uiapp = _uiapp_;
                if (!ObtenerPropiedad("DUT_DECIMAL_FEET")) return ent1;

                Type entityType = ent1.GetType();
                foreach (var item in entityType.GetMethods())
                {
                    try
                    {
                        if (item.Name == "Set")
                        {
                            var listaPAtametros = item.GetParameters();
                            MethodInfo meth = item.MakeGenericMethod(typeof(double));

                            var listaPAtametros2 = item.GetParameters();
                            if (listaPAtametros2.Length != 3) continue;
                            if (listaPAtametros2[0].ParameterType.Name != "String") continue;
                            //MethodInfo meth = genericSetMethod.MakeGenericMethod(typeof(XYZ));
                            meth.Invoke(ent1, new object[] { nombreCampo, numero, selectedValue });
                            return ent1;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puedo asignar tipo 'Asignar_DUT_DECIMAL_FEET' ");
            }
            return ent1;
            //caso corrector
            //ent1.Set<double>(nombreCampo, numero, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
            //return ent1;
        }

        public static double Obtener_DUT_NUMERO_FEET_(UIApplication _uiapp_, Entity ent1, string nombreCampo)
        {


            try
            {
                // return ent1.Get<double>(nombreCampo, UnitTypeId.Feet);// UnitTypeId.Feet);

               // var result2 = ent1.Get<double>(nombreCampo, UnitTypeId.Feet);
                _uiapp = _uiapp_;
                if (!ObtenerPropiedad("DUT_DECIMAL_FEET")) return 0;
                double result = 0;
                MethodInfo meth = ent1.GetType().GetMethod("Get", new Type[] { typeof(string), parameterType });
                if (meth != null)
                {
                    MethodInfo meth1 = meth.MakeGenericMethod(typeof(double));
                    result = (double)meth1.Invoke(ent1, new object[] { nombreCampo, selectedValue });
                    return result;
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puedo obtener tipo 'Obtener_DUT_DECIMAL_FEET' ");

            }
            return 0;
            //return ent1.Get<double>(nombreCampo, DisplayUnitType.DUT_DECIMAL_FEET);// UnitTypeId.Feet);
        }


        //4) string

        public static Entity Asignar_DUT_String_FEET(UIApplication _uiapp_, Entity ent1, string nombreCampo, string texto)
        {
            try
            {
                _uiapp = _uiapp_;
                if (!ObtenerPropiedad("DUT_DECIMAL_FEET")) return ent1;

                Type entityType = ent1.GetType();
                foreach (var item in entityType.GetMethods())
                {
                    try
                    {
                        if (item.Name == "Set")
                        {
                            var listaPAtametros = item.GetParameters();
                            MethodInfo meth = item.MakeGenericMethod(typeof(string));

                            var listaPAtametros2 = item.GetParameters();
                            if (listaPAtametros2.Length != 3) continue;
                            if (listaPAtametros2[0].ParameterType.Name != "String") continue;
                            //MethodInfo meth = genericSetMethod.MakeGenericMethod(typeof(XYZ));
                            meth.Invoke(ent1, new object[] { nombreCampo, texto, selectedValue });
                            return ent1;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puedo asignar tipo 'Asignar_DUT_DECIMAL_FEET' ");
            }
            return ent1;

            //caso corrector
            //ent.Set(nombreCampo, numero, UnitTypeId.Feet);// UnitTypeId.Feet);
            //return ent;
        }

        public static double Obtener_DUT_String_FEET(UIApplication _uiapp_, Entity ent1, string nombreCampo)
        {

            try
            {
                // return ent1.Get<double>(nombreCampo, UnitTypeId.Feet);// UnitTypeId.Feet);

                var result2 = ent1.Get<string>(nombreCampo, UnitTypeId.Feet);
                _uiapp = _uiapp_;
                if (!ObtenerPropiedad("DUT_DECIMAL_FEET")) return 0;
                double result = 0;
                MethodInfo meth = ent1.GetType().GetMethod("Get", new Type[] { typeof(string), FeetProperty.PropertyType });
                if (meth != null)
                {
                    MethodInfo meth1 = meth.MakeGenericMethod(typeof(string));
                    result = (double)meth1.Invoke(ent1, new object[] { nombreCampo, selectedValue });
                    return result;
                }
            }
            catch (Exception)
            {
                Util.ErrorMsg("No se puedo obtener tipo 'Obtener_DUT_DECIMAL_FEET' ");

            }
            return 0;
            //return ent1.Get<double>(nombreCampo, UnitTypeId.Feet);// UnitTypeId.Feet);

        }


    }
}
