
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.ViewFilter;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista
{
    public class ManejadorVisibilidadActualizarNombreVista
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        public ManejadorVisibilidadActualizarNombreVista(UIApplication _uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._uiapp = _uiapp;
        }

        public void Ejecutar()
        {
            if (_uiapp == null) return;
            View _view = _uiapp.ActiveUIDocument.ActiveView;
            string nombreView = _view.Name;

            if (_view is View3D)
            {
                Util.InfoMsg("Comandos No se puede aplicar en Vistas 3D.");
                return;
            }


            int _paraViewNombre;

            Parameter _para = _view.GetParameter2("View Template");
            if (_para == null)
                _paraViewNombre = -1;
            else
            {
                _paraViewNombre = _para.AsElementId().IntegerValue;
                if (_paraViewNombre != -1)
                {
                    var viewTemplate = _doc.GetElement(_para.AsElementId());
                    string nombre = viewTemplate.Name;
                    if (!(nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_FUND || nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_ESTRUC || nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA || nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV))
                    {
                        _view.DesactivarViewTemplate_ConTrans();
                    }
                    _paraViewNombre = -1;
                }
            }

            ActualizarNameViewForm _ActualizarNameViewForm = new ActualizarNameViewForm(nombreView);
            _ActualizarNameViewForm.ShowDialog();

            if (_ActualizarNameViewForm.Ok)
            {

                UpdateGeneral.M5_DesCargarGenerar(_uiapp);

                try
                {
                    SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                    SeleccionarAreaPath seleccionarAreaPath = new SeleccionarAreaPath(_uiapp, _view);
                    VisibilidadElemenNoEnView VisibilidadElement = new VisibilidadElemenNoEnView(_uiapp);
                    string ViewNombre_ParamtroActual = _view.ObtenerNombre_ViewNombre();
                    string ViewNombre_ = _view.Name;

                    if (ViewNombre_ != ViewNombre_ParamtroActual)
                    {

                        //if (ViewNombre_.Contains(" Copy1 "))
                        //{
                        //    ViewNombre_ParamtroActual = ViewNombre_;
                        //}
                        //else
                        //{
                            DiferenciaNombrePara _DiferenciaNombrePara = new DiferenciaNombrePara(ViewNombre_, ViewNombre_ParamtroActual);
                            _DiferenciaNombrePara.ShowDialog();
                            if (!_DiferenciaNombrePara.resultado) return;
                            if (_DiferenciaNombrePara.resultadoString == "") return;

                            ViewNombre_ParamtroActual = _DiferenciaNombrePara.resultadoString;
                        //}
                    }

                    if (_paraViewNombre == -1)
                    {
                        using (TransactionGroup t = new TransactionGroup(_doc))
                        {
                            t.Start("ActualizarNombreView-NH");

                            ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, seleccionarRebar, seleccionarAreaPath, _view, _view.Name);
                            ManejadorVisibilidad.M6_ActualizarNombreVista(_ActualizarNameViewForm.nombreView, ViewNombre_ParamtroActual);

                            CreadorViewFilter_OcultarBarrasNoDeVista _CreadorViewFilter = new CreadorViewFilter_OcultarBarrasNoDeVista(_uiapp);
                            _CreadorViewFilter.M2_CreateViewFilterTodos(_view);

                            Util.InfoMsg("Actualizacion de nombre de view finalizado");

                            t.Assimilate();
                        }
                    }
                    else
                    {
                        Util.InfoMsg("No fue posible actualizar nombre de vista, posiblemente 'viewTemplate' este bloquendo parametro de 'ViewNombre'");
                    }
                }
                catch (Exception ex)
                {
                    Util.DebugDescripcion(ex);
                }
                UpdateGeneral.M4_CargarGenerar(_uiapp);
            }

        }


        public void Ejecutar(View _view, string nuevoNombreView)
        {
            if (_uiapp == null) return;
            _uiapp.ActiveUIDocument.ActiveView= _view;
            string nombreView = _view.Name;

            if (_view is View3D)
            {
                Util.InfoMsg("Comandos No se puede aplicar en Vistas 3D.");
                return;
            }
   

            int _paraViewNombre;

            Parameter _para = _view.GetParameter2("View Template");
            if (_para == null)
                _paraViewNombre = -1;
            else
            {
                _paraViewNombre = _para.AsElementId().IntegerValue;
                if (_paraViewNombre != -1)
                {
                    var viewTemplate = _doc.GetElement(_para.AsElementId());
                    string nombre = viewTemplate.Name;
                    if (!(nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_FUND || nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_ESTRUC || nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_LOSA || nombre == ConstNH.NOMBRE_VIEW_TEMPLATE_ELEV))
                    {
                        _view.DesactivarViewTemplate_ConTrans();
                    }
                    _paraViewNombre = -1;
                }
            }

            //UpdateGeneral.M5_DesCargarGenerar(_uiapp);
            try
            {
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _view);
                SeleccionarAreaPath seleccionarAreaPath = new SeleccionarAreaPath(_uiapp, _view);
                VisibilidadElemenNoEnView VisibilidadElement = new VisibilidadElemenNoEnView(_uiapp);
                string ViewNombre_ParamtroActual = _view.ObtenerNombre_ViewNombre();
                string ViewNombre_ = _view.Name;

                //if (ViewNombre_ != ViewNombre_ParamtroActual)
                //{

                //    if (ViewNombre_.Contains(" Copy "))
                //    {
                //        ViewNombre_ParamtroActual = ViewNombre_;
                //    }
                //    //else if (IsparametroIgualNuevoNombre)
                //    //{ 
                    
                //    //}
                //    else
                //    {
                //        DiferenciaNombrePara _DiferenciaNombrePara = new DiferenciaNombrePara(ViewNombre_, ViewNombre_ParamtroActual);
                //        _DiferenciaNombrePara.ShowDialog();
                //        if (!_DiferenciaNombrePara.resultado) return;
                //        if (_DiferenciaNombrePara.resultadoString == "") return;

                //        ViewNombre_ParamtroActual = _DiferenciaNombrePara.resultadoString;
                //    }
                //}

                if (_paraViewNombre == -1)
                {
                    using (TransactionGroup t = new TransactionGroup(_doc))
                    {
                        t.Start("ActualizarNombreView-NH");



                        try
                        {
                            using (Transaction tr = new Transaction(_doc, "CambiarNombre"))
                            {
                                tr.Start();
                                _view.Name = nuevoNombreView;
                                tr.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                
                        }

                   
                        ManejadorVisibilidad ManejadorVisibilidad = new ManejadorVisibilidad(_uiapp, VisibilidadElement, seleccionarRebar, seleccionarAreaPath, _view, _view.Name);
                        ManejadorVisibilidad.M6_ActualizarNombreVista(nuevoNombreView, nuevoNombreView);

                        CreadorViewFilter_OcultarBarrasNoDeVista _CreadorViewFilter = new CreadorViewFilter_OcultarBarrasNoDeVista(_uiapp);
                        _CreadorViewFilter.M2_CreateViewFilterTodos(_view);

                      //  Util.InfoMsg("Actualizacion de nombre de view finalizado");

                        t.Assimilate();
                    }
                }
                else
                {
                    Util.InfoMsg("No fue posible actualizar nombre de vista, posiblemente 'viewTemplate' este bloquendo parametro de 'ViewNombre'");
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
           // UpdateGeneral.M4_CargarGenerar(_uiapp);


        }


    }
}
