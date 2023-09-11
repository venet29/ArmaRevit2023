using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Ayuda
{
    class ayudaObtenerElement
    {
        public static IEnumerable<Element> ObtenerID_segunTipo(ElementoPath cc)
        {
            if (cc is ElementoPathRein)
            {
                return ((ElementoPathRein)cc)._lista_A_DeRebarInSystem;
            }
            else if (cc is ElementoPathRebar)
            {
                List<Element> newList = new List<Element>();
                newList.Add(((ElementoPathRebar)cc)._rebar);
                return newList;
            }
            else
                return new List<Element>();
        }
    }
}
