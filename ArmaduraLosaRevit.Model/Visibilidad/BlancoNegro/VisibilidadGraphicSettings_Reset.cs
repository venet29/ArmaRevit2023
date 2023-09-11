using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Viewnh;
using ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Visibilidad.BlancoNegro
{
    public class VisibilidadGraphicSettings_Reset : VisibilidadGraphicSettings_View_BlancoNegro
    {

        public VisibilidadGraphicSettings_Reset(UIApplication application,View _view) :base(application, _view)
        {

        }

        public bool nombreFuncion()
        {
            try
            {
                M0_ObtenerListaElementosVisibles(CategoryType.Model);
                if (ListElementoVisibles.Count == 0) return false;

                int i = 0;
                string namecageroria = "";
                using (Transaction t = new Transaction(_view.Document))
                {
                    t.Start($"CambioBancoInegro3-NH");
                    for (i = 0; i < ListElementoVisibles.Count; i++)
                    {
                        var item = ListElementoVisibles.ElementAt(i);
                        //var _nombre = item.nombre;
                        Category _categoria = item.Category_;
                        //namecageroria = _categoria.Name;
                        // categoria princiapal
                       // M1_1_cambiarColorCategory(estado, _categoria);

                        //sub categoria
                        foreach (CategoriasDTO subItem in item.SubCategoria)
                        {
                          //  M1_1_cambiarColorCategory(estado, subItem.Category_);
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'function'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

    }
}
