using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.AnalisisRoom
{
    public class AnotationGeneralPelotaLosa
    {


        // 0)propiedades 

        public string Espesor { get; set; }
        public string Numero { get; set; }
        public Element PelotaLosa { get; set; }
        public XYZ PointUbicacion { get; set; }
        public bool Isborrar { get; set; } = false;
        public double Angulo { get;  set; }
        public List<XYZ> ListaInterDireLosa { get; set; }
        public List<XYZ> ListaInterPerpDireLosa { get; set; }
        public float largoMin { get; set; }

        public bool IsEspesorVariable { get; set; }
        //1)contructor 
        public AnotationGeneralPelotaLosa()
        {



        }

        public AnotationGeneralPelotaLosa(string Espesor, string Numero, Element PelotaLosa, XYZ PointUbicacion, double Angulo, List<XYZ> ListaInterDireLosa, List<XYZ> ListaInterPerpDireLosa, float largoMin,bool IsEspesorVariable=false)
        {
            this.Espesor = Espesor;
            this.Numero = Numero;
            this.PelotaLosa = PelotaLosa;
            this.PointUbicacion = PointUbicacion;
            this.Angulo = Angulo;
            this.ListaInterDireLosa = ListaInterDireLosa;
            this.ListaInterPerpDireLosa = ListaInterPerpDireLosa;
            this.largoMin = largoMin;
            this.IsEspesorVariable = IsEspesorVariable;
        }


       //2)metodos


 

    }
}