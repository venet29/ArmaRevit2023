using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.CopiarEntreLosa.wpf
{


    public partial class CopiarRebarEntreLosas : Form
    {
        public bool IsOk { get;  set; }

        private List<string> resultLevelSeleccionado;
        private readonly string _viewActaul;

        public TipoCaso casoEjecutar { get; set; }
        public List<string> ListaView { get; }

        public CopiarRebarEntreLosas(List<string> listaView, string _viewActaul)
        {
            InitializeComponent();

            IsOk = false;
            ListaView = listaView;
            this._viewActaul = _viewActaul;
        }



        private void button_cerrar_Click(object sender, EventArgs e)
        {
            IsOk = false;
            resultLevelSeleccionado = new List<string>();
            this.Close();
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            IsOk = true;
            resultLevelSeleccionado = new List<string>();
            casoEjecutar = TipoCaso.Copiar;
            foreach (ListViewItem item in listView_allLevel.CheckedItems)
            {
                if (item.Checked)
                {
                    resultLevelSeleccionado.Add(item.Text);
                }
            }

            this.Close();
        }

        private void CopiarRebarWpf_Load(object sender, EventArgs e)
        {
            listView_allLevel.Columns.Add("Nombre");
            listView_allLevel.Columns[0].Width = 600;
            label_viewActual.Text = _viewActaul;


            foreach (string lv in ListaView)
            {
                ListViewItem item = new ListViewItem(lv);
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

        public List<string> ObtenerListaPlantasSeleccionadas()
        {
            var resultLevelSeleccionado2 = new List<string>();

            foreach (ListViewItem item in listView_allLevel.CheckedItems)
            {
                if (item.Checked)
                {
                    resultLevelSeleccionado2.Add(item.Text);
                }
            }

            // listView_allLevel.Items
            return resultLevelSeleccionado;
        }

        private void button_visualizar_Click(object sender, EventArgs e)
        {
            casoEjecutar = TipoCaso.Desocultar;
            IsOk = true;
            resultLevelSeleccionado = new List<string>();
            this.Close();
        }

        private void button_ocultar_Click(object sender, EventArgs e)
        {
            casoEjecutar = TipoCaso.ocultar;
            IsOk = true;
            resultLevelSeleccionado = new List<string>();
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
