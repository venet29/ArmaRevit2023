using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.EditarBarra;
using ArmaduraLosaRevit.Model.BarraV;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.EditarBarra
{
    public class ManejadorBarraRefuerzo_CambiarBarra: ManejadorBarraV_CambiarBarra
    {

        protected EditarBarraLosa _editarBarraLosa;
        public ManejadorBarraRefuerzo_CambiarBarra(UIApplication uiapp, EditarBarraDTO editarBarraDTO):base(uiapp, editarBarraDTO)
        {

        }

        public override void CambiarFormaBarra()
        {
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                //
                if (!M1_CalculosIniciales()) return;
                //1
                SeleccionarRebarElemento seleccionarRebarElemento = new SeleccionarRebarElemento(_uiapp, _uiapp.ActiveUIDocument.ActiveView);

                if (!seleccionarRebarElemento.GetSelecionarRebar())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {
                        //Util.ErrorMsg("Error Al Selecciona barra de referencia");
                    }
                    else
                        Debug.WriteLine("Error Al Selecciona barra de referencia");
                    return;
                }



                _editarBarraLosa = new EditarBarraLosa(_uiapp, _editarBarraDTO, seleccionarRebarElemento);
                if (!_editarBarraLosa.ObtenerDatosDebarraRef()) return;
                itemIntervaloBarrasDTO = _editarBarraLosa.GenerarIntervaloBarrasDTO(
                                                            ViewDirectionEntradoView: new XYZ(0, 0, -1),
                                                            DireccionRecorridoBarra: new XYZ(0, 0, -1));

                try
                {
                    using (Transaction transaction = new Transaction(_doc))
                    {
                        transaction.Start("Cambiando Rebar-NH");
                      
                        if (!M6_CreadoNuevaBarraSinTAg())
                        {
                            transaction.RollBack();
                            return;
                        }

                        var idele = newIbarraVertical.M3_ObtenerIdRebar();
                        if (idele == null)
                        {
                            transaction.RollBack();
                            return;
                        }
                        M7_CAmbiarColor(idele);

                        M8_BorrarRebarSeleccionadoSinTRANS(_editarBarraLosa.RebarSeleccion.Id);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show($"Error en 'CambiarBarraVertical'", ex.Message);
                }


            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
#if DEBUG
            //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
            return;
        }
    }
}
