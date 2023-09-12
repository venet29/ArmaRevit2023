using ArmaduraLosaRevit.Model.BarraV.TipoTagH;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.TipoTag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTag
    {


        public static IGeometriaTag CrearGeometriaTag(Document doc, SeleccionarLosaBarraRoom seleccionarLosaBarraRoom, SolicitudBarraDTO _solicitudBarraDTO, List<XYZ> listaPtosPerimetroBarras)
        {
            if (seleccionarLosaBarraRoom == null) return new GeomeTagNull();
            return CrearGeometriaTag(doc, seleccionarLosaBarraRoom.PtoConMouseEnlosa1, _solicitudBarraDTO, listaPtosPerimetroBarras);
        }

        public static IGeometriaTag CrearGeometriaTag(Document doc, XYZ ptoMOuse, SolicitudBarraDTO _solicitudBarraDTO, List<XYZ> listaPtosPerimetroBarras)
        {
            //devuelve nulo

            if (ptoMOuse == null) return new GeomeTagNull();
            if (listaPtosPerimetroBarras == null) return new GeomeTagNull();
            if (listaPtosPerimetroBarras.Count != 4) return new GeomeTagNull();
            //crea clase
            IGeometriaTag newGeometriaTag =  obtenerTipoTagPOrTipoBarra(doc, ptoMOuse,  listaPtosPerimetroBarras, _solicitudBarraDTO);
        
            return newGeometriaTag;
        }

   

        private static IGeometriaTag obtenerTipoTagPOrTipoBarra(Document doc, XYZ ptoMOuse,  List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
        {


            switch (EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, _solicitudBarraDTO.TipoBarra))
            {
                case TipoBarra.f1:
                    return new GeomeTagF1(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f1_esc135_sinpata:
                case TipoBarra.f1_esc45_conpata:
                    return new GeomeTagF1Esc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
   
                case TipoBarra.f3_ab:
                case TipoBarra.f3_ba:
                case TipoBarra.f3_a0:
                case TipoBarra.f3_b0:
                case TipoBarra.f3_0a:
                case TipoBarra.f3_0b:
                    return new GeomeTagF3pata(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3_incli:
                    Util.ErrorMsg($"Error al definir caso {TipoBarra.f3_incli.ToString()}");
                   return new GeomeTagNull();
                //escalera********
                case TipoBarra.f3_esc45:
                    return new GeomeTagF3Esc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3_esc135:
                    return new GeomeTagF3Esc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3b_esc45:
                    return new GeomeTagF3BEsc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3b_esc135:
                    return new GeomeTagF3BEsc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                    //************************************************************
                case TipoBarra.f3:
                    return new GeomeTagF3(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3_refuezoSuple:
                    return new GeomeTagF3_refuerzoSuple(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f4:
                    return new GeomeTagF4(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);

                
                case TipoBarra.f7:
                    return new GeomeTagF7(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f11:
                    return new GeomeTagF11(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f11a:
                    return new GeomeTagF11A(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
            
                case TipoBarra.f9:
                    return new GeomeTagF9(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f9a:
                    return new GeomeTagF9A(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f10:
                    return new GeomeTagF10(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f10a:
                    return new GeomeTagF10A(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f16b:
                case TipoBarra.f16a:
                case TipoBarra.f16:
                    return new GeomeTagF16(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f16_Izq:
                    return new GeomeTagF16_Izq(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f16_Dere:
                    return new GeomeTagF16_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f17b:
                case TipoBarra.f17a:
                case TipoBarra.f17:
                    return new GeomeTagF17(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f17A_Tras:
                    return new GeomeTagF17A_Tras(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f17B_Tras:
                    return new GeomeTagF17B_Tras(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f18:
                    return new GeomeTagF18(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f19:
                    return new GeomeTagF19(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f19_Izq:
                    return new GeomeTagF19_Izq(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f19_Dere:
                    return new GeomeTagF19_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f20b:
                case TipoBarra.f20a:
                case TipoBarra.f20:
                    return new GeomeTagF20(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f20bInv:
                case TipoBarra.f20aInv:                
                    return new GeomeTagF20Inv(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f20A_Izq_Tras:
                case TipoBarra.f20B_Dere_Tras:
                    return new GeomeTagF20_Izq(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f20A_Dere_Tras:
                case TipoBarra.f20B_Izq_Tras:
                    return new GeomeTagF20_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21b:
                case TipoBarra.f21a:
                case TipoBarra.f21:
                    return new GeomeTagF21(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21A_Izq_Tras:
                    return new GeomeTagF21(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21A_Dere_Tras:
                    return new GeomeTagF16_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21B_Izq_Tras:
                    return new GeomeTagF16_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21B_Dere_Tras:
                    return new GeomeTagF21(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
        
                case TipoBarra.f22aInv:
                    return new GeomeTagF22aInv(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f22bInv:
                    return new GeomeTagF22bInv(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f22a:
                    return new GeomeTagF22a(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f22b:
                    return new GeomeTagF22b(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f22Tras:
                case TipoBarra.f22_Dere:
                case TipoBarra.f22_Izq:
                case TipoBarra.f22:
                    {
                        if(_solicitudBarraDTO.UbicacionEnlosa==UbicacionLosa.Izquierda || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Inferior)
                            return new GeomeTagF22a(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                        else if (_solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Derecha || _solicitudBarraDTO.UbicacionEnlosa == UbicacionLosa.Superior)
                            return new GeomeTagF22b(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                        return new GeomeTagNull();
                    }
                case TipoBarra.f1_SUP:
                    return new GeomeTagF1_SUP(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                    
                case TipoBarra.s1:
                    return new GeomeTagS1(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.s2:
                    return new GeomeTagS2(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.s3:
                    return new GeomeTagS3(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                default:
                    return new GeomeTagNull();
            }
          
        }


      

        public static IGeometriaTag CrearGeometriaTag_casoRebar( UIApplication uiapp,  RebarInferiorDTO rebarInferiorDTO1)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            XYZ ptoMOuse = rebarInferiorDTO1.ptoSeleccionMouse;
            List<XYZ> listaPtosPerimetroBarras = rebarInferiorDTO1.listaPtosPerimetroBarras;
            SolicitudBarraDTO _solicitudBarraDTO = rebarInferiorDTO1.Obtener_solicitudBarraDTO();

            switch (EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, _solicitudBarraDTO.TipoBarra))
            {
                case TipoBarra.f1:
                    return new GeomeTagF1(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f1_incli:
                    return new GeomeTagF1_incliInf(doc, rebarInferiorDTO1);
                case TipoBarra.f1_esc135_sinpata:
                case TipoBarra.f1_esc45_conpata:
                    return new GeomeTagF1Esc(doc, rebarInferiorDTO1);
                case TipoBarra.f1_b:
                case TipoBarra.f1_a:
                case TipoBarra.f1_ab:
                    return new GeomeTagF1_a(doc, rebarInferiorDTO1);
                case TipoBarra.f3_ab:
                case TipoBarra.f3_ba:
                case TipoBarra.f3_a0:
                case TipoBarra.f3_b0:
                case TipoBarra.f3_0a:
                case TipoBarra.f3_0b:
                    return new GeomeTagF3pata(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3_incli:
                    return new GeomeTagF3_incli(doc, rebarInferiorDTO1);
                //escalera********
                case TipoBarra.f3_esc45:
                    return new GeomeTagF3Esc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3_esc135:
                    return new GeomeTagF3Esc(doc, rebarInferiorDTO1);// new GeomeTagF3Esc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3b_esc45:
                    return new GeomeTagF3BEsc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f3b_esc135:
                    return new GeomeTagF3BEsc(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                //************************************************************
                case TipoBarra.f3:
                    return new GeomeTagF3(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f4:
                    return new GeomeTagF4(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f4_incli:
                    return new GeomeTagF4_incli(doc, rebarInferiorDTO1);
                case TipoBarra.f7:
                    return new GeomeTagF7(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f11:
                    return new GeomeTagF11_rebar(doc, rebarInferiorDTO1);
                case TipoBarra.f9:
                    return new GeomeTagF9(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f16:
                    return new GeomeTagF16(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f16_Izq:
                    return new GeomeTagF16_Izq(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f16_Dere:
                    return new GeomeTagF16_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f17:
                    return new GeomeTagF17(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f17A_Tras:
                    return new GeomeTagF17A_Tras(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f17B_Tras:
                    return new GeomeTagF17B_Tras(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f18:
                    return new GeomeTagF18(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f19:
                    return new GeomeTagF19(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f19_Izq:
                    return new GeomeTagF19_Izq(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f19_Dere:
                    return new GeomeTagF19_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f20:
                    return new GeomeTagF20(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f20A_Izq_Tras:
                case TipoBarra.f20B_Dere_Tras:
                    return new GeomeTagF20_Izq(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f20A_Dere_Tras:
                case TipoBarra.f20B_Izq_Tras:
                    return new GeomeTagF20_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21:
                    return new GeomeTagF21(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21A_Izq_Tras:
                    return new GeomeTagF21(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21A_Dere_Tras:
                    return new GeomeTagF16_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21B_Izq_Tras:
                    return new GeomeTagF16_Dere(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.f21B_Dere_Tras:
                    return new GeomeTagF21(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);

                case TipoBarra.f1_SUP:
                    return new GeomeTagF1_SUP(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);

                case TipoBarra.s1:
                    return new GeomeTagS1(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.s2:
                    return new GeomeTagS2(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.s3:
                    return new GeomeTagS3(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                case TipoBarra.s4_Inclinada:
                    return new GeomeTagS4_incli(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO);
                default:
                    return new GeomeTagNull();
            }

        }


        public static IGeometriaTag CrearGeometriaTagH_Fundaciones(UIApplication uiapp, TipoPataBarra tipoBarrav, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag, XYZ DireccionEfierrado)
        {
            Document doc = uiapp.ActiveUIDocument.Document;


            //string nombreFasmiliaBase = "";
            switch (tipoBarrav)
            {
                case TipoPataBarra.BarraVPataInicial:
                    return new GeomeTagConTagHconPata(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVPataFinal:
                    return new GeomeTagConTagHconPata(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVSinPatas:
                    return new GeomeTagConTagH_RefuerzoViga(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVPataAmbos:
                    return new GeomeTagConTagHconPata(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                default:
                    return new GeomeTagConTagH_RefuerzoViga(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
            }


        
        }

        public static IGeometriaTag CrearGeometriaTag_RebarFundacionesAUto(UIApplication uiapp, TipoPataFund tipoBarrav, XYZ ptoMOuse, PathReinfSeleccionDTO _PathReinfSeleccionDTO)
        {
            Document doc = uiapp.ActiveUIDocument.Document;


            //string nombreFasmiliaBase = "";
            switch (tipoBarrav)
            {
                case TipoPataFund.IzqInf:
                    return new GeomeTagFundRebarIzqPata(uiapp, ptoMOuse, _PathReinfSeleccionDTO);
                case TipoPataFund.DereSup:
                    return new GeomeTagFundRebarDerePata(uiapp, ptoMOuse, _PathReinfSeleccionDTO);
                case TipoPataFund.Sin:
                    return new GeomeTagFundRebarSinPata(uiapp, ptoMOuse, _PathReinfSeleccionDTO);
                case TipoPataFund.Ambos:
                    return new GeomeTagFundRebarAmbasPata_auto(uiapp, ptoMOuse,_PathReinfSeleccionDTO);
                default:
                    return new GeomeTagNull();
            }

        }


    }

}
