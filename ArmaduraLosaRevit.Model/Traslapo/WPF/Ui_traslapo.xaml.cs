using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.Traslapo.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class Ui_traslapo : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;
        private UIApplication _uiApp;

        public ObservableCollection<string> TipoCaso { get; set; }







        private System.Windows.Visibility isvisibletipoDesp;
        public System.Windows.Visibility IsvisibletipoDesp
        {
            get { return isvisibletipoDesp; }
            set
            {
                if (isvisibletipoDesp != value)
                {
                    isvisibletipoDesp = value;
                    RaisePropertyChanged("IsvisibletipoDesp");
                }
            }
        }
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public DireccionEdicionPathRein direccion { get; internal set; }
        public double valorCm { get; internal set; }

        public Ui_traslapo(UIApplication uiApp,
                            EventHandlerWithStringArg evExternalMethodStringArg,
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


            this.Topmost = true;



            TipoCaso = new ObservableCollection<string>() { "Largo PathSymbol Izquierda-Inferior", "Largo PathSymbol Derecha-Superior", "Equidistante", "Normal Mouse" };

            IsvisibletipoDesp = System.Windows.Visibility.Hidden;
            // this.TipoBarra_ = TipoBarraTraslapoDereArriba.f1;
        }

        internal CalcularLargoTraslapoPAthDTO ObtenerParametrosENtrada()
        {
            CalcularLargoTraslapoPAthDTO _CalcularLargoPAthDTO = null;
            try
            {
                _CalcularLargoPAthDTO = new CalcularLargoTraslapoPAthDTO()
                {

                    pathDefinir = ObtenerTipoCaso(),
                    largopathFoot = ObtenerLargo(),
                    IsDefinirLargo = true
                    
                };
            }
            catch (Exception)
            {
                _CalcularLargoPAthDTO = new CalcularLargoTraslapoPAthDTO();
            }
            return _CalcularLargoPAthDTO;
        }

        private double ObtenerLargo()
        {
            if (dtTipo.Text == "Largo PathSymbol Izquierda-Inferior")
                return Util.CmToFoot(Util.ConvertirStringInDouble(dtLargo.Text));
            else if (dtTipo.Text == "Largo PathSymbol Derecha-Superior")
                return Util.CmToFoot(Util.ConvertirStringInDouble(dtLargo.Text));
          
            else
                return 0;
        }

        private TipoPAthDefinir ObtenerTipoCaso()
        {

            if (dtTipo.Text == "Equidistante")
                return TipoPAthDefinir.Mitad;
            else if (dtTipo.Text == "Largo PathSymbol Izquierda-Inferior")
                return TipoPAthDefinir.PathInicial;
            else if (dtTipo.Text == "Largo PathSymbol Derecha-Superior")
                return TipoPAthDefinir.PathFinal;
            else
                return TipoPAthDefinir.Normal;
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

        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            Button bton = (Button)sender;

            BotonOprimido = bton.Name;
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
        }

        #endregion

        #region Non-External Project Methods

        private void UserAlert()
        {
            //TaskDialog.Show("Non-External Method", "Non-External Method Executed Successfully");
            MessageBox.Show("Non-External Method Executed Successfully", "Non-External Method");

            //Dispatcher.Invoke(() =>
            //{
            //    TaskDialog mainDialog = new TaskDialog("Non-External Method")
            //    {
            //        MainInstruction = "Hello, Revit!",
            //        MainContent = "Non-External Method Executed Successfully",
            //        CommonButtons = TaskDialogCommonButtons.Ok,
            //        FooterText = "<a href=\"http://usa.autodesk.com/adsk/servlet/index?siteID=123112&id=2484975 \">"
            //                     + "Click here for the Revit API Developer Center</a>"
            //    };


            //    TaskDialogResult tResult = mainDialog.Show();
            //    Debug.WriteLine(tResult.ToString());
            //});
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



        private void TipoDesp_DropDownClosed(object sender, EventArgs e)
        {

        }


        private void dtTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dtTipo_DropDownClosed(object sender, EventArgs e)
        {
            if (dtTipo.Text == "Equidistante")
                dtLargo.Text = "*";

            else if (dtTipo.Text == "Largo PathSymbol Izquierda-Inferior")
                dtLargo.Text = "200";
            else if (dtTipo.Text == "Largo PathSymbol Derecha-Superior")
                dtLargo.Text = "200";
            else
                dtLargo.Text = "";

        }
    }


}