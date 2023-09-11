using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.EditarBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.EditarBarra
{
    public class EditarBarraLosa: EditarBarraV
    {



        public EditarBarraLosa(UIApplication uiapp, EditarBarraDTO editarBarraDTO, SeleccionarRebarElemento seleccionarRebarElemento) :base( uiapp,  editarBarraDTO, seleccionarRebarElemento)
        {
        }

        public bool ObtenerDatosDebarraRef()
        {
            try
            {
                A_ObtenerDatosDebarra();
                ObtenerDireccionENfierradoRefuerzoLosa();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'ObtenerDatosDebarra' ->  ex:{ex.Message}");
                return IsOk = false;
            }

            return true;
        }
        private void ObtenerDireccionENfierradoRefuerzoLosa()
        {

            if (_editarBarraDTO.TipoCasobarra == TipoCasobarra.BarraVertical)
            {

                XYZ vectorSeleccion = ptoini.GetXY0() - ptoPosicionTAg.GetXY0();
                double resul = Util.GetProductoEscalar(vectorSeleccion.Normalize(), _view.RightDirection.GetXY0());
                DireccionEnFierrado = (resul > 0 ?
                                           _view.RightDirection :
                                           new XYZ(-_view.RightDirection.X, -_view.RightDirection.Y, _view.RightDirection.Z));
            }
            else if (_editarBarraDTO.TipoCasobarra == TipoCasobarra.BarraVertical)
            {
                
                DireccionEnFierrado = (ptoPosicionTAg.Z > ptoini.Z ? new XYZ(0, 0, -1) : new XYZ(0, 0, 1));

            }
            else if (_editarBarraDTO.TipoCasobarra == TipoCasobarra.BarraRefuerzoLosa)
            {
                XYZ vectorSeleccion = (ptoini.GetXY0() - ptoPosicionTAg.GetXY0()).Normalize();
                DireccionEnFierrado = _editarBarraDTO.ModificadorDireccionEnfierrado* Util.CrossProduct(vectorSeleccion.Normalize(), new XYZ(0, 0, 1)).Normalize();
         
            }

        }
    }
}
