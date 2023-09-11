using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.BarraEstriboP.WPFp
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_EstriboP : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;
   
        private UIApplication _uiApp;


        public ObservableCollection<float> Listaespacimiamiento { get; set; }
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }
        public ObservableCollection<string> ListaTraba { get; set; }
        public ObservableCollection<string> ListaTrabaOrien { get; private set; }


        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;

        public Ui_EstriboP(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg,
            EventHandlerWithWpfArg eExternalMethodWpfArg)
        {
            _uiApp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

            Listaespacimiamiento = new ObservableCollection<float>() { 5,6,7.5f,8,10,12,14,15,16,18,20,22,24,25};
            ListaDiam = new ObservableCollection<float>() {8,10,12,16,18,22,25,28,32,36};
            ListaEstribo = new ObservableCollection<string>() { "E.", "E.D.", "E.T.", "E.C." };

            ListaTraba = new ObservableCollection<string>() { "A", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            ListaTrabaOrien = new ObservableCollection<string>() { "A", "B", };

            this.Topmost = true;
           // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
        }
    

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        #region External Project Methods

        private void BExtString_Click(object sender, RoutedEventArgs e)
        {
            // Raise external event with a string argument. The string MAY
            // be pulled from a Revit API context because this is an external event
            _mExternalMethodStringArg.Raise($"Title: {_doc.Title}");
        }



        #endregion

   
        //**********************************************************************************************************
        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            Button bton = (Button)sender;

            if (bton == null)
            {
                Util.ErrorMsg("Error al ejecutar comando, boton seleccionado nulo");
                return;
            }

            BotonOprimido = bton.Name;

            if (BotonOprimido != "btnCerrar_e" || BotonOprimido != "btnCerrar_var")
            {
                // Raise external event with this UI instance (WPF) as an argument
                _mExternalMethodWpfArg.Raise(this);
            }
            else
            {
                Close();
            }
      
        }
        private void DebugUtility_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
          //  this.Left = desktopWorkingArea.Right - 300;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void Rdbton_Editar_Checked(object sender, RoutedEventArgs e)
        {
            tipo_estribo.IsEnabled = false;
            cantidad_lat.IsEnabled= false;
            cantidads_traba.IsEnabled = false;  
        }

        private void Rdbton_dibujar_Checked(object sender, RoutedEventArgs e)
        {
            tipo_estribo.IsEnabled = true;
            cantidad_lat.IsEnabled = true;
            cantidads_traba.IsEnabled = true;
        }
    }


}