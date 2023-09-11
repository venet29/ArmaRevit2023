using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura.Dimensiones
{
    public partial class DimensionesBarras
    {
        public List<dimensiones> listaDimensiones = null;
        public dimensiones a { get; set; }
        public dimensiones b { get; set; }
        public dimensiones c { get; set; }
        public dimensiones c2 { get; set; }
        public dimensiones d { get; set; }
        public dimensiones e { get; set; }
        public dimensiones g { get; set; }
        public dimensiones l { get; set; }
        // numero de datos que son distinto de cero
        public int nDatos { get; set; }
        public dimensiones Largo { get; internal set; }
        public dimensiones Largo2 { get;  set; }
        public string LetrasCambiosEspesor { get; private set; }

        public double _LargoAhorro_Izq_ { get; set; }
        public double _LargoAhorro_Dere_ { get; set; }
        // public List<dimensiones> listaDimensiones = null;
        public DimensionesBarras(double a, double b, double c, double d, double e, double g, string caso, string LetrasCambiosEspesor)
        {


            listaDimensiones = new List<dimensiones>();
            if (caso == "RebarShapeNh")
            {
                this.a = new dimensiones("A", a);
                listaDimensiones.Add(this.a);
                this.b = new dimensiones("B", b);
                listaDimensiones.Add(this.b);
                this.c = new dimensiones("C", c);
                listaDimensiones.Add(this.c);
                this.d = new dimensiones("D", d);
                listaDimensiones.Add(this.d);
                this.e = new dimensiones("E", e);
                listaDimensiones.Add(this.e);
                this.g = new dimensiones("G", g);
                listaDimensiones.Add(this.g);
            }
            else if (caso == "PathReinforment")
            {
                this.a = new dimensiones("A_", a);
                listaDimensiones.Add(this.a);
                this.b = new dimensiones("B_", b);
                listaDimensiones.Add(this.b);
                this.c = new dimensiones("C_", c);
                listaDimensiones.Add(this.c);
                this.d = new dimensiones("D_", d);
                listaDimensiones.Add(this.d);
                this.e = new dimensiones("E_", e);
                listaDimensiones.Add(this.e);
                this.g = new dimensiones("G_", g);
                listaDimensiones.Add(this.g);
            }

            this.c2 = new dimensiones("C2", -1);
            listaDimensiones.Add(this.c2);
            this.Largo2 = new dimensiones("L2barra", -1);
            listaDimensiones.Add(this.Largo2);

            nDatos = 5;
            if (a == 0) nDatos = 0;
            else if (b == 0) nDatos = 1;
            else if (c == 0) nDatos = 2;
            else if (d == 0) nDatos = 3;
            else if (e == 0) nDatos = 4;
            else if (this.g.valor == 0) nDatos = 5;
            Largo = new dimensiones("LARGO_", a + b + c + d + e + g);
            this.LetrasCambiosEspesor = LetrasCambiosEspesor;



        }

        public DimensionesBarras(double a, double b, double c, double c2, double d, double e, double g,double largo2, string caso, string LetrasCambiosEspesor)
        {


            listaDimensiones = new List<dimensiones>();
            if (caso == "RebarShapeNh")
            {
                this.a = new dimensiones("A", a);
                listaDimensiones.Add(this.a);
                this.b = new dimensiones("B", b);
                listaDimensiones.Add(this.b);
                this.c = new dimensiones("C", c);
                listaDimensiones.Add(this.c);
                this.c2 = new dimensiones("C2", c2);
                listaDimensiones.Add(this.c2);
                this.d = new dimensiones("D", d);
                listaDimensiones.Add(this.d);
                this.e = new dimensiones("E", e);
                listaDimensiones.Add(this.e);
                this.g = new dimensiones("G", g);
                listaDimensiones.Add(this.g);
                this.Largo2 = new dimensiones("L2barra", largo2);
                listaDimensiones.Add(this.Largo2);
            }
            else if (caso == "PathReinforment")
            {
                this.a = new dimensiones("A_", a);
                listaDimensiones.Add(this.a);
                this.b = new dimensiones("B_", b);
                listaDimensiones.Add(this.b);
                this.c = new dimensiones("C_", c);
                listaDimensiones.Add(this.c);
                this.c2 = new dimensiones("C2", c2);
                listaDimensiones.Add(this.c2);
                this.d = new dimensiones("D_", d);
                listaDimensiones.Add(this.d);
                this.e = new dimensiones("E_", e);
                listaDimensiones.Add(this.e);
                this.g = new dimensiones("G_", g);
                listaDimensiones.Add(this.g);
                this.Largo2 = new dimensiones("L2barra", largo2);
                listaDimensiones.Add(this.Largo2);
            }

            nDatos = 5;
            if (a == 0) nDatos = 0;
            else if (b == 0) nDatos = 1;
            else if (c == 0) nDatos = 2;
            else if (d == 0) nDatos = 3;
            else if (e == 0) nDatos = 4;
            else if (this.g.valor == 0) nDatos = 5;
            Largo = new dimensiones("LARGO_", a + b + c + d + e + g);
            this.LetrasCambiosEspesor = LetrasCambiosEspesor;



        }

        public DimensionesBarras(double a, double b, double c, double d, double e, string caso, string LetrasCambiosEspesor)
        {

            listaDimensiones = new List<dimensiones>();
            if (caso == "RebarShapeNh")
            {
                this.a = new dimensiones("A", a);
                listaDimensiones.Add(this.a);
                this.b = new dimensiones("B", b);
                listaDimensiones.Add(this.b);
                this.c = new dimensiones("C", c);
                listaDimensiones.Add(this.c);
                this.d = new dimensiones("D", d);
                listaDimensiones.Add(this.d);
                this.e = new dimensiones("E", e);
                listaDimensiones.Add(this.e);
                this.g = new dimensiones("G_", 0);
                listaDimensiones.Add(this.g);
            }
            else if (caso == "PathReinforment")
            {
                this.a = new dimensiones("A_", a);
                listaDimensiones.Add(this.a);
                this.b = new dimensiones("B_", b);
                listaDimensiones.Add(this.b);
                this.c = new dimensiones("C_", c);
                listaDimensiones.Add(this.c);
                this.d = new dimensiones("D_", d);
                listaDimensiones.Add(this.d);
                this.e = new dimensiones("E_", e);
                listaDimensiones.Add(this.e);
                this.g = new dimensiones("G_", 0);
                listaDimensiones.Add(this.g);
            }

            this.c2 = new dimensiones("C2", -1);
            listaDimensiones.Add(this.c2);
            this.Largo2 = new dimensiones("L2barra", -1);
            listaDimensiones.Add(this.Largo2);


            nDatos = 5;
            if (a == 0) nDatos = 0;
            else if (b == 0) nDatos = 1;
            else if (c == 0) nDatos = 2;
            else if (d == 0) nDatos = 3;
            else if (e == 0) nDatos = 4;

            Largo = new dimensiones("LARGO_", a + b + c + d + e);
            this.LetrasCambiosEspesor = LetrasCambiosEspesor;

        }


        public void clearAll()
        {
            listaDimensiones.Clear();
            this.a = new dimensiones();
            this.b = new dimensiones();
            this.c = new dimensiones();
            this.d = new dimensiones();
            this.e = new dimensiones();
        }

    }


}


