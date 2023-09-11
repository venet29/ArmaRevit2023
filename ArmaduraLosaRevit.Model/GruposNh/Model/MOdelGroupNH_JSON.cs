using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.GruposNh.Model
{
    public class MOdelGroupNH_JSON
    {

        public string Nombre { get; set; }
        public List<int> ListaGruopId { get; set; }
        public int IdGroup { get;  set; }
        public int GroupNH { get; set; }  //Group
        public int GroupTipo { get; set; }
    }
}
