using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad.Entidades
{
    public class FactoryColores
    {


        public static Color ObtenerColoresPorNombre(TipoCOlores nombreColor)
        {

            switch (nombreColor)
            {
                case TipoCOlores.magenta:
                    return new Color((byte)255, (byte)0, (byte)255);
                case TipoCOlores.negro:
                    return new Color((byte)0, (byte)0, (byte)0);
                case TipoCOlores.amarillo:
                    return new Color((byte)236, (byte)255, (byte)0);
                case TipoCOlores.azul:
                    return new Color((byte)51, (byte)54, (byte)255);
                case TipoCOlores.rojopuro:
                    return new Color((byte)255, (byte)0, (byte)0);
                case TipoCOlores.rojo:
                    return new Color((byte)205, (byte)0, (byte)0);
                case TipoCOlores.ParaMalla:
                    return  new Color((byte)102, (byte)102, (byte)102);
                case TipoCOlores.blanco:
                    return new Color((byte)255, (byte)255, (byte)255);
                case TipoCOlores.EstriboConf:
                    return new Color((byte)254, (byte)254, (byte)254);
                case TipoCOlores.EstriboMuro:
                    return new Color((byte)38, (byte)76, (byte)76);
                case TipoCOlores.EstriboViga:
                    return new Color((byte)38, (byte)76, (byte)76);
                case TipoCOlores.MallaMuro:
                    return new Color((byte)102, (byte)102, (byte)102);
                default:
                    return new Color((byte)255, (byte)0, (byte)255); //magenta
            }


        }


        public static Color ObtenerColorEstribo(TipoConfiguracionEstribo _tipoConfiguracionBarra)
        {
            switch (_tipoConfiguracionBarra)
            {
                case TipoConfiguracionEstribo.Estribo:
                case TipoConfiguracionEstribo.Estribo_Lateral:
                case TipoConfiguracionEstribo.Estribo_Traba:
                case TipoConfiguracionEstribo.Estribo_Lateral_Traba:
                    return ObtenerColoresPorNombre(TipoCOlores.EstriboConf);
                case TipoConfiguracionEstribo.Traba:
                    return ObtenerColoresPorNombre(TipoCOlores.blanco);
                case TipoConfiguracionEstribo.EstriboMuro:
                case TipoConfiguracionEstribo.EstriboMuro_Lateral:
                case TipoConfiguracionEstribo.EstriboMuro_Traba:
                case TipoConfiguracionEstribo.EstriboMuro_Lateral_Traba:
                    return ObtenerColoresPorNombre(TipoCOlores.EstriboMuro);
                case TipoConfiguracionEstribo.EstriboMuroTraba:
                    return ObtenerColoresPorNombre(TipoCOlores.blanco);
                case TipoConfiguracionEstribo.EstriboViga:
                case TipoConfiguracionEstribo.EstriboViga_Lateral:
                case TipoConfiguracionEstribo.EstriboViga_Traba:
                case TipoConfiguracionEstribo.EstriboViga_Lateral_Traba:
                    return ObtenerColoresPorNombre(TipoCOlores.EstriboViga);
                case TipoConfiguracionEstribo.VigaTraba:
                    return ObtenerColoresPorNombre(TipoCOlores.blanco);
                case TipoConfiguracionEstribo.ElevMallaV:
                case TipoConfiguracionEstribo.ElevMallaH:
                    return ObtenerColoresPorNombre(TipoCOlores.MallaMuro);
                case TipoConfiguracionEstribo.ELEV_BA_H:
                case TipoConfiguracionEstribo.ELEV_BA_V:
                    return ObtenerColoresPorNombre(TipoCOlores.magenta);
            }

            return new Color((byte)38, (byte)76, (byte)76); ;
        }


        public static Color ObtenerColoresPorDiametro(int diamtro)
        {

          
                switch (diamtro)
                {
                    case 6:
                        return new Color((byte)51, (byte)54, (byte)255);
                    case 8:
                        return new Color((byte)164, (byte)243, (byte)24);// verde lima
                    case 10:
                        return new Color((byte)128, (byte)64, (byte)0); //cafe
                    case 12:
                        return new Color((byte)0, (byte)143, (byte)57);// verde osucri
                    case 16:
                        return new Color((byte)81, (byte)209, (byte)246); //celeste               
                    case 18:
                        return new Color((byte)27, (byte)085, (byte)131); //azul
                    case 22:
                        return new Color((byte)255, (byte)128, (byte)0); //naranja
                    case 25:
                        return new Color((byte)255, (byte)0, (byte)0); //rojo
                    case 28:
                        return new Color((byte)207, (byte)52, (byte)118); //magenta
                    case 32:
                        return new Color((byte)87, (byte)35, (byte)100); //morado
                    case 36:
                        return new Color((byte)30, (byte)45, (byte)110); //axul osucuro
                    default:
                        return new Color((byte)0, (byte)255, (byte)255); //cian
                }        
        }
        public static System.Windows.Media.Color ObtenerColoresPorDiametro_wpf(int diamtro)
        {

       
            switch (diamtro)
            {
                case 6:
                    return System.Windows.Media.Color.FromRgb((byte)51, (byte)54, (byte)255);
                case 8:
                    return System.Windows.Media.Color.FromRgb((byte)164, (byte)243, (byte)24);// verde lima
                case 10:
                    return System.Windows.Media.Color.FromRgb((byte)128, (byte)64, (byte)0); //cafe
                case 12:
                    return System.Windows.Media.Color.FromRgb((byte)0, (byte)143, (byte)57);// verde osucri
                case 16:
                    return System.Windows.Media.Color.FromRgb((byte)81, (byte)209, (byte)246); //celeste               
                case 18:
                    return System.Windows.Media.Color.FromRgb((byte)27, (byte)085, (byte)131); //azul
                case 22:
                    return System.Windows.Media.Color.FromRgb((byte)255, (byte)128, (byte)0); //naranja
                case 25:
                    return System.Windows.Media.Color.FromRgb((byte)255, (byte)0, (byte)0); //rojo
                case 28:
                    return System.Windows.Media.Color.FromRgb((byte)207, (byte)52, (byte)118); //magenta
                case 32:
                    return System.Windows.Media.Color.FromRgb((byte)87, (byte)35, (byte)100); //morado
                case 36:
                    return System.Windows.Media.Color.FromRgb((byte)30, (byte)45, (byte)110); //axul osucuro
                default:
                    return System.Windows.Media.Color.FromRgb((byte)0, (byte)255, (byte)255); //cian
            }
        }
    }
}
