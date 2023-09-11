using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{

    public class TiposPathReinformentSymbol : ITiposPathReinformentSymbol
    {
        private string TipoBarra;
        private UbicacionLosa ubicacionEnlosa;
        //private SeleccionarLosaBarraRoom seleccionarLosaBarraRoom;
        private string nombreSimboloPathReinforcement;
        private double AnguloBordeRoomYSegundoPtoMouseGrado;
        private Document _doc;
        private View _view;

        public double angle_PelotaLosa1Grado { get; set; }

        public TiposPathReinformentSymbol(SolicitudBarraDTO _solicitudDTO, double AnguloBordeRoomYSegundoPtoMouseGrado=0, double Angle_pelotaLosa1Grado=0)
        {
            this.ubicacionEnlosa = _solicitudDTO.UbicacionEnlosa;
            this.TipoBarra = _solicitudDTO.TipoBarra;
            this.AnguloBordeRoomYSegundoPtoMouseGrado = AnguloBordeRoomYSegundoPtoMouseGrado;
            this._doc = _solicitudDTO.UIdoc.Document;
            this._view = _doc.ActiveView;
            this.angle_PelotaLosa1Grado = Angle_pelotaLosa1Grado;
        }

        public TiposPathReinformentSymbol(SolicitudBarraDTO _solicitudDTO, SeleccionarLosaBarraRoom seleccionarLosaBarraRoom,double Angle_pelotaLosa1Grado) 
        {
            this.ubicacionEnlosa = _solicitudDTO.UbicacionEnlosa;
            this.TipoBarra = _solicitudDTO.TipoBarra;
            this.AnguloBordeRoomYSegundoPtoMouseGrado =  seleccionarLosaBarraRoom.AnguloBordeRoomYSegundoPtoMouseGrado;
            this._doc = _solicitudDTO.UIdoc.Document;
            this.angle_PelotaLosa1Grado = Angle_pelotaLosa1Grado;
        }

        /// <summary>
        /// POR DESARROLLAR
        /// definir la forma de pathReinformentsymbol para la barra del tipo de dinido
        /// </summary>
        /// <param name="ubicacionEnlosa"> ubicacion de la barra</param>
        public string M1_DefinirPathReinformentSymbol()
        {
             nombreSimboloPathReinforcement = "";

            //losa inclinada
            if (M1_1_BuscarSiEsTipoLosaInclinada_TIPOF1($"{TipoBarra}_{ubicacionEnlosa}")) return nombreSimboloPathReinforcement;
            if (M1_2_BuscarSiEsTipoLosaInclinada_TIPOF3(TipoBarra)) return nombreSimboloPathReinforcement;
            
            //escalra tipo f1
            if  (M1_3_BuscarSiEsTipoEscalera_TIPOF1($"{TipoBarra}_{ubicacionEnlosa}"))return nombreSimboloPathReinforcement;
            //escalera tipo f3
            if (M1_4_BuscarSiEsTipoEscalera_TIPOF3($"{TipoBarra}_{ubicacionEnlosa}")) return nombreSimboloPathReinforcement;

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
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9A";
                            break;
                        case "s1":
                            //define el nmbre de la familia de simbolo
                            if (AnguloBordeRoomYSegundoPtoMouseGrado<0.0f)
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1INV";
                            else
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1";
                  

                            break;
                            
                        case "f10":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10";
                            break;
                        case "f10a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10AIzq";
                            break;
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11";
                            break;
                        case "f11a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A";
                            break;
                        case "f16a":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16A";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;
                        case "f22_Izq":
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Izq";
                            break;
                        case "f16_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Dere";
                            break;
                        case "f16_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Izq";
                            break;
                        case "f17a":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17A";
                            break;
                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17B";
                            break;
                        case "f17A_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17ATras";
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
                        case "f19_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19Izq";
                            break;
                        case "f20a":
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20A";
                            break;
                        case "f20aInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20AInv";
                            break;
                        case "f20A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20AIzqTras";
                            break;
                        case "f20B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BIzqTras";
                            break;
                        case "f21a":
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21A";
                            break;
                        case "f21A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21AIzqTras";
                            break;
                        case "f21B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BIzqTras";
                            break;
                        case "f22aInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22AInv";
                            break;
                        case "f22a":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22A";
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
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9B";
                            break;

                        case "s1":
                            //define el nmbre de la familia de simbolo
                            //define el nmbre de la familia de simbolo
                            if (180 >= AnguloBordeRoomYSegundoPtoMouseGrado && AnguloBordeRoomYSegundoPtoMouseGrado > 0.0f)
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
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10ADer";
                            break;
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11";
                            break;
                        case "f11a":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B";
                            break;
                        case "f16b":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16B";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;
                        case "f22_Dere":
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Dere";
                            break;
                        case "f16_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Izq";
                            break;
                        case "f16_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Dere";
                            break;
                        case "f17b":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17B";
                            break;
                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17A";
                            break;
                        case "f17A_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17ATras";
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
                        case "f20b":
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20B";
                            break;
                        case "f20bInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BInv";
                            break;
                        case "f20A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20ADereTras";
                            break;
                        case "f20B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BDereTras";
                            break;
                        case "f21b":
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21B";
                            break;
                        case "f21A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21ADereTras";
                            break;
                        case "f21B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BDereTras";
                            break;
                        case "f22Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Tras";
                            break;
                        case "f22bInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22BInv";
                            break;
                        case "f22b":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22B";
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
                            {  //define el nmbre de la familia de simbolo

                                if (angle_PelotaLosa1Grado > 1 && (ubicacionEnlosa == UbicacionLosa.Inferior || ubicacionEnlosa == UbicacionLosa.Superior))
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupBInv";
                                else
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupB";
                                break;
                            }
                        case "s2":
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
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9B";
                            break;
                        case "s1":
                            //define el nmbre de la familia de simbolo
                            if (AnguloBordeRoomYSegundoPtoMouseGrado <= 0.0f)
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1INV";
                            else
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1";
                            break;
                        case "f10":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10";
                            break;
                        case "f10a":
                            {  //define el nmbre de la familia de simbolo

                                if (angle_PelotaLosa1Grado > 1 && (ubicacionEnlosa == UbicacionLosa.Inferior || ubicacionEnlosa == UbicacionLosa.Superior))
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B";
                                else
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10ADer";
                                break;
                            }
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11";
                            break;
                        case "f11a":
                            {  //define el nmbre de la familia de simbolo
                                
                                if (angle_PelotaLosa1Grado > 1 && (ubicacionEnlosa == UbicacionLosa.Inferior || ubicacionEnlosa == UbicacionLosa.Superior))
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10ADer";
                                else
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11B";
                                break;
                            }
                        case "f16b":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16B";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;
                        case "f22_Dere":
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Dere";
                            break;
                        case "f16_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Izq";
                            break;
                        case "f16_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Dere";
                            break;
                        case "f17b":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17B";
                            break;
                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17A";
                            break;
                        case "f17A_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17ATras";
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
                        case "f20b":
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20B";
                            break;
                        case "f20bInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BInv";
                            break;
                        case "f20A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20ADereTras";
                            break;
                        case "f20B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BDereTras";
                            break;
                        case "f21b":
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21B";
                            break;
                        case "f21A_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21ADereTras";
                            break;
                        case "f21B_Dere_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BDereTras";
                            break;
                        case "f22Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Tras";
                            break;
                        case "f22bInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22BInv";
                            break;
                        case "f22b":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22B";
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
                            {  //define el nmbre de la familia de simbolo

                                if (angle_PelotaLosa1Grado > 1 && (ubicacionEnlosa == UbicacionLosa.Inferior || ubicacionEnlosa == UbicacionLosa.Superior))
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupAInv";
                                else
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1SupA";
                                break;
                            }
                       
                        case "s2":
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
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F9A";
                            break;
                        case "s1":
                            //define el nmbre de la familia de simbolo
                            if (180 >= AnguloBordeRoomYSegundoPtoMouseGrado && AnguloBordeRoomYSegundoPtoMouseGrado > 0.0f)
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1INV";
                            else
                                nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_S1";
                            break;
                        case "f10":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10";
                            break;
                        case "f10a":
                            {
                                if (angle_PelotaLosa1Grado > 1 && (ubicacionEnlosa == UbicacionLosa.Inferior || ubicacionEnlosa == UbicacionLosa.Superior))
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A";
                                else
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10AIzq";
                                break;
                            }
                        case "f11":
                            //define el nmbre de la familia de simbolo
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11";
                            break;
                        case "f11a":
                            //define el nmbre de la familia de simbolo
                            {
                                if (angle_PelotaLosa1Grado>1 && (ubicacionEnlosa==UbicacionLosa.Inferior || ubicacionEnlosa == UbicacionLosa.Superior))
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F10AIzq";
                                else
                                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F11A";                                
                                break;
                            }
                        case "f16a":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16A";
                            break;
                        case "f16":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16";
                            break;

                        case "f22_Izq":
                        case "f22":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Izq";
                            break;
                        case "f16_Dere":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Dere";
                            break;                        
                        case "f16_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F16Izq";
                            break;
                        case "f17a":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17A";
                            break;
                        case "f17":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17B";
                            break;
                        case "f17A_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F17ATras";
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
                        case "f19_Izq":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F19Izq";
                            break;
                        case "f20a":
                        case "f20":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20A";
                            break;
                        case "f20aInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20AInv";
                            break;
                        case "f20A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20AIzqTras";
                            break;
                        case "f20B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F20BIzqTras";
                            break;
                        case "f21a":
                        case "f21":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21A";
                            break;
                        case "f21A_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21AIzqTras";
                            break;
                        case "f21B_Izq_Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F21BIzqTras";
                            break;
                        case "f22Tras":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22Tras";
                            break;
                        case "f22aInv":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22AInv";
                            break;
                        case "f22a":
                            nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F22A";
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

        private bool M1_1_BuscarSiEsTipoLosaInclinada_TIPOF1(string tipoBarra)
        {
            //_a   pata arriba
            //_b   pata abajo
            switch (tipoBarra)
            {


                case "f1_a_Izquierda":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A_PATAA";
                    return true;
                    
                case "f1_b_Izquierda":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A_PATAB";
                    return true;
                    
                case "f1_a_Derecha":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B_PATAA";
                    return true;
                    
                case "f1_b_Derecha":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B_PATAB";
                    return true;
                    
                case "f1_a_Superior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B_PATAA";
                    return true;
                    
                case "f1_b_Superior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B_PATAB";
                    return true;
                    
                case "f1_a_Inferior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A_PATAA";
                    return true;
                    
                case "f1_b_Inferior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A_PATAB";
                    return true;
                case "f1A_pataB_Inferior":
                case "f1A_pataB_Izquierda":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A_PATAB";
                    return true;
                case "f1B_pataB_Derecha":
                case "f1B_pataB_Superior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B_PATAB";
                    return true;

                case "f1A_pataA_Inferior":
                case "f1A_pataA_Izquierda":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A_PATAA";
                    return true;
                case "f1B_pataA_Derecha":
                case "f1B_pataA_Superior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B_PATAA";
                    return true;
            }
            return false;
        }

        private bool M1_2_BuscarSiEsTipoLosaInclinada_TIPOF3(string tipoBarra)
        {
            switch (tipoBarra)
            {
                case "f3_a0":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3_PATAA0";
                    return true;
                case "f3_b0":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3_PATAB0";
                    return true;
                case "f3_0a":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3_PATA0A";
                    return true;
                case "f3_0b":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3_PATA0B";
                    return true;
                case "f3_ab":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3_PATAAB";

                    return true;
                case "f3_ba":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3_PATABA";
                    return true;
            }
            return false;
        }

        private bool M1_4_BuscarSiEsTipoEscalera_TIPOF3(string tipoBarra)
        {
            //f3_esc45
            //f3_esc135
            //Derecha, Izquierda, Superior, Inferior/
            switch (tipoBarra)

            {
                case "f3_esc45_Izquierda":
                case "f3_esc45_Inferior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3A_ESC45";
                    return true;
                case "f3_esc135_Izquierda":
                case "f3_esc135_Inferior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3A_ESC135";
                    return true;
                case "f3_esc45_Superior":
                case "f3_esc45_Derecha":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3B_ESC45";
                    return true;
                case "f3_esc135_Superior":
                case "f3_esc135_Derecha":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3B_ESC135";
                    return true;

                case "f3b_esc45_Izquierda"://casoos descartado por ingeniero
                case "f3b_esc45_Inferior"://casoos descartado por ingeniero
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3A2_ESC45";
                    return true;
                case "f3b_esc135_Izquierda"://casoos descartado por ingeniero
                case "f3b_esc135_Inferior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3A2_ESC135";
                    return true;
                case "f3b_esc45_Superior"://casoos descartado por ingeniero
                case "f3b_esc45_Derecha"://casoos descartado por ingeniero
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3B2_ESC45";
                    return true;
                case "f3b_esc135_Superior"://casoos descartado por ingeniero
                case "f3b_esc135_Derecha"://casoos descartado por ingeniero
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F3B2_ESC135";
                    return true;

            }
            return false;
        }

        private bool M1_3_BuscarSiEsTipoEscalera_TIPOF1(string tipoBarra)
        {
            //f1_esc135_sinpata
            //Derecha, Izquierda, Superior, Inferior/
            switch (tipoBarra)

            {
                case "f1_esc45_conpata_Izquierda":
                case "f1_esc45_conpata_Inferior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A2_ESC45";
                    return true;

                case "f1_esc45_conpata_Superior":
                case "f1_esc45_conpata_Derecha":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B2_ESC45";
                    return true;

                case "f1_esc135_sinpata_Izquierda":
                case "f1_esc135_sinpata_Inferior":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1A2_ESC135";
                    return true;
                case "f1_esc135_sinpata_Superior":
                case "f1_esc135_sinpata_Derecha":
                    nombreSimboloPathReinforcement = "M_Path Reinforcement Symbol_F1B2_ESC135";
                    return true;

            }
            return false;
        }

        public Element ObtenerFamilia()
        {
            string scala = _view.ObtenerNombre_EscalaConfiguracion().ToString();  //ConstNH.CONST_ESCALA_BASE.ToString();// view.Scale.ToString();

            return TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreSimboloPathReinforcement + "_" + scala, _doc);
        }

    }
}
