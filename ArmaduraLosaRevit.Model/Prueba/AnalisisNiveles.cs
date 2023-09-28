using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.Prueba
{
    public class AnalisisNiveles
    {
        // analisas como saber si se esta usando
        public static bool nombreFuncion(Document _doc)
        {
            try
            {
                // Document _doc = uidoc.Document;

                // Obtener todos los niveles del documento
                FilteredElementCollector levelsCollector = new FilteredElementCollector(_doc).OfClass(typeof(Level));
                List<Level> levels = levelsCollector.Cast<Level>().ToList();

                // Obtener Project Base Point
                //FilteredElementCollector basePointCollector = new FilteredElementCollector(_doc).OfClass(typeof(BasePoint));
                //BasePoint projectBasePoint = basePointCollector.FirstOrDefault() as BasePoint;

                XYZ pBaseLocation = InfoProject.OBtenerbase_point(_doc);

                // Obtener Survey Point
                //FilteredElementCollector surveyPointCollector = new FilteredElementCollector(_doc).OfClass(typeof(SurveyPoint));
                //SurveyPoint surveyPoint = surveyPointCollector.FirstOrDefault() as SurveyPoint;

                XYZ sPointLocation = InfoProject.OBtenerbase_SurveyPointPosition(_doc);

                foreach (var level in levels)
                {
                    double levelElevation = level.Elevation;
                    double levelProjectElevation = level.ProjectElevation;
                    if (pBaseLocation != XYZ.Zero)
                    {
                        // XYZ pBaseLocation = projectBasePoint.Position;
                        if (Math.Abs(pBaseLocation.Z - levelElevation) < 0.001) // tolerancia
                        {
                            Util.InfoMsg($"El 'Project Base Point' está asociado con el nivel");
                        }
                    }

                    if (pBaseLocation != XYZ.Zero)
                    {
                        //XYZ sPointLocation = surveyPoint.Position;
                        if (Math.Abs(sPointLocation.Z - levelElevation) < 0.001) // tolerancia
                        {
                            Util.InfoMsg($"El 'Survey Point' está asociado con el nivel.");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public static bool CAmbiarPAramet(Document _doc)
        {
            try
            {
                // Obtener todos los niveles del documento
                FilteredElementCollector levelsCollector = new FilteredElementCollector(_doc).OfClass(typeof(Level));
                List<Level> levels = levelsCollector.Cast<Level>().ToList();
                // Asumiendo que tienes una referencia al Level que deseas modificar:
                Level myLevel = levels.FirstOrDefault();

                // 1. Obtener LevelType del Level
                LevelType levelType = _doc.GetElement(myLevel.GetTypeId()) as LevelType;

                // 2. Acceder al parámetro 'Elevation Base'
                Parameter elevationRelativeParam = levelType.get_Parameter(BuiltInParameter.LEVEL_RELATIVE_BASE_TYPE);

               // Parameter elevationBaseParam = levelType.get_Parameter(BuiltInParameter.CONTOUR_LABELS_ELEV_BASE_TYPE);

                if (elevationRelativeParam != null && elevationRelativeParam.HasValue && !elevationRelativeParam.IsReadOnly)
                {
                    using (Transaction trans = new Transaction(_doc, "Cambiar Elevation Base"))
                    {
                        trans.Start();

                        // 3. Modificar el valor del parámetro
                        // Por ejemplo, para cambiar a "Survey Level":
                        elevationRelativeParam.Set(1); // 0 = Project Base Point, 1 = Survey Level

                        trans.Commit();
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


    }



}
