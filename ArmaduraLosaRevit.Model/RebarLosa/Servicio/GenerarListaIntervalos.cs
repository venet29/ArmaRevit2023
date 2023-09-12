using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.ListaPtos;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.RebarLosa.Servicio
{
    public enum TipoPosicionMouse { centroPath, segunMouse }
    public class GenerarListaIntervalosDTo
    {
        public string _tipoBarra { get; set; }
        public XYZ ptoMouse { get; set; }
        public List<XYZ> ListaIntervalos { get; set; }
        public UbicacionLosa _ubicacionEnlosa { get; set; }
    }


    public class ParametrosListaIntervalosDTo
    {
        public UIApplication uiapp { get; set; }
        public BarraRoom barraRoom { get; set; }
        public UbicacionPtoMouse _ubicacionPtoMouse { get; set; }
        public TipoPosicionMouse _tipoPosicionMouse { get; set; }
        public bool _Iscaso_intervalo { get; set; }
        public bool IsPataInicial { get; set; } = false;
        public bool IsPataFinal { get; set; } = false;
        public int _diametroMM { get; set; }
        public double _espaciamiento { get; set; }
        public List<XYZ> ListaPtosPerimetroBarras { get; set; }
        public XYZ PtoSeleccionMouse1 { get; set; }
        public string TipoBarra { get; internal set; }
        public UbicacionLosa ubicacionEnlosa { get; set; }
    }


    public class GenerarListaIntervalos
    {
        protected UIApplication _uiapp;
        protected View _view;

        // private readonly ElementoBarraRoom.BarraRoom _barraRoom;
        private readonly UbicacionPtoMouse _ubicacionPtoMouse;
        private TipoPosicionMouse _tipoPosicionMouse;
        private bool _Iscaso_intervalo;
        private bool IsPataInicial;
        private bool IsPataFinal;
        protected   int diametroMM;
        private double espaciamiento;
        private XYZ PtoSeleccionMouse1;
        protected List<XYZ> ListaPtosPerimetroBarras;
        public List<XYZ> ListaPtosPerimetroBarrasParaDimension { get; set; }
        private XYZ dire_2_to_1;
        private XYZ dire_0_to_1;
        protected List<XYZ> listaPtos;
        private string _TipoBarra;
        private UbicacionLosa _ubicacionEnlosa;
        protected double factor;

        public List<GenerarListaIntervalosDTo> ListaIntervalosDTO { get; set; }
        public GenerarListaIntervalos(ParametrosListaIntervalosDTo _parametrosListaIntervalosDTo)
        {
            _uiapp = _parametrosListaIntervalosDTo.uiapp;
            _view = _uiapp.ActiveUIDocument.ActiveView;
            // this._barraRoom = _parametrosListaIntervalosDTo.barraRoom;
            this._ubicacionPtoMouse = _parametrosListaIntervalosDTo._ubicacionPtoMouse;
            this._tipoPosicionMouse = _parametrosListaIntervalosDTo._tipoPosicionMouse;
            this._Iscaso_intervalo = _parametrosListaIntervalosDTo._Iscaso_intervalo;
            this.IsPataInicial = _parametrosListaIntervalosDTo.IsPataInicial;
            this.IsPataFinal = _parametrosListaIntervalosDTo.IsPataFinal;

            this.diametroMM = _parametrosListaIntervalosDTo._diametroMM;
            this.espaciamiento = _parametrosListaIntervalosDTo._espaciamiento;
            this.PtoSeleccionMouse1 = _parametrosListaIntervalosDTo.PtoSeleccionMouse1;
            this.ListaPtosPerimetroBarras = _parametrosListaIntervalosDTo.ListaPtosPerimetroBarras;

            _TipoBarra = _parametrosListaIntervalosDTo.TipoBarra;
            _ubicacionEnlosa = _parametrosListaIntervalosDTo.ubicacionEnlosa;
            this.ListaPtosPerimetroBarrasParaDimension = new List<XYZ>();
            this.ListaIntervalosDTO = new List<GenerarListaIntervalosDTo>();
            this.listaPtos = new List<XYZ>();
            factor = (_ubicacionPtoMouse == UbicacionPtoMouse.inferior ? 0.5 : 1.5);
        }


        public virtual bool M1_ObtenerIntervalos()
        {
            try
            {


                if (!M1_1_CalculosIniciales()) return false;

                //MOVER POLIGO MEDIO ESPACIAMIENTO
                M1_2_MoverPoligonoMedioEspaciamiento();

                if (!M1_3_ObtenerPtosDefinirPath()) return false;


                double largoTraslapo = UtilBarras.largo_traslapoFoot_diamMM(diametroMM);
                // List<XYZ> intervalo = new List<XYZ>();


                XYZ pto_p1 = ListaPtosPerimetroBarras[0];
                XYZ pto_p2 = ListaPtosPerimetroBarras[1];

                if (listaPtos.Count == 0)
                {
                    ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                    {
                        ListaIntervalos = ListaPtosPerimetroBarras,
                        ptoMouse = PtoSeleccionMouse1,
                        _tipoBarra = _TipoBarra,
                        _ubicacionEnlosa = _ubicacionEnlosa
                    });
                    return false;
                }

                int dire = 1;

                List<XYZ> intervalo = new List<XYZ>();

                bool isInicial = true;
                foreach (XYZ pto_ in listaPtos)
                {
                    XYZ pto_p4 = Line.CreateBound(ListaPtosPerimetroBarras[0], ListaPtosPerimetroBarras[3]).ProjectSINExtendida3D(pto_) + dire_2_to_1 * largoTraslapo / 2;

                    XYZ pto_p3 = Line.CreateBound(ListaPtosPerimetroBarras[1], ListaPtosPerimetroBarras[2]).ProjectSINExtendida3D(pto_) + dire_2_to_1 * largoTraslapo / 2;

                    intervalo = new List<XYZ>();
                    intervalo.Add(pto_p1);
                    intervalo.Add(pto_p2);
                    intervalo.Add(pto_p3);
                    intervalo.Add(pto_p4);
                    if (isInicial)
                    {
                        ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                        {
                            ListaIntervalos = intervalo,
                            ptoMouse = M1_4_ObtenerPtoMouse(intervalo, dire, _tipoPosicionMouse),
                            _tipoBarra = (IsPataInicial ? TipoBarra.f1_incli.ToString() : _TipoBarra),
                            _ubicacionEnlosa = _ubicacionEnlosa

                        });
                        isInicial = false;
                    }
                    else
                    {
                        ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                        {
                            ListaIntervalos = intervalo,
                            ptoMouse = M1_4_ObtenerPtoMouse(intervalo, dire, _tipoPosicionMouse),
                            _tipoBarra = _TipoBarra,
                            _ubicacionEnlosa = _ubicacionEnlosa
                        }); //(intervalo[0]+ intervalo[2])/2 + dire_0_to_1*Util.CmToFoot(1)*dire   _  ObtenerPtoMouse(intervalo, dire)
                    }
                    //recalcular
                    pto_p1 = pto_p4.GEtNewXYZNH() - dire_2_to_1 * largoTraslapo;
                    pto_p2 = pto_p3.GEtNewXYZNH() - dire_2_to_1 * largoTraslapo;
                    dire = dire * -1;
                }


                //linea final
                intervalo = new List<XYZ>();
                intervalo.Add(pto_p1);
                intervalo.Add(pto_p2);
                intervalo.Add(ListaPtosPerimetroBarras[2]);
                intervalo.Add(ListaPtosPerimetroBarras[3]);
                ListaIntervalosDTO.Add(new GenerarListaIntervalosDTo()
                {
                    ListaIntervalos = intervalo,
                    ptoMouse = M1_4_ObtenerPtoMouse(intervalo, dire, _tipoPosicionMouse),
                    _tipoBarra = (IsPataFinal ? TipoBarra.f1_incli.ToString() : _TipoBarra),
                    _ubicacionEnlosa = (IsPataFinal
                                        ? (_ubicacionEnlosa == UbicacionLosa.Inferior ? UbicacionLosa.Superior : UbicacionLosa.Derecha)
                                        : _ubicacionEnlosa)
                });
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg($"Error al ");
                return false;
            }
            return true;
        }
        private bool M1_1_CalculosIniciales()
        {
            if (ListaPtosPerimetroBarras == null)
            {
                Util.ErrorMsg("Error ListaPtosPerimetroBarras ==null ");
                return false;
            }
            if (ListaPtosPerimetroBarras.Count != 4)
            {
                Util.ErrorMsg("Error ListaPtosPerimetroBarras debe contener 4 puntos ");
                return false;
            }
            dire_2_to_1 = (ListaPtosPerimetroBarras[2] - ListaPtosPerimetroBarras[1]).Normalize();
            dire_0_to_1 = (ListaPtosPerimetroBarras[0] - ListaPtosPerimetroBarras[1]).Normalize();

            return true;
        }
        private void M1_2_MoverPoligonoMedioEspaciamiento()
        {
            ListaPtosPerimetroBarrasParaDimension.AddRange(ListaPtosPerimetroBarras);
         
            ListaPtosPerimetroBarras[1] = ListaPtosPerimetroBarras[1] + dire_0_to_1 * espaciamiento * factor;
            ListaPtosPerimetroBarras[2] = ListaPtosPerimetroBarras[2] + dire_0_to_1 * espaciamiento * factor;
        }

        protected bool M1_3_ObtenerPtosDefinirPath()
        {
            try
            {
                if (_Iscaso_intervalo)
                {
                    ListaPtoTramoResultadoDTo result = ObtenerListaPtos.M2_ListaPtoTramoPorIntervalos(ListaPtosPerimetroBarras, diametroMM, PtoSeleccionMouse1);
                    if (result.IsOk == false) return false;
                    listaPtos = result._listaptoTram;
                }
                else// con mouse
                    listaPtos = CrearListaPtos.M2_ListaPtoTramo(_uiapp.ActiveUIDocument, ListaPtosPerimetroBarras);
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        //mueve path encaso que se ainferior
        private XYZ M1_4_ObtenerPtoMouse(List<XYZ> intervalo, int dire, TipoPosicionMouse _tipoPosicionMouse)
        {
            XYZ result = XYZ.Zero;
            if (_tipoPosicionMouse == TipoPosicionMouse.centroPath)
            {
                result = (intervalo[0] + intervalo[2]) / 2 + dire_0_to_1 * Util.CmToFoot(1) * dire;
            }
            else if (_tipoPosicionMouse == TipoPosicionMouse.segunMouse)
            {
                XYZ ptoCentral = (intervalo[0] + intervalo[2]) / 2 + dire_0_to_1 * Util.CmToFoot(1) * dire;

                XYZ pto_1_4 = Line.CreateBound(intervalo[0], intervalo[3]).ProjectSINExtendida3D(ptoCentral);
                XYZ pto_2_3 = Line.CreateBound(intervalo[1], intervalo[2]).ProjectSINExtendida3D(ptoCentral);

                result = Line.CreateBound(pto_1_4, pto_2_3).ProjectSINExtendida3D(PtoSeleccionMouse1) + dire_0_to_1 * Util.CmToFoot(1) * dire;
            }

            return result;
        }


    }
}