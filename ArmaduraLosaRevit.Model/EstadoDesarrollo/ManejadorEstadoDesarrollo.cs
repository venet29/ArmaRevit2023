using ArmaduraLosaRevit.Model.EstadoDesarrollo.JsonNh;
using ArmaduraLosaRevit.Model.EstadoDesarrollo.Modelo;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EstadoDesarrollo
{
    public class ManejadorEstadoDesarrollo
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ContenedorProyectos _ListaProyectos { get; set; }

        private string _nombreArchivo;
        private string _rutaArchivo;
        private string _rutaCompleta;

        public Proyecto ActualProyecto { get; private set; }

        public ManejadorEstadoDesarrollo(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            _ListaProyectos = new ContenedorProyectos();
            _nombreArchivo = "ListaEstadoProyectos";
            _rutaArchivo = @"\\SERVER-CDV\Dibujo2\Proyectos\PROYECTOS REVIT\DATA\BASE DE DATOS PROYECTOS DE ESTRUCTURA\";
            _rutaCompleta = _rutaArchivo + _nombreArchivo + ".json";
        }


        public bool Cargar()
        {
            try
            {
                if (!File.Exists(_rutaCompleta)) return true;

                _ListaProyectos = CreadoJsonEStado.LeerArchivoJsonING_conruta(_rutaArchivo + _nombreArchivo + ".json");
                if (_ListaProyectos == null)
                    _ListaProyectos = new ContenedorProyectos();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearVigasConSacados'. Ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool Ejecutar()
        {
            try
            {

                ActualProyecto = new Proyecto();
                if (!ActualProyecto.ObtenerDatosDeProyecto(_doc)) return false;

                Proyecto _proyecantiguao = _ListaProyectos.ListaPRoyectosTotal.Where(c => c.NombreProyecto == ActualProyecto.NombreProyecto && c.numero == ActualProyecto.numero).FirstOrDefault();
                if (_proyecantiguao != null)
                    _ListaProyectos.ListaPRoyectosTotal.Remove(_proyecantiguao);

                _ListaProyectos.ListaPRoyectosTotal.Add(ActualProyecto);

                //cargar vistas
                var aux_ListaEstructura = SeleccionarView.ObtenerView_losa_elev_fund_estructura_sheet(_uiapp.ActiveUIDocument);
                foreach (View item in aux_ListaEstructura)
                {
                    viewNH _newviewNH = new viewNH(item.Id.IntegerValue);
                    if (_newviewNH.ObtenerDatos(_doc))
                        ActualProyecto.ListasView.Add(_newviewNH);
                }

                var aux_ListaSheet = SeleccionarView.Getsheet(_uiapp.ActiveUIDocument);

                foreach (ViewSheet item in aux_ListaSheet)
                {
                    viewNH _newviewNH = new viewNH(item.Id.IntegerValue);
                    if (_newviewNH.ObtenerDatos(_doc))
                        ActualProyecto.ListasView.Add(_newviewNH);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearVigasConSacados'. Ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool GuardarDatosInternoVIewa()
        {
            try
            {

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start($"GuardarEstadoProyecto-NH");
                    for (int i = 0; i < ActualProyecto.ListasView.Count; i++)
                    {
                        var _view = ActualProyecto.ListasView[i];

                        if (_view.IsTerminado)
                            ParameterUtil.SetParaStringNH(_view.OBtenerVistaElement(_doc), FactoryNombre.EstadoViewIsTerminado, EstadoView.Terminado.ToString());
                        else
                            ParameterUtil.SetParaStringNH(_view.OBtenerVistaElement(_doc), FactoryNombre.EstadoViewIsTerminado, EstadoView.Incompleto.ToString());
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearVigasConSacados'. Ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool Exportar()
        {
            try
            {

                CreadoJsonEStado.ExportarAJson_conRuta(_ListaProyectos, _rutaArchivo + _nombreArchivo + ".json");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Exprtar datos json'. Ex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
