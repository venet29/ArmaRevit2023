using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.AnalisisRoom;

using ArmaduraLosaRevit.Model.AnalisisRoom.BordeRoom;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzo;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Ocultar;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.BarraRefuerzoCreador
{

    /// <summary>
    /// objeto que se utiliza cuando los borde de room  'BoundarySegmen' busca elemnto conttiguo  o vecino
    /// si encuntra una viga se crea objecto 'BarraViga' que contiene los objetos
    /// -  BoundarySegmentNH del borde
    /// -  la viga encontrada como vecino 'FamilyInstance'
    /// </summary>BoundarySegmen

    public class CreadorRefuerzoCabezaMuro
    {
        private readonly UIApplication _uiapp;


        #region 0)propiedades
        private readonly ICalculosRefuerzo _calculosRefuerzoTipoViga;
        private readonly IGeometriaTag _iGeometriaTagRefuerzo;
        private readonly Floor _floor;
        private readonly Document _doc;

        public List<BarraRefuerzoBordeLibre> listaDibujadosRefuerzo { get; set; }

        #endregion



        #region 1) Constructores
        public CreadorRefuerzoCabezaMuro(UIApplication _uiapp, ICalculosRefuerzo calculosRefuerzoTipoViga, IGeometriaTag iGeometriaTagRefuerzo, Floor floor)
        {
            this._uiapp = _uiapp;
            this._calculosRefuerzoTipoViga = calculosRefuerzoTipoViga;
            this._iGeometriaTagRefuerzo = iGeometriaTagRefuerzo;
            this._floor = floor;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.listaDibujadosRefuerzo = new List<BarraRefuerzoBordeLibre>();
        }

        public bool Ejecutar()
        {
            //DibujarEstribo();
            //DibujarBarra();
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("DibujarBarra-NH");

                    if (!DibujarBarra_sinTrans()) return false;
                    t.Commit();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }

            if (!CambiarColor_ConTrans()) return false;

            return true;
        }

        private void DibujarBarra()
        {
            int count = 1;
            int ultimoBarraSup = _calculosRefuerzoTipoViga.ListaBArrasSuperior.Count;
            foreach (CalculoBarraRefuerzo br in _calculosRefuerzoTipoViga.ListaBArrasSuperior)
            {
                DtoCrearBarraRefuerzoBordeLibre _newDToBRBL = new DtoCrearBarraRefuerzoBordeLibre()
                {
                    TipoBorde = TipoBorde.estribo,
                    floor = _floor,
                    br = br,
                    _iGeometriaTagRefuerzo = _iGeometriaTagRefuerzo,
                    numeroBarra = _calculosRefuerzoTipoViga.NumeroBArras,
                    textREfuerzoBarra = "F'=",
                    _TipoRebar = TipoRebar.REFUERZO_BA_CAB_MURO
                };

                BarraRefuerzoBordeLibre nwBArra = new BarraRefuerzoBordeLibre(_uiapp, _newDToBRBL);
                nwBArra.generarBarra_ConTrans();


                if (ultimoBarraSup == count)
                {
                    var _configuracionTAgEstriboDTo = new ConfiguracionTAgBarraDTo()
                    {
                        desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                        IsDIrectriz = false,
                        tagOrientation = TagOrientation.Horizontal,
                    };
                    nwBArra.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo);

                }
                count += 1;
            }
            foreach (CalculoBarraRefuerzo br in _calculosRefuerzoTipoViga.ListaBArrasInferior)
            {
                DtoCrearBarraRefuerzoBordeLibre _newDToBRBL = new DtoCrearBarraRefuerzoBordeLibre()
                {
                    TipoBorde = TipoBorde.estribo,
                    floor = _floor,
                    br = br,
                    _iGeometriaTagRefuerzo = _iGeometriaTagRefuerzo,
                    numeroBarra = _calculosRefuerzoTipoViga.NumeroBArras,
                    textREfuerzoBarra = "F'=",
                    _TipoRebar = TipoRebar.REFUERZO_BA_CAB_MURO
                };

                BarraRefuerzoBordeLibre nwBArra = new BarraRefuerzoBordeLibre(_uiapp, _newDToBRBL);
                nwBArra.generarBarra_ConTrans();
            }
        }

        private bool DibujarBarra_sinTrans()
        {
            try
            {
                int count = 1;
                int ultimoBarraSup = _calculosRefuerzoTipoViga.ListaBArrasSuperior.Count;
                foreach (CalculoBarraRefuerzo br in _calculosRefuerzoTipoViga.ListaBArrasSuperior)
                {
                    DtoCrearBarraRefuerzoBordeLibre _newDToBRBL = new DtoCrearBarraRefuerzoBordeLibre()
                    {
                        TipoBorde = TipoBorde.estribo,
                        floor = _floor,
                        br = br,
                        _iGeometriaTagRefuerzo = _iGeometriaTagRefuerzo,
                        numeroBarra = _calculosRefuerzoTipoViga.NumeroBArras,
                        textREfuerzoBarra = "F'=",
                        _TipoRebar = TipoRebar.REFUERZO_BA_CAB_MURO
                    };
                    BarraRefuerzoBordeLibre nwBArra = new BarraRefuerzoBordeLibre(_uiapp, _newDToBRBL);
                    if (!nwBArra.generarBarra_SinTrans()) return false;


                    if (ultimoBarraSup == count)
                    {
                        var _configuracionTAgEstriboDTo = new ConfiguracionTAgBarraDTo()
                        {
                            desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                            IsDIrectriz = false,
                            tagOrientation = TagOrientation.Horizontal,
                        };
                        if (!nwBArra.DibujarTagRebarRefuerzoLosa_SinTrans(_configuracionTAgEstriboDTo)) return false;
                    }
                    listaDibujadosRefuerzo.Add(nwBArra);
                    count += 1;
                }

                foreach (CalculoBarraRefuerzo br in _calculosRefuerzoTipoViga.ListaBArrasInferior)
                {
                    DtoCrearBarraRefuerzoBordeLibre _newDToBRBL = new DtoCrearBarraRefuerzoBordeLibre()
                    {
                        TipoBorde = TipoBorde.estribo,
                        floor = _floor,
                        br = br,
                        _iGeometriaTagRefuerzo = _iGeometriaTagRefuerzo,
                        numeroBarra = _calculosRefuerzoTipoViga.NumeroBArras,
                        textREfuerzoBarra = "F'=",
                        _TipoRebar = TipoRebar.REFUERZO_BA_CAB_MURO
                    };
                    BarraRefuerzoBordeLibre nwBArra = new BarraRefuerzoBordeLibre(_uiapp, _newDToBRBL);
                    if (!nwBArra.generarBarra_SinTrans()) return false;


                    listaDibujadosRefuerzo.Add(nwBArra);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true; ;
        }


        #endregion


        #region 2) metodos
        public bool CambiarColor_ConTrans()
        {
            if (listaDibujadosRefuerzo == null) return false;
            if (listaDibujadosRefuerzo.Count == 0) return true;
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarColorTipoViga-NH");

                    //barra
                    foreach (BarraRefuerzoBordeLibre er in listaDibujadosRefuerzo)
                    {
                        if (er.M1_6_visualizar_sintrans() == Result.Cancelled) return false;
                    }

                    //NOTA 01-10-2021 -> OCUltaba los refuerzo tipo viga en losas. si no hay inconveniente borrar las lineas 5 inferiore
                    //  OcultarBarras_refuerzoLosa _OcultarBarras_refuerzoLosa = new OcultarBarras_refuerzoLosa(_doc);
                    //  _OcultarBarras_refuerzoLosa.OcultarVariosBarraRefuerzo_SINTrans(listaDibujadosRefuerzo.Select(r => (Element)r._rebar).ToList());
                    //  OcultarBarrasRebaroPathrein _OcultarBarras_refuerzoLosa = new OcultarBarrasRebaroPathrein(_doc);
                    //_OcultarBarras_refuerzoLosa.OcultarListaBarraCreada_SinTrans(listaDibujadosRefuerzo.Select(r => (Element)r._rebar).ToList());
                    t.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        #endregion

    }
}
