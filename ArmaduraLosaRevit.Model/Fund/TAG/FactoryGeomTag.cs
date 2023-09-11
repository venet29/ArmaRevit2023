using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.TipoTag;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.Fund.TAG
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTag_fund
    {


        public static IGeometriaTag CrearGeometriaTag(Document doc, XYZ ptoMOuse, SolicitudBarraDTO _solicitudBarraDTO, PathReinfSeleccionDTO _PathReinfSeleccionDTO)
        {
            //devuelve nulo
            if (_PathReinfSeleccionDTO==null) return new GeomeTagNull();
            List<XYZ> listaPtosPerimetroBarras = _PathReinfSeleccionDTO.ListaPtosPerimetroBarras;
            if (ptoMOuse == null) return new GeomeTagNull();
            if (listaPtosPerimetroBarras == null) return new GeomeTagNull();
            if (listaPtosPerimetroBarras.Count != 4) return new GeomeTagNull();
            //crea clase
            IGeometriaTag newGeometriaTag =  obtenerTipoTagPOrTipoBarra(doc, ptoMOuse, _PathReinfSeleccionDTO, _solicitudBarraDTO);
        
            return newGeometriaTag;
        }

   

        private static IGeometriaTag obtenerTipoTagPOrTipoBarra(Document doc, XYZ ptoMOuse, PathReinfSeleccionDTO _PathReinfSeleccionDTO, SolicitudBarraDTO _solicitudBarraDTO)
        {
            switch (EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, _solicitudBarraDTO.TipoBarra.ToLower()))
            {
                case TipoBarra.f11b:
                case TipoBarra.f11a:
                case TipoBarra.f11:
                case TipoBarra.f12b:
                case TipoBarra.f12a:
                case TipoBarra.f12:               
                    return new GeomeTagF12Fund(doc, ptoMOuse, _PathReinfSeleccionDTO, _solicitudBarraDTO);
                case TipoBarra.f3:
                    return new GeomeTagF3Fund(doc, ptoMOuse, _PathReinfSeleccionDTO, _solicitudBarraDTO);
                default:
                    return new GeomeTagNull();
            }          
        }    
    }

}
