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
    public partial class ActualizarNameViewForm : Form
    {
 
        public string nombreView { get; set; }
        public ActualizarNameViewForm(string nombreView)
        {
            InitializeComponent();
            this.nombreView = nombreView;
            textBox_nombreVista.Text = nombreView;
        }

  

        public bool Ok { get; internal set; }

        private void button2_Click(object sender, EventArgs e)
        {
            Ok = false;
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_cambiar.Text.ToLower() == "cambiar")
            {
                Ok = true;
                nombreView = textBox_nombreVista.Text;
            }
            else
                Ok = false;

            this.Close();
        }
    }
}
