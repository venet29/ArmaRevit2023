using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ocultar;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.PathReinf.DTO;
using ArmaduraLosaRevit.Model.PathReinf.Servicios;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.PathReinf.Barras
{
    public class PathReinf_nh
    {
        private Document _doc;
        private View3D view3D;
        private View3D view3D_Visualizar;
#pragma warning disable CS0414 // The field 'PathReinf_nh.IsTest' is assigned but its value is never used
        private bool IsTest;
#pragma warning restore CS0414 // The field 'PathReinf_nh.IsTest' is assigned but its value is never used
        public static int contador { get; set; }
        public Result statusbarra { get; set; }
        private UIApplication _uiapp;
        private SolicitudBarraDTO _solicitudBarraDTO;
        private bool _isDirectriz;
        private PathReinfSeleccionDTO _pathReinfSeleccionDTO;
        private DatosNuevaBarraDTO _datosNuevaBarraDTO;
        private ICasoBarraX _iTipoBarra;
        private IGeometriaTag _listaTAgBArra;
        private PathReinfVerificacion _pathReinfVErificacion;
        public PathReinforcement m_createdPathReinforcement = null;
        public View _view;

        private RebarBarType rebarBarType;
        private PathReinSpanSymbol symbolPath;
        private Element _elemtoSymboloPath;
        private ElementId _InvalidrebarHookTypeId;
        private TipoRebar _BarraTipo;
        private string numeroBarrasPrimaria;
        private bool IsRedefinirTagHeadPosition; // para elevacon es true, para fundacion es false

        public PathReinf_nh(UIApplication uiapp, SolicitudBarraDTO solicitudBarraDTO, PathReinfSeleccionDTO pathReinfSeleccionDTO,
                                            DatosNuevaBarraDTO datosNuevaBarraDTO, ICasoBarraX iTipoBarra, IGeometriaTag _listaTAgBArra)
        {
            this._uiapp = uiapp;
            this._solicitudBarraDTO = solicitudBarraDTO;
            this._isDirectriz = solicitudBarraDTO.IsDirectriz;
            this._doc = uiapp.ActiveUIDocument.Document;
            //this._pathReinfDTO = pathReinfDTO;
            this._pathReinfSeleccionDTO = pathReinfSeleccionDTO;
            this._datosNuevaBarraDTO = datosNuevaBarraDTO;
            this._iTipoBarra = iTipoBarra;
            this._listaTAgBArra = _listaTAgBArra;
            this._pathReinfVErificacion = new PathReinfVerificacion();
            this._view = _doc.ActiveView;
            contador += 1;
            this._elemtoSymboloPath = datosNuevaBarraDTO.ElementSimboloPathReinforcementElement;
            this._InvalidrebarHookTypeId = ElementId.InvalidElementId;
            this._BarraTipo = datosNuevaBarraDTO._BarraTipo;
            IsRedefinirTagHeadPosition = datosNuevaBarraDTO.IsRedefinirTagHeadPosition;
            M0_CargarDatosIniciales();

        }

        public PathReinf_nh(UIApplication uiapp)
        {
            _uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }


        //crea  refuerzo path
        //flip true se dibuja hacia abajo de la curva de trayectoria
        //flip false se dibuja hacia arriba de la curva de trayectoria
        //bool aux_flop = false;
        public Result CrearBarra(bool aux_flop = false)
        {
            if (XYZ.Zero.DistanceTo(_datosNuevaBarraDTO.DesplazamientoPathReinSpanSymbol) > 0.1)
            {
                _pathReinfSeleccionDTO.ptoConMouse = _datosNuevaBarraDTO.DesplazamientoPathReinSpanSymbol;
                _datosNuevaBarraDTO.DesplazamientoPathReinSpanSymbol = XYZ.Zero;
            }

            if (!_pathReinfVErificacion.VerificarDatos(_datosNuevaBarraDTO)) return Result.Failed;
            try
            {
                using (TransactionGroup transGroup = new TransactionGroup(_doc))
                {
                    transGroup.Start("Transaction Group-NH");

                    using (Transaction trans = new Transaction(_doc))
                    {
                        trans.Start("CreatePathReinforcement-NH");
                        //crea  refuerzo path
                        //flip true se dibuja hacia abajo de la curva de trayectoria
                        //flip false se dibuja hacia arriba de la curva de trayectoria
                        //bool aux_flop = false;

                        //**************************************************************************************************************************************

                        if (!M0_BuscarTipoHookYDiamtro())
                        {
                            trans.RollBack();
                            return statusbarra = Result.Failed; ;
                        }

                        //1)crea el pathreinformet 
                        _datosNuevaBarraDTO.CurvesPathreiforment = AcortarCUrva(_datosNuevaBarraDTO.CurvesPathreiforment.ToList(), _datosNuevaBarraDTO);

                        m_createdPathReinforcement = M1_CreatePathReinforcement(aux_flop, _pathReinfSeleccionDTO.ElementoSeleccionada1, _datosNuevaBarraDTO.CurvesPathreiforment, (int)_datosNuevaBarraDTO.DiametroMM);

                        if (m_createdPathReinforcement == null)
                        {
                            trans.RollBack();
                            return statusbarra = Result.Failed; ;
                        }
                 
                        //2)
                        //a) seleciona que barra se botton - inferior
                        //b) activa barra alternativa de ser necesario
                        //c) asigna largo de barra princiapales y alterniva si corresonde
                        //b) asigna largos              
                      
                        if (!_iTipoBarra.LayoutRebar_PathReinforcement(m_createdPathReinforcement, _datosNuevaBarraDTO))
                        {
                            trans.RollBack();
                            return statusbarra = Result.Failed; ;
                        }

                        if (!_iTipoBarra.CambiarCaraSuperior(m_createdPathReinforcement))
                        {
                            trans.RollBack();
                            return statusbarra = Result.Failed; ;
                        }

                        //3)crear path reiforme
                        if (!M4_CrearPAthSymbol())
                        {
                            trans.RollBack();
                            return statusbarra = Result.Failed; ;
                        }
                        // doc.Regenerate();
                        trans.Commit();
                        //   uidoc.RefreshActiveView();
                    } // fin trans

                    if (!m_createdPathReinforcement.IsValidObject) return Result.Cancelled;

                    using (Transaction trans = new Transaction(_doc))
                    {
                        trans.Start("CreatePathReinforcement_tag-NH");

                        ConfiguracionTAgBarraDTo confBarraTag = new ConfiguracionTAgBarraDTo()
                        {
                            desplazamientoPathReinSpanSymbol = new XYZ(0, 0, 0),
                            IsDIrectriz = _isDirectriz,
                            LeaderElbow = new XYZ(5, -5, 0),
                            tagOrientation = TagOrientation.Horizontal
                        };



                        if (!M2_copiarDatosInternos())
                        {
                            trans.RollBack();
                            return statusbarra = Result.Failed; ;
                        }

                        //4) crea el tag con la cuentia de las barra nnv
                        M5_CrearTag(confBarraTag);

                        //5
                        OcultarBarras _OcultarBarras = new OcultarBarras(_doc);
                        _OcultarBarras.Ocultar1BarraCreada_AgregarParametroARebarSystem(m_createdPathReinforcement, Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo), IsConTransaccion: false);
                        //7
                        M7_MOverPathReinSpanSymbol();

                        CreadorCirculo.BorrasCirculosCreado_Sintrans(_doc);
                        trans.Commit();
                    }
                    //    _uiapp.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                    // M6_OcultarBarraCreada();



                    transGroup.Assimilate();
                    //    //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                    //    return Result.Succeeded;
                }// fin trasn group 

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                string message = ex.Message;
                statusbarra = Result.Failed;
                Util.ErrorMsg("Error al crear Path Symbol :" + ex.Message);
                return Result.Failed;
            }



        }


        private bool M0_BuscarTipoHookYDiamtro()
        {
            try
            {
                //1.b)asigna RebarHookType defaul
                // ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_document);
                ElementId rebarHookTypeId = ElementId.InvalidElementId;
                rebarHookTypeId = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc).Id;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M0_BuscarTipoHookYDiamtro' ex:{ex.Message}");
                return false;
            }
            return true;
        }
        private IList<Curve> AcortarCUrva(List<Curve> curvesPathreiforment_, DatosNuevaBarraDTO datosNuevaBarraDTO)
        {


            XYZ p1 = curvesPathreiforment_[0].GetPoint2(0);
            XYZ p2 = curvesPathreiforment_[0].GetPoint2(1);
            XYZ Vector = (p2 - p1).Normalize();


            double cantidadBArras = Math.Round((datosNuevaBarraDTO.LargoRecorridoFoot) / datosNuevaBarraDTO.EspaciamientoFoot, 5);

            if (cantidadBArras < 1)
                cantidadBArras = 1;

            double parteDecimal = Util.ParteDecimal(cantidadBArras);

            long CantidadBarra = (long)cantidadBArras;



            if (parteDecimal < ConstNH.CONST_FACTOR_DISMINUIR_1BARRA) CantidadBarra -= 1;

            if (CantidadBarra < 1)
                CantidadBarra = 1;

            if (DatosDiseño.IS_PATHREIN_AJUSTADO == false) return curvesPathreiforment_;

            double largoREcorridoborrar = datosNuevaBarraDTO.LargoRecorridoFoot - (CantidadBarra * datosNuevaBarraDTO.EspaciamientoFoot + Util.MmToFoot(datosNuevaBarraDTO.DiametroMM));

            p1 = p1 + Vector * largoREcorridoborrar / 2;
            p2 = p2 - Vector * largoREcorridoborrar / 2;

            Line nuevaline1 = Line.CreateBound(p1, p2);

            if (!datosNuevaBarraDTO.IsAcortarCUrva) return curvesPathreiforment_;

            return new List<Curve>() { nuevaline1 };
        }




        private bool M0_CargarDatosIniciales()
        {


            view3D = TiposFamilia3D.Get3DBuscar(_doc);
            if (view3D == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }
            view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);

            _view = _doc.ActiveView;

            IsTest = false;

            return true;

        }

        //se utilza para fundaciones
        public PathReinforcement M1_CreatePathReinforcement(bool flip, Element _element, IList<Curve> curvesPathreiforment_, int diametro_)
        {
            PathReinforcement result = null;
            try
            {
                //Line curve;
                //IList<Curve> curves = new List<Curve>();
                ////configuracion
                //// 0  - 3
                //// 1 -  2
                //curve = Line.CreateBound(points[0], points[3]);
                //curves.Add(curve);

                var start = new TimeSpan(DateTime.Now.Ticks);
                ElementId pathReinforcementTypeId = PathReinforcementType.CreateDefaultPathReinforcementType(_doc);

                //1) asigna el tipo de la barra en funcion del diametro, que debe estar creados en la libreria
                rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametro_, _doc, true);

                if (rebarBarType == null) return null;

                result = PathReinforcement.Create(_doc, _element, curvesPathreiforment_, flip, pathReinforcementTypeId,
                    rebarBarType.Id, _InvalidrebarHookTypeId, _InvalidrebarHookTypeId);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear 'M1_CreatePathReinforcement'  ex:{ex.Message}");
                return null;
            }
            return result;

        }

        private bool M2_copiarDatosInternos()
        {

            try
            {


                Element paraElem = _doc.GetElement(m_createdPathReinforcement.Id);

                DimensionesBarras dimBarras_parameterShareLetras = _datosNuevaBarraDTO.dimBarras_parameterSharedLetras;
                if (dimBarras_parameterShareLetras != null) ParameterUtil.SetParaInt(paraElem, "A_", dimBarras_parameterShareLetras.a.valor);
                if (dimBarras_parameterShareLetras != null) ParameterUtil.SetParaInt(paraElem, "B_", dimBarras_parameterShareLetras.b.valor);
                if (dimBarras_parameterShareLetras != null) ParameterUtil.SetParaInt(paraElem, "C_", dimBarras_parameterShareLetras.c.valor);
                if (dimBarras_parameterShareLetras != null) ParameterUtil.SetParaInt(paraElem, "D_", dimBarras_parameterShareLetras.d.valor);
                if (dimBarras_parameterShareLetras != null) ParameterUtil.SetParaInt(paraElem, "E_", dimBarras_parameterShareLetras.e.valor);
                if (dimBarras_parameterShareLetras != null) ParameterUtil.SetParaInt(paraElem, "G_", dimBarras_parameterShareLetras.g.valor);
                if (dimBarras_parameterShareLetras != null) ParameterUtil.SetParaInt(paraElem, "EspesorCambio", dimBarras_parameterShareLetras.LetrasCambiosEspesor);

                if (_datosNuevaBarraDTO.Prefijo_cuantia != "")
                {
                    ParameterUtil.SetParaStringNH(paraElem, "Prefijo_F", _datosNuevaBarraDTO.Prefijo_cuantia);
                }

                ParameterUtil.SetParaStringNH(paraElem, "NombreVista", _view.ObtenerNombreIsDependencia());


                numeroBarrasPrimaria = m_createdPathReinforcement.ObtenerNumeroBarras().ToString();

                ParameterUtil.SetParaStringNH(paraElem, "NumeroPrimario", numeroBarrasPrimaria.ToString());

                // if (ParameterUtil.FindParaByName(paraElem, "BarraTipo") != null && _BarraTipo != TipoRebar.NONE)
                //     ParameterUtil.SetParaInt(paraElem, "BarraTipo", Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo));  //"nombre de vista"

                if (ParameterUtil.FindParaByName(paraElem, "IDNumero") != null) ParameterUtil.SetParaInt(paraElem, "IDNumero", contador);
                if (ParameterUtil.FindParaByName(paraElem, "IDTipo") != null) ParameterUtil.SetParaInt(paraElem, "IDTipo", _solicitudBarraDTO.TipoBarra);
                if (ParameterUtil.FindParaByName(paraElem, "IDTipoDireccion") != null) ParameterUtil.SetParaInt(paraElem, "IDTipoDireccion", _solicitudBarraDTO.UbicacionEnlosa.ToString());
                if (ParameterUtil.FindParaByName(paraElem, "EsPrincipal") != null)
                    ParameterUtil.SetParaInt(paraElem, "EsPrincipal", (_datosNuevaBarraDTO.IsLuzSecuandiria == false ? 1 : 0));


                if (ParameterUtil.FindParaByName(paraElem, "LargoTotal") != null)
                    ParameterUtil.SetParaInt(paraElem, "LargoTotal", _datosNuevaBarraDTO.LargoTotal);
                else if (ParameterUtil.FindParaByName(paraElem, "LargoTotal2") != null)
                    ParameterUtil.SetParaInt(paraElem, "LargoTotal2", _datosNuevaBarraDTO.LargoTotal);

                if (ParameterUtil.FindParaByName(paraElem, "LargoParciales") != null)
                    ParameterUtil.SetParaInt(paraElem, "LargoParciales", _datosNuevaBarraDTO.LargoParciales);
                else if (ParameterUtil.FindParaByName(paraElem, "LargoParciales2") != null)
                    ParameterUtil.SetParaInt(paraElem, "LargoParciales2", _datosNuevaBarraDTO.LargoParciales);


                if (ParameterUtil.FindParaByName(paraElem, "BarraTipo") != null && _BarraTipo != TipoRebar.NONE)
                {
                    string BarraTTipo = Tipos_Barras.M1_Buscar_nombreTipoBarras_porTipoRebar(_BarraTipo);
                    ParameterUtil.SetParaInt(paraElem, "BarraTipo", BarraTTipo);  //"nombre de vista"
                    AgergarParametroRebarSystem(paraElem as PathReinforcement, BarraTTipo);
                }


                //CONFIGURACION DE PATHREINFORMENT
                if (view3D_Visualizar != null)
                {
                    //permite que la barra se vea en el 3d como solido
                    m_createdPathReinforcement.SetSolidInView(view3D_Visualizar, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    m_createdPathReinforcement.SetUnobscuredInView(view3D_Visualizar, true);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M2_copiarDatosInternos' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void AgergarParametroRebarSystem(PathReinforcement paraElem, string BarraTTipo)
        {
            if (paraElem == null) return;
            List<ElementId> ListElemId = paraElem.GetRebarInSystemIds().ToList();
            foreach (var item in ListElemId)
            {
                Element elm = _doc.GetElement2(item);
                if (ParameterUtil.FindParaByName(elm, "BarraTipo") != null)
                    ParameterUtil.SetParaInt(elm, "BarraTipo", BarraTTipo);  //"nombre de vista"
            }

        }

        private bool M4_CrearPAthSymbol()
        {
            try
            {
                //2)Family fam = TiposFamilyRebar.getFamilyRebarShape("Structural Path Reinforcement 3", doc);
                //obtiene elemento dentos de la biblioteca de familia del PathReinforcementSymbol
                //string scala = ConstantesGenerales.CONST_ESCALA_BASE.ToString();// view.Scale.ToString();
                //if (IsTest) scala = "100";
                //Element elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(_datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + scala,  _doc);

                // ParameterUtil.SetParaInt(elemtoSymboloPath, "A_", Util.CmToFoot(100 / 100));
                if (_elemtoSymboloPath == null)
                {
                    Util.ErrorMsg($"Pathsymbol no encontrado para tipo: {_solicitudBarraDTO.TipoBarra}");
                    // trans.RollBack();
                    return false;
                }
                // busca el elemnto 'Arrow Filled 20 Degree' que es la flecha del recorrido una un metodo muy generico tratar de mejorar
                List<Element> listaArrow = Tipos_Arrow.FindAllArrowheads(_doc, "Arrow Filled 20 Degree");
                //asigna el tipo de flecha de recorrido al symbolo del pathrebar
                ParameterUtil.SetParaElementId(_elemtoSymboloPath, BuiltInParameter.LEADER_ARROWHEAD, listaArrow[0].Id);


                // 2.1)crea el symbolo con la forma de la barra
                //double x = 0;// Util.CmToFoot(10);
                //double y = 0; //Util.CmToFoot(10);
                symbolPath = PathReinSpanSymbol.Create(_doc, _view.Id, new LinkElementId(m_createdPathReinforcement.Id), _datosNuevaBarraDTO.DesplazamientoPathReinSpanSymbol, _elemtoSymboloPath.Id);

                if (_pathReinfSeleccionDTO.ptoConMouse != null && symbolPath != null && IsRedefinirTagHeadPosition)
                    symbolPath.TagHeadPosition = _pathReinfSeleccionDTO.ptoConMouse;// - _datosNuevaBarraDTO.CentroPAth;

                ParameterUtil.SetParaInt(symbolPath, "A_r2", Util.CmToFoot(100 / 100));
                // ParameterUtil.SetParaInt(symbolPath, "B_sy", Util.CmToFoot(100 / 100));
                if (symbolPath == null)
                {

                    //trans.RollBack();
                    return false;
                }




            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M4_CrearPAthSymbol' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public Result M7_MOverPathReinSpanSymbol()
        {
            if (_pathReinfSeleccionDTO.ptoConMouse == null) return Result.Succeeded;
            if (IsRedefinirTagHeadPosition == false) return Result.Succeeded;
            try
            {
                if (symbolPath.TagHeadPosition.AsignarZ(0).DistanceTo(_pathReinfSeleccionDTO.ptoConMouse.AsignarZ(0)) > 0.1)
                {
                    if (_pathReinfSeleccionDTO?.ptoConMouse != null)
                    {
                        //pathsymbol
                        symbolPath.TagHeadPosition = _pathReinfSeleccionDTO.ptoConMouse;
                        // tag 
                        //if (_listaTAgBArra.M4_IsFAmiliaValida())
                        //{
                        //    foreach (TagBarra _TagBarra in _listaTAgBArra.listaTag)
                        //    {
                        //        if (_TagBarra.IsOk)
                        //            _TagBarra.ReAsignarFreeEnd();
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        private void M5_CrearTag(ConfiguracionTAgBarraDTo confBarraTag)
        {
            if (_listaTAgBArra.M4_IsFAmiliaValida())
            {
                foreach (TagBarra _TagBarra in _listaTAgBArra.listaTag)
                {


                    if (_TagBarra.IsOk)
                        _TagBarra.DibujarTagRebarFund(m_createdPathReinforcement, _uiapp, _view, confBarraTag); //  M5_1_DibujarTagPathReinforment(trans, item, desplazamientoPathReinSpanSymbol);
                }
            }
        }



        //COMANDO IMPLEMENTADO EN 
        private void M6_OcultarBarraCreada()
        {
            if (!m_createdPathReinforcement.IsValidObject) return;
            var Ilist_RebarInSystem = m_createdPathReinforcement.GetRebarInSystemIds();

            if (Ilist_RebarInSystem == null) return;

            List<View> listaView = Util.GetListtViewCOntengaNombre(_doc, _view.Name.Replace(ConstNH.NOMBRE_PLANOLOSA_INF, "")
                                                                                 .Replace(ConstNH.NOMBRE_PLANOLOSA_SUP, "")
                                                                                 .Replace("(", "")
                                                                                 .Replace(")", "").Trim());
            if (listaView.Count == 0) listaView.Add(_view);

            try
            {
                //segunda trasn
                using (Transaction trans2 = new Transaction(_doc))
                {
                    trans2.Start("Crear CreatePathReinforcement2-NH");
                    List<ElementId> ListElemId = Ilist_RebarInSystem.ToList();
                    for
                        (int i = 0; i < ListElemId.Count; i++)
                    {
                        RebarInSystem rebarInSystem = (RebarInSystem)_doc.GetElement(ListElemId[i]);
                        // RebarInSystem rebInsyte = ListElemId[0];
                        for (int j = 0; j < listaView.Count; j++)
                        {
                            if (rebarInSystem.CanBeHidden(listaView[j]))
                            {
                                listaView[j].HideElements(new List<ElementId> { rebarInSystem.Id });
                            }
                        }
                    }
                    // doc.Regenerate();
                    trans2.Commit();
                    // uidoc.RefreshActiveView();
                } // fin trans 
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                statusbarra = Result.Failed;
                message = "Error al crear Path Symbol";
            }
        }
    }


}
