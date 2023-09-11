using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.WPF;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_Elevaciones : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;


        public ObservableCollection<float> Listaespacimiamiento { get; set; }
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }
        public ObservableCollection<string> ListaTraba { get; set; }




        private bool _extenderIzq;
        public bool ExtenderIzq
        {
            get { return _extenderIzq; }
            set
            {
                if (_extenderIzq != value)
                {
                    _extenderIzq = value;
                    RaisePropertyChanged("ExtenderIzq");
                }
            }
        }

        private bool _extenderDere;
        public bool ExtenderDere
        {
            get { return _extenderDere; }
            set
            {
                if (_extenderDere != value)
                {
                    _extenderDere = value;
                    RaisePropertyChanged("ExtenderDere");
                }
            }
        }

        public string casoTipo { get; private set; }
        public string NombreTEmplete { get; set; }

        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

 

        public Ui_Elevaciones(UIApplication uiApp , string NombreTEmplete  )
        {
            _uiApp = uiApp;
            this.NombreTEmplete = NombreTEmplete;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();
            this.DataContext = this;

            NombreTemplate.Text = NombreTEmplete;
            Listaespacimiamiento = new ObservableCollection<float>() { 5, 6, 7.5f, 8, 10, 12, 14, 15, 16, 18, 20, 22, 24, 25 };
            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };
            ListaEstribo = new ObservableCollection<string>() { "E.", "E.D.", "E.T.", "E.C." };

            ListaTraba = new ObservableCollection<string>() { "A", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            ExtenderIzq = false;
            ExtenderDere = false;
            casoTipo = "";
            this.Topmost = true;
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
        }


        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }

        #region External Project Methods





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
            casoTipo = bton.Name;

            NombreTEmplete= NombreTemplate.Text;
            Close();


        }


        private void DebugUtility_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - 300;
            //this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }


}