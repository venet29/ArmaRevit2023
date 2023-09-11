using ArmaduraLosaRevit.Model.Visibilidad.Entidades;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ArmaduraLosaRevit.Model.Visibilidad
{
    /// <summary>
    /// Interaction logic for Visualizacion.xaml
    /// </summary>
    public partial class Visualizacion : Window
    {

        public AccionTipoBarra fx { get; set; }
        public AccionTipoBarra sx { get; set; }
        public Visualizacion()
        {
            InitializeComponent();
        }

        private void aceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            fx = (cbx_fx.Text=="Ver" ? AccionTipoBarra.Ver: (cbx_fx.Text == "Ocultar" ? AccionTipoBarra.Ocultar : AccionTipoBarra.Omitir));
            sx = (cbx_sx.Text == "Ver" ? AccionTipoBarra.Ver : (cbx_sx.Text == "Ocultar" ? AccionTipoBarra.Ocultar : AccionTipoBarra.Omitir));
        }
     

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - 300;
            //this.Top = desktopWorkingArea.Bottom - this.Height;

        }
    }
}
