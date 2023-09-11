using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.BorrarSeleccion.SharePArameter
{
    public partial class BorrarShareParameterForm : System.Windows.Forms.Form
    {
        public int idElemet { get; set; }
        public bool IsCrearLista { get; set; }
        public BorrarShareParameterForm()
        {
            InitializeComponent();
            IsCrearLista = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            idElemet = Util.ConvertirStringInInteger( textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IsCrearLista = true;
            Close();
        }
    }
}
