using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Pasadas.Servicio
{
    public class ServicioCrearOpenin
    {
        private UIApplication _uiapp;
        private Document _doc;
        //private readonly ObservableCollection<EnvoltorioBase> _listaEnvoltorioPipes;
        public ObservableCollection<EnvoltorioBase> ListaEnvoltorioPipes { get; set; }

        public List<EnvoltorioBase> ListaEnvoltorioPipes_SOLOAgregados { get; set; }
        public List<string> ListaEjes { get; set; }
        public ServicioCrearOpenin(UIApplication _uiapp, ObservableCollection<EnvoltorioBase> listaEnvoltorioPipes)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.ListaEnvoltorioPipes = listaEnvoltorioPipes;
            this.ListaEjes = new List<string>();
            this.ListaEnvoltorioPipes_SOLOAgregados = new List<EnvoltorioBase>();

        }

        internal bool M1_ObtenerInterseccion(string nombre3d, double AreaMaxCrearShaft_foot2, TipoElementoBArraV filtroPorTipoInterseccion)
        {
            try
            {
                bool isDibujarRAyyoBusqueda = false;
                View3D _view3D = TiposFamilia3D.Get3DBuscar(_doc, nombre3d);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial: 3D Buscar");
                    return false;
                }

                Debug.Write($"-------------------------------------------");
                foreach (var item in ListaEnvoltorioPipes)
                {
                    double Area_trasversal = item.LargoAlto_DibujarPasada_foot * item.LargoAncho_DibujarPasada_foot;
                    Debug.WriteLine($" Area foot2:{Area_trasversal}   id elem:{item._elemento.Id}     nombre elem:{item._elemento.Name}");
                    if (Area_trasversal < AreaMaxCrearShaft_foot2) continue;
                    if (item.ListaPasadas.Count > 0) continue;


                    BuscarElementosHorizontal _buscarElementosBajo = new BuscarElementosHorizontal(_uiapp, item.LargoPipe, _view3D, true);
                    if (_buscarElementosBajo.BuscarObjetos(item.Pto1, (item.Pto2 - item.Pto1).Normalize(), isDibujarRAyyoBusqueda))
                    {
                        item.Comentario = "Lista Interseciones :" + _buscarElementosBajo.listaObjEncontrados.Count;
                        foreach (var obj in _buscarElementosBajo.listaObjEncontrados)
                        {
                            if (filtroPorTipoInterseccion == TipoElementoBArraV.losa && TipoElementoBArraV.losa != obj.nombreTipo) continue;
                            if (filtroPorTipoInterseccion != TipoElementoBArraV.losa && filtroPorTipoInterseccion != TipoElementoBArraV.none && TipoElementoBArraV.losa == obj.nombreTipo) continue;

                            item.ListaPasadas.Add(new EnvoltorioShaft(obj));
                        }

                        if (item.ListaPasadas.Count == 0)
                        {
                            item.EstadoShaft = "NoNecesario";
                            item.ColorEstadoShaft = "Yellow";
                            continue;
                        }

                        if (_buscarElementosBajo.listaObjEncontrados.Exists(c => c.PlanarfaceIntersectada == null))
                            item.NombreDucto = "ConError";
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerInterseccion' con elementos. Ex:{ex.Message}");
                return false;
            }
            return true;
        }


        internal bool M1_ObtenerInterseccion_PasadasRojas(string nombre3d, double AreaMaxCrearShaft_foot2, TipoElementoBArraV filtroPorTipoInterseccion)
        {
            try
            {
                bool isDibujarRAyyoBusqueda = false;
                View3D _view3D = TiposFamilia3D.Get3DBuscar(_doc, nombre3d);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial: 3D Buscar");
                    return false;
                }

                foreach (var item in ListaEnvoltorioPipes)
                {

                    if (item.ListaPasadas.Count > 0) continue;

                    BuscarElementosHorizontal _buscarElementosBajo = new BuscarElementosHorizontal(_uiapp, item.LargoPipe, _view3D, true);
                    if (_buscarElementosBajo.BuscarObjetos_Pasadas(item.Pto1, (item.Pto2 - item.Pto1).Normalize(), isDibujarRAyyoBusqueda))
                    {
                        foreach (var obj in _buscarElementosBajo.listaObjEncontrados)
                        {
                            item.ListaPasadas.Add(new EnvoltorioShaft(obj));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ObtenerInterseccion' con elementos. Ex:{ex.Message}");
                return false;
            }
            return true;
        }


        //dibuj shaft y pasadas
        public bool M2_DibujarShaft_Pasadas(string _nombrePasada, double AreaMaxAutovalid)
        {
            EnvoltorioBase _actualEnvoltorioPipes = default;
            List<PAsadasCuandradaDTO> ListaPAsadas = new List<PAsadasCuandradaDTO>();
            int cont = 0;
            try
            {
                //ServicioCrearPAsadas _ServicioCrearPAsadas_crarFAmilia = new ServicioCrearPAsadas(_uiapp);
                //_ServicioCrearPAsadas_crarFAmilia.M1_ObtenerFamilia_ModelGeneric(_nombrePasada);

                using (Transaction t = new Transaction(_doc, "DibujarShaft_Pasadas"))
                {
                    t.Start();
                    var resultCorrecto = ListaEnvoltorioPipes.Where(r => r.ListaPasadas.Exists(vv => vv._ObjetosEncontradosDTO.PlanarfaceIntersectada != null)).ToList();

                    foreach (EnvoltorioBase _envoltorioElementoDucto in resultCorrecto)
                    {
                        cont += 1;

                        //if (cont < inicial || cont > Final) continue;
                        _actualEnvoltorioPipes = _envoltorioElementoDucto;

                        if (_envoltorioElementoDucto.IsOK == false || _envoltorioElementoDucto.ListaPasadas.Count == 0)
                        {
                            _envoltorioElementoDucto.EstadoShaft = "NoNecesario";
                            _envoltorioElementoDucto.ColorEstadoShaft = "Yelow";
                            continue;
                        }

                        XYZ direccionBusqueda = (_envoltorioElementoDucto.Pto2 - _envoltorioElementoDucto.Pto1).Normalize();

                        // si tiene una sola interseccion  no re ejecuta
                       // if (_envoltorioElementoDucto.ListaPasadas.Count < 2) continue;
                        // puede que una elemto aprezca dos veces pq intersecta las dos cara de un lemento, esto filtra para seleccionar solo una cara del elemento
                        List<EnvoltorioShaft> ListaPasadas = _envoltorioElementoDucto.ListaPasadas.GroupBy(p => p._ObjetosEncontradosDTO.elemtid).Select(g => g.First()).ToList();
                        _envoltorioElementoDucto.ListaPasadas = ListaPasadas;
                        var ResulListaPasadas = ListaPasadas.Where(r => r._ObjetosEncontradosDTO.PlanarfaceIntersectada != null && r._ObjetosEncontradosDTO.PlanarfaceIntersectada?.Reference != null)
                                                                        .Where(c => Math.Abs(Util.GetProductoEscalar(c._ObjetosEncontradosDTO.NormalFace, direccionBusqueda)) > 0.5)
                                                                        .ToList();

                        ServicioCrearPAsadas _ServicioCrearPAsadas = new ServicioCrearPAsadas(_uiapp);

                        //** para guardar datos internos
                        DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                        CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                        //*********
                        for (int i = 0; i < ResulListaPasadas.Count; i++)
                        {
                            EnvoltorioShaft pasada = ResulListaPasadas[i];
                            Opening resultOpenin = default;
                            double espesor = 0;
                            ColorTipoPasada _ColorTipoPasada = ColorTipoPasada.PASADA_VERDE;

                            double area = _envoltorioElementoDucto.LargoAncho_DibujarPasada_foot * _envoltorioElementoDucto.LargoAlto_DibujarPasada_foot;

                            try
                            {
                                if (pasada != null)
                                {
                                    var ObjEnc = pasada._ObjetosEncontradosDTO;

                                    if (pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.losa)
                                    {
                                        //  var ObjEnc = resul._ObjetosEncontradosDTO;
                                        espesor = pasada._ObjetosEncontradosDTO.EspesorLosa;
                                        resultOpenin = _doc.Create.NewOpening(pasada._ObjetosEncontradosDTO.elemt, _envoltorioElementoDucto.profile, true);
                                    }
                                    else if (pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.muro)
                                    {
                                        // var ObjEnc = resul._ObjetosEncontradosDTO;
                                        XYZ potCnetro = (_envoltorioElementoDucto.ptoInf_diseñoEnMuro + _envoltorioElementoDucto.ptoSup_diseñoEnMuro) / 2.0;
                                        espesor = pasada._ObjetosEncontradosDTO.EspesorMuro;
                                        resultOpenin = _doc.Create.NewOpening((Wall)pasada._ObjetosEncontradosDTO.elemt, _envoltorioElementoDucto.ptoInf_diseñoEnMuro, _envoltorioElementoDucto.ptoSup_diseñoEnMuro);
                                    }
                                    else if (pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.viga)
                                    {
                                        _envoltorioElementoDucto.EstadoShaft = "ShaftViga";
                                        _envoltorioElementoDucto.ColorEstadoShaft = "Cyan";
                                        espesor = pasada._ObjetosEncontradosDTO.EspesorViga;

                                        XYZ normaface = pasada._ObjetosEncontradosDTO.NormalFace;
                                        XYZ vectoPLano = XYZ.BasisZ.CrossProduct(normaface);

                                        var face = ObjEnc.PlanarfaceIntersectada;
                                        Autodesk.Revit.Creation.eRefFace iFace = default;

                                        iFace = Autodesk.Revit.Creation.eRefFace.CenterY;

                                        resultOpenin = _doc.Create.NewOpening(pasada._ObjetosEncontradosDTO.elemt, _envoltorioElementoDucto.profile, iFace);
                                    }
                                    else
                                    {

                                        _envoltorioElementoDucto.EstadoShaft = "Error";
                                        _envoltorioElementoDucto.ColorEstadoShaft = "Red";
                                        Util.InfoMsg($"No esta la opcion para crear shaf en elemento :{pasada._ObjetosEncontradosDTO.nombreTipo}");
                                        continue;
                                    }


                                    //****crear pasada
                                    //PAsadasCuandradaDTO _PAsadasCuandradaDTO = new PAsadasCuandradaDTO()
                                    //{
                                    //    ancho_foot = _envoltorioElementoDucto.LargoAncho_DibujarPasada_foot,
                                    //    Espesor_foot = espesor,
                                    //    largo_foot = _envoltorioElementoDucto.LargoAlto_DibujarPasada_foot,
                                    //    face = resul._ObjetosEncontradosDTO.PlanarfaceIntersectada,
                                    //    NormalFace = resul._ObjetosEncontradosDTO.NormalFace,
                                    //    puntoInsercion = resul._ObjetosEncontradosDTO.ptoInterseccion,
                                    //    NombrePasada = _ColorTipoPasada
                                    //};

                                    //ListaPAsadas.Add(_PAsadasCuandradaDTO);

                                    //if (_ServicioCrearPAsadas.Crear_sintrasn(_PAsadasCuandradaDTO))
                                    //{
                                    //    resul.Pasada = _ServicioCrearPAsadas.familyInstance;
                                    //}
                                    ////****

                                    pasada.OpeningCreado = resultOpenin;
                                    //_envoltorioElementoDucto.EstadoShaft = "ConPasada";
                                    //_envoltorioElementoDucto.ColorEstadoShaft = "Green";
                                    //item.ListaPasadas.Clear();
                                    //item.ListaPasadas.Add(resul);

                                    // agregar datos internos
                                    // double area = item.LargoAncho_DibujarPasada_foot * item.LargoAlto_DibujarPasada_foot;
                                    if (resultOpenin != null)
                                        _CreadorExtStore.M1_SET_DataInElement_XYZ_SInTrans(resultOpenin, pasada._ObjetosEncontradosDTO.ptoInterseccion, EstadoPasada.Validado, area, "");

                                    //if (resul.Pasada != null)
                                    //    _CreadorExtStore.M1_SET_DataInElement_XYZ_SInTrans(resul.Pasada, resul._ObjetosEncontradosDTO.ptoInterseccion, EstadoPasada.Validado, area, "");
                                }
                            }
                            catch (Exception ex2)
                            {
                                _envoltorioElementoDucto.EstadoShaft = "Error";
                                _envoltorioElementoDucto.ColorEstadoShaft = "Red";
                                Debug.WriteLine($"Error en iteracion cont:{cont}   Idelem={_actualEnvoltorioPipes._elemento.Id}   nombre:{_actualEnvoltorioPipes.NombreDucto}.\n\n ex:{ex2.Message}");
                            }
                        }
                    }
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DIbujarShaft' cont:{cont} id elem={_actualEnvoltorioPipes._elemento.Id} nombre:{_actualEnvoltorioPipes.NombreDucto}. Ex:{ex.Message}");
                return true;
            }
            return false;
        }
        public bool M3_DibujarSOLOPasadas(List<EnvoltorioPasadas> ListaEnvoltorioPasadas)
        {
            EnvoltorioBase _actualEnvoltorioPipes = default;
            List<PAsadasCuandradaDTO> ListaPAsadas = new List<PAsadasCuandradaDTO>();
            int cont = 0;
            try
            {
                ColorTipoPasada _ColorTipoPasada = ColorTipoPasada.PASADA_VERDE;

                using (Transaction t = new Transaction(_doc, "DibujarSOLOPasadas"))
                {
                    t.Start();
                    var resultCorrecto = ListaEnvoltorioPipes.Where(r => r.ListaPasadas.Exists(vv => vv._ObjetosEncontradosDTO.PlanarfaceIntersectada != null)).ToList();

                    foreach (EnvoltorioBase _envoltorioElementoDucto in resultCorrecto)
                    {
                        cont += 1;

                        if (_envoltorioElementoDucto.IsOK == false || _envoltorioElementoDucto.ListaPasadas.Count == 0)
                        {
                            _envoltorioElementoDucto.EstadoShaft = "NoNecesario";
                            _envoltorioElementoDucto.ColorEstadoShaft = "Yelow";
                            continue;
                        }

                        XYZ direccionBusqueda = (_envoltorioElementoDucto.Pto2 - _envoltorioElementoDucto.Pto1).Normalize();

                        // si tiene una sola interseccion  no re ejecuta
                        if (_envoltorioElementoDucto.ListaPasadas.Count < 2 && _envoltorioElementoDucto.Orientacion3D_ == "Vertical") continue;
                        // puede que una elemto aprezca dos veces pq intersecta las dos cara de un lemento, esto filtra para seleccionar solo una cara del elemento
                        List<EnvoltorioShaft> ListaPasadas = _envoltorioElementoDucto.ListaPasadas.GroupBy(p => p._ObjetosEncontradosDTO.elemtid).Select(g => g.First()).ToList();

                        _envoltorioElementoDucto.ListaPasadas = ListaPasadas;

                        var ListaResul = ListaPasadas.Where(r => r._ObjetosEncontradosDTO.PlanarfaceIntersectada != null && r._ObjetosEncontradosDTO.PlanarfaceIntersectada?.Reference != null)
                                                                        .Where(c => Math.Abs(Util.GetProductoEscalar(c._ObjetosEncontradosDTO.NormalFace, direccionBusqueda)) > 0.5)
                                                                        .ToList();

                        //** para guardar datos internos
                        DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                        CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                        //*********
                        for (int i = 0; i < ListaResul.Count; i++)
                        {
                            EnvoltorioShaft Pasada = ListaResul[i];
                            EnvoltorioBase _envoltorioElementoDucto_Aux = default;

                            if (i == 0)
                                _envoltorioElementoDucto_Aux = _envoltorioElementoDucto;
                            else
                            {
                                _envoltorioElementoDucto.ListaPasadas.Remove(Pasada);
                                _envoltorioElementoDucto.Comentario = "Lista Interseciones :" + _envoltorioElementoDucto.ListaPasadas.Count +
                                                                _envoltorioElementoDucto.ListaPasadas[0]._ObjetosEncontradosDTO.elemt.Id.IntegerValue;

                                _envoltorioElementoDucto_Aux = _envoltorioElementoDucto.Clone();
                                _envoltorioElementoDucto_Aux.ListaPasadas.Clear();
                                Pasada = (EnvoltorioShaft)Pasada.Clone(); ConstNH.corte();
                                _envoltorioElementoDucto_Aux.ListaPasadas.Add(Pasada);
                                _envoltorioElementoDucto_Aux.Comentario = "Lista Interseciones :" + _envoltorioElementoDucto_Aux.ListaPasadas.Count +
                                                                _envoltorioElementoDucto.ListaPasadas[0]._ObjetosEncontradosDTO.elemt.Id.IntegerValue;
                            }

                            double area = _envoltorioElementoDucto_Aux.LargoAncho_DibujarPasada_foot * _envoltorioElementoDucto_Aux.LargoAlto_DibujarPasada_foot;
                            //Opening resultOpenin = default;
                            double espesor = 0;
                            try
                            {
                                if (Pasada != null)
                                {
                                    var ObjEnc = Pasada._ObjetosEncontradosDTO;

                                    if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.losa)
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorLosa;
                                    else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.muro)
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorMuro;
                                    else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.viga)
                                    {
                                        _envoltorioElementoDucto_Aux.EstadoShaft = "ShaftViga";
                                        _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Cyan";
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorViga;
                                    }
                                    else
                                    {
                                        _envoltorioElementoDucto_Aux.EstadoShaft = "Error";
                                        _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Red";
                                        Util.InfoMsg($"No esta la opcion para crear shaf en elemento :{Pasada._ObjetosEncontradosDTO.nombreTipo}");
                                        continue;
                                    }

                                    if (!ListaEjes.Contains(_envoltorioElementoDucto_Aux.ejesGrilla))
                                        ListaEjes.Add(_envoltorioElementoDucto_Aux.ejesGrilla);

                                    //****crear pasada
                                    PAsadasCuandradaDTO _pasadasCuandradaDTO = new PAsadasCuandradaDTO()
                                    {
                                        ancho_foot = _envoltorioElementoDucto_Aux.LargoAncho_DibujarPasada_foot,
                                        Espesor_foot = espesor,
                                        largo_foot = _envoltorioElementoDucto_Aux.LargoAlto_DibujarPasada_foot,
                                        face = Pasada._ObjetosEncontradosDTO.PlanarfaceIntersectada,
                                        NormalFace = Pasada._ObjetosEncontradosDTO.NormalFace,
                                        puntoInsercion = Pasada._ObjetosEncontradosDTO.ptoInterseccion,
                                        NombrePasada = _ColorTipoPasada,
                                        NombreEje= _envoltorioElementoDucto_Aux.ejesGrilla,
                                        NombreElementoIntersectado= Pasada._ObjetosEncontradosDTO.nombreTipo
                                    };

                                    ListaPAsadas.Add(_pasadasCuandradaDTO);

                                    // buscar si ya existe
                                    

                                    ServicioSiYaExiste_Yes_Igual _SiYaExiste_Yes_Igual = new ServicioSiYaExiste_Yes_Igual(_uiapp, ListaEnvoltorioPasadas);
                                    if (_SiYaExiste_Yes_Igual.Buscar(Pasada._ObjetosEncontradosDTO.ptoInterseccion))
                                    {
                                        Pasada.Pasada = _SiYaExiste_Yes_Igual.Pasada;
                                        string comentario = _CreadorExtStore.M3_OBtenerResultado_String(Pasada.Pasada, "comentario", "SubFieldTest41");

                                        if (Pasada.Pasada.Name == ColorTipoPasada.PASADA_AZUL.ToString())
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_AZUL;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "Revision";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Blue";
                                            _envoltorioElementoDucto_Aux.Comentario = comentario;
                                            Pasada.Estado = EstadoPasada.Revision;
                                        }
                                        else if (Pasada.Pasada.Name == ColorTipoPasada.PASADA_NARANJO.ToString())
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_NARANJO;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "Correccion";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Orange";
                                            Pasada.Estado = EstadoPasada.Correcion;
                                        }
                                        else if (Pasada.Pasada.Name == ColorTipoPasada.PASADA_GRIS.ToString())
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_GRIS;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "Rechazar";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Gray";
                                            _envoltorioElementoDucto_Aux.Comentario = comentario;
                                            Pasada.Estado = EstadoPasada.Correcion;
                                        }
                                        else
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_VERDE;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "ConPasada";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Green";
                                            Pasada.Estado = EstadoPasada.Validado;
                                        }

                                        _envoltorioElementoDucto_Aux.PasadaId = Pasada.Pasada.Id.IntegerValue;
                                        // agrega id del elemto intersectado  -- pueden haber varias pasadas por  registro 'EnvoltorioBase' depende de las intersecciones, se registra el 
                                        //itimo elemtento (puede ser muro,losa, viga)
                                        _envoltorioElementoDucto_Aux.ElementoId = Pasada._ObjetosEncontradosDTO.elemt.Id.IntegerValue.ToString();
                                    }
                                    else // crear nuevo
                                    {

                                        ServicioCrearPAsadas _ServicioCrearPAsadas = new ServicioCrearPAsadas(_uiapp);
                                        if (_ServicioCrearPAsadas.Crear_sintrasn(_pasadasCuandradaDTO))
                                        {
                                            Pasada.Pasada = _ServicioCrearPAsadas.familyInstance;
                                            Pasada.Estado = EstadoPasada.Validado;
                                            _envoltorioElementoDucto_Aux.PasadaId = _ServicioCrearPAsadas.familyInstance.Id.IntegerValue;
                                            // agrega id del elemto intersectado  -- pueden haber varias pasadas por  registro 'EnvoltorioBase' depende de las intersecciones, se registra el 
                                            //itimo elemtento (puede ser muro,losa, viga)
                                            _envoltorioElementoDucto_Aux.ElementoId = Pasada._ObjetosEncontradosDTO.elemt.Id.IntegerValue.ToString();
                                        }
                                        else
                                            Pasada.Estado = EstadoPasada.ConError;

                                        _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_VERDE;
                                        _envoltorioElementoDucto_Aux.EstadoShaft = "ConPasada";
                                        _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Green";

                                        // agregar datos internos
                                        _CreadorExtStore.M1_SET_DataInElement_XYZ_SInTrans(Pasada.Pasada, Pasada._ObjetosEncontradosDTO.ptoInterseccion, EstadoPasada.Validado, area, "");

                                    }
                                    //****
                                    Pasada.DatosPasadas = _pasadasCuandradaDTO;
                                    Pasada.OpeningCreado = null;

                                    // agregar los otras pasasdas creadas por el mismo ducto
                                    if (i != 0)
                                    {
                                        ListaEnvoltorioPipes.Add(_envoltorioElementoDucto_Aux);
                                        ListaEnvoltorioPipes_SOLOAgregados.Add(_envoltorioElementoDucto_Aux);
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                _envoltorioElementoDucto.EstadoShaft = "Error";
                                _envoltorioElementoDucto.ColorEstadoShaft = "Red";
                                Debug.WriteLine($"Error en iteracion cont:{cont}   Idelem={_actualEnvoltorioPipes._elemento.Id}   nombre:{_actualEnvoltorioPipes.NombreDucto}.\n\n ex:{ex2.Message}");
                            }
                        }
                    }
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DIbujarShaft' cont:{cont} id elem={_actualEnvoltorioPipes._elemento.Id} nombre:{_actualEnvoltorioPipes.NombreDucto}.\n\n ex:{ex.Message}");
                return true;
            }
            return false;
        }


        public bool M4_Revision_SOLOPasadas(List<EnvoltorioPasadas> ListaEnvoltorioPasadas)
        {
            EnvoltorioBase _actualEnvoltorioPipes = default;
            List<PAsadasCuandradaDTO> ListaPAsadas = new List<PAsadasCuandradaDTO>();
            int cont = 0;
            try
            {
                ColorTipoPasada _ColorTipoPasada = ColorTipoPasada.PASADA_VERDE;

                using (Transaction t = new Transaction(_doc, "DibujarSOLOPasadas"))
                {
                    t.Start();
                    var resultCorrecto = ListaEnvoltorioPipes.Where(r => r.ListaPasadas.Exists(vv => vv._ObjetosEncontradosDTO.PlanarfaceIntersectada != null)).ToList();

                    foreach (EnvoltorioBase _envoltorioElementoDucto in resultCorrecto)
                    {
                        cont += 1;

                        if (_envoltorioElementoDucto.IsOK == false || _envoltorioElementoDucto.ListaPasadas.Count == 0)
                        {
                            _envoltorioElementoDucto.EstadoShaft = "NoNecesario";
                            _envoltorioElementoDucto.ColorEstadoShaft = "Yelow";
                            continue;
                        }

                        XYZ direccionBusqueda = (_envoltorioElementoDucto.Pto2 - _envoltorioElementoDucto.Pto1).Normalize();

                        // si tiene una sola interseccion  no re ejecuta
                        if (_envoltorioElementoDucto.ListaPasadas.Count < 2 && _envoltorioElementoDucto.Orientacion3D_ == "Vertical") continue;
                        // puede que una elemto aprezca dos veces pq intersecta las dos cara de un lemento, esto filtra para seleccionar solo una cara del elemento
                        List<EnvoltorioShaft> ListaPasadas = _envoltorioElementoDucto.ListaPasadas.GroupBy(p => p._ObjetosEncontradosDTO.elemtid).Select(g => g.First()).ToList();

                        _envoltorioElementoDucto.ListaPasadas = ListaPasadas;

                        var ListaResul = ListaPasadas.Where(r => r._ObjetosEncontradosDTO.PlanarfaceIntersectada != null && r._ObjetosEncontradosDTO.PlanarfaceIntersectada?.Reference != null)
                                                                        .Where(c => Math.Abs(Util.GetProductoEscalar(c._ObjetosEncontradosDTO.NormalFace, direccionBusqueda)) > 0.5)
                                                                        .ToList();
                        Debug.WriteLine($" {cont}) Id elemn:{_envoltorioElementoDucto._elemento.Id.IntegerValue }   cantidad pasadas:{ListaResul.Count} ");


                        //** para guardar datos internos
                        DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                        CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                        //*********
                        for (int i = 0; i < ListaResul.Count; i++)
                        {
                            EnvoltorioShaft Pasada = ListaResul[i];
                            EnvoltorioBase _envoltorioElementoDucto_Aux = default;

                            if (i == 0)
                                _envoltorioElementoDucto_Aux = _envoltorioElementoDucto;
                            else
                            {
                                _envoltorioElementoDucto.ListaPasadas.Remove(Pasada);
                                _envoltorioElementoDucto.Comentario = "Lista Interseciones :" + _envoltorioElementoDucto.ListaPasadas.Count;

                                _envoltorioElementoDucto_Aux = _envoltorioElementoDucto.Clone();
                                _envoltorioElementoDucto_Aux.ListaPasadas.Clear();
                                Pasada = (EnvoltorioShaft)Pasada.Clone();
                                _envoltorioElementoDucto_Aux.ListaPasadas.Add(Pasada);
                                _envoltorioElementoDucto_Aux.Comentario = "Lista Interseciones :" + _envoltorioElementoDucto_Aux.ListaPasadas.Count;
                            }

                            double area = _envoltorioElementoDucto_Aux.LargoAncho_DibujarPasada_foot * _envoltorioElementoDucto_Aux.LargoAlto_DibujarPasada_foot;
                            //Opening resultOpenin = default;
                            double espesor = 0;
                            try
                            {
                                if (Pasada != null)
                                {
                                    var ObjEnc = Pasada._ObjetosEncontradosDTO;

                                    if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.losa)
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorLosa;
                                    else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.muro)
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorMuro;
                                    else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.viga)
                                    {
                                        _envoltorioElementoDucto_Aux.EstadoShaft = "ShaftViga";
                                        _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Cyan";
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorViga;
                                    }
                                    else
                                    {
                                        _envoltorioElementoDucto_Aux.EstadoShaft = "Error";
                                        _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Red";
                                        Util.InfoMsg($"No esta la opcion para crear shaf en elemento :{Pasada._ObjetosEncontradosDTO.nombreTipo}");
                                        continue;
                                    }


                                    //****crear pasada
                                    PAsadasCuandradaDTO _pasadasCuandradaDTO = new PAsadasCuandradaDTO()
                                    {
                                        ancho_foot = _envoltorioElementoDucto_Aux.LargoAncho_DibujarPasada_foot,
                                        Espesor_foot = espesor,
                                        largo_foot = _envoltorioElementoDucto_Aux.LargoAlto_DibujarPasada_foot,
                                        face = Pasada._ObjetosEncontradosDTO.PlanarfaceIntersectada,
                                        NormalFace = Pasada._ObjetosEncontradosDTO.NormalFace,
                                        puntoInsercion = Pasada._ObjetosEncontradosDTO.ptoInterseccion,
                                        NombrePasada = _ColorTipoPasada
                                    };

                                    ListaPAsadas.Add(_pasadasCuandradaDTO);


                                    // agregar datos internos
                                    string comentario = _CreadorExtStore.M3_OBtenerResultado_String(Pasada.Pasada, "comentario", "SubFieldTest41");

                                    ServicioSiYaExiste_Yes_Igual _SiYaExiste_Yes_Igual = new ServicioSiYaExiste_Yes_Igual(_uiapp, ListaEnvoltorioPasadas);
                                    if (_SiYaExiste_Yes_Igual.Buscar(Pasada._ObjetosEncontradosDTO.ptoInterseccion))
                                    {
                                        Pasada.Pasada = _SiYaExiste_Yes_Igual.Pasada;


                                        if (Pasada.Pasada.Name == ColorTipoPasada.PASADA_AZUL.ToString())
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_AZUL;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "Revision";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Blue";
                                            _envoltorioElementoDucto_Aux.Comentario = comentario;
                                            Pasada.Estado = EstadoPasada.Revision;
                                        }
                                        else if (Pasada.Pasada.Name == ColorTipoPasada.PASADA_NARANJO.ToString())
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_NARANJO;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "Correccion";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Orange";
                                            Pasada.Estado = EstadoPasada.Correcion;
                                        }
                                        else if (Pasada.Pasada.Name == ColorTipoPasada.PASADA_GRIS.ToString())
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_GRIS;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "Rechazar";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Gray";
                                            _envoltorioElementoDucto_Aux.Comentario = comentario;
                                            Pasada.Estado = EstadoPasada.Correcion;
                                        }
                                        else
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_VERDE;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "ConPasada";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Green";
                                            Pasada.Estado = EstadoPasada.Validado;
                                        }

                                        _envoltorioElementoDucto_Aux.PasadaId = Pasada.Pasada.Id.IntegerValue;
                                        // agrega id del elemto intersectado  -- pueden haber varias pasadas por  registro 'EnvoltorioBase' depende de las intersecciones, se registra el 
                                        //itimo elemtento (puede ser muro,losa, viga)
                                        _envoltorioElementoDucto_Aux.ElementoId = Pasada._ObjetosEncontradosDTO.elemt.Id.IntegerValue.ToString();

                                    }
                                    else
                                    {
                                        ServicioCrearPAsadas _ServicioCrearPAsadas = new ServicioCrearPAsadas(_uiapp);
                                        if (_ServicioCrearPAsadas.Crear_sintrasn(_pasadasCuandradaDTO))
                                        {
                                            Pasada.Pasada = _ServicioCrearPAsadas.familyInstance;
                                            Pasada.Estado = EstadoPasada.Correcion;
                                            _envoltorioElementoDucto_Aux.PasadaId = _ServicioCrearPAsadas.familyInstance.Id.IntegerValue;
                                            // agrega id del elemto intersectado  -- pueden haber varias pasadas por  registro 'EnvoltorioBase' depende de las intersecciones, se registra el 
                                            //itimo elemtento (puede ser muro,losa, viga)
                                            _envoltorioElementoDucto_Aux.ElementoId = Pasada._ObjetosEncontradosDTO.elemt.Id.IntegerValue.ToString();
                                        }
                                        else
                                            Pasada.Estado = EstadoPasada.ConError;

                                        _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_NARANJO;
                                        _envoltorioElementoDucto_Aux.EstadoShaft = "Correccion";
                                        _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Orange";

                                        _CreadorExtStore.M1_SET_DataInElement_XYZ_SInTrans(Pasada.Pasada, Pasada._ObjetosEncontradosDTO.ptoInterseccion, EstadoPasada.Correcion, area, comentario);
                                    }

                                    Pasada.DatosPasadas = _pasadasCuandradaDTO;
                                    Pasada.OpeningCreado = null;

                                    if (i != 0)
                                    {
                                        ListaEnvoltorioPipes.Add(_envoltorioElementoDucto_Aux);
                                        ListaEnvoltorioPipes_SOLOAgregados.Add(_envoltorioElementoDucto_Aux);
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                _envoltorioElementoDucto.EstadoShaft = "Error";
                                _envoltorioElementoDucto.ColorEstadoShaft = "Red";
                                Debug.WriteLine($"Error en iteracion cont:{cont}   Idelem={_actualEnvoltorioPipes._elemento.Id}   nombre:{_actualEnvoltorioPipes.NombreDucto}  \n\n ex:{ex2.Message}");
                            }
                        }
                    }
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DIbujarShaft' cont:{cont} id elem={_actualEnvoltorioPipes._elemento.Id} nombre:{_actualEnvoltorioPipes.NombreDucto}. Ex:{ex.Message}");
                return true;
            }
            return false;
        }

        public bool M3_DibujarSOLOPasadas_PasadasaRojas(List<EnvoltorioPasadas> ListaEnvoltorioPasadas)
        {
            EnvoltorioBase _actualEnvoltorioPipes = default;
            List<PAsadasCuandradaDTO> ListaPAsadas = new List<PAsadasCuandradaDTO>();
            int cont = 0;
            try
            {
                ColorTipoPasada _ColorTipoPasada = ColorTipoPasada.PASADA_ROJA;

                using (Transaction t = new Transaction(_doc, "DibujarSOLOPasadas"))
                {
                    t.Start();
                    var resultCorrecto = ListaEnvoltorioPipes.Where(r => r.ListaPasadas.Exists(vv => vv._ObjetosEncontradosDTO.PlanarfaceIntersectada != null)).ToList();

                    foreach (EnvoltorioBase _envoltorioElementoDucto in resultCorrecto)
                    {
                        cont += 1;

                        if (_envoltorioElementoDucto.IsOK == false || _envoltorioElementoDucto.ListaPasadas.Count == 0)
                        {
                            _envoltorioElementoDucto.EstadoShaft = "NoNecesario";
                            _envoltorioElementoDucto.ColorEstadoShaft = "Yelow";
                            continue;
                        }

                        XYZ direccionBusqueda = (_envoltorioElementoDucto.Pto2 - _envoltorioElementoDucto.Pto1).Normalize();

                        // si tiene una sola interseccion  no re ejecuta
                        if (_envoltorioElementoDucto.ListaPasadas.Count < 2 && _envoltorioElementoDucto.Orientacion3D_ == "Vertical") continue;
                        // puede que una elemto aprezca dos veces pq intersecta las dos cara de un lemento, esto filtra para seleccionar solo una cara del elemento
                        List<EnvoltorioShaft> ListaPasadas = _envoltorioElementoDucto.ListaPasadas.GroupBy(p => p._ObjetosEncontradosDTO.elemtid).Select(g => g.First()).ToList();

                        _envoltorioElementoDucto.ListaPasadas = ListaPasadas;

                        var ListaResul = ListaPasadas.Where(r => r._ObjetosEncontradosDTO.PlanarfaceIntersectada != null && r._ObjetosEncontradosDTO.PlanarfaceIntersectada?.Reference != null)
                                                                        .Where(c => Math.Abs(Util.GetProductoEscalar(c._ObjetosEncontradosDTO.NormalFace, direccionBusqueda)) > 0.5)
                                                                        .ToList();

                        //** para guardar datos internos
                        DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                        CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                        //*********
                        for (int i = 0; i < ListaResul.Count; i++)
                        {
                            EnvoltorioShaft Pasada = ListaResul[i];
                            EnvoltorioBase _envoltorioElementoDucto_Aux = default;

                            if (i == 0)
                                _envoltorioElementoDucto_Aux = _envoltorioElementoDucto;
                            else
                            {
                                _envoltorioElementoDucto.ListaPasadas.Remove(Pasada);
                                _envoltorioElementoDucto.Comentario = "Lista Interseciones :" + _envoltorioElementoDucto.ListaPasadas.Count;

                                _envoltorioElementoDucto_Aux = _envoltorioElementoDucto.Clone();
                                _envoltorioElementoDucto_Aux.ListaPasadas.Clear();
                                Pasada = (EnvoltorioShaft)Pasada.Clone(); ConstNH.corte();
                                _envoltorioElementoDucto_Aux.ListaPasadas.Add(Pasada);
                                _envoltorioElementoDucto_Aux.Comentario = "Lista Interseciones :" + _envoltorioElementoDucto_Aux.ListaPasadas.Count;
                            }

                            double area = _envoltorioElementoDucto_Aux.LargoAncho_DibujarPasada_foot * _envoltorioElementoDucto_Aux.LargoAlto_DibujarPasada_foot;
                            //Opening resultOpenin = default;
                            double espesor = 0;
                            try
                            {
                                if (Pasada != null)
                                {
                                    var ObjEnc = Pasada._ObjetosEncontradosDTO;

                                    //if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.losa)
                                    //    espesor = Pasada._ObjetosEncontradosDTO.EspesorLosa;
                                    //else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.muro)
                                    //    espesor = Pasada._ObjetosEncontradosDTO.EspesorMuro;
                                    //else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.viga)
                                    //{
                                    //    _envoltorioElementoDucto_Aux.EstadoShaft = "ShaftViga";
                                    //    _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Cyan";
                                    //    espesor = Pasada._ObjetosEncontradosDTO.EspesorViga;
                                    //}
                                    //else
                                    //{
                                    //    _envoltorioElementoDucto_Aux.EstadoShaft = "Error";
                                    //    _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Red";
                                    //    Util.InfoMsg($"No esta la opcion para crear shaf en elemento :{Pasada._ObjetosEncontradosDTO.nombreTipo}");
                                    //    continue;
                                    //}

                                    if (!ListaEjes.Contains(_envoltorioElementoDucto_Aux.ejesGrilla))
                                        ListaEjes.Add(_envoltorioElementoDucto_Aux.ejesGrilla);

                                    //****crear pasada
                                    PAsadasCuandradaDTO _pasadasCuandradaDTO = new PAsadasCuandradaDTO()
                                    {
                                        ancho_foot = _envoltorioElementoDucto_Aux.LargoAncho_DibujarPasada_foot,
                                        Espesor_foot = espesor,
                                        largo_foot = _envoltorioElementoDucto_Aux.LargoAlto_DibujarPasada_foot,
                                        face = Pasada._ObjetosEncontradosDTO.PlanarfaceIntersectada,
                                        NormalFace = Pasada._ObjetosEncontradosDTO.NormalFace,
                                        puntoInsercion = Pasada._ObjetosEncontradosDTO.ptoInterseccion,
                                        NombrePasada = _ColorTipoPasada
                                    };

                                    ListaPAsadas.Add(_pasadasCuandradaDTO);

                                    // buscar si ya existe
                                    string comentario = _CreadorExtStore.M3_OBtenerResultado_String(Pasada.Pasada, "comentario", "SubFieldTest41");

                                    ServicioSiYaExiste_Yes_Igual _SiYaExiste_Yes_Igual = new ServicioSiYaExiste_Yes_Igual(_uiapp, ListaEnvoltorioPasadas);
                                    if (_SiYaExiste_Yes_Igual.Buscar(Pasada._ObjetosEncontradosDTO.ptoInterseccion))
                                    {
                                        Pasada.Pasada = _SiYaExiste_Yes_Igual.Pasada;


                                        if (Pasada.Pasada.Name == ColorTipoPasada.PASADA_ROJA.ToString())
                                        {
                                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_ROJA;
                                            _envoltorioElementoDucto_Aux.EstadoShaft = "ShaftFInal";
                                            _envoltorioElementoDucto_Aux.ColorEstadoShaft = "Red";
                                            _envoltorioElementoDucto.ColorEstadoShaft_letra = "Red";
                                            _envoltorioElementoDucto_Aux.Comentario = "Aceptada";
                                            Pasada.Estado = EstadoPasada.ShaftCreado;
                                        }
                                        else
                                            continue;

                                        _envoltorioElementoDucto_Aux.PasadaId = Pasada.Pasada.Id.IntegerValue;
                                        // agrega id del elemto intersectado  -- pueden haber varias pasadas por  registro 'EnvoltorioBase' depende de las intersecciones, se registra el 
                                        //itimo elemtento (puede ser muro,losa, viga)
                                        _envoltorioElementoDucto_Aux.ElementoId = Pasada._ObjetosEncontradosDTO.elemt.Id.IntegerValue.ToString();

                                        //****
                                        Pasada.DatosPasadas = _pasadasCuandradaDTO;
                                        Pasada.OpeningCreado = null;
                                    }


                                    // agregar los otras pasasdas creadas por el mismo ducto
                                    if (i != 0)
                                    {
                                        ListaEnvoltorioPipes.Add(_envoltorioElementoDucto_Aux);
                                        ListaEnvoltorioPipes_SOLOAgregados.Add(_envoltorioElementoDucto_Aux);
                                    }
                                }
                            }
                            catch (Exception ex2)
                            {
                                _envoltorioElementoDucto.EstadoShaft = "Error";
                                _envoltorioElementoDucto.ColorEstadoShaft = "Red";
                                Debug.WriteLine($"Error en iteracion cont:{cont}   Idelem={_actualEnvoltorioPipes._elemento.Id}   nombre:{_actualEnvoltorioPipes.NombreDucto}.\n\n ex:{ex2.Message}");
                            }
                        }
                    }
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DIbujarShaft' cont:{cont} id elem={_actualEnvoltorioPipes._elemento.Id} nombre:{_actualEnvoltorioPipes.NombreDucto}.\n\n ex:{ex.Message}");
                return true;
            }
            return false;
        }



        public bool DibujarSOLOShaft()
        {
            EnvoltorioBase _actualEnvoltorioPipes = default;
            List<PAsadasCuandradaDTO> ListaPAsadas = new List<PAsadasCuandradaDTO>();
            int cont = 0;
            try
            {
                //ServicioCrearPAsadas _ServicioCrearPAsadas_crarFAmilia = new ServicioCrearPAsadas(_uiapp);
                //_ServicioCrearPAsadas_crarFAmilia.M1_ObtenerFamilia_ModelGeneric(_ColorTipoPasada.ToString());

                using (Transaction t = new Transaction(_doc, "DibujarSOLOShaft"))
                {
                    t.Start();
                    var resultCorrecto = ListaEnvoltorioPipes.Where(r => r.ListaPasadas.Exists(vv => vv._ObjetosEncontradosDTO.PlanarfaceIntersectada != null)).ToList();

                    foreach (EnvoltorioBase _envoltorioBase in resultCorrecto)
                    {
                        cont += 1;
                        //if (cont < inicial || cont > Final) continue;
                        _actualEnvoltorioPipes = _envoltorioBase;


                        if (_envoltorioBase.IsOK == false || _envoltorioBase.ListaPasadas.Count == 0)
                        {
                            _envoltorioBase.EstadoShaft = "NoNecesario";
                            _envoltorioBase.ColorEstadoShaft = "Yelow";
                            continue;
                        }

                        XYZ direccionBusqueda = (_envoltorioBase.Pto2 - _envoltorioBase.Pto1).Normalize();

                        // si tiene una sola interseccion  no re ejecuta

                        // puede que una elemto aprezca dos veces pq intersecta las dos cara de un lemento, esto filtra para seleccionar solo una cara del elemento
                        List<EnvoltorioShaft> ListaPasadas = _envoltorioBase.ListaPasadas.GroupBy(p => p._ObjetosEncontradosDTO.elemtid).Select(g => g.First()).ToList();

                        var ListaEnvoltorioShaft = ListaPasadas.Where(r => r._ObjetosEncontradosDTO.PlanarfaceIntersectada != null && r._ObjetosEncontradosDTO.PlanarfaceIntersectada?.Reference != null)
                                                                        .Where(c => Math.Abs(Util.GetProductoEscalar(c._ObjetosEncontradosDTO.NormalFace, direccionBusqueda)) > 0.5)
                                                                        .ToList();

                        ServicioCrearPAsadas _ServicioCrearPAsadas = new ServicioCrearPAsadas(_uiapp);

                        //** para guardar datos internos
                        DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                        CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                        //*********
                        for (int i = 0; i < ListaEnvoltorioShaft.Count; i++)
                        {
                            EnvoltorioShaft Pasada = ListaEnvoltorioShaft[i];

                            if (Pasada.Estado != EstadoPasada.Validado) continue;
                            Opening resultOpenin = default;
                            double espesor = 0;

                            double area = _envoltorioBase.LargoAncho_DibujarPasada_foot * _envoltorioBase.LargoAlto_DibujarPasada_foot;

                            try
                            {
                                if (Pasada != null)
                                {
                                    var ObjEnc = Pasada._ObjetosEncontradosDTO;

                                    if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.losa)
                                    {
                                        //  var ObjEnc = resul._ObjetosEncontradosDTO;
                                        resultOpenin = _doc.Create.NewOpening(Pasada._ObjetosEncontradosDTO.elemt, _envoltorioBase.profile, true);
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorLosa;


                                    }
                                    else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.muro)
                                    {
                                        // var ObjEnc = resul._ObjetosEncontradosDTO;
                                        XYZ potCnetro = (_envoltorioBase.ptoInf_diseñoEnMuro + _envoltorioBase.ptoSup_diseñoEnMuro) / 2.0;
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorMuro;
                                        resultOpenin = _doc.Create.NewOpening((Wall)Pasada._ObjetosEncontradosDTO.elemt, _envoltorioBase.ptoInf_diseñoEnMuro, _envoltorioBase.ptoSup_diseñoEnMuro);


                                    }
                                    else if (Pasada._ObjetosEncontradosDTO.nombreTipo == TipoElementoBArraV.viga)
                                    {
                                        _envoltorioBase.EstadoShaft = "ShaftViga";
                                        _envoltorioBase.ColorEstadoShaft = "Cyan";
                                        espesor = Pasada._ObjetosEncontradosDTO.EspesorViga;

                                        XYZ normaface = Pasada._ObjetosEncontradosDTO.NormalFace;
                                        XYZ vectoPLano = XYZ.BasisZ.CrossProduct(normaface);

                                        var face = ObjEnc.PlanarfaceIntersectada;
                                        Autodesk.Revit.Creation.eRefFace iFace = default;

                                        iFace = Autodesk.Revit.Creation.eRefFace.CenterY;

                                        resultOpenin = _doc.Create.NewOpening(Pasada._ObjetosEncontradosDTO.elemt, _envoltorioBase.profile, iFace);
                                    }
                                    else
                                    {
                                        _envoltorioBase.EstadoShaft = "Error";
                                        _envoltorioBase.ColorEstadoShaft = "Red";
                                        Util.InfoMsg($"No esta la opcion para crear shaf en elemento :{Pasada._ObjetosEncontradosDTO.nombreTipo}");
                                        continue;
                                    }


                                    // agregar datos internos
                                    _CreadorExtStore.M1_SET_DataInElement_XYZ_SInTrans(resultOpenin, Pasada._ObjetosEncontradosDTO.ptoInterseccion, EstadoPasada.Validado, area, "");

                                    Pasada.OpeningCreado = resultOpenin;
                                    _envoltorioBase.EstadoShaft = "ConPasada";
                                    _envoltorioBase.ColorEstadoShaft = "Green";

                                }
                            }
                            catch (Exception ex2)
                            {
                                _envoltorioBase.EstadoShaft = "Error";
                                _envoltorioBase.ColorEstadoShaft = "Red";
                                Debug.WriteLine($"Error en iteracion cont:{cont}   Idelem={_actualEnvoltorioPipes._elemento.Id}   nombre:{_actualEnvoltorioPipes.NombreDucto}.\n\nex:{ex2.Message}");
                            }
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DIbujarShaft' cont:{cont} id elem={_actualEnvoltorioPipes._elemento.Id} nombre:{_actualEnvoltorioPipes.NombreDucto}. Ex:{ex.Message}");
                return true;
            }
            return false;
        }
    }
}
