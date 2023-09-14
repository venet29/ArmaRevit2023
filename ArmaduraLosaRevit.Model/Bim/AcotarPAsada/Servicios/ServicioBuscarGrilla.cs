using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.GRIDS.model;
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
using static System.Windows.Forms.LinkLabel;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Servicios
{




    public class ServicioBuscarGrilla
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<EnvoltorioGridPasada> ListaGridTotal;
        private List<ElementId> _listaElementExiste;

        public List<EnvoltorioMuro> ListaMurosPorVIew { get; private set; }

        public List<EnvoltorioConduit> ListaConduit;
        public List<EnvoltorioCableTray> ListaCableTary;


        public ServicioBuscarGrilla(UIApplication uiapp, List<ElementId> _listaElementExiste)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._listaElementExiste = _listaElementExiste;
            //  ListaLinks= new List<LinkDOcumentosDTO>();
        }



        // itera para buscar los link, pero si hay dos poryectos con link (cado uno) encuentra los dos link
        public bool ObtenerListaGrillas_EnLink(LinkDOcumentosDTO LinkSeleccionado)
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
                var poitnInvertida=tottrans.Inverse.OfPoint(tottrans.Origin);
                var poitnNormal = tottrans.OfPoint(tottrans.Origin);
                _origenRevitLinkInstance = tottrans.Origin;
                Document linkedDoc = LinkSeleccionado.documento;


                //************************* level
                SeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);
                List<EnvoltoriLevel> listaLevel = _seleccionarNivel.ObtenerListaEnvoltoriLevelOrdenadoPorElevacion();

                var listaEnvoltorioGrid = TiposGrid.ObtenerTodosGrid(_doc);
                //********* 1)pipes
                FilteredElementCollector collLinked = new FilteredElementCollector(linkedDoc);
                if(_listaElementExiste.Count>0)
                    ListaGridTotal = collLinked.OfClass(typeof(Grid)).Excluding(_listaElementExiste).Select(c => new EnvoltorioGridPasada(_uiapp, c, tottrans)).ToList();
                else
                    ListaGridTotal = collLinked.OfClass(typeof(Grid)).Select(c => new EnvoltorioGridPasada(_uiapp,c, tottrans)).ToList();

                LinkSeleccionado.Inicio = 0;
                LinkSeleccionado.Cantidad = ListaGridTotal.Count-1;

                int cantidadQUedan=ListaGridTotal.Count - LinkSeleccionado.Inicio - 1;
                if(cantidadQUedan > LinkSeleccionado.Cantidad && ListaGridTotal.Count>0)
                    ListaGridTotal= ListaGridTotal.GetRange(LinkSeleccionado.Inicio, LinkSeleccionado.Cantidad);
                else if(ListaGridTotal.Count > 0)
                    ListaGridTotal = ListaGridTotal.GetRange(LinkSeleccionado.Inicio, cantidadQUedan);
              
                // ListaPIepes = ListaPIepes.Where(c => c._elemento.Id.IntegerValue == 5478103 || c._elemento.Id.IntegerValue == 5478129).ToList();

                //for (int i = 0; i < ListaGridTotal.Count; i++)
                //{
                //    var Muro = ListaGridTotal[i];
                  
                //    //if (pipe._elemento.Id.IntegerValue != 5467063) continue;
                //    Muro.ObtenerDatos();
 
                //}
         
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

        //**********************************************************************
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
