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
using System.IO;

namespace ArmaduraLosaRevit.Model.Elemento_Losa
{
    public class PtosCrearSuples
    {

        #region 0)propiedades
        private bool IsValid { get; set; }

        public XYZ pto1 { get; set; }  // posicion desde donde se  comeinza a dibujar el traslapo --  seria el punto interno de la losas en que se dibuja , tb sirve para conocer que lado de la losa esta empotrado
        public XYZ pto2 { get; set; }

        public string nombre1 { get; set; }
        public string nombre2 { get; set; }

        public NH_RefereciaCrearSuple Plosa2 { get; set; }

        private NH_RefereciaCrearSuple plosa1;
        public NH_RefereciaCrearSuple Plosa1
        {
            get { return plosa1; }
            set
            {
                plosa1 = value;
                this.tipo = plosa1.UbicacionEnLosa;
            }
        }
        public UbicacionLosa tipo { get; set; }

        public int diametro { get; set; }
        public int espaciamiento { get; set; }
        public string cuantia { get; set; } 
        #endregion

        #region 1) Constructor
        public PtosCrearSuples()
        {
            IsValid = true;
        }

        public PtosCrearSuples(XYZ ptoRefe, XYZ ppr, string nombre1, string nombre2, int diametro, int espaciamiento)
        {
            // TODO: Complete member initialization
            pto1 = ptoRefe;
            pto2 = ppr;
            this.nombre1 = nombre1;
            this.nombre2 = nombre2;
            this.diametro = diametro;
            this.espaciamiento = espaciamiento;
            this.cuantia = "@"+ diametro+diametro+"a"+ espaciamiento+"";

            IsValid = true;
        }

        public PtosCrearSuples(string item)
        {
            // TODO: Complete member initialization

            var datos = item.Split(new char[] { ',', ':' });

            this.nombre1 = datos[0];
            this.nombre2 = datos[1];

            this.pto1 = new XYZ(Convert.ToDouble(datos[2]), Convert.ToDouble(datos[3]), Convert.ToDouble(datos[4]));
            this.pto2 = new XYZ(Convert.ToDouble(datos[5]), Convert.ToDouble(datos[6]), Convert.ToDouble(datos[7]));

            diametro = Convert.ToInt32(datos[9]);
            espaciamiento = Convert.ToInt32(datos[10]);
            cuantia = datos[11];

            if (datos[8] == "Derecha") 
            { this.tipo = UbicacionLosa.Derecha; }
            else if (datos[8] == "Izquierda")
            { this.tipo = UbicacionLosa.Izquierda; }
            else if (datos[8] == "Superior")
            { this.tipo = UbicacionLosa.Superior; }
            else if (datos[8] == "Inferior")
            { this.tipo = UbicacionLosa.Inferior; }

            IsValid = true;
        }
    
        #endregion
    }
}
