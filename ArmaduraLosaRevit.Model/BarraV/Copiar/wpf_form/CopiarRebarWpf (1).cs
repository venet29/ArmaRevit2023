using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArmaduraLosaRevit.Model.Seleccionar;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar.wpf
{


    public partial class CopiarRebarWpf : Form
    {
        public bool IsOk { get;  set; }
     //   Dictionary<string, double> ListaLevel;
        private List<double> resultLevelSeleccionado;
        private List<EnvoltoriLevel> ListaLevel;

        public TipoCaso casoEjecutar { get; set; }
        public CopiarRebarWpf(List<EnvoltoriLevel> listaLevel)
        {
            InitializeComponent();
            //this.ListaLevel = ListaLevel;
            this.ListaLevel = listaLevel;
            IsOk = false;
        }

  

        private void button_cerrar_Click(object sender, EventArgs e)
        {
            IsOk = false;
            resultLevelSeleccionado = new List<double>();
            this.Close();
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            IsOk = true;
            resultLevelSeleccionado = new List<double>();
            casoEjecutar = TipoCaso.Copiar;
            foreach (ListViewItem item in listView_allLevel.CheckedItems)
            {
                if (item.Checked)
                {
                    resultLevelSeleccionado.Add(ListaLevel.Where(c => c.NombreLevel == item.Text).Select(c => c.ElevacionRedondeada).FirstOrDefault());
                }
            }

            this.Close();
        }

        private void CopiarRebarWpf_Load(object sender, EventArgs e)
        {
            listView_allLevel.Columns.Add("Nombre");
            listView_allLevel.Columns[0].Width = 145;
            listView_allLevel.Columns.Add("Elev");
            listView_allLevel.Columns[1].Width = 145;

            foreach (EnvoltoriLevel lv in ListaLevel)
            {
                ListViewItem item = new ListViewItem(lv.NombreLevel);
                item.SubItems.Add(Math.Round(Util.FootToCm(lv.ElevacionRedondeada), 2).ToString());

                item.Checked = false;

                listView_allLevel.Items.Add(item);
            }
            listView_allLevel.ItemCheck += new ItemCheckEventHandler(GridListView_ItemCheck);
        }

        private void GridListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool visible = e.NewValue == CheckState.Checked ? true : false;

            string nombre = listView_allLevel.Items[e.Index].Text;
        }

        public List<double> ObtenerListaLevelZ()
        {
            //var resultLevelSeleccionado2 = new List<double>();

            //foreach (ListViewItem item in listView_allLevel.CheckedItems)
            //{
            //    if (item.Checked)
            //    {
            //        resultLevelSeleccionado2.Add(ListaLevel.Where(c=> c.NombreLevel== item.Text).Select(c=>c.ElevacionRedondeada).FirstOrDefault());
            //    }
            //}

            // listView_allLevel.Items
            return resultLevelSeleccionado;
        }

        private void button_visualizar_Click(object sender, EventArgs e)
        {
            casoEjecutar = TipoCaso.Desocultar;
            IsOk = true;
            resultLevelSeleccionado = new List<double>();
            this.Close();
        }

        private void button_ocultar_Click(object sender, EventArgs e)
        {
            casoEjecutar = TipoCaso.ocultar;
            IsOk = true;
            resultLevelSeleccionado = new List<double>();
            this.Close();
        }
    }

    public  enum TipoCaso
    {
        Copiar,
        Desocultar ,
        ocultar
    }
}
