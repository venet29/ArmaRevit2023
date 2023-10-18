using System;
using System.IO;
using System.Windows.Forms;
using ApiRevit.FILTROS;
using ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad;

using Autodesk.Revit.UI;
using OfficeOpenXml;

namespace ArmaduraLosaRevit.Model.BarraV
{
    public partial class CargadorBarraV : Form
    {
        private ExternalCommandData commandData;

        public CargadorBarraV()
        {
            InitializeComponent();
            LoadNh();
        }

        public CargadorBarraV(ExternalCommandData commandData)
        {
            this.commandData = commandData;
            InitializeComponent();
            LoadNh();
        }

        private void LoadNh()
        {
            IVisibilidadView PathRein_ = VisibilidadView.Creador_Visibilidad_SinInterfase(commandData.View, Autodesk.Revit.DB.BuiltInCategory.OST_PathRein, "Structural Path Reinforcement");
            if (PathRein_ == null) return;
            Autodesk.Revit.UI.Selection.ISelectionFilter filtroMuro = SelFilter.GetElementFilter(typeof(Autodesk.Revit.DB.Wall), typeof(Autodesk.Revit.DB.FamilyInstance));

            FileInfo excelFile = new FileInfo($"C:\\");
            ExcelPackage excel = new ExcelPackage(excelFile);
            var rerer = excel.File;
        }

        public string tipodeCaso { get; private set; }

        public string tipoCasoUpdate { get; private set; }

        private void Button_CargarReactor_Click(object sender, EventArgs e)
        {
            tipodeCaso = "CargarUpdateREbar";
            tipoCasoUpdate = comboBox_casoUpate.Text;
            this.Close();
        }

        private void Button_DescargarReactor_Click(object sender, EventArgs e)
        {
            tipodeCaso = "DesCargarUpdateREbar";
            tipoCasoUpdate = comboBox_casoUpate.Text;
            this.Close();
        }

        private void Button_rebarInclinadoEspecial_Click(object sender, EventArgs e)
        {
            tipodeCaso = "REbarInclinadas ";

            this.Close();
        }

        private void Button_rebarInclinadoEspecial_Click_1(object sender, EventArgs e)
        {
            tipodeCaso = "rebarInclinadoEspecial";

            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            tipodeCaso = "wpfFundaciones";

            this.Close();
        }

        private void Button_supleSX_Click(object sender, EventArgs e)
        {
            tipodeCaso = "WPFSuple";
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            tipodeCaso = "WPFRefuerzoLosa";
            this.Close();
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void Button7_Click(object sender, EventArgs e)
        {
            tipodeCaso = "caso_barras_Auto";
            this.Close();
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            tipodeCaso = "caso_barras";
            this.Close();
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            tipodeCaso = "caso_barras_manual";
            this.Close();
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            tipodeCaso = "caso_malla";
            this.Close();

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            tipodeCaso = "caso_econf";
            this.Close();

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            tipodeCaso = "caso_epilar";
            this.Close();
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            tipodeCaso = "caso_eviga";
            this.Close();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            tipodeCaso = "crear_pelota_losa";
            this.Close();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            tipodeCaso = "crear_room_losa";
            this.Close();
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            tipodeCaso = "editarPathRein";
            this.Close();

        }

        private void Button12_Click(object sender, EventArgs e)
        {
            tipodeCaso = "borrarLosaRebar";
            this.Close();
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            tipodeCaso = "Elevbarra";
            this.Close();
        }

        private void Button18_Click(object sender, EventArgs e)
        {
            tipodeCaso = "ElevTodos";
            this.Close();
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            tipodeCaso = "Elevmalla";
            this.Close();
        }

        private void Button19_Click(object sender, EventArgs e)
        {
            tipodeCaso = "agruparbarra";
            this.Close();
        }

        private void Button20_Click(object sender, EventArgs e)
        {
            tipodeCaso = "crearTextoGrid";
            this.Close();
        }

        private void Button21_Click(object sender, EventArgs e)
        {
            tipodeCaso = "PelotaLosaEstru";
            this.Close();

        }

        private void button21_Click_1(object sender, EventArgs e)
        {

            tipodeCaso = "ExtStore";
            this.Close();
        }

        private void Button22_Click(object sender, EventArgs e)
        {
            tipodeCaso = "traslapar";
            this.Close();
        }
        private void button31_Click(object sender, EventArgs e)
        {
            tipodeCaso = "traslaparFund";
            this.Close();
        }
        private void Button23_Click(object sender, EventArgs e)
        {
            tipodeCaso = "_AssemblyInstanceNH";
            this.Close();
        }

        private void Button24_Click(object sender, EventArgs e)
        {
            tipodeCaso = "pinCargar";
            this.Close();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            tipodeCaso = "copiarEntrelosa";
            this.Close();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            tipodeCaso = "cub";
            this.Close();
        }

        private void Button28_Click(object sender, EventArgs e)
        {
            tipodeCaso = "Formayuda";
            this.Close();
        }

        private void Button29_Click(object sender, EventArgs e)
        {
            tipodeCaso = "CArgar_CopiarElev";
            this.Close();

        }

        private void Button30_Click(object sender, EventArgs e)
        {
            tipodeCaso = "cambio_espesor";
            this.Close();
        }

        private void Button32_Click(object sender, EventArgs e)
        {
            tipodeCaso = "suple_muro";
            this.Close();
        }

        private void Button33_Click(object sender, EventArgs e)
        {
            tipodeCaso = "editFund";
            this.Close();
        }

        private void Button_schedule_Click(object sender, EventArgs e)
        {
            tipodeCaso = "schedule";
            this.Close();

        }

        private void Button35_Click(object sender, EventArgs e)
        {
            tipodeCaso = "datosschedule";
            this.Close();
        }

        private void Button_Horq_Click(object sender, EventArgs e)
        {
            tipodeCaso = "casoHorquilla";
            this.Close();
        }

        private void Button_BlancoNegro_Click(object sender, EventArgs e)
        {
            tipodeCaso = "BlancoNegro";
            this.Close();
        }

        private void Button_InvertirBlancoNegro_Click(object sender, EventArgs e)
        {

            tipodeCaso = "InvertirBlancoNegro";
            this.Close();
        }

        private void Button25_Click(object sender, EventArgs e)
        {
            tipodeCaso = "prueba";
            this.Close();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            tipodeCaso = "borrarPArametros";
            this.Close();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            tipodeCaso = "borrarfamilias";
            this.Close();
        }

        private void button37_Click(object sender, EventArgs e)
        {
            tipodeCaso = "cargarFamiliasYotros";
            this.Close();
        }

        private void button38_Click(object sender, EventArgs e)
        {
            tipodeCaso = "cargarParametros";
            this.Close();
        }

        private void CargadorBarraV_Load(object sender, EventArgs e)
        {

        }

        private void Button39_Click(object sender, EventArgs e)
        {
            tipodeCaso = "dibujarPathsymbol";
            this.Close();
        }

        private void Button40_Click(object sender, EventArgs e)
        {
            tipodeCaso = "editarBarra";
            this.Close();

        }

        private void Button_losaexporta_Click(object sender, EventArgs e)
        {
            tipodeCaso = "button_losaexporta";
            this.Close();

        }

        private void Button_ConfigLosa_Click(object sender, EventArgs e)
        {

            tipodeCaso = "ConfigLosa";
            this.Close();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            tipodeCaso = "ListaPara";
            this.Close();
        }

        private void Button42_Click(object sender, EventArgs e)
        {
            tipodeCaso = "Filtro3D";
            this.Close();
        }

        private void Button_pasada_Click(object sender, EventArgs e)
        {
            tipodeCaso = "pasadas";
            this.Close();
        }

        private void Button_copiaLocal_Click(object sender, EventArgs e)
        {
            tipodeCaso = "button_copiaLocal";
            this.Close();

        }

        private void Button_crearRoomLosa_Click(object sender, EventArgs e)
        {

            tipodeCaso = "button_crearRoomLosa";
            this.Close();

        }

        private void Button43_Click(object sender, EventArgs e)
        {
            tipodeCaso = "Actualizar_familias";
            this.Close();

        }

        private void Button44_Click(object sender, EventArgs e)
        {
            tipodeCaso = "actualizaNombre1_vistaActual";
            this.Close();

        }

        private void Button_ConfElevaciones_Click(object sender, EventArgs e)
        {
            tipodeCaso = "confiiguracion_Elevaciones";
            this.Close();
        }

        private void Button_VigaAuto_Click(object sender, EventArgs e)
        {
            tipodeCaso = "VigaAuto_";
            this.Close();

        }

        private void Button45_Click(object sender, EventArgs e)
        {
            tipodeCaso = "LosaImportInf";
            this.Close();
        }

        private void Button46_Click(object sender, EventArgs e)
        {

            tipodeCaso = "LosaImportSup";
            this.Close();
        }

        private void Button47_Click(object sender, EventArgs e)
        {

            tipodeCaso = "estadoProyecto";
            this.Close();
        }

        private void button48_Click(object sender, EventArgs e)
        {

            tipodeCaso = "AcotarShaft";
            this.Close();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button49_Click(object sender, EventArgs e)
        {
            tipodeCaso = "agruparbarraH";
            this.Close();

        }

        private void button_cambiarGrid_Click(object sender, EventArgs e)
        {
            tipodeCaso = "button_cambiarGrid";
            this.Close();

       
        }

        private void button50_Click(object sender, EventArgs e)
        {
            tipodeCaso = "refuerzoFUndaciones";
            this.Close();
        }

        private void button51_Click(object sender, EventArgs e)
        {
            tipodeCaso = "CrearSheet";
            this.Close();
        }
    }
}
