using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar.Ayuda
{
   public  class SelecPto
    {
        public static XYZ ptoMouse { get;  set; }

        public static bool SeleccionarDireccionMouse( UIDocument _uidoc, ObjectSnapTypes snapTypes,string mjs)
        {
            // usar filto para que solo se pueda seleccionar floor con el mouse (PickObject)
            try
            {

                //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                ptoMouse = _uidoc.Selection.PickPoint(snapTypes, mjs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
                return false;
            }
            return true;
        }
    }
}
