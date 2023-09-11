using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cubicacion.model
{
    public class LevelDTO
    {
        public bool IsSelected { get; set; }

        public double Elevacion { get; set; }
        public string Nombre_cub { get; set; }
        public string Nombre_cub_orig { get; set; }
        public Level Level_ { get; set; }

        public string Nombre { get; set; }
        public LevelDTO(Level c)
        {
            this.Level_ = c;
            this.Nombre = c.Name.Trim();
            this.Nombre_cub = RectificarNivel(c.Name);
            this.Nombre_cub_orig = RectificarNivel(c.Name);
            this.Elevacion = c.ProjectElevation;
            this.IsSelected = true;

        }

        private string RectificarNivel(string name)
        {
            string rec = name.Replace("NIVEL", "").Replace("PISO", "").Replace("°", "").Trim();

            if (rec == "-5")
                rec = "S5";
            else if(rec == "-4")
                rec = "S4";
            else if (rec == "-3")
                rec = "S3";
            else if (rec == "-2")
                rec = "S2";
            else if (rec == "-1")
                rec = "S1";

            return rec;
        }
    }
}
