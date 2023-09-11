using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.BuscarUbicacion.Modelo;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Servicio
{
    internal class ServicioBuscardoUbicacion : ServicioBuscardorBase
    {
        private UIApplication _uiapp;
        private List<EnvoltoriPasada> listaALl_EnvoltoriPasada;
        private readonly List<EnvoltoriPasada> lista_EnvoltoriPasadasSeleccionados;

        public List<PasadasParaUbicDimensiones> ListaAll_PasadasParaUbicDimensiones { get; set; }
        public List<PasadasParaUbicDimensiones> ListaSelec_PasadasParaUbicDimensiones { get; private set; }
        public bool Isdibujar { get; private set; }

        public ServicioBuscardoUbicacion(UIApplication uiapp, List<EnvoltoriPasada> _listaALl_EnvoltoriPasada, List<EnvoltoriPasada> lista_EnvoltoriPasadasSeleccionados) : base(uiapp)
        {
            this._uiapp = uiapp;
            this.listaALl_EnvoltoriPasada = _listaALl_EnvoltoriPasada;
            this.lista_EnvoltoriPasadasSeleccionados = lista_EnvoltoriPasadasSeleccionados;
            // ListaDePasadasCercanas = new List<EnvoltoriPasada>();
            this.ListaAll_PasadasParaUbicDimensiones = new List<PasadasParaUbicDimensiones>();
            this.ListaSelec_PasadasParaUbicDimensiones = new List<PasadasParaUbicDimensiones>();
            Isdibujar = false;
        }

        //  public List<EnvoltoriPasada> ListaDePasadasCercanas { get; private set; }

        public bool M0_AnalizarPasadas()
        {
            try
            {
                //a) analizar todas las pasadas
                for (int i = 0; i < listaALl_EnvoltoriPasada.Count; i++)
                {
                    PasadasParaUbicDimensiones _PasadasParaUbicDimensiones = new PasadasParaUbicDimensiones(_uiapp, listaALl_EnvoltoriPasada[i]);
                    if (_PasadasParaUbicDimensiones.ObtenerDatos())
                        ListaAll_PasadasParaUbicDimensiones.Add(_PasadasParaUbicDimensiones);
                }


                //b) buscar pasadad ya ananisadas dentro de lista de todas las pasadas analizadas
                for (int i = 0; i < lista_EnvoltoriPasadasSeleccionados.Count; i++)
                {
                    var item = lista_EnvoltoriPasadasSeleccionados[i];
                    var result = ListaAll_PasadasParaUbicDimensiones.Where(c => c.EnvoltoriPasada_._pasada.Id == item._pasada.Id).FirstOrDefault();
                    ListaSelec_PasadasParaUbicDimensiones.Add(result);
                }


                //  asigna propiedes   'TipoInterseccion.IntersectaConPAsada' a  'DimensionParaUbicDimensiones'
                for (int i = 0; i < ListaSelec_PasadasParaUbicDimensiones.Count; i++)
                {
                    var pasada = ListaSelec_PasadasParaUbicDimensiones[i];


                    List<PasadasParaUbicDimensiones> ListaPAsadasCercanas = ListaSelec_PasadasParaUbicDimensiones
                        .Where(c => pasada.EnvoltoriPasada_._pasada.Id != c.EnvoltoriPasada_._pasada.Id &&
                                   c.EnvoltoriPasada_.PtoCentralCaraSuperior.DistanceTo(pasada.EnvoltoriPasada_.PtoCentralCaraSuperior) < (c.RadioBusqueda + pasada.RadioBusqueda + Util.CmToFoot(100)))
                        .OrderBy(c => c.RadioBusqueda + pasada.RadioBusqueda + Util.CmToFoot(100)).ToList();


                    for (int j = 0; j < ListaPAsadasCercanas.Count; j++)
                    {
                        var pasadaIterada = ListaPAsadasCercanas[j];

                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaIzq.Dimension_LadoIzqInf);
                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaIzq.Dimension_LadoDereSup);

                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaDere.Dimension_LadoIzqInf);
                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaDere.Dimension_LadoDereSup);

                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaArriba.Dimension_LadoIzqInf);
                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaArriba.Dimension_LadoDereSup);

                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaBajo.Dimension_LadoIzqInf);
                        BuscarInterseccionRegionDImension(pasada, pasadaIterada.CaraPasadaBajo.Dimension_LadoDereSup);

                    }
                }


                ServicioBuscardorVertical _ServicioBuscardorVertical = new ServicioBuscardorVertical(_uiapp);
                ServicioBuscardorHorizontal _ServicioBuscardorHorizontal = new ServicioBuscardorHorizontal(_uiapp);
                //c)buscar ubicacion
                for (int i = 0; i < ListaSelec_PasadasParaUbicDimensiones.Count; i++)
                {

                    var itemSelect = ListaSelec_PasadasParaUbicDimensiones[i];
                    itemSelect.ConfigInicial();

                    List<PasadasParaUbicDimensiones> ListaPAsadasCercanas =  ListaAll_PasadasParaUbicDimensiones//ListaSelec_PasadasParaUbicDimensiones
                        .Where(c => itemSelect.EnvoltoriPasada_._pasada.Id != c.EnvoltoriPasada_._pasada.Id &&
                                   c.EnvoltoriPasada_.PtoCentralCaraSuperior.DistanceTo(itemSelect.EnvoltoriPasada_.PtoCentralCaraSuperior) < (c.RadioBusqueda + itemSelect.RadioBusqueda + Util.CmToFoot(100)))
                        .OrderBy(c => c.RadioBusqueda + itemSelect.RadioBusqueda + Util.CmToFoot(100)).ToList();

                    // NOTAconfigInicial, sepodria busca el mas corto



                    string casoIniciar = "izq";
                    bool recalcular = true;
                    while (recalcular)
                    {

                        if (casoIniciar == "izq")
                        {
                            // verticales
                            _ServicioBuscardorVertical.Resetear();
                            _ServicioBuscardorVertical.M1_Buscar_comenzarIzq(itemSelect, ListaPAsadasCercanas);


                            // horizontal
                            _ServicioBuscardorHorizontal.Resetear();

                            if (itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion == EnumPasadasConGrilla.Izquieda_inf ||
                                itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion == EnumPasadasConGrilla.Derecha_inf)
                                _ServicioBuscardorHorizontal.Buscar_comenzarArriba(itemSelect, ListaPAsadasCercanas);
                            else
                                _ServicioBuscardorHorizontal.Buscar_comenzarBAJO(itemSelect, ListaPAsadasCercanas);

                            //_ServicioBuscardorHorizontal.Buscar_comenzarArriba(itemSelect, ListaPAsadasCercanas);
                            recalcular = _ServicioBuscardorHorizontal.IsSeguir;
                        }
                        else
                        {
                            // verticales
                            _ServicioBuscardorVertical.Resetear();
                            _ServicioBuscardorVertical.Buscar_comenzarDere(itemSelect, ListaPAsadasCercanas);


                            // horizontal
                            _ServicioBuscardorHorizontal.Resetear();
                            if (itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion == EnumPasadasConGrilla.Izquieda_inf ||
                                itemSelect.EnvoltoriPasada_.DimesionVerticalDireccion == EnumPasadasConGrilla.Derecha_inf)
                                _ServicioBuscardorHorizontal.Buscar_comenzarArriba(itemSelect, ListaPAsadasCercanas);
                            else
                                _ServicioBuscardorHorizontal.Buscar_comenzarBAJO(itemSelect, ListaPAsadasCercanas);
                            recalcular = false;
                        }

                        casoIniciar = "dere";

                    }
                }

                if (Isdibujar)
                {
                    //dibujar
                    using (Transaction t = new Transaction(_uiapp.ActiveUIDocument.Document))
                    {

                        t.Start($"CambioAct-NH");
                        for (int i = 0; i < ListaSelec_PasadasParaUbicDimensiones.Count; i++)
                        {
                            var item = ListaSelec_PasadasParaUbicDimensiones[i];
                            item.DibujarRectagulo();
                        }
                        t.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'BuscardorPasadasCercanas'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private static void BuscarInterseccionRegionDImension(PasadasParaUbicDimensiones pasada, DimensionParaUbicDimensiones _dimensiones)
        {
            //if (IsInterseccion.IsInterseccionPoligonos_XY0(pasada.EnvoltoriPasada_.ListaPtosFaceCaraNormalSup, pasadaIterada.CaraPasadaIzq.Dimension_LadoIzqInf.ListaPuntos))
            //    pasadaIterada.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;

            //if (IsDentroPoligono.IsPointInsidePolyline(pasada.EnvoltoriPasada_.PtoCentralCaraSuperior, pasadaIterada.CaraPasadaIzq.Dimension_LadoIzqInf.ListaPuntos))
            //    pasadaIterada.CaraPasadaIzq.Dimension_LadoIzqInf.EstadoIteracion_ = EstadoIteracion.NoIterrar;


            if (IsInterseccion.IsInterseccionPoligonos_XY0(pasada.EnvoltoriPasada_.ListaPtosFaceCaraNormalSup, _dimensiones.ListaPuntos))
                _dimensiones.EstadoIteracion_ = EstadoIteracion.NoIterrar;

            if (IsDentroPoligono.IsPointInsidePolyline(pasada.EnvoltoriPasada_.PtoCentralCaraSuperior, _dimensiones.ListaPuntos))
                _dimensiones.EstadoIteracion_ = EstadoIteracion.NoIterrar;
        }



    }
}