using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.GRIDS.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Ayuda

{
    internal class AyudaBuscarInterseccionPlanosPasadasConGrid
    {
        public static bool Ejecutar(UIApplication _uiapp, List<EnvoltoriPasada> lista_EnvoltoriPasada, List<EnvoltorioGrid> listaGridIntersectarPorVIew)
        {

            var listaGridInterse = listaGridIntersectarPorVIew.Select(c => new EnvoltorioElementoIntersectar(c)).ToList();

            listaGridInterse.ForEach(r => r.Obtener());

            try
            {
                for (int i = 0; i < lista_EnvoltoriPasada.Count; i++)
                {
                    for (int j = 0; j < lista_EnvoltoriPasada[i].ListaPLanosPAsadas.Count; j++)
                    {
                        var planarFace = lista_EnvoltoriPasada[i].ListaPLanosPAsadas[j];
                        // planarFace.M1_BuscarInterseccionesConElemento_ambosSentidos(listaElementoInterse);
                        planarFace.M1_BuscarInterseccionesConElemento_ambosSentidos(listaGridInterse);

                        //planarFace.M0_GenerarDIreccion_DerechaSuperior();
                        //planarFace.M2_BuscarMismoSentido(listaGridInterse);
                        //planarFace.M3_BUscarSentidoContrario(listaGridInterse);
                        //planarFace.M4_generarReferenciasSelecionadaMO();

                    }
                    // agrupar por normal


                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" ex:{ex.Message}");
                return false;
            }

            return true;
        }
    }
}
