using ArmaduraLosaRevit.Model.BarraV.UpDate;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.RebarLosa.UpDate;
using ArmaduraLosaRevit.Model.Viewnh.UpDate;
using ArmaduraLosaRevit.Model.Visibilidad.UpdateVistas;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UpdateGenerar
{
    //01-06-2023   clase de respaldo de 'UpdateGeneral' se dejaran los paramtro como estaticos para que se cargue solo una instancia
    //borrar paso por lo menos 1 mes
    public class UpdateGeneral_respaldo
    {
        private readonly UIApplication _uiapp;
        private Manejador_UpdateRebar _manejadorUpdateRebar;
        private Manejador_UpDateMoverPathSymbol _manejador_UpDateMoverPathSymbol;
        private Manejador_UpDateMoverTagPathSymbol _manejador_UpDateMoverTagPathSymbol;
        private ManejadorNombreVistaEnBarras _manejadorNombreVistaEnBarras;
        private ManejadorUpdaterRebarPath _manejadorUpdaterRebarPath;
        private ManejadorUpdaterNombreView _manejadorUpdaterNombreView;
       // private Manejador_UpDateEditPathReinf _Manejador_UpDateEditPathReinf;
        public UpdateGeneral_respaldo(UIApplication _uiapp)
        {
           /*
            this._uiapp = _uiapp;
            _manejadorUpdateRebar = new Manejador_UpdateRebar(_uiapp);
            _manejador_UpDateMoverPathSymbol = new Manejador_UpDateMoverPathSymbol(_uiapp);

            _manejador_UpDateMoverTagPathSymbol = new Manejador_UpDateMoverTagPathSymbol(_uiapp);

            //no se usa verificar
            _manejadorNombreVistaEnBarras = new ManejadorNombreVistaEnBarras(_uiapp);
            _manejadorUpdaterRebarPath = new ManejadorUpdaterRebarPath(_uiapp);
            _manejadorUpdaterNombreView = new ManejadorUpdaterNombreView(_uiapp);
            //_Manejador_UpDateEditPathReinf = new Manejador_UpDateEditPathReinf(_uiapp);
           */
        }


        /// <summary>
        /// importante no modificar nombre, pq este nombre se busca en 'DocumentOpened' para cargar esta rutina cuando se abre un nuevo docuementop
        /// </summary>
        /// <returns></returns>
        public bool M1_ConfiguracionAlCArgarREvit()
        {
            try
            {
                /*
                _manejadorUpdateRebar.CargarUpdateREbar();
                _manejadorUpdaterRebarPath.CargarUpdateREbarPath();

                _manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol();
                _manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol();
                //_Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();

                _manejadorUpdaterNombreView.CargarUpdaterNombreView();*/

                Debug.WriteLine("-->ConfiguracionAlCArgarREvit finalizado");
               // Util.ErrorMsg ("-->ConfiguracionAlCArgarREvit finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool M2_CargarBArras()
        {
            try
            {
                /*
                _manejadorUpdateRebar.CargarUpdateREbar();
                _manejadorUpdaterRebarPath.CargarUpdateREbarPath();

                _manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol();
                _manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol();
                //_Manejador_UpDateEditPathReinf.CargarUpdateEditPathReinf();

                //descativar
                _manejadorUpdaterNombreView.CargarUpdaterNombreView();*/
                Debug.WriteLine("-->CargarBArras finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool M3_DesCargarBarras()
        {
            try
            { /*
                _manejadorUpdateRebar.DesCargarUpdateREbar();
                _manejadorUpdaterRebarPath.DesCargarUpdateREbarPAth();

                _manejador_UpDateMoverPathSymbol.DesCargarUpdatePathSymbol();
                _manejador_UpDateMoverTagPathSymbol.DescargarTagUpdatePathSymbol();
              //  _Manejador_UpDateEditPathReinf.DesCargarUpdatePathReinf();

                _manejadorUpdaterNombreView.DesCargarUpdaterNombreView();*/
                Debug.WriteLine("-->DesCargarBarras finalizado");
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        //generales
        public bool M4_CargarGenerar()
        {
            try
            {/*
                _manejadorUpdateRebar.CargarUpdateREbar();
                _manejadorUpdaterRebarPath.CargarUpdateREbarPath();

                _manejador_UpDateMoverTagPathSymbol.CargarUpdateTagPathSymbol();
                _manejador_UpDateMoverPathSymbol.CargarUpdatePathSymbol();
   

                _manejadorUpdaterNombreView.CargarUpdaterNombreView();*/
         
                Debug.WriteLine("-->CargarGenerar finalizado");
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public bool M5_DesCargarGenerar()
        {
            try
            {/*
                _manejadorUpdateRebar.DesCargarUpdateREbar();

                _manejador_UpDateMoverPathSymbol.DesCargarUpdatePathSymbol();
                _manejador_UpDateMoverTagPathSymbol.DescargarTagUpdatePathSymbol();
               

                _manejadorUpdaterNombreView.DesCargarUpdaterNombreView();*/
                Debug.WriteLine("-->DesCargarGenerar finalizado");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public bool M6_CargarRebarParaEditar()
        {
            try
            {
              //  _manejadorUpdateRebar.CargarUpdateREbar();
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
