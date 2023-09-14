using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update;
using ArmaduraLosaRevit.Model.FILTROS.Ayuda;
using ArmaduraLosaRevit.Model.RebarLosa.UpDate;
using ArmaduraLosaRevit.Model.Viewnh.UpDate;
using ArmaduraLosaRevit.Model.Visibilidad.UpdateVistas;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.UpdateGenerar
{
    public class UpdateGeneral
    {
        private static UIApplication _uiapp;
        //private static Manejador_UpdateRebar _manejadorUpdateRebar;
        //private static Manejador_UpDateMoverPathSymbol _manejador_UpDateMoverPathSymbol;
        //private static Manejador_UpDateMoverTagPathSymbol _manejador_UpDateMoverTagPathSymbol;
        //private static ManejadorNombreVistaEnBarras _manejadorNombreVistaEnBarras;
        //private static ManejadorUpdaterRebarPath _manejadorUpdaterRebarPath;
        //private static ManejadorUpdaterNombreView _manejadorUpdaterNombreView;

        private static bool IsM1_ConfiguracionAlCArgarREvit = false;

        private static bool IsM4_CargarGenerar = false;

        private static bool IsM6_CargarRebarParaEditar = false;

        // obs1)contructor se ultiza pra cargar desde ribon y asignar 'uiapp' cuando se carga rutina 'M1_ConfiguracionAlCArgarREvit' desde dll        
        public UpdateGeneral(UIApplication uiapp)
        {
            _uiapp = uiapp;
        }

        //NOTA IMPORT  NO BORRAR CONSTRUCTOR  PORQUE CARGA VALIABLE QUE SE USA CON EL RIBBON AL ABRIR NUEVOS PROYECTO EN EÑ METODO '?cargarMetodoNOmbreClase'
        // QUE EJECUTARA M1_ConfiguracionAlCArgarREvit()
        //public UpdateGeneral(UIApplication uiapp)
        //{
        //    _uiapp = uiapp;
        //    //_manejadorUpdateRebar = new Manejador_UpdateRebar(_uiapp);
        //    //_manejador_UpDateMoverPathSymbol = new Manejador_UpDateMoverPathSymbol(_uiapp);

        //    //_manejador_UpDateMoverTagPathSymbol = new Manejador_UpDateMoverTagPathSymbol(_uiapp);

        //    ////no se usa verificar
        //    //_manejadorNombreVistaEnBarras = new ManejadorNombreVistaEnBarras(_uiapp);
        //    //_manejadorUpdaterRebarPath = new ManejadorUpdaterRebarPath(_uiapp);
        //    //_manejadorUpdaterNombreView = new ManejadorUpdaterNombreView(_uiapp);
        //    //_Manejador_UpDateEditPathReinf = new Manejador_UpDateEditPathReinf(_uiapp);
        //}
        #region  se usan en el 'crearRibon'

        //NOTA IMPORT  NO BORRAR CONSTRUCTOR  PORQUE CARGA VALIABLE QUE SE USA CON EL RIBBON AL ABRIR NUEVOS PROYECTO EN EÑ METODO '?cargarMetodoNOmbreClase'
        // QUE EJECUTARA M1_ConfiguracionAlCArgarREvit()

        public static bool AsignaUapp(UIApplication uiapp)
        {
            try
            {
                _uiapp = uiapp;

            }
            catch (Exception )
            {
                Util.ErrorMsg($"Error en 'AsignaUapp'. ");
                return false;
            }
            return true;
        }

        /// <summary>
        /// IMPORTANTE NO MODIFICAR ESTA CLASE , SE TUTIIZA AL CARGAR PROYECTO DESDE RIBON.
        /// EN RIBON SE EJECUTA EN EL METODO ' public bool cargarMetodoNOmbreClase()', pq este nombre se busca en 'DocumentOpened' para cargar esta rutina cuando se abre un nuevo docuementop
        /// </summary>
        /// <returns></returns>
        public static bool M1_ConfiguracionAlCArgarREvit()
        {
            try
            {
                if (IsM1_ConfiguracionAlCArgarREvit) return true;

                // if (!CrearInstancia(_uiapp)) return false;

                if (!IsM6_CargarRebarParaEditar)
                    Manejador_UpdateRebar.CargarUpdateREbar(_uiapp);

                ManejadorUpdaterRebarPath.CargarUpdateREbarPath(_uiapp);

                Manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol(_uiapp);
                Manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol(_uiapp);
                //_Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();

                ManejadorUpdaterNombreView.CargarUpdaterNombreView(_uiapp);

                IsM6_CargarRebarParaEditar = true;
                IsM1_ConfiguracionAlCArgarREvit = true;

                Debug.WriteLine("-->ConfiguracionAlCArgarREvit finalizado");
                // Util.ErrorMsg ("-->ConfiguracionAlCArgarREvit finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion
        public static bool M2_CargarBArras(UIApplication _uiapp)
        {
            try
            {
                if (IsM1_ConfiguracionAlCArgarREvit) return true;

                //   if (!CrearInstancia(_uiapp)) return false;

                if (!IsM6_CargarRebarParaEditar)
                    Manejador_UpdateRebar.CargarUpdateREbar(_uiapp);

                ManejadorUpdaterRebarPath.CargarUpdateREbarPath(_uiapp);

                Manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol(_uiapp);
                Manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol(_uiapp);
                //_Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();

                //descativar
                ManejadorUpdaterNombreView.CargarUpdaterNombreView(_uiapp);

                IsM6_CargarRebarParaEditar = true;
                IsM1_ConfiguracionAlCArgarREvit = true;

                Debug.WriteLine("-->CargarBArras finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool M3_DesCargarBarras(UIApplication _uiapp)
        {
            try
            {
                //  if (!CrearInstancia(_uiapp)) return false;

                Manejador_UpdateRebar.DesCargarUpdateREbar(_uiapp);

                ManejadorUpdaterRebarPath.DesCargarUpdateREbarPAth(_uiapp);

                Manejador_UpDateMoverPathSymbol.DesCargarUpdatePathSymbol(_uiapp);
                Manejador_UpDateMoverTagPathSymbol.DescargarTagUpdatePathSymbol(_uiapp);
                //  _Manejador_UpDateEditPathReinf.DesCargarUpdatePathReinf();

                ManejadorUpdaterNombreView.DesCargarUpdaterNombreView(_uiapp);

                IsM6_CargarRebarParaEditar = false;
                IsM1_ConfiguracionAlCArgarREvit = false;
                Debug.WriteLine("-->DesCargarBarras finalizado");
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        //generales
        public static bool M4_CargarGenerar(UIApplication _uiapp)
        {
            try
            {
                if (IsM4_CargarGenerar) return true;

                //  if (!CrearInstancia(_uiapp)) return false;

                if (!IsM6_CargarRebarParaEditar)
                    Manejador_UpdateRebar.CargarUpdateREbar(_uiapp);

                ManejadorUpdaterRebarPath.CargarUpdateREbarPath(_uiapp);

                Manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol(_uiapp);
                Manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol(_uiapp);
                //Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();

                ManejadorUpdaterNombreView.CargarUpdaterNombreView(_uiapp);
                ///  _manejadorNombreVistaEnBarras.CargarUpdateView();
                ///  
                IsM6_CargarRebarParaEditar = true;
                IsM4_CargarGenerar = true;
                Debug.WriteLine("-->CargarGenerar finalizado");
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public static bool M5_DesCargarGenerar(UIApplication _uiapp)
        {
            try
            {
                //  if (!CrearInstancia(_uiapp)) return false;

                Manejador_UpdateRebar.DesCargarUpdateREbar(_uiapp);
                Manejador_UpDateMoverPathSymbol.DesCargarUpdatePathSymbol(_uiapp);
                Manejador_UpDateMoverTagPathSymbol.DescargarTagUpdatePathSymbol(_uiapp);
                //_Manejador_UpDateEditPathReinf.DesCargarUpdatePathReinf();

                ManejadorUpdaterNombreView.DesCargarUpdaterNombreView(_uiapp);

                IsM6_CargarRebarParaEditar = false;
                IsM4_CargarGenerar = false;
                Debug.WriteLine("-->DesCargarGenerar finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public static bool M6_CargarRebarParaEditar(UIApplication _uiapp)
        {
            try
            {
                if (IsM6_CargarRebarParaEditar) return true;
                //   if (!CrearInstancia(_uiapp)) return false;

                Manejador_UpdateRebar.CargarUpdateREbar(_uiapp);
                IsM6_CargarRebarParaEditar = true;
                Debug.WriteLine("-->CargarBArras finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
