using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.GRIDS.model;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.GRIDS.AgregarEje.Servicios
{
    class ServicioObtenerGridDeVIew
    {
        private UIApplication _uiapp;
        private Document _doc;
        private List<EnvoltorioGrid> listaEnvoltorioGrid;

        private List<GridIntersectado> listaEnvoltorioGrid_existenEnView;
        private List<GridIntersectado> listaEnvoltorioGrid_NOexistenEnView;

        public ServicioObtenerGridDeVIew(UIApplication uiapp, List<EnvoltorioGrid> listaEnvoltorioGrid)
        {
            _uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            this.listaEnvoltorioGrid = listaEnvoltorioGrid;
            listaEnvoltorioGrid_existenEnView = new List<GridIntersectado>();
            listaEnvoltorioGrid_NOexistenEnView = new List<GridIntersectado>();
        }

        internal bool Buscar(View _view, string nombreGridPrincipal)
        {
            try
            {
                ServicioObtenerInterseccionEntreGrid _SOIEG_viewActual = new ServicioObtenerInterseccionEntreGrid(_uiapp);
                if (!_SOIEG_viewActual.M2_Ejecutar(_view)) return false;
                var ListaGridEnVIew = _SOIEG_viewActual.ListaEnvoltorioGrid;


                //var gridCOntodoLosVIewIntersectaods = listaEnvoltorioGrid.Where(c => c.Nombre == _view.Name.Replace(nombreGridPrincipal, "").Trim()).FirstOrDefault();
                var gridCOntodoLosVIewIntersectaods = listaEnvoltorioGrid.Where(c => c.Nombre.ToLower() == nombreGridPrincipal.ToLower()).FirstOrDefault();
                if (gridCOntodoLosVIewIntersectaods == null) return false;

                for (int i = 0; i < gridCOntodoLosVIewIntersectaods.ListaGridIntersectado.Count; i++)
                {
                    var gridAnalisado = gridCOntodoLosVIewIntersectaods.ListaGridIntersectado[i];

                    var gridEnconradoENview = ListaGridEnVIew.Where(c => c.Nombre == gridAnalisado.Grid.Nombre).FirstOrDefault();

                    if (gridEnconradoENview == null)
                        listaEnvoltorioGrid_NOexistenEnView.Add(gridAnalisado);
                    else
                        listaEnvoltorioGrid_existenEnView.Add(gridAnalisado);

                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en Interseccion Entre Grid.\n Ex:{ex.Message}");
                return false;
            }
            return true;
        }

        internal bool CrearLineas(View _view)
        {
            try
            {
                // FALTA PRECREAR LIENA TIPO EJE
                // FALTA PRECREAR TEXTO TIPO eJE
                // FALTA AGRUPAR
                // FALTA QUE TODOS SE CREEE EN UN GRUPO DE TRANSACCIONES


                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("DibujarLargosMin-NH");


                    SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp, true);
                    _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();

                    var lista = CrearListaPtos.M2_ListaPtoSimple(_uiapp, 1);
                    if (lista.Count == 0) return false;

                    XYZ ptoMASALto = lista[0];

                   

                    foreach (var item in listaEnvoltorioGrid_NOexistenEnView)
                    {
                        double ZmasAlto = (ptoMASALto.Z > item.Grid.MinimumOint.Z ? ptoMASALto.Z : item.Grid.MaximumOint.Z);

                        var ptInt = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(_view.ViewDirection, _view.Origin, item.PtoIntersectado);
                        XYZ P1 = new XYZ(ptInt.X, ptInt.Y, item.Grid.MinimumOint.Z);
                        XYZ P2 = new XYZ(ptInt.X, ptInt.Y, ZmasAlto  );

                        var lineaCreada = CrearDetailLineAyuda.modelarlineas_ConTransaccion(_doc, _view, P1, P2, "LINEA DE EJES");
                        if (lineaCreada == null) continue;

                        CrearTexNote crearTexNote = new CrearTexNote(_uiapp, "EJE");
                        var textCreado = crearTexNote.M1_CrearConTrans(P2 + new XYZ(0, 0, 1 - Util.CmToFoot(8)), item.Grid.Nombre, 0);
                        if (textCreado == null) continue;

                        var circ = CreadorCirculo.CrearCirculo_DetailLine_ConTrans(Util.CmToFoot(45), P2 + new XYZ(0, 0, 1- Util.CmToFoot(8)), _uiapp.ActiveUIDocument, _view.RightDirection, XYZ.BasisZ, "MAGENTA");
                        if (circ == null) continue;

                        List<ElementId> listaCreados = new List<ElementId>();
                        listaCreados.Add(lineaCreada.Id);
                        listaCreados.Add(textCreado.Id);
                        listaCreados.Add(circ.Id);

                        using (Transaction tx = new Transaction(_doc))
                        {
                            tx.Start("Creando grupoGrid-NH");

                            if (listaCreados.Count > 1)
                                _doc.Create.NewGroup(listaCreados);

                            //  if(listaGrupo_DimensionCirculo.Count>1)
                            //_doc.Create.NewGroup(listaGrupo_DimensionCirculo); 
                            //var grouptag = _doc.Create.NewGroup(listaGrupo_Tag);
                            tx.Commit();
                        }
                    }

                    t.Assimilate();
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
}
