using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.TagF
{
    public class TagF_NULL : ATagF_Base,ITagF
    {
        public TagF_NULL():base(null,null)
        {
            numeroBarrasPrimaria = 0;
             IsBarraPrimaria = true;
            IsBarraSecuandaria = false;
        }    
        public bool Ejecutar()
        {
            numeroBarrasPrimaria = 0;
            return true;
        }
    }
}
