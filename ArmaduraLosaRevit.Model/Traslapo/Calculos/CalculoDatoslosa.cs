using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Traslapo.Calculos
{
    public class CalculoDatoslosa
    {
        private PathReinforcement _pathReinforcement;
        private Room _room;
        private Document _doc;

        public ContenedorDatosLosaDTO _datosLosaYpathInicialesDTO { get; private set; }

        public CalculoDatoslosa(SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto, Document doc)
        {
            this._room = seleccionarPathReinfomentConPto.RoomSelecionado;
            this._pathReinforcement = seleccionarPathReinfomentConPto.PathReinforcement;
            this._doc = doc;
            _datosLosaYpathInicialesDTO = new ContenedorDatosLosaDTO(seleccionarPathReinfomentConPto);
        }


        public ContenedorDatosLosaDTO ObtenerContenedorDatosLosa()
        {
            try
            {
                if (ObtenerEspesor() == Result.Failed ||
                     ObtenerLargoMinimo() == Result.Failed ||
                     ObtenerEspaciamietos() == Result.Failed ||
                     ObtenerDiametro() == Result.Failed ||
                     ObtenerAnguloPEltalosa() == Result.Failed ||
                     ObtenerTipoDireccionBarra() == Result.Failed)
                    return _datosLosaYpathInicialesDTO;

                _datosLosaYpathInicialesDTO.IsOk = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error    ex:{ex.Message}");
                _datosLosaYpathInicialesDTO.IsOk = false;
            }
            return _datosLosaYpathInicialesDTO;
        }
        public ContenedorDatosLosaDTO ObtenerContenedorDatosFUND()
        {
            if (
                 ObtenerEspaciamietos() == Result.Failed ||
                 ObtenerDiametro() == Result.Failed ||
                 ObtenerTipoDireccionBarra() == Result.Failed)
                return _datosLosaYpathInicialesDTO;

            _datosLosaYpathInicialesDTO.IsOk = true;
            return _datosLosaYpathInicialesDTO;
        }

        private Result ObtenerEspesor()
        {

            double aux_espesor = 0;
            bool result = double.TryParse(ParameterUtil.FindParaByBuiltInParameter(_datosLosaYpathInicialesDTO._losa, BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM, _doc), out aux_espesor);
            if (!result) { TaskDialog.Show("Obtener Espesor ", "No se encuentra espesor en losa"); return Result.Failed; }
            _datosLosaYpathInicialesDTO.espesorLosaCM = Util.FootToCm(aux_espesor);/// aux_espesor;
            return Result.Succeeded;

        }


        private Result ObtenerLargoMinimo()
        {
            if (_room == null)
            {
                Util.ErrorMsg("Room seleccionado null");
                return Result.Failed;
            }

            double aux_largomin = 0;
            bool resultLargoMin = double.TryParse(ParameterUtil.FindValueParaByName(_room.Parameters, "LargoMin", _doc), out aux_largomin);
            if (!resultLargoMin) { TaskDialog.Show("Obtener Largo Minimo ", "No se encuentra Largo minimo"); return Result.Failed; }
            _datosLosaYpathInicialesDTO.largoMinimo = aux_largomin;
            return Result.Succeeded;
        }


        private Result ObtenerEspaciamietos()
        {

            //double aux_espaciamiento = 0;
            //bool result = double.TryParse(ParameterUtil.FindValueParaByName(_pathReinforcement.Parameters, "Bar Spacing", _doc), out aux_espaciamiento);
            //if (!result) { TaskDialog.Show("Obtener Espaciamiento Barra ", "No se encuentra espaciamiento barra"); return Result.Failed; }
            _datosLosaYpathInicialesDTO.Espaciamiento = _pathReinforcement.ObtenerEspaciamiento_foot();
            return Result.Succeeded;
        }


        private Result ObtenerDiametro()
        {
            _datosLosaYpathInicialesDTO.diametroEnFoot = _pathReinforcement.ObtenerDiametro_foot();
            return Result.Succeeded;
        }
        private Result ObtenerTipoDireccionBarra()
        {
            Parameter elemntRebarType = ParameterUtil.FindParaByName(_pathReinforcement, "TipoDireccionBarra");
            if (elemntRebarType == null) { TaskDialog.Show("Obtener TipoDireccionBarra ", "No se encuentra TipoDireccionBarra"); return Result.Failed; }
            string _TipoDireccionBarra = elemntRebarType.AsString();

            _datosLosaYpathInicialesDTO.TipoDireccionBarra = _TipoDireccionBarra;
            return Result.Succeeded;
        }



        private Result ObtenerAnguloPEltalosa()
        {
            double AnguloRoom = 0;
            bool resultLargoMin = double.TryParse(ParameterUtil.FindValueParaByName(_room.Parameters, "Angulo", _doc), out AnguloRoom);
            if (!resultLargoMin) { TaskDialog.Show("Obtener Largo Minimo ", "No se encuentra Largo minimo"); return Result.Failed; }
            _datosLosaYpathInicialesDTO.AnguloRoomRad = AnguloRoom;
            return Result.Succeeded;
        }
    }
}
