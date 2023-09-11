using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using Autodesk.Revit.UI;
using System.Runtime.InteropServices;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.UTILES
{
    public class UbicacionVentana
    {
        private readonly UIApplication _uiapp;

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public bool IsEncontrado { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Screen Screen_ { get; set; }

        public UbicacionVentana(UIApplication _uiapp)
        {
            this._uiapp = _uiapp;
            this.IsEncontrado = false;
        }
        public  bool ObtenerMOnitor()
        {
            try
            {
                var windowHandle = _uiapp.MainWindowHandle;

                // Obtiene la posición de la ventana principal de Revit
                RECT windowRect;
                if (GetWindowRect(windowHandle, out windowRect))
                {
                    X = windowRect.Left;
                    Y= windowRect.Top;
                    int width = windowRect.Right - windowRect.Left;
                    int height = windowRect.Bottom - windowRect.Top;

                    Console.WriteLine("Coordenadas de la ventana:");
                    Console.WriteLine("X: " + X);
                    Console.WriteLine("Y: " + Y);
                    Console.WriteLine("Ancho: " + width);
                    Console.WriteLine("Alto: " + height);

        
                    // Itera a través de los monitores disponibles
                    foreach (Screen screen in Screen.AllScreens)
                    {
                        // Comprueba si la ventana principal de Revit está en el monitor actual
                        if (screen.Bounds.Contains(X+50,Y+50))
                        {
                            Screen_ = screen;
                            // Aquí puedes hacer algo con el monitor
                            // Por ejemplo, imprimir el nombre del dispositivo
                            Console.WriteLine(screen.DeviceName);
                            IsEncontrado = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerMOnitor'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
