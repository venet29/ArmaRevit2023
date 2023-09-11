using ApiRevit.FILTROS;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.Seleccion.Vigas;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{

    public class SeleccionarElementos_RefuerzoBorde : SeleccionarElementosH
    {
        public double Largo1 { get; set; }

        public SeleccionarElementosHAuto _seleccionarElementos1_auto { get; set; }

        public DatosMuroSeleccionadoDTO _vigaSeleccionadoDTO { get; set; }
        public List<XYZ> _listaptoTramo { get; set; }
        public TipoCaraObjeto UbicacionBarras { get; set; }

        public TipoCaraObjeto UbicacionBarrasLado { get; set; }
        XYZ p1;
        XYZ p2;
        private CoordenadasElementoDTO CoordenadasElementoDTO1;

        public SeleccionarElementos_RefuerzoBorde(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO, DireccionRecorrido _DireccionRecorrido)
            : base(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido)
        {
            p1 = XYZ.Zero;
            p2 = XYZ.Zero;
        }

        public override bool M1_ObtenerPtoinicio_RefuerzoBorde()
        {
            _seleccionarElementos1_auto = new SeleccionarElementosHAuto(_uiapp, _configuracionInicialBarraHorizontalDTO, _direccionRecorrido);

            if (!_seleccionarElementos1_auto.M1_ObtenerPtoinicio_RefuerzoBorde()) return false;

            ElemetSelect = _seleccionarElementos1_auto.ElemetSelect;
            CoordenadasElementoDTO1 = _seleccionarElementos1_auto.CoordenadasElementoDTO;

            PtoSeleccionMouseCentroCaraMuro6 = _seleccionarElementos1_auto.PtoSeleccionMouseCaraMuro;
            PtoSeleccionMouseCaraMuro = _seleccionarElementos1_auto.PtoSeleccionMouseCaraMuro;
            UbicacionBarras = _seleccionarElementos1_auto.Obtener_UbicacionBarras_Altura();

            UbicacionBarrasLado = _seleccionarElementos1_auto.Obtener_UbicacionBarras_IZqOderecha();

            EspesorViga = _seleccionarElementos1_auto.EspesorViga;
            DireccionMuro6 = _seleccionarElementos1_auto.DireccionMuro6;
            Largo1 = _seleccionarElementos1_auto.LargoViga;

            return true;
        }



        public bool M1_6_ObtenerListaIntervalos()
        {
            _listaptoTramo = new List<XYZ>();
            // XYZ recubrimieto = _view.ViewDirection * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT; //con esto se obtiene los pto sobre la cara de seleccion
            try
            {
                if (TipoCaraObjeto.Superior == UbicacionBarras)
                {
                    if (UbicacionBarrasLado == TipoCaraObjeto.Izquierdo)
                    {
                        p1 = CoordenadasElementoDTO1._ptobarra1Sup_conrecub;
                        p2 = CoordenadasElementoDTO1._ptobarra1Sup_conrecub;
                    }
                    else if (UbicacionBarrasLado == TipoCaraObjeto.Derecho)
                    {

                        p1 = CoordenadasElementoDTO1._ptobarra2Sup_conrecub;
                        p2 = CoordenadasElementoDTO1._ptobarra2Sup_conrecub;
                    }
                    PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = (p1 + p2) / 2 + (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
                }
                else
                {
                    if (UbicacionBarrasLado == TipoCaraObjeto.Izquierdo)
                    {
                        p1 = CoordenadasElementoDTO1._ptobarra1Inf_conrecub;
                        p2 = CoordenadasElementoDTO1._ptobarra1Inf_conrecub;
                    }
                    else if (UbicacionBarrasLado == TipoCaraObjeto.Derecho)
                    {

                        p1 = CoordenadasElementoDTO1._ptobarra2Inf_conrecub;
                        p2 = CoordenadasElementoDTO1._ptobarra2Inf_conrecub;
                    }

                    PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = (p1 + p2) / 2 - (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
                }

                //  p2 = p2.AsignarZ(p1.Z);

                DireccionBordeElemeto = _view.RightDirection;

              _PtoInicioBaseBordeViga6 = PtoInicioBaseBordeViga_ProyectadoCaraMuroHost.ObtenerCopia();

                _listaptoTramo.Add(p1);
                _listaptoTramo.Add(p2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }
            return true;
        }

   
    }
}
