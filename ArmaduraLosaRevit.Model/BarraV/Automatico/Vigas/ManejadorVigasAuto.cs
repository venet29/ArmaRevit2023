using ArmaduraLosaRevit.Model.BarraEstribo;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;
using ArmaduraLosaRevit.Model.BarraV.Automatico.WPFAuto;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Traslapos;
using ArmaduraLosaRevit.Model.BarraV.Traslapos.model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.Vigas
{
    // archivo incial  J:\_revit\PROYECTO\2022\2022-022-talca\reportevigas

    public class ManejadorVigasAuto
    {
        private readonly UIApplication _uiapp;
        private readonly Ui_AutoElev ui_Barrav;
        private View _view;
        private Document _doc;
        private List<VigaAutomatico> ListaVigaAutomatico;
        private ManejadorBarraHVigaAuto _ManejadorBarraHVigaAuto;
        public List<RebarTraslapo_paraDubujarTraslapoDimensionDTO> listaCOntendoraFinalParaTraslapo { get; set; }
        // public List<TraslapoDTO> ListaTraslapo { get; set; }
        private UtilStopWatch _utilStopWatch;
        Stopwatch timeMeasure = new Stopwatch();
        bool IsdibujarTraslapo;
        protected ManejadorTraslapo _manejadorTraslapo;

        public ManejadorVigasAuto(UIApplication uiapp, ref Ui_AutoElev ui_Barrav)
        {
            this._uiapp = uiapp;
            this.ui_Barrav = ui_Barrav;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._doc = uiapp.ActiveUIDocument.Document;
            _manejadorTraslapo = new ManejadorTraslapo(uiapp);
            listaCOntendoraFinalParaTraslapo = new List<RebarTraslapo_paraDubujarTraslapoDimensionDTO>();
            IsdibujarTraslapo = true;
        }


        public bool ImportarBarrasViga()
        {
            timeMeasure.Start();
            bool result = false;
            bool IsNotificacionAnitguo = UtilBarras.IsConNotificaciones;
            try
            {
                UtilBarras.ContadorMAyor12mt = 0;
                UtilBarras.IsConNotificaciones = false;

                ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
                bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR EjecutarImportacion VIGAS automatico ");

                if (!resultadoConexion)
                {
                    Util.ErrorMsg(ConstNH.ERROR_CONEXION);
                    return false;
                }
                else if (!_ManejadorUsuarios.ObteneResultado())
                {
                    Util.ErrorMsg(ConstNH.ERROR_OBTENERDATOS);
                    return false;
                }



                ListaVigaAutomatico = new List<VigaAutomatico>();

                if (_view.ViewType != ViewType.Section || (!Directory.Exists(ConstNH.CONST_COT)))
                {
                    Util.ErrorMsg("Comando Se debe ejecutar en una SeccionView");
                    return false;
                }

                if (!M1_cargarDatos()) return false;

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("DibujarImportarBarrasVigas-NH");

                    M2_DibujarBarrasDeViga(ListaVigaAutomatico.Where(c => c.VigaIdem.ToLower() == "none").ToList());

                    var ListavigasIDem = ListaVigaAutomatico.Where(c => c.VigaIdem.ToLower() != "none").ToList();

                    M2_DibujarBarrasDeViga(ListavigasIDem);

                    M3_AignarPArametrosBarras(ListavigasIDem);

                    M4_DibujarTraslaposRojos();

                    t.Assimilate();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ImportarBarrasViga. ex:{ex.Message}");
                result = false;
            }

            TimeSpan ts = timeMeasure.Elapsed;
            timeMeasure.Stop();
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            UtilBarras.IsConNotificaciones = IsNotificacionAnitguo;


            if (UtilBarras.ContadorMAyor12mt > 0)
            {
                Util.InfoMsg($"Proceso Terminado, duracion de enfierrado: {elapsedTime}\n Barras enfierradas:{ListaVigaAutomatico.SelectMany(c => c.ListaBArras).Count()}" +
               $"\n Estribos enfierradas:{ListaVigaAutomatico.SelectMany(c => c.ListaEstribos).Count()} " +
               $"\n Estribos enfierradas:{ListaVigaAutomatico.SelectMany(c => c.ListaEstribos.SelectMany(l => l.ListaLateralesRebar)).Count()} " +
               $"\n\nSe encontraron {UtilBarras.ContadorMAyor12mt} barras mayores de 12mt, estan represnetadas con color amarillos");
                UtilBarras.ContadorMAyor12mt = 0;
            }
            else
            {
                Util.InfoMsg($"Proceso Terminado, duracion de enfierrado: {elapsedTime}\n Barras enfierradas:{ListaVigaAutomatico.SelectMany(c => c.ListaBArras).Count()}" +
                    $"\n Estribos enfierradas:{ListaVigaAutomatico.SelectMany(c => c.ListaEstribos).Count()} " +
                    $"\n Estribos enfierradas:{ListaVigaAutomatico.SelectMany(c => c.ListaEstribos.SelectMany(l => l.ListaLateralesRebar)).Count()} ");
            }



            return result;
        }
        private bool M1_cargarDatos()
        {
            try
            {
                var nombrearchivo1 = @"\\SERVER-CDV\Dibujo2\Proyectos\SaveRevit\2020\2020-033 Libertad_D-2020-033_EDFICIO LIBERTAD_20230209123709\DatosIng\_ARM_Vigas.TXT";

                if (!AdministrarFile.LeerArchivoJsonING()) return false;
                nombrearchivo1 = AdministrarFile.RutaArchivo;

                var _RepositoryAceroElecacion = new RepositoryAceroElecacionVigas(nombrearchivo1);
                if (!_RepositoryAceroElecacion.ObtenerBArras_Info_Barras_Flexion_json()) return false;

                ListaVigaAutomatico = _RepositoryAceroElecacion.ListaVigaAutomaticoConBArras;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cargar archivo. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private void M2_DibujarBarrasDeViga(List<VigaAutomatico> ListaVigaAutomatico_Reducida)
        {
            for (int i = 0; i < ListaVigaAutomatico_Reducida.Count; i++)
            {
                Debug.WriteLine($"barra {i} de {ListaVigaAutomatico_Reducida.Count}");
                VigaAutomatico viga = ListaVigaAutomatico_Reducida[i];
                Element _viga = _doc.GetElement(new ElementId(viga.IdentificadorViga_revit));
                viga.ListaEstribos.ForEach(c => c.configuracionInicialEstriboDTO.ElementoSeleccionado = _viga);

                ManejadorEstriboCOnfinamientoViga manejadorEstriboCOnfinamiento = new ManejadorEstriboCOnfinamientoViga(_uiapp, viga.ListaEstribos.Take(1).ToList());

                if (ui_Barrav.itemEstriboH.IsChecked == true)
                {
                    //crear estribos
                    manejadorEstriboCOnfinamiento.M1_Ejecutar();
                }
                else
                    manejadorEstriboCOnfinamiento.M1_EjecutarSoloCAlculos();

                if (ui_Barrav.itemBarraH.IsChecked == true)
                {
                    // para crear barras Horizontal auto
                    M2_1_CrearBarrarHorizontales(manejadorEstriboCOnfinamiento, viga);
                    listaCOntendoraFinalParaTraslapo.AddRange(_ManejadorBarraHVigaAuto.listaCOntendoraFinalParaTraslapo);
                }
            }
        }
        private void M2_1_CrearBarrarHorizontales(ManejadorEstriboCOnfinamientoViga manejadorEstriboCOnfinamiento, VigaAutomatico viga)
        {
            ConfiguracionInicialBarraHorizontalDTO confiEnfierrado_BarrasSUperiorDTO = new ConfiguracionInicialBarraHorizontalDTO()
            {
                IsDibujarBArra = true,
                Inicial_Cantidadbarra = "2",
                incial_diametroMM = 10,
                Inicial_espacienmietoCm_direccionmuro = "8",
            };

            ISeleccionarNivel _seleccionarNivel = new SeleccionarNivel(_uiapp);

            double deltaElevacionBasePoint = AyudaObtenerDeltaElevacion.ObtenerDeltaElevation(_uiapp);
            ArmaduraTrasformada ArmaduraTrasformada = new ArmaduraTrasformada(_view, deltaElevacionBasePoint);

            _ManejadorBarraHVigaAuto = new ManejadorBarraHVigaAuto(_uiapp, _seleccionarNivel, confiEnfierrado_BarrasSUperiorDTO, ArmaduraTrasformada);

            XYZ ptocentrocara = manejadorEstriboCOnfinamiento.generarDatosIniciales_Service._SeleccionPtosEstriboViga_sinSeleccionBarras._ptoSeleccionMouseCentroCaraMuro;
            Element VigaSeleccion = manejadorEstriboCOnfinamiento.generarDatosIniciales_Service._SeleccionPtosEstriboViga_sinSeleccionBarras._ElemetSelect;

            //_ManejadorBarraHVigaAuto.CrearBArraHorizontalVigaAuto(confiEnfierrado_BArrasInferiorDTO, ptocentrocara, VigaSeleccion);
            _ManejadorBarraHVigaAuto.CrearBArraHorizontalVigaAuto_conviga(viga, ptocentrocara, VigaSeleccion);
        }


        private void M3_AignarPArametrosBarras(List<VigaAutomatico> ListavigasIDem)
        {
            int contador = 0;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("AgregarPArametrosREbarIdem");

                    for (int i = 0; i < ListavigasIDem.Count; i++)
                    {
                        contador += 1;
                        var viga = ListavigasIDem[i];

                        //asiganr itema abarra
                        foreach (var item in viga.ListaBArras)
                        {
                            if (item.BarraIdem == null) continue;
                            AsignarPArametros((Rebar)item.BarraCreadoElem, (Rebar)item.BarraIdem?.BarraCreadoElem);
                        }

                        //asignar  aestribos
                        foreach (var item in viga.ListaEstribos)
                        {
                            if (item.EstriboIdem == null) continue;
                            var EstriboIDem = item.EstriboIdem.RebarEstribo;
                            var listaLateralesIDem = item.EstriboIdem.ListaLateralesRebar;
                            var listaTrabasRebarIdem = item.EstriboIdem.ListaTrabasRebar;

                            //estribo
                            AsignarPArametros(item.RebarEstribo, EstriboIDem);

                            //laterales
                            if (listaLateralesIDem.Count > 0)
                            {
                                var lateralIDem = _doc.GetElement(listaLateralesIDem[0]) as Rebar;
                                for (int j = 0; j < item.ListaLateralesRebar.Count; j++)
                                {
                                    var lateralActual = _doc.GetElement(item.ListaLateralesRebar[j]) as Rebar;
                                    AsignarPArametros(lateralActual, lateralIDem);
                                }
                            }
                            //traba
                            if (listaTrabasRebarIdem.Count > 0)
                            {
                                var trabaIDem = _doc.GetElement(listaTrabasRebarIdem[0]) as Rebar;
                                for (int j = 0; j < item.ListaTrabasRebar.Count; j++)
                                {
                                    var tarbaActual = _doc.GetElement(item.ListaTrabasRebar[j]) as Rebar;
                                    AsignarPArametros(tarbaActual, trabaIDem);
                                }
                            }

                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
        }

        private static void AsignarPArametros(Rebar elemtActual, Rebar elemtIdem)
        {
            if (elemtActual == null) return;
            if (elemtIdem == null) return;

            var idBrarraoriginal = elemtIdem.Id.IntegerValue;

            if (elemtActual.GetHostId().IntegerValue == 1369456)
            {
                var listaEleme = elemtActual.GetShapeDrivenAccessor().ComputeDrivingCurves();
            }

            bool resultIdBarraCopiar = ParameterUtil.SetParaIntNH(elemtActual, "IdBarraCopiar", idBrarraoriginal);
            return;
        }

        private void M4_DibujarTraslaposRojos()
        {
            if (IsdibujarTraslapo)
            {
                for (int i = 0; i < listaCOntendoraFinalParaTraslapo.Count; i++)
                {
                    var barra1 = listaCOntendoraFinalParaTraslapo[i];
                    barra1.BUscarInicio(listaCOntendoraFinalParaTraslapo);
                    barra1.BUscarFin(listaCOntendoraFinalParaTraslapo);
                }


                // obten  rebar incial y final para crear traslapo rojo
                foreach (var envoltorio in listaCOntendoraFinalParaTraslapo)
                {
                    if (!envoltorio.Isok) continue;
                    try
                    {
                        //inicial
                        //CasosTraslapoDTO barrarTraslapoAnterior = envoltorio.TraslaInicio;
                        //if (barrarTraslapoAnterior != null)
                        //{
                        if (envoltorio?.BarraAnterior_Inicial != null && envoltorio?.BarrasPosterior_Inicial != null)
                        {
                            // barrarTraslapoAnterior.BarraTramosAnterior.IsOk = false;
                            //barrarTraslapoAnterior.BarraTramosPosterior.IsOk = false;
                            // codo para  obtener pares de barras.i
                            TraslapoDTO newTraslapoDTO = new TraslapoDTO();

                            newTraslapoDTO.Id_RebarFinalRef = envoltorio.BarrasPosterior_Inicial.Id.IntegerValue;
                            newTraslapoDTO.RebarFinal = envoltorio.BarrasPosterior_Inicial;
                            newTraslapoDTO.Id_RebarInicialRef = envoltorio.BarraAnterior_Inicial.Id.IntegerValue;
                            newTraslapoDTO.RebarInicial = envoltorio.BarraAnterior_Inicial;

                            newTraslapoDTO.IsOK = true;
                            envoltorio.Isok = false;

                            _manejadorTraslapo.ListaTraslapo.Add(newTraslapoDTO);
                        };
                        //}
                        //final
                        CasosTraslapoDTO barrarTraslapoPosteriro = envoltorio.TraslaFinal;
                        //if (barrarTraslapoPosteriro != null)
                        //{
                        if (envoltorio?.BarraAnterior_Final != null && envoltorio?.BarrasPosterior_Final != null)
                        {
                            //barrarTraslapoPosteriro.BarraTramosPosterior.IsOk = false;
                            //barrarTraslapoPosteriro.BarraTramosAnterior.IsOk = false;
                            // codo para  obtener pares de barras
                            TraslapoDTO newTraslapoDTO = new TraslapoDTO();

                            newTraslapoDTO.Id_RebarFinalRef = envoltorio.BarrasPosterior_Final.Id.IntegerValue;
                            newTraslapoDTO.RebarFinal = envoltorio.BarrasPosterior_Final;
                            newTraslapoDTO.Id_RebarInicialRef = envoltorio.BarraAnterior_Final.Id.IntegerValue;
                            newTraslapoDTO.RebarInicial = envoltorio.BarraAnterior_Final;

                            newTraslapoDTO.IsOK = true;
                            envoltorio.Isok = false;

                            _manejadorTraslapo.ListaTraslapo.Add(newTraslapoDTO);
                        }
                        //}

                    }
                    catch (Exception ex)
                    {
                        Util.ErrorMsg($"Error al generar tralaspo ROJO.   ex:{ex.Message}");

                    }
                }


                _manejadorTraslapo.M4_DibujarTraslapoConTrans();
            }
        }



    }
}
