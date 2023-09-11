
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Ayuda;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class VisibilidadElementOrientacion  : VisibilidadElementBase
    {
     

        public VisibilidadElementOrientacion(UIApplication _uiapp) : base( _uiapp)
        {
  
        }
        public VisibilidadElementOrientacion(Document doc) : base(doc)
        {

        }
        public override void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view)
        {
            M1_CAmbiarColorPorOrientacion(lista_A_VisibilidadElementoPathDTO[0]);

            //M1_CAmbiarColorPorOrientacion_ParaRebar(lista_A_VisibilidadElementoPathDTO[0]);

        }

        //public void EjecutarRebar(List<ElementId> listaBarraRebarComoPAthLosa, View viewMantenerBArras)
        //{
        //    throw new NotImplementedException();
        //}
        private void M1_CAmbiarColorPorOrientacion(List<ElementoPath> lista_A_VisibilidadElementoPathDTO)
        {
         

            try
            {

                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("ChangeElement COlorOrientacion-NHr");

                    //List<ElementId>
                    List<ElementId> listaPathHorizontales = lista_A_VisibilidadElementoPathDTO.Where(ve => ve.orientacionBarra == UbicacionLosa.Derecha ||
                                                                                                           ve.orientacionBarra == UbicacionLosa.Izquierda)
                                                                                            .SelectMany(cc => ayudaObtenerElement.ObtenerID_segunTipo(cc)).Select(el => el.Id)
                                                                                            .ToList();
                    ChangeListElementsColorSinTrans(listaPathHorizontales, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.rojo));



                    List<ElementId> listaPathVertical = lista_A_VisibilidadElementoPathDTO.Where(ve => ve.orientacionBarra == UbicacionLosa.Superior ||
                                                                                               ve.orientacionBarra == UbicacionLosa.Inferior)
                                                                                .SelectMany(cc => ayudaObtenerElement.ObtenerID_segunTipo(cc)).Select(el => el.Id)
                                                                                .ToList();
                    ChangeListElementsColorSinTrans(listaPathVertical, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.azul));

                    tx.Commit();
                }

            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }

     

    }
}
