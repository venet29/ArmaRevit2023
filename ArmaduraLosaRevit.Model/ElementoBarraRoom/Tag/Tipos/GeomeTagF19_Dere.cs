using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF19_Dere : GeomeTagBaseAhorro,  IGeometriaTag
    {

        public GeomeTagF19_Dere(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
            : base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        {


        }



        public override void M3_DefinirRebarShape() {

            GeomeTagF3 geomeTagF3 = new GeomeTagF3();   
            _geomeTagF4_Superior.M5_DefinirRebarShapeAhorro(geomeTagF3.AsignarPArametros);


            GeomeTagF1 geomeTagF1 = new GeomeTagF1();
            _geomeTagF4_Inferior.M5_DefinirRebarShapeAhorro(geomeTagF1.AsignarPArametros);

            //unir los datos
            listaTag.AddRange(_geomeTagF4_Superior.listaTag);
            listaTag.AddRange(_geomeTagF4_Inferior.listaTag);
        }

   

    }
}
