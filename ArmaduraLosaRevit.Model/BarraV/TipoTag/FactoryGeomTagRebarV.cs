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
    public class FactoryGeomTagRebarV
    {

        public static IGeometriaTag CrearGeometriaTagV(UIApplication uiapp, TipoPataBarra tipoBarrav, XYZ ptoIni, XYZ ptoFin, XYZ posiciontag, XYZ DireccionEfierrado)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            double desplazamietoParaTExtoEstribo = 1.5;
            //string nombreFasmiliaBase = "";
            switch (tipoBarrav)
            {
                case TipoPataBarra.BarraVPataInicial:
                    return new GeomeTagPataInicialV(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado.Normalize()* desplazamietoParaTExtoEstribo);
                case TipoPataBarra.BarraVPataFinal:
                    return new GeomeTagPataFinalV(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado.Normalize()* desplazamietoParaTExtoEstribo);
                case TipoPataBarra.BarraVSinPatas:
                    return new GeomeTagSinPata(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado.Normalize()* desplazamietoParaTExtoEstribo);
                case TipoPataBarra.BarraVPataAmbos_Horquilla:
                    return new GeomeTagPataAmbosV_Horquilla(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado.Normalize() * desplazamietoParaTExtoEstribo);
                case TipoPataBarra.BarraVPataAmbos:
                    return new GeomeTagPataAmbosV(doc, ptoIni + DireccionEfierrado, ptoFin + DireccionEfierrado, posiciontag + DireccionEfierrado.Normalize()* desplazamietoParaTExtoEstribo);
                default:
                    return new GeomeTagNull();
            }
        }

    }

}
