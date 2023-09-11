using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.RebarShapeNh.Tipos
{
    internal class REbarShapeNHFXXDTONull : RebarShapeNhBase, IRebarShapeNh
    {
      
        public REbarShapeNHFXXDTONull(UIApplication uiapp, SolicitudBarraDTO solicitudDTO,DatosNuevaBarraDTO datosNuevaBarraDTO) : base(uiapp,  solicitudDTO ,datosNuevaBarraDTO, null)
        {
            this._uiapp = uiapp;
            DatosNuevaBarraDTO_ = datosNuevaBarraDTO;
        }
        public bool M0_Ejecutar() => false;

        public bool M1_PreCAlculos() => false;


    }
}