using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cambiar.CuantiasRoom
{
    public interface ICambiarDatosRoom
    {

        void Ejecutar();
    }
    public class CambiarDatosRoomBase
    {
        protected Document _doc;
        protected UIDocument _uidoc;
        protected UIApplication _uiapp;
        protected View _view;

        protected List<Element> ListaRoom;
        protected readonly RoomCuantiaDatosDto roomCuantiaDatosDto;

        public CambiarDatosRoomBase(ExternalCommandData commandData, RoomCuantiaDatosDto roomCuantiaDatosDto, bool isTest = false)
        {
            this._doc = commandData.Application.ActiveUIDocument.Document;
            this._uidoc = commandData.Application.ActiveUIDocument;
            this._uiapp = commandData.Application;
            this._view = this._doc.ActiveView;
            this.roomCuantiaDatosDto = roomCuantiaDatosDto;

        }




        public void CambiarCuantiaRoom(List<Element> ListaRoom)
        {

            for (int i = 0; i < ListaRoom.Count; i++)
            {
                CopiarNuevosParametrosARoom(ListaRoom[i], roomCuantiaDatosDto);
            }

        }

        public void CopiarNuevosParametrosARoom(Element idObj_, RoomCuantiaDatosDto roomDto)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Agregando los parametros internos-NH");
                    //var nm = ParameterUtil.SetParaInt(idObj_, "Numero Losa", roomDto.nombreLosa);
                    //var espesor = ParameterUtil.SetParaInt(idObj_, "Espesor", roomDto.espesor);
                    //var angle = ParameterUtil.SetParaInt(idObj_, "Angulo", Util.GradosToRadianes(item.anguloPelotaLosa));
                    var CH = ParameterUtil.SetParaInt(idObj_, "Cuantia Horizontal", roomDto.diamH + "a" + roomDto.espaH);
                    var CV = ParameterUtil.SetParaInt(idObj_, "Cuantia Vertical", roomDto.diamV + "a" + roomDto.espaV);

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }

    }
}
