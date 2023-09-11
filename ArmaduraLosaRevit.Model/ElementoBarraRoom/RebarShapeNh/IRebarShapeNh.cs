using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh
{
    interface IRebarShapeNh
    {


        RebarShape tipoRebarShapePrincipal { get; set; }
        RebarShape tipoRebarShapeAlternativa { get; set; }
        DatosNuevaBarraDTO DatosNuevaBarraDTO_ { get; set; }

        bool M0_Ejecutar();
        bool M1_PreCAlculos();
        bool M2_EjecutarGeneral();


    }
}
