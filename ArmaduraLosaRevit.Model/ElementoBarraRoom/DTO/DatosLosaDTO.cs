using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.Traslapo.DTO;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO
{
    public class ContenedorDatosLosaDTO
    {
        private Document _doc;

        public bool IsOk { get; set; } = false;
        public UbicacionLosa ubicacionLosa { get; set; }
        public TipoBarra tipoBarra { get; set; }
        public TipoCaraObjeto UbicacionEnLosa { get;  set; }
        public string TipoDireccionBarraVocal { get;  set; }
        public Floor _losa { get; set; }
        public Room _room { get; set; }
        public double espesorLosaCM { get; set; }
        public double diametroEnFoot { get; set; }
        public double Espaciamiento { get; set; }
        public double largoMinimo { get; set; }
        public double _largoTraslapoFoot { get;  set; }
        public string IDtipo { get;  set; }
        public double AnguloRoomRad { get;  set; }
        public string TipoDireccionBarra { get; internal set; }

        //public double LargoPathreiforment { get; set; }


        public ContenedorDatosLosaDTO(TipoPathReinfDTO tipoPathReinf, ContenedorDatosLosaDTO datosLosaDTO)
        {
            this.ubicacionLosa = tipoPathReinf.Direccion;
            this.tipoBarra = tipoPathReinf.Tipobarra;
            this.UbicacionEnLosa = tipoPathReinf.UbicacionEnLosa;
            this.TipoDireccionBarraVocal = tipoPathReinf.TipoDireccionBarraVocal;

            this.diametroEnFoot = datosLosaDTO.diametroEnFoot;
            this.Espaciamiento = datosLosaDTO.Espaciamiento;
            this.espesorLosaCM = datosLosaDTO.espesorLosaCM;
            this._losa = datosLosaDTO._losa;
            this.largoMinimo = datosLosaDTO.largoMinimo;
            this._room = datosLosaDTO._room;            
            this._largoTraslapoFoot = UtilBarras.largo_traslapFoot_diamFoot(diametroEnFoot);
        }
        public ContenedorDatosLosaDTO()
        {

        }

        public ContenedorDatosLosaDTO(SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto)
        {
            this._doc = seleccionarPathReinfomentConPto.GetDoc();
            ElementId elementId = seleccionarPathReinfomentConPto.PathReinforcement.GetHostId();
            _losa = _doc.GetElement2(elementId) as Floor; ;
            TipoDireccionBarraVocal = seleccionarPathReinfomentConPto._TipoDireccionBarra;
            _room = seleccionarPathReinfomentConPto.RoomSelecionado;

        }
    }
}
