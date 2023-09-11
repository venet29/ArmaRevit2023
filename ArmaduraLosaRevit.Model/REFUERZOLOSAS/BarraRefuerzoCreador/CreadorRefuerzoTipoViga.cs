using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.AnalisisRoom;

using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Ocultar;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoCreador
{

    /// <summary>
    /// objeto que se utiliza cuando los borde de room  'BoundarySegmen' busca elemnto conttiguo  o vecino
    /// si encuntra una viga se crea objecto 'BarraViga' que contiene los objetos
    /// -  BoundarySegmentNH del borde
    /// -  la viga encontrada como vecino 'FamilyInstance'
    /// </summary>BoundarySegmen

    public class CreadorRefuerzoTipoViga
    {
        private readonly UIApplication _uiapp;

        #region 0)propiedades
        private ICalculosRefuerzo _calculosRefuerzoTipoViga;
        private IGeometriaTag _iGeometriaTagRefuerzo;
        private IGeometriaTag _iGeometriaTagEstribo;
        private Floor _floor;
        private Document _doc;
        private List<BarraRefuerzoBordeLibre> listaDibujadosRefuerzo;
        private List<BarraRefuerzoEstribo> listaDibujadosEstribo;
        private int numeroBarrasBarraRef;

        public TipoRebar _casoTipoRebar { get; private set; }
        public TipoRebar _casoTipoEstriboRebar { get; private set; }
        #endregion

        #region 1) Constructores
        public CreadorRefuerzoTipoViga(UIApplication uiapp,
            ICalculosRefuerzo calculosRefuerzoTipoViga,
            IGeometriaTag IGeometriaTagRefuerzo,
            IGeometriaTag IGeometriaTagEstribo, Floor floor)
        {
            this._uiapp = uiapp;
            this._calculosRefuerzoTipoViga = calculosRefuerzoTipoViga;
            this._iGeometriaTagRefuerzo = IGeometriaTagRefuerzo;
            this._iGeometriaTagEstribo = IGeometriaTagEstribo;
            this._floor = floor;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.listaDibujadosRefuerzo = new List<BarraRefuerzoBordeLibre>();
            this.listaDibujadosEstribo = new List<BarraRefuerzoEstribo>();
            numeroBarrasBarraRef = 2;
        }

        public bool Ejecutar(TipoRebar _casoTipoRebar)
        {
            try
            { 
                this._casoTipoRebar = _casoTipoRebar;
                this._casoTipoEstriboRebar = (_casoTipoRebar == TipoRebar.REFUERZO_BA_REF_LO
                                                             ? TipoRebar.REFUERZO_EST_REF_LO
                                                             : TipoRebar.REFUERZO_EST_BORDE);
                //DibujarEstribo();
                if (!M1_DibujarBarra_ConTrans()) return false;
                //CambiarEspaEstribo_ConTrans();
                if (!M2_CambiarColor_ConTrans()) return false;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }
        private bool M1_DibujarBarra_ConTrans()
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("DibujarBarra-NH");
                    M1_1_DibujarEstribo_sintras();
                    M1_2_DibujarBarra_sinTrans();
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void M1_1_DibujarEstribo_sintras()
        {
           // CambiarColorBarras_Service _cambiarColorBarras_Service = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.Estribo);
           // List<ElementId> _listaRebarIdCambiarColor = new List<ElementId>();
            bool isUNo = false;
            foreach (EstriboRefuerzoDTO er in _calculosRefuerzoTipoViga.ListaEstriboRefuerzoDTO)
            {
                // dibujar estribo               
                BarraRefuerzoEstribo _estribo = new BarraRefuerzoEstribo(_uiapp, TipoBorde.shaft, _floor, er, _iGeometriaTagEstribo, _casoTipoEstriboRebar);
                //  _estribo.generarBarra_Sintrans();
                if (!_estribo.generarBarra_Sintrans()) continue;

                if (isUNo)
                {
                    isUNo = false;
                    var _configuracionTAgEstriboDTo = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                        IsDIrectriz = false,
                        tagOrientation = TagOrientation.Horizontal,
                    };
                    //_estribo.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo);
                    _estribo.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo);
                }
                if (_estribo._rebar != null)
                    listaDibujadosEstribo.Add(_estribo);
            }
        }
        private void M1_2_DibujarBarra_sinTrans()
        {


            if (_calculosRefuerzoTipoViga.ListaBArrasSuperior.Count == 0 && _calculosRefuerzoTipoViga.ListaBArrasInferior.Count == 1)
            {//solo para casoso de se dibuja un solo refuerzo 
                _calculosRefuerzoTipoViga.ListaBArrasSuperior.Add(_calculosRefuerzoTipoViga.ListaBArrasInferior[0]);
                _calculosRefuerzoTipoViga.ListaBArrasInferior.Clear();
            }


            if (_calculosRefuerzoTipoViga._datosRefuerzoTipoViga?.IsBArras==false)
            {
                _calculosRefuerzoTipoViga.ListaBArrasSuperior.Clear();
                _calculosRefuerzoTipoViga.ListaBArrasInferior.Clear();
            }

            int count = 1;
            int ultimoBarraSup = _calculosRefuerzoTipoViga.ListaBArrasSuperior.Count;
            foreach (CalculoBarraRefuerzo br in _calculosRefuerzoTipoViga.ListaBArrasSuperior)
            {
                DtoCrearBarraRefuerzoBordeLibre _newDToBRBL = new DtoCrearBarraRefuerzoBordeLibre()
                {
                    TipoBorde = TipoBorde.estribo,
                    floor = _floor,
                    br = br,
                    _iGeometriaTagRefuerzo = _iGeometriaTagRefuerzo,
                    numeroBarra = _calculosRefuerzoTipoViga.NumeroBArras,
                    textREfuerzoBarra = "F=F'=",
                    _TipoRebar = _casoTipoRebar //TipoRebar.REFUERZO_BA_REF_LO
                };
                BarraRefuerzoBordeLibre nwBArra = new BarraRefuerzoBordeLibre(_uiapp, _newDToBRBL);

                if (!nwBArra.generarBarra_SinTrans()) continue;

                if (ultimoBarraSup == count)
                {
                    if (nwBArra._rebar == null) continue;
                    var _configuracionTAgEstriboDTo = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                        IsDIrectriz = false,
                        tagOrientation = TagOrientation.Horizontal,
                    };
                    nwBArra.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo);
                }
                listaDibujadosRefuerzo.Add(nwBArra);
                count += 1;
            }
            int primeraBarra = 1;
            foreach (CalculoBarraRefuerzo br in _calculosRefuerzoTipoViga.ListaBArrasInferior)
            {
                DtoCrearBarraRefuerzoBordeLibre _newDToBRBL = new DtoCrearBarraRefuerzoBordeLibre()
                {
                    TipoBorde = TipoBorde.estribo,
                    floor = _floor,
                    br = br,
                    _iGeometriaTagRefuerzo = _iGeometriaTagRefuerzo,
                    numeroBarra = _calculosRefuerzoTipoViga.NumeroBArras,
                    textREfuerzoBarra = "F=F'=",
                    _TipoRebar = _casoTipoRebar //TipoRebar.REFUERZO_BA_REF_LO
                };
                BarraRefuerzoBordeLibre nwBArra = new BarraRefuerzoBordeLibre(_uiapp, _newDToBRBL);
                if (!nwBArra.generarBarra_SinTrans()) continue;

                if (primeraBarra == count && ultimoBarraSup == 0) //signifca que es caso inferior ultimoBarraSup==0
                {
                    if (nwBArra._rebar == null) continue;
                    var _configuracionTAgEstriboDTo = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                        IsDIrectriz = false,
                        tagOrientation = TagOrientation.Horizontal,
                    };
                    nwBArra.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo);
                }
                listaDibujadosRefuerzo.Add(nwBArra);
                count += 1;
            }
        }

        public bool M2_CambiarColor_ConTrans()
        {
            if (listaDibujadosRefuerzo == null) return false;
            if (listaDibujadosEstribo == null) return false;
            if (listaDibujadosRefuerzo.Count == 0 && listaDibujadosEstribo.Count==0) return true;

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarColorTipoViga-NH");

                    //barra
                    M2_1_DibujarBarra_visualiza();
                    VisibilidadElementRebarLosa visibilidadElement = new VisibilidadElementRebarLosa(_uiapp);
                    var listaIdRebar = listaDibujadosRefuerzo.Select(rb => rb._rebar.Id).ToList();
                    visibilidadElement.ChangeListElementsColorSinTrans(listaIdRebar, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta), false);

                    //estribo
                    M2_2_DibujarEstribo_espaciemiento_estribo_sintras();
                    if (listaDibujadosEstribo.Count > 0)
                    {
                        var listaEstribo = listaDibujadosEstribo.Select(rb => rb._rebar.Id).ToList();
                        CambiarColorBarras_Service CambiarColorBarras_Service = new CambiarColorBarras_Service(_uiapp, TipoConfiguracionEstribo.Estribo);
                        CambiarColorBarras_Service.M1_2_CAmbiarColor_sintrans(listaEstribo);
                    }

                    //NOTA 01-10-2021 -> OCUltaba los refuerzo tipo viga en losas. si no hay inconveniente borrar las lineas 5 inferiore
                    // OcultarBarrasRebaroPathrein _OcultarBarras_refuerzoLosa = new OcultarBarrasRebaroPathrein(_doc);
                    //List<Element> listaElemetosOcultar = new List<Element>();
                    //listaElemetosOcultar.AddRange(listaDibujadosRefuerzo.Select(r => (Element)r._rebar).ToList());
                    //listaElemetosOcultar.AddRange(listaDibujadosEstribo.Select(r => (Element)r._rebar).ToList());
                    //_OcultarBarras_refuerzoLosa.OcultarListaBarraCreada_SinTrans(listaElemetosOcultar);


                    t.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void M2_1_DibujarBarra_visualiza()
        {
            foreach (BarraRefuerzoBordeLibre er in listaDibujadosRefuerzo)
            {
                double espacimeinto = er.espesorLosa - Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm + ConstNH.RECUBRIMIENTO_LOSA_SUP_cm + 2 * er.diamtroBarraMM / 10 + 2 * er.diamtroBarraEstriboMM / 10);
                er.M5_ConfiguraEspaciamiento_SetLayoutAsNumberWithSpacing(numeroBarrasBarraRef, espacimeinto);
                er.M1_6_visualizar_sintrans();
            }
        }

        private void M2_2_DibujarEstribo_espaciemiento_estribo_sintras()
        {
            bool isUNo = true;
            foreach (BarraRefuerzoEstribo er in listaDibujadosEstribo)
            {
                er.ConfigurarAsignarParametrosRebarshape_Sintrans();
                er.M1_6_visualizar_sintrans();

                if (isUNo)
                {
                    isUNo = false;
                    var _configuracionTAgEstriboDTo = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                        IsDIrectriz = false,
                        tagOrientation = TagOrientation.Horizontal,
                    };
                    //_estribo.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo);
                    er.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo);
                }

            }
        }

        #endregion


        #region 2) metodos

        #endregion

    }
}
