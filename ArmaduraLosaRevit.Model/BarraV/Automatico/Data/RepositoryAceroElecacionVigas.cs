
using ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Automatico.Data.Servicio;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data
{
    public class RepositoryAceroElecacionVigas
    {
        private readonly string _nombreArchivos;
        private RepositoryDataAccess RepositorioNh;
        private List<BarraFlexionTramosDTO> ListaBarrasVigas_secciones;  // obtener las seccioens de las barras , en una viga puede tener hasta 4 secciones una barra
        private ServicioOrdenTraslapo _servicioOrdenTraslapo;

        public List<BarraCorteTramos> ListaEstriboVigas_secciones { get; private set; }

        public List<TraslapoVigaAuto> ListaTraslapo { get; set; }
        private List<VigaGeometriaDTO> ListaVigas;
        private Tabla05_Objeto_con_todas_las_Tablas ListaIntervalosBarraAutoDtoIMPORTAR;

        public List<VigaAutomatico> ListaVigaAutomaticoConBArras { get; set; }
        public StringBuilder sbLog { get; set; }
        public int contadorErrores { get; set; }
        public RepositoryAceroElecacionVigas(string NombreArchivos)
        {
            RepositorioNh = new RepositoryDataAccess();

            _nombreArchivos = NombreArchivos;
            sbLog = new StringBuilder();
            contadorErrores = 0;
            ListaVigaAutomaticoConBArras = new List<VigaAutomatico>();
            ListaBarrasVigas_secciones = new List<BarraFlexionTramosDTO>();
            ListaEstriboVigas_secciones = new List<BarraCorteTramos>();
            ListaVigas = new List<VigaGeometriaDTO>();
            ListaTraslapo = new List<TraslapoVigaAuto>();
        }


        public bool ObtenerBArras_Info_Barras_Flexion_json(string NombreEje = "")
        {
            try
            {
                ListaIntervalosBarraAutoDtoIMPORTAR =
                   CreadoJson.LeerArchivoJsonING_conruta_Tabla05_Objeto_con_todas_las_Tablas(_nombreArchivos);

                //***borrarTraslapoIdem
                //ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla01_Info_Barras_Flexion = ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla01_Info_Barras_Flexion
                //                                                                                                            .Where(b => (b.ID_Name_REVIT == "1368197" || b.ID_Name_REVIT == "1368195" || b.ID_Name_REVIT == "1368199" || b.ID_Name_REVIT == "1368201")
                //                                                                                                            ).ToList();
                //ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla03_Info_Traslapos_Vigas = ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla03_Info_Traslapos_Vigas
                //                                                                                                            .Where(b => (b.ID_Name_REVIT == "1368197" || b.ID_Name_REVIT == "1368195" || b.ID_Name_REVIT == "1368199" || b.ID_Name_REVIT == "1368201") &&
                //                                                                                                            b.Fila_Barra == "SUP_1").ToList();
                //*****************************

                //ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla01_Info_Barras_Flexion = ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla01_Info_Barras_Flexion
                //                                                                                                            .Where(b => (b.ID_Name_REVIT == "1609018" || b.ID_Name_REVIT == "1609020")
                //                                                                                                            ).ToList();
                //ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla03_Info_Traslapos_Vigas = ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla03_Info_Traslapos_Vigas
                //                                                                                                            .Where(b => (b.ID_Name_REVIT == "1609018" || b.ID_Name_REVIT == "1609020") &&
                                                                                                                             //b.Fila_Barra == "SUP_1").ToList();




                if (!M0_GenerarListaVigas(ListaIntervalosBarraAutoDtoIMPORTAR)) return false;

                //GENERAR LISTA TRASLAPO
                _servicioOrdenTraslapo = new ServicioOrdenTraslapo(ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla03_Info_Traslapos_Vigas, ListaBarrasVigas_secciones);
                if (!_servicioOrdenTraslapo.Ejecutar()) return false;

                if (!M1_CrearLIstaCOnBarras(ListaIntervalosBarraAutoDtoIMPORTAR)) return false;

                //var ListaTOTALBArras = ListaVigaAutomaticoConBArras.SelectMany(c => c.ListaBArras).ToList();
                // UnirBarrasEntreVigasPOrIguakDIamtro(ListaTOTALBArras);

                if (!M1_2_MOdificarBarrasPorContinudadEnOtroViga()) return false;

                if (!M2_CrearLIstaCOnBarrasCORTE(ListaIntervalosBarraAutoDtoIMPORTAR)) return false;

                if (!M3_CrearLIstaGeometriaVigas(ListaIntervalosBarraAutoDtoIMPORTAR)) return false;

                if (!M4_BuscarIdem()) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener lista de barras desde json. ex:{ex.Message}");

                return false;
            }
            return true;
        }

        private bool M0_GenerarListaVigas(Tabla05_Objeto_con_todas_las_Tablas ListaIntervalosBarraAutoDtoIMPORTAR)
        {
            try
            {
                // //GENERAR BARRAS
                foreach (var obj in ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla01_Info_Barras_Flexion)
                {
                    var nuevaItem = new BarraFlexionTramosDTO(obj);
                    if (nuevaItem.Crear())
                        ListaBarrasVigas_secciones.Add(nuevaItem);
                }

                //borrarTraslapoIdem
//                ListaBarrasVigas_secciones = ListaBarrasVigas_secciones.Where(c => c.IdentiFIcadorParaTraslapo == "SUP_1").ToList();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear en Lista de vigas.\nEx: {ex.Message} ");
                return false;
            }
            return true;
        }

        private bool M1_CrearLIstaCOnBarras(Tabla05_Objeto_con_todas_las_Tablas ListaIntervalosBarraAutoDtoIMPORTAR)
        {
            string idVIgas = "";
            try
            {

                // var ListasTraslapoPOrVIgas = _newServicioOrdenTraslapo.ListaTraslapo;
                // creas lista de barras en vigas
                var listaGrupoViga_secciones = ListaBarrasVigas_secciones.GroupBy(c => new { IDNameREVIT = c.ID_Name_REVIT_Inicio, nombreEje = c.Eje_REVIT }).ToList();//quitar lsita
                foreach (var item in listaGrupoViga_secciones)
                {
                    var IdentificadorViga_revit = item.Key.IDNameREVIT;
                    idVIgas = IdentificadorViga_revit.ToString();
                    var nombreEje = item.Key.nombreEje; ;

                    var ListaBArras = item.GroupBy(c => new { ubicacion = c.Ubicacion_Armado, linea = c.Fila_Barra }).ToList();//quitarlist

                    // var ListaTraslapoEnviga = ListasTraslapoPOrVIgas.Where(vs => vs.ID_Name_REVIT == IdentificadorViga_revit.ToString()).ToList();

                    // crear lista barras
                    List<BarraFlexion> listaBArras = new List<BarraFlexion>();
                    foreach (var barras in ListaBArras)
                    {
                        TipoCaraObjeto ubicacion = barras.Key.ubicacion;
                        int linea = barras.Key.linea;
                        List<BarraFlexionTramosDTO> listaDebarrasPorLine = barras.Where(c => c.IsOk).OrderBy(c => c.Seccion_Inicio).ToList();

                        if (listaDebarrasPorLine.Count == 0) continue;

                        ServicioModificarCoordinaBArrasPorTraslapo _SMCB = new ServicioModificarCoordinaBArrasPorTraslapo();
                        ServicioConfigurarBarraRefuerzo SCBR = new ServicioConfigurarBarraRefuerzo(ListaBarrasVigas_secciones);

                        BarraFlexionTramosDTO barra = listaDebarrasPorLine[0];
                        barra = _SMCB.ModificarInicial(barra);

                        for (int i = 0; i <= listaDebarrasPorLine.Count - 1; i++)
                        {
                            if (barra == null) // barra incial de viga
                            {
                                barra = listaDebarrasPorLine[i];
                                barra = _SMCB.ModificarInicial(barra);
                            }

                            //esta en seccion final = 4
                            if (i == listaDebarrasPorLine.Count - 1)
                            {
                                if (listaDebarrasPorLine[i].Seccion_Inicio == 4)
                                    barra.IsBuscarContinudadEntreVigas = true;

                                if (barra.Seccion_Inicio == 4 || barra.Seccion_Inicio == 2)
                                    barra = SCBR.Analizar(barra);
                                else
                                    barra = _SMCB.ModificarFinal(barra);

                                barra = M1_1_CargarDAtos(IdentificadorViga_revit, listaBArras, ubicacion, linea, barra);
                                //BarraFlexion barraFlexion = new BarraFlexion(IdentificadorViga_revit, ubicacion, linea, barra);
                                //listaBArras.Add(barraFlexion);
                                //barra = null;
                                continue;
                            }
                            //barra sigueinte
                            var barraSiguiente = listaDebarrasPorLine[i + 1];

                            if (barra.diametro_Barras__mm != barraSiguiente.diametro_Barras__mm || barra.n_Barras != barraSiguiente.n_Barras)
                            {
                                barra = _SMCB.ModificarFinal(barra);
                                barra = M1_1_CargarDAtos(IdentificadorViga_revit, listaBArras, ubicacion, linea, barra);
                                //BarraFlexion barraFlexion = new BarraFlexion(IdentificadorViga_revit, ubicacion, linea, barra);
                                //listaBArras.Add(barraFlexion);
                                //barra = null;
                            }
                            else if (barra.CasosTraslapoDTO_FinTramo != null)
                            {
                                barra = _SMCB.ModificarFinal(barra);
                                barra = M1_1_CargarDAtos(IdentificadorViga_revit, listaBArras, ubicacion, linea, barra);
                                //BarraFlexion barraFlexion = new BarraFlexion(IdentificadorViga_revit, ubicacion, linea, barra);
                                //listaBArras.Add(barraFlexion);
                                //barra = null;
                            }
                            else if (Math.Abs(barra.Seccion_Inicio - barraSiguiente.Seccion_Inicio) > 1 && barra.p2_mm.GetXYZ().DistanceTo(barraSiguiente.p1_mm.GetXYZ()) > 10) // caso donde barra no son continuas
                            {
                                //ServicioConfigurarBarraRefuerzo SCBR = new ServicioConfigurarBarraRefuerzo(listaDebarrasPorLine);
                                barra = SCBR.Analizar(barra);
                                barra = M1_1_CargarDAtos(IdentificadorViga_revit, listaBArras, ubicacion, linea, barra);
                            }
                            else
                            {
                                barra.Seccion_Final = barraSiguiente.Seccion_Final;
                                barra.ID_Name_REVIT_Final = barraSiguiente.ID_Name_REVIT_Final;
                                barra.p2_mm = barraSiguiente.p2_mm;
                                barra.CasosTraslapoDTO_FinTramo = barraSiguiente.CasosTraslapoDTO_FinTramo;
                            }
                        }
                    }
                    var vigaAutomatico = new VigaAutomatico(IdentificadorViga_revit, nombreEje, listaBArras);
                    // vigaAutomatico.ASignarVIgaABarra();
                    ListaVigaAutomaticoConBArras.Add(vigaAutomatico);
                }



            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear barras en viga id:{idVIgas}.\nEx: {ex.Message} ");
                return false;
            }
            return true;
        }

        private BarraFlexionTramosDTO M1_1_CargarDAtos(int IdentificadorViga_revit, List<BarraFlexion> listaBArras, TipoCaraObjeto ubicacion, int linea, BarraFlexionTramosDTO barra)
        {
            BarraFlexion barraFlexion = new BarraFlexion(IdentificadorViga_revit, ubicacion, linea, barra);
            listaBArras.Add(barraFlexion);
            //  barra = null;
            return null;
        }


        //se debe modificar traslapo
        private bool M1_2_MOdificarBarrasPorContinudadEnOtroViga()
        {
            try
            {

                List<BarraFlexion> ListaTOTALBArras = ListaVigaAutomaticoConBArras.SelectMany(c => c.ListaBArras).ToList();
                // unir barras entre vigas con traslapo
                for (int i = 0; i < ListaVigaAutomaticoConBArras.Count; i++)
                {
                    VigaAutomatico vigaActual = ListaVigaAutomaticoConBArras[i];

                    for (int j = 0; j < vigaActual.ListaBArras.Count; j++)
                    {
                        BarraFlexion _BarraFlexion = vigaActual.ListaBArras[j];

                        if (!_BarraFlexion.IsOK) continue;
                        if (_BarraFlexion.BarraFlexionTramosDTO_.CasosTraslapoDTO_FinTramo != null) continue;
                        if (!_BarraFlexion.BarraFlexionTramosDTO_.IsBuscarContinudadEntreVigas) continue;

                        double largoTraslapo_mm = Util.FootToMm(UtilBarras.largo_traslapoFoot_diamMM(_BarraFlexion.BarraFlexionTramosDTO_.diametro_Barras__mm));

                        var ListabarraCOntinuidad = ListaTOTALBArras.Where(c => c.IsOK && c.Nombreviga != _BarraFlexion.Nombreviga &&
                                                      Math.Abs(c.BarraFlexionTramosDTO_.p1_mm.Z - _BarraFlexion.BarraFlexionTramosDTO_.p2_mm.Z) < 10 && // como esta en mm y por la direcian de diamtro puedes esar desplazados los diametros algunao milimetros
                                                     AyudaTraslapoIguales.EstanTraslapadas(_BarraFlexion.BarraFlexionTramosDTO_, c.BarraFlexionTramosDTO_, largoTraslapo_mm)
                                                     ).OrderBy(c => c.BarraFlexionTramosDTO_.Seccion_Inicio).ToList();

                        if (ListabarraCOntinuidad.Count == 0) continue;
                        var barraCOntinuidad = ListabarraCOntinuidad[0];

                        if (barraCOntinuidad == null) continue;

                        barraCOntinuidad.IsOK = false;

                        //_BarraFlexion.BarraFlexionTramosDTO_.ID_Name_REVIT = barraCOntinuidad.BarraFlexionTramosDTO_.ID_Name_REVIT;// simpre que referenciado ala viga de la izquierda

                        _BarraFlexion.BarraFlexionTramosDTO_.TipoPataDereSup = barraCOntinuidad.BarraFlexionTramosDTO_.TipoPataDereSup;
                        _BarraFlexion.BarraFlexionTramosDTO_.p2_mm = XYZnh.ObtenerCOpia(barraCOntinuidad.BarraFlexionTramosDTO_.p2_mm);
                        _BarraFlexion.BarraFlexionTramosDTO_.CasosTraslapoDTO_FinTramo = barraCOntinuidad.BarraFlexionTramosDTO_.CasosTraslapoDTO_FinTramo;
                     
                        _BarraFlexion.BarraFlexionTramosDTO_.Seccion_Final = barraCOntinuidad.BarraFlexionTramosDTO_.Seccion_Final;
                        _BarraFlexion.BarraFlexionTramosDTO_.ID_Name_REVIT_Final = barraCOntinuidad.BarraFlexionTramosDTO_.ID_Name_REVIT_Final;
                        // cambiarlista tralapo
                        var resultViga=_servicioOrdenTraslapo.ListaTraslapo.Where(c => c.ID_Name_REVIT== barraCOntinuidad.BarraFlexionTramosDTO_.ID_Name_REVIT_Inicio.ToString()).FirstOrDefault();
                                                                                // c.ListaCasosTraslapos.Exists(r => r.BarraTramosAnterior.CasosTraslapoDTO_InicioTramo?.BarraTramosAnterior.ID_Name_REVIT == barraCOntinuidad.BarraFlexionTramosDTO_.ID_Name_REVIT)).FirstOrDefault();
                        if (resultViga != null)
                        {
                            var result=resultViga.ListaCasosTraslapos.Where(c=>c.BarraTramosAnterior?.ID_Name_REVIT_Inicio== barraCOntinuidad.BarraFlexionTramosDTO_.ID_Name_REVIT_Inicio &&
                                                                               c.BarraTramosAnterior?.IdentiFIcadorParaTraslapo == barraCOntinuidad.BarraFlexionTramosDTO_.IdentiFIcadorParaTraslapo &&
                                                                               c.BarraTramosAnterior?.Seccion_Final== barraCOntinuidad.BarraFlexionTramosDTO_.Seccion_Final).FirstOrDefault();
                            if (result != null)
                            {
                                result.BarraTramosAnterior = _BarraFlexion.BarraFlexionTramosDTO_;
                            }
                        }

                        j = j - 1;

                    }
                }


                //borrar Barras Isok=false
                for (int i = 0; i < ListaVigaAutomaticoConBArras.Count; i++)
                {
                    VigaAutomatico vigaActual = ListaVigaAutomaticoConBArras[i];
                    vigaActual.ListaBArras = vigaActual.ListaBArras.Where(c => c.IsOK).ToList();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'Modificar Barras Por Continudad En Otro Viga'.\nEx: {ex.Message} ");
                return false;
            }
            return true;
        }

        private bool M2_CrearLIstaCOnBarrasCORTE(Tabla05_Objeto_con_todas_las_Tablas ListaIntervalosBarraAutoDtoIMPORTAR)
        {
            string idVIgas = "";
            try
            {
                //GENERAR BARRAS
                foreach (Tabla02_Info_Barras_Corte obj in ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla02_Info_Barras_Corte)
                {
                    var nuevaItem = new BarraCorteTramos(obj);
                    if (nuevaItem.Crear())
                        ListaEstriboVigas_secciones.Add(nuevaItem);
                }

                // creas lista de barras en vigas
                var listaGrupoViga_secciones = ListaEstriboVigas_secciones.GroupBy(c => new { IDNameREVIT = c.ID_Name_REVIT, nombreEje = c.Eje_REVIT }).ToList();//quitar lsita
                foreach (var item in listaGrupoViga_secciones)
                {
                    var IdentificadorViga_revit = item.Key.IDNameREVIT;
                    idVIgas = IdentificadorViga_revit.ToString();
                    var nombreEje = item.Key.nombreEje; ;

                    List<BarraCorteTramos> listaDebarrasPorLine = item.Where(c => c.IsOk).OrderBy(c => c.Seccion).ToList();//quitarlist

                    if (listaDebarrasPorLine.Count == 0) continue;

                    for (int i = 0; i < listaDebarrasPorLine.Count; i++)
                    {
                        BarraCorteTramos estriboActual = listaDebarrasPorLine[i];
                        if (estriboActual.configuracionInicialEstriboDTO.DiamtroEstriboMM == 0) continue;

                        var vigaContenedor = ListaVigaAutomaticoConBArras.Where(c => c.IdentificadorViga_revit == estriboActual.ID_Name_REVIT).FirstOrDefault();

                        if (vigaContenedor == null) continue;
                        if (vigaContenedor.ListaEstribos.Exists(c => c.Seccion == estriboActual.Seccion)) continue;

                        vigaContenedor.ListaEstribos.Add(estriboActual);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear barras en viga id:{idVIgas}.\nEx: {ex.Message} ");
                return false;
            }
            return true;
        }

        private bool M3_CrearLIstaGeometriaVigas(Tabla05_Objeto_con_todas_las_Tablas ListaIntervalosBarraAutoDtoIMPORTAR)
        {
            try
            {
                foreach (var obj in ListaIntervalosBarraAutoDtoIMPORTAR.Lista_Tabla04_Info_Geometria_Vigas)
                {
                    VigaGeometriaDTO nuevaItem = new VigaGeometriaDTO(obj);
                    if (!nuevaItem.Crear()) continue;

                    ListaVigas.Add(nuevaItem);

                    var vigaencontrada = ListaVigaAutomaticoConBArras.Where(c => c.IdentificadorViga_revit.ToString() == nuevaItem.ID_Name_REVIT).FirstOrDefault();

                    if (vigaencontrada == null) continue;

                    vigaencontrada.VigaGeometriaDTO = nuevaItem;
                }
                // reajustar barras de refuerzo
                ListaVigaAutomaticoConBArras.ForEach(c => c.ReAjustarBarras(ListaVigas));
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearLIstaGeometriaVigas'. ex{ex.Message}");
                return false;
            }
            return true;
        }

        private bool M4_BuscarIdem()
        {
            try
            {
                //A) asigna barras flexion
                foreach (VigaAutomatico _viga in ListaVigaAutomaticoConBArras)
                {
                    if (_viga.VigaIdem == "None") continue;

                    var vigaEncontrda = ListaVigaAutomaticoConBArras.Where(c => c.VigaGeometriaDTO.Unique_Name_ETABS == _viga.VigaIdem).FirstOrDefault();
                    if (vigaEncontrda == null) continue;

                    //asignar barras

                    EjecutarLinea(_viga, vigaEncontrda, "SUP_1");
                    EjecutarLinea(_viga, vigaEncontrda, "SUP_2");
                    EjecutarLinea(_viga, vigaEncontrda, "SUP_3");
                    EjecutarLinea(_viga, vigaEncontrda, "SUP_4");

                    EjecutarLinea(_viga, vigaEncontrda, "INF_1");
                    EjecutarLinea(_viga, vigaEncontrda, "INF_2");
                    EjecutarLinea(_viga, vigaEncontrda, "INF_3");
                    EjecutarLinea(_viga, vigaEncontrda, "INF_4");
                }

                //B) asigna ESTRIBO corte
                foreach (VigaAutomatico _viga in ListaVigaAutomaticoConBArras)
                {
                    if (_viga.VigaIdem == "None") continue;

                    var vigaEncontrda = ListaVigaAutomaticoConBArras.Where(c => c.VigaGeometriaDTO.Unique_Name_ETABS == _viga.VigaIdem).FirstOrDefault();
                    if (vigaEncontrda == null) continue;

                    EjecutarEstribo(_viga, vigaEncontrda);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al buscar barras idem. ex{ex.Message}");
                return false;
            }
            return true;
        }

        private static bool EjecutarLinea(VigaAutomatico _viga, VigaAutomatico vigaEncontrda, string linea)
        {

            try
            {
                List<BarraFlexion> ListaBarras_supX = _viga.ListaBArras.Where(c => c.BarraFlexionTramosDTO_.IdentiFIcadorParaTraslapo == linea).OrderBy(r => r.BarraFlexionTramosDTO_.Seccion_Inicio).ToList();
                List<BarraFlexion> ListaBarrasIdem_supX = vigaEncontrda.ListaBArras.Where(c => c.BarraFlexionTramosDTO_.IdentiFIcadorParaTraslapo == linea).OrderBy(r => r.BarraFlexionTramosDTO_.Seccion_Inicio).ToList();

                if (ListaBarras_supX.Count != ListaBarrasIdem_supX.Count)
                {
                    Util.ErrorMsg("Error, cantidad de BARRAS en contradas  en asignar vigas idem son dististas. No se puede asignar BARRAS");
                    return false;
                }

                if (ListaBarras_supX.Count == 0)
                    return true;
                else if (ListaBarras_supX.Count == 1)
                {
                    ListaBarras_supX[0].BarraIdem = ListaBarrasIdem_supX[0];
                }
                else if (ListaBarras_supX.Count == 2)
                {
                    ListaBarras_supX[0].BarraIdem = ListaBarrasIdem_supX[0];
                    ListaBarras_supX[1].BarraIdem = ListaBarrasIdem_supX[1];
                }
                else if (ListaBarras_supX.Count > 2)
                {
                    Util.ErrorMsg("NO PORGRAMADO, se encuentras mas de dos BARRAS por linea de viga, caso no progrmado, revisar.");

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al asignar BARRAS en vigas idem  vigaID:{_viga.IdentificadorViga_revit}.\nex:{ex.Message}");
                return false;
            }
            return true;
        }

        private static bool EjecutarEstribo(VigaAutomatico _viga, VigaAutomatico vigaEncontrda)
        {

            try
            {
                List<BarraCorteTramos> ListaBarras_supX = _viga.ListaEstribos.OrderBy(r => r.LargoEstriboEnbarra).ToList();
                List<BarraCorteTramos> ListaBarrasIdem_supX = vigaEncontrda.ListaEstribos.OrderBy(r => r.LargoEstriboEnbarra).ToList();

                if (ListaBarras_supX.Count != ListaBarrasIdem_supX.Count)
                {
                    Util.ErrorMsg("Error, cantidad de estribos encontrados en asignar vigas idem son dististas. No se puede asignar ESTRIBO");
                    return false;
                }

                if (ListaBarras_supX.Count == 0)
                    return true;
                else if (ListaBarras_supX.Count == 1)
                {
                    ListaBarras_supX[0].EstriboIdem = ListaBarrasIdem_supX[0];
                }
                else if (ListaBarras_supX.Count == 2)
                {
                    ListaBarras_supX[0].EstriboIdem = ListaBarrasIdem_supX[0];
                    ListaBarras_supX[1].EstriboIdem = ListaBarrasIdem_supX[1];
                }
                else if (ListaBarras_supX.Count == 3)
                {
                    ListaBarras_supX[0].EstriboIdem = ListaBarrasIdem_supX[0];
                    ListaBarras_supX[1].EstriboIdem = ListaBarrasIdem_supX[1];
                    ListaBarras_supX[2].EstriboIdem = ListaBarrasIdem_supX[2];
                }
                else if (ListaBarras_supX.Count == 4)
                {
                    ListaBarras_supX[0].EstriboIdem = ListaBarrasIdem_supX[0];
                    ListaBarras_supX[1].EstriboIdem = ListaBarrasIdem_supX[1];
                    ListaBarras_supX[2].EstriboIdem = ListaBarrasIdem_supX[2];
                    ListaBarras_supX[3].EstriboIdem = ListaBarrasIdem_supX[3];
                }
                else if (ListaBarras_supX.Count > 4)
                {
                    Util.ErrorMsg("NO PROGRAMADO, se encuentras mas de 3 ESTRIBOS por viga, revisar.");

                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al asignabar ESTRIBO en vigas idem vigaID:{_viga.IdentificadorViga_revit}.\nex:{ex.Message}");
                return false;
            }
            return true;
        }
    }
}
