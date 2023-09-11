
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraEstribo.Barras;
using ArmaduraLosaRevit.Model.BarraEstribo.Servicios;
using ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion;
using ArmaduraLosaRevit.Model.UTILES.AyudasView;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Dimensiones;

namespace ArmaduraLosaRevit.Model.BarraEstribo
{
    public class ManejadorEstriboCOnfinamientoAuto : AManejadorEstriboCOnfinamiento
    {
        private List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboMuro;
        private List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboLateralMuro;
        private List<BarraRefuerzoEstriboMuroSinTras> ListBarraRefuerzoEstriboTrabasMuro;


        public List<IntervalosConfinaDTOAuto> ListIntervalosConfinaDTOAuto { get; set; }
        public int CantidadDibujadas { get; internal set; }

        // ConfiguracionTAgEstriboDTo _configuracionTAgEstriboDTo = null;
        // List<EstriboMuroDTO> resul_EstriboMuroDTO = null;
        // private View3D _view3D_BUSCAR;

        public ManejadorEstriboCOnfinamientoAuto(UIApplication uiapp,
                                                DatosConfinamientoAutoDTO configuracionInicialEstriboDTO,
                                                List<IntervalosConfinaDTOAuto> ListIntervalosConfinDTOAuto) : base(uiapp, configuracionInicialEstriboDTO)
        {
            // this._uiapp = uiapp;
            // this._configuracionInicialEstriboDTO = configuracionInicialEstriboDTO;
            this.ListIntervalosConfinaDTOAuto = ListIntervalosConfinDTOAuto;
            this.CantidadDibujadas = 0;
            //  this._doc = uiapp.ActiveUIDocument.Document;
            //  _listaRebarIdCambiarColor = new List<ElementId>();
            //  this._configuracionTAgEstriboDTo = null;
            // this.resul_EstriboMuroDTO = null;
        }

        public bool CrearEstriboMuro() => CrearConfinaminetoMuro();
     
        public bool CrearConfinaminetoMuro()
        {
            string pier = "";
            string Story = "";

            if (ListIntervalosConfinaDTOAuto.Count == 0) return true;
            _listaRebarIdCambiarColor.Clear();

        //    UtilStopWatch _utilStopWatch = new UtilStopWatch();
          //  _utilStopWatch.IniciarMedicion();
            CalculosIniciales();
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

             ListBarraRefuerzoEstriboMuro = new List<BarraRefuerzoEstriboMuroSinTras>();
            ListBarraRefuerzoEstriboLateralMuro = new List<BarraRefuerzoEstriboMuroSinTras>();
            ListBarraRefuerzoEstriboTrabasMuro = new List<BarraRefuerzoEstriboMuroSinTras>();

            //provisorio
            bool IsDibujarDetail = false;

            try
            {
      
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Dibujar Estribo1-NH");
                    for (int i = 0; i < ListIntervalosConfinaDTOAuto.Count; i++)
                    {
                     
                        IntervalosConfinaDTOAuto newIntervalosMallaDTOAuto = ListIntervalosConfinaDTOAuto[i];
                        //Util.InfoMsg($"  estribodiametr{newIntervalosMallaDTOAuto._datosConfinaDTO.DiamtroEstriboMM  }   anchovisible:  {newIntervalosMallaDTOAuto._datosConfinaDTO.largoVisible}   ListaPto: { newIntervalosMallaDTOAuto.ListaPtos[0].REdondearString(4) }  /  { newIntervalosMallaDTOAuto.ListaPtos[1].REdondearString(4) }   "); 
                        pier = newIntervalosMallaDTOAuto.Pier;
                        Story = newIntervalosMallaDTOAuto.Story;
                        _configuracionInicialEstriboDTO = newIntervalosMallaDTOAuto._datosConfinaDTO;
                         SeleccionPtosEstriboConfinamientoAuto seleccionPtosEstriboConfinamientoAuto = new SeleccionPtosEstriboConfinamientoAuto(_uiapp,
                                                                                                                                                 newIntervalosMallaDTOAuto,
                                                                                                                                                 _view3D_buscar,
                                                                                                                                                 ConTransaccionAlCrearSketchPlane:false);
                        if (!seleccionPtosEstriboConfinamientoAuto.Ejecutar_SeleccionarEstriboPtoAuto()) continue;

                        _configuracionEnfierrado = ObtenerConfiguracionEnfierrado_Service.obtener(_configuracionInicialEstriboDTO);


                        GenerarDatosIniciales_Service generarDatosIniciales_Service = new GenerarDatosIniciales_Service(_uiapp, newIntervalosMallaDTOAuto._datosConfinaDTO.tipoEstriboGenera, _configuracionInicialEstriboDTO);
                        if (!generarDatosIniciales_Service.M2_GeneralDatosIniciales_Auto())  continue; 


                        //estribo conf
                        if (newIntervalosMallaDTOAuto._datosConfinaDTO.IsEstribo == true)
                        {
                            BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboMuro = m1_1_GEnerarBArrasSinTras(generarDatosIniciales_Service, seleccionPtosEstriboConfinamientoAuto.newvo_EstriboMuroDTO);
                            if (barraRefuerzoEstriboMuro != null)
                            {
                                barraRefuerzoEstriboMuro.pier = pier;
                                barraRefuerzoEstriboMuro.story = Story;
                                ListBarraRefuerzoEstriboMuro.Add(barraRefuerzoEstriboMuro);
                            }
                        }
                        //laterales
                        if (newIntervalosMallaDTOAuto._datosConfinaDTO.IsLateral == true)
                        {
                            BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboLateralMuro =
                                M1_2_GenerarLAteralesGeneralSinTrans(generarDatosIniciales_Service, seleccionPtosEstriboConfinamientoAuto.newvo_EstriboMuroDTO);
                            if (barraRefuerzoEstriboLateralMuro != null)
                            {
                                barraRefuerzoEstriboLateralMuro.pier = pier;
                                barraRefuerzoEstriboLateralMuro.story = Story;
                                ListBarraRefuerzoEstriboLateralMuro.Add(barraRefuerzoEstriboLateralMuro);
                            }
                        }
                        //traba
                        if (newIntervalosMallaDTOAuto._datosConfinaDTO.IsTraba == true)
                        {
                            BarraRefuerzoEstriboMuroSinTras barraRefuerzoEstriboTrabasMuro = M1_3_GenerarTrabasGeneralSinTrans(generarDatosIniciales_Service, seleccionPtosEstriboConfinamientoAuto.newvo_EstriboMuroDTO);
                            if (barraRefuerzoEstriboTrabasMuro != null)
                            {
                                barraRefuerzoEstriboTrabasMuro.pier = pier;
                                barraRefuerzoEstriboTrabasMuro.story = Story;
                                ListBarraRefuerzoEstriboTrabasMuro.Add(barraRefuerzoEstriboTrabasMuro);
                            }
                        }

                        if (newIntervalosMallaDTOAuto._datosConfinaDTO.IsAgregarDetail && IsDibujarDetail)
                        {
                            CrearViewNH _CrearView = new CrearViewNH(_doc,50,50,"");
                            _CrearView.M2_1_CrearDetailViewSinTrans(newIntervalosMallaDTOAuto.ListaPtos);
                            
                        }

                        if (newIntervalosMallaDTOAuto._datosConfinaDTO.IsDImensionPorBajarFUndacion)
                        {
                            CreadorDimensiones _creardimension = new CreadorDimensiones(_doc,
                                                                                            newIntervalosMallaDTOAuto._datosConfinaDTO.ptoFinalDimension, 
                                                                                            newIntervalosMallaDTOAuto._datosConfinaDTO.ptoInicialDimension,
                                                                                            "COTA 50 (J.D.)");
                         _creardimension.Crear_sintrans();

                        }


                        //  _utilStopWatch.StopYContinuar($" ----------> e) Termino IsTraba",false);
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al analizar confinamiento en pier: {pier}   Story:{Story}");
                Debug.WriteLine($"ex:{ex.Message}");

            }
            //******************************************
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Dibujar Estribo2-NH");

                    ConfiguracionTAgEstriboDTo _configuracionTAgEstriboDTo = new ConfiguracionTAgEstriboDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                        IsDIrectriz = false,
                        tagOrientation = TagOrientation.Vertical,

                    };

                    foreach (var item in ListBarraRefuerzoEstriboMuro)
                    {
                        pier = item.pier;
                        Story = item.pier;
                        item.M1_GenerarBarra_2parte();
                        item.DibujarTagsEstribo(_configuracionTAgEstriboDTo);
                    }
                    foreach (var item in ListBarraRefuerzoEstriboLateralMuro)
                    {
                        pier = item.pier;
                        Story = item.pier;
                        item.GenerarLaterales_2parte();
                    }
                    foreach (var item in ListBarraRefuerzoEstriboTrabasMuro)
                    {
                        pier = item.pier;
                        Story = item.pier;
                        item.GenerarTrabas_2parte();
                    }

                    CantidadDibujadas = Math.Max(ListBarraRefuerzoEstriboMuro.Count, Math.Max(ListBarraRefuerzoEstriboLateralMuro.Count, ListBarraRefuerzoEstriboTrabasMuro.Count));

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
            //****************************
            try
            {
                CambiarColorBarras_Service cambiarColorBarras_Service = new CambiarColorBarras_Service(_uiapp, _configuracionInicialEstriboDTO.tipoConfiguracionEstribo);
                cambiarColorBarras_Service.M1_2_CAmbiarColor(_listaRebarIdCambiarColor);
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            return true;

        }


    }
}
