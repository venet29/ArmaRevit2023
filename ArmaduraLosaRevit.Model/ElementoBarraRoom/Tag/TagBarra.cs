using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using Autodesk.Revit.DB;
using System;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Extension;
using System.Linq;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.ExtStore.model;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag
{
    public class TagBarra
    {
        Document _doc;
        private View _view;
        private int? _escala;
        UIApplication _uiapp;
        private XYZ factorPorEscala;
        private CreadorExtStore _CreadorExtStore;

        public string valorTag { get; set; } = "";// se se uso temporamente para remplazar tag

        public XYZ posicion { get; set; }
        // public XYZ _TagHeadPosition { get; set; }
        public XYZ PtoCodo_LeaderElbow { get; set; }
        public XYZ Ptocodo_LeaderEnd { get; set; }
        public double anguloGrado { get; set; }
        public string nombre { get; set; }
        public string nombreFamilia { get; set; }
        public Element ElementIndependentTagPath { get; set; }
        public IndependentTag independentTag { get; set; }
        public bool IsOk { get; set; } = true;
        public bool IsDIrectriz { get; set; } = true;
        public bool IsLibre { get; set; }
        public TagOrientation HorientacionTag { get; set; }

        private DatosExtStoreDTO _CreadorExtStoreDTO;

        public TagBarra(XYZ posicion, string nombre, string nombreFamilia, Element ElementIndependentTagPath)
        {

            this.posicion = posicion;
            this.nombre = nombre;
            this.nombreFamilia = nombreFamilia;
            this.ElementIndependentTagPath = ElementIndependentTagPath;
            this.IsLibre = false;
            this.IsDIrectriz = false;
            if (ElementIndependentTagPath != null)
            {
                IsOk = true;

                _doc = ElementIndependentTagPath?.Document;
                _view = ElementIndependentTagPath.Document?.ActiveView;

            }
            else
                IsOk = false;

            _escala = _view.Scale;//  ObtenerNombre_EscalaConfiguracion();
            HorientacionTag = TagOrientation.Horizontal;

            _CreadorExtStoreDTO = FactoryExtStore.ObtnerPosicionTagLosa();

        }


        public void CAmbiar(TagBarra tagP0_)
        {
            if (tagP0_ == null)
            {
                IsOk = false;
                return;
            }
            nombre = tagP0_.nombre;
            nombreFamilia = tagP0_.nombreFamilia;
            if (tagP0_.ElementIndependentTagPath != null)
            {
                ElementIndependentTagPath = tagP0_.ElementIndependentTagPath;
                IsOk = true;
            }
            else
            { IsOk = false; }
        }

        public TagBarra Copiar()
        {
            return new TagBarra(this.posicion, this.nombre, this.nombreFamilia, this.ElementIndependentTagPath);

        }


        public bool DibujarTagPathReinforment(Element element, UIApplication _uiapp, View viewActual, XYZ desplazamientoPathReinSpanSymbol)
        {
            try
            {
                this._uiapp = _uiapp;
                Document _doc = _uiapp.ActiveUIDocument.Document;
                if (!IsOk) return false;
                independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), false,
                                                          TagOrientation.Horizontal, desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
                independentTag.TagHeadPosition = posicion;

                if (independentTag != null)
                {
                    _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);
                    _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(independentTag, independentTag.TagHeadPosition);
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;

            }
            return true;
        }

        public void DibujarTagrREbarLosa(Element element, UIApplication _uiapp, View viewActual, XYZ desplazamientoPathReinSpanSymbol)
        {
            this._uiapp = _uiapp;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            if (valorTag != "")
            {
                AyudaCreartexto.M4_CrearTExtoSinTrans(_doc, posicion, valorTag, "2.5mm Arial", 0, TipoCOloresTexto.Blanco);
                return;
            }
            if (!IsOk) return;
            independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), IsDIrectriz,
                                                              HorientacionTag, desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicion;

            if (IsLibre)
            {
                independentTag.LeaderEndCondition = LeaderEndCondition.Free;
                independentTag.Set_LeaderEnd(_uiapp, Ptocodo_LeaderEnd);
            }

            if (IsDIrectriz == true)
            {
                if (independentTag == null) return;
                if (PtoCodo_LeaderElbow == null) return;
                independentTag.Set_LeaderElbow(_uiapp, PtoCodo_LeaderElbow);
                FamilySymbol tagSymbol = _doc.GetElement(independentTag.GetTypeId()) as FamilySymbol;

                Element _Arrow = Tipos_Arrow.ObtenerPrimerArrowheads(_doc, "Filled Dot 2mm_" + _view.Scale);
                if (_Arrow != null)
                { //asigna el tipo de flecha de recorrido al symbolo del pathrebar
                    ParameterUtil.SetParaElementId(tagSymbol, BuiltInParameter.LEADER_ARROWHEAD, _Arrow.Id);
                }
            }


            if (independentTag == null)
            {
                _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);
                _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(independentTag, independentTag.TagHeadPosition);
            }
        }


        public void DibujarTagRebarFund(Element element, UIApplication _uiapp, View viewActual, ConfiguracionTAgBarraDTo confBarraTab)
        {
            this._uiapp = _uiapp;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            if (!IsOk) return;
            independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), IsDIrectriz,
                                                     confBarraTab.tagOrientation, confBarraTab.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)

            if (IsLibre)
            {
                independentTag.LeaderEndCondition = LeaderEndCondition.Free;
                independentTag.Set_LeaderEnd(_uiapp, Ptocodo_LeaderEnd);
            }

            independentTag.TagHeadPosition = posicion;

            if (IsDIrectriz == true)
            {
                if (independentTag == null) return;
                if (PtoCodo_LeaderElbow == null) return;
                independentTag.Set_LeaderElbow(_uiapp,PtoCodo_LeaderElbow);
                FamilySymbol tagSymbol = _doc.GetElement(independentTag.GetTypeId()) as FamilySymbol;

                Element _Arrow = Tipos_Arrow.ObtenerPrimerArrowheads(_doc, "Filled Dot 2mm_" + _view.Scale);
                if (_Arrow != null)
                { //asigna el tipo de flecha de recorrido al symbolo del pathrebar
                    ParameterUtil.SetParaElementId(tagSymbol, BuiltInParameter.LEADER_ARROWHEAD, _Arrow.Id);
                }
            }

            if (independentTag == null)
            {
                _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);
                _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(independentTag, independentTag.TagHeadPosition);
            }
        }


        public void DibujarTagRebarV(Element element, UIApplication _uiapp, View viewActual, ConfiguracionTAgBarraDTo confBarraTab)
        {
            this._uiapp = _uiapp;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            //   if (!IsOk) return;
            independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), confBarraTab.IsDIrectriz,
                                                      confBarraTab.tagOrientation, confBarraTab.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            int escala = viewActual.Scale;
            factorPorEscala = XYZ.Zero;

            DesplazamientoPorEscalaYPosicionSeleccion(confBarraTab, escala);
            independentTag.TagHeadPosition = posicion + factorPorEscala;
            AgregarDirectriz(confBarraTab, independentTag, _uiapp);

        }
        public void DibujarTagRebar_ConLibre(Rebar rebar, UIApplication _uiapp, View view, ConfiguracionTAgBarraDTo confBarraTag)
        {
            this._uiapp = _uiapp;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, view.Id, new Reference(rebar), IsDIrectriz,
                                                confBarraTag.tagOrientation, confBarraTag.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)

            if (IsLibre)
            {
                independentTag.LeaderEndCondition = LeaderEndCondition.Free;
                independentTag.Set_LeaderEnd(_uiapp, Ptocodo_LeaderEnd);
            }

            independentTag.TagHeadPosition = posicion;

            AgregarDirectriz(IsDIrectriz, PtoCodo_LeaderElbow, independentTag, _uiapp);

        }

        public void DibujarTagRebar_HorquillaHorizontal(Rebar rebar, UIApplication _uiapp, View view, ConfiguracionTAgBarraDTo confBarraTag)
        {
            this._uiapp = _uiapp;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, view.Id, new Reference(rebar), IsDIrectriz,
                                                 confBarraTag.tagOrientation, confBarraTag.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)

            if (IsLibre)
            {
                independentTag.LeaderEndCondition = LeaderEndCondition.Free;
                independentTag.Set_LeaderEnd(_uiapp, Ptocodo_LeaderEnd);
            }

            double diferenciaEntreCoord = confBarraTag.RecalcularPtosYEspaciamieto_Horqu.PtoInicial_Corregido.Z - posicion.Z;
            double deltaEntreCOtas = (PtoCodo_LeaderElbow.Z - posicion.Z);

            independentTag.TagHeadPosition = posicion.AsignarZ(confBarraTag.RecalcularPtosYEspaciamieto_Horqu.PtoInicial_Original.Z - diferenciaEntreCoord);

            XYZ ptoAux = PtoCodo_LeaderElbow.AsignarZ(confBarraTag.RecalcularPtosYEspaciamieto_Horqu.PtoInicial_Original.Z - diferenciaEntreCoord + deltaEntreCOtas);
            AgregarDirectriz(IsDIrectriz, ptoAux, independentTag, _uiapp);

        }

        private void DesplazamientoPorEscalaYPosicionSeleccion(ConfiguracionTAgBarraDTo confBarraTab, int escala)
        {
            if (confBarraTab.BarraTipo == TipoRebar.ELEV_BA_V)
            {
                if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.75);

                if (escala == 100) factorPorEscala = -new XYZ(0, 0, 1.5);
            }
            else if (confBarraTab.BarraTipo == TipoRebar.ELEV_BA_CABEZA_HORQ)
            {
                //factorPorEscala = (posicion - LeaderElbow) - new XYZ(0, 0, -1.38);
            }
            else if (confBarraTab.BarraTipo == TipoRebar.ELEV_BA_H)
            {
                if (confBarraTab.TipoCaraObjeto_ == Enumeraciones.TipoCaraObjeto.Inferior)
                {
                    if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.2);

                    if (escala == 100) factorPorEscala = -new XYZ(0, 0, 0.4);
                }
                else
                {
                    if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.1);

                    if (escala == 100) factorPorEscala = -new XYZ(0, 0, 0.1);
                }

            }
            else
            {
                if (escala == 75) factorPorEscala = -new XYZ(0, 0, 0.75);

                if (escala == 100) factorPorEscala = -new XYZ(0, 0, 1.5);
            }
        }

        public void DibujarTagRebarRefuerzoLosa(Element element, UIApplication _uiapp, View viewActual, ConfiguracionTAgBarraDTo confBarraTab)
        {
            this._uiapp = _uiapp;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            if (!IsOk) return;
            independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), confBarraTab.IsDIrectriz,
                                                      confBarraTab.tagOrientation, confBarraTab.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicion;

            AgregarDirectriz(confBarraTab, independentTag, _uiapp);

        }

        public void DibujarTagEstribo(Element element, UIApplication _uiapp, View viewActual, ConfiguracionTAgEstriboDTo configuracionTAgEstriboDTo, XYZ posicionTag)
        {
            this._uiapp = _uiapp;
            Document _doc = _uiapp.ActiveUIDocument.Document;
            if (!IsOk) return;
            independentTag = IndependentTag.Create(_doc, ElementIndependentTagPath.Id, viewActual.Id, new Reference(element), configuracionTAgEstriboDTo.IsDIrectriz,
                                                      configuracionTAgEstriboDTo.tagOrientation, configuracionTAgEstriboDTo.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
            independentTag.TagHeadPosition = posicionTag;
        }

        private void AgregarDirectriz(ConfiguracionTAgBarraDTo confBarraTab, IndependentTag independentTag, UIApplication _uiapp)
        {
            if (!confBarraTab.IsDIrectriz) return;
            AgregarDirectriz(confBarraTab.IsDIrectriz, posicion + confBarraTab.LeaderElbow, independentTag, _uiapp);
        }
        private void AgregarDirectriz(bool IsDIrectriz, XYZ ptoelbow, IndependentTag independentTag, UIApplication _uiapp)
        {

            if (!IsDIrectriz) return;

            if (independentTag == null) return;
            //independentTag.SetLeaderElbow(independentTag.GetTaggedReferences().FirstOrDefault(), ptoelbow);
            independentTag.Set_LeaderElbow(_uiapp, ptoelbow);

            FamilySymbol tagSymbol = _doc.GetElement(independentTag.GetTypeId()) as FamilySymbol;

            var elem = Tipos_Arrow.ObtenerArrowheadPorNombre(_doc, "Filled Dot 2mm_" + _escala);
            if (elem != null)
                tagSymbol.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(elem.Id);

        }

        internal void ReAsignarFreeEnd()
        {
            if (independentTag != null)
            {
                independentTag.Set_LeaderEnd(_uiapp, Ptocodo_LeaderEnd);
            }
        }
    }

}
