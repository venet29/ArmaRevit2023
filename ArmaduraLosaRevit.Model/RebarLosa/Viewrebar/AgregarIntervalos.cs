using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.RebarLosa.Viewrebar
{
    public partial class AgregarIntervalos : Form
    {
        private double largobarra_mt;
        private double anchoRecorrido_mt;
        private readonly double diametro;

        public bool Isok { get; set; }
        public List<double> ListaIntervalos { get;  set; }



        public AgregarIntervalos(double largobarra_mt, double anchoRecorrido_mt, double diametro)
        {
            this.largobarra_mt = largobarra_mt;
            this.anchoRecorrido_mt = anchoRecorrido_mt;
            this.diametro = diametro;
   
            ListaIntervalos = new List<double>();
            InitializeComponent();
        }

        private void AgregarIntervalos_Load(object sender, EventArgs e)
        {
            label_recorrido.Text = anchoRecorrido_mt.ToString();
            label_largoBarra.Text = largobarra_mt.ToString();
            label_diam.Text = diametro.ToString();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Isok = false;
            ListaIntervalos.Clear();
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (Verificar())
            {
                Isok = true;
                Close();
            }
            else
                Util.InfoMsg("Error en el formato de los intevalos. Revisar");
        }

        private bool Verificar()
        {
            try
            {
                string[] subs = textBox_listaIntervalos.Text.Split(',');

                foreach (var sub in subs)
                {
                    if (sub == "") continue;
                    if (!Util.IsNumeric(sub))
                    {
                        ListaIntervalos.Clear();
                        return false;
                    }

                    ListaIntervalos.Add(Util.ConvertirStringInDouble(sub));
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


    }
}
