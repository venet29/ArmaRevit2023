using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Armadura.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom
{



    //[System.Runtime.InteropServices.Guid("FFF94424-FCC2-4A3E-BC2A-12FA727DA794")]
    public sealed class BarraRoom_AreaRefuerzo
    {
        #region 0)propiedades generales

        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian
        //Propiedades***********************************************************************************************************************************
        // porcentaje de largo minimo
        XYZ ptocero = XYZ.Zero;
        bool Ismensaje12Mt = true;
        public static int contador { get; set; }


        List<XYZ> ListaPtos = new List<XYZ>();
        // private ExternalCommandData commandData;
#pragma warning disable CS0649 // Field 'BarraRoom_AreaRefuerzo._doc' is never assigned to, and will always have its default value null
        private Document _doc;
#pragma warning restore CS0649 // Field 'BarraRoom_AreaRefuerzo._doc' is never assigned to, and will always have its default value null
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.opt' is never used
        private Options opt;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.opt' is never used
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.uidoc' is never used
        private UIDocument uidoc;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.uidoc' is never used
#pragma warning disable CS0649 // Field 'BarraRoom_AreaRefuerzo._uiapp' is never assigned to, and will always have its default value null
        private UIApplication _uiapp;
#pragma warning restore CS0649 // Field 'BarraRoom_AreaRefuerzo._uiapp' is never assigned to, and will always have its default value null
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.app' is never used
        private Application app;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.app' is never used
#pragma warning disable CS0649 // Field 'BarraRoom_AreaRefuerzo.view3D' is never assigned to, and will always have its default value null
        private View3D view3D;
#pragma warning restore CS0649 // Field 'BarraRoom_AreaRefuerzo.view3D' is never assigned to, and will always have its default value null
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.view3D_Visualizar' is never used
        private View3D view3D_Visualizar;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.view3D_Visualizar' is never used
        //   private string rutaRaiz = InfoSystema.ObtenerRutaDeFamilias();



        //private string rutaRaiz = @"F:\_revit\FAMILY\";
        // Room m_roomSelecionado = null;

        public BarraRoomGeometria barraLosaGeometria { get; set; }

        //level de losa
        public Level LevelLosa { get; set; }

        //direccion mayor de la losa
        public XYZ DireccionMayor { get; set; }

        // view en que se analiza la losa
        public View _view { get; set; }

        ///public string TipoBarra { get; set; }

        public IList<Curve> curvesPathreiforment;
        public IList<Curve> CurvesPathreiforment
        {
            get { return curvesPathreiforment; }
            set { curvesPathreiforment = value; }
        }

        public IList<Curve> CurvesPathreiforment_IzqInf { get; set; }
        public IList<Curve> CurvesPathreiforment_DereSup { get; set; }
        //  public double LargoPathreiforment { get; set; }

        private double largoPathreiforment;

        public double LargoPathreiforment
        {
            get { return largoPathreiforment; }
            set
            {
                largoPathreiforment = value;

                if (largoPathreiforment > Util.CmToFoot(1200) && Ismensaje12Mt)
                { Util.InfoMsg($"Barra Mayor a 12mt   LArgo: {Math.Round(Util.FootToCm(largoPathreiforment) / 100, 1)} mt"); }

            }
        }

        public double LargoRecorrido { get; set; }

        //se utiliza para modificar la familas de rebarshape previo a agergarlos a los pathrteinforment
        public DimensionesBarras dimBarras { get; set; }
        public DimensionesBarras dimBarrasAlternativa { get; set; }

        // se utiliza para guardar los valores en los parametrso compartido del rebar shape ----  A,B,C,D,E
        public DimensionesBarras dimBarras_parameterSharedLetras { get; set; }
        // tipo de barra -- PathRefuerza o AreaRefuerza
        public TipoRefuerzo Tiporefuerzo { get; set; }

        public TipoDireccionBarra TipoDireccionBarra_ { get; set; } = TipoDireccionBarra.NONE;
        //orientacion de barra  vertical u orizontal
        // public TipoOrientacionBarra TipoOrientacion { get; set; }
        // cuantia de la barra a dibuja
        public string CuantiaB { get; set; }


        #endregion

        #region Propiedades  BarraLosaGeometria

        public UbicacionLosa ubicacionEnlosa { get; set; } = UbicacionLosa.Derecha;


#pragma warning disable CS0414 // The field 'BarraRoom_AreaRefuerzo.rebarBarType' is assigned but its value is never used
        RebarBarType rebarBarType = null;
#pragma warning restore CS0414 // The field 'BarraRoom_AreaRefuerzo.rebarBarType' is assigned but its value is never used


        public PathReinforcement m_createdPathReinforcement { get; set; }
        public PathReinforcement m_createdPathReinforcement_dere { get; set; }
        public PathReinforcement m_createdPathReinforcement_izq { get; set; }

        public PathReinSpanSymbol _PathReinSpanSymbol { get; set; }


        public string nombreSimboloPathReinforcement = "";
        public AreaReinforcement m_createdAreaReinforcement = null;
        public string nombreSimboloAreaReinforcement = "";
        //public IList<Curve> curvesPathreiforment { get; set; }
        public string TipoBarraStr { get; set; }
        public TipoOrientacionBarra TipoOrientacion { get; set; }


        public Room _roomSelecionado1;


        public Room RoomSelecionado_1
        {
            get { return _roomSelecionado1; }
            set { _roomSelecionado1 = value; }
        }
        public Room _roomSelecionado2;
        public Room RoomSelecionado_2
        {
            get { return _roomSelecionado2; }
            set { _roomSelecionado2 = value; }
        }



        //angulo de pelota de losa
        public double Angle_pelotaLosa1Grado { get; set; }
        //angulo de pelota de losa
        public double Angle_pelotaLosa2 { get; set; }

        //Espesor de losa
        public double EspesorLosaCm_1 { get; set; }
        public double EspesorLosa_2 { get; set; }

        //Espesor de losa
        public double EspesorMuro_Izq_abajo { get; set; }
        public double EspesorMuro_Dere_Sup { get; set; }



        public double largoMin_1;
        public double LargoMin_1
        {
            get { return largoMin_1; }
            set { largoMin_1 = value; }
        }
        public double largoMin_2;
        public double LargoMin_2
        {
            get { return largoMin_2; }
            set { largoMin_2 = value; }
        }
        #region Hoook

        //HOOK barra Principal
        // star es el que tiene x menor   
        // si es vertical start y menor
        public RebarHookType tipodeHookStartPrincipal { get; set; }
        public RebarHookType tipodeHookEndPrincipal { get; set; }

        //Rebar shape Principal y secundaria
        public RebarShape tipoRebarShapePrincipal { get; set; }
        public RebarShape tipoRebarShapeAlternativa { get; set; }


        // HOOK barra alternativa
        // star es el que tiene x menor   
        // si es vertical start y menor
        public RebarHookType tipodeHookStarAlternativa { get; set; }
        public RebarHookType tipodeHookEndAlternativa { get; set; }
        public bool IsBarrAlternative { get; set; }
        #endregion


        //lista con los puntos poligono de los segmentos intersecatados , 4 ptos, inical y final de los dos segmentos
        // [ ptoini1, ptofin1 , ptoini2 , ptofin2]
        //  1 __ 1    sentido horizontal          1 __ 2    sentido vertical
        //  2    2                                1 __ 2 
        public List<XYZ> ListaPtosPoligonoLosa { get; set; }

        //lista con los puntos que circunscribe el area que ocupara la losa
        //0  - 3
        //1  - 2
        public List<XYZ> ListaPtosPerimetroBarras { get; set; }
        public SeleccionarLosaBarraRoom _seleccionarLosaBarraRoom { get; set; }
        public int DiametroOrientacionPrincipal_mm { get; set; } = 0;

        //floor que pertenece las barras
        public Floor LosaSeleccionada1 { get; set; }
        //floor que pertenece las barras
        public Floor SelecFloor_2 { get; set; }

        public string rutaImagenPricipal { get; set; }
        public string rutaImagenAlternativa { get; set; }
        public Dictionary<string, string> listaImagenes = new Dictionary<string, string>(20);


        public Dictionary<string, string> listaRutasFamilias = new Dictionary<string, string>(20);


        public Result statusbarra { get; set; }
        public string message { get; set; }

        #endregion

        // XYZ ptoSinDesface_ = XYZ.Zero;
        public bool IsLuzSecuandiria { get; set; }

        public int diametroEnMM { get; set; }
        public double Espaciamiento { get; set; }

        /// <summary>
        /// largo del recorrido de la barra
        /// esto es opcional, pq se utilza solo cuando se genera enfierra barra inferior automatio
        /// F:\_revit\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit.Model\observaciones.docx
        /// Observacion 1)
        /// </summary>
        public double LargoRecorridoX { get; set; }
        public double LargoRecorridoY { get; set; }


        //  public BarraRoomDatos barraRoomDatos { get; set; }
        public string TipoBarra_izq_Inf { get; set; }
        public string TipoBarra_dere_sup { get; set; }
        public XYZ ptoConMouseEnlosaF1_SUPIzqINf { get; private set; }
        public XYZ ptoConMouseEnlosaF1_SUPDereSup { get; private set; }
        public XYZ ptoIZqInf_Dimension { get; internal set; }
        public XYZ ptoDereSUp_Dimension { get; internal set; }
        public XYZ ptoDereSup_Dimension { get; private set; }
        public object Enumeracion { get; private set; }
        public double AnguloBordeRoomYSegundoPtoMouseGrado { get; private set; }

        // BarraRoomPersis barraRoomPersis;

        private XYZ ptoConMouseEnlosa1;
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.ptoConMouseEnlosa2' is never used
        private XYZ ptoConMouseEnlosa2;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.ptoConMouseEnlosa2' is never used
        //private CargarFAmilias_carga cargarFAmilias_carga;

#pragma warning disable CS0649 // Field 'BarraRoom_AreaRefuerzo._datosNuevaBarraDTO' is never assigned to, and will always have its default value null
        private DatosNuevaBarraDTO _datosNuevaBarraDTO;
#pragma warning restore CS0649 // Field 'BarraRoom_AreaRefuerzo._datosNuevaBarraDTO' is never assigned to, and will always have its default value null
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.IsTest' is never used
        private bool IsTest;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.IsTest' is never used
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo._solicitudBarraDTO' is never used
        private SolicitudBarraDTO _solicitudBarraDTO;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo._solicitudBarraDTO' is never used



        public ReferenciaRoomDatos _refereciaRoomDatos { get; set; }
        //private PathReinformeTraslapoDatos datosNuevoPath1;
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo._utilfallas' is never used
        private UtilitarioFallasAdvertencias _utilfallas;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo._utilfallas' is never used
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo._listaTAgBArra' is never used
        private IGeometriaTag _listaTAgBArra;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo._listaTAgBArra' is never used
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.IsCasoS4' is never used
        private bool IsCasoS4;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.IsCasoS4' is never used
#pragma warning disable CS0414 // The field 'BarraRoom_AreaRefuerzo._BarraTipo' is assigned but its value is never used
        private TipoRebar _BarraTipo = TipoRebar.NONE;
#pragma warning restore CS0414 // The field 'BarraRoom_AreaRefuerzo._BarraTipo' is assigned but its value is never used
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo._NombreVista' is never used
        private string _NombreVista;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo._NombreVista' is never used
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.elemtoSymboloPath' is never used
        private Element elemtoSymboloPath;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.elemtoSymboloPath' is never used

        public BarraRoom newBarralosa_izq { get; set; }
        public BarraRoom newBarralosa_dere { get; set; }

        // verifca si todo esta ok
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.IsOK' is never used
        bool IsOK;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.IsOK' is never used
#pragma warning disable CS0169 // The field 'BarraRoom_AreaRefuerzo.ptSymbol' is never used
        private TiposPathReinformentSymbol ptSymbol;
#pragma warning restore CS0169 // The field 'BarraRoom_AreaRefuerzo.ptSymbol' is never used

        //cosntructor************************************************************************************************************************
        #region 1)constructor



        //1-CONTRUCTOR ORIGINAL -- diseñar con mouse
        public BarraRoom_AreaRefuerzo(UIApplication uiapp, string tipoBarra, UbicacionLosa ubicacionEnlosa_, DatosDiseñoDto _datosDiseñoDto,
                            bool IsBuscarTipoBarra = false, XYZ ptoCOnMOuse1 = null, XYZ ptoCOnMOuse2 = null, Floor floor = null, bool IsTest = false)
        {

        }



    
        #endregion

        //meotods************************************************************************************************************************
        #region 2)metodo



        public Result CrearBarra(IList<Curve> curvesPathreiforment_, double LargoPathreiforment_, string nombreSimboloPathReinforcement_, int diametro_, double espaciamiento_, XYZ desplazamientoPathReinSpanSymbol)
        {
            Stopwatch timeMeasure = Stopwatch.StartNew();


            if (XYZ.Zero.DistanceTo(desplazamientoPathReinSpanSymbol) > 0.1)
            {
                ptoConMouseEnlosa1 = desplazamientoPathReinSpanSymbol;
                desplazamientoPathReinSpanSymbol = XYZ.Zero;
            }

            if (!VerificarDatos(_datosNuevaBarraDTO)) return Result.Failed;
            ConstNH.sbLog.AppendLine($"       C.0)Tiempo   VerificarDatos : {timeMeasure.ElapsedMilliseconds } ms");
            ConstNH.sbLog.Clear();
            //   _uiapp.Application.FailuresProcessing -= new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);

 
            if (Tiporefuerzo == TipoRefuerzo.AreaRefuerzo && ListaPtosPerimetroBarras.Count > 0)
            {

                try
                {
                    using (Transaction trans = new Transaction(_doc))
                    {
                        trans.Start("Crear areaFeinfoment-NH");

                        IList<Curve> curves = new List<Curve>();
                        curves = BarraRoomGeometria.CrearCurva(ListaPtosPerimetroBarras);


                        //crea el refuerzo de area
                        m_createdAreaReinforcement = CrearAreaRefuerzo(LosaSeleccionada1, curves, TipoOrientacion, tipodeHookStartPrincipal.Id);
                        if (m_createdAreaReinforcement == null)
                        {
                            message = "NO se pudo crear m_createdAreaReinforcement";
                            trans.RollBack();
                            return statusbarra = Result.Failed;
                        }


                        if (view3D != null)
                        {
                            //permite que la barra se vea en el 3d como solido
                            m_createdAreaReinforcement.SetSolidInView(view3D, true);
                            //permite que la barra se vea en el 3d como sin interferecnias 
                            m_createdAreaReinforcement.SetUnobscuredInView(view3D, true);
                        }

                        // desactiva las capa inferior y define que capa inferior aparece botton minor , major
                        LayoutRebar_AreaRefuerzo(TipoOrientacion);


                        //agrega Hook - si esta direccion botton Major activa
                        //Botton Major Hook Type
                        if (ParameterUtil.FindParaByName(m_createdAreaReinforcement, "Bottom Major Direction").AsInteger() == 1)
                            ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BOTTOM_DIR_1, tipodeHookStartPrincipal.Id);

                        //agrega Hook - si esta direccion botton minor activa
                        //Botton minor Hook Type   
                        if (ParameterUtil.FindParaByName(m_createdAreaReinforcement, "Bottom Minor Direction").AsInteger() == 1)
                            ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_HOOK_TYPE_BOTTOM_DIR_2, tipodeHookStartPrincipal.Id);


                        // Obtiene el symbolo dentro de la libreria de familia
                        Element elemtoSymboloArea = TiposAreaReinSpanSymbol.getAreaReinSpanSymbol("M_Area Reinforcement Symbol", _doc);
                        if (elemtoSymboloArea == null)
                        {
                            Util.ErrorMsg($"NO se pudo  encontar PathSymbol M_Area Reinforcement Symbo");
                            trans.RollBack();
                            return statusbarra = Result.Failed;
                        }

                        int x = 0;
                        int y = 0;


                        // Agrega el simbolo de la barra, para definir y generar la forma en el view
                        RebarSystemSpanSymbol symbolPath = RebarSystemSpanSymbol.Create(_doc, _view.Id, new LinkElementId(m_createdAreaReinforcement.Id), new XYZ(x, y, 0), elemtoSymboloArea.Id);
                        if (symbolPath == null) { message = "NO se pudo crear Area Symbol"; trans.RollBack(); return statusbarra = Result.Failed; }

                        // crea el tag con la cuentia de las barra 
                        Element IndependentTagAreaReif = TiposAreaReinTags.M1_GetFamilySymbol_nh("M_Area Reinforcement Tag", _doc);
                        if (IndependentTagAreaReif == null) { message = "NO se pudo obtener el TAg de AreaReinforcement"; trans.RollBack(); return statusbarra = Result.Failed; }

                        IndependentTag asd = IndependentTag.Create(_doc, IndependentTagAreaReif.Id, _view.Id, new Reference(m_createdAreaReinforcement), false,
                                                                    (TipoOrientacion == TipoOrientacionBarra.Horizontal ? TagOrientation.Horizontal : TagOrientation.Vertical), new XYZ(0, 0, 0));

                        // doc.Regenerate();
                        trans.Commit();
                        //   uidoc.RefreshActiveView();
              

                    }//fin trans

                }
                catch (Exception)
                {
                    //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);
                    statusbarra = Result.Failed;
                    message = "Error al crear Path Symbol";
                    return Result.Failed;
                }

            }

            // solo se usa para  casos especiales
            Transaction transaction2 = new Transaction(_doc, "CreatePathReinforcement2");

            transaction2.Start();
            // cambia AreaReinforcement a Rebar  -----  se puede aplicar tanto a  'AreaReinforcement' como 'PathReinforcement'
            IList<ElementId> aux_rebar_internaId = AreaReinforcement.RemoveAreaReinforcementSystem(_doc, m_createdAreaReinforcement);
            // lo pasa a elemnt
            Element aux_rebar_interna = _doc.GetElement(aux_rebar_internaId[0]);
            //comprueb q sea rebar
            if (aux_rebar_interna is Rebar)
            {
                Rebar grupoBrras = aux_rebar_interna as Rebar;
                //cambia gancho inicial 
                grupoBrras.SetHookTypeId(0, TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc).Id);
                //cambia gancho final
                grupoBrras.SetHookTypeId(1, TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc).Id);
            }
            transaction2.Commit();

            ConstNH.sbLog.AppendLine($"       C.11)Tiempo   final : {timeMeasure.ElapsedMilliseconds } ms");
            return Result.Succeeded;
        }


        private bool VerificarDatos(DatosNuevaBarraDTO datosNuevaBarraDTO)
        {
            if (datosNuevaBarraDTO.nombreFamiliaRebarShape == "")
            {
                Util.ErrorMsg($"sin nombreFamiliaRebarShape");
                return false;
            }

            if (datosNuevaBarraDTO.tipoRebarShapePrincipal == null)
            {
                Util.ErrorMsg($"sin tipoRebarShapePrincipal : \n NombreRebarShape {datosNuevaBarraDTO.nombreFamiliaRebarShape} " +
                                                             $"\n TipoRebarShapePrincipal  {datosNuevaBarraDTO.tipoRebarShapePrincipal}");
                return false;
            }
            if (datosNuevaBarraDTO.LargoPathreiforment < Util.CmToFoot(10))
            {
                Util.ErrorMsg($"LargoPathreiforment valor min (10cm) --> {datosNuevaBarraDTO.LargoPathreiforment}");
                return false;
            }
            if (datosNuevaBarraDTO.EspaciamientoFoot < Util.CmToFoot(5))
            {
                Util.ErrorMsg($"Espaciamiento de barra menor a 5cb --> {datosNuevaBarraDTO.EspaciamientoFoot}");
                return false;
            }

            return true;
        }

   
        #region 2.1)  3 Metodos Crear AreaReinforcement



        /// <summary>
        /// crea refuerzo de area 
        /// </summary>
        /// <param name="selecFloor">floor seleccionada</param>
        /// <param name="curves">lista con curvas de area a reforzar</param>
        private AreaReinforcement CrearAreaRefuerzo(Floor selecFloor, IList<Curve> curves, TipoOrientacionBarra orien, ElementId rebarHookTypeId)
        {
            AreaReinDataOnFloor dataOnFloor = new AreaReinDataOnFloor();




            //int aux_vect = 1;

            //si caso vertical si vector es positivo (hacia arriba)  symbol path area crea una barras patas arriba
            // si caso vertical si vector es negativo (hacia bajo)  symbol path area crea una barras patas abajo --> suple

            //si caso horizontal si vector es positivo (hacia adelante)  symbol path area crea una barras patas arriba
            // si caso horizontal si vector es negativo (hacia atras)  symbol path area crea una barras patas abajo --> suple

            //  majorDirection = majorDirection * aux_vect;

            //Create AreaReinforcement
            ElementId areaReinforcementTypeId = AreaReinforcementType.CreateDefaultAreaReinforcementType(_uiapp.ActiveUIDocument.Document);
            ElementId rebarBarTypeId = RebarBarType.CreateDefaultRebarBarType(_uiapp.ActiveUIDocument.Document);
            // ElementId rebarHookTypeId = RebarHookType.CreateDefaultRebarHookType(m_revit.Application.ActiveUIDocument.Document);
            AreaReinforcement areaRein = AreaReinforcement.Create(_uiapp.ActiveUIDocument.Document, selecFloor, curves, DireccionMayor, areaReinforcementTypeId, rebarBarTypeId, rebarHookTypeId);

            //set AreaReinforcement and it's AreaReinforcementCurves parameters
            dataOnFloor.FillIn(areaRein);

            return areaRein;
        }


        /// <summary>
        /// confugura AreaReinforcement
        /// a) desactva capa superior
        /// b) desactiva barras horizontaes o deverticales segun sea el caso
        /// </summary>
        /// <param name="Orientacion"></param>
        private void LayoutRebar_AreaRefuerzo(TipoOrientacionBarra Orientacion)
        {
            if (m_createdAreaReinforcement == null)
            {
                TaskDialog.Show("Error", "Referencia a AreaDeRefuerzo null " + ListaPtosPerimetroBarras.Count);
                return;
            }

            ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1, 0);// 	"Top Major Direction" 
            ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2, 0);// 	"Top Minor Direction" 

            if (Orientacion == TipoOrientacionBarra.Horizontal)
            {
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 0);// 	 	"Bottom Major Direction" 

                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2, 1);// 	 	"Bottom Minor Direction" 

                RebarBarType rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametroEnMM, _doc, true);
                if (null == rebarBarType) { message += "Error al obtener la familiar Ø" + diametroEnMM + " LayoutRebar_AreaRefuerzo"; statusbarra = Result.Cancelled; return; }

                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BOTTOM_DIR_2, rebarBarType.Id);
                ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2, Espaciamiento);
            }
            else
            {
                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1, 1);// 	 	"Bottom Major Direction" 
                                                                                                                           //1) asigna el tipo de la barra en funcion del diametro, que debe estar creados en la libreria
                RebarBarType rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametroEnMM, _doc, true);
                if (null == rebarBarType) { message += "Error al obtener la familiar Ø" + diametroEnMM + " LayoutRebar_AreaRefuerzo"; statusbarra = Result.Cancelled; return; }
                ParameterUtil.SetParaElementId(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_BAR_TYPE_BOTTOM_DIR_1, rebarBarType.Id);
                ParameterUtil.SetParaDouble(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_1, Espaciamiento);

                ParameterUtil.SetParaInt(m_createdAreaReinforcement, BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2, 0);// 	 	"Bottom Minor Direction" }

            }

            string str = "";
            ParameterSet pars = m_createdAreaReinforcement.Parameters;

            var adas = ParameterUtil.FindParaByName(pars, "REBAR_ELEMENT_VISIBILITY");

            foreach (Parameter param in pars)
            {
                string val = "";
                string name = param.Definition.Name;
                Autodesk.Revit.DB.StorageType type = param.StorageType;
                switch (type)
                {
                    case Autodesk.Revit.DB.StorageType.Double:
                        val = param.AsDouble().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.ElementId:
                        Autodesk.Revit.DB.ElementId id = param.AsElementId();
                        Autodesk.Revit.DB.Element paraElem = _doc.GetElement(id);
                        if (paraElem != null)
                            val = paraElem.Name;
                        break;
                    case Autodesk.Revit.DB.StorageType.Integer:
                        val = param.AsInteger().ToString();
                        break;
                    case Autodesk.Revit.DB.StorageType.String:
                        val = param.AsString();
                        break;
                    default:
                        break;
                }
                str = str + name + ": " + val + "\r\n";
            }
        }

        private void AgregarParametosShared()
        {
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Agergando parametros a areapath-NH");

                    var lista = m_createdPathReinforcement.GetRebarInSystemIds();
                    foreach (var item in lista)
                    {
                        Element elm = _doc.GetElement2(item);
                        if (_view != null && ParameterUtil.FindParaByName(elm, "NombreVista") != null) ParameterUtil.SetParaInt(elm, "NombreVista", _view.ObtenerNombreIsDependencia());  //"nombre de vista"
                    }

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {

                Debug.Write($"Error al agrgar parametros a areaPath {ex.Message}");
            }
        }


        #endregion

   
        #endregion

    }
}
