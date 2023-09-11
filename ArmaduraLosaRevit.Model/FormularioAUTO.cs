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
    public partial class FormularioAUTO : Form
    {
        public  bool IsAhorro { get; set; }
        public bool IsS4 { get; set; }
        public bool IsConVerificar { get; set; }
        public double LargoBarras { get; set; }
        public double LargoRecorrido { get; set; }
        public string tipoPorF1 { get; set; }
        public string tipoPorF3 { get; set; }
        public string tipoPorF4 { get; set; }
        public bool IsSoloCopiarDatos { get; set; }
        public ConfiguracionAhorro configuracionAhorro { get; set; }
        public bool Ok { get; set; }
        public FormularioAUTO()
        {
            InitializeComponent();
        }
        public FormularioAUTO(VariablesSistemasDTO _variablesSistemasDTO)
        {
            InitializeComponent();

            checkBoxIsAhorro.Checked = _variablesSistemasDTO.IsConAhorro;
            if (_variablesSistemasDTO.IsDibujarS4)
                comboBox_tipoF1.Text = "F1_SUP"; //no se utilza
            else
                comboBox_tipoF1.Text = "F1";

            checkBox_verificarEspesor.Checked = _variablesSistemasDTO.IsVerificarEspesor;
            textBox_largoBarra.Text = _variablesSistemasDTO.LargoBarras_cm.ToString();
            textBox_largoRecorrido.Text = _variablesSistemasDTO.LargoRecorrido_cm.ToString();

            comboBox_F1.Text = _variablesSistemasDTO.tipoPorF1.ToString();
            textBox_F3.Text = _variablesSistemasDTO.tipoPorF3.ToString();
            textBox_F4.Text = _variablesSistemasDTO.tipoPorF4.ToString();

            ///----------------------------------
            IsAhorro = _variablesSistemasDTO.IsConAhorro;
            IsS4 = _variablesSistemasDTO.IsDibujarS4;

            IsConVerificar = _variablesSistemasDTO.IsVerificarEspesor;
            LargoBarras = _variablesSistemasDTO.LargoBarras_cm;
            LargoRecorrido = _variablesSistemasDTO.LargoRecorrido_cm;

            tipoPorF1 = _variablesSistemasDTO.tipoPorF1.ToString();
            tipoPorF3 = _variablesSistemasDTO.tipoPorF3.ToString();
            tipoPorF4 = _variablesSistemasDTO.tipoPorF4.ToString();

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            IsSoloCopiarDatos = checkBox_solocopiardatos.Checked;

            if (checkBoxIsAhorro.CheckState == CheckState.Checked)
            {

                    configuracionAhorro = new ConfiguracionAhorro(true, Util.CmToFoot(Util.ConvertirStringInDouble(textBox_largoRecorrido.Text)),
                                                                 Util.CmToFoot(Util.ConvertirStringInDouble(textBox_largoBarra.Text)),
                                                                 Util.CmToFoot(60),
                                                                  checkBox_verificarEspesor.Checked, comboBox_tipoF1.Text.ToLower(),
                                                                 comboBox_F1.Text.ToLower(), textBox_F3.Text.ToLower(), textBox_F4.Text.ToLower()
                                                                );
                IsAhorro = true;
                IsS4 = false;
                IsConVerificar = checkBox_verificarEspesor.Checked;
                LargoBarras = Util.ConvertirStringInDouble(textBox_largoBarra.Text);
                LargoRecorrido = Util.ConvertirStringInDouble(textBox_largoRecorrido.Text);
                tipoPorF1 = comboBox_F1.Text;
                tipoPorF3 = textBox_F3.Text;
                tipoPorF4 = textBox_F4.Text;

            }
            else
            {
                configuracionAhorro = new ConfiguracionAhorro(false,1000000,
                                                              10000000, 115,
                                                              checkBox_verificarEspesor.Checked, comboBox_tipoF1.Text.ToLower(),
                                                               comboBox_F1.Text, textBox_F3.Text, textBox_F4.Text
                                                             );
                IsAhorro = false;
            }

            Ok = true;
            Close();
        }


        private void button_cerrar_Click(object sender, EventArgs e)
        {
            Ok = false;
            Close();

        }

        private void checkBoxIsAhorro_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIsAhorro.CheckState == CheckState.Checked)
            {
                cambiarEstado(true);
            }
            else
            { cambiarEstado(false); }
        }

        private void cambiarEstado(bool estado)
        {
            textBox_largoRecorrido.Enabled = estado;
            textBox_largoBarra.Enabled = estado;
            comboBox_F1.Enabled = estado;
            textBox_F3.Enabled = estado;
            textBox_F4.Enabled = estado;
        }

        private void textBox_largoBarra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox_largoRecorrido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void ComboBox_tipoF1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_tipoF1.Text == "F1")
            {
                checkBox_verificarEspesor.CheckState = CheckState.Unchecked;
                checkBox_verificarEspesor.Enabled = false;
            }
            else
            { checkBox_verificarEspesor.Enabled = true; }
        }
    }
}
