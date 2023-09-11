using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ahorro;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model
{
    public partial class FormularioAUTOSUP : Form
    {


        public ConfiguracionAhorro configuracionAhorro { get; set; }
        public bool Ok { get; set; }
        public FormularioAUTOSUP()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {

            configuracionAhorro = new ConfiguracionAhorro(comboBox_SX.Text.ToLower());
            Ok = true;
            Close();
        }


        private void button_cerrar_Click(object sender, EventArgs e)
        {
            Ok = false;
            Close();

        }



 
       

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

    }
}
