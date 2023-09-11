using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarPath.CambiarLetras
{
    public class LetraCambiarNULL: LetraCambiar_base, ILetraCambiar
    {

        public LetraCambiarNULL():base()
        {
            Isok = false;
        }

        public void Ejecutar()
        {

        }
    
    }
}
