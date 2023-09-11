using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Geometry;
using planta_aux_C.enumera;
using Autodesk.AutoCAD.DatabaseServices;

namespace planta_aux_C.Elemento_Losa
{
    public class NH_RefereciaLosaParaSuple
    {

        public string nombreLosa { get; set; }
        public float espesor { get; set; }
        public double anguloPelotaLosa { get; set; }
        public Point3d PosicionPelotaLosa { get; set; }

        public Point3d PosicionPto_suple { get; set; }
        public UbicacionLosa tipo { get; set; }

        public ObjectId PoligonoLosa { get; set; }
        public ObjectId PelotaLosa { get; set; }

        public NH_RefereciaLosaParaSuple(string nombreLosa, float espesor, double anguloPelotaLosa, Point3d PosicionPelotaLosa,
                                         Point3d PosicionPto_suple, UbicacionLosa tipo, ObjectId PelotaLosa, ObjectId PoligonoLosa)
        {
            this.nombreLosa = nombreLosa;
            this.espesor = espesor;
            this.anguloPelotaLosa = anguloPelotaLosa;
            this.PosicionPelotaLosa = PosicionPelotaLosa;

            this.PosicionPto_suple = PosicionPto_suple;
            this.tipo = tipo;
            this.PelotaLosa = PelotaLosa;
            this.PoligonoLosa = PoligonoLosa;
        }
    }
}
