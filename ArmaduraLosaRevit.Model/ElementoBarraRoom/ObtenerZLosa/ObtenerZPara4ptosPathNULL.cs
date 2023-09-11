using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa
{
    public class ObtenerZPara4ptosPathNULL : IObtenerZPara4ptosPath
    {
        private List<XYZ> _listaPtos;
#pragma warning disable CS0169 // The field 'ObtenerZPara4ptosPathNULL._face' is never used
        private Face _face;
#pragma warning restore CS0169 // The field 'ObtenerZPara4ptosPathNULL._face' is never used

        public ObtenerZPara4ptosPathNULL()
        {
            this._listaPtos = new List<XYZ>();
         
        }

        public List<XYZ> M1_Obtener4PtoConZCorrespondiente()
        {
            return _listaPtos;
            
        }
    }
}
