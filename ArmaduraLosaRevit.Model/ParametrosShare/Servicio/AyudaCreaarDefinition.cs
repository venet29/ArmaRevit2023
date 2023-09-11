using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.ParametrosShare.Servicio
{
    public class AyudaCreaarDefinition
    {
        private ExternalDefinitionCreationOptions instance;
        private UIApplication _uiapp;

        public AyudaCreaarDefinition(UIApplication uiapp)
        {
            this._uiapp = uiapp;
        }

        public ExternalDefinitionCreationOptions Ejecutar( string NombrePArametro, string TipoParametro)
        {
            instance = null;
            if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
            {
                Ejecutar22UP(_uiapp, NombrePArametro, TipoParametro);//"Altura","SpecTypeId.String.Text"
            }
            else
            {
                Ejecutar21Down(_uiapp, NombrePArametro, TipoParametro);//"Altura", "ParameterType.Text"
            }
            return instance;
            //Definition value = null;

            //Type parameterType = default;


            //List<Type> types = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "SpecTypeId").ToList();
            //if (types.Count > 0)
            //{
            //    Type specTypeId = types[0];
            //    // Obteniendo la clase anidada Boolean
            //    parameterType = _doc.GetType().Assembly.GetType(specTypeId.FullName + "+Boolean");

            //}
            //else
            //{
            //    // 1. Carga el ensamblado
            //    //   Assembly assembly = Assembly.LoadFrom("ruta_al_ensamblado.dll");
            //    List<Type> Tipo_ParameterType = _doc.GetType().Assembly.GetTypes().Where(a => a.IsEnum && a.Name == "ParameterType").ToList();

            //    // 2. Busca el tipo ParameterType
            //    parameterType = Tipo_ParameterType.FirstOrDefault();// assembly.GetType("Autodesk.Revit.DB.ParameterType");
            //}


            //object selectedValue = default;
            //// 3. Verifica si es un enum
            //if (parameterType != null && parameterType.IsEnum)
            //{
            //    // 4. Obtiene los valores del enum
            //    Array values = Enum.GetValues(parameterType);

            //    // 5. Selecciona un valor del enum (por ejemplo, el primer valor)
            //    selectedValue = values.GetValue(0);
            //}
            //else if (parameterType != null && parameterType.IsClass)
            //{
            //    // Obtener la propiedad YesNo de Clase1.Boolean
            //    PropertyInfo yesNoProperty = parameterType.GetProperty("YesNo");
            //    parameterType = yesNoProperty.PropertyType;
            //    // Como es una propiedad estática, pasamos null al método GetValue y SetValue
            //    // Leer el valor de YesNo (esto retornará null si nunca se ha establecido un valor)
            //    selectedValue = yesNoProperty.GetValue(null);
            //}
            //else
            //    return;

            //// 6. Obtiene el Type de Clase1
            ////Type clase1Type = assembly.GetType("Autodesk.Revit.DB.Clase1");
            //List<Type> ls = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "ExternalDefinitionCreationOptions").ToList();
            //Type clase1Type = ls.FirstOrDefault();
            ////ExternalDefinitionCreationOptions ExtDefinitionCreationOptions = new ExternalDefinitionCreationOptions(entidadDefinition.nombreParametro, entidadDefinition.TipoParametro);

            //// 7. Encuentra el constructor que acepta un ParameterType
            //ConstructorInfo constructor = clase1Type.GetConstructor(new Type[] { typeof(string), parameterType });

            //if (constructor == null) return;

            //string nombreDefinico = "Altura";
            //// 8. Crea una instancia de Clase1 pasando el valor del enum como argumento
            //ExternalDefinitionCreationOptions instance = constructor.Invoke(new object[] { nombreDefinico, selectedValue }) as ExternalDefinitionCreationOptions;

            //Console.WriteLine("Instancia de Clase1 creada!");



        }

        private bool Ejecutar22UP(UIApplication _uiapp, string NombrePArametro, string TipoParametro)//NombrePArametro
        {
            try
            {
                
                    
                Document _doc = _uiapp.ActiveUIDocument.Document;
                Definition value = null;
                Type parameterType = default;
                string[] nombre = TipoParametro.Split('.'); //"SpecTypeId.String.Text"
                string tipo= nombre[1];
                string valor = nombre[2];

                List<Type> types = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "SpecTypeId").ToList();
                if (types.Count > 0)
                {
                    Type specTypeId = types[0];
                    // Obteniendo la clase anidada Boolean
                    parameterType = _doc.GetType().Assembly.GetType(specTypeId.FullName + "+"+ tipo);//Boolean
                }
                else
                {
                    Util.ErrorMsg("Util no se puedo obtener para tipo de parametro compartido A");
                    return false;
                }

                object selectedValue = default;

                if (parameterType != null && parameterType.IsClass)
                {
                    // Obtener la propiedad YesNo de Clase1.Boolean
                    PropertyInfo yesNoProperty = parameterType.GetProperty(valor);//"YesNo"
                    parameterType = yesNoProperty.PropertyType;
                    // Como es una propiedad estática, pasamos null al método GetValue y SetValue
                    // Leer el valor de YesNo (esto retornará null si nunca se ha establecido un valor)
                    selectedValue = yesNoProperty.GetValue(null);
                }
                else
                {
                    Util.ErrorMsg("Util no se puedo obtener para tipo de parametro compartido B");
                    return false;
                }

                // 6. Obtiene el Type de Clase1
                //Type clase1Type = assembly.GetType("Autodesk.Revit.DB.Clase1");
                List<Type> ls = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "ExternalDefinitionCreationOptions").ToList();
                Type clase1Type = ls.FirstOrDefault();
                //ExternalDefinitionCreationOptions ExtDefinitionCreationOptions = new ExternalDefinitionCreationOptions(entidadDefinition.nombreParametro, entidadDefinition.TipoParametro);

                // 7. Encuentra el constructor que acepta un ParameterType
                ConstructorInfo constructor = clase1Type.GetConstructor(new Type[] { typeof(string), parameterType });

                if (constructor == null) return false;

                // 8. Crea una instancia de Clase1 pasando el valor del enum como argumento
                instance = constructor.Invoke(new object[] { NombrePArametro, selectedValue }) as ExternalDefinitionCreationOptions;

                Console.WriteLine("Instancia de Clase1 creada!");
            }
            catch (Exception ex)
            {

                Util.ErrorMsg("Error al crear parametro compartido individual");
                return false;
            }
            return true;
        }
        private bool Ejecutar21Down(UIApplication _uiapp, string NombrePArametro, string TipoParametro)//NombrePArametro
        {
            try
            {
                Document _doc = _uiapp.ActiveUIDocument.Document;
                Definition value = null;

                string[] nombre= TipoParametro.Split('.');

                Type parameterType = default;

                // 1. Carga el ensamblado
                //   Assembly assembly = Assembly.LoadFrom("ruta_al_ensamblado.dll");
                List<Type> Tipo_ParameterType = _doc.GetType().Assembly.GetTypes().Where(a => a.IsEnum && a.Name == "ParameterType").ToList();
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
                        if (name.ToString() == nombre[1])
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

                // 6. Obtiene el Type de Clase1
                //Type clase1Type = assembly.GetType("Autodesk.Revit.DB.Clase1");
                List<Type> ls = _doc.GetType().Assembly.GetTypes().Where(a => a.IsClass && a.Name == "ExternalDefinitionCreationOptions").ToList();
                Type clase1Type = ls.FirstOrDefault();
                //ExternalDefinitionCreationOptions ExtDefinitionCreationOptions = new ExternalDefinitionCreationOptions(entidadDefinition.nombreParametro, entidadDefinition.TipoParametro);

                // 7. Encuentra el constructor que acepta un ParameterType
                ConstructorInfo constructor = clase1Type.GetConstructor(new Type[] { typeof(string), parameterType });

                if (constructor == null) return false;

                // 8. Crea una instancia de Clase1 pasando el valor del enum como argumento
                instance = constructor.Invoke(new object[] { NombrePArametro, selectedValue }) as ExternalDefinitionCreationOptions;

                Console.WriteLine("Instancia de Clase1 creada!");

            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Error al crear parametro compartido individual");
                return false;
            }
            return true;
        }

    }
}
