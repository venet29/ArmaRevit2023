using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{



    public class TiposPathReinformentSymbol_X : ITiposPathReinformentSymbol
    {
        private string TipoBarra;
        private Document _doc;
        private UbicacionLosa ubicacionEnlosa;
#pragma warning disable CS0649 // Field 'TiposPathReinformentSymbol_X.seleccionarLosaBarraRoom' is never assigned to, and will always have its default value null
        private SeleccionarLosaBarraRoom seleccionarLosaBarraRoom;
#pragma warning restore CS0649 // Field 'TiposPathReinformentSymbol_X.seleccionarLosaBarraRoom' is never assigned to, and will always have its default value null
        private string nombreSimboloPathReinforcement;

        public TiposPathReinformentSymbol_X(SolicitudBarraDTO _solicitudDTO)
        {
            this.ubicacionEnlosa = _solicitudDTO.UbicacionEnlosa;
            this.TipoBarra = _solicitudDTO.TipoBarra;
            this._doc = _solicitudDTO.UIdoc.Document;
        }

    

        /// <summary>
        /// POR DESARROLLAR
        /// definir la forma de pathReinformentsymbol para la barra del tipo de dinido
        /// </summary>
        /// <param name="ubicacionEnlosa"> ubicacion de la barra</param>
        public string M1_DefinirPathReinformentSymbol()
        {
             nombreSimboloPathReinforcement = "";

            switch (ubicacionEnlosa)
            {
                case UbicacionLosa.Izquierda:

                    switch (TipoBarra)
                    {
    
                        case "f1":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A";
                            break;
                        case "f1_SUP":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupA";
                            break;
                        case "s2":
                        case "f3_refuezoSuple":
                        case "f3":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3";
                            break;

                        case "f3_A0":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3";
                            break;
                        case "f4":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F4";
                            break;
                        case "f9":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9";
                            break;
                        case "s3":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S3B";
                            break;

                        case "f7":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F7A";
                            break;

                        case "f9a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F6";
                            break;
                
                        case "f10":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10";
                            break;
                        case "f10a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A";
                            break;
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;
                        case "f16_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Izq";
                            break;
                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17A";
                            break;
                        case "f17A_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17ATras";
                            break;
                        case "f18":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F18";
                            break;
                        case "f19":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19";
                            break;
                        case "f19_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19Izq";
                            break;
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20A";
                            break;
                        case "f20A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20AIzqTras";
                            break;
                        case "f20B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BIzqTras";
                            break;
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21A";
                            break;
                        case "f21A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21AIzqTras";
                            break;
                        case "f21B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BIzqTras";
                            break;
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Izq";
                            break;
                        default:
                            break;
                    }
                    break;
                case UbicacionLosa.Derecha:
                    //  TipoOrientacion = TipoOrientacionBarra.Horizontal;
                    switch (TipoBarra)
                    {
     
                        case "f1":
                            //para dibujar el symbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B";
                            break;

                        case "f1_SUP":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupB";
                            break;
                        case "s2":
                        case "f3_refuezoSuple":
                        case "f3":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3";
                            break;
                        case "f4":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F4";
                            break;
                        case "f9":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9";
                            break;
                        case "s3":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S3A";
                            break;

                        case "f7":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F7B";
                            break;

                        case "f9a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F6";
                            break;
                    

                        case "f10":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10";
                            break;
                        case "f10a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B";
                            break;
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;
                        case "f16_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Dere";
                            break;
                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17B";
                            break;
                        case "f17B_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17BTras";
                            break;
                        case "f18":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F18";
                            break;
                        case "f19":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19";
                            break;
                        case "f19_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19Dere";
                            break;
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20B";
                            break;
                        case "f20A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20ADereTras";
                            break;
                        case "f20B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BDereTras";
                            break;
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21B";
                            break;
                        case "f21A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21ADereTras";
                            break;
                        case "f21B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BDereTras";
                            break;
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Dere";
                            break;
                        default:
                            break;
                    }
                    break;
                case UbicacionLosa.Superior:
                    //  TipoOrientacion = TipoOrientacionBarra.Vertical;
                    switch (TipoBarra)
                    {
         
                        case "f1":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B";
                            break;
                        case "f1_SUP":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupB";
                            break;
                        case "s2":
                        case "f3_refuezoSuple":
                        case "f3":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3";
                            break;
                        case "f4":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F4";
                            break;
                        case "f9":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9";
                            break;
                        case "s3":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_FS3A";
                            break;

                        case "f7":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F7B";
                            break;
                        case "f9a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F6";
                            break;

                        case "f10":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10";
                            break;
                        case "f10a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B";
                            break;
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;
                        case "f16_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Dere";
                            break;
                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17B";
                            break;
                        case "f17B_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17BTras";
                            break;
                        case "f18":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F18";
                            break;
                        case "f19":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19";
                            break;
                        case "f19_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19Dere";
                            break;
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20B";
                            break;
                        case "f20A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20ADereTras";
                            break;
                        case "f20B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BDereTras";
                            break;
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21B";
                            break;
                        case "f21A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21ADereTras";
                            break;
                        case "f21B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BDereTras";
                            break;
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Dere";
                            break;
                        default:
                            break;
                    }
                    break;
                case UbicacionLosa.Inferior:
                    //  TipoOrientacion = TipoOrientacionBarra.Vertical;
                    switch (TipoBarra)
                    {

                        case "f1":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A";
                            break;
                        case "f1_SUP":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupA";
                            break;
                        case "s2":
                        case "f3_refuezoSuple":
                        case "f3":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3";
                            break;
                        case "f4":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F4";
                            break;
                        case "f9":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9";
                            break;
                        case "s3":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_FS3B";
                            break;

                        case "f7":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F7A";
                            break;
                        case "f9a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F6";
                            break;
                        case "s1":
                            //define el nmbre de la familia de simbolo
                            if (180 >= seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado && seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado > 0.0f)
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1INV";
                            else
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1";
                            break;
                        case "f10":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10";
                            break;
                        case "f10a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A";
                            break;
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;
                        case "f16_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Izq";
                            break;


                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17A";
                            break;
                        case "f17A_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17ATras";
                            break;
                        case "f18":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F18";
                            break;
                        case "f19":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19";
                            break;
                        case "f19_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19Izq";
                            break;
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20A";
                            break;
                        case "f20A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20AIzqTras";
                            break;
                        case "f20B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BIzqTras";
                            break;
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21A";
                            break;
                        case "f21A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21AIzqTras";
                            break;
                        case "f21B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BIzqTras";
                            break;
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Izq";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return nombreSimboloPathReinforcement;
        }

        public Element ObtenerFamilia()
        {
            string scala = ConstNH.CONST_ESCALA_BASE.ToString();// view.Scale.ToString();

            return  TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreSimboloPathReinforcement + "_" + scala, _doc);
        }

    }
}
