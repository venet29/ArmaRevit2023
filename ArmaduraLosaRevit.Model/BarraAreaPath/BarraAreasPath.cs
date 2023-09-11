using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraAreaPath.Intervalo;
using ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Visibilidad;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath
{
    public class BarraAreasPath : BarraAreasPathBase
    {
        private List<Curve> _curvas;
        //   public AreaReinforcement m_createdAreaReinforcement = null;
        //  public string nombreSimboloAreaReinforcement = "";
        private readonly UIApplication _uiapp;
        public View3D _view3D_paraVisualizar;
        private readonly InterrvalosPtosPathArea _interrvalosPtosPathArea;



        private DatosMallasAutoDTO _datosMallasDTO;
        private Document _doc;
        private View view;
#pragma warning disable CS0169 // The field 'BarraAreasPath.TipoOrientacion' is never used
        private TipoOrientacionBarra TipoOrientacion;
#pragma warning restore CS0169 // The field 'BarraAreasPath.TipoOrientacion' is never used
        private IList<ElementId> listaRebarInSystemIds;
        private static Element IndependentTagAreaReif;
        private static Element _elemtoSymboloArea;

        public BarraAreasPath(UIApplication uiapp, View3D view3D_paraVisualizar, InterrvalosPtosPathArea interrvalosPtosPathArea) : base(uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view3D_paraVisualizar = view3D_paraVisualizar;
            this._interrvalosPtosPathArea = interrvalosPtosPathArea;
            //  this.seleccionMuroAreaPath = seleccionMuroAreaPath;
            this.view = _doc.ActiveView;


            // this._datosMallasDTO = _datosMallasDTO;
        }



        public void Ejecutar_conIntervalos()
        {
            DatosIniciales();
            // List<IntervalosMallaDTO> ListaIntervalosMallaDTO 
            foreach (IntervalosMallaDTO _intervalosMallaDTO in _interrvalosPtosPathArea.ListaIntervalosMallaDTO)
            {
                if (_intervalosMallaDTO._muroSeleccionado == null) continue;
                this._curvas = _intervalosMallaDTO.ListaCurvaPAthArea;
                DireccionMayor = _interrvalosPtosPathArea._direccionMayor;
                this.ElementHost = _intervalosMallaDTO._muroSeleccionado;
                this._datosMallasDTO = _intervalosMallaDTO._datosMallasDTO;
                DibujarConTrans();
                CAmbiarColor();

            }
        }


        public void Ejecutar_conIntervalos2()
        {
            if (!DatosIniciales2()) return;
            // List<IntervalosMallaDTO> ListaIntervalosMallaDTO 
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Crear areaFeinfoment-NH");
                    foreach (IntervalosMallaDTO _intervalosMallaDTO in _interrvalosPtosPathArea.ListaIntervalosMallaDTO)
                    {
                        if (_intervalosMallaDTO._muroSeleccionado == null) continue;
                        this._curvas = _intervalosMallaDTO.ListaCurvaPAthArea;
                        DireccionMayor = _interrvalosPtosPathArea._direccionMayor;
                        this.ElementHost = _intervalosMallaDTO._muroSeleccionado;
                        this._datosMallasDTO = _intervalosMallaDTO._datosMallasDTO;
                        DibujarSinTrans();


                    }
                    trans.Commit();
                }

                var listId = ListaAreaReinforcement.SelectMany(c => c.GetRebarInSystemIds()).ToList();
                CAmbiarColor(listId);
                //cambiar color y borrra barras horizontales superiores
                // ModificacionesExtra();

            }
            catch (Exception)
            {

                throw;
            }


        }

        private void CAmbiarColor()
        {
            listaRebarInSystemIds = m_createdAreaReinforcement.GetRebarInSystemIds();
            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(listaRebarInSystemIds.ToList(), FactoryColores.ObtenerColoresPorNombre(TipoCOlores.ParaMalla), true);
            //visibilidadElement
        }
        protected void CAmbiarColor(List<ElementId> listID)
        {
            // listaRebarInSystemIds = m_createdAreaReinforcement.GetRebarInSystemIds();
            VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
            visibilidadElement.ChangeListaElementColorConTrans(listID, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.ParaMalla), true);
            //visibilidadElement
        }
        private void CAmbiarColor2(Transaction trans, AreaReinforcement m_createdAreaReinforcement)
        {
            try
            {
                listaRebarInSystemIds = m_createdAreaReinforcement.GetRebarInSystemIds();
                //Color newcolor = new Color((byte)102, (byte)102, (byte)102);
                VisibilidadElementMallaMuro visibilidadElement = new VisibilidadElementMallaMuro(_uiapp);
                visibilidadElement.ChangeListElementsColorSinTrans( listaRebarInSystemIds.ToList(), FactoryColores.ObtenerColoresPorNombre(TipoCOlores.ParaMalla), true);
                //visibilidadElement
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CAmbiarColor2   ->   ex:{ex.Message}");
            }
        }
        public void DatosIniciales()
        {
            // _itipoHook.voidDefinirHookWall();
            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);

            tipodeHookEndAlternativa = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
            tipodeHookStarAlternativa = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);

        }

        public bool DatosIniciales2()
        {
            //ElementHost = seleccionMuroAreaPath._WallSelect;

            try
            {


                // _itipoHook.voidDefinirHookWall();
                if (tipodeHookStartPrincipal == null)
                    tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
                if (tipodeHookEndPrincipal == null)
                    tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);

                if (tipodeHookEndAlternativa == null)
                    tipodeHookEndAlternativa = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
                if (tipodeHookStarAlternativa == null)
                    tipodeHookStarAlternativa = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);


                // Obtiene el symbolo dentro de la libreria de familia
                #region Agregar AreaReinSpanSymbol
                if (_elemtoSymboloArea == null)
                    _elemtoSymboloArea = TiposAreaReinSpanSymbol.getAreaReinSpanSymbol("M_Area Reinforcement SymbolNH", _doc);
                if (_elemtoSymboloArea == null)
                {
                    Util.ErrorMsg("NO se pudo  encontar Area Symbol en libreria");

                    return false;
                }


                #endregion

                // crea el tag con la cuentia de las barra 
                if (IndependentTagAreaReif == null)
                    IndependentTagAreaReif = TiposAreaReinTags.M1_GetFamilySymbol_nh("M_Area Reinforcement TagNH", _doc);
                if (IndependentTagAreaReif == null) { Util.ErrorMsg("NO se pudo obtener el TAg de AreaReinforcement"); return false; }

                if (ListaAreaReinforcement == null)
                    ListaAreaReinforcement = new List<AreaReinforcement>();
                else
                    ListaAreaReinforcement.Clear();
            }
            catch (Exception)
            {
                tipodeHookStartPrincipal = null;
                tipodeHookEndAlternativa = null;
                _elemtoSymboloArea = null;
                IndependentTagAreaReif = null;
                ListaAreaReinforcement = null;
                return false;
            }

            return true;
        }
        public bool DibujarConTrans()
        {

            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("Crear areaFeinfoment-NH");
                    //crea el refuerzo de area
                    m_createdAreaReinforcement = CrearAreaRefuerzo(ElementHost, _curvas, tipodeHookStartPrincipal.Id);
                    if (m_createdAreaReinforcement == null)
                    {
                        Util.ErrorMsg("NO se pudo crear m_createdAreaReinforcement");
                        trans.RollBack();
                        return false;
                    }
                    ListaAreaReinforcement.Add(m_createdAreaReinforcement);

                    Visualizacion();


                    // desactiva las capa inferior y define que capa inferior aparece botton minor , major
                    Configurar_LayoutRebar_AreaRefuerzo(_datosMallasDTO, m_createdAreaReinforcement);


                    // agergarparametros
                    AgregarParametosShared();
                    // Obtiene el symbolo dentro de la libreria de familia
                    #region Agregar AreaReinSpanSymbol
                    Element elemtoSymboloArea = TiposAreaReinSpanSymbol.getAreaReinSpanSymbol("M_Area Reinforcement SymbolNH", _doc);
                    if (elemtoSymboloArea == null)
                    {
                        Util.ErrorMsg("NO se pudo  encontar Area Symbol en libreria");
                        trans.RollBack();
                        return false;
                    }

                    int x = 0;
                    int y = 0;


                    // Agrega el simbolo de la barra, para definir y generar la forma en el view
                    RebarSystemSpanSymbol symbolPath = RebarSystemSpanSymbol.Create(_doc, view.Id, new LinkElementId(m_createdAreaReinforcement.Id), new XYZ(x, y, 0), elemtoSymboloArea.Id);
                    if (symbolPath == null)
                    { Util.ErrorMsg("NO se pudo crear Area Symbol"); trans.RollBack(); return false; }
                    #endregion

                    // crea el tag con la cuentia de las barra 
                    Element IndependentTagAreaReif = TiposAreaReinTags.M1_GetFamilySymbol_nh("M_Area Reinforcement TagNH", _doc);
                    if (IndependentTagAreaReif == null) { Util.ErrorMsg("NO se pudo obtener el TAg de AreaReinforcement"); trans.RollBack(); return false; }

                    IndependentTag asd = IndependentTag.Create(_doc, IndependentTagAreaReif.Id, view.Id, new Reference(m_createdAreaReinforcement), false, TagOrientation.Horizontal, new XYZ(0, 0, 0));

                    // doc.Regenerate();
                    trans.Commit();

                }//fin trans

                ModificacionesExtra();
                //AgregarParametosShared();



                return true;
            }
            catch (Exception ex)
            {
                //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);

                Util.ErrorMsg($"Error al crear Path Symbol {ex.Message}   \n {_interrvalosPtosPathArea.GetInfo()} ");
                return false;
            }


        }

        public bool DibujarSinTrans()
        {

            try
            {

                //crea el refuerzo de area
                m_createdAreaReinforcement = CrearAreaRefuerzo(ElementHost, _curvas, tipodeHookStartPrincipal.Id);
                if (m_createdAreaReinforcement == null)
                {
                    Util.ErrorMsg("NO se pudo crear m_createdAreaReinforcement");

                    return false;
                }

                ListaAreaReinforcement.Add(m_createdAreaReinforcement);

                Visualizacion();
                // desactiva las capa inferior y define que capa inferior aparece botton minor , major
                Configurar_LayoutRebar_AreaRefuerzo(_datosMallasDTO, m_createdAreaReinforcement);
                // agergarparametros
                AgregarParametosShared();

                int x = 0;
                int y = 0;
                // Agrega el simbolo de la barra, para definir y generar la forma en el view
                RebarSystemSpanSymbol symbolPath = RebarSystemSpanSymbol.Create(_doc, view.Id, new LinkElementId(m_createdAreaReinforcement.Id), new XYZ(x, y, 0), _elemtoSymboloArea.Id);
                if (symbolPath == null)
                { Util.ErrorMsg("NO se pudo crear Area Symbol"); return false; }

                IndependentTag asd = IndependentTag.Create(_doc, IndependentTagAreaReif.Id, view.Id, new Reference(m_createdAreaReinforcement), false, TagOrientation.Horizontal, new XYZ(0, 0, 0));

                return true;
            }
            catch (Exception ex)
            {
                //  _uiapp.Application.FailuresProcessing += new EventHandler<FailuresProcessingEventArgs>(_utilfallas.ProcesamientoFallas);

                Util.ErrorMsg($"Error al crear Path Symbol {ex.Message}   \n {_interrvalosPtosPathArea.GetInfo()} ");
                return false;
            }


        }

        private void ModificacionesExtra()
        {
            try
            {
                using (Transaction trans = new Transaction(_doc))
                {
                    trans.Start("AgregarColor-NH");
                    for (int i = 0; i < ListaAreaReinforcement.Count; i++)
                    {
                        if (!ListaAreaReinforcement[i].IsValidObject) continue;
                        CAmbiarColor2(trans, ListaAreaReinforcement[i]);
                        Configurar_geometria_AreaRefuerzo(_datosMallasDTO, ListaAreaReinforcement[i]);
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {

                Debug.Write($"Error al agrgar parametros a areaPath {ex.Message}");
            }
        }

       



        private void AgregarParametosShared()
        {
            try
            {
                //using (Transaction trans = new Transaction(_doc))
                //{
                //    trans.Start("Agergando parametros a areapath");

                Element paraElem = _doc.GetElement(m_createdAreaReinforcement.Id);
                if (ParameterUtil.FindParaByName(paraElem, "CuantiaMuro") != null) ParameterUtil.SetParaInt(paraElem, $"CuantiaMuro", _datosMallasDTO.ObtenerTExto());
                if (view != null && ParameterUtil.FindParaByName(paraElem, "NombreVista") != null) ParameterUtil.SetParaInt(paraElem, "NombreVista", view.ObtenerNombreIsDependencia());  //"nombre de vista"

                var lista = m_createdAreaReinforcement.GetRebarInSystemIds();
                foreach (var item in lista)
                {
                    Element elm = _doc.GetElement2(item);
                    if (view != null && ParameterUtil.FindParaByName(elm, "NombreVista") != null) ParameterUtil.SetParaInt(elm, "NombreVista", view.ObtenerNombreIsDependencia());  //"nombre de vista"
                }

                //    trans.Commit();
                //}
            }
            catch (Exception ex)
            {

                Debug.Write($"Error al agrgar parametros a areaPath {ex.Message}");
            }
        }

        private void Visualizacion()
        {
            if (_view3D_paraVisualizar != null)
            {
                //permite que la barra se vea en el 3d como solido
                m_createdAreaReinforcement.SetSolidInView(_view3D_paraVisualizar, true);
                //permite que la barra se vea en el 3d como sin interferecnias 
                m_createdAreaReinforcement.SetUnobscuredInView(_view3D_paraVisualizar, true);
            }
        }
    }
}
