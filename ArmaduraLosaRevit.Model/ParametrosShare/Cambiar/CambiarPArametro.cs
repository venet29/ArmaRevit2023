using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.ParametrosShare.Cambiar
{
    public partial  class CambiarPArametro : Form
    {

        public string nombreparametro { get; set; }

        public string valor { get; set; }
        public bool IsOK { get; set; }
        public CambiarPArametro()
        {
            InitializeComponent();
            IsOK = false;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            IsOK = false;
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox_parametro.Text!="" && textBox_valor.Text != "")
            {
                nombreparametro = textBox_parametro.Text;
                valor = textBox_valor.Text;
                IsOK = true;
            }
            else
                IsOK = false;


            this.Close();

        }
    }
}
