
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
    public class NH_RefereciaCrearSuple : INH_Referecia
    {
        #region 0)propiedades  n:10
        public string nombreLosa1 { get; set; }
        public string nombreLosa2 { get; set; }
        public float espesor { get; set; }
        public double anguloPelotaLosa { get; set; }

        public XYZ PosicionPelotaLosa { get; set; }

        public XYZ PosicionPto_suple { get; set; }

        // public XYZ PosicionPto_inicia { get; set; }

        public string tipoBarra { get; internal set; }

        public UbicacionLosa UbicacionEnLosa { get; set; }


        public Element PoligonoLosa { get; set; }
        public Element PelotaLosa { get; set; }


        public XYZ PosicionPtoSupleFinal { get; set; }
        public XYZ PosicionPtoSupleInicial { get; set; }

        public Room roomVecino { get; set; }

        public double diametro { get; set; }
        public double espaciamiento { get; set; }
        public string cuantia { get; set; }
        private bool IsValid { get; set; }
        #endregion

        #region 1)Constructore   n:3
        public NH_RefereciaCrearSuple()
        {

        }
        public NH_RefereciaCrearSuple(string item)
        {
            // TODO: Complete member initialization

            var datos = item.Split(new char[] { ',', ':' });
            this.tipoBarra = "s1";
            this.nombreLosa1 = datos[0];
            this.nombreLosa2 = datos[1];

            this.PosicionPtoSupleInicial = new XYZ(Util.CmToFoot(Convert.ToDouble(datos[2])), Util.CmToFoot(Convert.ToDouble(datos[3])), Util.CmToFoot(Convert.ToDouble(datos[4])));
            this.PosicionPtoSupleFinal = new XYZ(Util.CmToFoot(Convert.ToDouble(datos[5])), Util.CmToFoot(Convert.ToDouble(datos[6])), Util.CmToFoot(Convert.ToDouble(datos[7])));

            diametro = Util.CmToFoot(Convert.ToDouble(datos[9]));
            espaciamiento = Util.CmToFoot(Convert.ToDouble(datos[10]));
            cuantia = datos[11];

            if (datos[8] == "Derecha")
            { this.UbicacionEnLosa = UbicacionLosa.Derecha; }
            else if (datos[8] == "Izquierda")
            { this.UbicacionEnLosa = UbicacionLosa.Izquierda; }
            else if (datos[8] == "Superior")
            { this.UbicacionEnLosa = UbicacionLosa.Superior; }
            else if (datos[8] == "Inferior")
            { this.UbicacionEnLosa = UbicacionLosa.Inferior; }

            IsValid = true;
        }
        public NH_RefereciaCrearSuple(string nombreLosa, float espesor, double anguloPelotaLosa, XYZ PosicionPelotaLosa,
                                         XYZ PosicionPto_suple, UbicacionLosa tipo, Element PelotaLosa, Element PoligonoLosa)
        {
            this.nombreLosa1 = nombreLosa;
            this.espesor = espesor;
            this.anguloPelotaLosa = anguloPelotaLosa;
            this.PosicionPelotaLosa = PosicionPelotaLosa;

            this.PosicionPto_suple = PosicionPto_suple;
            this.UbicacionEnLosa = tipo;
            this.PelotaLosa = PelotaLosa;
            this.PoligonoLosa = PoligonoLosa;
            this.diametro = diametro;
            this.espaciamiento = espaciamiento;
            this.cuantia = "@" + diametro + diametro + "a" + espaciamiento + "";
        }
        #endregion

        #region 2) metodos   n=1
        public void getRoomVecino()
        {


        }


        #endregion
    }
}
