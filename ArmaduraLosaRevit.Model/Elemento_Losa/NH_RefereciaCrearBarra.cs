
using System;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using ArmaduraLosaRevit;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.Elemento_Losa
{
    public class NH_RefereciaCrearBarra: INH_Referecia
    {


        public string nombreLosa { get; set; }
        public float espesor { get; set; }
        public double anguloPelotaLosa { get; set; }


        public XYZ PosicionPelotaLosa { get; set; }

        public XYZ PosicionPto_Barra { get; set; }
        public XYZ PosicionPto_Suples { get; set; }
        public UbicacionLosa UbicacionEnLosa { get; set; }
        public string tipoBarra { get; internal set; }
        /// <summary>
        /// largo del recorrido de la barra
        /// esto es opcional, pq se utilza solo cuando se genera enfierra barra inferior automatio
        /// F:\_revit\REVITArmaduraLosaRevit\ArmaduraLosaRevit\ArmaduraLosaRevit.Model\observaciones.docx
        /// Observacion 1)
        /// </summary>
        public double LargoRecorridoX { get; set; }
        public double LargoRecorridoY { get; set; }

        public Element PoligonoLosa { get; set; } // REPRESENTA AL ROOM
        public Element PelotaLosa { get; set; }  // REPRESNETA AL FLOOR


        public double diametro { get; set; }
        public double espaciamiento { get; set; }
        public string cuantia { get; set; }

        #region 1) contructores


        public NH_RefereciaCrearBarra()
        { }
        public NH_RefereciaCrearBarra(string nombreLosa, float espesor, double anguloPelotaLosa, XYZ PosicionPelotaLosa,
                                         XYZ PosicionPto_Barra, UbicacionLosa tipo, Element PelotaLosa, Element PoligonoLosa)
        {
            //this.tipoBarra = "barraIngerior";
            this.nombreLosa = (nombreLosa!=null ? nombreLosa:"");
            this.espesor = espesor;
            this.anguloPelotaLosa = anguloPelotaLosa;
            this.PosicionPelotaLosa = PosicionPelotaLosa;

            this.PosicionPto_Barra = PosicionPto_Barra;
            this.UbicacionEnLosa = tipo;
            this.PelotaLosa = PelotaLosa;
            this.PoligonoLosa = PoligonoLosa;
            this.diametro = 8;
            this.espaciamiento = 20;
            this.cuantia = "@" + diametro + "a" + espaciamiento + "";

        }

        public NH_RefereciaCrearBarra(string item, double CoordZ_pelotalosa)
        {
            // TODO: Complete member initialization
            this.PoligonoLosa = PoligonoLosa;
            var datos = item.Split(new char[] { ',', ':' });

            this.nombreLosa = datos[0];
            this.espesor = Convert.ToSingle(datos[1]);
            this.anguloPelotaLosa = Convert.ToSingle(datos[2]);

            this.PosicionPelotaLosa = new XYZ(Util.CmToFoot(Convert.ToDouble(datos[3])), Util.CmToFoot(Convert.ToDouble(datos[4])), Util.CmToFoot(Convert.ToDouble(datos[5])));
            this.PosicionPto_Barra = new XYZ(Util.CmToFoot(Convert.ToDouble(datos[6])), Util.CmToFoot(Convert.ToDouble(datos[7])), Util.CmToFoot(Convert.ToDouble(datos[8])));

            //this.PelotaLosa =  datos[8];
            //this.PoligonoLosa =  datos[9];
            this.LargoRecorridoX = Util.CmToFoot(Convert.ToDouble(datos[12]));
            this.LargoRecorridoY = Util.CmToFoot(Convert.ToDouble(datos[13]));



            this.diametro = 8;
            this.espaciamiento = 20;
            this.cuantia = "@" + diametro + "a" + espaciamiento + "";

            if (datos[9] == "Derecha")
            { this.UbicacionEnLosa = UbicacionLosa.Derecha; }
            else if (datos[9] == "Izquierda")
            { this.UbicacionEnLosa = UbicacionLosa.Izquierda; }
            else if (datos[9] == "Superior")
            { this.UbicacionEnLosa = UbicacionLosa.Superior; }
            else if (datos[9] == "Inferior")
            { this.UbicacionEnLosa = UbicacionLosa.Inferior; }
        }

        #endregion
    }
}
