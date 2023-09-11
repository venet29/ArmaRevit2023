using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Geometry;
using planta_aux_C.Elemento_Losa;
using planta_aux_C.enumera;

namespace planta_aux_C.Elementos
{
    public class PtosCrearSuples
    {
        private string item;
        private bool IsValid  { get; set; }

        public Point3d pto1 { get; set; }  // posicion desde donde se  comeinza a dibujar el traslapo --  seria el punto interno de la losas en que se dibuja , tb sirve para conocer que lado de la losa esta empotrado
        public Point3d pto2 { get; set; }

        public string nombre1 { get; set; }
        public string nombre2 { get; set; }

        public NH_RefereciaLosaParaSuple Plosa2 { get; set; }

        private NH_RefereciaLosaParaSuple plosa1;
        public NH_RefereciaLosaParaSuple Plosa1
        {
            get { return plosa1; }
            set { 
                plosa1 = value;
                this.tipo = plosa1.tipo;
                  }
        }
        public UbicacionLosa tipo { get; set; }

        public int diametro { get; set; }
        public int  espaciamiento { get; set; }
        public string cuantia { get; set; }

        #region Constructor
        public PtosCrearSuples()
        {
            IsValid = true;
        }

        public PtosCrearSuples(Point3d ptoRefe, Point3d ppr, string nombre1, string nombre2)
        {
            // TODO: Complete member initialization
            pto1 = ptoRefe;
            pto2 = ppr;
            this.nombre1 = nombre1;
            this.nombre2 = nombre2;
            diametro = 8;
            espaciamiento = 20;
            cuantia = "@8a20";

            IsValid = true;
        }

        public PtosCrearSuples(string item)
        {
            // TODO: Complete member initialization

            var datos = item.Split(new char[] { ',', ':' });

            this.nombre1 = datos[0];
            this.nombre2 = datos[1];

            this.pto1 = new Point3d(Convert.ToDouble(datos[2]), Convert.ToDouble(datos[3]),0);
            this.pto2 = new Point3d(Convert.ToDouble(datos[4]), Convert.ToDouble(datos[5]),0);

            diametro = Convert.ToInt32(datos[7]);
            espaciamiento = Convert.ToInt32(datos[8]);
            cuantia = datos[9];

            if (datos[6] == "Derecha") 
            { this.tipo = UbicacionLosa.Derecha; }
            else if (datos[6] == "Izquierda")
            { this.tipo = UbicacionLosa.Izquierda; }
            else if (datos[6] == "Superior")
            { this.tipo = UbicacionLosa.Superior; }
            else if (datos[6] == "Inferior")
            { this.tipo = UbicacionLosa.Inferior; }

            IsValid = true;
        }
    
        #endregion
    }
}
