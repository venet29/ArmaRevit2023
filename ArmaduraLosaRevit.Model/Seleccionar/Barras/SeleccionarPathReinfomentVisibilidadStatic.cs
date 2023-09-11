using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar.Barras
{
    // clase para almacenar  las listas de los pathreinforment de la clase 'SeleccionarPathReinfomentVisibilidad.cs'
    public class SeleccionarPathReinfomentVisibilidadStatic
    {
        public static List<ElementoPathRein> _lista_A_VisibilidadElementoPathReinDTO { get; set; } = new List<ElementoPathRein>();

        // public List<Element> _lista_B_DePathSymbolNivelActual { get; set; }
        public static List<WrapperPathSymbol> _lista_B_DePathSymbolNivelActual { get; set; }= new List<WrapperPathSymbol>();
        public static List<WrapperTagPath> _lista_C_DePathTagNivelActual { get; set; } = new List<WrapperTagPath>();    


        public static ElementoPathRein Obtener_ElementoPathRein_conPathSymbol(UIApplication _uiapp, View _view, Element _element)
        {
            ElementoPathRein ElementoPathReinEncontrado = null;
            if (_lista_A_VisibilidadElementoPathReinDTO != null)
            {
                SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad( _uiapp,  _view);
                _SelecPathReinVisibilidad.M1_ejecutar();

                ElementoPathReinEncontrado = _lista_A_VisibilidadElementoPathReinDTO.Where(c => c.pathSymbol.Id.IntegerValue == _element.Id.IntegerValue)
                                                                                   .FirstOrDefault();
            }
            else
            {
                ElementoPathReinEncontrado = _lista_A_VisibilidadElementoPathReinDTO.Where(c => c.pathSymbol.Id.IntegerValue == _element.Id.IntegerValue)
                                                                                   .FirstOrDefault();
                if (ElementoPathReinEncontrado == null)
                {
                    SeleccionarPathReinfomentVisibilidad _SelecPathReinVisibilidad = new SeleccionarPathReinfomentVisibilidad(_uiapp, _view);
                    _SelecPathReinVisibilidad.M1_ejecutar();

                    ElementoPathReinEncontrado = _lista_A_VisibilidadElementoPathReinDTO.Where(c => c.pathSymbol.Id.IntegerValue == _element.Id.IntegerValue)
                                                                            .FirstOrDefault();
                }
            }
            return ElementoPathReinEncontrado;
        }
    }
}
