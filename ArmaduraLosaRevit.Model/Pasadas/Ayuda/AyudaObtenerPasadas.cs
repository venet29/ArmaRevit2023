using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Pasadas.Ayuda
{
    public class AyudaObtenerPasadas
    {
        public static IEnumerable<ElementId> listaPasdas_Azul { get; internal set; }
        public static IEnumerable<ElementId> listaPasdas_Naranjo { get; internal set; }
        public static IEnumerable<ElementId> listaPasdas_Verde { get; internal set; }

        public static IEnumerable<ElementId> listaPasdas_Gris { get; internal set; }
        public static IEnumerable<ElementId> listaPasdas_Rojo { get; internal set; }




        public static List<Element> ListaPasdasElem_Azul { get; internal set; }
        public static List<Element> ListaPasdasElem_Naranjo { get; internal set; }
        public static List<Element> ListaPasdasElem_Verde { get; internal set; }
        public static List<Element> ListaPasdasElem_Gris { get; internal set; }
        public static List<Element> ListaPasdasElem_Rojo { get; internal set; }
        public static List<EnvoltorioPasadas> ListaPAsadas { get; private set; }

        public static bool ObtenerPasadas(Document _doc, CreadorExtStoreComplejo _CreadorExtStore)
        {
            try
            {
                ListaPasdasElem_Azul = SeleccionarOpening.SeleccionarAll_pasadas_ConNombre(_doc, ColorTipoPasada.PASADA_AZUL.ToString())
                                        .Where(c => _CreadorExtStore.M3_OBtenerResultado_String(c, "estado", "SubFieldTest21") != "").ToList();
                listaPasdas_Azul = ListaPasdasElem_Azul.Select(c => c.Id).ToList();

                ListaPasdasElem_Rojo = SeleccionarOpening.SeleccionarAll_pasadas_ConNombre(_doc, ColorTipoPasada.PASADA_ROJA.ToString())
                               .Where(c => _CreadorExtStore.M3_OBtenerResultado_String(c, "estado", "SubFieldTest21") != "").ToList();

                listaPasdas_Rojo = ListaPasdasElem_Rojo.Select(c => c.Id).ToList();


                ListaPasdasElem_Naranjo = SeleccionarOpening.SeleccionarAll_pasadas_ConNombre(_doc, ColorTipoPasada.PASADA_NARANJO.ToString())
                   .Where(c => _CreadorExtStore.M3_OBtenerResultado_String(c, "estado", "SubFieldTest21") != "").ToList();
                listaPasdas_Naranjo = ListaPasdasElem_Naranjo.Select(c => c.Id).ToList();

                ListaPasdasElem_Verde= SeleccionarOpening.SeleccionarAll_pasadas_ConNombre(_doc, ColorTipoPasada.PASADA_VERDE.ToString())
                           .Where(c => _CreadorExtStore.M3_OBtenerResultado_String(c, "estado", "SubFieldTest21") != "").ToList();
                listaPasdas_Verde = ListaPasdasElem_Verde.Select(c => c.Id).ToList();



                ListaPasdasElem_Gris = SeleccionarOpening.SeleccionarAll_pasadas_ConNombre(_doc, ColorTipoPasada.PASADA_GRIS.ToString())
                           .Where(c => _CreadorExtStore.M3_OBtenerResultado_String(c, "estado", "SubFieldTest21") != "").ToList();
                listaPasdas_Gris = ListaPasdasElem_Gris.Select(c => c.Id).ToList();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener pasadas del 3D.  ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public static List<EnvoltorioPasadas> ObtenerListaPAsadas(UIApplication _uiapp)
        {
            ListaPAsadas = new List<EnvoltorioPasadas>();
            List<Element> ListaBorraElement = new List<Element>();
            try
            {           
                ListaBorraElement.AddRange(ListaPasdasElem_Azul);
                ListaBorraElement.AddRange(ListaPasdasElem_Naranjo);
                ListaBorraElement.AddRange(ListaPasdasElem_Rojo);
                ListaBorraElement.AddRange(ListaPasdasElem_Verde);
                ListaBorraElement.AddRange(ListaPasdasElem_Gris);

                DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);

                foreach (Element item in ListaBorraElement)
                {
                    var _newEnvoltorioPasadas = new EnvoltorioPasadas(item);
                    if(_newEnvoltorioPasadas.OBtenerPArametro( _uiapp,_CreadorExtStore))
                        ListaPAsadas.Add(_newEnvoltorioPasadas);
                }

                return ListaPAsadas;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<EnvoltorioPasadas>();
            }
        }
    }
}
