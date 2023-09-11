using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraMallaRebar;
using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Calculos;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Creador;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ArmaduraLosaRevit.Model.BarraV.Horquilla
{
    //NOTA : LAS HORQUILLAS siguen la misma directri

    public class ManejadorBarraV_Horq : ManejadorBarraVMalla_BASE
    {
        private readonly DireccionRecorrido _DireccionRecorridoBarra;
        private ConfiguracionInicialBarraVerticalHorqDTO confiEnfierradoHorqDTO;
#pragma warning disable CS0169 // The field 'ManejadorBarraV_Horq._listaRebarCambiarColor' is never used
        private List<Rebar> _listaRebarCambiarColor;
#pragma warning restore CS0169 // The field 'ManejadorBarraV_Horq._listaRebarCambiarColor' is never used
        private DatosBarraElevacionHorquilla _datosBarraElevacionHorquilla;
        private RecalcularPtosYEspaciamieto_Horquilla _RecalcularPtosYEspaciamieto_HORQUILLA;
        private SeleccionarElementosV _seleccionarElementos;
        private DatosMuroSeleccionadoDTO muroSeleccionadoBArraDTO;
        private DatosMuroSeleccionadoDTO muroSeleccionadoDTO;

        public SelecionarPtoSup selecionarPtoSup { get; private set; }

        public ManejadorBarraV_Horq(UIApplication uiapp,
            ISeleccionarNivel seleccionarNivel,
            ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO,
            DireccionRecorrido _DireccionRecorridoHorquilla, DireccionRecorrido _DireccionRecorridoBarra,
            ConfiguracionInicialBarraVerticalHorqDTO confiEnfierradoHorqDTO) :
            base(uiapp, seleccionarNivel)
        {
            this._uiapp = uiapp;
            this._uidoc = uiapp.ActiveUIDocument;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._seleccionarNivel = seleccionarNivel;
            this._confiWPFEnfierradoDTO = confiWPFEnfierradoDTO;
            this._DireccionRecorrido = _DireccionRecorridoHorquilla;
            this._view = this._uidoc.ActiveView;
            _listaDebarra = new List<IbarraBase>();
            _listaLevelTotal = new List<Level>();
            _listaCreadorBarrasV = new List<CreadorBarrasV>();
            this._DireccionRecorridoBarra = _DireccionRecorridoBarra;
            this.confiEnfierradoHorqDTO = confiEnfierradoHorqDTO;
        }

        public override void CrearBArra()
        {
            ////UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            //UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            CreadorBarrasV_Horq creadorBarrasV_Horqu = null; ///  Horquilla Horizontal, en elevacion de ve como linea
            CreadorBarrasV creadorBarrasV = null;  /// Horquilla vertical, en elevacion de ve como  C
            try
            {
                //
                if (!M1_CalculosIniciales() || (!Directory.Exists(ConstNH.CONST_COT))) return;

                //1
                _seleccionarElementos = new SeleccionarElementosV(_uiapp, _confiWPFEnfierradoDTO, _listaLevelTotal);
                if (!_seleccionarElementos.M1_ObtenerPtoinicio())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {
                        // Util.ErrorMsg("Error Al Selecciona muro de referencia");
                    }
                    else
                        Debug.WriteLine("Error Al Selecciona muro de referencia");
                    return;
                }

                if (Util.IsParallel(_seleccionarElementos._direccionMuro, _view.ViewDirection))
                {
                    Util.ErrorMsg("Seleccionar elemento que sea paralelo a vista, para poder obtener espesor. El borde de selecciona no necesita ser el mimso del elemeto seleccionado.");

                    return ;
                }

                muroSeleccionadoDTO = _seleccionarElementos.M5_OBtenerElementoREferenciaDTO(_DireccionRecorrido);
                if (muroSeleccionadoDTO == null) return;

                double espesorFoot = _seleccionarElementos._espesorMuroFoot;
                //2nn
                _confiWPFEnfierradoDTO.TipoSelecion = TipoSeleccion.ConMouse;
                if (!_seleccionarElementos.M2_SeleccionarPtoInicio()) return;
                // _seleccionarElementos._PtoInicioIntervaloBarra = _seleccionarElementos._PtoInicioBaseBordeMuro.AsignarZ(_seleccionarElementos._PtoInicioIntervaloBarra.Z);// _seleccionarElementos._PtoInicioIntervaloBarra.AsignarZ()
            

                selecionarPtoSup = _seleccionarElementos.M4_SeleccionarPtoSuperiorLineaBarras_paraHorquilla();
                if (selecionarPtoSup == null) return;
                if (selecionarPtoSup.ListaLevelIntervalo.Count == 0)
                {
                    Util.ErrorMsg("Error en la seleccion de nivel. Creacion de barrar finalizada");
                    return;
                }


                //obtener puntos para barra
                ObtenerPtosParaBarraV(selecionarPtoSup, muroSeleccionadoDTO);

                RecalcularEspaciamientoLineasBarrasVertical(_seleccionarElementos, 3);

                _DireccionRecorrido.LargoRecorridoCm = _seleccionarElementos.LargoRecorridoHorizontalSeleccionCM;

                if (!Util.IsNumeric(_confiWPFEnfierradoDTO.Inicial_Cantidadbarra))
                {
                    Util.ErrorMsg("Error: cantidad de barras debe ser numerico");
                    return;
                }
                _RecalcularPtosYEspaciamieto_HORQUILLA = new RecalcularPtosYEspaciamieto_Horquilla(selecionarPtoSup._PtoInicioIntervaloBarra,
                                                                                                            selecionarPtoSup._PtoFinalIntervaloBarra_ProyectadoCaraMuroHost,
                                                                                                           Util.ConvertirStringInInteger(_confiWPFEnfierradoDTO.Inicial_Cantidadbarra));
                _RecalcularPtosYEspaciamieto_HORQUILLA.RecalcularPtosYCalcularEspaciamiento(_datosBarraElevacionHorquilla);
                _RecalcularPtosYEspaciamieto_HORQUILLA.CalcularDireccionSeleccion(_view, _confiWPFEnfierradoDTO.inicial_diametroMM);

                _confiWPFEnfierradoDTO.IntervaloBarras_HorqDTO_ = _RecalcularPtosYEspaciamieto_HORQUILLA.IntervaloBarras_HorqDTO_;

                //espciamiento segun numero de horquilla
                double espaciamiento = _RecalcularPtosYEspaciamieto_HORQUILLA.EspaciamientoCorregidoCM;
                selecionarPtoSup._RecalcularPtosYEspaciamieto_HORQUILLA = _RecalcularPtosYEspaciamieto_HORQUILLA;

                selecionarPtoSup._PtoInicioIntervaloBarra = _RecalcularPtosYEspaciamieto_HORQUILLA.PtoInicial_Corregido;
                selecionarPtoSup._PtoFinalIntervaloBarra = _RecalcularPtosYEspaciamieto_HORQUILLA.PtoFinal_Corregido;
                selecionarPtoSup._PtoFinalIntervaloBarra_ProyectadoCaraMuroHost = _RecalcularPtosYEspaciamieto_HORQUILLA.PtoFinal_Corregido;

                _confiWPFEnfierradoDTO.EspaciamietoRecorridoBarraFoot = Util.CmToFoot(espaciamiento).ToString();//20
                RecalcularEspaciamientoLineasBarrasHorizontal(muroSeleccionadoDTO._EspesorMuroFoot, 1);


                //segun sentido seleccion
                // _confiWPFEnfierradoDTO.tipobarraH = TipoPataBarra.BarraVPataInicial;

                for (int i = 0; i < 1; i++)
                {

                    using (TransactionGroup transGroup = new TransactionGroup(_doc))
                    {
                        transGroup.Start("CrearGrupoBarraVertical-NH");

                        //horquillas
                        _confiWPFEnfierradoDTO.LineaBarraAnalizada = i + 1;
                        creadorBarrasV_Horqu = new CreadorBarrasV_Horq(_uiapp, selecionarPtoSup, _confiWPFEnfierradoDTO, muroSeleccionadoDTO);
                        creadorBarrasV_Horqu.Ejecutar(i);
                        _listaCreadorBarrasV.Add(creadorBarrasV_Horqu);


                        //barra vertical                     
                        CAmbiar_confiWPFEnfierradoDTO_ParaBarra();
                        creadorBarrasV = new CreadorBarrasV(_uiapp, selecionarPtoSup, _confiWPFEnfierradoDTO, muroSeleccionadoBArraDTO);
                        creadorBarrasV.Ejecutar(i);
                        _listaCreadorBarrasV.Add(creadorBarrasV);
                        
                        //**************************************************************
                        //CASO PARCHE PARA CAMBIAR  PARAMETRO COMPRATIDO 17-07-23
                        using (Transaction t = new Transaction(_doc))
                        {
                            t.Start("dibujar barrasV-NH");
                            if (ParameterUtil.FindParaByName(creadorBarrasV._listaRebar[0], "BarraTipo") != null)
                            ParameterUtil.SetParaInt(creadorBarrasV._listaRebar[0], "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(TipoRebar.ELEV_BA_HORQ));  //"nombre de vista"                                            
                            t.Commit();
                        }
                        //**************************************************************

                        M7_CAmbiarColor(TipoCOlores.magenta, false);
                        transGroup.Assimilate();                    }
                    _listaCreadorBarrasV.Clear();

                }

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra Vertical:" + ex.Message);

                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

                return;
            }


        }

        private void ObtenerPtosParaBarraV(SelecionarPtoSup selecionarPtoSup, DatosMuroSeleccionadoDTO muroSeleccionadoDTO)
        {
            _datosBarraElevacionHorquilla = new DatosBarraElevacionHorquilla();
            _datosBarraElevacionHorquilla.PtoInicioIntervaloBarra = selecionarPtoSup._PtoInicioIntervaloBarra;
            _datosBarraElevacionHorquilla.PtoFinalIntervaloBarra = selecionarPtoSup._PtoFinalIntervaloBarra;
            _datosBarraElevacionHorquilla.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;
            _datosBarraElevacionHorquilla.LargoPata = selecionarPtoSup._PtoFinalIntervaloBarra.GetXY0().DistanceTo(muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost.GetXY0());
            _datosBarraElevacionHorquilla.EspaciamientoREcorridoFoot = muroSeleccionadoDTO._EspesorMuroFoot - Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM * 2 + (confiEnfierradoHorqDTO.inicial_diametroMM) / 10f);

        }

        private void CAmbiar_confiWPFEnfierradoDTO_ParaBarra()
        {

            muroSeleccionadoBArraDTO = _seleccionarElementos.M5_OBtenerElementoREferenciaDTO(_DireccionRecorridoBarra);
            if (muroSeleccionadoBArraDTO == null) return;
            muroSeleccionadoBArraDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost = muroSeleccionadoDTO.PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost;

            //
            selecionarPtoSup._PtoInicioIntervaloBarra = _datosBarraElevacionHorquilla.PtoInicioIntervaloBarra +
                                                                     // mover 1 cm mas para que no se tope
                                                                     new XYZ(0, 0, 1) * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM + (confiEnfierradoHorqDTO.inicial_diametroMM / 2) / 10f);
            selecionarPtoSup._PtoFinalIntervaloBarra = _datosBarraElevacionHorquilla.PtoFinalIntervaloBarra +
                                                        new XYZ(0, 0, -1) * Util.CmToFoot(ConstNH.RECUBRIMIENTO_BARRA_VERT_CM + (confiEnfierradoHorqDTO.inicial_diametroMM / 2) / 10f);


            _confiWPFEnfierradoDTO.BarraTipo = TipoRebar.ELEV_BA_V; //TipoRebar.ELEV_BA_HORQ;  //TipoRebar.ELEV_BA_V; 
            //_confiWPFEnfierradoDTO.NuevaLineaCantidadbarra = Convert.ToInt32( confiEnfierradoHorqDTO.inicial_Numero);
            _confiWPFEnfierradoDTO.IntervalosCantidadBArras[0] = Convert.ToInt32(confiEnfierradoHorqDTO.inicial_Numero);
            _confiWPFEnfierradoDTO.inicial_diametroMM = confiEnfierradoHorqDTO.inicial_diametroMM;

            _confiWPFEnfierradoDTO.TipoSeleccionMousePtoSuperior = TipoSeleccionMouse.mouse;
            _confiWPFEnfierradoDTO.TipoSeleccionMousePtoInferior = TipoSeleccionMouse.mouse;
            //_confiWPFEnfierradoDTO.TipoBarraRebar_ = TipoBarraVertical.Cabeza;

            _confiWPFEnfierradoDTO.EspaciamietoRecorridoBarraFoot = _datosBarraElevacionHorquilla.EspaciamientoREcorridoFoot.ToString();
            _confiWPFEnfierradoDTO.incial_ComoIniciarTraslapo_LineaImpar = 2;
            _confiWPFEnfierradoDTO.incial_ComoIniciarTraslapo_LineaPAr = 1;
            _confiWPFEnfierradoDTO.inicial_ComoTraslapo = 2;
            _confiWPFEnfierradoDTO.inicial_tipoBarraV = TipoPataBarra.BarraVPataAmbos_Horquilla;
            _confiWPFEnfierradoDTO.TipoBarraRebar_ = TipoBarraVertical.Cabeza_BarraVHorquilla;



        }
    }

    public class DatosBarraElevacionHorquilla
    {
        public XYZ PtoInicioIntervaloBarra { get; internal set; }
        public XYZ PtoFinalIntervaloBarra { get; internal set; }
        public XYZ PtoInicioBaseBordeElemen_ProyectadoCaraElemenHost { get; internal set; }
        public double LargoPata { get; internal set; }
        public double EspaciamientoREcorridoFoot { get; internal set; }
    }
}
