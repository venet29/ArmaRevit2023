
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using ArmaduraLosaRevit.Model.Visibilidad.Ayuda;
using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    public class VisibilidadElementDireccion : VisibilidadElementBase
    {
        //protected readonly Document _doc;

        public VisibilidadElementDireccion(UIApplication _uiapp) : base(_uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
        }
        public VisibilidadElementDireccion(Document doc) : base(doc)
        {
            this._doc = doc;
        }
        public override void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view)
        {
            M1_CAmbiarColorPorDIreccion(lista_A_VisibilidadElementoPathDTO[0], view);
        }
        private void M1_CAmbiarColorPorDIreccion(List<ElementoPath> lista_A_VisibilidadElementoPathDTO, View view)
        {
            try
            {
                if (lista_A_VisibilidadElementoPathDTO == null) return;
                if (lista_A_VisibilidadElementoPathDTO.Count == 0) return;

                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("ChangeElement Direccion-NHr");

                    //List<ElementId>
                    List<ElementId> listaPathHorizontales = lista_A_VisibilidadElementoPathDTO.Where(ve => ve.TipoDireccionBarra_ == TipoDireccionBarra.Primaria)
                                                                                            .SelectMany(cc => ayudaObtenerElement.ObtenerID_segunTipo(cc)).Select(el => el.Id)
                                                                                            .ToList();
                    ChangeListElementsColorSinTrans(listaPathHorizontales, FactoryColores.ObtenerColoresPorNombre(TipoCOlores.rojo));



                    List<ElementId> listaPathVertical = lista_A_VisibilidadElementoPathDTO.Where(ve => ve.TipoDireccionBarra_ == TipoDireccionBarra.Secundario)
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
