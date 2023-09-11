using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.GRIDS.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.GRIDS.AgregarEje.Servicios
{
    public class ServicioObtenerInterseccionEntreGrid
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ServicioObtenerInterseccionEntreGrid()
        {

        }

        public ServicioObtenerInterseccionEntreGrid(UIApplication uiapp)
        {
            _uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
        }

        public List<EnvoltorioGrid> ListaEnvoltorioGrid { get; private set; }

        public bool M1_CargarGris(View _view=null)
        {
            try
            {
                ListaEnvoltorioGrid = TiposGrid.ObtenerTodosGrid(_doc, _view?.Id);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en Obtener ListaEnvoltorioGrid.\n Ex:{ex.Message}");

                return false;
            }
            return true;
        }

        public bool M2_Ejecutar(View _view=null)
        {
            try
            {
                if (!M1_CargarGris(_view)) return false;

                for (int i = 0; i < ListaEnvoltorioGrid.Count; i++)
                {
                    var gridActual = ListaEnvoltorioGrid[i];

                    for (int j = 0; j < ListaEnvoltorioGrid.Count; j++)
                    {
                        var gridAnalisar = ListaEnvoltorioGrid[j];
                        if (gridAnalisar.Nombre == gridActual.Nombre) continue;

                        XYZ PtoInterseccion = Util.Intersection2(gridActual.Curva, gridAnalisar.Curva);

                        if (PtoInterseccion.IsAlmostEqualTo(XYZ.Zero)) continue;

                        var GridInter = new GridIntersectado() { Grid = gridAnalisar, PtoIntersectado = PtoInterseccion };

                        gridActual.ListaGridIntersectado.Add(GridInter);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en Interseccion Entre Grid.\n Ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool M3_BuscarGridEnEjes(View _view = null)
        {
            try
            {
                if (!M2_Ejecutar(_view)) return false;

                for (int i = 0; i < ListaEnvoltorioGrid.Count; i++)
                {
                    var gridActual = ListaEnvoltorioGrid[i];

                    for (int j = 0; j < ListaEnvoltorioGrid.Count; j++)
                    {
                        var gridAnalisar = ListaEnvoltorioGrid[j];
                        if (gridAnalisar.Nombre == gridActual.Nombre) continue;

                        XYZ PtoInterseccion = Util.Intersection2(gridActual.Curva, gridAnalisar.Curva);

                        if (PtoInterseccion.IsAlmostEqualTo(XYZ.Zero)) continue;

                        var GridInter = new GridIntersectado() { Grid = gridAnalisar, PtoIntersectado = PtoInterseccion };

                        gridActual.ListaGridIntersectado.Add(GridInter);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en Interseccion Entre Grid.\n Ex:{ex.Message}");
                return false;
            }
            return true;
        }




    }






    public class GridIntersectado
    {
        public XYZ PtoIntersectado { get; set; }
        public EnvoltorioGrid Grid { get; set; }

    }
}
