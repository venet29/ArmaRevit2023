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

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoTag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTagRefuerzo
    {

     
        public static IGeometriaTag CrearGeometriaTagBarraRefuerzo(UIApplication uiapp, TipoBarraRefuerzo tipoBarrav, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
  

            //string nombreFasmiliaBase = "";
            switch (tipoBarrav)
            {
                case TipoBarraRefuerzo.BarraRefPataAmbos:
                case TipoBarraRefuerzo.BarraRefPataInicial:
                case TipoBarraRefuerzo.BarraRefPataFinal:
                    return new GeomeTagConPataRef(doc, ptoIni, ptoFin, posiciontag);
                case TipoBarraRefuerzo.BarraRefSinPatas:
                    return new GeomeTagSinPataRef(doc, ptoIni , ptoFin , posiciontag );           
                case TipoBarraRefuerzo.EstriboRef:
                    return new GeomeTagEstriboRef(doc, ptoIni, ptoFin, posiciontag);
                default:
                    return new GeomeTagNull();
            }

        }


        public static IGeometriaTag CrearGeometriaTagBarraEstribo(UIApplication uiapp, TipoPataBarra tipoBarrav, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag)
        {
            Document doc = uiapp.ActiveUIDocument.Document;


            //string nombreFasmiliaBase = "";
            switch (tipoBarrav)
            {
                case TipoPataBarra.BarraVPataInicial:
                    return new GeomeTagNull();
                case TipoPataBarra.BarraVPataFinal:
                    return new GeomeTagNull();
                case TipoPataBarra.BarraVSinPatas:
                    return new GeomeTagSinPataRef(doc, ptoIni, ptoFin, posiciontag);
                case TipoPataBarra.BarraVPataAmbos:
                    return new GeomeTagNull();
                default:
                    return new GeomeTagNull();
            }

        }

    }

}
