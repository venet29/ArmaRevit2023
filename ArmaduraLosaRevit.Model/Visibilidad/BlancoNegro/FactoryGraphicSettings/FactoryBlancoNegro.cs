using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.Ayuda;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.FactoryGraphicSettings
{

    public class FactoryBlancoNegro
    {
        static List<GraphicSettingDTO> ListaFactoryBlancoNegro;

        static List<GraphicSettingDTO> ListaFactoryBlancoNegro_Original;

        public static ViewDetailLevel detailLevel { get; private set; }

        public static void CArgar_blancoNegro(Document _doc)
        {
            Color _colorNegro = new Color(0, 0, 0);
            ListaFactoryBlancoNegro = new List<GraphicSettingDTO>();



            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Rebar", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø8", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø10", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø12", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø16", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø18", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø22", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø25", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø28", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø30", _colorNegro, null, null, _colorNegro, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Ø32", _colorNegro, null, null, _colorNegro, null, null));


            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Rebar Tags", _colorNegro, null, null, null, null, null)); //seleccionado todo
        
            //0
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Floors", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));

            //1
            ColorPatternDTO _ColorPatternDTO_2 = new ColorPatternDTO(TiposPatternType.ObtenerTipoPattern("0.7_EST_ESPESORES", _doc));
            ColorPatternDTO _ColorPatternDTO_3 = new ColorPatternDTO(_colorNegro);
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Walls") { CutLineColor = _colorNegro });

            //2
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Wall Tags", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Generic Annotations", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Spot Elevations", _colorNegro, null, null, null, null, null));


            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Framing", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));            
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Framing Tags", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Path Reinforcement Tags", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Path Reinforcement", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Path Reinforcement Symbols", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Rebar", _colorNegro, null, null, null, null, null)); //subclase Structural Path Reinforcement Symbols
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Barra", _colorNegro, null, null, null, null, null));//Structural Path Reinforcement Symbols

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("<Hidden Lines>", _colorNegro, null, null, null, null, null));//Structural Path Reinforcement Symbols


            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Room Tags", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Dimensiones", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Text Notes", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Generic Annotations", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Dimensions", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Grids", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Detail Groups", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro.Add(new GraphicSettingDTO("Structural Foundations", _colorNegro, null, null, _colorNegro, null, null));
        
        }

        public static void CArgar_Normal_original(Document _doc)
        {
            Color _colorNegro = new Color(0, 0, 0);
            ListaFactoryBlancoNegro_Original = new List<GraphicSettingDTO>();

            Color _rojo = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.rojo);
            Color _magenta = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta);
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Rebar", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø8", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø10", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø12", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø16", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø18", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø22", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø25", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø28", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø30", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø32", _magenta, null, null, _magenta, null, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Foundations", _rojo, null, null, _rojo, null, null));
            return;
#pragma warning disable CS0162 // Unreachable code detected
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Rebar Tags", _colorNegro, null, null, null, null, null)); //seleccionado todo
#pragma warning restore CS0162 // Unreachable code detected

            //0
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Floors", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));

            //1
            ColorPatternDTO _ColorPatternDTO_2 = new ColorPatternDTO(TiposPatternType.ObtenerTipoPattern("0.7_EST_ESPESORES", _doc));
            ColorPatternDTO _ColorPatternDTO_3 = new ColorPatternDTO(_colorNegro);
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Walls") { CutLineColor = _colorNegro });

            //2
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Wall Tags", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Generic Annotations", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Spot Elevations", _colorNegro, null, null, null, null, null));


            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Framing", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Framing", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Framing Tags", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Path Reinforcement Tags", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Path Reinforcement", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Path Reinforcement Symbols", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Dimensiones", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Dimensions", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Grid", _colorNegro, null, null, null, null, null));
        }

        public static void CArgar_Normal(Document _doc)
        {
            Color _colorNegro = new Color(0, 0, 0);
            ListaFactoryBlancoNegro_Original = new List<GraphicSettingDTO>();

            Color _rojo = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.rojo);
            Color _magenta = FactoryColores.ObtenerColoresPorNombre(TipoCOlores.magenta);
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Rebar", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø8", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø10", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø12", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø16", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø18", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø22", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø25", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø28", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø30", _magenta, null, null, _magenta, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Ø32", _magenta, null, null, _magenta, null, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Foundations", _rojo, null, null, _rojo, null, null));
            return;
#pragma warning disable CS0162 // Unreachable code detected
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Rebar Tags", _colorNegro, null, null, null, null, null)); //seleccionado todo
#pragma warning restore CS0162 // Unreachable code detected

            //0
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Floors", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));

            //1
            ColorPatternDTO _ColorPatternDTO_2 = new ColorPatternDTO(TiposPatternType.ObtenerTipoPattern("0.7_EST_ESPESORES", _doc));
            ColorPatternDTO _ColorPatternDTO_3 = new ColorPatternDTO(_colorNegro);
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Walls") { CutLineColor = _colorNegro });

            //2
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Wall Tags", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Generic Annotations", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Spot Elevations", _colorNegro, null, null, null, null, null));


            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Framing", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Framing", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Framing Tags", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Path Reinforcement Tags", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Path Reinforcement", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Structural Path Reinforcement Symbols", _colorNegro, _colorNegro, null, _colorNegro, _colorNegro, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Dimensiones", _colorNegro, null, null, null, null, null));

            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Dimensions", _colorNegro, null, null, null, null, null));
            ListaFactoryBlancoNegro_Original.Add(new GraphicSettingDTO("Grid", _colorNegro, null, null, null, null, null));
        }

        public static OverrideGraphicSettings ObtenerOverrideGraphicSettings_BlancoNegro(string _catname, Document _doc)
        {
            if (ListaFactoryBlancoNegro == null) CArgar_blancoNegro(_doc);
            if (ListaFactoryBlancoNegro.Count == 0) CArgar_blancoNegro(_doc);


            //OverrideGraphicSettings ogs = new OverrideGraphicSettings();

            var encontrado = ListaFactoryBlancoNegro.Where(c => c.Nombre == _catname).FirstOrDefault();
            if (encontrado == null) return null;

            return Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(encontrado);

        }

        public static OverrideGraphicSettings ObtenerOverrideGraphicSettings_Normal(string _catname, Document _doc)
        {
            if (ListaFactoryBlancoNegro_Original == null) CArgar_Normal(_doc);
            if (ListaFactoryBlancoNegro_Original.Count == 0) CArgar_Normal(_doc);


           // OverrideGraphicSettings ogs = new OverrideGraphicSettings();

            var encontrado = ListaFactoryBlancoNegro_Original.Where(c => c.Nombre == _catname).FirstOrDefault();
            if (encontrado == null) return null;


            return Creador_OverrideGraphicSettings.ObtenerOverrideGraphicSettings(encontrado);

        }
      
    };

}








