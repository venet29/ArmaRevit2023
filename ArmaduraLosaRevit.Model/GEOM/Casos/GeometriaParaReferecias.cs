using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GEOM.Casos
{

    public class GeometriaParaReferecias : GeometriaBase
    {    //se utiliza solo para caso obtener referencias de plnanar fase para dibujar dimensiones 
        //utiliza  instanciaanidada.GetSymbolGeometry();  en vez del clasico instanciaanidada.GetInstanceGeometry()

        public GeometriaParaReferecias(Document _doc) : base(_doc)
        {
            IsInstanceGeometry = false;
        }
    }
}
