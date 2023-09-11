using System;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.IO;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Elementos_viga;
using System.Linq;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Armadura;

namespace ArmaduraLosaRevit.Model.Elementos_viga
{
    public class NH_ListaOpening
    {
        #region 1) prop y atributos

        #region ATributos de documento
        /// <summary>
        /// object which contains reference to Revit Application
        /// </summary>
        protected Autodesk.Revit.UI.ExternalCommandData m_commandData;
        private UIApplication _uiapp;

        /// <summary>
        /// Revit UI document
        /// </summary>
        Autodesk.Revit.UI.UIDocument m_rvtUIDoc;
        /// <summary>
        /// Revit DB document
        /// </summary>
        protected Autodesk.Revit.DB.Document _doc;
        private View _view;

        #endregion

        /// <summary>
        /// Lista con los puntos de los poligonos de losa
        /// </summary>
        public List<List<XYZ>> ListaPoligoLosa { get; set; }
        public Level _nivelActual { get; set; }
        public List<ProfileOpening> ListaProfileOpening { get; set; }
        #endregion    




        #region 2)contructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandData"></param>
        public NH_ListaOpening(UIApplication uiapp)
        {
            _uiapp = uiapp;
            m_rvtUIDoc = _uiapp.ActiveUIDocument;
            ListaPoligoLosa = new List<List<XYZ>>();
            ListaProfileOpening = new List<ProfileOpening>();
            _doc = m_rvtUIDoc.Document;

            _view = _doc.ActiveView;
            _nivelActual = _view.GenLevel;

        }

        #endregion


        #region 3) metodos

        /// <summary>
        /// 1)obtiene lista de vigas en el nivel de trabajo 
        /// 2)obtiene su  geometria ProfileBeam

        /// </summary>
        public void GetOpeningPoligonos()
        {
            List<Element> listaOpening = SeleccionarOpening.GetOpeningFromLevel(_doc, _nivelActual);
            foreach (var openn in listaOpening)
            {

                if (null != openn)
                {
                    Opening op = (Opening)openn;

                    //nivel de la opening seleccionado
                    Level nivel = (Level)_doc.GetElement(openn.LevelId);
                    ProfileOpening profileOpening = new ProfileOpening(openn, _uiapp, _nivelActual);

                    if (profileOpening != null)
                    {
                        if (profileOpening != null) { ListaProfileOpening.Add(profileOpening); }
                    }
                }
            }

        }




        /// <summary>
        /// Genera LAs lineas de separacion de rooms
        /// 
        /// Utiliza la lista de ListaVigas de 'ProfileBeam' que contiene las poligonos de la
        /// parte superior de cada viga.
        /// 
        /// Crea Una linea y despues con 'm_document.Create.NewRoomBoundaryLines' lo transforma en
        /// 'SeparateRoom'
        /// </summary>
        public void DibujarLineasSeparacicionRoom()
        {

            foreach (var profileOpening in ListaProfileOpening)
            {
                // se sibujam las lines
                List<Line> listaRoomSeparator = profileOpening.CrearSeparacionRoom(_doc);

                if (listaRoomSeparator.Count == 0) continue;

                CurveArray cArray = new CurveArray();
                foreach (Curve item in listaRoomSeparator)
                {
                    cArray.Append(item);
                }

                try
                {

                    SketchPlane skP = SketchPlane.Create(_doc, profileOpening.NivelLosa.Id);
                    var linea = _doc.Create.NewRoomBoundaryLines(skP, cArray, _doc.ActiveView);

                    // cambiar paramatro PHASE_CREATED =Existing
                    foreach (var item in linea)
                    {
                        Element line_e = item as Element;
                        // get the phase id "New construction"
                        if (!TipoPhases_.ObtenerFasesExistenete(_doc)) return;

                        ElementId idPhase = TipoPhases_.idPhase;
                        
                        line_e.get_Parameter(BuiltInParameter.PHASE_CREATED).Set(idPhase);
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    Util.ErrorMsg(" Error al crear 'DibujarLineasSeparacicionRoom' :" + AyudaLosa.ObtenerListaPto(cArray));
                }

            }

        }

        /// <summary>
        /// borra elemntos 'ProfileBeam' de la lista  ListaProfileBeam
        /// </summary>
        public void ClearProfileBeam()
        {
            ListaProfileOpening.Clear();
        }


        //no utlizado
        public void ClearPoligonos()
        {
            ListaPoligoLosa.Clear();
        }
        //no utlizado
        private List<XYZ> ListaFinal_pto(List<XYZ> listaPtos, Element viga)
        {
            List<XYZ> ListaPtoFinal = new List<XYZ>();

            Element floor_1 = null;
            floor_1 = viga as BeamSystem;

            Options gOptions = new Options();
            gOptions.ComputeReferences = false;
            gOptions.DetailLevel = ViewDetailLevel.Coarse;
            gOptions.IncludeNonVisibleObjects = false;
            GeometryElement geo = viga.get_Geometry(gOptions);

            foreach (GeometryObject obj in geo) // 2013
            {
                GeometryInstance geomInst = null;
                if (obj is Solid)
                {
                    Solid solid = obj as Solid;
                    foreach (Face face in solid.Faces)
                    {
                        ////string s = face.MaterialElement.Name; // 2011
                        //string s = FaceMaterialName(doc, face); // 2012
                        //materials.Add(s);
                    }
                }
                else if (obj is GeometryInstance)
                {
                    geomInst = obj as GeometryInstance;
                    //GeometryInstance i = o as GeometryInstance;
                    //materials.AddRange(GetMaterials1(
                    //  doc, i.SymbolGeometry));
                }

                if (geomInst != null)
                {
                    GeometryElement getInstGeo = geomInst.GetInstanceGeometry();
                    GeometryElement getSymbGeo = geomInst.GetSymbolGeometry();
                }

                //var tarsas=getInstGeo.GetTransformed();

                Solid floor_ = obj as Solid;
                if (floor_ != null)
                {
                    foreach (Face f in floor_.Faces)
                    {
                        BoundingBoxUV b = f.GetBoundingBox();
                        UV p = b.Min;
                        UV q = b.Max;
                        UV midparam = p + 0.5 * (q - p);
                        // XYZ midpoint = f.Evaluate(midparam);
                        XYZ normal = f.ComputeNormal(midparam);
                        /// XYZ minxyz = f.Evaluate(b.Min);
                        if (Util.IsVertical(normal) && Util.PointsUpwards(normal))
                        {
                            XYZ ptXAxis = XYZ.BasisX;
                            XYZ ptYAxis = XYZ.BasisY;


                        }
                    }
                }
            }




            return ListaPtoFinal;
        }




        #endregion




    }
}
