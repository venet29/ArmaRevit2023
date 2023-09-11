using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Vigas
{
    class DIbujarTAgVIgasView
    {
        private readonly UIApplication _Uiapp;
        private  Element tagViga;
        private Document  _doc;
        private XYZ direccionView;

      //  public Line locationLine { get; private set; }
        public DIbujarTAgVIgasView(UIApplication _uiapp)
        {
            _Uiapp = _uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
          //  locationLine = null;// direccion de view
        }


        public bool ObtenerTodasView()
        {
            try
            {

                tagViga = TipoTagVIga.M1_GetVigaTag("Standard", "TAG VIGA ELEVACION", _doc);

                if (tagViga == null)
                {
                    Util.InfoMsg($"No se encontro tag de viga:'Standard'");
                }
                SeleccionarView _SeleccionarView = new SeleccionarView();
                var ListaViewSection = _SeleccionarView.ObtenerTodosViewSection(_doc);

                for (int i = 0; i < ListaViewSection.Count; i++)
                {
                    dibujarVigas(ListaViewSection[i]);
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }


        public bool dibujarVigas(ViewSection viewSection)
        {
            try
            {
                direccionView = viewSection.RightDirection;
                if (tagViga == null)
                {
                    //Util.InfoMsg($"No se encontro tag de viga:'Standard'");
                    return false;
                }
                List<Element> elemntos = new FilteredElementCollector(_doc, viewSection.Id).OfClass(typeof(Wall)).ToList();

                //a) lista de elemtod vigas - wall
                //   ElementId currentTextTypeId = _uidoc.Document.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

                using (Transaction t = new Transaction(_doc, "Agregar tag Murovigas"))
                {
                    t.Start();
                    foreach (Wall wall in elemntos)
                    {
                        if (wall.Name.Contains("V") && IsWallParaleloSeccion(wall))
                        {
                            //crear texto


                            if (tagViga != null)
                            {
                                IndependentTag independentTag = IndependentTag.Create(_doc, tagViga.Id, viewSection.Id, new Reference(wall), false, TagOrientation.Horizontal, new XYZ(0, 0, 0));
                                independentTag.TagHeadPosition = PosicionTexttoVIga(wall);
                            }


                        }
                    }//fin for
                    t.Commit();
                }
                //b) lista de elemtod vigas - beam
                List<FamilyInstance> ListaVigas = new List<FamilyInstance>();
                ListaVigas = GetVigaFromLevel(_doc, viewSection);

                using (Transaction t = new Transaction(_doc, "Agregar tag vigas"))
                {
                    t.Start();
                    //int i = 0;
                    // var _stopwatchTiempoIntervalo = new Stopwatch();
                    // _stopwatchTiempoIntervalo.Start();

                    // Parallel.ForEach(ListaVigas, viga =>
                    foreach (var viga in ListaVigas)
                    {
                        if (viga.Name.Contains("V") && IsBeamParaleloSeccion(viga))
                        {
                            //crear texto
                            if (tagViga != null)
                            {
                                IndependentTag independentTag = IndependentTag.Create(_doc, tagViga.Id, viewSection.Id, new Reference(viga), false, TagOrientation.Horizontal, new XYZ(0, 0, 0));
                                independentTag.TagHeadPosition = PosicionTexttoVIga(viga);
                                // Debug.Print($" Tiempo iteracion {i}   tiempo:{String.Format("{0} : {1:hh\\:mm\\:ss}", "Tiempo Final Cierre", _stopwatchTiempoIntervalo.Elapsed)} ");
                                //  i = i + 1;
                            }
                        }
                    }//fin for
                     // );
                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al  crear tag de vigas { ex.Message}");
                return false;
            }
            return true;

        }

        /// <summary>
        /// obtiene lista de las vigas presentes en seccion
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public List<FamilyInstance> GetVigaFromLevel(Document document, View viewSection)
        {
            // Structural type filters firstly
            LogicalOrFilter stFilter = new LogicalOrFilter(new ElementStructuralTypeFilter(StructuralType.Beam), new ElementStructuralTypeFilter(StructuralType.Beam));
            //  LogicalOrFilter stFilter = new LogicalOrFilter(new ElementStructuralTypeFilter(StructuralType.Beam), new ElementStructuralTypeFilter(StructuralType.Column));
            // StructuralMaterialType should be Concrete
            LogicalAndFilter hostFilter = new LogicalAndFilter(stFilter, new StructuralMaterialTypeFilter(StructuralMaterialType.Concrete));

            FilteredElementCollector collector3 = new FilteredElementCollector(document, viewSection.Id);
            collector3.OfClass(typeof(FamilyInstance)).WherePasses(hostFilter); // Filters;
            List<FamilyInstance> listLevelLINQ = collector3.OfType<FamilyInstance>().ToList();

            return listLevelLINQ;
        }



        /// <summary>
        /// comproeba si wall esta en direccion paralela a la direccion del la seccion
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        private bool IsWallParaleloSeccion(Wall wall)
        {
            LocationCurve location = (wall as Wall).Location as LocationCurve;
            Line line = location.Curve as Line;
            Autodesk.Revit.DB.XYZ first = line.GetEndPoint(0);
            Autodesk.Revit.DB.XYZ second = line.GetEndPoint(1);


            return Util.IsParallel(first - second, direccionView);
        }
        /// <summary>
        /// comproeba si viga esta en direccion paralela a la direccion del la seccion
        /// </summary>
        /// <param name="fminst"></param>
        /// <returns></returns>
        private bool IsBeamParaleloSeccion(FamilyInstance fminst)
        {
            LocationCurve location = (fminst as Element).Location as LocationCurve;
            Line line = location.Curve as Line;
            Autodesk.Revit.DB.XYZ first = line.GetEndPoint(0);
            Autodesk.Revit.DB.XYZ second = line.GetEndPoint(1);


            return Util.IsParallel(first - second, direccionView);
        }


        public static Autodesk.Revit.DB.XYZ PosicionTexttoVIga(Element elem)
        {
            LocationCurve location = elem.Location as LocationCurve;
            Line locationLine = location.Curve as Line;

            FamilyInstance instance = elem as FamilyInstance;
            double startOffset = instance.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE).AsDouble();

            Autodesk.Revit.DB.XYZ first = locationLine.GetEndPoint(0);
            Autodesk.Revit.DB.XYZ second = locationLine.GetEndPoint(1);
            double x = (first.X + second.X) / 2;
            double y = (first.Y + second.Y) / 2;
            double z = (first.Z + second.Z) / 2 + 1.3 + startOffset;




            Autodesk.Revit.DB.XYZ midPoint = new Autodesk.Revit.DB.XYZ(x, y, z);
            return midPoint;
        }

    }
}
