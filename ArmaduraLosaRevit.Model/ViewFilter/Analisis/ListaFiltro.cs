using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ViewFilter.Analisis
{
    public class ListaFiltro
    {
        internal static List<string> ListaFiltroBorrarRevision_sinLArgo() => new List<string>() { "MostrarBarras_BarraTipo-NH", "MostrarBarras_NombreVista-NH" };
        internal static List<string> ListaFiltroBorrarLargoRevision() => new List<string>() {  "MostrarBarras_LargoRevision-NH" };

        internal static List<string> ListaFiltroBorrarDiamAll() {
            var lsitacolor=ListaFiltroBorrarDiamColor();
            lsitacolor.Add(ListaFiltroBorrarDiam()[0]);
            return lsitacolor;
        }
        internal static List<string> ListaFiltroBorrarDiam() => new List<string>() { "MostrarBarrasDiam-NH" };
        internal static List<string> ListaFiltroBorrarDiamColor() => new List<string>() { "MostrarBarrasDiam8-NH", "MostrarBarrasDiam10-NH", "MostrarBarrasDiam12-NH", "MostrarBarrasDiam16-NH", "MostrarBarrasDiam18-NH", "MostrarBarrasDiam22-NH", "MostrarBarrasDiam25-NH", "MostrarBarrasDiam28-NH", "MostrarBarrasDiam32-NH", "MostrarBarrasDiam36-NH" };
        internal static List<string>  ListaNombreIgualExcluir() => new List<string>() { "" };

        internal static List<string> ListaVacio() => new List<string>() { "" };

        internal static List<string> ListaFiltroBorrarTipo() => new List<string>{ "MostrarBarras_PorTipo-NH"};
        internal static List<string> ListaFiltroBorrarView() => new List<string> { "MostrarBarras_PorView-NH" };
    }
}
