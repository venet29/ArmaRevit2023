using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Cambiar.CuantiasRoom
{
    public class CambiarDatosRoomMANEJADOR
    {
        private readonly ExternalCommandData commandData;

        public CambiarDatosRoomMANEJADOR(ExternalCommandData commandData)
        {
            this.commandData = commandData;
        }

        public Result M1_Ejecutar()
        {

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            List<ElementId> seleccionPreviaDeRoom = uidoc.Selection.GetElementIds().ToList();

            if (seleccionPreviaDeRoom.Count == 0)
            {
                M1_1_EjecutarSinSeleccionaPrevia();
            }
            else
            {
                M1_2_EjecutarConSeleccionPrevia(uidoc, seleccionPreviaDeRoom);
            }

            return Result.Succeeded;
        }

        private void M1_1_EjecutarSinSeleccionaPrevia()
        {
            //CArgarFormulario datos default
            CambiarCuantia CambiarCuantia = new CambiarCuantia();
            CambiarCuantia.ShowDialog();

            //Ejecutar
            if (CambiarCuantia.Estado == "Ok")
            {
                RoomCuantiaDatosDto roomCuantiaDatosDto =  new RoomCuantiaDatosDto(CambiarCuantia.diamH, CambiarCuantia.diamV, CambiarCuantia.EspaH, CambiarCuantia.EspaV);
                ICambiarDatosRoom cambiarDatosRoom = new CambiarDatosRoomConSelec(commandData, roomCuantiaDatosDto);
                cambiarDatosRoom.Ejecutar();
            }
        }

        private void M1_2_EjecutarConSeleccionPrevia(UIDocument uidoc, List<ElementId> seleccionactual)
        {
            #region VAlidar Elemento Previo Seleccion

            List<Element> listROmm = new List<Element>();
            if (seleccionactual.Count > 1) Util.ErrorMsg("Solo debe tener un solo Room seleccionado");

            Element roomElem = uidoc.Document.GetElement(seleccionactual[0]);
            if (roomElem == null) Util.ErrorMsg("Error en la preseleccion del ROmm");
            if (!(roomElem is Room)) Util.ErrorMsg("Error en la preseleccion del ROom. Elemento seleccinado no es un Room");
            listROmm.Add(roomElem);
            #endregion

            #region CArgar formulario
            CambiarCuantia CambiarCuantia = new CambiarCuantia(commandData.Application,roomElem);
            CambiarCuantia.ObtenerCuantias();
            CambiarCuantia.ShowDialog(); 
            #endregion

            //ejecutar
            if (CambiarCuantia.Estado == "Ok")
            {
                RoomCuantiaDatosDto roomCuantiaDatosDto =
                    new RoomCuantiaDatosDto(CambiarCuantia.diamH, CambiarCuantia.diamV, CambiarCuantia.EspaH, CambiarCuantia.EspaV);
                ICambiarDatosRoom cambiarDatosRoom = new CambiarDatosRoomSinSelec(commandData, roomCuantiaDatosDto, listROmm);
                cambiarDatosRoom.Ejecutar();
            }
        }
    }
}
