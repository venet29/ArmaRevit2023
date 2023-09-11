using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF20_Dere : GeomeTagF19,  IGeometriaTag
    {


        public GeomeTagF20_Dere(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO)
            : base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        {


        }



        public override void M3_DefinirRebarShape() {

            GeomeTagF3 geomeTagF3 = new GeomeTagF3();
         
            _geomeTagF4_Superior.M5_DefinirRebarShapeAhorro(geomeTagF3.AsignarPArametros);


            //GeomeTagF3 geomeTagF3 = new GeomeTagF3();
   
            _geomeTagF4_Inferior.M5_DefinirRebarShapeAhorro(geomeTagF3.AsignarPArametros);

            //unir los datos
            listaTag.AddRange(_geomeTagF4_Superior.listaTag);
            listaTag.AddRange(_geomeTagF4_Inferior.listaTag);
        }

    

#pragma warning disable CS0108 // 'GeomeTagF20_Dere.M4_IsFAmiliaValida()' hides inherited member 'GeomeTagF19.M4_IsFAmiliaValida()'. Use the new keyword if hiding was intended.
        public bool M4_IsFAmiliaValida() => true;
#pragma warning restore CS0108 // 'GeomeTagF20_Dere.M4_IsFAmiliaValida()' hides inherited member 'GeomeTagF19.M4_IsFAmiliaValida()'. Use the new keyword if hiding was intended.

    }
}
