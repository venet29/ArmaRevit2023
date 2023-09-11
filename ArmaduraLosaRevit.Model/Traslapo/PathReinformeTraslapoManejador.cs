
using ArmaduraLosaRevit.Model.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.Renombrar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.CalculoPath;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ArmaduraLosaRevit.Model.Traslapo
{


    public class PathReinformeTraslapoManejador
    {
        protected UIApplication _uiapp;
        protected Document _doc;


        #region 0) propiedades
        //  public bool IsDibujarPath { get; set; }
        public PathReinforcement _pathReinforcement { get; private set; }
        public ContenedorDatosLosaDTO DatosLosaYpathInicialesDTO { get; set; }
        public XYZ _puntoSeleccionMouse { get; private set; }
        protected iCalculoDatosParaReinforment calculoDatosParaReinforment;
        protected SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto;
        private readonly CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO;
        protected UIApplication uiapp;
        protected BarraRoom newBarralosa_IzqAbajo;
        protected BarraRoom newBarralosa_dere;

        #endregion

        #region 1) cotructor

        public PathReinformeTraslapoManejador(UIApplication _uiapp, SeleccionarPathReinfomentConPto seleccionarPathReinfomentConPto, CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO)
        {
            this._uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._pathReinforcement = seleccionarPathReinfomentConPto.PathReinforcement;
            this._puntoSeleccionMouse = seleccionarPathReinfomentConPto.PuntoSeleccionMouse;
            this._seleccionarPathReinfomentConPto = seleccionarPathReinfomentConPto;
            this._CalcularLargoPAthDTO = _CalcularLargoPAthDTO;


        }

        #endregion




        public void M0_EjecutarTraslapo()
        {

            bool _iS_PATHREIN_AJUSTADO = DatosDiseño.IS_PATHREIN_AJUSTADO;
            bool _iS_PATHREIN_AJUSTADO_LARGO = VariablesSistemas.IsAjusteBarra_Largo;
            DatosDiseño.IS_PATHREIN_AJUSTADO = false;
            VariablesSistemas.IsAjusteBarra_Largo = false;
            try
            {

                if (!M1_GenerarCalculosGenerales()) return;


                //** para denombrar familia pathsymbol 13-04-2022
                ManejadorReNombrarFamPathSymbol _ManejadorReNombrar = new ManejadorReNombrarFamPathSymbol(_uiapp);
                if (_ManejadorReNombrar.IsFamiliasAntiguas())
                    _ManejadorReNombrar.Renombarra();
                //**

                using (TransactionGroup t = new TransactionGroup(_doc))
                {

                    t.Start("Crear Traslapor-NH");

                    Result result=  M2_Crear2PathReinformentPorTraslapo();
                    if (result != Result.Succeeded)
                    {
                        return;
                    }
                    M3_CrearDImension();

                    M2_2_BorrarPathReinfAntigua(newBarralosa_IzqAbajo, newBarralosa_dere);
                    t.Assimilate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error ex:{ex.Message}");
            }
            DatosDiseño.IS_PATHREIN_AJUSTADO = _iS_PATHREIN_AJUSTADO;
            VariablesSistemas.IsAjusteBarra_Largo = _iS_PATHREIN_AJUSTADO_LARGO;
        }
        //public void M0_EjecutarCambio()
        //{
        //    M1_GenerarCalculosGenerales();
        //    M2_Crear2PathReinformentPorTraslapo();

        //}
        public bool M1_GenerarCalculosGenerales()
        {
            try
            {


                //1
                ICalculoTiposTraslapos calculoTiposTraslapos = CalculoTiposTraslapos.CreatorCalculoTiposTraslapos(_seleccionarPathReinfomentConPto.PathReinforcement, _doc);
                if (!calculoTiposTraslapos.IsOk)
                {
                    Util.ErrorMsg($"Error al crear traslapo:  'M1_GenerarCalculosGenerales'");
                    return false;
                }
                //2
                CalculoDatoslosa calculoDatoslosa = new CalculoDatoslosa(_seleccionarPathReinfomentConPto, _doc);
                DatosLosaYpathInicialesDTO = calculoDatoslosa.ObtenerContenedorDatosLosa();
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


                if (_CalcularLargoPAthDTO != null && _CalcularLargoPAthDTO.pathDefinir != TipoPAthDefinir.Normal)
                {
                    _CalcularLargoPAthDTO.puntoSeleccionMouse = _seleccionarPathReinfomentConPto.PuntoSeleccionMouse;
                    _CalcularLargoPAthDTO.LargoTraslapo = UtilBarras.largo_traslapFoot_diamFoot(DatosLosaYpathInicialesDTO.diametroEnFoot);
                    // definir largo path
                    CalcularLargoPAth _CalcularLargoPAth = new CalcularLargoPAth(pathReinformeCalculos.Obtener4pointPathReinf(), _CalcularLargoPAthDTO);
                    if (_CalcularLargoPAth.Calcular())
                    {
                        _puntoSeleccionMouse = _CalcularLargoPAth._NuevoPuntoSeleccionMouse;
                    }
                }

                //final
                calculoDatosParaReinforment =
                    FactoryITraslapo.CreateNewPathReinformentV2(pathReinformeCalculos.Obtener4pointPathReinf(), _puntoSeleccionMouse,
                                                                DatosLosaYpathInicialesDTO, calculoTiposTraslapos);

                if (!calculoDatosParaReinforment.M1_Obtener2PathReinformeTraslapoDatos()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M1_GenerarCalculosGenerales' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public Result M2_Crear2PathReinformentPorTraslapo()
        {
            try
            {


                if (!calculoDatosParaReinforment.M2_IsPuedoDibujarPath()) return Result.Failed;
                calculoDatosParaReinforment.datosNuevoPathIzqAbajoDTO.AnguloRoomRad = DatosLosaYpathInicialesDTO.AnguloRoomRad;
                calculoDatosParaReinforment.datosNuevoPathDereArribaDTO.AnguloRoomRad = DatosLosaYpathInicialesDTO.AnguloRoomRad;

                newBarralosa_IzqAbajo = M2_1_CrearNewBarraPorTraslapo(calculoDatosParaReinforment.datosNuevoPathIzqAbajoDTO, new XYZ(0, 0, 0));
                if (newBarralosa_IzqAbajo == null) return Result.Failed;
                newBarralosa_dere = M2_1_CrearNewBarraPorTraslapo(calculoDatosParaReinforment.datosNuevoPathDereArribaDTO, new XYZ(Util.CmToFoot(ConstNH.CONST_PATH_SYMBOL_CM_PATH), Util.CmToFoot(0), 0));
                if (newBarralosa_dere == null) return Result.Failed;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M2_Crear2PathReinformentPorTraslapo' ex:{ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }

        public bool M3_CrearDImension()
        {
            try
            {
                double amguloBArra = calculoDatosParaReinforment.datosNuevoPathIzqAbajoDTO.AnguloP2toP3Rad;
                double _largoTraslapoFoot = UtilBarras.largo_traslapFoot_diamFoot(DatosLosaYpathInicialesDTO.diametroEnFoot);
                double _desplaY = ObtenerDesplazamientoEnY();
                XYZ _ptoIZqInf_Dimension = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_puntoSeleccionMouse, amguloBArra, _largoTraslapoFoot / 2, Util.CmToFoot(_desplaY));
                XYZ _ptoDereSup_Dimension = Util.ExtenderPuntoRespectoOtroPtosConAngulo(_puntoSeleccionMouse, amguloBArra, -_largoTraslapoFoot / 2, Util.CmToFoot(_desplaY));
                CreadorDimensiones EditarPathReinMouse =
                                        new CreadorDimensiones(_doc, _ptoIZqInf_Dimension, _ptoDereSup_Dimension, "COTA 50 (J.D.)");
                EditarPathReinMouse.Crear_conTrans();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en '' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private double ObtenerDesplazamientoEnY()
        {
            string tipoBarra = calculoDatosParaReinforment.datosNuevoPathDereArribaDTO.datosLosaDTO.tipoBarra.ToString();
            if (tipoBarra == "f1" || tipoBarra == "f3" || tipoBarra == "f4" || tipoBarra == "f1_SUP" || tipoBarra == "f7" || tipoBarra == "f11")
            { return 10; }
            else
            { return 30; }
        }

        private BarraRoom M2_1_CrearNewBarraPorTraslapo(ContenedorDatosPathReinformeDTO contenedorDatosPathReinformeDTO, XYZ despla)
        {
            try
            {
                BarraRoom newBarralosa_ = new BarraRoom(_uiapp, contenedorDatosPathReinformeDTO);

                if (newBarralosa_.statusbarra == Result.Succeeded)
                {
                    Result result = newBarralosa_.CrearBarraPorTraslapo(despla);

                    if (result != Result.Succeeded) return null;
                }
                return newBarralosa_;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'M2_1_CrearNewBarraPorTraslapo' ex:{ex.Message}");
                return null;
            }
        }

        private void M2_2_BorrarPathReinfAntigua(BarraRoom newBarralosa_IzqAbajo, BarraRoom newBarralosa_dere)
        {

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Borrar pathreiforment por traslapo-NH");

                    if (newBarralosa_dere.statusbarra == Result.Succeeded && newBarralosa_IzqAbajo.statusbarra == Result.Succeeded)
                    {
                        _doc.Delete2(_pathReinforcement);
                    }
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
