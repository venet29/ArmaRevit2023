using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    public partial class FormCub : Form
    {

        public string casoTipo { get; set; }
        public FormCub()
        {
            InitializeComponent();
           // this.Icon = new Icon("Resources/revit-wire.ico");
            casoTipo = "";
        }

        private void Button_creaExcel_Click(object sender, EventArgs e)
        {
            casoTipo = "CrearExcelCubicaciones";
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            casoTipo = "CrearSchedulesCubicacionBarras";
            Close();
        }

 
    }
}
