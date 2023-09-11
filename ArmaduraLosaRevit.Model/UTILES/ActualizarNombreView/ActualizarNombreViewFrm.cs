using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.UTILES.ActualizarNombreView
{
    public partial class ActualizarNombreViewFrm : Form
    {
        public bool Isok { get;  set; }
        public string ActualizarNombreView { get; private set; }

        public ActualizarNombreViewFrm(string nombreview)
        {
            ActualizarNombreView = nombreview;
            InitializeComponent();
        }

  

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
            Isok = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ActualizarNombreView = textBox_nombreView.Text;
            Close();
            Isok = true;
        }

        private void ActualizarNombreViewFrm_Load(object sender, EventArgs e)
        {
            textBox_nombreView.Text = ActualizarNombreView;
        }
    }
}
