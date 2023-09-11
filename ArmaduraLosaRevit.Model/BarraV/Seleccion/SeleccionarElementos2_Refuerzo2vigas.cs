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

    public class SeleccionarElementos2_Refuerzo2vigas : SeleccionarElementosH
    {

        public double Largo1  { get; set; }
        public double Largo2 { get; set; }
        public SeleccionarElementosHAuto _seleccionarElementos1_auto { get; set; }
        public SeleccionarElementosHAuto _seleccionarElementos2_auto { get; set; }
        public DatosMuroSeleccionadoDTO _vigaSeleccionadoDTO { get; set; }
        public List<XYZ> _listaptoTramo { get; set; }
        public TipoCaraObjeto UbicacionBarras { get; internal set; }

        XYZ p1;
        XYZ p2;
        private CoordenadasElementoDTO CoordenadasElementoDTO1;
        private CoordenadasElementoDTO CoordenadasElementoDTO2;

        public SeleccionarElementos2_Refuerzo2vigas(UIApplication _uiapp, ConfiguracionInicialBarraHorizontalDTO _configuracionInicialBarraHorizontalDTO, DireccionRecorrido _DireccionRecorrido)
            : base(_uiapp, _configuracionInicialBarraHorizontalDTO, _DireccionRecorrido)
        {
            p1 = XYZ.Zero;
            p2 = XYZ.Zero;
        }


        public override bool M1_ObtenerPtoinicio_RefuerzoBorde()
        {
            _seleccionarElementos1_auto = new SeleccionarElementosHAuto(_uiapp, _configuracionInicialBarraHorizontalDTO, _direccionRecorrido);

            if (!_seleccionarElementos1_auto.M1_ObtenerPtoinicio_RefuerzoBorde()) return false;

            _seleccionarElementos2_auto = new SeleccionarElementosHAuto(_uiapp, _configuracionInicialBarraHorizontalDTO, _direccionRecorrido);

            if (!_seleccionarElementos2_auto.M1_ObtenerPtoinicio_RefuerzoBorde()) return false;

            if (_seleccionarElementos1_auto.ElemetSelect.Id.IntegerValue == _seleccionarElementos2_auto.ElemetSelect.Id.IntegerValue)
            {
                Util.ErrorMsg("Error no se puede seleccionar la misma viga.");
                return false;
            }

            ElemetSelect = _seleccionarElementos1_auto.ElemetSelect;
            CoordenadasElementoDTO1 = _seleccionarElementos1_auto.CoordenadasElementoDTO;
            CoordenadasElementoDTO2 = _seleccionarElementos2_auto.CoordenadasElementoDTO;

            PtoSeleccionMouseCentroCaraMuro6 = _seleccionarElementos1_auto.PtoSeleccionMouseCentroCaraMuro6;
            PtoSeleccionMouseCaraMuro = _seleccionarElementos1_auto.PtoSeleccionMouseCaraMuro;

            UbicacionBarras = _seleccionarElementos1_auto.Obtener_UbicacionBarras_Altura();

            EspesorViga = _seleccionarElementos1_auto.EspesorViga;
            DireccionMuro6= _seleccionarElementos1_auto.DireccionMuro6;
            Largo1 = _seleccionarElementos1_auto.LargoViga;
            Largo2 = _seleccionarElementos2_auto.LargoViga;
            return true;
        }



        public bool M1_6_ObtenerListaIntervalos(TipoCaraObjeto _ubicacionBarras)
        {
            _listaptoTramo = new List<XYZ>();
           // XYZ recubrimieto = _view.ViewDirection * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT; //con esto se obtiene los pto sobre la cara de seleccion
            try
            {
            

                if (TipoCaraObjeto.Superior == _ubicacionBarras)
                {
                    p1 = CoordenadasElementoDTO1._ptobarra2Sup_conrecub;
                    p2 = CoordenadasElementoDTO2._ptobarra1Sup_conrecub;

                    Line _lineaux = Line.CreateBound(CoordenadasElementoDTO1._ptobarra1Sup_conrecub, CoordenadasElementoDTO1._ptobarra2Sup_conrecub);
                    XYZ ptoInterSeccion = _lineaux.ProjectExtendida3D(p2);
                    p2 = p2.AsignarZ(ptoInterSeccion.Z);
                    XYZ direccionAux = (p2 - p1).Normalize();
                    if (Util.IsParallel_DistintoSentido(direccionAux, _view.RightDirection, 0.9))
                    {
                        p1 = CoordenadasElementoDTO1._ptobarra1Sup_conrecub;
                        p2 = CoordenadasElementoDTO2._ptobarra2Sup_conrecub;

                        Line _lineaux2 = Line.CreateBound(CoordenadasElementoDTO1._ptobarra1Sup_conrecub, CoordenadasElementoDTO1._ptobarra2Sup_conrecub);
                        XYZ ptoInterSeccion2 = _lineaux.ProjectExtendida3D(p2);
                        p2 = p2.AsignarZ(ptoInterSeccion.Z);
                        PtoSeleccionMouseCentroCaraMuro6 = _seleccionarElementos2_auto.PtoSeleccionMouseCentroCaraMuro6;
                    }

                    
                    PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = (p1 + p2) / 2 + (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
                }
                else
                {
                    p1 = CoordenadasElementoDTO1._ptobarra2Inf_conrecub;
                    p2 = CoordenadasElementoDTO2._ptobarra1Inf_conrecub;

                    Line _lineaux = Line.CreateBound(CoordenadasElementoDTO1._ptobarra1Inf_conrecub, CoordenadasElementoDTO1._ptobarra2Inf_conrecub);
                    XYZ ptoInterSeccion = _lineaux.ProjectExtendida3D(p2);

                    p2 = p2.AsignarZ(ptoInterSeccion.Z);
                    XYZ direccionAux = (p2 - p1).Normalize();
                    if (Util.IsParallel_DistintoSentido(direccionAux, _view.RightDirection, 0.9))
                    {
                        p1 = CoordenadasElementoDTO2._ptobarra2Inf_conrecub;
                        p2 = CoordenadasElementoDTO1._ptobarra1Inf_conrecub;

                        Line _lineaux2 = Line.CreateBound(CoordenadasElementoDTO1._ptobarra1Inf_conrecub, CoordenadasElementoDTO1._ptobarra1Inf_conrecub);
                        XYZ ptoInterSeccion2 = _lineaux.ProjectExtendida3D(p2);
                        p2 = p2.AsignarZ(ptoInterSeccion.Z);

                        PtoSeleccionMouseCentroCaraMuro6 = _seleccionarElementos2_auto.PtoSeleccionMouseCentroCaraMuro6;
                    }


                    PtoInicioBaseBordeViga_ProyectadoCaraMuroHost = (p1 + p2) / 2 - (new XYZ(0, 0, 1) * ConstNH.RECUBRIMIENTO_ESTRIBO_FOOT);
                }

                p2 = p2.AsignarZ(p1.Z);

                DireccionBordeElemeto = (p2 - p1).Normalize();

                //esto por si no hay muro de separacion entre viga(coronacion) entonces p1==p2
                if (DireccionBordeElemeto.IsAlmostEqualTo(XYZ.Zero))
                    DireccionBordeElemeto = (CoordenadasElementoDTO1._ptobarra2Sup_conrecub - CoordenadasElementoDTO1._ptobarra1Sup_conrecub).Normalize();

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
