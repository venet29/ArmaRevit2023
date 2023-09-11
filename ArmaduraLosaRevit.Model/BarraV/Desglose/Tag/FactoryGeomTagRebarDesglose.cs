using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
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

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Tag
{
    //public interface IGeometriaTag
    //{
    //    List<TagBarra> listaTag { get; set; }
    //    void CAlcularPtosDeTAg(bool IsGarficarEnForm=false);
    //}
    public class FactoryGeomTagRebarDesglose
    {

        public static IGeometriaTag CrearIGeomTagRebarDesaglose(UIApplication _uiapp, RebarElevDTO _RebarElevDTO)
        {
       
            switch (_RebarElevDTO.tipoBarra)
            {

                case TipoRebarElev.Estribo:
                    return new GeomeTagEstribo(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboTraba:
                    return new GeomeTagTraba(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboViga:
                    return new GeomeTagEstriboViga(_uiapp, _RebarElevDTO);
                case TipoRebarElev.EstriboVigaTraba:
                    return new GeomeTagTrabaViga(_uiapp, _RebarElevDTO);
                default:
                    return  new GeomeTagNull(); 

            }
        }

    }

}
