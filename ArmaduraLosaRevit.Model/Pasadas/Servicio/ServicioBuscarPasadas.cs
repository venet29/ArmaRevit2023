using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Pasadas
{




    public class ServicioBuscarPasadas
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<EnvoltorioPipes> ListaPIepes;
        private List<ElementId> _listaElementExiste;

        public ObservableCollection<EnvoltorioBase> ListaMEPObser { get; private set; }

        public ObservableCollection<EnvoltorioDuct> ListaDucstObser { get; private set; }

        public ObservableCollection<EnvoltorioPipes> ListaObserTOtal { get; private set; }
        public ObservableCollection<EnvoltorioCableTray> ListaCableTaryObser { get; private set; }
        public ObservableCollection<EnvoltorioConduit> ListaObsConduit { get; private set; }

        public List<EnvoltorioConduit> ListaConduit;
        public List<EnvoltorioCableTray> ListaCableTary;
        private List<EnvoltorioDuct> ListaDucts;


        public ServicioBuscarPasadas(UIApplication uiapp, List<ElementId> _listaElementExiste)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._listaElementExiste = _listaElementExiste;
            //  ListaLinks= new List<LinkDOcumentosDTO>();
        }



        // itera para buscar los link, pero si hay dos poryectos con link (cado uno) encuentra los dos link
        public bool ObtenerListaDUctosPipen_EnLink(View3D elem3d, LinkDOcumentosDTO LinkSeleccionado)
        {
            try
            {
                if (LinkSeleccionado == null)
                {
                    Util.ErrorMsg($"Link Seleccionado igual a null");
                    return false;
                }
                if (LinkSeleccionado.RevitLinkInstanc == null)
                {
                    Util.ErrorMsg($"Error al obtener   del LinkSeleccionado:  {LinkSeleccionado.Pathname}");
                    return false;
                }

                var tottrans = LinkSeleccionado.RevitLinkInstanc.GetTotalTransform();// _RevitLinkInstanc.GetTotalTransform();
                                                                                     //   tottrans = LinkSeleccionado.RevitLinkInstanc.GetLinkDocument().ActiveProjectLocation.GetTransform();

                var tottrans2 = LinkSeleccionado.RevitLinkInstanc.GetTransform();// _RevitLinkInstanc.GetTotalTransform();
                XYZ _origenRevitLinkInstance = default;


                //obs1) NOTA: 15-02-2023 se encontro en el caso de poryecto en la nube el origen estaba desplazado pero al parecer no era necesario trasladar los puntos, y ese archivo tenia tottrans.IsTranslation=false
                if (tottrans.IsTranslation)
                    _origenRevitLinkInstance = tottrans.Origin;
                else
                    _origenRevitLinkInstance = XYZ.Zero;

                //var poitnInvertida = tottrans.Inverse.OfPoint(tottrans.Origin);
                var poitnInvertida = tottrans.Inverse.OfPoint(tottrans.Origin);
                var poitnNormal = tottrans.OfPoint(tottrans.Origin);
                _origenRevitLinkInstance = tottrans.Origin;
                Document linkedDoc = LinkSeleccionado.documento;


                //************************* level
                SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                List<EnvoltoriLevel> listaLevel = _seleccionarNivel.ObtenerListaEnvoltoriLevelOrdenadoPorElevacion();

                //  List<Task> listaTareas = new List<Task>();
                var listaEnvoltorioGrid = TiposGrid.ObtenerTodosGrid(_doc);

                //NOTA: ingresar id de ducto para filtrar, mantener doble comilla para no iltrar
                string filtrar = "";

                //********* 1)pipes


                FilteredElementCollector collLinked = new FilteredElementCollector(linkedDoc);
                if (_listaElementExiste.Count > 0)
                    ListaPIepes = collLinked.OfClass(typeof(Pipe)).Excluding(_listaElementExiste).Select(c => new EnvoltorioPipes((Pipe)c, tottrans)).ToList();
                else
                    ListaPIepes = collLinked.OfClass(typeof(Pipe)).Select(c => new EnvoltorioPipes((Pipe)c, tottrans)).ToList();

                int cantidadQUedan = ListaPIepes.Count - LinkSeleccionado.Inicio - 1;
                if (cantidadQUedan > LinkSeleccionado.Cantidad && ListaPIepes.Count > 0)
                    ListaPIepes = ListaPIepes.GetRange(LinkSeleccionado.Inicio, LinkSeleccionado.Cantidad);
                else if (ListaPIepes.Count > 0)
                    ListaPIepes = ListaPIepes.GetRange(LinkSeleccionado.Inicio, cantidadQUedan);
                // ListaPIepes = ListaPIepes.Where(c => c._elemento.Id.IntegerValue == 5478103 || c._elemento.Id.IntegerValue == 5478129).ToList();

#if DEBUG
                if (filtrar != "")
                    ListaPIepes = ListaPIepes.Where(c => c._elemento.Id.ToString() == filtrar).ToList();
#endif

                for (int i = 0; i < ListaPIepes.Count; i++)
                {

                    var pipe = ListaPIepes[i];
                    pipe._transform = tottrans;
                    //if (pipe._elemento.Id.IntegerValue != 5467063) continue;
                    pipe.ObtenerDatos(elem3d);
                    pipe.ObtenerLevelYgrilla(listaLevel, listaEnvoltorioGrid);
                    pipe.ObtenerLArgoAncho();
                    pipe.ObtenerGeometria();

                }
                ListaMEPObser = new ObservableCollection<EnvoltorioBase>(ListaPIepes);


                //********* 2)ductos

                FilteredElementCollector collLinked2 = new FilteredElementCollector(linkedDoc);
                if (_listaElementExiste.Count > 0)
                    ListaDucts = collLinked2.OfClass(typeof(Duct)).Excluding(_listaElementExiste).Select(c => new EnvoltorioDuct((Duct)c, tottrans)).ToList();
                else
                    ListaDucts = collLinked2.OfClass(typeof(Duct)).Select(c => new EnvoltorioDuct((Duct)c, tottrans)).ToList();

#if DEBUG
                if (filtrar != "")
                    ListaDucts = ListaDucts.Where(c => c._elemento.Id.ToString() == filtrar).ToList();
#endif

                for (int i = 0; i < ListaDucts.Count; i++)
                {
                    var duct = ListaDucts[i];
                    duct.ObtenerDatos(elem3d);
                    duct.ObtenerLArgoAncho();
                    duct.ObtenerGeometria();
                    ListaMEPObser.Add(duct);

                }


                ListaDucstObser = new ObservableCollection<EnvoltorioDuct>(ListaDucts);

                //********* 3)ductos

                FilteredElementCollector collLinked3 = new FilteredElementCollector(linkedDoc);
                if (_listaElementExiste.Count > 0)
                    ListaCableTary = collLinked3.OfClass(typeof(CableTray)).Excluding(_listaElementExiste).Select(c => new EnvoltorioCableTray((CableTray)c, tottrans)).ToList();
                else
                    ListaCableTary = collLinked3.OfClass(typeof(CableTray)).Select(c => new EnvoltorioCableTray((CableTray)c, tottrans)).ToList();

#if DEBUG
                if (filtrar != "")
                    ListaCableTary = ListaCableTary.Where(c => c._elemento.Id.ToString() == filtrar).ToList();
#endif

                for (int i = 0; i < ListaCableTary.Count; i++)
                { 
                    var duct = ListaCableTary[i];
                    duct.ObtenerDatos(elem3d);
                    duct.ObtenerLArgoAncho();
                    duct.ObtenerGeometria();
                    ListaMEPObser.Add(duct);

                }

                ListaCableTaryObser = new ObservableCollection<EnvoltorioCableTray>(ListaCableTary);


                //***************4)Conduit

                FilteredElementCollector collLinked4 = new FilteredElementCollector(linkedDoc);
                if (_listaElementExiste.Count > 0)
                    ListaConduit = collLinked4.OfClass(typeof(Conduit)).Excluding(_listaElementExiste).Select(c => new EnvoltorioConduit((Conduit)c, tottrans)).ToList();
                else
                    ListaConduit = collLinked4.OfClass(typeof(Conduit)).Select(c => new EnvoltorioConduit((Conduit)c, tottrans)).ToList();

#if DEBUG
                if (filtrar != "")
                    ListaConduit = ListaConduit.Where(c => c._elemento.Id.ToString() == filtrar).ToList();
#endif

                for (int i = 0; i < ListaConduit.Count; i++)
                {
                    var duct = ListaConduit[i];
                    duct.ObtenerDatos(elem3d);
                    duct.ObtenerLArgoAncho();
                    duct.ObtenerGeometria();
                    ListaMEPObser.Add(duct);
                }

                ListaObsConduit = new ObservableCollection<EnvoltorioConduit>(ListaConduit);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error 'ObtenerListaDUctosPipen EnLink'. ex{ex.Message}");
                return false;
            }

            //BORRAR
            //var lsit = ListaMEPObser.ToList();
            //ListaMEPObser = new ObservableCollection<EnvoltorioBase>(lsit.Where(c => c._elemento.Id.IntegerValue == 3551229 || c._elemento.Id.IntegerValue == 3546924));

            return true;
        }


        public bool BuscarInterferencias()
        {
            try
            {
                var _view3D = (View3D)_view;
                for (int i = 0; i < ListaPIepes.Count; i++)
                {
                    var pipe = ListaPIepes[i];

                    var _buscar = new BuscarElementosHorizontal(_uiapp, pipe.LargoPipe, _view3D, true);
                    if (_buscar.BuscarObjetos(pipe.Pto1, pipe.Dire))
                    {
                        if (_buscar.listaObjEncontrados.Count > 0)
                        {
                            pipe.InterseccionPipe_ = InterseccionPipe.SeEncontroInterseccion;
                        }
                        else
                            pipe.InterseccionPipe_ = InterseccionPipe.NoSeEncontroInterseccion;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public class ReferenceComparer : IEqualityComparer<Reference>
        {
            public bool Equals(Reference x, Reference y)
            {
                if (x.ElementId == y.ElementId)
                {
                    if (x.LinkedElementId == y.LinkedElementId)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }

            public int GetHashCode(Reference obj)
            {
                int hashName = obj.ElementId.GetHashCode();
                int hashId = obj.LinkedElementId.GetHashCode();
                return hashId ^ hashId;
            }
        }
    }
}
