using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment
{
    interface IPathSymbol
    {

         Element elemtoSymboloPath { get; set; }
        bool M1_ObtenerPArametros();
        bool M2_ejecutar(ParametroNewTipoPathSymbol TipoFamilia_conpata = null);

    }
}
