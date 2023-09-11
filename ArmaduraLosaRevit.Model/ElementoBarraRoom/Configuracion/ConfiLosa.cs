using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArmaduraLosaRevit.Model.Enumeraciones;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Configuracion
{
    public partial class ConfiLosa : Form
    {
 
        public bool IsAhorro { get; set; }
        public bool IsS4 { get; set; }
        public bool IsConVerificar { get; set; }
        public bool IsReSeleccionarPuntoRango { get; private set; }
        public double LargoBarras { get; set; }
        public double LargoRecorrido { get; set; }
        public string tipoPorF1 { get; set; }
        public string tipoPorF3 { get; set; }
        public string tipoPorF4 { get; set; }
        public bool IsajusteBarra_Recorrido { get;  set; }
        public bool IsajusteBarra_Largo { get;  set; }
        public bool IsOK { get; set; }
        public ConfiLosa()
        {
            InitializeComponent();
            IsOK = false;
        }

        public ConfiLosa(VariablesSistemasDTO _variablesSistemasDTO)
        {
            InitializeComponent();
            IsOK = false;

            checkBox_ahorro.Checked = _variablesSistemasDTO.IsConAhorro;
            checkBox_agregar_s4.Checked = _variablesSistemasDTO.IsDibujarS4;
            checkBox_ajusteBarra.Checked = _variablesSistemasDTO.IsAjusteBarra_Recorrido;
            checkBox_ajusteBarraLargo.Checked = _variablesSistemasDTO.IsAjusteBarra_Largo;

            checkBox_reseleccionarPuntoRango.Checked = _variablesSistemasDTO.IsReSeleccionarPuntoRango;

            checkBox_verificarEspesor.Checked = _variablesSistemasDTO.IsVerificarEspesor;
            textBox_largoBarra.Text = _variablesSistemasDTO.LargoBarras_cm.ToString();
            textBox_largoRecorrido.Text = _variablesSistemasDTO.LargoRecorrido_cm.ToString();

            comboBox_F1.Text = _variablesSistemasDTO.tipoPorF1.ToString();
            textBox_F3.Text = _variablesSistemasDTO.tipoPorF3.ToString();
            textBox_F4.Text = _variablesSistemasDTO.tipoPorF4.ToString();

            //--------------------------------------------------------------
            IsAhorro = _variablesSistemasDTO.IsConAhorro;
            IsS4 = _variablesSistemasDTO.IsDibujarS4;
            IsajusteBarra_Recorrido = _variablesSistemasDTO.IsAjusteBarra_Recorrido;
            IsReSeleccionarPuntoRango= _variablesSistemasDTO.IsReSeleccionarPuntoRango;

            IsConVerificar = _variablesSistemasDTO.IsVerificarEspesor;
            LargoBarras = _variablesSistemasDTO.LargoBarras_cm;
            LargoRecorrido = _variablesSistemasDTO.LargoRecorrido_cm;

            tipoPorF1 = _variablesSistemasDTO.tipoPorF1.ToString();
            tipoPorF3 = _variablesSistemasDTO.tipoPorF3.ToString();
            tipoPorF4 = _variablesSistemasDTO.tipoPorF4.ToString();
        }

        public ConfiLosa(bool Isahorro, bool iss4)
        {
            InitializeComponent();
            IsOK = false;

            checkBox_ahorro.Checked = Isahorro;

        }

   

        private void CheckBox_ahorro_CheckedChanged(object sender, EventArgs e)
        {
            IsAhorro = checkBox_ahorro.Checked;
        }

        private void CheckBox_agregar_s4_CheckedChanged(object sender, EventArgs e)
        {
            IsS4 = checkBox_agregar_s4.Checked;
        }

        private void Button_aceptar_Click(object sender, EventArgs e)
        {
            RecargarDatos();

            IsOK = true;
            this.Close();
        }

        private void RecargarDatos()
        {
            IsAhorro = checkBox_ahorro.Checked;
            IsS4 = checkBox_agregar_s4.Checked;
            IsajusteBarra_Recorrido = checkBox_ajusteBarra.Checked;
            IsajusteBarra_Largo = checkBox_ajusteBarraLargo.Checked;
            IsConVerificar = checkBox_verificarEspesor.Checked;
            IsReSeleccionarPuntoRango = checkBox_reseleccionarPuntoRango.Checked;

            LargoBarras = Convert.ToDouble(textBox_largoBarra.Text);


            LargoRecorrido = Convert.ToDouble(textBox_largoRecorrido.Text);

            tipoPorF1 = comboBox_F1.Text;
            tipoPorF3 = textBox_F3.Text;
            tipoPorF4 = textBox_F4.Text;
           
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckBox_verificarEspesor_CheckedChanged(object sender, EventArgs e)
        {
            IsConVerificar = checkBox_verificarEspesor.Checked;
        }
    }
}
