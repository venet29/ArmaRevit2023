
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
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
    public class VisibilidadElementRebarLosa: VisibilidadElementBase
    {
    
  
        public VisibilidadElementRebarLosa(UIApplication _uiapp) : base(_uiapp)
        {
        
  
        }

        public override void Ejecutar(List<List<ElementoPath>> lista_A_VisibilidadElementoPathDTO, View view)
        {
          
        }
    }
}
