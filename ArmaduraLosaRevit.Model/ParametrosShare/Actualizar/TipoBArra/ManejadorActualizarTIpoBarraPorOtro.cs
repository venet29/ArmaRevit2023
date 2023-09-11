using ArmaduraLosaRevit.Model.BarraV.Automatico.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Actualizar;
using ArmaduraLosaRevit.Model.Fund.Traslapo;
using ArmaduraLosaRevit.Model.ParametosShare.Actualizar;
using ArmaduraLosaRevit.Model.ParametrosShare.Actualizar.Model;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ParametrosShare.Actualizar.TipoBArra
{
    public class ManejadorActualizarTIpoBarraPorOtro
    {
        public static bool IsEjecutar { get; set; } = true;
        public static bool Ejecutar(UIApplication _uiapp)
        {
            try
            {
                if (!IsEjecutar) return true;
                string antiguoNombreParametroCompartido = "BarraTipo";
                string nuevoNombreParametroCompartido = "BarraTipo";

                List<CambirSOLOTipoBarraDTO> ListaParamtrosCambiar = FactoryTIpoBarraPorOtro.ObtenerListaDto();
                // seleccionar barras
                SeleccionarRebarElemento seleccionarRebar = new SeleccionarRebarElemento(_uiapp, _uiapp.ActiveUIDocument.ActiveView);
                seleccionarRebar.BuscarListaRebarEnVistaActual();
                seleccionarRebar.BuscarListaPathReinformetEnVistaActual();

                // busca si tipo pathreinforment diferen al tipo  rebarInsystem
                ActualizarRebarInSystem.ActualizarTipoPAthDiferenteRebarSystem_ConTrans(_uiapp.ActiveUIDocument.Document, seleccionarRebar._lista_A_DePathReinfVistaActual);


                //cambiar segun lista 'ListaParamtrosCambiar'
                ManejadorActualizarBarraTipo _ManejadorActualizarBarraTipo = new ManejadorActualizarBarraTipo(_uiapp);
                _ManejadorActualizarBarraTipo.Ejecutar_CambirSOLOTipoBrrar(antiguoNombreParametroCompartido, nuevoNombreParametroCompartido,
                                                        ListaParamtrosCambiar, seleccionarRebar);


                IsEjecutar = false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'ManejadorActualizarTIpoBarraPorOtro'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }

    public class FactoryTIpoBarraPorOtro
    {
        private static List<CambirSOLOTipoBarraDTO> ListaParamtrosCambiar;



        public static List<CambirSOLOTipoBarraDTO> ObtenerListaDto()
        {
            if (ListaParamtrosCambiar == null)
                CArgar();
            return ListaParamtrosCambiar;
        }

        private static void CArgar()
        {
            ListaParamtrosCambiar = new List<CambirSOLOTipoBarraDTO>();

            //1)
            CambirSOLOTipoBarraDTO _CambirSOLOTipoBarraDTO = new CambirSOLOTipoBarraDTO()
            {
                valor_inicial = "FUND_BA_INF",
                SaltarSiesIgual = new List<string>(),
                ContinuarSiesIgual = new List<string>() { "FUND_BA" },
            };

            //2)
            CambirSOLOTipoBarraDTO _CambirELEV_ESCA = new CambirSOLOTipoBarraDTO()
            {
                valor_inicial = "ELEV_ESC",
                SaltarSiesIgual = new List<string>(),
                ContinuarSiesIgual = new List<string>() { "ELEV_ESCA" },
            };

            //3
            CambirSOLOTipoBarraDTO _CambirELEV_HORQ = new CambirSOLOTipoBarraDTO()
            {
                valor_inicial = "ELEV_BA_HORQ",
                SaltarSiesIgual = new List<string>(),
                ContinuarSiesIgual = new List<string>() { "ELEV_HORQ" },
            };

            //4
            CambirSOLOTipoBarraDTO _CambirFUND_BA_BPT = new CambirSOLOTipoBarraDTO()
            {
                valor_inicial = "FUND_SUP_BPT",
                SaltarSiesIgual = new List<string>(),
                ContinuarSiesIgual = new List<string>() { "FUND_BA_BPT" },
            };

            //*******
            ListaParamtrosCambiar.Add(_CambirSOLOTipoBarraDTO);
            ListaParamtrosCambiar.Add(_CambirELEV_ESCA);
            ListaParamtrosCambiar.Add(_CambirELEV_HORQ);
            ListaParamtrosCambiar.Add(_CambirFUND_BA_BPT);
        }
    }
}
