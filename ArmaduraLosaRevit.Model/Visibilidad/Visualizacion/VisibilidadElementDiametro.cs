
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
    public class VisibilidadElementDiametro : VisibilidadElementBase
    {
#pragma warning disable CS0108 // 'VisibilidadElementDiametro._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.
        private readonly Document _doc;
#pragma warning restore CS0108 // 'VisibilidadElementDiametro._doc' hides inherited member 'VisibilidadElementBase._doc'. Use the new keyword if hiding was intended.

        public VisibilidadElementDiametro(UIApplication _uiapp) : base(_uiapp)
        {
            this._doc = _uiapp.ActiveUIDocument.Document;
        }
        public override void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view)
        {
            M1_CAmbiarColorPorDiametro(lista_A_VisibilidadElementoPathDTO[0], view);
        }

        public void M1_CAmbiarColorPorDiametro(List<ElementoPath> lista_A_VisibilidadElementoPathDTO, View view)
        {
            int[] diams = { 6, 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };
            foreach (var dia in diams)
            {
                List<ElementId> listaPathHorizontales = lista_A_VisibilidadElementoPathDTO.Where(ve => ve.DiametroBarra == dia)
                                                                        .SelectMany(cc => ayudaObtenerElement.ObtenerID_segunTipo(cc)).Select(el => el.Id)
                                                                        .ToList();
                ChangeListaElementColorConTrans(listaPathHorizontales, FactoryColores.ObtenerColoresPorDiametro(dia));
            }
        }


    }
}
