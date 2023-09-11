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

namespace ArmaduraLosaRevit.Model.UTILES.CargarVarios
{
    public partial class FormVariosAyuda : Form
    {
        public FormVariosAyuda()
        {
            InitializeComponent();
        }

        public string tipoDeEjecucion { get; set; }
        public string nombreUSer { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "actualizarBarraTipoTodos";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizarBarraTipo1";
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "";
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "versiondll";
            this.Close();
        }
        private void Button13_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizaNombreVista_seleccionar";
            this.Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizaNombre1_vistaActual";
            this.Close();
        }

        private void button_actualizaNombreVista_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizaNombreVistaTodos";
            this.Close();
        }

        private void button_id_correlativo_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "genararId_Correlativo_1_view";
            this.Close();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            Util.InfoMsg("Comando no implementado");
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "AgregarEspesor_All_view";
            this.Close();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "AgregarEspesor_1_view";
            this.Close();
        }

        private void Button6_Click(object sender, EventArgs e)
        {

        }

        private void Button6_Click_1(object sender, EventArgs e)
        {
            tipoDeEjecucion = "agregarFIltroBarra_1view";
            this.Close();
        }

        private void Button10_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "agregarFIltroBarra_Allview";
            this.Close();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "aCONFIGURACION ELEVA";
            this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "prueba";
            this.Close();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizarNumeroBarro";
            this.Close();
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "UnobscuredInView";
            this.Close();

        }

        private void Button16_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "Cubicar";
            this.Close();
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "cambiarUsuario";
            nombreUSer = comboBox1_user.Text;
            this.Close();
        }

        private void FormVariosAyuda_Load(object sender, EventArgs e)
        {
            label_usuario.Text = $"Usuario: {VariablesSistemas.CONST_NOMBRE_3D_PARA_BUSCAR.Replace("3D_NoEditar", "User")}";
        }

        private void Button18_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "schedule";
            this.Close();
        }

        private void Button19_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "cambiarNombreVista";
            this.Close();
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "volverNombreVista";
            this.Close();
        }

        private void Button21_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizarLargoFundaciones";
            this.Close();
        }

        private void Button22_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizarNOmbreyItipoPAthReinf";
            this.Close();
        }

        private void Button23_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "SelecAyuda";
            this.Close();
        }

        private void Button24_Click(object sender, EventArgs e)
        {
            // actualiza el nombre 'nombrevista' de la barra seleccionada segun el nombre de view (elevacion ) actual
            tipoDeEjecucion = "ActualizarSeleccionar";
            this.Close();
        }

        private void Button25_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "cargartag";
            this.Close();
        }

        private void button_ocultar_Traba_Click(object sender, EventArgs e)
        {
            // se ejecuta en las vista elevaciones  y vualve el color blanco- tranasparente para las trabas
            tipoDeEjecucion = "ocultarTraba";
            this.Close();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            // se ejecuta en las vista elevaciones  y vualve el color de referencia para las trabas
            tipoDeEjecucion = "MostrarTraba";
            this.Close();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            // se ejecuta en las vista elevaciones y busca las barra tipo 'CO_T' y si tiene espaciamiento 7,5 lo cambia  a 7.5
            tipoDeEjecucion = "actualize7.5";
            this.Close();


        }

        private void button_en3D_Click(object sender, EventArgs e)
        {
            // rutina que se ejecuta en 3D y copiar el parametro 'nombrevista'  en sus rebarinsystem
            tipoDeEjecucion = "NombreVista_solopath_en3D";
            this.Close();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "DatosExcel_Volument_moldaje";
            this.Close();
        }

        private void button_Desglose_elev_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "Desglose_elev";
            this.Close();
        }

        private void button_corte_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "Desglose_corte";
            this.Close();
        }

        private void Button29_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "Ver_barra_otra_vista";
            this.Close();
        }

        private void Button31_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "BorrarTodosFiltros";
            this.Close();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "AgregarFormato";
            this.Close();
        }

        private void Button32_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "cambiarToArial";
            this.Close();
        }

        private void button_actualizarFund_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "actualizarFund";
            this.Close();

        }

        private void button_ListaFamilias_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "ListaFamilias";
            this.Close();
        }

        private void Button_WPF_Level_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "button_WPF_Level";
            this.Close();
        }

        private void Button33_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "ActualizarNbarrastodas";
            this.Close();

        }

        private void Button34_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "copiarFundaciones";
            this.Close();
        }

        private void Button35_Click(object sender, EventArgs e)
        {

            tipoDeEjecucion = "copiarcolumna";
            this.Close();
        }

        private void Button36_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "pruebaSinvista";
            this.Close();
        }

        private void Button37_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "pruebaSintipo";
            this.Close();
        }

        private void Button38_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "SelecConfiguracion";
            this.Close();
        }

        private void Button39_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "AgregarEjes";
            this.Close();
        }

        private void button40_Click(object sender, EventArgs e)
        {
            tipoDeEjecucion = "cambiarParametroTrabas";
            this.Close();
           
        }
    }
}

