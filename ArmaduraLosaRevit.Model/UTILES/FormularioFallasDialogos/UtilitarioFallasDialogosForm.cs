using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.UTILES.FormFallasDialogo
{
    public partial class UtilitarioFallasDialogosForm : Form
    {
        private readonly string idConError;
        public bool IsAceptar { get; private set; }
        public int TipoTrasaccion { get; private set; }

        public UtilitarioFallasDialogosForm(string descripcion, string idConError)
        {
            InitializeComponent();
            IsAceptar = false;
            TipoTrasaccion = 1;
            textBox_msaje.Text = $"Error:\r\n  Descripcion de error:{Traducir(descripcion)} \r\n\r\nId Elementos:{idConError}";
            this.idConError = idConError;
        }

        private object Traducir(string descripcion)
        {
            if (descripcion == "Rebar is placed completely outside of its host.")
                return "La barra esta completamente fuera de su host-anfitrión(Muro,losa,viga o fundacion).";
            else if (descripcion == "Can't find a host for Path Reinforcement.")
                return "La barra no encuentra   host-anfitrión(Muro,losa,viga o fundacion).";
            else
                return descripcion;
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            if (radioButton_normal.Checked)
                TipoTrasaccion = 1;
            else
                TipoTrasaccion = 2;
            IsAceptar = true;
            Close();
        }

        private void button_cerrar_Click(object sender, EventArgs e)
        {
            IsAceptar = false;
            Close();
        }
    }
}
