using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra
{
     public  interface ICasoBarra
    {
        void LayoutRebar_PathReinforcement();
    }

    public interface ICasoBarraX
    {
        bool LayoutRebar_PathReinforcement(PathReinforcement _createdPathReinforcement, DatosNuevaBarraDTO _datosNuevaBarraDTO);
        bool CambiarCaraSuperior(PathReinforcement _createdPathReinforcement);
    }
}
