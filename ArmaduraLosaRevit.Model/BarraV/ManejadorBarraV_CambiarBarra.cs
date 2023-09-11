using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.EditarBarra;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.BarraV.Traslapos;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.ayuda;

namespace ArmaduraLosaRevit.Model.BarraV
{
    public class ManejadorBarraV_CambiarBarra
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected  EditarBarraDTO _editarBarraDTO;


        // private List<IbarraBase> _listaDebarra;
        //  private List<CreadorBarrasV> _listaCreadorBarrasV;
        protected IntervaloBarrasDTO itemIntervaloBarrasDTO;
        protected IbarraBase newIbarraVertical;
        protected EditarBarraV EditarBarra;

        public ManejadorBarraV_CambiarBarra(UIApplication uiapp, EditarBarraDTO editarBarraDTO)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._editarBarraDTO = editarBarraDTO;

        }

        public virtual void CambiarFormaBarra()
        {

            try
            {
                //
                if (!M1_CalculosIniciales()) return;
                //1
                SeleccionarTagRebar seleccionarTagRebar = new SeleccionarTagRebar(_uiapp);

                if (!seleccionarTagRebar.GetSelecionarRebarTag())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {
                        //Util.ErrorMsg("Error Al Selecciona barra de referencia");
                    }
                    else
                        Debug.WriteLine("Error Al Selecciona barra de referencia");
                    return;
                }

                EditarBarra = new EditarBarraV(_uiapp, _editarBarraDTO, seleccionarTagRebar);
                if (!EditarBarra.ObtenerRebarYTag()) return;

                if (!EditarBarra.A_ObtenerDatosDebarra()) return;
                itemIntervaloBarrasDTO = EditarBarra.GenerarIntervaloBarrasDTO(
                                                            ViewDirectionEntradoView:-_doc.ActiveView.ViewDirection,
                                                            DireccionRecorridoBarra: -_doc.ActiveView.ViewDirection);
                int valorIdBarraBorrada = 0;
                ManejadorRemplazarTraslapo _ManejadorRemplazarTraslapo = new ManejadorRemplazarTraslapo(_uiapp);
                _ManejadorRemplazarTraslapo.ObtenerListaTraslapoDTO();

                try
                {
                    using (Transaction transaction = new Transaction(_doc))
                    {
                        transaction.Start("Cambiando Rebar-NH");

                        if (!M6_CreadoNuevaBarra())
                        {
                            transaction.RollBack();
                            return; 
                        }

                        var idele = newIbarraVertical.M3_ObtenerIdRebar();
                      
                        if (idele==null)
                        {
                            transaction.RollBack();
                            return;
                        }
                        M7_CAmbiarColor(idele);


                        valorIdBarraBorrada = EditarBarra.RebarSeleccion.Id.IntegerValue;
                        M8_BorrarRebarSeleccionadoSinTRANS(EditarBarra.RebarSeleccion.Id);
                        transaction.Commit();
                    }


                    var BarraCreada = newIbarraVertical.GetResult();

                    _ManejadorRemplazarTraslapo.CrearNuevoTraslapo(valorIdBarraBorrada, BarraCreada._rebar);
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
#if DEBUG
            //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
            return;
        }

        protected bool M6_CreadoNuevaBarra()
        {
            try
            {
                //creando barra
                ConfiguracionTAgBarraDTo confBarraTag;
                if (_editarBarraDTO.TipoCasobarra == TipoCasobarra.BarraVertical)
                {
                    double desfaseCodo = ConstNH.DESPLAZAMIENTO_DESPLA_LEADERELBOW_V_FOOT;
                    //creando barra
                    confBarraTag = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 1.5),
                        IsDIrectriz = true,
                        LeaderElbow = new XYZ(0, 0, desfaseCodo),
                        tagOrientation = TagOrientation.Vertical,
                        BarraTipo=TipoRebar.ELEV_BA_V
                    };
                }
                else
                {         //creando barra
                    confBarraTag = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, -0),
                        IsDIrectriz = false,
                        LeaderElbow = XYZ.Zero,
                        tagOrientation = TagOrientation.Horizontal,
                        BarraTipo = TipoRebar.ELEV_BA_H
                    };

                }


                newIbarraVertical = FactoryBarraVertical.FActoryIGeneraraTagVertical_Horizontal_cambiarBarras(_uiapp, itemIntervaloBarrasDTO, _editarBarraDTO.TipoCasobarra);
                if (!newIbarraVertical.M1_DibujarBarra()) return false;

                newIbarraVertical.M2_DibujarTags(confBarraTag);
                newIbarraVertical.M3_ObtenerIdRebar();

                newIbarraVertical.M1_1_DibujarBarraCOnfiguracion();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"M6_CreadoNuevaBarra -> ex: {ex.Message}");

                return false;
            }

            return true;
        }


        protected bool M6_CreadoNuevaBarraSinTAg()
        {
            try
            {
                newIbarraVertical = FactoryBarraVertical.FActoryIGeneraraTagVertical_Horizontal_cambiarBarras(_uiapp, itemIntervaloBarrasDTO, _editarBarraDTO.TipoCasobarra);
                if (!newIbarraVertical.M1_DibujarBarra()) return false;

                newIbarraVertical.M3_ObtenerIdRebar();

                newIbarraVertical.M1_1_DibujarBarraCOnfiguracion();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"M6_CreadoNuevaBarra -> ex: {ex.Message}");

                return false;
            }

            return true;
        }

        protected bool M1_CalculosIniciales()
        {
            try
            {

                View3D _view3D_paraVisualizar = TiposFamilia3D.Get3DVisualizar(_doc);
                if (_view3D_paraVisualizar == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial : 3D VISUALIZAR");
                    return false;
                }
                View3D _view3D = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial : 3D BUSCAR");
                    return false;
                }
                _editarBarraDTO.view3D_paraBuscar = _view3D;
                _editarBarraDTO.view3D_paraVisualizar = _view3D_paraVisualizar;
                _editarBarraDTO.viewActual = _doc.ActiveView;

                AyudaManejadorTraslapo.Reset();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error En los calculos iniciales: {ex.Message}");
                return false;
            }
            return true;
        }





        protected void M7_CAmbiarColor(ElementId elemid)
        {

            if (elemid == null) return;

            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeElementColorSinTrans(elemid, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta));
            //visibilidadElement
        }

        public void M8_BorrarRebarSeleccionadoConTRANS(ElementId elemId)
        {
            try
            {
                using (Transaction transaction = new Transaction(_doc))
                {
                    transaction.Start("Borrar Rebar-NH");
                    _doc.Delete(elemId);
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.Message);
            }
        }


        public bool M8_BorrarRebarSeleccionadoSinTRANS(ElementId elemId)
        {
            try
            {
                _doc.Delete(elemId);

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Revit", ex.Message);
                return false; ;
            }

            return true;
        }

    }
}
