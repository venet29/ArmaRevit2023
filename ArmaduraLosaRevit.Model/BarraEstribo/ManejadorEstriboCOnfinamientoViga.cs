
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Seleccion;
using ArmaduraLosaRevit.Model.BarraEstribo.TAG;
using ArmaduraLosaRevit.Model.Enumeraciones;

using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.BarraEstribo.Barras;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga;

namespace ArmaduraLosaRevit.Model.BarraEstribo
{
    class ManejadorEstriboCOnfinamientoViga : AManejadorEstriboCOnfinamiento
    {
        private List<BarraCorteTramos> _ListaBarraCorteTramosDTO;
       // private readonly List<DatosConfinamientoAutoDTO> listaConfiguracionInicialEstriboDTO;

        public GenerarDatosIniciales_Service generarDatosIniciales_Service { get; private set; }
        public ManejadorEstriboCOnfinamientoViga(UIApplication uiapp, List<BarraCorteTramos> ListaBarraCorteTramosDTO) : base(uiapp, null)
        {
            _ListaBarraCorteTramosDTO = ListaBarraCorteTramosDTO;

           // listaConfiguracionInicialEstriboDTO = ListaBarraCorteTramosDTO.Select(c => c.configuracionInicialEstriboDTO).ToList();
        }



        public bool M1_Ejecutar()
        {
            _listaRebarIdCambiarColor.Clear();

            #region Validar

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorEstriboCOnfinamiento");

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
            #endregion

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            try
            {
                //NOTA: BORRRA EL  -3
                for (int i = 0; i < _ListaBarraCorteTramosDTO.Count; i++)
                {
                    var BarraCorteDTO= _ListaBarraCorteTramosDTO[i];

                    _configuracionInicialEstriboDTO = BarraCorteDTO.configuracionInicialEstriboDTO;

                    _configuracionEnfierrado = ObtenerConfiguracionEnfierrado_Service.obtener(_configuracionInicialEstriboDTO);

                    generarDatosIniciales_Service = new GenerarDatosIniciales_Service(_uiapp, TipoEstriboGenera.EConfinamiento, _configuracionInicialEstriboDTO);
                    if (!generarDatosIniciales_Service.M1_GeneralDatosIniciales()) return false;

                    if (!CambiarLargoVigas.Ejecutar(generarDatosIniciales_Service, BarraCorteDTO)) return false;
                    generarDatosIniciales_Service = CambiarLargoVigas.Recal_CambiarLargoVigas;

                    List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboMuro = new List<BarraRefuerzoEstriboMuroSinTras>();
                    List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboLateralMuro = new List<BarraRefuerzoEstriboMuroSinTras>();
                    List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboTrabasMuro = new List<BarraRefuerzoEstriboMuroSinTras>();

                    if (generarDatosIniciales_Service.resul_EstriboMuroDTO.Count == 0) return true;

                    using (TransactionGroup tt = new TransactionGroup(_doc))
                    {
                        tt.Start("Dibujar estribo-NH");

                        try
                        {
                            using (Transaction t = new Transaction(_doc))
                            {
                                t.Start("Dibujar estribo-NHa");
                                foreach (EstriboMuroDTO item in generarDatosIniciales_Service.resul_EstriboMuroDTO)
                                {
                                    //4) ejecutar
                                    if (_configuracionInicialEstriboDTO.IsEstribo == true)
                                    {
                                        BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboMuro = m1_1_GEnerarBArrasSinTras(generarDatosIniciales_Service, item);
                                        if (barraRefuerzoEstriboMuro != null)
                                        {
                                            ListBarraRefuerzoEstriboMuro.Add(barraRefuerzoEstriboMuro);
                                            BarraCorteDTO.RebarEstribo = barraRefuerzoEstriboMuro.EstriboRebarCreado;
                                        }
                                        else
                                            Util.ErrorMsg("Error al crear estribo");
                                    }

                                    //laterales
                                    if (_configuracionInicialEstriboDTO.IsLateral == true)
                                    {
                                        BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboLateralMuro = M1_2_GenerarLAteralesGeneralSinTrans(generarDatosIniciales_Service, item);
                                        if (barraRefuerzoEstriboLateralMuro != null)
                                        {
                                            ListBarraRefuerzoEstriboLateralMuro.Add(barraRefuerzoEstriboLateralMuro);
                                            BarraCorteDTO.ListaLateralesRebar = ListaLateralesRebarIdCreados;
                                        }
                                        else
                                            Util.ErrorMsg("Error al crear Laterales");
                                    }

                                    //traba
                                    if (_configuracionInicialEstriboDTO.IsTraba == true)
                                    {

                                        BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboTrabasMuro = M1_3_GenerarTrabasGeneralSinTrans(generarDatosIniciales_Service, item);
                                        if (barraRefuerzoEstriboTrabasMuro != null)
                                        {
                                            ListBarraRefuerzoEstriboTrabasMuro.Add(barraRefuerzoEstriboTrabasMuro);
                                            BarraCorteDTO.ListaTrabasRebar = ListaTrabasRebarIdCreadas;
                                        }
                                        else
                                            Util.ErrorMsg("Error al crear Trabas");

                                    }
                                }
                                t.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            tt.RollBack();
                            string msj = ex.Message;
                            return false;
                        }

                        //******************************************
                        try
                        {
                            using (Transaction trans = new Transaction(_doc))
                            {
                                trans.Start("Dibujar 2parte-NH");

                                foreach (var item in ListBarraRefuerzoEstriboMuro)
                                {
                                    item.M1_GenerarBarra_2parte();
                                    item.DibujarTagsEstribo(generarDatosIniciales_Service._configuracionTAgEstriboDTo);
                                }
                                foreach (var item in ListBarraRefuerzoEstriboLateralMuro)
                                {
                                    item.GenerarLaterales_2parte();
                                }
                                foreach (var item in ListBarraRefuerzoEstriboTrabasMuro)
                                {
                                    item.GenerarTrabas_2parte();
                                }

                                CambiarColorBarras_Service cambiarColorBarras_Service = new CambiarColorBarras_Service(_uiapp, _configuracionInicialEstriboDTO.tipoConfiguracionEstribo);
                                cambiarColorBarras_Service.M1_2_CAmbiarColor_sintrans(_listaRebarIdCambiarColor);

                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            tt.RollBack();
                            string msj = ex.Message;
                            return false;

                        }
                        tt.Assimilate();
                    }
                }
                //CREAR GRUPO
                //M1_4_CrearGrupo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                return false;
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return true;
        }



        // solo para caso de aramdura de vigas automatico, cuando nos quiere enfierrar estribos
        //se necesitan estos calculos previos para dibujar las barras 
        public bool M1_EjecutarSoloCAlculos()
        {
            _listaRebarIdCambiarColor.Clear();

            #region Validar

            ManejadorDatos _ManejadorUsuarios = new ManejadorDatos();
            bool resultadoConexion = _ManejadorUsuarios.PostBitacora("CARGAR ManejadorEstriboCOnfinamiento");

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
            #endregion

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            try
            {
                //NOTA: BORRRA EL  -3
                for (int i = 0; i < _ListaBarraCorteTramosDTO.Count; i++)
                {
                    var BarraCorteDTO = _ListaBarraCorteTramosDTO[i];

                    _configuracionInicialEstriboDTO = BarraCorteDTO.configuracionInicialEstriboDTO;

                    _configuracionEnfierrado = ObtenerConfiguracionEnfierrado_Service.obtener(_configuracionInicialEstriboDTO);

                    generarDatosIniciales_Service = new GenerarDatosIniciales_Service(_uiapp, TipoEstriboGenera.EConfinamiento, _configuracionInicialEstriboDTO);
                    if (!generarDatosIniciales_Service.M1_GeneralDatosIniciales()) return false;

                    if (!CambiarLargoVigas.Ejecutar(generarDatosIniciales_Service, BarraCorteDTO)) return false;
                    generarDatosIniciales_Service = CambiarLargoVigas.Recal_CambiarLargoVigas;

                    List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboMuro = new List<BarraRefuerzoEstriboMuroSinTras>();
                    List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboLateralMuro = new List<BarraRefuerzoEstriboMuroSinTras>();
                    List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboTrabasMuro = new List<BarraRefuerzoEstriboMuroSinTras>();

                    if (generarDatosIniciales_Service.resul_EstriboMuroDTO.Count == 0) return true;


                }
                //CREAR GRUPO
                //M1_4_CrearGrupo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                return false;
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return true;
        }
    }
}
