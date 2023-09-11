
using ArmaduraLosaRevit.Model.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension.modelo;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.PathReinf;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using static ArmaduraLosaRevit.Model.Extension.ExtensionPathReinforment;

namespace ArmaduraLosaRevit.Model.Fund.Traslapo
{


    public class PathReinformeTraslapoManejadorFund : PathReinformeTraslapoManejador
    {
        private List<DatosIntervalosFundDTO> LIStaDe_ListaPtosPerimetroBarras;

        private Element hostElement;

        public List<ManejadorPathReinf> ListaFundCreadas { get; private set; }



        #region 1) cotructor

        public PathReinformeTraslapoManejadorFund(UIApplication _uiapp, SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto, CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO) :
            base(_uiapp, seleccionarPathReinfomentConPto, _CalcularLargoPAthDTO)
        {

            LIStaDe_ListaPtosPerimetroBarras = new List<DatosIntervalosFundDTO>();
            ListaFundCreadas = new List<ManejadorPathReinf>();
        }

        #endregion



        public void M0_EjecutarTraslapoFund()
        {
            try
            {

                if (!M1_GenerarCalculosGeneralesFund()) return;

                if (!ObtenerLIStaDe_ListaPtosPerimetroBarras()) return;

                using (TransactionGroup t = new TransactionGroup(_doc))
                {

                    t.Start("Crear Traslapor-NH");

                    M2_Crear2PathReinformentFundPorTraslapo();
                    //   M3_CrearDImension();
                    if (ListaFundCreadas.Count == 2)
                        M2_2_BorrarPathReinfAntigua();

                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error ex:{ex.Message}");
            }

        }

        private bool ObtenerLIStaDe_ListaPtosPerimetroBarras()
        {
            try
            {
                //adatos 
                CoordenadaPath coord = calculoDatosParaReinforment._coordCalculos;
                XYZ direccionAbajo = (coord.p2 - coord.p1).Normalize();
                XYZ direccionDerecha = (coord.p3 - coord.p2).Normalize();
                HookPAthRein _hoookPAthRein = _seleccionarPathReinfomentConPto._HoookPAthRein;

                //1
                ContenedorDatosPathReinformeDTO fun1 = calculoDatosParaReinforment.datosNuevoPathIzqAbajoDTO;
                DatosIntervalosFundDTO _DatosIntervalosFundDTO1 = new DatosIntervalosFundDTO()
                {
                    _listaptos = fun1.Lista4ptosPAth,
                    PtoMouse = fun1.ptoTagSoloTraslapo,
                    tipoPataFun = (_hoookPAthRein.rebarHookTypePrincipal_end != null ? TipoPataFund.IzqInf : TipoPataFund.Sin),
                    PtoCodoDireztriz = fun1.ptoTagSoloTraslapo + direccionAbajo * Util.CmToFoot(80) + direccionDerecha * Util.CmToFoot(10),
                    PtoDireccionDireztriz = fun1.ptoTagSoloTraslapo + direccionAbajo * Util.CmToFoot(80) + direccionDerecha * Util.CmToFoot(150),
                    rebarHookTypePrincipal_star = null,
                    rebarHookTypePrincipal_end = _hoookPAthRein.rebarHookTypePrincipal_end

                };

                LIStaDe_ListaPtosPerimetroBarras.Add(_DatosIntervalosFundDTO1);

                //2
                ContenedorDatosPathReinformeDTO fun2 = calculoDatosParaReinforment.datosNuevoPathDereArribaDTO;
                DatosIntervalosFundDTO _DatosIntervalosFundDTO2 = new DatosIntervalosFundDTO()
                {
                    _listaptos = fun2.Lista4ptosPAth,
                    PtoMouse = fun2.ptoTagSoloTraslapo,
                    tipoPataFun = (_hoookPAthRein.rebarHookTypePrincipal_star != null ? TipoPataFund.DereSup : TipoPataFund.Sin),
                    PtoCodoDireztriz = fun2.ptoTagSoloTraslapo + direccionAbajo * Util.CmToFoot(80) + direccionDerecha * Util.CmToFoot(10),
                    PtoDireccionDireztriz = fun2.ptoTagSoloTraslapo + direccionAbajo * Util.CmToFoot(80) + direccionDerecha * Util.CmToFoot(150),
                    rebarHookTypePrincipal_star = _hoookPAthRein.rebarHookTypePrincipal_star,
                    rebarHookTypePrincipal_end = null
                };

                LIStaDe_ListaPtosPerimetroBarras.Add(_DatosIntervalosFundDTO2);


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'ListaPtosPerimetroBarras'  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_GenerarCalculosGeneralesFund()
        {
            try
            {

                var hostId = _seleccionarPathReinfomentConPto.PathReinforcement.GetHostId();
                if (hostId == null)
                {
                    return true;
                }
                hostElement = _doc.GetElement(hostId);
                //1
                ICalculoTiposTraslapos calculoTiposTraslapos = CalculoTiposTraslapos.CreatorCalculoTiposTraslapos(_seleccionarPathReinfomentConPto.PathReinforcement, _doc);
                if (!calculoTiposTraslapos.IsOk)
                {
                    Util.ErrorMsg($"Error al crear traslapo:  'M1_GenerarCalculosGenerales'");
                    return false;
                }
                //2
                CalculoDatoslosa calculoDatoslosa = new CalculoDatoslosa(_seleccionarPathReinfomentConPto, _doc);
                DatosLosaYpathInicialesDTO = calculoDatoslosa.ObtenerContenedorDatosFUND();
                if (!DatosLosaYpathInicialesDTO.IsOk)
                {
                    Util.ErrorMsg($"Error al obtener datos losa:  'M1_GenerarCalculosGenerales'");
                    return false;
                }
                //3
                CalculoCoordPathReinforme pathReinformeCalculos = new CalculoCoordPathReinforme(_seleccionarPathReinfomentConPto, _doc);
                pathReinformeCalculos.Calcular4PtosPathReinf();
                if (!pathReinformeCalculos.IsPtoOK)
                {
                    Util.ErrorMsg($"Error al obtener datos pathRein:  'M1_GenerarCalculosGenerales'");
                    return false;
                }
                //final
                calculoDatosParaReinforment =
                    FactoryITraslapo.CreateNewPathReinformentV2(pathReinformeCalculos.Obtener4pointPathReinf(), _puntoSeleccionMouse,
                                                                DatosLosaYpathInicialesDTO, calculoTiposTraslapos);

                if (!calculoDatosParaReinforment.M1_Obtener2PathReinformeTraslapoDatos()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M1_GenerarCalculosGeneralesFund' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public Result M2_Crear2PathReinformentFundPorTraslapo()
        {
            try
            {


                foreach (DatosIntervalosFundDTO _datosIntervalosDTO in LIStaDe_ListaPtosPerimetroBarras)
                {
                    Element fund = hostElement;
                    DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales = new DatosNuevaBarraDTO()
                    {
                        EspaciamientoFoot = DatosLosaYpathInicialesDTO.Espaciamiento,
                        DiametroMM = Math.Round(Util.FootToMm(DatosLosaYpathInicialesDTO.diametroEnFoot), 0),
                        TipoPataFun = _datosIntervalosDTO.tipoPataFun,
                        PtoMouse = _datosIntervalosDTO.PtoMouse,
                        PtoCodoDireztriz = _datosIntervalosDTO.PtoCodoDireztriz,
                        PtoDireccionDirectriz = _datosIntervalosDTO.PtoDireccionDireztriz,
                        rebarBarTypePrincipal_star = _datosIntervalosDTO.rebarHookTypePrincipal_star,
                        rebarBarTypePrincipal_end = _datosIntervalosDTO.rebarHookTypePrincipal_end,
                        IsAcortarCUrva = false
                    };

                    if (CargadorPAthReinf.CrearBarraFundaciones(_uiapp, fund, _datosIntervalosDTO._listaptos, _datosNuevaBarraDTOIniciales))
                        ListaFundCreadas.Add(CargadorPAthReinf._manejadorPathReinf);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M2_Crear2PathReinformentFundPorTraslapo'   ex:{ex.Message}");
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }





        private void M2_2_BorrarPathReinfAntigua()
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Borrar pathreiforment por traslapo-NH");

                    _doc.Delete(_seleccionarPathReinfomentConPto.PathReinforcement.Id);
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }

        }
    }




}
