using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Dibujar2D;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.Desglose.WPF;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.AyudasView;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose
{
    public class ManejadorDesgloseV
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly UI_desglose _ui;
        private bool IScrearCorte;
        private CrearViewNH _CrearView;

        private View _ViewOriginal;


        public ManejadorDesgloseV(UIApplication uiapp, UI_desglose _ui)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._ui = _ui;
            this.IScrearCorte = true;
        }

        public bool EjecutarDibujarBarrasEnElevacionV(Config_EspecialElev _Config_EspecialElv)
        {
            try
            {
                
                // ThisApplication _ThisApplication = new ThisApplication(_uiapp);
                //  _ThisApplication.C4_M2_InfoGeometriaAnidada();
                bool isId = (bool)_ui.chb_id.IsChecked;
                SeleccionarRebarRectangulo administrador_ReferenciaRoom = new SeleccionarRebarRectangulo(_uiapp);
                if (!administrador_ReferenciaRoom.GetUnicamenteRebarSeleccionadosConRectaguloYFiltros()) return false;

                if (!AyudaObtenerListaDesglosada.ObtenerLista(administrador_ReferenciaRoom._ListaRebarSeleccionado, _uiapp)) return false;

                //a)BARRAS
                GruposListasTraslapo_V _GruposListasTraslapo = new GruposListasTraslapo_V(_uiapp, AyudaObtenerListaDesglosada.Lista_RebarDesglose);
                if (!_GruposListasTraslapo.ObtenerGruposTraslapos()) return false;

                GruposListasTraslapoIguales_V _GruposListasTraslapoIguales = new GruposListasTraslapoIguales_V(_uiapp, _GruposListasTraslapo.GruposRebarMismaLinea);
                if (!_GruposListasTraslapoIguales.ObtenerGruposTraslaposIguales(_Config_EspecialElv)) return false;

                //a)ESTRIBO
                GruposListasEstribo_V _GruposListasEstribo = new GruposListasEstribo_V(_uiapp, AyudaObtenerListaDesglosada.Lista_RebarDesglose);
                if (!_GruposListasEstribo.ObtenerGruposEstribo()) return false;

                try
                {
                    using (TransactionGroup t = new TransactionGroup(_doc))
                    {
                        t.Start("Crear Elevacion");
                        //b)dibujar  barra
                        Dibujar2D_Barra_elevacion_V _Dibujar2D_elevcion = new Dibujar2D_Barra_elevacion_V(_uiapp, _GruposListasTraslapoIguales, _Config_EspecialElv);
                        if (_Dibujar2D_elevcion.PreDibujar(isId))
                            _Dibujar2D_elevcion.Dibujar();

                        //b)dibujar estribo
                        Dibujar2D_Estribos_elevacion_V _Dibujar2D_Estribo_Elev = new Dibujar2D_Estribos_elevacion_V(_uiapp, _GruposListasEstribo);
                        _Dibujar2D_Estribo_Elev.Dibujar(_Dibujar2D_elevcion.posicionInicial);
                        t.Assimilate();
                    }
                }
                catch (Exception ex)
                {
                    string msj = ex.Message;
                }


            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        public bool EjecutarDibujarBarrasEncorteV(Config_EspecialCorte _Config_EspecialCorte)
        {
            try
            { 
                _CrearView = null;

                var  uidoc=_uiapp.ActiveUIDocument;

                _ViewOriginal = _uiapp.ActiveUIDocument.ActiveView;

                SeleccionarRebarRectangulo administrador_ReferenciaRoom = new SeleccionarRebarRectangulo(_uiapp);
                if (!administrador_ReferenciaRoom.GetUnicamenteRebarSeleccionadosConRectaguloYFiltros()) return false;

                if (!AyudaObtenerListaDesglosada.ObtenerLista(administrador_ReferenciaRoom._ListaRebarSeleccionado, _uiapp)) return false;

                //ESTRIBO
                GruposListasEstribo_V _GruposListasEstribo = new GruposListasEstribo_V(_uiapp, AyudaObtenerListaDesglosada.Lista_RebarDesglose);
                if (!_GruposListasEstribo.ObtenerGruposEstribo()) return false;

                if (!_GruposListasEstribo.ObteneGruposDeBarraEnELev()) return false;

                CalcularREferenciasCaraPilar _CalcularREferenciasCaraPilar = new CalcularREferenciasCaraPilar(_uiapp, AyudaObtenerListaDesglosada.Lista_RebarDesglose,_ViewOriginal);
                if (!_CalcularREferenciasCaraPilar.Calcular()) return false;


                if (!GenerarCorte(_GruposListasEstribo)) return false;

                if (_GruposListasEstribo.GruposRebarMismaLinea.Count != 1)
                {
                    Util.ErrorMsg($"Error al seleccionar estribos. Se han seleccionado {_GruposListasEstribo.GruposRebarMismaLinea.Count} grupos de estribos, se debe seleccionar solo 1 grupo de Estribo");
                }


                try
                {
                    using (TransactionGroup t = new TransactionGroup(_doc))
                    {
                        t.Start("Crear cortes");

                        Dibujar2D_Estribos_Corte_V _Dibujar2D_Estribos_Corte = new Dibujar2D_Estribos_Corte_V(_uiapp, _GruposListasEstribo, _Config_EspecialCorte);
                        if (_Dibujar2D_Estribos_Corte.PreDibujar(_CrearView.section, _ViewOriginal))
                        {
                            _Dibujar2D_Estribos_Corte.Dibujar();
                        }

                        Dibujar2D_Barra_Corte_TAg_V _dibujar2D_Barra_Corte_TAg = new Dibujar2D_Barra_Corte_TAg_V(_uiapp, 
                                                    _GruposListasEstribo.listaBArrasEnElev, _GruposListasEstribo._DatosHost.CaraCentral, _Config_EspecialCorte);
                        _dibujar2D_Barra_Corte_TAg.CrearTAg(_CrearView.section);


                        _CalcularREferenciasCaraPilar.GenerarDimesionesPilar4lados(_Dibujar2D_Estribos_Corte.ptoCentroPilarAlturaCOrte);


                        t.Assimilate();
                    }
                }
                catch (Exception ex)
                {
                    string msj = ex.Message;
                }



            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private bool GenerarCorte(GruposListasEstribo_V _GruposListasEstribo)
        {
            if (IScrearCorte)
            {
                List<XYZ> listaPto = _GruposListasEstribo.ListaPtoSeccion;// ()  CrearListaPtos.M2_ListaPtoSimple(_uiapp, 2);
                _CrearView = new CrearViewNH(_doc, 20, Util.FootToCm(_GruposListasEstribo._DatosHost.LargoMAximoHost_foot * 0.8), _ui.dtnameCorte.Text);
                _CrearView.M2_CrearDetailViewConTrasn2(listaPto);
                if (_CrearView == null)
                {
                    Util.ErrorMsg($"Error al crear corte  View=null");
                    return false;
                }
                _uiapp.ActiveUIDocument.ActiveView = _CrearView.section;
            }


            if (_CrearView == null)
            {
                Util.ErrorMsg($"Error al crear corte  View=null");
                return false;
            }
            if (_CrearView.section == null)
            {
                Util.ErrorMsg($"Error al crear corte  View=null");
                return false;
            }
            return true;
        }
    }
}
