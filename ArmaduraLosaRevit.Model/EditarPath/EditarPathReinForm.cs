using ArmaduraLosaRevit.Model.Enumeraciones;
using System;

namespace ArmaduraLosaRevit.Model.EditarPath
{
    public partial class EditarPathReinForm : System.Windows.Forms.Form
    {
        public string tipoPosiicon { get; set; }
        public DireccionEdicionPathRein direccion { get; set; }
        public double valorCm { get; set; }
        public string Estado { get;  set; }

        public EditarPathReinForm()
        {
            InitializeComponent();
            
        }

        private void button_pathapath_Click(object sender, EventArgs e)
        {
            tipoPosiicon = "pathToPath";
            Estado = "ok";
            Close();
        }

        private void button_pathapto_Click(object sender, EventArgs e)
        {
            tipoPosiicon = "pathtoPto";
            Estado = "ok";
            Close();
        }

        private void button_extIzquierdo_Click(object sender, EventArgs e)
        {
            tipoPosiicon = "Extender";
            Estado = "ok";
            direccion = DireccionEdicionPathRein.Izquierda;
            valorCm = Util.ConvertirStringInDouble(textBox_extender.Text);
            Close();
        }

        private void button_extSuperior_Click(object sender, EventArgs e)
        {
            tipoPosiicon = "Extender";
            Estado = "ok";
            direccion = DireccionEdicionPathRein.Superior;
            valorCm = Util.ConvertirStringInDouble(textBox_extender.Text);
            Close();
        }

        private void button_extDerecho_Click(object sender, EventArgs e)
        {
            tipoPosiicon = "dere";
            Estado = "ok";
            direccion = DireccionEdicionPathRein.Derecha;
            valorCm = Util.ConvertirStringInDouble(textBox_extender.Text);
            Close();
        }

        private void button_extInferior_Click(object sender, EventArgs e)
        {
            tipoPosiicon = "Extender";
            Estado = "ok";
            direccion = DireccionEdicionPathRein.Inferior;
            valorCm = Util.ConvertirStringInDouble(textBox_extender.Text);
            Close();
        }
    }
}
