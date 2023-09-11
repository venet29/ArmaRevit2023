using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.Visibilidad.ActualizarNombreVista
{
    public partial class DiferenciaNombrePara : Form
    {
        private readonly string nombreView;
        private readonly string nombrePara;

        public DiferenciaNombrePara(string nombreView , string nombrePara)
        {
            InitializeComponent();
            this.nombreView = nombreView;
            this.nombrePara = nombrePara;
            textBox_nombreVista.Text= nombreView;
            textBox_ParaView.Text = nombrePara;
            resultado = false;
            resultadoString = "";
        }

        public bool resultado { get; private set; }
        public string resultadoString { get; private set; }

        private void Button1_Click(object sender, EventArgs e)
        {
            resultado = true;
            resultadoString = textBox_ParaView.Text;
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
