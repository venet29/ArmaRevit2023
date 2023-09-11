using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Geometry;
using planta_aux_C.enumera;
using Autodesk.AutoCAD.DatabaseServices;


namespace planta_aux_C.Elemento_Losa
{
   public class NH_RefereciaLosaParaBarra
    {
       private string item;

        public string nombreLosa { get; set; }
        public float espesor { get; set; }
        public double anguloPelotaLosa { get; set; }
        public Point3d PosicionPelotaLosa { get; set; }

        public Point3d PosicionPto_Barra { get; set; }
        public UbicacionLosa tipo { get; set; }

        public ObjectId PoligonoLosa { get; set; }
        public ObjectId PelotaLosa { get; set; }

        public NH_RefereciaLosaParaBarra(string nombreLosa, float espesor, double anguloPelotaLosa, Point3d PosicionPelotaLosa,
                                         Point3d PosicionPto_Barra, UbicacionLosa tipo, ObjectId PelotaLosa, ObjectId PoligonoLosa)
        {
            this.nombreLosa = nombreLosa;
            this.espesor = espesor;
            this.anguloPelotaLosa = anguloPelotaLosa;
            this.PosicionPelotaLosa = PosicionPelotaLosa;

            this.PosicionPto_Barra = PosicionPto_Barra;
            this.tipo = tipo;
            this.PelotaLosa = PelotaLosa;
            this.PoligonoLosa = PoligonoLosa;
        }

        public NH_RefereciaLosaParaBarra(string item)
        {
            // TODO: Complete member initialization
            var datos = item.Split(new char[] { ',', ':' });

            this.nombreLosa = datos[0];
            this.espesor = Convert.ToInt32(datos[1]);
            this.anguloPelotaLosa = Convert.ToSingle(datos[2]);

            this.PosicionPelotaLosa = new Point3d(Convert.ToDouble(datos[3]), Convert.ToDouble(datos[4]), 0);
            this.PosicionPto_Barra = new Point3d(Convert.ToDouble(datos[5]), Convert.ToDouble(datos[6]), 0);

            //this.PelotaLosa =  datos[8];
            //this.PoligonoLosa =  datos[9];
          

            if (datos[7] == "Derecha")
            { this.tipo = UbicacionLosa.Derecha; }
            else if (datos[7] == "Izquierda")
            { this.tipo = UbicacionLosa.Izquierda; }
            else if (datos[7] == "Superior")
            { this.tipo = UbicacionLosa.Superior; }
            else if (datos[7] == "Inferior")
            { this.tipo = UbicacionLosa.Inferior; }
        }
    }
}
