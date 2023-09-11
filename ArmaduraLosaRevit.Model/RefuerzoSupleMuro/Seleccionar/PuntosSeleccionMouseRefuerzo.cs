using ArmaduraLosaRevit.Model.Fund.Servicios;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RefuerzoSupleMuro.Seleccionar
{
    public class PuntosSeleccionMouseRefuerzo : PuntosSeleccionMouse
    {
        public PuntosSeleccionMouseRefuerzo(UIApplication uiapp) : base(uiapp)
        {
        }

        public bool OrdenarPtosRefuerzo()
        {
            try
            { 

                var result =Util.Ordena2Ptos(p1_origenDiretriz, p2_SentidoDiretriz);
                p1_origenDiretriz = result[0];
                p2_SentidoDiretriz = result[1];
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en '' ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
