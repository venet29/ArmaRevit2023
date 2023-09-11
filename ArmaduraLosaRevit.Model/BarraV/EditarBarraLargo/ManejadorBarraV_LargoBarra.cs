using ArmaduraLosaRevit.Model.BarraV.Creador;
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
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.EditarBarra;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using System.Runtime.InteropServices.WindowsRuntime;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo
{
    public class ManejadorBarraV_LargoBarra
    {
        protected UIApplication _uiapp;
        protected Document _doc;
        protected EditarBarraDTO _editarBarraDTO;
        private EditarBarraLargoDTO editarBarraLargoDTO;
        private View _view;
        private bool IsObtenerLArgoConMouse;


        // private List<IbarraBase> _listaDebarra;
        //  private List<CreadorBarrasV> _listaCreadorBarrasV;
        protected IntervaloBarrasDTO itemIntervaloBarrasDTO;
        protected IbarraBase newIbarraVertical;
        private double LargoExtender_foot;
        protected EditarBarraVLargos EditarBarra;

        public ManejadorBarraV_LargoBarra(UIApplication uiapp, EditarBarraDTO newEditarBarraDTO, EditarBarraLargoDTO _EditarBarraLargoDTO)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._editarBarraDTO = newEditarBarraDTO;
            editarBarraLargoDTO = _EditarBarraLargoDTO;
            this._view = _doc.ActiveView;
            this.IsObtenerLArgoConMouse = false;

        }

        public virtual void CambiarLargoBarra()
        {
            ////UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            //UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                //
                if (!M1_CalculosIniciales()) return;
                //1

                SeleccionarRebarElemento seleccionaRebar = new SeleccionarRebarElemento(_uiapp, _view);


                if (!seleccionaRebar.GetSelecionarRebar())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {
                        //Util.ErrorMsg("Error Al Selecciona barra de referencia");
                    }
                    else
                        Debug.WriteLine("Error Al Selecciona barra de referencia");
                    return;
                }

                EditarBarra = new EditarBarraVLargos(_uiapp,
                                                     _editarBarraDTO,
                                                     seleccionaRebar,
                                                     IsObtenerLArgoConMouse);

         
                if (!EditarBarra.A_ObtenerDatosDebarra()) return;

                if (!EditarBarra.B_EjecutarListaWrapper()) return;
               // if (!EditarBarra.A_ObtenerListaParametros()) return;
               // if (!EditarBarra.B_ObtenerListaCurvas()) return;

                if (editarBarraLargoDTO.IsUsarMouse)
                {
                    if (!EditarBarra.C_ObtenerLargoExtenderConMouse()) return;
                    if (!EditarBarra.E_recortarDiametros(editarBarraLargoDTO.DeltaUsarMouse_cm)) return;
                    LargoExtender_foot = EditarBarra.LargoExtender;
                }
                else
                {
                    LargoExtender_foot = Util.CmToFoot(editarBarraLargoDTO.largoExtender_cm);
                }
                if (!EditarBarra.D_ExtenderBarra(LargoExtender_foot)) return;

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra:" + ex.Message);
            }
            //UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
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

                    //creando barra
                    confBarraTag = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 1.5),
                        IsDIrectriz = true,
                        LeaderElbow = new XYZ(0, 0, 1.5),
                        tagOrientation = TagOrientation.Vertical
                    };
                }
                else
                {         //creando barra
                    confBarraTag = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, -0),
                        IsDIrectriz = false,
                        LeaderElbow = XYZ.Zero,
                        tagOrientation = TagOrientation.Horizontal
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
                _editarBarraDTO.viewActual = _doc.ActiveView; ;
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
