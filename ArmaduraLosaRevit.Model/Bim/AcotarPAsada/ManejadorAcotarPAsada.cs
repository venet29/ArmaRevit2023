using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Ayuda;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Servicio;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Servicios;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.GRIDS.model;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada
{
    public class ManejadorAcotarPAsada
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        public List<LinkDOcumentosDTO> _listaRevitLink { get; set; }

        public List<EnvoltoriPasada> Lista_EnvoltoriPasadasSeleccionados { get; set; }

        private string _nombreTipo;
        private DimensionType _dimensionType;
        private List<EnvoltorioGrid> ListaEnvoltorioGrid;

        private List<EnvoltoriPasada> _ListaALl_EnvoltoriPasada; // genera todas las pasadas de la vista

        public List<Element> ListaAllPasadas { get; private set; }

        public ManejadorAcotarPAsada(UIApplication uiapp)//para atuomatico
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.Lista_EnvoltoriPasadasSeleccionados = new List<EnvoltoriPasada>();
            this._ListaALl_EnvoltoriPasada = new List<EnvoltoriPasada>();
            this._nombreTipo = ConstBim.CONST_NOMBRE_COTA;
            ListaEnvoltorioGrid= new List<EnvoltorioGrid>();
        }


        //NOTA : ESTE METODO SE DEBE DESPLAZAR AL CARGAR  WPF PARA EVIATAR CALCULAR CADA VEZ
        private bool M1_ObtenerListaAllPasadaENview()
        {
            if (ListaEnvoltorioGrid.Count == 0)
                ListaEnvoltorioGrid = TiposGrid.ObtenerTodosGrid(_doc, _view?.Id);

            _ListaALl_EnvoltoriPasada.Clear();

            ListaAllPasadas = SeleccionarOpening.SeleccionarAll_pasadas_Contenga(_doc, _view, "PASADA_").ToList();

            // 3)procesar las pasadas
            foreach (var _pasada in ListaAllPasadas)
            {
                EnvoltoriPasada _EnvoltoriPasada = new EnvoltoriPasada(_uiapp, _pasada);
                if (_EnvoltoriPasada.ObtenerInfo_PLanos())
                {
                    _ListaALl_EnvoltoriPasada.Add(_EnvoltoriPasada);
                }
            }
            if (_ListaALl_EnvoltoriPasada.Count == 0) return false;



            if (!AyudaBuscarInterseccionPlanosPasadasConGrid
                            .Ejecutar(_uiapp, _ListaALl_EnvoltoriPasada, ListaEnvoltorioGrid)) return false;

            return true;
        }

        public bool M3_EjecutarAcotarPasadasDeUNo_selecconadaMouse_2doPtoAutomatico(EnumPasadasConGrilla dimesionHorizonal, EnumPasadasConGrilla dimesionVertical)
        {
            if (!CAlculosIniciales()) return false;
    
            while (true)
            {
                try
                {
                    if (!M3_1_ObtenerListaPasadaConInterseccionesSelecMOuse()) return false;

                    // 5)acortar pasada
                    ServicionACotarPAsada _ServiicionACotarPAsada = new ServicionACotarPAsada(_uiapp, EnumPasadas.Izquieda, _dimensionType);

                    foreach (var EnvoltoriPasada in Lista_EnvoltoriPasadasSeleccionados)
                    {
                        if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Rectangular)
                            _ServiicionACotarPAsada.AcotarRectangularYGrilla(EnvoltoriPasada, dimesionVertical);
                        else if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Circular)
                            _ServiicionACotarPAsada.AcotarCircular(EnvoltoriPasada);
                    }


                    ServicionACotarPAsada _ServiicionACotarPAsadav2 = new ServicionACotarPAsada(_uiapp, EnumPasadas.Arriba, _dimensionType);

                    foreach (var EnvoltoriPasada in Lista_EnvoltoriPasadasSeleccionados)
                    {
                        if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Rectangular)
                            _ServiicionACotarPAsadav2.AcotarRectangularYGrilla(EnvoltoriPasada, dimesionHorizonal);
                        else if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Circular)
                            _ServiicionACotarPAsadav2.AcotarCircular(EnvoltoriPasada);
                    }


                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al ejecutar acotar pasadas. ex:{ex.Message}");
                    return false;
                }

            }

        }




        private bool M3_1_ObtenerListaPasadaConInterseccionesSelecMOuse()
        {
            Lista_EnvoltoriPasadasSeleccionados.Clear();


            //1)seleccionar pasadas
            //-- crear obtejeo
            SeleccionarPasadasConMouse _SeleccionarPasadasConMouse = new SeleccionarPasadasConMouse(_uiapp);
            if (!_SeleccionarPasadasConMouse.M1_SeleccionarPAsadas()) return false;


            // 3)procesar las pasadas
            foreach (var _pasada in _SeleccionarPasadasConMouse.ListaPAsadas)
            {
                EnvoltoriPasada _EnvoltoriPasada = new EnvoltoriPasada(_uiapp, _pasada);
                if (_EnvoltoriPasada.ObtenerInfo_PLanos())
                {
                    Lista_EnvoltoriPasadasSeleccionados.Add(_EnvoltoriPasada);
                }
            }
            if (Lista_EnvoltoriPasadasSeleccionados.Count == 0) return false;

            if (!AyudaBuscarInterseccionPlanosPasadasConGrid
                            .Ejecutar(_uiapp, Lista_EnvoltoriPasadasSeleccionados, ListaEnvoltorioGrid)) return false;

            return true;
        }

        public bool M4_EjecutarAcotarPasadasD_conRectagulo_2doPtoAutomatico_segunUsuario(EnumPasadasConGrilla dimesionHorizonal, EnumPasadasConGrilla dimesionVertical)
        {


            if (!CAlculosIniciales()) return false;

            while (true)
            {
                try
                {
                    if (!M4_1_ObtenerListaPasadaConInterseccionesRectangulo()) return false;

                    // 5)acortar pasada
                    ServicionACotarPAsada _ServiicionACotarPAsada = new ServicionACotarPAsada(_uiapp, EnumPasadas.Izquieda, _dimensionType);

                    foreach (var EnvoltoriPasada in Lista_EnvoltoriPasadasSeleccionados)
                    {
                        if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Rectangular)
                            _ServiicionACotarPAsada.AcotarRectangularYGrilla(EnvoltoriPasada, dimesionVertical);
                        else if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Circular)
                            _ServiicionACotarPAsada.AcotarCircular(EnvoltoriPasada);
                    }



                    ServicionACotarPAsada _ServiicionACotarPAsadav2 = new ServicionACotarPAsada(_uiapp, EnumPasadas.Arriba, _dimensionType);

                    foreach (var EnvoltoriPasada in Lista_EnvoltoriPasadasSeleccionados)
                    {
                        if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Rectangular)
                            _ServiicionACotarPAsadav2.AcotarRectangularYGrilla(EnvoltoriPasada, dimesionHorizonal);
                        else if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Circular)
                            _ServiicionACotarPAsadav2.AcotarCircular(EnvoltoriPasada);
                    }


                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al ejecutar acotar pasadas. ex:{ex.Message}");
                    return false;
                }

            }
            return true;
        }

        private bool M4_1_ObtenerListaPasadaConInterseccionesRectangulo()
        {
            try
            {
                if(ListaEnvoltorioGrid.Count==0)
                    ListaEnvoltorioGrid = TiposGrid.ObtenerTodosGrid(_doc, _view?.Id);

                Lista_EnvoltoriPasadasSeleccionados.Clear();

                //1)seleccionar pasadas
                //-- crear obtejeo
                SeleccionarPasadasConMouse _SeleccionarPasadasConMouse = new SeleccionarPasadasConMouse(_uiapp);
                if (!_SeleccionarPasadasConMouse.M2_SelecconaShaftOpeningRectagulo()) return false;


                // 3)procesar las pasadas
                foreach (var _pasada in _SeleccionarPasadasConMouse.ListaPAsadas)
                {
                    EnvoltoriPasada _EnvoltoriPasada = new EnvoltoriPasada(_uiapp, _pasada);
                    if (_EnvoltoriPasada.ObtenerInfo_PLanos())
                    {
                        Lista_EnvoltoriPasadasSeleccionados.Add(_EnvoltoriPasada);
                    }
                }
                if (Lista_EnvoltoriPasadasSeleccionados.Count == 0) return false;

                if (!AyudaBuscarInterseccionPlanosPasadasConGrid
                                .Ejecutar(_uiapp, Lista_EnvoltoriPasadasSeleccionados, ListaEnvoltorioGrid)) return false;

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool M5_EjecutarAcotarPasadasD_conRectagulo_2doPtoAutomatico()
        {
            if (!CAlculosIniciales()) return false;

            if (!M1_ObtenerListaAllPasadaENview()) return false;

            while (true)
            {
                try
                {
                    if (!M4_1_ObtenerListaPasadaConInterseccionesRectangulo()) return false;
                    // 5)acortar pasada           
                    //ServicioBuscardorPasadasCercanas _BuscardorPasadasCercanas = new ServicioBuscardorPasadasCercanas(_uiapp, _ListaALl_EnvoltoriPasada, largoDebusquedaPAraInterseccionCOnOtrasPasadas_foot);

                    ServicioBuscardoUbicacion _ServicioBuscardoUbicacion = new ServicioBuscardoUbicacion(_uiapp, _ListaALl_EnvoltoriPasada, Lista_EnvoltoriPasadasSeleccionados);
                    _ServicioBuscardoUbicacion.M0_AnalizarPasadas();
              
                  //  return true;

                    ServicionACotarPAsada _ServiicionACotarPAsada = new ServicionACotarPAsada(_uiapp, EnumPasadas.Izquieda, _dimensionType);
                    foreach (var item in _ServicioBuscardoUbicacion.ListaSelec_PasadasParaUbicDimensiones)
                    {
                        var EnvoltoriPasada = item.EnvoltoriPasada_;
                        EnumPasadasConGrilla dimesionVertical = EnvoltoriPasada.DimesionHorizonalDireccion;// EnumPasadasConGrilla.Izquieda_sup;

                        if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Rectangular)
                            _ServiicionACotarPAsada.AcotarRectangularYGrilla(EnvoltoriPasada, dimesionVertical);
                        else if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Circular)
                            _ServiicionACotarPAsada.AcotarCircular(EnvoltoriPasada);
                    }

                    ServicionACotarPAsada _ServiicionACotarPAsadav2 = new ServicionACotarPAsada(_uiapp, EnumPasadas.Arriba, _dimensionType);

                    foreach (var item in _ServicioBuscardoUbicacion.ListaSelec_PasadasParaUbicDimensiones)
                    {
                        var EnvoltoriPasada = item.EnvoltoriPasada_;

                        EnumPasadasConGrilla dimesionHorizonal = EnvoltoriPasada.DimesionVerticalDireccion;// EnumPasadasConGrilla.Arriba_dere;

                        if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Rectangular)
                            _ServiicionACotarPAsadav2.AcotarRectangularYGrilla(EnvoltoriPasada, dimesionHorizonal);
                        else if (EnvoltoriPasada.parBordeParalelo1.TipoPasada == EnumTipoPAsada.Circular)
                            _ServiicionACotarPAsadav2.AcotarCircular(EnvoltoriPasada);
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"Error al ejecutar acotar pasadas. ex:{ex.Message}");
                    return false;
                }
            }
        }

        public bool M6_BorrarDimensiones()
        {
            try
            {
                if (!SeleccionarDimensiones.SeleccionarMouseDimension(_uiapp.ActiveUIDocument, _nombreTipo)) return false;


                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Borrar Familias Armadura-NH");
                    _doc.Delete(SeleccionarDimensiones.ListaDimensiones.Select(c => c.Id).ToList());
                    trans.Commit();
                }
            }

            catch (Exception ex)

            {
                Util.ErrorMsg($"Error en ObtenerLista");

                return false;
            }
            return true;
        }

        private bool CAlculosIniciales()
        {
            try
            {
                this._dimensionType = SeleccionarDimensiones.ObtenerDimensionTypePorNombre(_doc, _nombreTipo);
                if (this._dimensionType == null)
                {
                    Util.ErrorMsg("Familia de Dimension No encontrada");
                    return false;
                }

                _listaRevitLink = Tipos_LinkDOcumento.ObtenerLinkDocumentoActual(_doc).ToList();
                if (_listaRevitLink.Count == 0)
                {
                    Util.InfoMsg($"Proyecto sin link de especialidades ");
                    return false;
                }

                //NOTA :   'ListaEnvoltorioGrid' SE DEBE DESPLAZAR AL CARGAR  WPF PARA EVIATAR CALCULAR CADA VEZ
                ListaEnvoltorioGrid = TiposGrid.ObtenerTodosGrid(_doc, _view?.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message} ");
                return false;
            }
            return true;
        }
    }
}
