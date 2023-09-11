using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Actualizar;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa.Servicios
{
    public class CopiarParametrosBarrasCopiadas
    {

        public static void M4_CopiarParametros(Document _doc, View _viewIncial, List<ElementId> ListaElementoCopiados_id)
        {
          var  view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            foreach (ElementId item in ListaElementoCopiados_id)
            {
                var elem = _doc.GetElement(item);

                if (elem is Rebar || elem is PathReinforcement)
                {
                    bool resultnombreElevacion = ParameterUtil.SetParaStringNH(elem, "NombreVista", _viewIncial.Name);
                }


                if (elem is Rebar)
                {
                    ((Rebar)elem).SetSolidInView(view3D_Visualizar, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    ((Rebar)elem).SetUnobscuredInView(view3D_Visualizar, true);
                }
                else if (elem is PathReinforcement)
                {
                    ActualizarRebarInSystem.AgregarParametroRebarSystem_sinTrans((PathReinforcement)elem, _viewIncial.Name);

                    ((PathReinforcement)elem).SetSolidInView(view3D_Visualizar, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    ((PathReinforcement)elem).SetUnobscuredInView(view3D_Visualizar, true);
                }

            }
        }

    }
}
