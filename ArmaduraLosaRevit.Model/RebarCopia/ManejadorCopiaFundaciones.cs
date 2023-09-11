using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa.Servicios;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.RebarCopia.Entidades;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;

namespace ArmaduraLosaRevit.Model.RebarCopia
{
    class ManejadorCopiaFundaciones
    {
        private UIApplication _uiapp;
        private View _view;
        private Document _doc;

        public List<ElementId> ListaElementoCopiados_id;
        private View3D _view3D_buscar;
        private List<XYZ> ListaDesplazamineto;
        private List<ElementId> _listaIdBarras;
        private List<ResulSelecionElementpCopiaDto> ListaDesplazaminetoColumna;
        private List<ResulSelecionElementpCopiaDto> ListaDesplazaminetoFundacion;

        public ManejadorCopiaFundaciones(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._doc = uiapp.ActiveUIDocument.Document;
            ListaElementoCopiados_id = new List<ElementId>();
            ListaDesplazaminetoFundacion = new List<ResulSelecionElementpCopiaDto>();
            ListaDesplazaminetoColumna = new List<ResulSelecionElementpCopiaDto>();
        }

        public bool EjecutarLosaFunda()
        {
            ISelectionFilter f = new FiltroFloorOrFund();
            if (!EjecutarFundacion(f)) return false;
            return CopiarElementoFundaciones(_listaIdBarras, ListaDesplazamineto);
        }

        public bool EjecutarColumna()
        {
            ISelectionFilter f = new FiltroColumns();
            if (!EjecutarColumna(f)) return false;

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);

             bool result= CopiarElementoColumna(_listaIdBarras) && MoverElementoColumna();

            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return result;
        }

        private bool EjecutarFundacion(ISelectionFilter f)
        {
            try
            {
                ListaDesplazamineto = new List<XYZ>();
                //seleccion elemento  // 
                SeleccionarLosaConMouse seleccionarLosaConMouse = new SeleccionarLosaConMouse(_uiapp);
                var elementoSeleccionado = seleccionarLosaConMouse.M1_Selecconar_segunFiltro(f);
                if (elementoSeleccionado == null)
                {
                    Util.ErrorMsg("No se encontro cara superior de elemento");
                    return false;
                }
                // obtenerc cara inferiror y centro

                var planarFaceInf = ((Element)elementoSeleccionado).ObtenerCaraInferior(seleccionarLosaConMouse._ptoSeleccionEnLosa, new XYZ(0, 0, -1));
               // var planarFaceInf = ((Element)elementoSeleccionado).ObtenerCaraInferiorElem();
                if (planarFaceInf == null) return false;

                XYZ centroCaraInf = planarFaceInf.ObtenerCenterDeCara();

                ElementoConBarras _ElementoConBarras = new ElementoConBarras(_uiapp, elementoSeleccionado);
                if (!_ElementoConBarras.Obtener()) return false;
                // obtener barras de elemento y desplazamiento respecto base

                List<ElementId> ListaElementosSelecionado = new List<ElementId>();

                bool isContinuar = true;
                while (isContinuar)
                {
                    // seleccionar nuevo elemeto
                    SeleccionarLosaConMouse seleccionarLosaParaCopiarMouse = new SeleccionarLosaConMouse(_uiapp);
                    var elementoSeleccionadoParaCopiar = seleccionarLosaParaCopiarMouse.M1_Selecconar_segunFiltro(f);
                    if (elementoSeleccionadoParaCopiar == null)
                        break;

                    if (ListaElementosSelecionado.Exists(c => c.IntegerValue == elementoSeleccionadoParaCopiar.Id.IntegerValue))
                        continue;

                    ListaElementosSelecionado.Add(elementoSeleccionadoParaCopiar.Id);

                    //var planarFaceInfCpiar = ((Element)elementoSeleccionadoParaCopiar).ObtenerCaraInferior(seleccionarLosaParaCopiarMouse._ptoSeleccionEnLosa, new XYZ(0, 0, -1));
                    var planarFaceInfCpiar = elementoSeleccionadoParaCopiar.ObtenerCaraInferior();
                    if (planarFaceInfCpiar == null) return false;

                    // obtenerc cara inferiror y centro
                    XYZ centroCaraCOpiar = planarFaceInfCpiar.ObtenerCenterDeCara();


                    XYZ deltaDesplazamineto = centroCaraCOpiar - centroCaraInf;

                    //ListaDesplazamineto.Add(deltaDesplazamineto);
                    ListaDesplazaminetoFundacion.Add(new ResulSelecionElementpCopiaDto()
                                {
                                    PtoDesfase = deltaDesplazamineto,
                                    ptoSobreCaraInferiorFundacion = centroCaraCOpiar,
                                    ElementoACopiar = (Element)elementoSeleccionadoParaCopiar
                                }
                    );

                }

                if (ListaDesplazaminetoFundacion.Count == 0) return false;
                //copiar con delta desplazamineto
                _listaIdBarras = _ElementoConBarras.ListaRebar.Select(c => c.Rebar.Id).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Util.ErrorMsg($"Error en 'ManejadorCopiaFundaciones'.\nex:{ex.Message} ");
                return false;
            }
            return true;
        }

        private bool CopiarElementoFundaciones(List<ElementId> listaELemento, List<XYZ> ListaVectorDeplazaminetos)
        {

            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start($"copiarBarrasSinColor-NH");
                    for (int i = 0; i < ListaDesplazaminetoFundacion.Count; i++)
                    {
                        var result = ListaDesplazaminetoFundacion[i];

                        var ListaCopiada = ElementTransformUtils.CopyElements(_doc, listaELemento, result.PtoDesfase).ToList();

                        for (int j = 0; j < ListaCopiada.Count; j++)
                        {
                            var _rebarCopiada = (Rebar)_doc.GetElement(ListaCopiada[j]);
                            _rebarCopiada.SetHostId(_doc, result.ElementoACopiar.Id);
                        }

                        ListaElementoCopiados_id = new List<ElementId>();
                        ListaElementoCopiados_id.AddRange(ListaCopiada);

                        CopiarParametrosBarrasCopiadas.M4_CopiarParametros(_doc, _view, ListaElementoCopiados_id);
                        result.ListaElementoCopiados_id = ListaElementoCopiados_id;
                    }
                    t.Commit();
                }

                //mover
                //using (Transaction t = new Transaction(_doc))
                //{
                //    t.Start($"copiarBarrasSinColor-NH");
                //    for (int i = 0; i < ListaDesplazaminetoFundacion.Count; i++)
                //    {
                //        var result = ListaDesplazaminetoColumna[i];

                //        for (int j = 0; j < result.ListaElementoCopiados_id.Count; j++)
                //        {
                //            XYZ desfase = result.ptoSobreCaraInferiorFundacion;

                //            var _barra = _doc.GetElement(result.ListaElementoCopiados_id[j]);

                //            if (_barra is PathReinforcement)
                //            {


                //            }
                //            else if (_barra is Rebar)
                //            {
                               
                //            }
                //        }
                //    }
                //    t.Commit();
                //}


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Util.ErrorMsg("No se puede mover barra");

            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return true;
        }
        //***********************

        public bool EjecutarColumna(ISelectionFilter f)
        {

            try
            {
                _view3D_buscar = TiposFamilia3D.Get3DBuscar(_doc);

                //seleccion elemento  // 
                SeleccionarLosaConMouse seleccionarLosaConMouse = new SeleccionarLosaConMouse(_uiapp);
                var elementoSeleccionado = seleccionarLosaConMouse.M1_Selecconar_segunFiltro(f);
                if (elementoSeleccionado == null) return false;
                // obtenerc cara inferiror y centro

                var planarFaceSup = ((Element)elementoSeleccionado).ObtenerCaraSuperior(seleccionarLosaConMouse._ptoSeleccionEnLosa, new XYZ(0, 0, -1));
                if (planarFaceSup == null)
                {
                    Util.ErrorMsg("No se encontro cara superior de elemento");
                    return false;
                }

                XYZ centroCaraInf = planarFaceSup.ObtenerCenterDeCara();

                ElementoConBarras _ElementoConBarras = new ElementoConBarras(_uiapp, elementoSeleccionado);
                if (!_ElementoConBarras.Obtener()) return false;
                // obtener barras de elemento y desplazamiento respecto base

                bool isContinuar = true;
                while (isContinuar)
                {
                    // seleccionar nuevo elemeto
                    SeleccionarLosaConMouse seleccionarLosaParaCopiarMouse = new SeleccionarLosaConMouse(_uiapp);
                    var elementoSeleccionadoParaCopiar = seleccionarLosaParaCopiarMouse.M1_Selecconar_segunFiltro(f);
                    if (elementoSeleccionadoParaCopiar == null)
                        break;

                    var planarFacesUPERIORCOpiar = ((Element)elementoSeleccionadoParaCopiar).ObtenerCaraSuperior(seleccionarLosaParaCopiarMouse._ptoSeleccionEnLosa, new XYZ(0, 0, 1));
                    if (planarFacesUPERIORCOpiar == null) return false;

                    // obtenerc cara inferiror y centro
                    XYZ centroCaraCOpiar = planarFacesUPERIORCOpiar.ObtenerCenterDeCara();


                    BuscarFundacionLosa BuscarMuros = new BuscarFundacionLosa(_uiapp, Util.CmToFoot(200));
                    XYZ _puntoSobreCaraInfFundacion = XYZ.Zero;
                    if (BuscarMuros.OBtenerRefrenciaFundacionSegunVector(_view3D_buscar, centroCaraCOpiar, new XYZ(0, 0, -1)))
                    {
                        _puntoSobreCaraInfFundacion = BuscarMuros._PtoSObreCaraInferiorFund;
                    }

                    XYZ deltaDesplazamineto = centroCaraCOpiar - centroCaraInf;



                    ListaDesplazaminetoColumna.Add(new ResulSelecionElementpCopiaDto()
                    {
                        PtoDesfase = deltaDesplazamineto,
                        ptoSobreCaraInferiorFundacion = _puntoSobreCaraInfFundacion,
                        ptoSuperiorColumna = centroCaraCOpiar,
                        ElementoACopiar = (Element)elementoSeleccionadoParaCopiar
                    }
                    );
                }

                if (ListaDesplazaminetoColumna.Count == 0) return false;
                //copiar con delta desplazamineto
                _listaIdBarras = _ElementoConBarras.ListaRebar.Select(c => c.Rebar.Id).ToList();


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Util.ErrorMsg($"Error en 'ManejadorCopiaFundaciones'.\nex:{ex.Message} ");
                return false;
            }
            return true;
        }

        private bool CopiarElementoColumna(List<ElementId> listaELemento)
        {


            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start($"copiarBarrasSinColor-NH");
                    for (int i = 0; i < ListaDesplazaminetoColumna.Count; i++)
                    {
                        var result = ListaDesplazaminetoColumna[i];

                        var ListaCopiada = ElementTransformUtils.CopyElements(_doc, listaELemento, result.PtoDesfase).ToList();

                        for (int j = 0; j < ListaCopiada.Count; j++)
                        {
                            var _rebarCopiada = (Rebar)_doc.GetElement(ListaCopiada[j]);
                            _rebarCopiada.SetHostId(_doc, result.ElementoACopiar.Id);
                        }

                        ListaElementoCopiados_id = new List<ElementId>();
                        ListaElementoCopiados_id.AddRange(ListaCopiada);

                        CopiarParametrosBarrasCopiadas.M4_CopiarParametros(_doc, _view, ListaElementoCopiados_id);

                        result.ListaElementoCopiados_id = ListaElementoCopiados_id;
                    }
                    t.Commit();
                }
  
          
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Util.ErrorMsg("No se puede mover barra");
            }
        
            return true;
        }
        private bool MoverElementoColumna()
        {
            try
            {
 
                //mover
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start($"copiarBarrasSinColor-NH");
                    for (int i = 0; i < ListaDesplazaminetoColumna.Count; i++)
                    {
                        var result = ListaDesplazaminetoColumna[i];

                        for (int j = 0; j < result.ListaElementoCopiados_id.Count; j++)
                        {
                            XYZ desfase = result.ptoSobreCaraInferiorFundacion;

                            var _barra = (Rebar)_doc.GetElement(result.ListaElementoCopiados_id[j]);

                            var driveLine = _barra.GetShapeDrivenAccessor();
                            if (Util.IsVertical(driveLine.Normal)) //estribo
                            {
                                if (!AyudaCurveRebar.GetPrimeraRebarCurves(_barra))
                                {
                                    Util.ErrorMsg("Error al obtener largos parciales de barra");
                                    continue;
                                }
                                List<Curve> listapto1 = AyudaCurveRebar.ListacurvesSoloLineas[0];
                                double coorZ = listapto1[0].GetEndPoint(0).Z;
                                double moverBarras = desfase.Z - coorZ + ConstNH.RECUBRIMIENTO_FUNDACIONES_foot + Util.CmToFoot(5);
                                ElementTransformUtils.MoveElement(_doc, _barra.Id, XYZ.BasisZ * moverBarras);


                                double largoEstribo = (result.ptoSuperiorColumna.Z - result.ptoSobreCaraInferiorFundacion.Z) - (ConstNH.RECUBRIMIENTO_FUNDACIONES_foot + Util.CmToFoot(5));

                                int cantidadMover = (int)Math.Ceiling(largoEstribo / Util.CmToFoot(_barra.ObtenerEspaciento_cm()));
                                ParameterUtil.SetParaIntNH(_barra, "Quantity", cantidadMover);
                            }
                            else// barras
                            {

                                RebarParametros _RebarParametros = new RebarParametros(_doc, _barra);

                                if (_RebarParametros.Ejecutar())
                                {
                                    var latraMasGrande = _RebarParametros.ListaLsesdfedtras.OrderByDescending(c => c.Valor).FirstOrDefault();
                                    if (latraMasGrande != null)
                                    {
                                        if (!AyudaCurveRebar.GetPrimeraRebarCurves(_barra))
                                        {
                                            Util.ErrorMsg("Error al obtener largos parciales de barra");
                                            continue;
                                        }

                                        List<Curve> listapto1 = AyudaCurveRebar.ListacurvesSoloLineas[0];
                                        var lineBArra = listapto1.OrderByDescending(c => c.ApproximateLength).FirstOrDefault();
                                        double coorZ = Math.Min(lineBArra.GetEndPoint(0).Z, lineBArra.GetEndPoint(1).Z);
                                        double moverBarras = desfase.Z - coorZ + ConstNH.RECUBRIMIENTO_FUNDACIONES_foot;
                                        ElementTransformUtils.MoveElement(_doc, _barra.Id, XYZ.BasisZ * moverBarras);

                                        ParameterUtil.SetParaDoubleNH(_barra, latraMasGrande.Letra, latraMasGrande.Valor - moverBarras);
                                    }
                                }
                            }
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Util.ErrorMsg("No se puede mover barra");
            }

            return true;
        }
    }
}
