using ArmaduraLosaRevit.Model.AnalisisRoom;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.Cambiar.CuantiasRoom
{
    public partial class CambiarCuantia : System.Windows.Forms.Form
    {
        private readonly UIApplication _Uiapp;
        private Element roomElem;

        public string Estado { get; private set; }
        public int diamH { get;  set; }
        public int diamV { get;  set; }
        public double EspaH { get;  set; }
        public double EspaV { get;  set; }

        public CambiarCuantia()
        {

            InitializeComponent();
        }

        public CambiarCuantia(UIApplication _uiapp ,Element roomElem)
        {
            InitializeComponent();
            _Uiapp = _uiapp;
            this.roomElem = roomElem;
        }

       public void ObtenerCuantias()
        {

            ReferenciaRoom newRegistroLosa = new ReferenciaRoom(_Uiapp.ActiveUIDocument.Document, (Room)roomElem);
            Tuple<int, int> tupleCuantiaH = UtilBarras.ObtenerDiametroEspesorEnMMyCM(newRegistroLosa.RefereciaRoomDatos.cuantiaHorizontal);
            Tuple<int, int> tupleCuantiaV = UtilBarras.ObtenerDiametroEspesorEnMMyCM(newRegistroLosa.RefereciaRoomDatos.cuantiaVertical);

            
            diamH = tupleCuantiaH.Item1; //dia
            EspaH = tupleCuantiaH.Item2;// espa
            diamV = tupleCuantiaV.Item1;
            EspaV = tupleCuantiaV.Item2;
            comboBox_diamH.Text = diamH.ToString();
            comboBox_diamV.Text = diamV.ToString();
            textBox_espaH.Text = EspaH.ToString();
            textBox_espaV.Text = EspaV.ToString();
        }


        private void button_cerrar_Click(object sender, EventArgs e)
        {
            Estado = "Salir";
            this.Close();
        }

        private void button_Aceptar_Click(object sender, EventArgs e)
        {
            diamH = Util.ConvertirStringInInteger(comboBox_diamH.Text);
            diamV = Util.ConvertirStringInInteger(comboBox_diamV.Text);
            EspaH = Util.ConvertirStringInInteger(textBox_espaH.Text);
            EspaV = Util.ConvertirStringInInteger(textBox_espaV.Text);
            Estado = "Ok";
            this.Close();
        }
    }
}
