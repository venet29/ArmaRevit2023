using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Prueba.ErrorNh
{
    public class CargarEvento
    {
        private UIControlledApplication application;

        public CargarEvento(UIControlledApplication application)
        {
            this.application = application;
        }
        public void LoadEventsFromCargarInicial()
        {
            // 1. Obtener el tipo de la clase 'cargarInicial'
            Type type = Type.GetType("NamespaceDondeReside.cargarInicial");  // Reemplaza "NamespaceDondeReside" con el namespace adecuado si es diferente

            if (type != null)
            {
                // 2. Crear una instancia de la clase 'cargarInicial'
                object instance = Activator.CreateInstance(type);

                if (instance is IExternalApplication)
                {
                    // 3. Invocar el método 'OnStartup'
                    MethodInfo onStartupMethod = type.GetMethod("OnStartup");
                    if (onStartupMethod != null)
                    {
                        Result startupResult = (Result)onStartupMethod.Invoke(instance, new object[] { application });
                        Console.WriteLine($"OnStartup returned: {startupResult}");
                    }

                    // 3. Invocar el método 'OnShutdown'
                    MethodInfo onShutdownMethod = type.GetMethod("OnShutdown");
                    if (onShutdownMethod != null)
                    {
                        Result shutdownResult = (Result)onShutdownMethod.Invoke(instance, new object[] { application });
                        Console.WriteLine($"OnShutdown returned: {shutdownResult}");
                    }
                }
            }
        }
    }

}


