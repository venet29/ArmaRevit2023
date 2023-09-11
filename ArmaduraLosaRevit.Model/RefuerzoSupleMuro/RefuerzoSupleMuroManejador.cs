using ArmaduraLosaRevit.Model.Fund.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.PathReinf;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using System;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.RefuerzoSupleMuro.Seleccionar;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;

namespace ArmaduraLosaRevit.Model.RefuerzoSupleMuro
{


    public class RefuerzoSupleMuroManejador
    {
        private UIApplication _uiapp;
        private Document _doc;
#pragma warning disable CS0169 // The field 'RefuerzoSupleMuroManejador._datosPLanarface' is never used
        private FundGeoDTO _datosPLanarface;
#pragma warning restore CS0169 // The field 'RefuerzoSupleMuroManejador._datosPLanarface' is never used

        public RefuerzoSupleMuroManejador(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
        }

        public Result execute(DatosNuevaBarraDTO _datosNuevaBarraDTOIniciales)
        {
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

            Result resul = Result.Succeeded;

            try
            {
                if (!M1_CalculosIniciales()) return Result.Cancelled;

                SeleccionarElementosV _seleccionarElementos = new SeleccionarElementosV(_uiapp);
                if (!_seleccionarElementos.M1_SeleccionarElementoHost())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {
                       // Util.ErrorMsg("Error Al Selecciona muro de referencia");
                    }
                    else
                        Debug.WriteLine("Error Al Selecciona muro de referencia");
                    return Result.Cancelled;
                }


                //2) selecion direccion barra  ,obtener pto inici y fin de barra con intersscion
                PuntosSeleccionMouseRefuerzo _puntosSeleccionMouse = new PuntosSeleccionMouseRefuerzo(_uiapp);
                if (!_puntosSeleccionMouse.M3_SelecionarPtosDirectriz("1) Seleccionar primer punto recorrido", "2) Seleccionar segundo punto recorrido")) return Result.Failed;
                _puntosSeleccionMouse.OrdenarPtosRefuerzo();


                _datosNuevaBarraDTOIniciales.PtoMouse = _seleccionarElementos._ptoSeleccionMouseCentroCaraMuro;

                //_datosNuevaBarraDTOIniciales.PtoFundaOriegenDireztriz = _puntosSeleccionMouse.p1_origenDiretriz;


                _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz = XYZ.Zero;//  _puntosSeleccionMouse.p2_SentidoDiretriz;

                Obtener4ptosMuro _Obtener4ptosMuro = new Obtener4ptosMuro(_uiapp, _seleccionarElementos, _puntosSeleccionMouse);
                if (!_Obtener4ptosMuro.EjecutarObtener4ptos()) return Result.Failed;

                //4)Obtener recorrido
                //4)dibujar
                //tengo que tener 4 ptos del poligono
                Element muro = _seleccionarElementos._ElemetSelect;
                List<XYZ> ListaPtosPerimetroBarras = new List<XYZ>();

                ListaPtosPerimetroBarras.Add(_Obtener4ptosMuro._p1);
                ListaPtosPerimetroBarras.Add(_Obtener4ptosMuro._p2);
                ListaPtosPerimetroBarras.Add(_Obtener4ptosMuro._p3);
                ListaPtosPerimetroBarras.Add(_Obtener4ptosMuro._p4);

                _datosNuevaBarraDTOIniciales.PtoCodoDireztriz = _puntosSeleccionMouse.p1_origenDiretriz;
                _datosNuevaBarraDTOIniciales.PtoDireccionDirectriz = _puntosSeleccionMouse.p2_SentidoDiretriz;
                _datosNuevaBarraDTOIniciales.EspesorElemento_foot = _seleccionarElementos._espesorMuroFoot;


                //  _datosNuevaBarraDTOIniciales.TipoPataFun = _Obtener4ptosFund.TipoPataFun;

                CargadorPAthReinf.CrearSupleMuro(_uiapp, muro, ListaPtosPerimetroBarras, _datosNuevaBarraDTOIniciales, 
                    (_datosNuevaBarraDTOIniciales.tipoSuple==TipoRefuerzoMuroSUple.Exterior?true:false) );


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en 'RefuerzoSupleMuroManejador' ex:{ex.Message}");
                resul = Result.Failed;
            }

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);

            return resul;
        }

        private bool M1_CalculosIniciales()
        {
            return true;
        }

    }
}