using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Seleccionar.IEqualityComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public interface ISeleccionarNivel
    {
        Level M1_ObtenerNivelPOrNombre(string nombre);
        List<Level> M2_ObtenerListaNivelOrdenadoPorElevacion(View _view);

    }




    public class SeleccionarNivel : ISeleccionarNivel
    {
        private UIApplication _uiapp;
        private Document _doc;

        public SeleccionarNivel(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        public Level M1_ObtenerNivelPOrNombre(string nombre)
        {
            //buscar primer nivel
            FilteredElementCollector Colectornivel = new FilteredElementCollector(_doc);
            Level Lv = Colectornivel
                     .OfClass(typeof(Level))
                     .OfCategory(BuiltInCategory.OST_Levels)
                     .Cast<Level>()
                     .Where(X => X.Name == nombre).FirstOrDefault();

            return Lv;
        }

        public List<Level> M2_ObtenerListaNivelOrdenadoPorElevacion(View _view)
        {
            //buscar primer nivel
            List<Level> ListaLevel = ObtenerNiveles(_view);

            ListaLevel = ObtenerNivelNoRepetido(ListaLevel).Select(c => c._Level).ToList();

            return ListaLevel;
        }



        public List<Level> M3_ObtenerListaNivelOrdenadoPorElevacionDeProyecto()
        {
            //buscar primer nivel
            List<Level> ListaLevel = ObtenerNiveles();
            //FilteredElementCollector Colectornivel = new FilteredElementCollector(_doc);
            //List<Level> ListaLevel = Colectornivel
            //         .OfClass(typeof(Level))
            //         .OfCategory(BuiltInCategory.OST_Levels)
            //         .Cast<Level>()
            //         .Where(X => X.Name != "").OrderBy(c => c.ProjectElevation).ToList(); 


            ListaLevel = ObtenerNivelNoRepetido(ListaLevel).Select(c => c._Level).ToList();

            return ListaLevel;
        }
        public List<EnvoltoriLevel> ObtenerListaEnvoltoriLevelOrdenadoPorElevacion(View _view = null)
        {
            try
            {
                //buscar primer nivel
                FilteredElementCollector Colectornivel = default;
                if (_view != null)
                    Colectornivel = new FilteredElementCollector(_doc, _view.Id);
                else
                    Colectornivel = new FilteredElementCollector(_doc);

                List<Level> ListaLevel = Colectornivel
                         .OfClass(typeof(Level))
                         .OfCategory(BuiltInCategory.OST_Levels)
                         .Cast<Level>()
                         .Where(X => X.Name != "").OrderBy(c => c.ProjectElevation).ToList(); ;

                return ObtenerNivelNoRepetido(ListaLevel);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener niveles 'ObtenerListaEnvoltoriLevelOrdenadoPorElevacion'. ex:{ex.Message}");
                return new List<EnvoltoriLevel>();
            }
        }
        private List<Level> ObtenerNiveles(View _view = null)
        {
            FilteredElementCollector Colectornivel = null;//= new FilteredElementCollector(_doc, _view.Id);

            if (_view != null)
                Colectornivel = new FilteredElementCollector(_doc, _view.Id);
            else
                Colectornivel = new FilteredElementCollector(_doc);

            List<Level> ListaLevel = Colectornivel
                     .OfClass(typeof(Level))
                     .OfCategory(BuiltInCategory.OST_Levels)
                     .Cast<Level>()
                     .Where(X => X.Name != "").OrderBy(c => c.ProjectElevation).ToList(); ;
            return ListaLevel;
        }

        private List<EnvoltoriLevel> ObtenerNivelNoRepetido(List<Level> listaLevel)
        {
            int numeroDecima = 4;

            List<EnvoltoriLevel> listaEnvoltoriLevel = new List<EnvoltoriLevel>();
            foreach (var item in listaLevel)
            {
                if (!listaEnvoltoriLevel.Exists(z => z.ElevacionRedondeada == Math.Round(item.ProjectElevation, numeroDecima)))
                {
                    listaEnvoltoriLevel.Add(new EnvoltoriLevel(item));
                }
            }


            return listaEnvoltoriLevel;
        }


    }


    public class EnvoltoriLevel
    {
        public bool IsSelecte { get; set; }
        public double ElevacionRedondeada { get; set; }
        public double ElevacionProjectadaRedondeada { get; set; }
        public double ElevacionProjectadaRedondeada_CM { get; set; }
        public Level _Level { get; set; }
        public string NombreLevel { get; set; }
        public EnvoltoriLevel(Level level)
        {
            int numeroDecima = 4;
            _Level = level;
            NombreLevel = level.Name;
            ElevacionRedondeada = Math.Round(level.Elevation, numeroDecima);
            ElevacionProjectadaRedondeada = Math.Round(level.ProjectElevation, numeroDecima);
            ElevacionProjectadaRedondeada_CM = Math.Round(Util.FootToCm(level.ProjectElevation), 0);
        }

        public bool ISok()
        {
            if (_Level == null) return false;
            if (NombreLevel == null) return false;
            return true;
        }
    }
}
