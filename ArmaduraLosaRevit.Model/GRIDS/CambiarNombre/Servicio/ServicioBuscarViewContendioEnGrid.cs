using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPF;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GRIDS.CambiarNombre.Model;
using ArmaduraLosaRevit.Model.GRIDS.model;
using ArmaduraLosaRevit.Model.modeloNH;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UpdateGenerar;
using ArmaduraLosaRevit.Model.ViewFilter.Filtro3D.ServiciosNH;
using ArmaduraLosaRevit.Model.ViewportnNH.model;
using ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.GRIDS.CambiarNombre.Servicios
{



    public class ServicioBuscarViewContendioEnGrid
    {
        private UIApplication _uiapp;
        private Document _doc;

        private List<EnvoltorioGrid> ListaEnvoltorioGrid;

        private List<PorTiposNh> ListaElev;
        private List<ViewGeom> ListaViewGeom;
        private Dictionary<string, View> ListaVIew;

        public List<EnvoltorioGrid_view> Lista_EnvoltorioGrid_view { get; set; }

        public ServicioBuscarViewContendioEnGrid(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            ListaViewGeom = new List<ViewGeom>();
            Lista_EnvoltorioGrid_view = new List<EnvoltorioGrid_view>();
        }



        public bool Ejecutar()
        {
            try
            {

                M1_CargarGris();

                M2_agruparGridConView();

                //M3_CAmbiarNOmbre();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ServicioBuscarViewContendioEnGrid'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_CargarGris()
        {
            try
            {
                ListaEnvoltorioGrid = TiposGrid.ObtenerTodosGrid(_doc);

                var listaView = SeleccionarView.ObtenerView_losa_elev_fund(_uiapp.ActiveUIDocument);// _uiapp.ActiveUIDocument);
                var listafinal = listaView.Where(c => c.ViewType == ViewType.Section).OrderBy(c => c.Name)
                                        .Select(c => new ViewDTO(c)).ToList();

                for (int i = 0; i < listafinal.Count; i++)
                {
                    var view_ = listafinal[i];
                    ViewGeom _ViewGeom = new ViewGeom(_uiapp, view_);
                    if (_ViewGeom.CAlcularGeometria())
                        ListaViewGeom.Add(_ViewGeom);
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en Obtener ListaEnvoltorioGrid.\n Ex:{ex.Message}");

                return false;
            }
            return true;
        }


        private void M2_agruparGridConView()
        {
            // buscar  elemento in griid

            for (int i = 0; i < ListaEnvoltorioGrid.Count; i++)
            {
                var _grid = ListaEnvoltorioGrid[i];

                Debug.WriteLine($"{i}) grid:{_grid.Nombre}");
                EnvoltorioGrid_view _EnvoltorioGrid_view = new EnvoltorioGrid_view(_grid);
                _EnvoltorioGrid_view.DireccionarGrid();

                for (int j = 0; j < ListaViewGeom.Count; j++)
                {
                    var _view = ListaViewGeom[j];
                    double productoCruz = Util.GetProductoEscalar((_grid.p2 - _grid.p1).Normalize(), _view.viewDTO_.View_.RightDirection);
                    if (Math.Abs(productoCruz) > 0.9 && Util.IsIntersection2(_grid.p1.GetXY0(), _grid.p2.GetXY0(), _view.ptoMinReal.GetXY0(), _view.ptoMaxReal.GetXY0()))
                    {
                        Debug.WriteLine($"          {j}) view:{_view.viewDTO_.Nombre}");
                        _EnvoltorioGrid_view.AgregraView(_view);
                    }

                }

                Lista_EnvoltorioGrid_view.Add(_EnvoltorioGrid_view);
            }

            //REDUCIR
 

        }



        public bool M3_CAmbiarNOmbre(List<EnvoltorioGrid_view> Lista_CambiarNombre)
        {
            try
            {
                ListaVIew = new Dictionary<string, View>();
                //a)
                using (Transaction tr = new Transaction(_doc, "ActualizarNombre-Nh"))
                {

                    tr.Start();


                    // reiniciar los celemetnos
                    Lista_EnvoltorioGrid_view.ForEach(view =>
                    {
                        view.IsAnalizado = false;
                        view.ListaGridAsociados.ForEach(grid =>
                        {
                            grid.IsAnalizado = false;
                        });
                    });

                    //**
                    for (int i = 0; i < Lista_CambiarNombre.Count; i++)
                    {

                        var grid = Lista_CambiarNombre[i];

                        if (grid.IsAnalizado) continue;

                        if (grid.IsSelected)
                        {
                            grid.IsAnalizado = true;

                            if (grid.Nombre_Nuevo != "" && grid.IsCambiarColor)
                            {
                                AnalizarSiNuevoNombreGridExiste(grid.Nombre_Nuevo);
                                grid.Grid_.Grid_.Name = grid.Nombre_Nuevo;// + "-"+grid.Nombre_Nuevo;                       
                            }

                            grid.ListaGridAsociados.Where(c => c.IsOK && c.Nombre_Nuevo != "" && c.IsCambiarColor).ForEach(r =>
                            {
                                r.IsAnalizado = true;
                                AnalizarSiNuevoViewExiste(r.Nombre_Nuevo);
                                //r.viewGeom.viewDTO_.View_.Name = r.Nombre_Nuevo;// + "-" + grid.Nombre_Nuevo;
                                ListaVIew.Add(r.Nombre_Nuevo, r.viewGeom.viewDTO_.View_);
                               
                                // Cambiar nombre con comando
                            });
                        }
                    }
                    tr.Commit();
                }

                // UpdateGeneral.M5_DesCargarGenerar(_uiapp);
                //b)
                foreach (var item in ListaVIew)
                {
                    ManejadorVisibilidadActualizarNombreVista _ManejadorVisibilidadActualizarNombreVista = new ManejadorVisibilidadActualizarNombreVista(_uiapp);
                    _ManejadorVisibilidadActualizarNombreVista.Ejecutar(item.Value, item.Key);
                }



                Util.InfoMsg("Proceso Terminado");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ServicioBuscarViewContendioEnGrid2'. ex:{ex.Message}");
                return false;
            }
            return true;
        }



        private void AnalizarSiNuevoNombreGridExiste(string nombre_Nuevo)
        {
            var gridEncontrado = Lista_EnvoltorioGrid_view.Where(c => c.Grid_?.Grid_.Name == nombre_Nuevo).FirstOrDefault();

            if (gridEncontrado == null) return;

            if (gridEncontrado.IsAnalizado) return;

            gridEncontrado.IsAnalizado = true;

            if (gridEncontrado.Nombre_Nuevo != "" && gridEncontrado.IsCambiarColor)
            { // cambiar view y eje
                AnalizarSiNuevoNombreGridExiste(gridEncontrado.Nombre_Nuevo);
                gridEncontrado.Grid_.Grid_.Name = gridEncontrado.Nombre_Nuevo;// + "-"+grid.Nombre_Nuevo;            
            }
            else
            {
                gridEncontrado.Grid_.Grid_.Name = gridEncontrado.Grid_.Grid_.Name + "Editar"; // encaso de encontra pero no tiene cambios previos

            }

            gridEncontrado.ListaGridAsociados.Where(c => c.IsOK && c.Nombre_Nuevo != "" && c.IsCambiarColor).ForEach(r =>
            {
                AnalizarSiNuevoViewExiste(r.Nombre_Nuevo);
                ListaVIew.Add(r.Nombre_Nuevo, r.viewGeom.viewDTO_.View_);
                r.IsAnalizado = true;
            });

        
        }


        private void AnalizarSiNuevoViewExiste(string nombre_Nuevo)
        {
            var gridEncontrado = Lista_EnvoltorioGrid_view.Find(c => c.ListaGridAsociados.Exists(v => v.IsOK && v.Nombre_Actual == nombre_Nuevo));

            if (gridEncontrado == null) return;

            if (gridEncontrado.IsAnalizado) return;
            gridEncontrado.IsAnalizado = true;

            gridEncontrado.ListaGridAsociados.Where(c => c.IsOK && c.Nombre_Nuevo != "" && c.IsCambiarColor).ForEach(r =>
            {
                if (r.Nombre_Actual == nombre_Nuevo)
                {
                    r.IsAnalizado = true;
                    AnalizarSiNuevoViewExiste(r.Nombre_Nuevo);
                    ListaVIew.Add(r.Nombre_Nuevo, r.viewGeom.viewDTO_.View_);
                   
                }
            });

       
        }
    }



}
