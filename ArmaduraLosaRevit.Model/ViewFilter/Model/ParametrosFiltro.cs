using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArmaduraLosaRevit.Model.ViewFilter.Model.ParametrosFiltro;

namespace ArmaduraLosaRevit.Model.ViewFilter.Model
{
    public enum TipoParametros
    {
        numero,
        texto
    }
    public class ParametrosFiltro
    {
        public bool IsVisibilidad { get; set; } = false;
        public string Nombrefilfro { get; set; }
        public TipoParametros TipoPArametroso { get; set; } = TipoParametros.texto;
        public string Valorfilfro { get; set; }
        public Func<ElementId, string, bool, FilterRule> reglafiltroSTR { get; internal set; }
        public Func<ElementId, double, double, FilterRule> reglafiltroDOUBLE { get; internal set; }

        internal ParametrosFiltro CambiarVisibili(bool isVisibilidad)
        {
            IsVisibilidad = isVisibilidad;
            return this;
        }
    }

    public class FactoryParametrosFiltro
    {
        public static ParametrosFiltro NONE => new ParametrosFiltro() { Nombrefilfro = "NONE", Valorfilfro = "NONE" };
        public static ParametrosFiltro FiltroBarraTipo => new ParametrosFiltro() { Nombrefilfro = "BarraTipo", Valorfilfro = "", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule };
        public static ParametrosFiltro FiltroSinView => new ParametrosFiltro() { Nombrefilfro = "NombreVista", Valorfilfro = "", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule };

        public static ParametrosFiltro LargoMAyor12Mt(string largo)
        {

            if (!(Util.IsNumeric(largo)))
            {
                Util.ErrorMsg($"Largo ingresado:'{largo}' no es valor numero. Se utiliza largo 1200cm ");
                largo = "1200";
            }

            return new ParametrosFiltro() { Nombrefilfro = "LargoRevision", Valorfilfro = largo, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateLessOrEqualRule, TipoPArametroso = TipoParametros.numero };
        }

        internal static ParametrosFiltro ObtenerFiltroView(string _tipo) => new ParametrosFiltro() { Nombrefilfro = "NombreVista", Valorfilfro = _tipo, reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule };
        internal static ParametrosFiltro ObtenerBarraTipo(string _tipo) => new ParametrosFiltro() { Nombrefilfro = "BarraTipo", Valorfilfro = _tipo, reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule };


        #region ParaFiltrarPorDiametro  dejar dimatros marcados


        public static ParametrosFiltro FiltroBarraDim8 => new ParametrosFiltro() { Nombrefilfro = "Diam8", Valorfilfro = "8.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim10 => new ParametrosFiltro() { Nombrefilfro = "Diam10", Valorfilfro = "10.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = false };
        public static ParametrosFiltro FiltroBarraDim12 => new ParametrosFiltro() { Nombrefilfro = "Diam12", Valorfilfro = "12.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim16 => new ParametrosFiltro() { Nombrefilfro = "Diam16", Valorfilfro = "16.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim18 => new ParametrosFiltro() { Nombrefilfro = "Diam18", Valorfilfro = "18.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim22 => new ParametrosFiltro() { Nombrefilfro = "Diam22", Valorfilfro = "22.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim25 => new ParametrosFiltro() { Nombrefilfro = "Diam25", Valorfilfro = "25.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim28 => new ParametrosFiltro() { Nombrefilfro = "Diam28", Valorfilfro = "28.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim32 => new ParametrosFiltro() { Nombrefilfro = "Diam32", Valorfilfro = "32.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim36 => new ParametrosFiltro() { Nombrefilfro = "Diam36", Valorfilfro = "36.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateNotEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateNotEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };

        public static List<ParametrosFiltro> ListaFiltroBarra = new List<ParametrosFiltro>() { FiltroBarraDim8, FiltroBarraDim10, FiltroBarraDim12, FiltroBarraDim16, FiltroBarraDim18, FiltroBarraDim22, FiltroBarraDim25, FiltroBarraDim28, FiltroBarraDim32, FiltroBarraDim36 };
        #endregion


        #region ParaFiltrarDiamtroCOlor


        public static ParametrosFiltro FiltroBarraDim8_color => new ParametrosFiltro() { Nombrefilfro = "Diam8", Valorfilfro = "8.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim10_color => new ParametrosFiltro() { Nombrefilfro = "Diam10", Valorfilfro = "10.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = false };
        public static ParametrosFiltro FiltroBarraDim12_color => new ParametrosFiltro() { Nombrefilfro = "Diam12", Valorfilfro = "12.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim16_color => new ParametrosFiltro() { Nombrefilfro = "Diam16", Valorfilfro = "16.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim18_color => new ParametrosFiltro() { Nombrefilfro = "Diam18", Valorfilfro = "18.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim22_color => new ParametrosFiltro() { Nombrefilfro = "Diam22", Valorfilfro = "22.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim25_color => new ParametrosFiltro() { Nombrefilfro = "Diam25", Valorfilfro = "25.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim28_color => new ParametrosFiltro() { Nombrefilfro = "Diam28", Valorfilfro = "28.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim32_color => new ParametrosFiltro() { Nombrefilfro = "Diam32", Valorfilfro = "32.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };
        public static ParametrosFiltro FiltroBarraDim36_color => new ParametrosFiltro() { Nombrefilfro = "Diam36", Valorfilfro = "36.0 mm", reglafiltroSTR = ParameterFilterRuleFactory.CreateEqualsRule, reglafiltroDOUBLE = ParameterFilterRuleFactory.CreateEqualsRule, TipoPArametroso = TipoParametros.numero, IsVisibilidad = true };

        public static List<ParametrosFiltro> ListaFiltroBarra_color = new List<ParametrosFiltro>() { FiltroBarraDim8_color, FiltroBarraDim10_color, FiltroBarraDim12_color, FiltroBarraDim16_color, FiltroBarraDim18_color, FiltroBarraDim22_color, FiltroBarraDim25_color, FiltroBarraDim28_color, FiltroBarraDim32_color, FiltroBarraDim36_color };
        #endregion
    };
}

