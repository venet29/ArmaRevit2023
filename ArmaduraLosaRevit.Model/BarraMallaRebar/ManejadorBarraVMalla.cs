using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraV.Creador;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Intervalos;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.TipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraMallaRebar
{
    public class ManejadorBarraVMalla : ManejadorBarraVMalla_BASE
    {

        public ManejadorBarraVMalla(UIApplication uiapp,
                ISeleccionarNivel seleccionarNivel,
                ConfiguracionIniciaWPFlBarraVerticalDTO confiWPFEnfierradoDTO,
                DireccionRecorrido _DireccionRecorrido,
                DatosMallasAutoDTO datosMallasDTO) : base(uiapp, seleccionarNivel, confiWPFEnfierradoDTO, _DireccionRecorrido, datosMallasDTO)
        {

        }

        public override void CrearBArra()
        {
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            try
            {

                //1
                if (!M1_CalculosIniciales()) return;

                SeleccionarElementosV _seleccionarElementos = new SeleccionarElementosV(_uiapp, _confiWPFEnfierradoDTO, _listaLevelTotal);
                if (!_seleccionarElementos.M1_SeleccionarElementoHost())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {   // Util.ErrorMsg("Error Al Selecciona muro de referencia");
                    }
                    else
                        Debug.WriteLine("Error Al Selecciona muro de referencia");
                    return;
                }

                _datosMallasDTO.espesorFoot = _seleccionarElementos._espesorMuroFoot;

                if (!_datosMallasDTO.VerificarEspesor()) return;

                //2nn
                if (!_seleccionarElementos.M2_SeleccionarPtoInicio()) return;

                SelecionarPtoSup selecionarPtoSup = _seleccionarElementos.M4_SeleccionarPtoSuperiorLineaBarras();
                if (selecionarPtoSup == null) return;
                if (selecionarPtoSup.ListaLevelIntervalo.Count ==0)
                {
                    Util.ErrorMsg("Error en la seleccion de nivel. Creacion de barrar finalizada");
                    return;
                }

                RecalcularEspaciamientoLineasBarrasVertical(_seleccionarElementos, 3);

                //selecionarPtoSup.VerificaSiEsSoloMallaCentral(_datosMallasDTO, _view);

                //3  _confiWPFEnfierradoDTO
                _DireccionRecorrido.LargoRecorridoCm = _seleccionarElementos.LargoRecorridoHorizontalSeleccionCM;
                DatosMuroSeleccionadoDTO _datosmuroSeleccionadoDTO = _seleccionarElementos.M5_OBtenerElementoREferenciaDTO(_DireccionRecorrido);
                if (_datosmuroSeleccionadoDTO == null) return;

                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CrearBArraVErticalMalla-NH");
                    DibujarBarraMalla _DibujarBarraMalla = new DibujarBarraMalla(_uiapp, _confiWPFEnfierradoDTO, _datosMallasDTO, _DireccionRecorrido);
                    _DibujarBarraMalla.CrearMAllaConRebar(selecionarPtoSup, _datosmuroSeleccionadoDTO);

                    _listaCreadorBarrasV.AddRange(_DibujarBarraMalla._listaCreadorBarrasV);

                    M7_CAmbiarColor();

                    t.Assimilate();
                }
                _listaCreadorBarrasV.Clear();

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", "Error al crear barra Vertical:" + ex.Message);
            }

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
#if DEBUG
            //LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh);
#endif
            return;
        }


    }
}
