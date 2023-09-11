using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.TipoTagH;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using MediaNH = System.Windows.Media;

namespace ArmaduraLosaRevit.Model.BarraV.TipoTag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTagRebarH
    {

     
        public static IGeometriaTag CrearGeometriaTagH(UIApplication uiapp, TipoPataBarra tipoBarrav, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag,  XYZ DireccionEfierrado)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
  

            //string nombreFasmiliaBase = "";
            switch (tipoBarrav)
            {
                case TipoPataBarra.BarraVPataInicial:
                    return new GeomeTagPataInicialH(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVPataFinal:
                    return new GeomeTagPataFinalH(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVSinPatas:
                    return new GeomeTagSinPataH(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                case TipoPataBarra.BarraVPataAmbos:
                    return new GeomeTagPataAmbosH(uiapp, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado);
                default:
                    return new GeomeTagNull();
            }

        }


        public static IGeometriaTag CrearGeometriaTagH_RefuerzoVIga(UIApplication uiapp, TipoPataBarra tipoBarrav, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag, XYZ DireccionEfierrado)
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



    }

}
