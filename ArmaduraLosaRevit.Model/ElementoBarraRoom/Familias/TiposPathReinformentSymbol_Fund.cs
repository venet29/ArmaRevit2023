using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{



    public class TiposPathReinformentSymbol_Fund : ITiposPathReinformentSymbol
    {

        private readonly DatosNuevaBarraDTO _datosNuevaBarraDTO;
        private string TipoBarra;
        private Document _doc;
        private UbicacionLosa ubicacionEnlosa;
        private string nombreSimboloPathReinforcement;

        public TiposPathReinformentSymbol_Fund(SolicitudBarraDTO _solicitudDTO, DatosNuevaBarraDTO _datosNuevaBarraDTO)
        {
            this.ubicacionEnlosa = _solicitudDTO.UbicacionEnlosa;
            this.TipoBarra = _solicitudDTO.TipoBarra;
            this._doc = _solicitudDTO.UIdoc.Document;
            this._datosNuevaBarraDTO = _datosNuevaBarraDTO;
        }



        /// <summary>
        /// POR DESARROLLAR
        /// definir la forma de pathReinformentsymbol para la barra del tipo de dinido
        /// </summary>
        /// <param name="ubicacionEnlosa"> ubicacion de la barra</param>
        public string M1_DefinirPathReinformentSymbol()
        {
            nombreSimboloPathReinforcement = "";

            if (_datosNuevaBarraDTO.TipoCaraObjeto_ == TipoCaraObjeto.Inferior)
                M1_DefinirPathReinformentSymbol_caraInferior();
            else
                M1_DefinirPathReinformentSymbol_caraSuperior();

            return nombreSimboloPathReinforcement;
        }

        private string M1_DefinirPathReinformentSymbol_caraSuperior()
        {
            nombreSimboloPathReinforcement = "";

            if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.Ambos)
            {
                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F12";
            }
            else if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.IzqInf)
            { nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10AIzq"; }
            else if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.DereSup)
            { nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10ADer"; }
            else { nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3"; }
        

            return nombreSimboloPathReinforcement;
        }

        public string M1_DefinirPathReinformentSymbol_caraInferior()
        {
            nombreSimboloPathReinforcement = "";

            if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.Ambos)
            {
                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11";
            }
            else if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.IzqInf)
            { nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A"; }
            else if (_datosNuevaBarraDTO.TipoPataFun == TipoPataFund.DereSup)
            { nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B"; }
            else { nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3"; }
            return nombreSimboloPathReinforcement;
        }

        public Element ObtenerFamilia()
        {
            string scala = ConstNH.CONST_ESCALA_BASE.ToString();// view.Scale.ToString();

            return TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreSimboloPathReinforcement + "_" + scala, _doc);
        }
    }



}
